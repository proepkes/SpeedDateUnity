using System.Collections;
using System.Net;
using SpeedDate.Client;
using SpeedDate.ClientPlugins.Peer.Auth;
using SpeedDate.ClientPlugins.Spawner;
using SpeedDate.Configuration;
using SpeedDate.Packets.Spawner;
using SpeedDate.Plugin.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour 
{
	private readonly SpeedDateClient _client = new SpeedDateClient();
	
	public string IpAddress = IPAddress.Loopback.ToString();

	public int Port = 60125;
	
	private void Awake()
	{
		DontDestroyOnLoad(this);
	}
	
	// Use this for initialization
	void Start ()
	{
		_client.Started += () =>
		{
			Debug.Log("Spawner connected to server"); 
			
			GetPlugin<SpawnerPlugin>().RegisterSpawner(new SpawnerOptions(), spawner =>
			{
				Debug.Log("Spawner registered to server");
			}, Debug.Log);
		};
		_client.Start(new DefaultConfigProvider(new NetworkConfig(IpAddress, Port), PluginsConfig.DefaultSpawnerPlugins));
	}
	
	public T GetPlugin<T>() where T : class, IPlugin
	{
		return _client.GetPlugin<T>();
	}
}
