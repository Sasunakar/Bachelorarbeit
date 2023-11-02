using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;


public class Agents : MonoBehaviour
{
    public GameObject focusBone;
    public GameObject ikTarget;
    public GameObject lEyeBone;
    public GameObject rEyeBone;
    float targetAngle;
    float curAngle;
    public float angleLerpAlpha = 0.5f;
    Animator animator;
    public ParticleSystem ps;
    AudioSource m_audio;
    public float timer = 0.0f;
    public float waitingTime;
    public float sneezeTimer = 0.0f;
    public float sneezeIntervall;
    public string sceneName;
    public float smilingCounter = 0.0f;

    // Start is called before the first frame update
    void Start()
    {   
        Scene currentScene = SceneManager.GetActiveScene ();
        sceneName = currentScene.name;
        curAngle = 0.0f;

        m_audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        curAngle = Mathf.Lerp(curAngle, targetAngle, angleLerpAlpha * Time.deltaTime);
        // y and z are -90° at default orientation (mesh bone orientation is messed up)
        ikTarget.transform.eulerAngles = new Vector3(0, -90 + curAngle, -90);

        if (sceneName == "VRBusstopDeceased")
        {
            if (gameObject.name == "Zina" || gameObject.name == "Lukas" || gameObject.name == "Sarah")
            {
                sneezeTimer += Time.deltaTime;
                if(sneezeTimer > sneezeIntervall)
                {
                sneezingTrigger();
                sneezeTimer = 0.0f;

                }
            }
        }
        

    }
    (Vector3 up, Vector3 right) MakeAxisFromForward(Vector3 forward) 
    {
        Vector3 pUp = new Vector3(0, 1, 0);
        Vector3 pRight = Vector3.Cross(forward, pUp);
        pRight.Normalize();
        // generate perpedicular up-vector (compared to forward)
        pUp = Vector3.Cross(pRight, forward);
        pUp.Normalize();
        return (pUp, pRight);
    }
    public void LookAtPoint(Vector3 p, Vector3 pBelow, float angleCorrectAlpha)
    {
        Vector3 forwardHead = pBelow - focusBone.transform.position;
        Vector2 fh2D = new Vector2(forwardHead.x, forwardHead.z); // we only want look at rot around y (easier)
        fh2D.Normalize();
        forwardHead = new Vector3(fh2D.x, 0, fh2D.y);
        
        float rotAngleDot = Vector3.Dot(forwardHead, transform.TransformVector(new Vector3(0, 0, 1)));
        float rotAngleDotRight = Vector3.Dot(forwardHead, transform.TransformVector(new Vector3(1, 0, 0)));
        float rotAngle = Mathf.Acos(rotAngleDot) * (rotAngleDotRight > 0 ? 1.0f : -1.0f);
        
        rotAngle *= 180.0f / Mathf.PI;
        targetAngle = rotAngle;
        
        Vector3 forwardEyeL = p - lEyeBone.transform.position;
        Vector3 forwardEyeR = p - rEyeBone.transform.position;
        forwardEyeL.Normalize();
        forwardEyeR.Normalize();
        
        var vecsEyeL = MakeAxisFromForward(forwardEyeL);
        var vecsEyeR = MakeAxisFromForward(forwardEyeR);
        Quaternion rotEyeL = Quaternion.LookRotation(-vecsEyeL.right, vecsEyeL.up);
        lEyeBone.transform.rotation = rotEyeL;
        Quaternion rotEyeR = Quaternion.LookRotation(-vecsEyeR.right, vecsEyeR.up);
        rEyeBone.transform.rotation = rotEyeR;
    }
    public void sneezingTrigger()
    {
        animator.SetTrigger("sneezing");
        ps.Play();
        m_audio.Play();
    }
    public void smileTrigger()
    {
        animator.SetTrigger("smile");
    }  
}
 