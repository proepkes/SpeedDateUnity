using System.Collections;
using System.Net;
using SpeedDate.Client;
using SpeedDate.ClientPlugins.Peer.Auth;
using SpeedDate.Configuration;
using SpeedDate.Plugin.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
	public static Client Instance;
	
	private readonly SpeedDateClient _client = new SpeedDateClient();
	
	public string IpAddress = IPAddress.Loopback.ToString();

	public int Port = 60125;
	
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(this.gameObject);
		}
	}
	
	void Start ()
	{
		_client.Started += () => { Debug.Log("Client connected to server"); };
		_client.Start(new DefaultConfigProvider(new NetworkConfig(IpAddress, Port), PluginsConfig.DefaultPeerPlugins));
	}
	
	public T GetPlugin<T>() where T : class, IPlugin
	{
		return _client.GetPlugin<T>();
	}
}
