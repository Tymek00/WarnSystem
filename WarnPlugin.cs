using System;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using WarnSystem.Handlers;

using ServerEvents = Exiled.Events.Handlers.Server;

namespace WarnSystem
{
    public class WarnPlugin : Plugin<Config>
    {
        public override string Name => "WarnSystem";
        public override string Author => "Tymek";
        public override Version Version => new Version(2, 0, 0);
        public override Version RequiredExiledVersion => new Version(9, 5, 2);

        public static WarnPlugin Instance { get; private set; }

        public DataHandler DataHandler { get; private set; }
        private EventHandlers _eventHandlers;

        public override void OnEnabled()
        {
            Instance = this;
            DataHandler = new DataHandler(this);
            _eventHandlers = new EventHandlers(this);

            ServerEvents.WaitingForPlayers += _eventHandlers.OnWaitingForPlayers;
            ServerEvents.RestartingRound += _eventHandlers.OnRestartingRound;

            Log.Info($"{Name} v{Version} by {Author} has been loaded.");
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            ServerEvents.WaitingForPlayers -= _eventHandlers.OnWaitingForPlayers;
            ServerEvents.RestartingRound -= _eventHandlers.OnRestartingRound;

            DataHandler?.SaveData();
            Log.Info($"{Name} has been disabled. Data saved (if available).");

            _eventHandlers = null;
            DataHandler = null;
            Instance = null;
            base.OnDisabled();
        }
    }

    public class EventHandlers
    {
        private readonly WarnPlugin _plugin;
        public EventHandlers(WarnPlugin plugin) => _plugin = plugin;

        public void OnWaitingForPlayers()
        {
            Log.Debug("WaitingForPlayers: Loading data...");
            _plugin.DataHandler?.LoadData();
        }

        public void OnRestartingRound()
        {
            Log.Debug("RestartingRound: Saving data...");
            _plugin.DataHandler?.SaveData();
        }
    }
}