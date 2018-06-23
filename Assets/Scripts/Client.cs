using System.Collections;
using System.Net;
using SpeedDate.Client;
using SpeedDate.ClientPlugins.Peer.Auth;
using SpeedDate.Configuration;
using UnityEngine;


public class Client : MonoBehaviour 
{
	private readonly SpeedDateClient _client = new SpeedDateClient();
	
	public string IpAddress = IPAddress.Loopback.ToString();

	public int Port = 60125;
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(CheckConnection());
		
		_client.Started += () => { Debug.Log("Connected to server"); };
		_client.Start(new DefaultConfigProvider(new NetworkConfig(IpAddress, Port), PluginsConfig.DefaultPeerPlugins));
	}
	
	private IEnumerator CheckConnection()
	{
		yield return new WaitForSeconds(2);

		if (!_client.IsConnected)
		{
			_client.Stop();
			Debug.Log("Connection failed");
		}
	}

	public void LoginAsGuest()
	{
		_client.GetPlugin<AuthPlugin>().LogInAsGuest(info =>
		{
			Debug.Log($"Logged in as {info.Username}");
		}, error =>
		{
			Debug.Log($"Login failed: {error}");
		});
	}
}
