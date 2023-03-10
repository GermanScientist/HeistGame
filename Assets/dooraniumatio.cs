using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class dooraniumatio : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        animator.ResetTrigger("CloseDoor");
        animator.SetTrigger("OpenDoor");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            animator.ResetTrigger("CloseDoor");
            animator.SetTrigger("OpenDoor");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            animator.ResetTrigger("OpenDoor");
            animator.SetTrigger("CloseDoor");
        }
    }
}
