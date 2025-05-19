using System.ComponentModel;
using Exiled.API.Interfaces;

namespace WarnSystem
{
    public class Config : IConfig
    {
        [Description("ON/OFF")]
        public bool IsEnabled { get; set; } = true;

        [Description("Debug Mode")]
        public bool Debug { get; set; } = false;

        [Description("Name of the file to store warn/notes data in the PluginData folder.")]
        public string DataFileName { get; set; } = "WarnData.json";

        [Description("Required permission to use warns.")]
        public string RequiredPermission { get; set; } = "perm.warn.adm";

        [Description("Main command name used by the plugin.")]
        public string CommandPrefix { get; set; } = "warn";

        [Description("Command aliases (alternative names)")]
        public string[] CommandAliases { get; set; } = { "ws", "warns" };

        [Description("Language code for localization (e.g., 'en' for English, 'ru' for Russian).")]
        public string Language { get; set; } = "en";
    }
}