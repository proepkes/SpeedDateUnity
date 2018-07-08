using SpeedDate.ClientPlugins.Peer.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUi : MonoBehaviour
{
	public RectTransform LoginDialog;
	public RectTransform RegisterDialog;
	
	public void OnLoginClick()
	{
		LoginDialog.gameObject.SetActive(true);
	}

	public void OnLoginAsGuestClick()
	{
		Client.Instance.GetPlugin<AuthPlugin>().LogInAsGuest(info =>
		{
			Debug.Log($"Logged in as {info.Username}");
			UnityMainThreadDispatcher.Instance().Enqueue(() => SceneManager.LoadScene("Lobby"));
		}, error =>
		{
			Debug.Log($"Login failed: {error}");
		});
	}

	public void OnRegisterClick()
	{
		RegisterDialog.gameObject.SetActive(true);
	}
}
