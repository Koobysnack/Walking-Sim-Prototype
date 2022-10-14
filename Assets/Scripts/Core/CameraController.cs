using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("The player to track")]
    [SerializeField] private Transform player;
    [Tooltip("How far ahead of the player the camera will look")]
    [SerializeField] private float aheadDistance;
    [Tooltip("How high relative to the player the camera will be")]
    [SerializeField] private float aboveDistance;
    [Tooltip("How fast the camera will keep up with the player")]
    [SerializeField] private float followCameraSpeed;
    [Tooltip("Orthographic Camera Size. The smaller the value, the more the zoom")]
    [SerializeField] private float cameraSize = 5.0f;

    private Camera cam;
    private float lookAhead;

    private void Awake() {
        cam = GetComponent<Camera>();
    }

    private void Update() {
        // moving camera
        transform.position = new Vector3(player.position.x + lookAhead, player.position.y + aboveDistance, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), followCameraSpeed * Time.deltaTime);

        cam.orthographicSize = cameraSize;      // this isn't super efficient, but for the sake of debugging, it can stay in update
    }
}
