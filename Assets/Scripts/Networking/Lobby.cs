using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class Lobby : MonoBehaviourPunCallbacks {
    [Header("Multiplayer lobby settings")]
    [SerializeField] private byte minimumToStart = 4;

    [Header("Panels")]
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject progressPanel;
    [SerializeField] private TextMeshProUGUI amountOfPlayers;

    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true; //Sync all scenes when the master client changes scenes.
    }

    private void Start() {
        ShowProgressionPanel(false);
    }

    private void Update() {
        amountOfPlayers.text = PhotonNetwork.PlayerList.Length.ToString();
    }

    public void StartGame() {
        if (!PhotonNetwork.IsMasterClient) return;
        if (PhotonNetwork.PlayerList.Length < minimumToStart) return;
        ShowProgressionPanel(true);

        PhotonNetwork.LoadLevel("Game");
    }

    public void Leave() {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene("LobbyNavigator");
    }

    private void ShowProgressionPanel(bool _showProgression) {
        if (progressPanel == null || controlPanel == null) return;
        progressPanel.SetActive(_showProgression);
        controlPanel.SetActive(!_showProgression);
    }
}
