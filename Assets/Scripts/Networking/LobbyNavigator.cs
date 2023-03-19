using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LobbyNavigator : MonoBehaviourPunCallbacks {
    [Header("Multiplayer lobby settings")]
    [SerializeField] private byte maxPlayersPerRoom = 7;
    [SerializeField] private byte maxRooms = 2;

    [Header("Panels")]
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject progressPanel;
    [SerializeField] private GameObject[] lobbyPanels;

    private List<RoomInfo> roomList;

    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true; //Sync all scenes when the master client changes scenes.
    }

    private void Start() {
        ShowProgressionPanel(false);
    }

    private void Update() {
        UpdateRoomList();
    }

    private void UpdateRoomList() {
        foreach (GameObject lobbyPanel in lobbyPanels) lobbyPanel.SetActive(false);

        int roomCount = PhotonNetwork.CountOfRooms;
        for (int i = 0; i < roomCount; i++) {
            if (i > maxRooms) continue;
            SetPanelActive(lobbyPanels[i], true);
        }
    }

    public void CreateRoom() {
        int roomCount = PhotonNetwork.CountOfRooms;
        if (roomCount >= maxRooms) return;

        int lobbyNumber = roomCount + 1;
        PhotonNetwork.CreateRoom("Lobby " + lobbyNumber, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public void Return() {
        SceneManager.LoadScene("Launcher");
    }

    public void JoinRoom(int _roomNumber) {
        Debug.Log("Room joining: Lobby " + _roomNumber);
        PhotonNetwork.JoinRoom("Lobby " + _roomNumber);
    }

    public override void OnJoinedRoom() {
        ShowProgressionPanel(true);
        SceneManager.LoadScene("Lobby");
    }

    private void ShowProgressionPanel(bool _showProgression) {
        SetPanelActive(progressPanel, _showProgression);
        SetPanelActive(controlPanel, !_showProgression);
    }

    private void SetPanelActive(GameObject _panel, bool _state) {
        if (_panel == null) return;
        _panel.SetActive(_state);
    }
}
