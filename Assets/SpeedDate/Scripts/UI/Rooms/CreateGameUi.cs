using System;
using System.Collections.Generic;
using System.Linq;
using SpeedDate;
using SpeedDate.ClientPlugins.Peer.Auth;
using SpeedDate.ClientPlugins.Peer.SpawnRequest;
using UnityEngine;
using UnityEngine.UI;

namespace Barebones.MasterServer
{
    /// <summary>
    ///     Game creation window
    /// </summary>
    public class CreateGameUi : MonoBehaviour
    {
        private Client client;
        
        public Dropdown Map;

        public Image MapImage;

        public List<MapSelection> Maps;
        public int MaxNameLength = 14;
        public Dropdown MaxPlayers;

        public int MaxPlayersLowerLimit = 2;
        public int MaxPlayersUpperLimit = 10;

        public int MinNameLength = 3;

        public CreateGameProgressUi ProgressUi;
        public InputField RoomName;

        public bool RequireAuthentication = true;

        public bool SetAsLastSiblingOnEnable = true;

        protected virtual void Awake()
        {
            ProgressUi = ProgressUi ?? FindObjectOfType<CreateGameProgressUi>();
            Map.ClearOptions();
            Map.AddOptions(Maps.Select(m => new Dropdown.OptionData(m.Name)).ToList());

            OnMapChange();
        }

        private void Start()
        {
            client = FindObjectOfType<Client>();
        }

        public void OnEnable()
        {
            if (SetAsLastSiblingOnEnable)
                transform.SetAsLastSibling();
        }

        public void OnCreateClick()
        {
            if (ProgressUi == null)
            {
                Debug.Log("You need to set a ProgressUi");
                return;
            }

            if (RequireAuthentication && !client.GetPlugin<AuthPlugin>().IsLoggedIn)
            {
                Debug.Log("You must be logged in to create a room");
                return;
            }

            var roomName = RoomName.text.Trim();

            if (string.IsNullOrEmpty(roomName) || (roomName.Length < MinNameLength) || (roomName.Length > MaxNameLength))
            {
                Debug.Log($"Invalid length of game name, shoul be between {MinNameLength} and {MaxNameLength}");
                return;
            }

            var maxPlayers = 0;
            int.TryParse(MaxPlayers.captionText.text, out maxPlayers);

            if ((maxPlayers < MaxPlayersLowerLimit) || (maxPlayers > MaxPlayersUpperLimit))
            {
                Debug.Log($"Invalid number of max players. Value should be between {MaxPlayersLowerLimit} and {MaxPlayersUpperLimit}");
                return;
            }

            var settings = new Dictionary<string, string>
            {
                {OptionKeys.MaxPlayers, maxPlayers.ToString()},
                {OptionKeys.RoomName, roomName},
                {OptionKeys.MapName, GetSelectedMap().Name},
                {OptionKeys.SceneName, GetSelectedMap().Scene.SceneName}
            };

            client.GetPlugin<SpawnRequestPlugin>().RequestSpawn(settings, "", requestController =>
            {
                ProgressUi.Display(requestController);
            }, error =>
            {
                ProgressUi.gameObject.SetActive(false);
                Debug.Log("Failed to create a game: " + error);
            });
        }

        public MapSelection GetSelectedMap()
        {
            var text = Map.captionText.text;
            return Maps.FirstOrDefault(m => m.Name == text);
        }

        public void OnMapChange()
        {
            var selected = GetSelectedMap();

            if (selected == null)
            {
                Debug.Log("Invalid map selection");
                return;
            }

            MapImage.sprite = selected.Sprite;
        }

        [Serializable]
        public class MapSelection
        {
            public string Name;
            public SceneField Scene;
            public Sprite Sprite;
        }
    }
}
