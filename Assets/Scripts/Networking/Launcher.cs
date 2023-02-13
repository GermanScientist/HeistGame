using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks {
    [Header("Multiplayer lobby settings")]
    [SerializeField] private string gameVersion = "1";
    [SerializeField] private byte maxPlayersPerRoom = 7;

    [Header("Panels")]
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject progressPanel;

    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true; //Sync all scenes when the master client changes scenes.
    }

    private void Start() {
        ShowProgressionPanel(false);
    }

    public void Connect() {
        ShowProgressionPanel(true);

        if (PhotonNetwork.IsConnected) PhotonNetwork.JoinRandomRoom();
        else {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public override void OnConnectedToMaster() {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause) {
        ShowProgressionPanel(false);
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\n Calling: PhotonNetwork.CreateRoom");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom() {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
    }

    private void ShowProgressionPanel(bool _showProgression) {
        if (progressPanel == null || controlPanel == null) return;
        progressPanel.SetActive(_showProgression);
        controlPanel.SetActive(!_showProgression);
    }
}
