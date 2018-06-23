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
	
	public int Port = 60125;
	
	public bool EnableGuestLogin = true;

	public string GuestPrefix = "Guest-";
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
	}
}
