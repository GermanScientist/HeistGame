using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks {
    [SerializeField] private List<Room> rooms;
    [SerializeField] private List<GameObject> playerPrefabs;
    private TMP_Text countdownTimer;
    private PhotonView myPhotonView;
    [SerializeField] private float startingTime = 300; //5 minutes
    [SerializeField] private float startWaitDuration = 30; //30 seconds
    private float remainingTime;
    private bool timerIsRunning;
    private bool gameStarted;
    private bool gameCompleted;
    private int objectivesCompleted = 0;

    public List<Room> Rooms { get { return rooms; } }

    public Player currentPlayer;

    private void Awake() {
        remainingTime = startingTime + startWaitDuration;
        myPhotonView = GetComponent<PhotonView>();
        countdownTimer = GameObject.Find("Timer").GetComponent<TMP_Text>();

        GameObject[] roomGOs = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject roomGO in roomGOs) {
            Room currentRoom = roomGO.GetComponent<Room>();
            if (currentRoom != null) {
                rooms.Add(currentRoom);
                if (!currentRoom.HasEntrance) currentRoom.SendOpenDoorRequest();
            }
        }
        
        SpawnPlayers();
    }

    private void Start() {
        timerIsRunning = true;
    }

    private void Update() {
        KeepTrackOfGameTime();
    }

    private void KeepTrackOfGameTime() {

        if (!timerIsRunning) return;

        if (remainingTime > 0) {
            float minutes;
            float seconds;
            CalculateTime(out minutes, out seconds);

            remainingTime -= Time.deltaTime;

            //30 seconds have passed
            if (remainingTime > startingTime - startWaitDuration) {
                minutes = 0;
            } else {
                if(!gameStarted) {
                    OpenEntranceRoomDoors();
                    gameStarted = true;
                }
            }

            if(objectivesCompleted >= 4 && !gameCompleted) {
                FinishGame();
            }

            DisplayTime(countdownTimer, minutes, seconds);
        } else { //The timer has reached 0
            DisplayTime(countdownTimer, 0, 0);
            timerIsRunning = false;
            FinishGame();
        }
    }

    public void CalculateTime(out float _minutes, out float _seconds) {
        _minutes = Mathf.FloorToInt(remainingTime / 60);
        _seconds = Mathf.FloorToInt(remainingTime % 60);
    }

    public void DisplayTime(TMP_Text _text, float _minutes, float _seconds) {
        _text.text = string.Format("{0:00}:{1:00}", _minutes, _seconds);
    }

    private void FinishGame() {
        gameCompleted = true;

        if(objectivesCompleted >= 4) {
            Debug.Log("Intruders have won");
        } else {
            Debug.Log("Guard has won");
        }
        
        LeaveRoom();
    }

    private void SpawnPlayers() {
        if (playerPrefabs.Count < 2 || playerPrefabs == null || photonView == null) return;

        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.Instantiate(this.playerPrefabs[0].name, new Vector3(Random.Range(30, 45), 2f, Random.Range(-45, 45)), Quaternion.identity, 0);
        } else { //Otherwise spawn a guard
            PhotonNetwork.Instantiate(this.playerPrefabs[1].name, new Vector3(Random.Range(30, 45), 2f, Random.Range(-45, 45)), Quaternion.identity, 0);
        }
    }

    private void OpenEntranceRoomDoors() {
        foreach (Room room in rooms) {
            if (room.HasEntrance) room.SendOpenDoorRequest();
            return;
        }
    }
    
    [PunRPC]
    public void CompleteObjective() {
        objectivesCompleted++; 
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene("Launcher");
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }
}
