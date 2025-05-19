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
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(9, 5, 1);

        public static WarnPlugin Instance { get; private set; }

        public DataHandler DataHandlers { get; private set; }
        private EventHandlers _eventHandlers;

        public override void OnEnabled()
        {
            Instance = this;
            DataHandler = new DataHandler(this);
            _eventHandlers = new EventHandlers(this);

            ServerEvents.WaitingForPlayers += _eventHandlers.OnWaitingForPlayers;
            ServerEvents.RestartingRound += _eventHandlers.OnRestartingRound;

            Log.Info($"{Name} v{Version} by {Author} został załadowany.");
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            ServerEvents.WaitingForPlayers -= _eventHandlers.OnWaitingForPlayers;
            ServerEvents.RestartingRound -= _eventHandlers.OnRestartingRound;

            DataHandler?.SaveData(); 
            Log.Info($"{Name} został wyłączony. Zapisano dane (jeśli były dostępne).");

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
            Log.Debug("WaitingForPlayers: Ładowanie danych...");
            _plugin.DataHandler?.LoadData();
        }

        public void OnRestartingRound()
        {
            Log.Debug("RestartingRound: Zapisywanie danych...");
            _plugin.DataHandler?.SaveData();
        }

    }
}