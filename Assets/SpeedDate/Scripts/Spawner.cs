using System.Net;
using SpeedDate.Client;
using SpeedDate.ClientPlugins.Spawner;
using SpeedDate.Configuration;
using SpeedDate.Packets.Spawner;
using SpeedDate.Plugin.Interfaces;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private static Spawner _instance;

    private readonly SpeedDateClient _client = new SpeedDateClient();
    public string MachineIpAddress = IPAddress.Loopback.ToString();
    public string PathToGameServerExecutable = @"C:\gameserver.exe";

    public string MasterIpAddress = IPAddress.Loopback.ToString();
    public int MasterPort = 60125;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _client.Started += () =>
        {
            Debug.Log("Spawner connected to server");

            GetPlugin<SpawnerPlugin>().Register(spawner => { Debug.Log("Spawner registered to server"); }, Debug.Log);
        };
        _client.Start(new DefaultConfigProvider(
            new NetworkConfig(MasterIpAddress, MasterPort),
            PluginsConfig.DefaultSpawnerPlugins, 
            new IConfig[]
            {
                new SpawnerConfig
                {
                    SpawnInBatchmode = true,
                    ExecutablePath = PathToGameServerExecutable,
                    MachineIp = MachineIpAddress
                }
            }));
    }

    public T GetPlugin<T>() where T : class, IPlugin
    {
        return _client.GetPlugin<T>();
    }
}
