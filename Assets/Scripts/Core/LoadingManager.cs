using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance { get; private set; }

    private void Awake() {
        // if no duplicates
        if(instance == null) {
            instance = this;        // makes instance a singleton
            DontDestroyOnLoad(gameObject);  // prevents LoadingManager from being destroyed when new level loaded
        }
        // if duplicates
        else if(instance != null && instance != this) {
            Destroy(gameObject);    // get rid of duplicate
        }
    }

    public void switchLevel(int nextLevel) {
        SceneManager.LoadScene(nextLevel);
    }
}
