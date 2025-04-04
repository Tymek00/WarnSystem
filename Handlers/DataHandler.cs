using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Exiled.API.Features;
using Newtonsoft.Json; 
using WarnSystem.Models; 

namespace WarnSystem.Handlers
{
    public class DataHandler
    {
        private readonly WarnPlugin _plugin;
        private readonly string _filePath; 
        private List<WarnEntry> _warnEntries;
        private int _nextId;
        private readonly object _lock = new object(); 

        public DataHandler(WarnPlugin plugin)
        {
            _plugin = plugin;
            string pluginDataDir = Path.Combine(Exiled.API.Features.Paths.Plugins, "data");
            string pluginSpecificDir = Path.Combine(pluginDataDir, _plugin.Name);
            _filePath = Path.Combine(pluginSpecificDir, _plugin.Config.DataFileName);

            _warnEntries = new List<WarnEntry>();
            _nextId = 1;

        }
        public void LoadData()
        {
            lock (_lock)
            {
                try
                {
                    string directoryPath = Path.GetDirectoryName(_filePath);
                    if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                        Log.Debug($"[DataHandler] Utworzono katalog danych: {directoryPath}");
                    }

                    if (File.Exists(_filePath))
                    {
                        string json = File.ReadAllText(_filePath);
                        _warnEntries = JsonConvert.DeserializeObject<List<WarnEntry>>(json) ?? new List<WarnEntry>();

                        _nextId = _warnEntries.Any() ? _warnEntries.Max(e => e.Id) + 1 : 1;

                        Log.Info($"[DataHandler] Załadowano {_warnEntries.Count} wpisów z {_filePath}. Następne ID: {_nextId}");
                    }
                    else
                    {
                        Log.Info($"[DataHandler] Plik danych {_filePath} nie istnieje. Zostanie utworzony przy pierwszym zapisie.");
                        _warnEntries = new List<WarnEntry>();
                        _nextId = 1;
                    }
                }
                catch (JsonException jsonEx)
                {
                    Log.Error($"[DataHandler] Błąd deserializacji pliku {_filePath}: {jsonEx.Message}. Rozpoczynanie z pustą listą.");
                    _warnEntries = new List<WarnEntry>();
                    _nextId = 1;
                }
                catch (Exception ex)
                {
                    Log.Error($"[DataHandler] Nie udało się załadować danych z pliku {_filePath}: {ex.Message}");
                    Log.Debug(ex.ToString());
                    _warnEntries = new List<WarnEntry>();
                    _nextId = 1;
                }
            }
        }

        public void SaveData()
        {
            lock (_lock)
            {
                try
                {
                    string directoryPath = Path.GetDirectoryName(_filePath);
                    if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                        Log.Debug($"[DataHandler] Utworzono katalog danych przed zapisem: {directoryPath}");
                    }

                    string json = JsonConvert.SerializeObject(_warnEntries, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(_filePath, json);
                    Log.Debug($"[DataHandler] Zapisano {_warnEntries.Count} wpisów do {_filePath}.");
                }
                catch (Exception ex)
                {
                    Log.Error($"[DataHandler] Nie udało się zapisać danych do pliku {_filePath}: {ex.Message}");
                    Log.Debug(ex.ToString());
                }
            }
        }

        public WarnEntry AddEntry(string type, string targetUserId, string targetNickname, Player admin, string reason)
        {
            lock (_lock)
            {
                string adminUserId = admin?.UserId ?? "SERVER";
                string adminNickname = admin?.Nickname ?? "Konsola Serwera";

                if (string.IsNullOrWhiteSpace(targetNickname))
                    targetNickname = $"({targetUserId})";

                WarnEntry newEntry = new WarnEntry(_nextId, type, targetUserId, targetNickname, adminUserId, adminNickname, reason);

                _warnEntries.Add(newEntry);
                _nextId++;
                SaveData(); 
                Log.Info($"[DataHandler] Dodano wpis {type} (ID: {newEntry.Id}) dla {targetNickname} ({targetUserId}) przez {adminNickname} ({adminUserId}).");
                return newEntry;
            }
        }

        public bool DeleteEntry(int entryId, out WarnEntry deletedEntry)
        {
            lock (_lock)
            {
                deletedEntry = _warnEntries.FirstOrDefault(e => e.Id == entryId);
                if (deletedEntry != null)
                {
                    bool removed = _warnEntries.Remove(deletedEntry);
                    if (removed)
                    {
                        SaveData();
                        Log.Info($"[DataHandler] Usunięto wpis ID: {entryId} (Typ: {deletedEntry.Type}, Gracz: {deletedEntry.PlayerNickname} {deletedEntry.PlayerUserId}).");
                        return true;
                    }
                    else
                    {
                        Log.Warn($"[DataHandler] Nie udało się usunąć wpisu ID: {entryId} z listy, mimo że został znaleziony.");
                    }
                }
                else
                {
                    Log.Debug($"[DataHandler] Nie znaleziono wpisu o ID: {entryId} do usunięcia.");
                }
                deletedEntry = null;
                return false;
            }
        }

        public List<WarnEntry> GetEntriesForPlayer(string playerUserId)
        {
            lock (_lock)
            {
                return _warnEntries.Where(e => e.PlayerUserId.Equals(playerUserId, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        public WarnEntry GetEntryById(int entryId)
        {
            lock (_lock)
            {
                return _warnEntries.FirstOrDefault(e => e.Id == entryId);
            }
        }
    }
}