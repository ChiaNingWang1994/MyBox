using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class baseAnimatorContral : MonoBehaviour {

    private Animator baseAnimator;

	// Use this for initialization
	void Start () {
        baseAnimator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");//左右
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");//前后
        //print(horizontal);
        //print(vertical);

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            baseAnimator.SetTrigger("jump");
        }
        if (Input.GetKeyDown("e") || Input.GetKeyDown("q"))
        {
            baseAnimator.SetTrigger("digging");
        }
        if (vertical > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                baseAnimator.SetBool("run", true);
            }
            else
            {
                baseAnimator.SetBool("run", false);
                baseAnimator.SetBool("walkforward", true);
            }
        }
        else { baseAnimator.SetBool("walkforward", false); }
        if (vertical < 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                baseAnimator.SetBool("run", true);
            }
            else
            {
                baseAnimator.SetBool("run", false);
                baseAnimator.SetBool("walkback", true);
            }
        }
        else { baseAnimator.SetBool("walkback", false); }
        if (horizontal > 0)
        {
            baseAnimator.SetBool("walkright", true);
        }
        else { baseAnimator.SetBool("walkright", false); }
        if (horizontal < 0)
        {
            baseAnimator.SetBool("walkleft", true);
        }
        else { baseAnimator.SetBool("walkleft", false); }
        
        
        
        
        


    }
}
