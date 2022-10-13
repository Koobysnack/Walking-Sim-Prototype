using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCamController : MonoBehaviour
{
    [Tooltip("How far ahead of the player the camera will look")]
    [SerializeField] private float aheadDistance;
    [Tooltip("How high relative to the player the camera will be")]
    [SerializeField] private float aboveDistance;
    [Tooltip("Orthographic Camera Size. The smaller the value, the more the zoom")]
    [SerializeField] private float cameraSize = 5.0f;

    private CinemachineVirtualCamera cam;
    private CinemachineFramingTransposer transposer;
    private PlayerMovement movement;

    private void Awake() {
        cam = GetComponent<CinemachineVirtualCamera>();
        transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
        movement = cam.m_Follow.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        transposer.m_TrackedObjectOffset = new Vector3(aheadDistance * movement.horizInput, aboveDistance, 
                                                       transposer.m_TrackedObjectOffset.z);
        cam.m_Lens.OrthographicSize = cameraSize;
    }
}
