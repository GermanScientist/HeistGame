using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks {
    [SerializeField] private List<Room> rooms;
    [SerializeField] private List<GameObject> playerPrefabs;
    [SerializeField] private TMP_Text countdownTimer;
    [SerializeField] private TMP_Text objectiveCounter;
    [SerializeField] private float startingTime = 300; //5 minutes
    [SerializeField] private float startWaitDuration = 30; //30 seconds

    private PhotonView myPhotonView;
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

        OpenAllDoors();
        SpawnPlayers();
    }

    private void Start() {
        FindCountdownTimer();

        timerIsRunning = true;
    }

    private void Update() {
        KeepTrackOfGameTime();
    }

    private void FindCountdownTimer() {
        GameObject timerGO = GameObject.Find("Timer");
        countdownTimer = timerGO != null ? timerGO.GetComponent<TMP_Text>() : null;
    }

    private void KeepTrackOfGameTime() {
        if(countdownTimer == null) FindCountdownTimer();

        if (!timerIsRunning) return;

        if (remainingTime > 0) {
            float minutes;
            float seconds;
            CalculateTime(out minutes, out seconds, remainingTime);

            remainingTime -= Time.deltaTime;

            //30 seconds have passed
            if (remainingTime > startingTime) {
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

    public void CalculateTime(out float _minutes, out float _seconds, float _remainingTime) {
        _minutes = Mathf.FloorToInt(_remainingTime / 60);
        _seconds = Mathf.FloorToInt(_remainingTime % 60);
    }

    public void DisplayTime(TMP_Text _text, float _minutes, float _seconds) {
        _text.text = string.Format("{0:00}:{1:00}", _minutes, _seconds);
    }

    private void FinishGame() {
        gameCompleted = true;

        if (objectivesCompleted >= 4) {
            Debug.Log("Intruders have won");
        } else {
            Debug.Log("Guard has won");
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
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

    private void OpenAllDoors() {
        GameObject[] roomGOs = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject roomGO in roomGOs) {
            Room currentRoom = roomGO.GetComponent<Room>();
            if (currentRoom != null) {
                rooms.Add(currentRoom);
                if (!currentRoom.HasEntrance) currentRoom.SendOpenDoorRequest();
            }
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
        if(objectiveCounter == null) {
            GameObject objectiveTextGO = GameObject.Find("ObjectiveCounterText");
            objectiveCounter = objectiveTextGO != null ? objectiveTextGO.GetComponent<TMP_Text>() : null;
        }

        objectivesCompleted++;
        objectiveCounter.text = objectivesCompleted + "/4";
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene("Launcher");
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }
}
