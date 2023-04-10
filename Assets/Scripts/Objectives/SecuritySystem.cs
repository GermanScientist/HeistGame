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

        GameObject objectiveTimerGO = GameObject.Find("ObjectiveTimer");
        timerText = objectiveTimerGO != null ? objectiveTimerGO.GetComponent<TMP_Text>() : null;
        SetTextActive(timerText, false);
        remainingTime = objectiveDuration;
    }

    public override void ActivateRoomMission(Intruder intruder) {
        base.ActivateRoomMission(intruder);
        SetTextActive(timerText, true);
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
        SetTextActive(timerText, false);
        completed = true;
        gameManager.photonView.RPC("CompleteObjective", RpcTarget.All);
    }

    private void SetTextActive(TMP_Text _text, bool _state) {
        if (_text == null) return;
        _text.gameObject.SetActive(_state);
    }
}
