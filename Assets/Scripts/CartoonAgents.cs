using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class CartoonAgents : MonoBehaviour
{
    public GameObject focusBone;       // Reference to the focus bone of the character.
    public GameObject ikTarget;        // Reference to the inverse kinematics (IK) target.
    float targetAngle;                // The target angle for character rotation.
    float curAngle;                   // The current character rotation angle.
    public float angleLerpAlpha = 0.5f;  // A parameter for lerping character rotation.
    public float timer = 0.0f;        // A timer for triggering actions at specific intervals.
    public float waitingTime;          // Time to wait between actions.
    public float smilingCounter = 0.0f;  // A counter for smiling actions.
    private SkinnedMeshRenderer skinnedMeshRenderer; // Reference to the skinnedMeshRenderer from the object.
    private int smileBlendShapeIndex = 0; // The index of the "Smile" BlendShape
    private Transform childTransform; // Get child of object.


    // Start is called before the first frame update
    void Start()
    {   
        curAngle = 0.0f;                                      // Initialize the character's rotation angle.
    }

    // Update is called once per frame
    void Update()
    {
        curAngle = Mathf.Lerp(curAngle, targetAngle, angleLerpAlpha * Time.deltaTime);  // Smoothly interpolate character rotation.
        ikTarget.transform.eulerAngles = new Vector3(0, -90 + curAngle, -90);  // Update IK target rotation.
    }

    // Helper function to create an axis (up and right) from a forward vector.
    (Vector3 up, Vector3 right) MakeAxisFromForward(Vector3 forward) 
    {
        // Calculate perpendicular up and right vectors.
        Vector3 pUp = new Vector3(0, 1, 0);
        Vector3 pRight = Vector3.Cross(forward, pUp);
        pRight.Normalize();
        pUp = Vector3.Cross(pRight, forward);
        pUp.Normalize();
        return (pUp, pRight);
    }

    // Function to make the character look at a point with correction.
    public void LookAtPoint(Vector3 p, Vector3 pBelow, float angleCorrectAlpha)
    {
        // Calculate the character's forward direction.
        Vector3 forwardHead = pBelow - focusBone.transform.position;
        Vector2 fh2D = new Vector2(forwardHead.x, forwardHead.z);
        fh2D.Normalize();
        forwardHead = new Vector3(fh2D.x, 0, fh2D.y);
        
        // Calculate the character's rotation angle to look at the specified point.
        float rotAngleDot = Vector3.Dot(forwardHead, transform.TransformVector(new Vector3(0, 0, 1)));
        float rotAngleDotRight = Vector3.Dot(forwardHead, transform.TransformVector(new Vector3(1, 0, 0)));
        float rotAngle = Mathf.Acos(rotAngleDot) * (rotAngleDotRight > 0 ? 1.0f : -1.0f);
        rotAngle *= 180.0f / Mathf.PI;
        targetAngle = rotAngle;
    }

    // Function to trigger a smile action (not currently used in the code).
    public void smileTrigger()
    {
        childTransform = transform.Find("ShapeKeysTest"); // Get child 
        skinnedMeshRenderer = childTransform.GetComponent<SkinnedMeshRenderer>();
        skinnedMeshRenderer.SetBlendShapeWeight(smileBlendShapeIndex, 100);

        Debug.Log("Smile triggered");
    }
}