using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using WarnSystem.Models;
using WarnSystem.Handlers;

namespace WarnSystem.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class WarnCommands : CommandSystem.ICommand 
    {
        public string Command => WarnPlugin.Instance?.Config?.CommandPrefix ?? "warn"; 

        public string[] Aliases => WarnPlugin.Instance?.Config?.CommandAliases ?? new[] { "ws", "warns" }; 

        public string Description => "Zarządza systemem ostrzeżeń i notatek graczy.";

        public bool Execute(ArraySegment<string> arguments, CommandSystem.ICommandSender sender, out string response) 
        {
            string requiredPermission = WarnPlugin.Instance.Config.RequiredPermission;
            if (!sender.CheckPermission(requiredPermission))
            {
                response = $"Brak uprawnień! Wymagana permisja: {requiredPermission}";
                return false;
            }

            if (WarnPlugin.Instance.DataHandler == null)
            {
                response = "Błąd: System ostrzeżeń nie został poprawnie zainicjowany. Skontaktuj się z administratorem serwera.";
                Log.Error("WarnCommands Execute: DataHandler is null!");
                return false;
            }


            if (arguments.Count == 0)
            {
                response = GetHelpMessage();
                return true;
            }

            string subCommand = arguments.At(0).ToLower();
            var subArguments = arguments.Skip(1).ToArray();

            switch (subCommand)
            {
                case "check":
                case "checkwarn":
                    return HandleCheckWarn(subArguments, sender, out response);
                case "add":
                case "warn":
                    return HandleWarn(subArguments, sender, out response);
                case "note":
                    return HandleNote(subArguments, sender, out response);
                case "delete":
                case "del":
                case "delwarn":
                case "deletewarn":
                    return HandleDeleteWarn(subArguments, sender, out response);
                case "help":
                default:
                    response = GetHelpMessage();
                    return true;
            }
        }
        private string GetHelpMessage()
        {
            string cmd = Command;
            return $"System Warnów/Notatek - Dostępne komendy:\n" +
                   $".{cmd} check <ID Gracza/@UserId> - Wyświetla warny i notatki gracza.\n" +
                   $".{cmd} add <ID Gracza/@UserId> <Powód> - Daje warna graczowi.\n" +
                   $".{cmd} note <ID Gracza/@UserId> <Treść notatki> - Dodaje notatkę o graczu.\n" +
                   $".{cmd} delete <ID Warna/Notatki> - Usuwa wpis o podanym ID.";
        }

        private bool TryGetPlayerData(string identifier, out string userId, out string nickname, out string errorResponse)
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
                errorResponse = "Nie znaleziono gracza online o podanym ID. Aby wykonać akcję dla gracza offline, podaj jego pełne UserId (np. 123456789@steam lub 12345@discord).";
                return false;
            }
        }


        private bool HandleCheckWarn(string[] args, CommandSystem.ICommandSender sender, out string response)
        {
            if (args.Length != 1)
            {
                response = $"Poprawne użycie: .{Command} check <ID Gracza/@UserId>";
                return false;
            }

            if (!TryGetPlayerData(args[0], out string targetUserId, out string targetNickname, out string errorMsg))
            {
                response = errorMsg;
                return false;
            }

            List<WarnEntry> entries = WarnPlugin.Instance.DataHandler.GetEntriesForPlayer(targetUserId);

            if (entries.Count == 0)
            {
                response = $"Nie znaleziono żadnych warnów ani notatek dla gracza {targetNickname} (UserId: {targetUserId}).";
                return true;
            }

            StringBuilder sb = new StringBuilder($"Warny/Notatki dla gracza {targetNickname} (UserId: {targetUserId}):\n");
            sb.AppendLine("--------------------");
            foreach (var entry in entries.OrderByDescending(e => e.Timestamp))
            {
                sb.AppendLine(entry.ToString());
            }

            response = sb.ToString();
            return true;
        }

        private bool HandleWarn(string[] args, CommandSystem.ICommandSender sender, out string response)
        {
            if (args.Length < 2)
            {
                response = $"Poprawne użycie: .{Command} add <ID Gracza/@UserId> <Powód>";
                return false;
            }

            string targetIdentifier = args[0];
            string reason = string.Join(" ", args.Skip(1));

            if (!TryGetPlayerData(targetIdentifier, out string targetUserId, out string targetNickname, out string errorMsg))
            {
                response = errorMsg;
                return false;
            }

            Player admin = Player.Get(sender);
            WarnEntry newWarn = WarnPlugin.Instance.DataHandler.AddEntry("Warn", targetUserId, targetNickname, admin, reason);

            response = $"Dodano warna (ID: {newWarn.Id}) dla gracza {targetNickname} (UserId: {targetUserId}). Powód: {reason}";
            return true;
        }

        private bool HandleNote(string[] args, CommandSystem.ICommandSender sender, out string response)
        {
            if (args.Length < 2)
            {
                response = $"Poprawne użycie: .{Command} note <ID Gracza/@UserId> <Treść notatki>";
                return false;
            }

            string targetIdentifier = args[0];
            string noteContent = string.Join(" ", args.Skip(1));

            if (!TryGetPlayerData(targetIdentifier, out string targetUserId, out string targetNickname, out string errorMsg))
            {
                response = errorMsg;
                return false;
            }

            Player admin = Player.Get(sender);
            WarnEntry newNote = WarnPlugin.Instance.DataHandler.AddEntry("Note", targetUserId, targetNickname, admin, noteContent);

            response = $"Dodano notatkę (ID: {newNote.Id}) dla gracza {targetNickname} (UserId: {targetUserId}).";
            return true;
        }

        private bool HandleDeleteWarn(string[] args, CommandSystem.ICommandSender sender, out string response)
        {
            if (args.Length != 1)
            {
                response = $"Poprawne użycie: .{Command} delete <ID Warna/Notatki>";
                return false;
            }

            if (!int.TryParse(args[0], out int entryId))
            {
                response = $"Niepoprawne ID: '{args[0]}'. ID musi być liczbą całkowitą.";
                return false;
            }

            if (WarnPlugin.Instance.DataHandler.DeleteEntry(entryId, out WarnEntry deletedEntry))
            {
                response = $"Usunięto wpis typu '{deletedEntry.Type}' o ID {entryId} (dotyczący gracza {deletedEntry.PlayerNickname} - {deletedEntry.PlayerUserId}).";
                return true;
            }
            else
            {
                response = $"Nie znaleziono wpisu o ID {entryId} lub wystąpił błąd podczas usuwania.";
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