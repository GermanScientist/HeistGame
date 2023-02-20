using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks {
    [SerializeField] private GameObject playerPrefab;

    private void Awake() {
        playerPrefab = Resources.Load<GameObject>("Player");
    }

    private void Start() {
        if(playerPrefab != null)
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(Random.Range(50, 100), 2f, Random.Range(100, 180)), Quaternion.identity, 0);
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene("Launcher");
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }
}
