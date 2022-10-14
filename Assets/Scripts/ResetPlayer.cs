using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
    [Tooltip("Player to reset")]
    [SerializeField] private Transform player;
    [Tooltip("X coordinate to reset the player to")]
    [SerializeField] private float resetX;
    [Tooltip("Y coordinate to reset the player to")]
    [SerializeField] private float resetY;

    private BoxCollider2D box;

    private void Awake() {
        box = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
            player.transform.position = new Vector3(resetX, resetY, player.transform.position.z);
    }
}
