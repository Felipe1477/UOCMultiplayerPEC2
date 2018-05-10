using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyDiscovery : NetworkDiscovery {

    void Awake() {
        Initialize();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data) {
		
        base.OnReceivedBroadcast(fromAddress, data);

        if (!NetworkManager.singleton.IsClientConnected())  {
            NetworkManager.singleton.networkAddress = fromAddress;
            NetworkManager.singleton.StartClient();
        }
    }

}