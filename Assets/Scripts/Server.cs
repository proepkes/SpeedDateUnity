using System.Collections;
using System.Collections.Generic;
using System.Net;
using SpeedDate.Configuration;
using SpeedDate.Interfaces;
using SpeedDate.Server;
using SpeedDate.ServerPlugins.Authentication;
using UnityEngine;

public class Server : MonoBehaviour 
{
	private readonly ISpeedDateStartable _server = new SpeedDateServer();
	// Use this for initialization
	void Start ()
	{
		_server.Started += () =>
		{
			Debug.Log("Server started");
		};
		_server.Start(new DefaultConfigProvider(new NetworkConfig(IPAddress.Any, 60125), PluginsConfig.DefaultServerPlugins, new []
		{
			new AuthConfig
			{
				GuestPrefix = "Guest-",
				EnableGuestLogin = true
			}
		}));
	}
}
