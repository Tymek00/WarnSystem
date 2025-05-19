using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using WarnSystem.Models;
using WarnSystem.Handlers;
using WarnSystem.Handlers.WarnSystem;

namespace WarnSystem.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class WarnCommands : ICommand
    {
        public string Command => WarnPlugin.Instance?.Config?.CommandPrefix ?? "warn";

        public string[] Aliases => WarnPlugin.Instance?.Config?.CommandAliases ?? new[] { "ws", "warns" };

        public string Description => Localization.GetTranslation("CommandDescription", WarnPlugin.Instance.Config.Language);

        public bool Execute(ArraySegment<string> arguments, CommandSystem.ICommandSender sender, out string response)
        {
            string language = WarnPlugin.Instance.Config.Language;
            string requiredPermission = WarnPlugin.Instance.Config.RequiredPermission;
            if (!sender.CheckPermission(requiredPermission))
            {
                response = Localization.GetTranslation("InsufficientPermissions", language, requiredPermission);
                return false;
            }

            if (WarnPlugin.Instance.DataHandler == null)
            {
                response = Localization.GetTranslation("SystemNotInitialized", language);
                Log.Error("WarnCommands Execute: DataHandler is null!");
                return false;
            }

            if (arguments.Count == 0)
            {
                response = GetHelpMessage(language);
                return true;
            }

            string subCommand = arguments.At(0).ToLower();
            var subArguments = arguments.Skip(1).ToArray();

            switch (subCommand)
            {
                case "check":
                case "checkwarn":
                    return HandleCheckWarn(subArguments, sender, language, out response);
                case "add":
                case "warn":
                    return HandleWarn(subArguments, sender, language, out response);
                case "note":
                    return HandleNote(subArguments, sender, language, out response);
                case "delete":
                case "del":
                case "delwarn":
                case "deletewarn":
                    return HandleDeleteWarn(subArguments, sender, language, out response);
                case "help":
                default:
                    response = GetHelpMessage(language);
                    return true;
            }
        }

        private string GetHelpMessage(string language)
        {
            return Localization.GetTranslation("NoArgumentsHelp", language, Command);
        }

        private bool TryGetPlayerData(string identifier, string language, out string userId, out string nickname, out string errorResponse)
        {
            Player targetPlayer = Player.Get(identifier);
            if (targetPlayer != null)
            {
                userId = targetPlayer.UserId;
                nickname = targetPlayer.Nickname;
                errorResponse = null;
                return true;
            }
            else if (identifier.Contains("@"))
            {
                userId = identifier;
                nickname = $"Offline ({identifier.Split('@')[0]})";
                errorResponse = null;
                return true;
            }
            else
            {
                userId = null;
                nickname = null;
                errorResponse = Localization.GetTranslation("PlayerNotFound", language);
                return false;
            }
        }

        private bool HandleCheckWarn(string[] args, CommandSystem.ICommandSender sender, string language, out string response)
        {
            if (args.Length != 1)
            {
                response = Localization.GetTranslation("CheckWarnUsage", language, Command);
                return false;
            }

            if (!TryGetPlayerData(args[0], language, out string targetUserId, out string targetNickname, out string errorMsg))
            {
                response = errorMsg;
                return false;
            }

            List<WarnEntry> entries = WarnPlugin.Instance.DataHandler.GetEntriesForPlayer(targetUserId);

            if (entries.Count == 0)
            {
                response = Localization.GetTranslation("NoWarnsOrNotes", language, targetNickname, targetUserId);
                return true;
            }

            StringBuilder sb = new StringBuilder(Localization.GetTranslation("WarnsNotesHeader", language, targetNickname, targetUserId));
            foreach (var entry in entries.OrderByDescending(e => e.Timestamp))
            {
                sb.AppendLine(entry.ToString());
            }

            response = sb.ToString();
            return true;
        }

        private bool HandleWarn(string[] args, CommandSystem.ICommandSender sender, string language, out string response)
        {
            if (args.Length < 2)
            {
                response = Localization.GetTranslation("WarnUsage", language, Command);
                return false;
            }

            string targetIdentifier = args[0];
            string reason = string.Join(" ", args.Skip(1));

            if (!TryGetPlayerData(targetIdentifier, language, out string targetUserId, out string targetNickname, out string errorMsg))
            {
                response = errorMsg;
                return false;
            }

            Player admin = Player.Get(sender);
            WarnEntry newWarn = WarnPlugin.Instance.DataHandler.AddEntry("Warn", targetUserId, targetNickname, admin, reason);

            response = Localization.GetTranslation("WarnAdded", language, newWarn.Id, targetNickname, targetUserId, reason);
            return true;
        }

        private bool HandleNote(string[] args, CommandSystem.ICommandSender sender, string language, out string response)
        {
            if (args.Length < 2)
            {
                response = Localization.GetTranslation("NoteUsage", language, Command);
                return false;
            }

            string targetIdentifier = args[0];
            string noteContent = string.Join(" ", args.Skip(1));

            if (!TryGetPlayerData(targetIdentifier, language, out string targetUserId, out string targetNickname, out string errorMsg))
            {
                response = errorMsg;
                return false;
            }

            Player admin = Player.Get(sender);
            WarnEntry newNote = WarnPlugin.Instance.DataHandler.AddEntry("Note", targetUserId, targetNickname, admin, noteContent);

            response = Localization.GetTranslation("NoteAdded", language, newNote.Id, targetNickname, targetUserId);
            return true;
        }

        private bool HandleDeleteWarn(string[] args, CommandSystem.ICommandSender sender, string language, out string response)
        {
            if (args.Length != 1)
            {
                response = Localization.GetTranslation("DeleteUsage", language, Command);
                return false;
            }

            if (!int.TryParse(args[0], out int entryId))
            {
                response = Localization.GetTranslation("InvalidId", language, args[0]);
                return false;
            }

            if (WarnPlugin.Instance.DataHandler.DeleteEntry(entryId, out WarnEntry deletedEntry))
            {
                response = Localization.GetTranslation("EntryDeleted", language, deletedEntry.Type, entryId, deletedEntry.PlayerNickname, deletedEntry.PlayerUserId);
                return true;
            }
            else
            {
                response = Localization.GetTranslation("EntryNotFound", language, entryId);
                return false;
            }
        }
    }

    public class CommandConfig
    {
        public string CommandPrefix { get; set; } = "warn";
        public string[] CommandAliases { get; set; } = { "ws", "warns" };
    }
}