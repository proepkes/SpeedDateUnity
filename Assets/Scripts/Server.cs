using System.Collections;
using System.Collections.Generic;
using System.Net;
using SpeedDate.Configuration;
using SpeedDate.Interfaces;
using SpeedDate.Server;
using SpeedDate.ServerPlugins.Authentication;
using SpeedDate.ServerPlugins.Lobbies;
using UnityEngine;

public class Server : MonoBehaviour 
{
	private readonly SpeedDateServer _server = new SpeedDateServer();
	
	public int Port = 60125;
	
	public bool EnableGuestLogin = true;

	public string GuestPrefix = "Guest-";

	private void Awake()
	{
		DontDestroyOnLoad(this);
	}

	// Use this for initialization
	void Start ()
	{
		
		_server.Started += () =>
		{
			Debug.Log("Server started");
		};
		_server.Start(new DefaultConfigProvider(new NetworkConfig(IPAddress.Any, Port), PluginsConfig.DefaultServerPlugins, new []
		{
			new AuthConfig
			{
				GuestPrefix = GuestPrefix,
				EnableGuestLogin = EnableGuestLogin
			}
		}));
		_server.GetPlugin<LobbiesPlugin>().AddFactory(new LobbyFactoryAnonymous("2 vs 2 vs 4", _server.GetPlugin<LobbiesPlugin>(), DemoLobbyFactories.TwoVsTwoVsFour));
		_server.GetPlugin<LobbiesPlugin>().AddFactory(new LobbyFactoryAnonymous("3 vs 3 auto", _server.GetPlugin<LobbiesPlugin>(), DemoLobbyFactories.ThreeVsThreeQueue));
	}
}
