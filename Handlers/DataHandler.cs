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
                        Log.Debug($"[DataHandler] Created data directory: {directoryPath}");
                    }

                    if (File.Exists(_filePath))
                    {
                        string json = File.ReadAllText(_filePath);
                        _warnEntries = JsonConvert.DeserializeObject<List<WarnEntry>>(json) ?? new List<WarnEntry>();

                        _nextId = _warnEntries.Any() ? _warnEntries.Max(e => e.Id) + 1 : 1;

                        Log.Info($"[DataHandler] Loaded {_warnEntries.Count} entries from {_filePath}. Next ID: {_nextId}");
                    }
                    else
                    {
                        Log.Info($"[DataHandler] Data file {_filePath} does not exist. It will be created on first save.");
                        _warnEntries = new List<WarnEntry>();
                        _nextId = 1;
                    }
                }
                catch (JsonException jsonEx)
                {
                    Log.Error($"[DataHandler] Deserialization error for file {_filePath}: {jsonEx.Message}. Starting with an empty list.");
                    _warnEntries = new List<WarnEntry>();
                    _nextId = 1;
                }
                catch (Exception ex)
                {
                    Log.Error($"[DataHandler] Failed to load data from file {_filePath}: {ex.Message}");
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
                        Log.Debug($"[DataHandler] Created data directory before saving: {directoryPath}");
                    }

                    string json = JsonConvert.SerializeObject(_warnEntries, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(_filePath, json);
                    Log.Debug($"[DataHandler] Saved {_warnEntries.Count} entries to {_filePath}.");
                }
                catch (Exception ex)
                {
                    Log.Error($"[DataHandler] Failed to save data to file {_filePath}: {ex.Message}");
                    Log.Debug(ex.ToString());
                }
            }
        }

        public WarnEntry AddEntry(string type, string targetUserId, string targetNickname, Player admin, string reason)
        {
            lock (_lock)
            {
                string adminUserId = admin?.UserId ?? "SERVER";
                string adminNickname = admin?.Nickname ?? "Server Console";

                if (string.IsNullOrWhiteSpace(targetNickname))
                    targetNickname = $"({targetUserId})";

                WarnEntry newEntry = new WarnEntry(_nextId, type, targetUserId, targetNickname, adminUserId, adminNickname, reason);

                _warnEntries.Add(newEntry);
                _nextId++;
                SaveData();
                Log.Info($"[DataHandler] Added {type} entry (ID: {newEntry.Id}) for {targetNickname} ({targetUserId}) by {adminNickname} ({adminUserId}).");
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
                        Log.Info($"[DataHandler] Deleted entry ID: {entryId} (Type: {deletedEntry.Type}, Player: {deletedEntry.PlayerNickname} {deletedEntry.PlayerUserId}).");
                        return true;
                    }
                    else
                    {
                        Log.Warn($"[DataHandler] Failed to remove entry ID: {entryId} from the list, even though it was found.");
                    }
                }
                else
                {
                    Log.Debug($"[DataHandler] No entry with ID: {entryId} found for deletion.");
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