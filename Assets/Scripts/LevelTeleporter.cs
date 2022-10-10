using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTeleporter : MonoBehaviour
{
    [Tooltip("The next level to load. Keep in mind that levels are 0-indexed in the build settings")]
    [SerializeField] private int nextLevel;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
            LoadingManager.instance.switchLevel(nextLevel);
    }
}
