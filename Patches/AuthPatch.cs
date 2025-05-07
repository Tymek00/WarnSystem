using MEC;
using PlayerRoles.FirstPersonControl;
using PlayerRoles;
using PlayerStatsSystem;
using RemoteAdmin.Communication;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using VoiceChat;
using NorthwoodLib.Pools;
using Exiled.API.Features;
using NorthwoodLib;
using HarmonyLib;
using WarnSystem.Models;
using WarnSystem.Handlers.WarnSystem;

/// <summary>
/// Contains patches for the RaPlayer class to customize Remote Admin data handling.
/// </summary>
namespace WarnSystem.Patches
{
    /// <summary>
    /// Patches the RaPlayer.ReceiveData method to handle custom player data processing.
    /// </summary>
    [HarmonyPatch(typeof(RaPlayer), nameof(RaPlayer.ReceiveData), new Type[] { typeof(CommandSender), typeof(string) })]
    public class AuthPatch
    {
        // Stores active coroutine handles for each sender to prevent concurrent executions
        public static Dictionary<string, CoroutineHandle> Handles = new Dictionary<string, CoroutineHandle>();

        /// <summary>
        /// Prefix method to intercept RaPlayer.ReceiveData calls and initiate coroutine processing.
        /// </summary>
        /// <param name="__instance">The RaPlayer instance being patched.</param>
        /// <param name="sender">The command sender requesting data.</param>
        /// <param name="data">The data string containing command and parameters.</param>
        /// <returns>False to prevent the original method from executing.</returns>
        public static bool Prefix(RaPlayer __instance, CommandSender sender, string data)
        {
            // Check if a coroutine is already running for this sender
            if (Handles.ContainsKey(sender.SenderId) && Handles[sender.SenderId].IsRunning)
                return false;

            // Start a new coroutine and store its handle
            Handles[sender.SenderId] = Timing.RunCoroutine(RaPlayerCoRoutine(__instance, sender, data));
            return false; // Prevent original method execution
        }

        /// <summary>
        /// Coroutine to process player data requests asynchronously.
        /// </summary>
        /// <param name="__instance">The RaPlayer instance being patched.</param>
        /// <param name="sender">The command sender requesting data.</param>
        /// <param name="data">The data string containing command and parameters.</param>
        /// <returns>An enumerator for coroutine execution.</returns>
        public static IEnumerator<float> RaPlayerCoRoutine(RaPlayer __instance, CommandSender sender, string data)
        {
            try
            {
                string language = WarnPlugin.Instance.Config.Language;

                // Split the data string; expect two elements (command and parameter)
                string[] spData = data.Split(' ');
                if (spData.Length != 2)
                    yield break; // Exit if data format is invalid

                // Parse the command type (1 for short, other for full)
                if (!int.TryParse(spData[0], out int commandType))
                    yield break; // Exit if command type is not a number

                bool isShort = commandType == 1; // Determine if short data is requested
                var playerSender = sender as PlayerCommandSender;

                Log.Debug($"Received data: {data} : {spData[1]}");

                // Check permissions for full data access
                if (!isShort && playerSender != null &&
                    !playerSender.ReferenceHub.authManager.RemoteAdminGlobalAccess &&
                    !playerSender.ReferenceHub.authManager.BypassBansFlagSet &&
                    !CommandProcessor.CheckPermissions(sender, PlayerPermissions.PlayerSensitiveDataAccess))
                {
                    yield break; // Exit if sender lacks permissions
                }

                // Process the list of players based on the provided parameters
                ArraySegment<string> playersData = new ArraySegment<string>(spData.Skip(1).ToArray());
                var players = RAUtils.ProcessPlayerIdOrNamesList(playersData, 0, out _);
                if (players.Count == 0)
                    yield break; // Exit if no players found

                // Handle multiple players case
                if (players.Count > 1)
                {
                    var infoMultiple = StringBuilderPool.Shared.Rent();
                    infoMultiple.AppendFormat("${0} ", __instance.DataId);
                    infoMultiple.Append("<color=white>Multiple players selected.</color>");
                    sender.RaReply(StringBuilderPool.Shared.ToStringReturn(infoMultiple), true, true, string.Empty);
                    yield break; // Exit after sending multiple players message
                }

                // Verify sensitive data access permissions
                if (!CommandProcessor.CheckPermissions(sender, PlayerPermissions.PlayerSensitiveDataAccess))
                {
                    sender.RaReply("<color=red>Insufficient permissions to view information</color>", false, true, string.Empty);
                    yield break; // Exit if permissions are insufficient
                }

                // Determine UserId access permissions
                bool userIdPerms;
                if (playerSender != null && playerSender.ReferenceHub.authManager.NorthwoodStaff)
                    userIdPerms = true;
                else
                    userIdPerms = PermissionsHandler.IsPermitted(sender.Permissions, ServerRoles.UserIdPerms);

                var player = players[0]; // Get the single selected player

                // Check gameplay data access permissions
                var gameplayData = PermissionsHandler.IsPermitted(sender.Permissions, PlayerPermissions.GameplayData);

                // Cache player components for efficiency
                var ccm = player.characterClassManager;
                var pam = player.authManager;
                var nms = player.nicknameSync;
                var con = player.networkIdentity.connectionToClient;
                var sr = player.serverRoles;

                // Grant gameplay data access to sender if applicable
                if (sender is PlayerCommandSender commandSender)
                    commandSender.ReferenceHub.queryProcessor.GameplayData = gameplayData;

                // Build the response string
                var info = StringBuilderPool.Shared.Rent();
                info.AppendFormat("${0} ", __instance.DataId);
                info.AppendFormat("<color=white>Nickname: {0}", nms.CombinedName);
                info.AppendFormat("\nPlayer ID: {0} <color=green><link=CP_ID>\uf0c5</link></color>", player.PlayerId);
                RaClipboard.Send(sender, RaClipboard.RaClipBoardType.PlayerId, $"{player.PlayerId}");

                // Handle IP address information
                if (con == null)
                {
                    RaClipboard.Send(sender, RaClipboard.RaClipBoardType.Ip, string.Empty);
                    info.Append("\nIP Address: null");
                }
                else if (!isShort)
                {
                    // Log IP access for auditing
                    ServerLogs.AddLog(ServerLogs.Modules.DataAccess,
                        $"{sender.LogName} accessed IP address of player {player.PlayerId} ({player.nicknameSync.MyNick}).",
                        ServerLogs.ServerLogType.RemoteAdminActivity_Misc);

                    string address = con.address;

                    info.AppendFormat("\nIP Address: {0} ", address);
                    RaClipboard.Send(sender, RaClipboard.RaClipBoardType.Ip, address);

                    if (con.IpOverride != null)
                        info.AppendFormat(" [routed via {0}]", con.OriginalIpAddress);

                    info.Append(" <color=green><link=CP_IP>\uf0c5</link></color>");
                }
                else
                {
                    RaClipboard.Send(sender, RaClipboard.RaClipBoardType.Ip, string.Empty);
                    info.Append("\nIP Address: [REDACTED]");
                }

                // Handle UserId information
                info.Append("\nUser ID: ");
                if (userIdPerms)
                {
                    if (string.IsNullOrEmpty(pam.UserId))
                        info.Append("(none)");
                    else
                        info.AppendFormat("{0} <color=green><link=CP_USERID>\uf0c5</link></color>", pam.UserId);

                    RaClipboard.Send(sender, RaClipboard.RaClipBoardType.UserId, pam.UserId ?? string.Empty);
                    if (pam.SaltedUserId != null && pam.SaltedUserId.Contains("$", StringComparison.Ordinal))
                        info.AppendFormat("\nSalted User ID: {0}", pam.SaltedUserId);
                }
                else
                {
                    info.Append("<color=#D4AF37>INSUFFICIENT PERMISSIONS</color>");
                    RaClipboard.Send(sender, RaClipboard.RaClipBoardType.UserId, string.Empty);
                }

                // Display server role
                info.Append("\nServer role: ");
                info.Append(sr.GetColoredRoleString());

                // Check permissions for hidden badge visibility
                bool vhb = CommandProcessor.CheckPermissions(sender, PlayerPermissions.ViewHiddenBadges);
                bool ghb = CommandProcessor.CheckPermissions(sender, PlayerPermissions.ViewHiddenGlobalBadges);

                if (playerSender != null)
                {
                    vhb = true;
                    ghb = true;
                }

                bool hidden = !string.IsNullOrEmpty(sr.HiddenBadge);
                bool show = !hidden || sr.GlobalHidden && ghb || !sr.GlobalHidden && vhb;

                // Display hidden role information if permitted
                if (show)
                {
                    if (hidden)
                    {
                        info.AppendFormat("\n<color=#DC143C>Hidden role: </color>{0}", sr.HiddenBadge);
                        info.AppendFormat("\n<color=#DC143C>Hidden role type: </color>{0}",
                            (sr.GlobalHidden ? "GLOBAL" : "LOCAL"));
                    }

                    if (player.authManager.RemoteAdminGlobalAccess)
                        info.Append("\nStudio Status: <color=#BCC6CC>Studio GLOBAL Staff (management or global moderation)</color>");
                    else if (player.authManager.NorthwoodStaff)
                        info.Append("\nStudio Status: <color=#94B9CF>Studio Staff</color>");
                }

                // Display mute status
                VcMuteFlags muteFlags = VoiceChatMutes.GetFlags(players[0]);
                if (muteFlags != 0)
                {
                    info.Append("\nMUTE STATUS:");
                    foreach (VcMuteFlags flag in EnumUtils<VcMuteFlags>.Values)
                    {
                        if (flag == 0 || (muteFlags & flag) != flag)
                            continue;

                        info.Append(" <color=#F70D1A>");
                        info.Append(flag);
                        info.Append("</color>");
                    }
                }

                // Display active flags
                info.Append("\nActive flag(s):");
                if (ccm.GodMode)
                    info.Append(" <color=#659EC7>[GOD MODE]</color>");

                if (player.playerStats.GetModule<AdminFlagsStat>().HasFlag(AdminFlags.Noclip))
                    info.Append(" <color=#DC143C>[NOCLIP ENABLED]</color>");
                else if (FpcNoclip.IsPermitted(player))
                    info.Append(" <color=#E52B50>[NOCLIP UNLOCKED]</color>");

                if (pam.DoNotTrack)
                    info.Append(" <color=#BFFF00>[DO NOT TRACK]</color>");

                if (sr.BypassMode)
                    info.Append(" <color=#BFFF00>[BYPASS MODE]</color>");

                if (show && sr.RemoteAdmin)
                    info.Append(" <color=#43C6DB>[RA AUTHENTICATED]</color>");

                if (sr.IsInOverwatch)
                    info.Append(" <color=#008080>[OVERWATCH MODE]</color>");
                else if (gameplayData)
                {
                    // Display gameplay-related data if permitted
                    info.Append("\nClass: ")
                        .Append(PlayerRoleLoader.AllRoles.TryGetValue(player.GetRoleId(), out PlayerRoleBase cl)
                            ? cl.RoleTypeId
                            : "None");
                    info.Append(" <color=#fcff99>[HP: ").Append(CommandProcessor.GetRoundedStat<HealthStat>(player))
                        .Append("]</color>");
                    info.Append(" <color=green>[AHP: ").Append(CommandProcessor.GetRoundedStat<AhpStat>(player))
                        .Append("]</color>");
                    info.Append(" <color=#977dff>[HS: ").Append(CommandProcessor.GetRoundedStat<HumeShieldStat>(player))
                        .Append("]</color>");
                    info.Append("\nPosition: ").Append(player.transform.position.ToPreciseString());
                }
                else
                    info.Append("\n<color=#D4AF37>Some fields were hidden. GameplayData permission required.</color>");

                info.Append("\n");

                // Retrieve and display comments
                try
                {
                    List<WarnEntry> comments = WarnPlugin.Instance.DataHandler.GetEntriesForPlayer(pam.UserId).Where(w => w.Type == "Note").ToList();
                    if (comments.Any())
                    {
                        info.Append("\n<color=#b200ff>" + Localization.GetTranslation("Comments", language) + "</color>:");
                        string cc = comments.Last().AdminNickname;
                        string lol = comments.Last().AdminUserId;
                        string ss = comments.Last().Reason;

                        info.Append("\n" + Localization.GetTranslation("CommentFormat", language, cc, lol, ss));
                    }
                    else
                    {
                        info.Append("\n<color=#b200ff>" + Localization.GetTranslation("Comments", language) + "</color>: <color=#ff00d4>" + Localization.GetTranslation("CommentsNone", language) + "</color>");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error retrieving comments: {ex}");
                    info.Append("\n<color=#b200ff>" + Localization.GetTranslation("Comments", language) + "</color>: <color=#ff0000>" + Localization.GetTranslation("CommentsError", language) + "</color>");
                }

                // Retrieve and display warns
                try
                {
                    List<WarnEntry> warns = WarnPlugin.Instance.DataHandler.GetEntriesForPlayer(pam.UserId).Where(w => w.Type == "Warn").ToList();
                    if (warns.Any())
                    {
                        info.Append($"\n<color=#ff0082>" + Localization.GetTranslation("Warns", language) + "</color>:\n");
                        info.AppendLine("--------------------");
                        foreach (var entry in warns.OrderByDescending(e => e.Timestamp))
                        {
                            info.AppendLine($"Admin Nickname: {entry.AdminNickname} ({entry.AdminUserId})\nReason/Content: {entry.Reason}\nDate: {entry.Timestamp} UTC\n");
                        }
                    }
                    else
                    {
                        info.Append("\n<color=#ff0082>" + Localization.GetTranslation("Warns", language) + "</color>: <color=#00ff22>" + Localization.GetTranslation("WarnsNone", language) + "</color>");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error retrieving warns: {ex}");
                    info.Append("\n<color=#ff0082>" + Localization.GetTranslation("Warns", language) + "</color>: <color=#ff0000>" + Localization.GetTranslation("WarnsError", language) + "</color>");
                }

                info.Append("</color>");
                // Send the constructed response to the sender
                sender.RaReply(StringBuilderPool.Shared.ToStringReturn(info), true, true, string.Empty);
                RaPlayerQR.Send(sender, false, string.IsNullOrEmpty(pam.UserId) ? string.Empty : pam.UserId);

                yield break; // End coroutine
            }
            catch (Exception ex)
            {
                Log.Error(ex); // Log any unexpected errors
            }
        }
    }
}