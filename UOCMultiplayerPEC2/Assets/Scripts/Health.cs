using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class Health : NetworkBehaviour {

    public bool destroyOnDeath;

    public const int maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    [SyncVar(hook = "OnChangeNickname")]
    public String nickname;

    public RectTransform healthBar;

    public Text textNickname;

    private NetworkStartPosition[] spawnPoints;

    void Start() {
        if (isLocalPlayer) {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
    }

    void Update() {
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

    private void OnChangeNickname(String nickname) {
        if (textNickname != null) {
            textNickname.text = nickname;
        }
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
}