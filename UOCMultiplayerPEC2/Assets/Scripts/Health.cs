using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Diagnostics;

public class Health : NetworkBehaviour {

    public bool isAI = true;

    public bool destroyOnDeath;

    public const int maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;
    
    public String nickname = "";

    public RectTransform healthBar;

    public Text textNickname;

    private NetworkStartPosition[] spawnPoints;

    private Stopwatch stopwatch;

    void Start() {
        if (isLocalPlayer) {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
        if (!isAI) {
            stopwatch = Stopwatch.StartNew();
            if (isLocalPlayer) {
                NetGOData.SaveData("client_" + nickname, gameObject.transform.position, gameObject.transform.eulerAngles.y, stopwatch.ElapsedMilliseconds);
            } else if (isServer) {
                NetGOData.SaveData("server_" + nickname, gameObject.transform.position, gameObject.transform.eulerAngles.y, stopwatch.ElapsedMilliseconds);
            }
        }
    }

    void Update() {
        if (!isAI) {
            if (isLocalPlayer) {
                NetGOData.SaveData("client_" + nickname, gameObject.transform.position, gameObject.transform.eulerAngles.y, stopwatch.ElapsedMilliseconds);
            } else if (isServer) {
                NetGOData.SaveData("server_" + nickname, gameObject.transform.position, gameObject.transform.eulerAngles.y, stopwatch.ElapsedMilliseconds);
            }
        }
    }

    public void TakeDamage(int amount) {
        if (!isServer) {
            return;
        }

        currentHealth -= amount;
        if (currentHealth <= 0) {
            if (destroyOnDeath) {
                Destroy(gameObject);
            } else {
                currentHealth = maxHealth;

                // called on the Server, but invoked on the Clients
                RpcRespawn();
            }
        }
    }

    private void OnChangeHealth(int currentHealth) {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn() {
        if (isLocalPlayer) {
            // Set the spawn point to origin as a default value
            Vector3 spawnPoint = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick a spawn point at random
            if (spawnPoints != null && spawnPoints.Length > 0) {
                spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform.position;
            }

            // Set the player�s position to the chosen spawn point
            transform.position = spawnPoint;
        }
    }

    [ClientRpc]
    public void RpcSetNickname(string name) {
        SetNickname(name);
    }

    public void SetNickname(string name) {
        nickname = name;
        if (textNickname != null) {
            textNickname.text = nickname;
        }
    }
}