using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarnSystem.Handlers
{
    using System;
    using System.Collections.Generic;

    namespace WarnSystem
    {
        public static class Localization
        {
            private static readonly Dictionary<string, Dictionary<string, string>> Translations = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "en", new Dictionary<string, string>
                {
                    // WarnCommands.cs
                    {"InsufficientPermissions", "Insufficient permissions! Required permission: {0}"},
                    {"SystemNotInitialized", "Error: The warning system was not properly initialized. Contact the server administrator."},
                    {"NoArgumentsHelp", "Warn/Note System - Available commands:\n" +
                                       ".{0} check <Player ID/@UserId> - Displays player's warns and notes.\n" +
                                       ".{0} add <Player ID/@UserId> <Reason> - Issues a warn to the player.\n" +
                                       ".{0} note <Player ID/@UserId> <Note Content> - Adds a note about the player.\n" +
                                       ".{0} delete <Warn/Note ID> - Deletes the entry with the specified ID."},
                    {"CheckWarnUsage", "Correct usage: .{0} check <Player ID/@UserId>"},
                    {"PlayerNotFound", "No online player found with the provided ID. To perform an action for an offline player, provide their full UserId (e.g., 123456789@steam or 12345@discord)."},
                    {"NoWarnsOrNotes", "No warns or notes found for player {0} (UserId: {1})."},
                    {"WarnsNotesHeader", "Warns/Notes for player {0} (UserId: {1}):\n--------------------"},
                    {"WarnUsage", "Correct usage: .{0} add <Player ID/@UserId> <Reason>"},
                    {"WarnAdded", "Added warn (ID: {0}) for player {1} (UserId: {2}). Reason: {3}"},
                    {"NoteUsage", "Correct usage: .{0} note <Player ID/@UserId> <Note Content>"},
                    {"NoteAdded", "Added note (ID: {0}) for player {1} (UserId: {2})."},
                    {"DeleteUsage", "Correct usage: .{0} delete <Warn/Note ID>"},
                    {"InvalidId", "Invalid ID: '{0}'. ID must be an integer."},
                    {"EntryDeleted", "Deleted entry of type '{0}' with ID {1} (concerning player {2} - {3})."},
                    {"EntryNotFound", "No entry with ID {0} found or an error occurred during deletion."},

                    // AuthPatch.cs (only specified keys)
                    {"Comments", "Comments"},
                    {"CommentsNone", "none"},
                    {"CommentsError", "unable to retrieve data"},
                    {"CommentFormat", "- {0}({1}) left a comment: {2}"},
                    {"Warns", "Warns"},
                    {"WarnsNone", "none"},
                    {"WarnsError", "unable to retrieve data"},
                }
            },
            {
                "ru", new Dictionary<string, string>
                {
                    // WarnCommands.cs
                    {"InsufficientPermissions", "Недостаточно прав! Требуемое разрешение: {0}"},
                    {"SystemNotInitialized", "Ошибка: Система предупреждений не была корректно инициализирована. Свяжитесь с администратором сервера."},
                    {"NoArgumentsHelp", "Система предупреждений/заметок - Доступные команды:\n" +
                                       ".{0} check <ID игрока/@UserId> - Показывает предупреждения и заметки игрока.\n" +
                                       ".{0} add <ID игрока/@UserId> <Причина> - Выдает предупреждение игроку.\n" +
                                       ".{0} note <ID игрока/@UserId> <Текст заметки> - Добавляет заметку об игроке.\n" +
                                       ".{0} delete <ID предупреждения/заметки> - Удаляет запись с указанным ID."},
                    {"CheckWarnUsage", "Правильное использование: .{0} check <ID игрока/@UserId>"},
                    {"PlayerNotFound", "Игрок с указанным ID не найден онлайн. Для действий с оффлайн игроком укажите полный UserId (например, 123456789@steam или 12345@discord)."},
                    {"NoWarnsOrNotes", "Предупреждения или заметки для игрока {0} (UserId: {1}) не найдены."},
                    {"WarnsNotesHeader", "Предупреждения/заметки для игрока {0} (UserId: {1}):\n--------------------"},
                    {"WarnUsage", "Правильное использование: .{0} add <ID игрока/@UserId> <Причина>"},
                    {"WarnAdded", "Добавлено предупреждение (ID: {0}) для игрока {1} (UserId: {2}). Причина: {3}"},
                    {"NoteUsage", "Правильное использование: .{0} note <ID игрока/@UserId> <Текст заметки>"},
                    {"NoteAdded", "Добавлена заметка (ID: {0}) для игрока {1} (UserId: {2})."},
                    {"DeleteUsage", "Правильное использование: .{0} delete <ID предупреждения/заметки>"},
                    {"InvalidId", "Неверный ID: '{0}'. ID должен быть целым числом."},
                    {"EntryDeleted", "Удалена запись типа '{0}' с ID {1} (касается игрока {2} - {3})."},
                    {"EntryNotFound", "Запись с ID {0} не найдена или произошла ошибка при удалении."},

                    // AuthPatch.cs (only specified keys)
                    {"Comments", "Комментарии"},
                    {"CommentsNone", "отсутствуют"},
                    {"CommentsError", "не удалось получить данные"},
                    {"CommentFormat", "- {0}({1}) оставил комментарий: {2}"},
                    {"Warns", "Предупреждения"},
                    {"WarnsNone", "отсутствуют"},
                    {"WarnsError", "не удалось получить данные"},
                }
            },
            {
                "de", new Dictionary<string, string>
                {
                    // WarnCommands.cs
                    {"InsufficientPermissions", "Unzureichende Berechtigungen! Erforderliche Berechtigung: {0}"},
                    {"SystemNotInitialized", "Fehler: Das Warnsystem wurde nicht korrekt initialisiert. Kontaktieren Sie den Serveradministrator."},
                    {"NoArgumentsHelp", "Warn-/Notizsystem - Verfügbare Befehle:\n" +
                                       ".{0} check <Spieler-ID/@UserId> - Zeigt die Warnungen und Notizen des Spielers an.\n" +
                                       ".{0} add <Spieler-ID/@UserId> <Grund> - Gibt dem Spieler eine Warnung.\n" +
                                       ".{0} note <Spieler-ID/@UserId> <Notizinhalt> - Fügt eine Notiz über den Spieler hinzu.\n" +
                                       ".{0} delete <Warn-/Notiz-ID> - Löscht den Eintrag mit der angegebenen ID."},
                    {"CheckWarnUsage", "Korrekte Verwendung: .{0} check <Spieler-ID/@UserId>"},
                    {"PlayerNotFound", "Kein Online-Spieler mit der angegebenen ID gefunden. Geben Sie für Offline-Spieler die vollständige UserId an (z.B. 123456789@steam oder 12345@discord)."},
                    {"NoWarnsOrNotes", "Keine Warnungen oder Notizen für Spieler {0} (UserId: {1}) gefunden."},
                    {"WarnsNotesHeader", "Warnungen/Notizen für Spieler {0} (UserId: {1}):\n--------------------"},
                    {"WarnUsage", "Korrekte Verwendung: .{0} add <Spieler-ID/@UserId> <Grund>"},
                    {"WarnAdded", "Warnung hinzugefügt (ID: {0}) für Spieler {1} (UserId: {2}). Grund: {3}"},
                    {"NoteUsage", "Korrekte Verwendung: .{0} note <Spieler-ID/@UserId> <Notizinhalt>"},
                    {"NoteAdded", "Notiz hinzugefügt (ID: {0}) für Spieler {1} (UserId: {2})."},
                    {"DeleteUsage", "Korrekte Verwendung: .{0} delete <Warn-/Notiz-ID>"},
                    {"InvalidId", "Ungültige ID: '{0}'. Die ID muss eine Ganzzahl sein."},
                    {"EntryDeleted", "Eintrag vom Typ '{0}' mit ID {1} gelöscht (betrifft Spieler {2} - {3})."},
                    {"EntryNotFound", "Kein Eintrag mit ID {0} gefunden oder ein Fehler beim Löschen aufgetreten."},

                    // AuthPatch.cs (only specified keys)
                    {"Comments", "Kommentare"},
                    {"CommentsNone", "keine"},
                    {"CommentsError", "Daten konnten nicht abgerufen werden"},
                    {"CommentFormat", "- {0}({1}) hat einen Kommentar hinterlassen: {2}"},
                    {"Warns", "Warnungen"},
                    {"WarnsNone", "keine"},
                    {"WarnsError", "Daten konnten nicht abgerufen werden"},
                }
            },
            {
                "fr", new Dictionary<string, string>
                {
                    // WarnCommands.cs
                    {"InsufficientPermissions", "Permissions insuffisantes ! Permission requise : {0}"},
                    {"SystemNotInitialized", "Erreur : Le système d'avertissements n'a pas été correctement initialisé. Contactez l'administrateur du serveur."},
                    {"NoArgumentsHelp", "Système d'avertissements/notes - Commandes disponibles :\n" +
                                       ".{0} check <ID Joueur/@UserId> - Affiche les avertissements et notes du joueur.\n" +
                                       ".{0} add <ID Joueur/@UserId> <Raison> - Donne un avertissement au joueur.\n" +
                                       ".{0} note <ID Joueur/@UserId> <Contenu de la note> - Ajoute une note sur le joueur.\n" +
                                       ".{0} delete <ID Avertissement/Note> - Supprime l'entrée avec l'ID spécifié."},
                    {"CheckWarnUsage", "Utilisation correcte : .{0} check <ID Joueur/@UserId>"},
                    {"PlayerNotFound", "Aucun joueur en ligne trouvé avec l'ID fourni. Pour effectuer une action sur un joueur hors ligne, fournissez son UserId complet (par ex. 123456789@steam ou 12345@discord)."},
                    {"NoWarnsOrNotes", "Aucun avertissement ou note trouvé pour le joueur {0} (UserId : {1})."},
                    {"WarnsNotesHeader", "Avertissements/Notes pour le joueur {0} (UserId : {1}) :\n--------------------"},
                    {"WarnUsage", "Utilisation correcte : .{0} add <ID Joueur/@UserId> <Raison>"},
                    {"WarnAdded", "Avertissement ajouté (ID : {0}) pour le joueur {1} (UserId : {2}). Raison : {3}"},
                    {"NoteUsage", "Utilisation correcte : .{0} note <ID Joueur/@UserId> <Contenu de la note>"},
                    {"NoteAdded", "Note ajoutée (ID : {0}) pour le joueur {1} (UserId : {2})."},
                    {"DeleteUsage", "Utilisation correcte : .{0} delete <ID Avertissement/Note>"},
                    {"InvalidId", "ID invalide : '{0}'. L'ID doit être un entier."},
                    {"EntryDeleted", "Entrée de type '{0}' avec l'ID {1} supprimée (concerne le joueur {2} - {3})."},
                    {"EntryNotFound", "Aucune entrée avec l'ID {0} trouvée ou une erreur s'est produite lors de la suppression."},

                    // AuthPatch.cs (only specified keys)
                    {"Comments", "Commentaires"},
                    {"CommentsNone", "aucun"},
                    {"CommentsError", "impossible de récupérer les données"},
                    {"CommentFormat", "- {0}({1}) a laissé un commentaire : {2}"},
                    {"Warns", "Avertissements"},
                    {"WarnsNone", "aucun"},
                    {"WarnsError", "impossible de récupérer les données"},
                }
            },
            {
                "da", new Dictionary<string, string>
                {
                    // WarnCommands.cs
                    {"InsufficientPermissions", "Utilstrækkelige tilladelser! Påkrævet tilladelse: {0}"},
                    {"SystemNotInitialized", "Fejl: Advarselssystemet blev ikke korrekt initialiseret. Kontakt serveradministratoren."},
                    {"NoArgumentsHelp", "Advarsels-/notatsystem - Tilgængelige kommandoer:\n" +
                                       ".{0} check <Spiller-ID/@UserId> - Viser spillerens advarsler og notater.\n" +
                                       ".{0} add <Spiller-ID/@UserId> <Årsag> - Udsteder en advarsel til spilleren.\n" +
                                       ".{0} note <Spiller-ID/@UserId> <Notatindhold> - Tilføjer et notat om spilleren.\n" +
                                       ".{0} delete <Advarsel-/Notat-ID> - Sletter posten med den angivne ID."},
                    {"CheckWarnUsage", "Korrekt brug: .{0} check <Spiller-ID/@UserId>"},
                    {"PlayerNotFound", "Ingen online spiller fundet med det angivne ID. For at udføre en handling for en offline spiller, angiv deres fulde UserId (f.eks. 123456789@steam eller 12345@discord)."},
                    {"NoWarnsOrNotes", "Ingen advarsler eller notater fundet for spilleren {0} (UserId: {1})."},
                    {"WarnsNotesHeader", "Advarsler/Notater for spilleren {0} (UserId: {1}):\n--------------------"},
                    {"WarnUsage", "Korrekt brug: .{0} add <Spiller-ID/@UserId> <Årsag>"},
                    {"WarnAdded", "Advarsel tilføjet (ID: {0}) for spilleren {1} (UserId: {2}). Årsag: {3}"},
                    {"NoteUsage", "Korrekt brug: .{0} note <Spiller-ID/@UserId> <Notatindhold>"},
                    {"NoteAdded", "Notat tilføjet (ID: {0}) for spilleren {1} (UserId: {2})."},
                    {"DeleteUsage", "Korrekt brug: .{0} delete <Advarsel-/Notat-ID>"},
                    {"InvalidId", "Ugyldigt ID: '{0}'. ID skal være et heltal."},
                    {"EntryDeleted", "Post af typen '{0}' med ID {1} slettet (vedrører spilleren {2} - {3})."},
                    {"EntryNotFound", "Ingen post med ID {0} fundet, eller der opstod en fejl under sletning."},

                    // AuthPatch.cs (only specified keys)
                    {"Comments", "Kommentarer"},
                    {"CommentsNone", "ingen"},
                    {"CommentsError", "kunne ikke hente data"},
                    {"CommentFormat", "- {0}({1}) efterlod en kommentar: {2}"},
                    {"Warns", "Advarsler"},
                    {"WarnsNone", "ingen"},
                    {"WarnsError", "kunne ikke hente data"},
                }
            },
            {
                "pl", new Dictionary<string, string>
                {
                    // WarnCommands.cs
                    {"InsufficientPermissions", "Niewystarczające uprawnienia! Wymagane uprawnienie: {0}"},
                    {"SystemNotInitialized", "Błąd: System ostrzeżeń nie został poprawnie zainicjowany. Skontaktuj się z administratorem serwera."},
                    {"NoArgumentsHelp", "System ostrzeżeń/notatek - Dostępne komendy:\n" +
                                       ".{0} check <ID Gracza/@UserId> - Wyświetla ostrzeżenia i notatki gracza.\n" +
                                       ".{0} add <ID Gracza/@UserId> <Powód> - Wystawia ostrzeżenie graczowi.\n" +
                                       ".{0} note <ID Gracza/@UserId> <Treść notatki> - Dodaje notatkę o graczu.\n" +
                                       ".{0} delete <ID Ostrzeżenia/Notatki> - Usuwa wpis o podanym ID."},
                    {"CheckWarnUsage", "Poprawne użycie: .{0} check <ID Gracza/@UserId>"},
                    {"PlayerNotFound", "Nie znaleziono gracza online o podanym ID. Aby wykonać akcję dla gracza offline, podaj jego pełne UserId (np. 123456789@steam lub 12345@discord)."},
                    {"NoWarnsOrNotes", "Nie znaleziono żadnych ostrzeżeń ani notatek dla gracza {0} (UserId: {1})."},
                    {"WarnsNotesHeader", "Ostrzeżenia/Notatki dla gracza {0} (UserId: {1}):\n--------------------"},
                    {"WarnUsage", "Poprawne użycie: .{0} add <ID Gracza/@UserId> <Powód>"},
                    {"WarnAdded", "Dodano ostrzeżenie (ID: {0}) dla gracza {1} (UserId: {2}). Powód: {3}"},
                    {"NoteUsage", "Poprawne użycie: .{0} note <ID Gracza/@UserId> <Treść notatki>"},
                    {"NoteAdded", "Dodano notatkę (ID: {0}) dla gracza {1} (UserId: {2})."},
                    {"DeleteUsage", "Poprawne użycie: .{0} delete <ID Ostrzeżenia/Notatki>"},
                    {"InvalidId", "Niepoprawny ID: '{0}'. ID musi być liczbą całkowitą."},
                    {"EntryDeleted", "Usunięto wpis typu '{0}' o ID {1} (dotyczący gracza {2} - {3})."},
                    {"EntryNotFound", "Nie znaleziono wpisu o ID {0} lub wystąpił błąd podczas usuwania."},

                    // AuthPatch.cs (only specified keys)
                    {"Comments", "Komentarze"},
                    {"CommentsNone", "brak"},
                    {"CommentsError", "nie udało się pobrać danych"},
                    {"CommentFormat", "- {0}({1}) zostawił komentarz: {2}"},
                    {"Warns", "Ostrzeżenia"},
                    {"WarnsNone", "brak"},
                    {"WarnsError", "nie udało się pobrać danych"},
                }
            },
            {
                "uk", new Dictionary<string, string>
                {
                    // WarnCommands.cs
                    {"InsufficientPermissions", "Недостатньо прав! Потрібний дозвіл: {0}"},
                    {"SystemNotInitialized", "Помилка: Система попереджень не була належним чином ініціалізована. Зверніться до адміністратора сервера."},
                    {"NoArgumentsHelp", "Система попереджень/нотаток - Доступні команди:\n" +
                                       ".{0} check <ID Гравця/@UserId> - Показує попередження та нотатки гравця.\n" +
                                       ".{0} add <ID Гравця/@UserId> <Причина> - Видає попередження гравцеві.\n" +
                                       ".{0} note <ID Гравця/@UserId> <Вміст нотатки> - Додає нотатку про гравця.\n" +
                                       ".{0} delete <ID Попередження/Нотатки> - Видаляє запис із зазначеним ID."},
                    {"CheckWarnUsage", "Правильне використання: .{0} check <ID Гравця/@UserId>"},
                    {"PlayerNotFound", "Гравця з вказаним ID не знайдено онлайн. Для дій з офлайн-гравцем вкажіть його повний UserId (наприклад, 123456789@steam або 12345@discord)."},
                    {"NoWarnsOrNotes", "Попередження чи нотатки для гравця {0} (UserId: {1}) не знайдені."},
                    {"WarnsNotesHeader", "Попередження/Нотатки для гравця {0} (UserId: {1}):\n--------------------"},
                    {"WarnUsage", "Правильне використання: .{0} add <ID Гравця/@UserId> <Причина>"},
                    {"WarnAdded", "Додано попередження (ID: {0}) для гравця {1} (UserId: {2}). Причина: {3}"},
                    {"NoteUsage", "Правильне використання: .{0} note <ID Гравця/@UserId> <Вміст нотатки>"},
                    {"NoteAdded", "Додано нотатку (ID: {0}) для гравця {1} (UserId: {2})."},
                    {"DeleteUsage", "Правильне використання: .{0} delete <ID Попередження/Нотатки>"},
                    {"InvalidId", "Невірний ID: '{0}'. ID має бути цілим числом."},
                    {"EntryDeleted", "Видалено запис типу '{0}' з ID {1} (стосується гравця {2} - {3})."},
                    {"EntryNotFound", "Запис з ID {0} не знайдено або сталася помилка під час видалення."},

                    // AuthPatch.cs (only specified keys)
                    {"Comments", "Коментарі"},
                    {"CommentsNone", "відсутні"},
                    {"CommentsError", "не вдалося отримати дані"},
                    {"CommentFormat", "- {0}({1}) залишив коментар: {2}"},
                    {"Warns", "Попередження"},
                    {"WarnsNone", "відсутні"},
                    {"WarnsError", "не вдалося отримати дані"},
                }
            },
            {
                "it", new Dictionary<string, string>
                {
                    // WarnCommands.cs
                    {"InsufficientPermissions", "Permessi insufficienti! Permesso richiesto: {0}"},
                    {"SystemNotInitialized", "Errore: Il sistema di avvisi non è stato inizializzato correttamente. Contatta l'amministratore del server."},
                    {"NoArgumentsHelp", "Sistema di avvisi/note - Comandi disponibili:\n" +
                                       ".{0} check <ID Giocatore/@UserId> - Mostra gli avvisi e le note del giocatore.\n" +
                                       ".{0} add <ID Giocatore/@UserId> <Motivo> - Emette un avviso al giocatore.\n" +
                                       ".{0} note <ID Giocatore/@UserId> <Contenuto della nota> - Aggiunge una nota sul giocatore.\n" +
                                       ".{0} delete <ID Avviso/Nota> - Elimina la voce con l'ID specificato."},
                    {"CheckWarnUsage", "Uso corretto: .{0} check <ID Giocatore/@UserId>"},
                    {"PlayerNotFound", "Nessun giocatore online trovato con l'ID fornito. Per eseguire un'azione su un giocatore offline, fornisci il suo UserId completo (es. 123456789@steam o 12345@discord)."},
                    {"NoWarnsOrNotes", "Nessun avviso o nota trovati per il giocatore {0} (UserId: {1})."},
                    {"WarnsNotesHeader", "Avvisi/Note per il giocatore {0} (UserId: {1}):\n--------------------"},
                    {"WarnUsage", "Uso corretto: .{0} add <ID Giocatore/@UserId> <Motivo>"},
                    {"WarnAdded", "Avviso aggiunto (ID: {0}) per il giocatore {1} (UserId: {2}). Motivo: {3}"},
                    {"NoteUsage", "Uso corretto: .{0} note <ID Giocatore/@UserId> <Contenuto della nota>"},
                    {"NoteAdded", "Nota aggiunta (ID: {0}) per il giocatore {1} (UserId: {2})."},
                    {"DeleteUsage", "Uso corretto: .{0} delete <ID Avviso/Nota>"},
                    {"InvalidId", "ID non valido: '{0}'. L'ID deve essere un numero intero."},
                    {"EntryDeleted", "Voce di tipo '{0}' con ID {1} eliminata (riguarda il giocatore {2} - {3})."},
                    {"EntryNotFound", "Nessuna voce con ID {0} trovata o si è verificato un errore durante l'eliminazione."},

                    // AuthPatch.cs (only specified keys)
                    {"Comments", "Commenti"},
                    {"CommentsNone", "nessuno"},
                    {"CommentsError", "impossibile recuperare i dati"},
                    {"CommentFormat", "- {0}({1}) ha lasciato un commento: {2}"},
                    {"Warns", "Avvisi"},
                    {"WarnsNone", "nessuno"},
                    {"WarnsError", "impossibile recuperare i dati"},
                }
            },
            {
                "zh", new Dictionary<string, string>
                {
                    // WarnCommands.cs
                    {"InsufficientPermissions", "权限不足！所需权限：{0}"},
                    {"SystemNotInitialized", "错误：警告系统未正确初始化。请联系服务器管理员。"},
                    {"NoArgumentsHelp", "警告/备注系统 - 可用命令：\n" +
                                       ".{0} check <玩家ID/@UserId> - 显示玩家的警告和备注。\n" +
                                       ".{0} add <玩家ID/@UserId> <原因> - 给玩家发出警告。\n" +
                                       ".{0} note <玩家ID/@UserId> <备注内容> - 添加关于玩家的备注。\n" +
                                       ".{0} delete <警告/备注ID> - 删除指定ID的条目。"},
                    {"CheckWarnUsage", "正确用法：.{0} check <玩家ID/@UserId>"},
                    {"PlayerNotFound", "未找到在线玩家与提供的ID匹配。要对离线玩家执行操作，请提供其完整UserId（例如123456789@steam或12345@discord）。"},
                    {"NoWarnsOrNotes", "未找到玩家 {0}（UserId：{1}）的任何警告或备注。"},
                    {"WarnsNotesHeader", "玩家 {0}（UserId：{1}）的警告/备注：\n--------------------"},
                    {"WarnUsage", "正确用法：.{0} add <玩家ID/@UserId> <原因>"},
                    {"WarnAdded", "已为玩家 {1}（UserId：{2}）添加警告（ID：{0}）。原因：{3}"},
                    {"NoteUsage", "正确用法：.{0} note <玩家ID/@UserId> <备注内容>"},
                    {"NoteAdded", "已为玩家 {1}（UserId：{2}）添加备注（ID：{0}）。"},
                    {"DeleteUsage", "正确用法：.{0} delete <警告/备注ID>"},
                    {"InvalidId", "无效ID：'{0}'。ID必须是整数。"},
                    {"EntryDeleted", "已删除类型为 '{0}' 的条目，ID为 {1}（涉及玩家 {2} - {3}）。"},
                    {"EntryNotFound", "未找到ID为 {0} 的条目，或删除时发生错误。"},

                    // AuthPatch.cs (only specified keys)
                    {"Comments", "评论"},
                    {"CommentsNone", "无"},
                    {"CommentsError", "无法检索数据"},
                    {"CommentFormat", "- {0}({1}) 留下了评论：{2}"},
                    {"Warns", "警告"},
                    {"WarnsNone", "无"},
                    {"WarnsError", "无法检索数据"},
                }
            },
            {
                "ja", new Dictionary<string, string>
                {
                    // WarnCommands.cs
                    {"InsufficientPermissions", "権限が不足しています！必要な権限：{0}"},
                    {"SystemNotInitialized", "エラー：警告システムが正しく初期化されていません。サーバー管理者に連絡してください。"},
                    {"NoArgumentsHelp", "警告/メモシステム - 利用可能なコマンド：\n" +
                                       ".{0} check <プレイヤーID/@UserId> - プレイヤーの警告とメモを表示します。\n" +
                                       ".{0} add <プレイヤーID/@UserId> <理由> - プレイヤーに警告を発行します。\n" +
                                       ".{0} note <プレイヤーID/@UserId> <メモ内容> - プレイヤーに関するメモを追加します。\n" +
                                       ".{0} delete <警告/メモID> - 指定されたIDのエントリを削除します。"},
                    {"CheckWarnUsage", "正しい使用法：.{0} check <プレイヤーID/@UserId>"},
                    {"PlayerNotFound", "指定されたIDのオンラインプレイヤーが見つかりません。オフラインプレイヤーに対して操作を行うには、完全なUserId（例：123456789@steamまたは12345@discord）を指定してください。"},
                    {"NoWarnsOrNotes", "プレイヤー {0}（UserId：{1}）の警告やメモは見つかりませんでした。"},
                    {"WarnsNotesHeader", "プレイヤー {0}（UserId：{1}）の警告/メモ：\n--------------------"},
                    {"WarnUsage", "正しい使用法：.{0} add <プレイヤーID/@UserId> <理由>"},
                    {"WarnAdded", "プレイヤー {1}（UserId：{2}）に警告（ID：{0}）を追加しました。理由：{3}"},
                    {"NoteUsage", "正しい使用法：.{0} note <プレイヤーID/@UserId> <メモ内容>"},
                    {"NoteAdded", "プレイヤー {1}（UserId：{2}）にメモ（ID：{0}）を追加しました。"},
                    {"DeleteUsage", "正しい使用法：.{0} delete <警告/メモID>"},
                    {"InvalidId", "無効なID：'{0}'。IDは整数でなければなりません。"},
                    {"EntryDeleted", "タイプ '{0}' のエントリ（ID：{1}）を削除しました（プレイヤー {2} - {3} に関連）。"},
                    {"EntryNotFound", "ID {0} のエントリが見つからないか、削除中にエラーが発生しました。"},

                    // AuthPatch.cs (only specified keys)
                    {"Comments", "コメント"},
                    {"CommentsNone", "なし"},
                    {"CommentsError", "データを取得できませんでした"},
                    {"CommentFormat", "- {0}({1}) がコメントを残しました：{2}"},
                    {"Warns", "警告"},
                    {"WarnsNone", "なし"},
                    {"WarnsError", "データを取得できませんでした"},
                }
            },
            {
                "ko", new Dictionary<string, string>
                {
                    // WarnCommands.cs
                    {"InsufficientPermissions", "권한이 부족합니다! 필요한 권한: {0}"},
                    {"SystemNotInitialized", "오류: 경고 시스템이 올바르게 초기화되지 않았습니다. 서버 관리자에게 문의하십시오."},
                    {"NoArgumentsHelp", "경고/노트 시스템 - 사용 가능한 명령어:\n" +
                                       ".{0} check <플레이어 ID/@UserId> - 플레이어의 경고 및 노트를 표시합니다.\n" +
                                       ".{0} add <플레이어 ID/@UserId> <사유> - 플레이어에게 경고를 발행합니다.\n" +
                                       ".{0} note <플레이어 ID/@UserId> <노트 내용> - 플레이어에 대한 노트를 추가합니다.\n" +
                                       ".{0} delete <경고/노트 ID> - 지정된 ID의 항목을 삭제합니다."},
                    {"CheckWarnUsage", "올바른 사용법: .{0} check <플레이어 ID/@UserId>"},
                    {"PlayerNotFound", "제공된 ID로 온라인 플레이어를 찾을 수 없습니다. 오프라인 플레이어에 대한 작업을 수행하려면 전체 UserId(예: 123456789@steam 또는 12345@discord)를 제공하십시오."},
                    {"NoWarnsOrNotes", "플레이어 {0} (UserId: {1})에 대한 경고나 노트가 없습니다."},
                    {"WarnsNotesHeader", "플레이어 {0} (UserId: {1})의 경고/노트:\n--------------------"},
                    {"WarnUsage", "올바른 사용법: .{0} add <플레이어 ID/@UserId> <사유>"},
                    {"WarnAdded", "플레이어 {1} (UserId: {2})에 경고(ID: {0})를 추가했습니다. 사유: {3}"},
                    {"NoteUsage", "올바른 사용법: .{0} note <플레이어 ID/@UserId> <노트 내용>"},
                    {"NoteAdded", "플레이어 {1} (UserId: {2})에 노트(ID: {0})를 추가했습니다."},
                    {"DeleteUsage", "올바른 사용법: .{0} delete <경고/노트 ID>"},
                    {"InvalidId", "잘못된 ID: '{0}'. ID는 정수여야 합니다."},
                    {"EntryDeleted", "'{0}' 유형의 항목(ID: {1})이 삭제되었습니다 (플레이어 {2} - {3} 관련)."},
                    {"EntryNotFound", "ID {0}의 항목을 찾을 수 없거나 삭제 중 오류가 발생했습니다."},

                    // AuthPatch.cs (only specified keys)
                    {"Comments", "댓글"},
                    {"CommentsNone", "없음"},
                    {"CommentsError", "데이터를 검색할 수 없습니다"},
                    {"CommentFormat", "- {0}({1})님이 댓글을 남겼습니다: {2}"},
                    {"Warns", "경고"},
                    {"WarnsNone", "없음"},
                    {"WarnsError", "데이터를 검색할 수 없습니다"},
                }
            }
        };

            /// <summary>
            /// Retrieves a translated string for the given key and language.
            /// </summary>
            /// <param name="key">The translation key.</param>
            /// <param name="language">The language code (e.g., 'en', 'ru').</param>
            /// <param name="args">Optional arguments for string formatting.</param>
            /// <returns>The translated string, or the key if translation is not found.</returns>
            public static string GetTranslation(string key, string language, params object[] args)
            {
                if (string.IsNullOrEmpty(language))
                    language = "en"; // Default to English if language is not set

                if (Translations.TryGetValue(language, out var languageDict) && languageDict.TryGetValue(key, out var translation))
                {
                    return args.Length > 0 ? string.Format(translation, args) : translation;
                }

                // Fallback to English if the language or key is not found
                if (Translations.TryGetValue("en", out var defaultDict) && defaultDict.TryGetValue(key, out var defaultTranslation))
                {
                    return args.Length > 0 ? string.Format(defaultTranslation, args) : defaultTranslation;
                }

                // Return the key itself as a last resort
                return key;
            }
        }
    }
}