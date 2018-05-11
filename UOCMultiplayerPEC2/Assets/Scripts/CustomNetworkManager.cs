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
            ++numPlayer;
            Health health = p.gameObject.GetComponent<Health>();
            TankController tankController = p.gameObject.GetComponent<TankController>();
            string nickname = "Player_" + numPlayer;
            Color playerColor = numPlayer - 1 < playerColors.Length ? playerColors[numPlayer - 1] : playerColors[playerColors.Length - 1];

            health.SetNickname(nickname);               // set the server name
            health.RpcSetNickname(nickname);            // set the client name
            tankController.SetColor(playerColor);       // set the server color
            tankController.RpcSetColor(playerColor);    // set the client color
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
