using System.Collections;
using System.Collections.Generic;
using SpeedDate.ClientPlugins.Peer.Auth;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{

	private Client _client;
	
	public Text WelcomeText;
	
	// Use this for initialization
	void Start ()
	{
		_client = FindObjectOfType<Client>();
		WelcomeText.text = WelcomeText.text.Replace("%name", _client.GetPlugin<AuthPlugin>().AccountInfo.Username);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
