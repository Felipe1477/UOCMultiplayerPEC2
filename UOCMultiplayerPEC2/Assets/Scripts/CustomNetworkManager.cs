using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    private Color[] playerColors = new Color[] { Color.blue, Color.green, Color.magenta, Color.white, Color.cyan, Color.red, Color.yellow, Color.grey, Color.black};

    private List<PlayerController> playerPlayerControllerList = new List<PlayerController>();

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        base.OnServerAddPlayer(conn, playerControllerId);
        foreach (PlayerController p in conn.playerControllers) {
            if (p.playerControllerId == playerControllerId) {
                playerPlayerControllerList.Add(p);
            }
        }
        int numPlayer = 0;
        foreach (PlayerController p in playerPlayerControllerList) {
            Health health = p.gameObject.GetComponent<Health>();
            health.nickname = ""; // to reset the name and make it update online, same values seem to not update
            health.nickname = "Player " + ++numPlayer;
            Color playerColor;
            if (numPlayer - 1 < playerColors.Length) {
                playerColor = playerColors[numPlayer - 1];
            } else {
                playerColor = playerColors[playerColors.Length-1];
            }
            TankController tankController = p.gameObject.GetComponent<TankController>();
            tankController.color = new Color(1f, 1f, 1f, 1f);
            tankController.color = playerColor;
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
