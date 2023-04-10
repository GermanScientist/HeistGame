using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class SecuritySystem : Objective {
    private TMP_Text timerText;
    [SerializeField] private float objectiveDuration = 30;
    private float remainingTime;

    public override void InitializeRoomMission(Room _room) {
        base.InitializeRoomMission(_room);
        currentRoom.ShowCenterPiece(true);

        timerText = GameObject.Find("ObjectiveTimer").GetComponent<TMP_Text>();
        timerText.gameObject.SetActive(false);
        remainingTime = objectiveDuration;
    }

    public override void ActivateRoomMission(Intruder intruder) {
        base.ActivateRoomMission(intruder);
        timerText.gameObject.SetActive(true);
    }

    public override void UpdateRoomMission(Intruder _intruder) {
        base.UpdateRoomMission(_intruder);

        if(Input.GetKeyDown(KeyCode.E) && !completed && !started) {
            started = true;
        }

        if(started && !completed) {
            KeepTrackOfObjective();
        }
    }

    public override void EndRoomMission(Intruder intruder) {
        base.EndRoomMission(intruder);

        remainingTime = objectiveDuration;
        started = false;
    }

    private void KeepTrackOfObjective() {
        if(remainingTime > 0) {
            float minutes;
            float seconds;
            gameManager.CalculateTime(out minutes, out seconds);
            gameManager.DisplayTime(timerText, minutes, seconds);

            remainingTime -= Time.deltaTime;
        } else {
            CompleteObjective();
        }
    }

    private void CompleteObjective() {
        timerText.gameObject.SetActive(false);
        completed = true;
        gameManager.photonView.RPC("CompleteObjective", RpcTarget.All);
    }
}
