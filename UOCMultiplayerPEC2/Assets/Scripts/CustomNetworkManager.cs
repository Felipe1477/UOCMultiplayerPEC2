using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    private List<PlayerController> playerPlayerControllerList = new List<PlayerController>();

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        base.OnServerAddPlayer(conn, playerControllerId);
        foreach (PlayerController p in conn.playerControllers) {
            if (p.playerControllerId == playerControllerId) {
                playerPlayerControllerList.Add(p);
            }
        }
        int numPlayer = 1;
        foreach (PlayerController p in playerPlayerControllerList) {
            Health health = p.gameObject.GetComponent<Health>();
            health.nickname = ""; // to reset the name and make it update online, same values seem to not update
            health.nickname = "Player " + numPlayer++;
            TankController tankController = p.gameObject.GetComponent<TankController>();
            tankController.color = new Color(1f, 1f, 1f, 1f);
            tankController.color = new Color(0.1f + UnityEngine.Random.value * 0.89f, 0.1f + UnityEngine.Random.value * 0.89f, 0.1f + UnityEngine.Random.value * 0.89f, 1f);
        }
    }

    
    public override void OnServerDisconnect(NetworkConnection conn) {
        base.OnServerDisconnect(conn);
        for (int i= playerPlayerControllerList.Count-1; i>= 0; i--) {
            if (playerPlayerControllerList[i].gameObject == null) {
                playerPlayerControllerList.RemoveAt(i);
            }
        }
    }

}
