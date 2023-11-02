using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class LookingAtPlayer : MonoBehaviour
{
    private Animator animator;
    public Transform objectToLookAt;
    public bool ikActive = false;
    public float lookWeight;
    GameObject objPivot;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        objPivot = new GameObject("DummyPivot");
        objPivot.transform.parent = transform;
        objPivot.transform.localPosition = new Vector3(0, 1.41f, 0);
        //lookWeight = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Assuming objPivot is the GameObject that needs to look at objectToLookAt
        Transform pivotTransform = objPivot.transform;

        // Calculate the direction vector from objPivot to objectToLookAt
        Vector3 lookDirection = objectToLookAt.transform.position - pivotTransform.position;

        // Calculate the angle in degrees (Y rotation) between the forward vector of the pivot and the lookDirection
        float pivotRotY = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;

        // Ensure the angle is between 0 and 360 degrees
        pivotRotY = (pivotRotY + 360f) % 360f;

        float dist = Vector3.Distance(pivotTransform.position, objectToLookAt.position);

        if ((pivotRotY < 90.0f && pivotRotY > 0.0f && dist < 3.0f) || (pivotRotY < 360.0f && pivotRotY > 270.0f && dist < 3.0f))
        {
            lookWeight = Mathf.Lerp(lookWeight, 1, Time.deltaTime * 2.5f);
        }
        else
        {
            lookWeight = Mathf.Lerp(lookWeight, 0, Time.deltaTime * 2.5f);
        }
    }

    public void OnAnimatorIK()
    {
        if(animator)
        {
            if(ikActive)
            {
                if(objectToLookAt != null)
                {
                        animator.SetLookAtPosition(objectToLookAt.position);
                        animator.SetLookAtWeight(lookWeight);
                }
            }
            else
            {
                animator.SetLookAtWeight(0);
            }
        }
    }
}
