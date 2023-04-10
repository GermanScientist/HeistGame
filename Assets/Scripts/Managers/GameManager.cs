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
    [SerializeField] private float startingTime = 330; //5 minutes and 30 seconds
    private float remainingTime;
    private bool timerIsRunning;
    private bool gameStarted;
    private int objectivesCompleted = 0;

    public List<Room> Rooms { get { return rooms; } }

    public Player currentPlayer;

    private void Awake() {
        remainingTime = startingTime;
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
        KeepTrackOfTime();
    }

    private void KeepTrackOfTime() {

        if (!timerIsRunning) return;

        if (remainingTime > 0) {
            float minutes = Mathf.FloorToInt(remainingTime / 60);
            float seconds = Mathf.FloorToInt(remainingTime % 60);

            remainingTime -= Time.deltaTime;

            //30 seconds have passed
            if (remainingTime > startingTime - 30) {
                minutes = 0;
            } else {
                if(!gameStarted) {
                    foreach (Room room in rooms) {
                        if(room.HasEntrance) room.SendOpenDoorRequest();
                        break;
                    }
                    
                    gameStarted = true;
                }
            }

            if(objectivesCompleted >= 4) {
                Debug.Log("Intruders win");
                LeaveRoom();
            }
            
            countdownTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        } else { //The timer has reached 0
            countdownTimer.text = string.Format("{0:00}:{1:00}", 0, 0);
            Debug.Log("Timer ran out");
            Debug.Log("Guard wins");
            LeaveRoom();
        }
        
    }

    private void SpawnPlayers() {
        if (playerPrefabs.Count < 2 || playerPrefabs == null || photonView == null) return;

        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.Instantiate(this.playerPrefabs[0].name, new Vector3(Random.Range(30, 45), 2f, Random.Range(-45, 45)), Quaternion.identity, 0);
        } else { //Otherwise spawn a guard
            PhotonNetwork.Instantiate(this.playerPrefabs[1].name, new Vector3(Random.Range(30, 45), 2f, Random.Range(-45, 45)), Quaternion.identity, 0);
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
