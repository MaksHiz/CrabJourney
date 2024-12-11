using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Animations; // Required for runtime AnimatorController manipulation
#endif

public class RotateWall : MonoBehaviour
{
    [SerializeField]
    Animator wallAnimator;
    [SerializeField]
    float DesiredRotationZ;
    [SerializeField]
    bool clockWiseRotation = true;
    private float clockTurnDirection = 0;
    private float animationDuration = 1.0f;
    
    private Animation animationComponent;
    private AnimationClip rotateClip;
    private AnimationClip rotateBackClip;

    const string initState = "InitialState";
    const string rotate = "Rotate";
    const string rotateback = "RotateBack";

    private void Start()
    {
        if (clockWiseRotation) { clockTurnDirection = transform.localEulerAngles.z + 360; }
        else { clockTurnDirection = transform.localEulerAngles.z; }
        animationComponent = gameObject.AddComponent<Animation>();
        CreateRotateAnimation();
        CreateRotateBackAnimation();
        animationComponent.AddClip(rotateClip, "Rotate");
        animationComponent.AddClip(rotateBackClip, "RotateBack");
    }
    void CreateRotateAnimation()
    {
        rotateClip = new AnimationClip
        {
            name = "RotateToDesiredAngle",
            legacy = true
        };

        // Define the rotation curve
        AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, clockTurnDirection, animationDuration, DesiredRotationZ);

        // Add rotation property to the clip
        rotateClip.SetCurve("", typeof(Transform), "localEulerAngles.z", rotationCurve);
    }

    void CreateRotateBackAnimation()
    {
        rotateBackClip = new AnimationClip
        {
            name = "RotateBackToInitial",
            legacy=true
        };

        // Define the rotation curve
        AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, DesiredRotationZ, animationDuration, clockTurnDirection);

        // Add rotation property to the clip
        rotateBackClip.SetCurve("", typeof(Transform), "localEulerAngles.z", rotationCurve);
    }
    public void AnimateWall()
    {
        // Check the current state
        AnimatorStateInfo currentState = wallAnimator.GetCurrentAnimatorStateInfo(0);

        // Toggle the `leverOn` parameter in the animator based on the state
        if (currentState.IsName(initState) || currentState.IsName(rotateback))
        {
            // Move to the "Rotate" state
            wallAnimator.SetBool("leverOn", true);
            animationComponent.Play("Rotate");
        }
        else if (currentState.IsName(rotate))
        {
            // Move to the "RotateBack" state
            wallAnimator.SetBool("leverOn", false);
            animationComponent.Play("RotateBack");
        }
    }
}
