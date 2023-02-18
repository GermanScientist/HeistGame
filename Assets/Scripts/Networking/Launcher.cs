using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks {
    [Header("Multiplayer lobby settings")]
    [SerializeField] private string gameVersion = "1";

    [Header("Panels")]
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject progressPanel;

    [Header("Buttons")]
    [SerializeField] private Button connectButton;

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

    private void DisconnectButton(Button _button) {
        if (_button != null) connectButton.interactable = false;
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
        DisconnectButton(connectButton);
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
    }

    private void ShowProgressionPanel(bool _showProgression) {
        if (progressPanel == null || controlPanel == null) return;
        progressPanel.SetActive(_showProgression);
        controlPanel.SetActive(!_showProgression);
    }
}
