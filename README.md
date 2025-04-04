# 📌 WarnSystem
SCP:SL EXILED advanced warn system!

Commands:

warn check (Player ID) - Displays a player's warns and notes.
warn add (Player ID) (Reason) - Gives a warn to a player.
warn note (Player ID) (Reason) - Adds a note about the player.
warn delete (Warn/Note ID) - Deletes the entry with the specified ID.

# 🛠️ Requirements
EXILED >= 9.5.1

# ⚙️ Configuration
```
warn_system:
# ON/OFF
  is_enabled: true
  # Debug Mode
  debug: false
  # Nazwa pliku do przechowywania danych warnów/notatek w folderze PluginData.
  data_file_name: 'WarnData.json'
  # Wymagana permisja do używania warnów.
  required_permission: 'perm.warn.adm'
  # Główna nazwa komendy, której używa plugin.
  command_prefix: 'warn'
  # Aliasy komendy (alternatywne nazwy)
  command_aliases:
  - 'ws'
  - 'warns'
```
