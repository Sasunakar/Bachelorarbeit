using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CartoonPlayer : MonoBehaviour
{
    public GameObject cam;
    public float triggerDist;
    public float triggerHalfAngle;
    public float lookAtHalfAngle;
    public float headVerticalTargetOffset;
    public float eyeTargetBackOffset;
    public float eyeAngleCorrectAlpha;
    public float smileCounter = 0.0f;
    public GameOverScreen GameOverScreen;
    public float totalTime;
    CartoonAgents[] a;
    // Start is called before the first frame update
    void Start()
    {
        a = (CartoonAgents[])GameObject.FindObjectsOfType(typeof(CartoonAgents));
    }

    // Update is called once per frame
    void LateUpdate()
    {
        totalTime += Time.deltaTime;
        Vector3 cp = cam.transform.position;
        foreach(CartoonAgents aa in a) 
        {
            float xDif = aa.transform.position.x - cp.x;
            float zDif = aa.transform.position.z - cp.z;
            Vector2 camDelta = new Vector2(xDif, zDif);
            float dist = camDelta.magnitude;

            // in distance?
            if(dist < triggerDist ) {
                Vector3 forward3 = aa.transform.TransformVector(new Vector3(0,0,1));
                forward3.Normalize();
                Vector2 fw = new Vector2(forward3.x, forward3.z);
                camDelta.Normalize();
                float dirDot = Vector2.Dot(fw, -camDelta);
                float angle = Mathf.Acos(dirDot) * 180.0f / Mathf.PI;

                // in front of?
                if(angle <= triggerHalfAngle) {
                    Vector3 aap = aa.focusBone.transform.position;
                    Vector3 camForward = cam.transform.TransformVector(new Vector3(0,0,1));
                    camForward.Normalize();
                    Vector3 targetDir = aap - cam.transform.position;
                    targetDir.Normalize();
                    

                    float lookAtAngleRad = Mathf.Acos(Vector3.Dot(camForward, targetDir));
                    float lookAtAngle = lookAtAngleRad * 180.0f / Mathf.PI;
                    
                    
                    // looking at?
                    if(lookAtAngle <= lookAtHalfAngle) {
                        // unique timer for every agent to check for smile
                        aa.timer += Time.deltaTime;

                        if (aa.timer > aa.waitingTime)
                        {
                            // did agent already smile?
                            if (aa.smilingCounter == 0)
                            {
                                aa.smileTrigger();
                                aa.smilingCounter += 1;
                                smileCounter += 1;
                            }
                            
                            //check if every agent smile
                            if(smileCounter == 10)
                            {
                                GameOverScreen.Setup(totalTime);
                            }

                            // Remove the recorded seconds.
                            aa.timer = 0.0f;
                            
                        }
                    }
                }
            }
        }
    }
}
