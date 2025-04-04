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

        [Description("Nazwa pliku do przechowywania danych warnów/notatek w folderze PluginData.")]
        public string DataFileName { get; set; } = "WarnData.json";

        [Description("Wymagana permisja do używania warnów.")]
        public string RequiredPermission { get; set; } = "perm.warn.adm";

        [Description("Główna nazwa komendy, której używa plugin.")]
        public string CommandPrefix { get; set; } = "warn";

        [Description("Aliasy komendy (alternatywne nazwy)")]
        public string[] CommandAliases { get; set; } = { "ws", "warns" };
    }
}