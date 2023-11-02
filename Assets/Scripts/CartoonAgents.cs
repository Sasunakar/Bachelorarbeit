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
    private float transitionDuration = 1.0f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {   
        //curAngle = 0.0f;                                      // Initialize the character's rotation angle.
        animator = GetComponent<Animator>();

        // Set faces to neutral at the start
        childTransform = transform.Find("CartoonCharacter"); // Get child 
        skinnedMeshRenderer = childTransform.GetComponent<SkinnedMeshRenderer>();
        skinnedMeshRenderer.SetBlendShapeWeight(smileBlendShapeIndex, 0);
    }

    // Update is called once per frame
    void Update()
    {
        curAngle = Mathf.Lerp(curAngle, targetAngle, angleLerpAlpha * Time.deltaTime);  // Smoothly interpolate character rotation.
        ikTarget.transform.eulerAngles = new Vector3(0, -90 + curAngle, -90);  // Update IK target rotation.
    }

    public void smileTrigger()
    {
        childTransform = transform.Find("CartoonCharacter"); // Get child 
        skinnedMeshRenderer = childTransform.GetComponent<SkinnedMeshRenderer>();

        // Start the coroutine to gradually set the blend shape weight to 100
        StartCoroutine(ChangeBlendShapeOverTime(smileBlendShapeIndex, 100, transitionDuration));
    }

    private IEnumerator ChangeBlendShapeOverTime(int blendShapeIndex, float targetWeight, float duration)
    {
        float startTime = Time.time;
        float startWeight = skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex);

        while (Time.time - startTime < duration)
        {
            float elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // Calculate the interpolation factor
            
            float newWeight = Mathf.Lerp(startWeight, targetWeight, t);
            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, newWeight);
            
            yield return null;
        }

        // Ensure the final blend shape weight is set to the target value
        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, targetWeight);
    }

}