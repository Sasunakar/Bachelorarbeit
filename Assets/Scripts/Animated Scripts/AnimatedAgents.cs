using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public class AnimatedAgents : MonoBehaviour
{
    public GameObject focusBone;
    public GameObject ikTarget;
    float targetAngle;
    float curAngle;
    public float angleLerpAlpha = 0.5f;
    Animator animator;
    public float timer = 0.0f;
    public float waitingTime;
    public float smilingCounter = 0.0f;

    // Start is called before the first frame update
    void Start()
    {   
        curAngle = 0.0f;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        curAngle = Mathf.Lerp(curAngle, targetAngle, angleLerpAlpha * Time.deltaTime);
        // y and z are -90° at default orientation (mesh bone orientation is messed up)
        ikTarget.transform.eulerAngles = new Vector3(0, -90 + curAngle, -90);
    }
    
    public void smileTrigger()
    {
        animator.SetTrigger("smile");
    }

    public void acknowledgeTrigger()
    {
        animator.SetTrigger("acknowledge");
    }
}
 