# 📌 WarnSystem
SCP:SL EXILED advanced warn system!

Commands:

<p>- warn check (Player ID) - Displays a player's warns and notes.</p>
<p>- warn add (Player ID) (Reason) - Gives a warn to a player.</p>
<p>- warn note (Player ID) (Reason) - Adds a note about the player.</p>
<p>- warn delete (Warn/Note ID) - Deletes the entry with the specified ID.</p>

# 🛠️ Requirements
EXILED >= 9.5.1

# ⚙️ Configuration
```
warn_system:
  is_enabled: true
  debug: false
  data_file_name: 'WarnData.json'
  required_permission: 'perm.warn.adm'
  command_prefix: 'warn'
  command_aliases:
  - 'ws'
  - 'warns'
```
