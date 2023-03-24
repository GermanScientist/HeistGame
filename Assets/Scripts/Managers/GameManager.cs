using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks {
    private GameObject playerPrefab;
    [SerializeField] private List<Room> rooms;

    public List<Room> Rooms { get { return rooms; } }

    public Player currentPlayer;

    private void Awake() {
        playerPrefab = Resources.Load<GameObject>("Player");
        
        GameObject[] roomGOs = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject roomGO in roomGOs) {
            Room currentRoom = roomGO.GetComponent<Room>();
            if (currentRoom != null) rooms.Add(currentRoom);
        }
    }

    private void Start() {
        if(playerPrefab != null)
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(Random.Range(30, 45), 2f, Random.Range(-45, 45)), Quaternion.identity, 0);
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene("Launcher");
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }
}
