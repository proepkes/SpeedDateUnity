using System;
using System.Collections.Generic;
using SpeedDate.ClientPlugins.Peer.Lobby;
using SpeedDate.ClientPlugins.Peer.MatchMaker;
using SpeedDate.ClientPlugins.Peer.Room;
using SpeedDate.Packets.Matchmaking;
using SpeedDate.Packets.Rooms;
using UnityEngine;
using UnityEngine.UI;

namespace Barebones.MasterServer
{
    /// <summary>
    ///     Represents a list of game servers
    /// </summary>
    public class GamesListUi : MonoBehaviour
    {
        private GenericUIList<GameInfoPacket> _items;
        public GameObject CreateRoomWindow;

        public Button GameJoinButton;
        public GamesListUiItem ItemPrefab;
        public LayoutGroup LayoutGroup;
        public LobbyUi LobbyUi;

        private Client client;

        // Use this for initialization
        protected virtual void Awake()
        {
            _items = new GenericUIList<GameInfoPacket>(ItemPrefab.gameObject, LayoutGroup);
        }

        private void Start()
        {
            client = FindObjectOfType<Client>();
        }

        protected virtual void HandleRoomsShowEvent(object arg1, object arg2)
        {
            gameObject.SetActive(true);
        }


        public void Setup(IEnumerable<GameInfoPacket> data)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                _items.Generate<GamesListUiItem>(data, (packet, item) => { item.Setup(packet); });
                UpdateGameJoinButton();
            });
        }

        private void UpdateGameJoinButton()
        {
            GameJoinButton.interactable = GetSelectedItem() != null;
        }

        public GamesListUiItem GetSelectedItem()
        {
            return _items.FindObject<GamesListUiItem>(item => item.IsSelected);
        }

        public void Select(GamesListUiItem gamesListItem)
        {
            _items.Iterate<GamesListUiItem>(item => { item.SetIsSelected(!item.IsSelected && (gamesListItem == item)); });
            UpdateGameJoinButton();
        }

        public void OnRefreshClick()
        {
            RequestRooms();
        }

        public void OnJoinGameClick()
        {
            var selected = GetSelectedItem();

            if (selected == null)
                return;

            if (selected.IsLobby)
            {
                OnJoinLobbyClick(selected.RawData);
                return;
            }

            client.GetPlugin<RoomPlugin>().GetAccess(
                selected.GameId, 
                OnPassReceived,
                selected.IsPasswordProtected ? "password" : string.Empty, 
                new Dictionary<string, string>(), 
                Debug.Log);
        }

        protected virtual void OnJoinLobbyClick(GameInfoPacket packet)
        {
            client.GetPlugin<LobbyPlugin>().JoinLobby(packet.Id, (lobby) =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    // Hide this window
                    gameObject.SetActive(false);

                    if (LobbyUi == null)
                    {
                        Debug.Log("Couldn't find appropriate UI element to display lobby data in the scene. " +
                                  "Override OnJoinLobbyClick method, if you want to handle this differently");
                        return;
                    }

                    lobby.SetListener(LobbyUi);
                    
                    LobbyUi.gameObject.SetActive(true);
                });
            }, Debug.Log);
        }

        protected virtual void OnPassReceived(RoomAccessPacket packet)
        {
            // Hope something handles the event
        }

        protected virtual void RequestRooms()
        {
            client.GetPlugin<MatchmakerPlugin>().FindGames(new Dictionary<string, string>(), Setup, Debug.Log);
        }

        public void OnCreateGameClick()
        {
            if (CreateRoomWindow == null)
            {
                Debug.Log("You need to set a CreateRoomWindow");
                return;
            }
            CreateRoomWindow.gameObject.SetActive(true);
        }

        public void OnCloseClick()
        {
            gameObject.SetActive(false);
        }
    }
}
