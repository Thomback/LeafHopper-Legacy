using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera mainCamera;
    [SerializeField]
    private Transform cameraFollow;
    [SerializeField]
    private float lerpSpeed = .5f;

    private Vector3 finish;

    private bool isLerping;


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


    public void CameraLerping(Vector3 finalFollow, Transform finalLookAt)
    {
        isLerping = true;
        finish = finalFollow;
        mainCamera.LookAt = finalLookAt;
    }
}
