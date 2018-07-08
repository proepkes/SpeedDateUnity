using System.Collections;
using System.Collections.Generic;
using SpeedDate.ClientPlugins.Peer.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
	public Text WelcomeText;
	
	// Use this for initialization
	void Start ()
	{
		WelcomeText.text = WelcomeText.text.Replace("%name", Client.Instance.GetPlugin<AuthPlugin>().AccountInfo.Username);
	}

	public void OnLogoutClick()
	{
		Client.Instance.GetPlugin<AuthPlugin>().LogOut();
		SceneManager.LoadScene("Login");
	}
}
