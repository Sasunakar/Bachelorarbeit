using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;


public class Agents : MonoBehaviour
{
    public GameObject focusBone;
    //public GameObject headBone;
    public GameObject ikTarget;
    public GameObject lEyeBone;
    public GameObject rEyeBone;
    float targetAngle;
    float curAngle;
    public float angleLerpAlpha = 0.5f;
    Animator animator;
    //public PlayableDirector playableDirector;
    //public List <TimelineAsset> timelines;
    //float duration = 5;
    //public float timer = 0;
    public ParticleSystem ps;
    AudioSource m_audio;
    public float timer = 0.0f;
    public float waitingTime;
    public float sneezeTimer = 0.0f;
    public float sneezeIntervall;
    public string sceneName;
    public float smilingCounter = 0.0f;
    //public float sneezingIntervall;

    // Start is called before the first frame update
    void Start()
    {   
        Scene currentScene = SceneManager.GetActiveScene ();
        sceneName = currentScene.name;
        curAngle = 0.0f;
        //ps = GameObject.Find(name+"PS").GetComponent<ParticleSystem>();
        m_audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        //playableDirector = GetComponent<PlayableDirector>();
        //PlayFromTimelines(0);
        //InvokeRepeating("PlayFromTimelines(1)", 7.0f, 9.0f);
        //InvokeRepeating(PlayFromTimelines(0), 9.0f, 9.0f)
        //if(gameObject.name == "Zina" || gameObject.name == "Lukas" || gameObject.name == "Fiona")
        //{
        //    InvokeRepeating("sneezingTrigger", 8.0f, 16.0f);
        //}
        //InvokeRepeating("sneezingTrigger", 8.0f, 16.0f);
        //InvokeRepeating("sneezingTriggerFalse", 8.0f, 8.0f);
    }
    // Update is called once per frame
    void Update()
    {
        curAngle = Mathf.Lerp(curAngle, targetAngle, angleLerpAlpha * Time.deltaTime);
        // y and z are -90° at default orientation (mesh bone orientation is messed up)
        ikTarget.transform.eulerAngles = new Vector3(0, -90 + curAngle, -90);
        //timer += Time.deltaTime;
        //if(timer > duration){
        //    PlayFromTimelines(1);
        //    timer = 0;
        //}

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
        //var vecsHead = MakeAxisFromForward(forwardHead);
        //Quaternion rotHead = Quaternion.LookRotation(forwardHead, Vector3.up);
        //ikTarget.transform.rotation = rotHead;
        // dot "forwardHead" with agent forward vector (direction body is orientated in)
        float rotAngleDot = Vector3.Dot(forwardHead, transform.TransformVector(new Vector3(0, 0, 1)));
        float rotAngleDotRight = Vector3.Dot(forwardHead, transform.TransformVector(new Vector3(1, 0, 0)));
        float rotAngle = Mathf.Acos(rotAngleDot) * (rotAngleDotRight > 0 ? 1.0f : -1.0f);
        //Debug.Log(rotAngleDot);
        rotAngle *= 180.0f / Mathf.PI;
        targetAngle = rotAngle;
        ////Quaternion rot = Quaternion.LookRotation(forward, up);
        //Quaternion rotHead = Quaternion.LookRotation(vecsHead.right, forwardHead);
        //headBone.transform.rotation = rotHead;

        //Vector3 eyeCenter = (lEyeBone.transform.position + rEyeBone.transform.position) * 0.5f;
        //Vector3 eyeCenterDir = p - eyeCenter;
        //eyeCenterDir.Normalize();
        //p += eyeCenterDir * eyeTargetBackOffset;
        Vector3 forwardEyeL = p - lEyeBone.transform.position;
        Vector3 forwardEyeR = p - rEyeBone.transform.position;
        forwardEyeL.Normalize();
        forwardEyeR.Normalize();
        //forwardEyeL = Vector3.Lerp(forwardEyeL, forwardHead, angleCorrectAlpha);
        //forwardEyeR = Vector3.Lerp(forwardEyeR, forwardHead, angleCorrectAlpha);
        var vecsEyeL = MakeAxisFromForward(forwardEyeL);
        var vecsEyeR = MakeAxisFromForward(forwardEyeR);
        Quaternion rotEyeL = Quaternion.LookRotation(-vecsEyeL.right, vecsEyeL.up);
        lEyeBone.transform.rotation = rotEyeL;
        Quaternion rotEyeR = Quaternion.LookRotation(-vecsEyeR.right, vecsEyeR.up);
        rEyeBone.transform.rotation = rotEyeR;
        // Debug.Log("Test");
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
    
    //public void PlayFromTimelines(int index)
    //{
    //    TimelineAsset selectedTimeline;
    //    selectedTimeline = timelines[index];
    //    playableDirector.Play(selectedTimeline);
    //}
    
}
 