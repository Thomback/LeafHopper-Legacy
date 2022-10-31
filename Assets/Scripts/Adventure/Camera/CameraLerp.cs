using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to attach to gameManager, used to lerp the camera movement between 2 points
/// </summary>
public class CameraLerp : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera mainCamera; // Reference to the main Cinemachine camera
    [SerializeField]
    private Transform cameraFollow;                         // Default camera location 
    [SerializeField]
    private float lerpSpeed = .5f;                          // Speed of camera location lerping

    private Vector3 finish;                                 // Private Vector3 of final location

    private bool isLerping;                                 // Private bool to check if lerping is enabled

    /// <summary>
    /// If missing component, tries to find them on Start
    /// </summary>
    void Start()
    {
        if(mainCamera == null)
        {
            mainCamera = GameObject.Find("CM VirtualCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        }
        if(cameraFollow == null)
        {
            cameraFollow = GameObject.Find("CameraFollow").GetComponent<Transform>();
        }
    }

    /// <summary>
    /// On each frame, if the camera is in lerping mode, update its location to lerp to final location.
    /// When 0.01 unit distance from destination, stop lerping and snap position to final location.
    /// </summary>
    void Update()
    {
        if (isLerping)
        {
            cameraFollow.localPosition = Vector3.Lerp(cameraFollow.localPosition, finish, lerpSpeed * Time.deltaTime);
            if ((cameraFollow.localPosition - finish).magnitude <= 0.01f)
            {
                cameraFollow.localPosition = finish;
                isLerping = false;
            }
        }
    }

    /// <summary>
    /// Public function to call to start lerping and set final location and lookat
    /// </summary>
    /// <param name="finalFollow">Desired final location</param>
    /// <param name="finalLookAt">Desired final lookAt reference</param>
    public void CameraLerping(Vector3 finalFollow, Transform finalLookAt)
    {
        isLerping = true;
        finish = finalFollow;
        mainCamera.LookAt = finalLookAt;
    }
}
