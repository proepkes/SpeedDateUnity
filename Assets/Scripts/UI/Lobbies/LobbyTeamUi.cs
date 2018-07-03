using SpeedDate.Packets.Lobbies;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a view of the lobby team
/// </summary>
public class LobbyTeamUi : MonoBehaviour
{
    public Text TeamName;
    public LayoutGroup UsersLayoutGroup;

    public GameObject Header;

    /// <summary>
    /// If the team name is empty, hides the team header (name)
    /// </summary>
    public bool DisableHeaderIfNameIsEmpty = true;

    public Button JoinButton;

    public LobbyTeamData RawData;

    public bool ShowMinMax = true;

    public LobbyUi Lobby;

    void Awake()
    {
        Lobby = Lobby ?? GetComponentInParent<LobbyUi>();
    }

    /// <summary>
    /// Name of the team
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// Sets up the team view from the data given
    /// </summary>
    /// <param name="teamName"></param>
    /// <param name="properties"></param>
    public virtual void Setup(string teamName, LobbyTeamData data)
    {
        RawData = data;

        Name = teamName;
        UpdateName();

        // Toggle header
        if (string.IsNullOrEmpty(teamName) && DisableHeaderIfNameIsEmpty)
        {
            Header.SetActive(false);
        }
        else
        {
            Header.SetActive(true);
        }
    }

    public virtual void UpdateName()
    {
        var newName = RawData.Name;

        if (ShowMinMax)
        {
            newName += string.Format(" (min: {0}, max:{1})", RawData.MinPlayers, RawData.MaxPlayers);
        }

        TeamName.text = newName;
    }

    /// <summary>
    /// Invoked, when user clicks a "Join" button
    /// </summary>
    public virtual void OnJoinClick()
    {
        Lobby.JoinedLobby.JoinTeam(Name, () => { }, Debug.Log);
    }

    /// <summary>
    /// Resets the team view
    /// </summary>
    public void Reset()
    {
        Header.gameObject.SetActive(true);
    }
}
