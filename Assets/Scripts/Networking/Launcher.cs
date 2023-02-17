using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks {
    [Header("Multiplayer lobby settings")]
    [SerializeField] private string gameVersion = "1";

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

        if (PhotonNetwork.IsConnected) SceneManager.LoadScene("LobbyNavigator");
        else {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public void Exit() {
        Application.Quit();
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
        SceneManager.LoadScene("LobbyNavigator");
    }

    public override void OnDisconnected(DisconnectCause cause) {
        ShowProgressionPanel(false);
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
    }

    private void ShowProgressionPanel(bool _showProgression) {
        if (progressPanel == null || controlPanel == null) return;
        progressPanel.SetActive(_showProgression);
        controlPanel.SetActive(!_showProgression);
    }
}
