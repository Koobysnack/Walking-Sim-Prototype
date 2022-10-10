using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // makes SoundManager a singleton
    // static ensure this is the only copy of this class
    public static SoundManager instance { get; private set; }
    private AudioSource source;

    private void Awake() {
        source = GetComponent<AudioSource>();

        // if no duplicates
        if(instance == null) {
            instance = this;        // makes instance a singleton
            DontDestroyOnLoad(gameObject);  // prevents SoundManager from being destroyed when new level loaded
        }
        // if duplicates
        else if(instance != null && instance != this) {
            Destroy(gameObject);    // get rid of duplicate
        }
    }

    public void playSound(AudioClip clip) {
        // plays a clip only once
        source.clip = clip;
        source.PlayOneShot(clip);
        StartCoroutine("setNotPlaying", clip.length);
    }

    public bool isPlayingSound(AudioClip clip) {
        return source.isPlaying && source.clip == clip;
    }

    private IEnumerator setNotPlaying(float clipLength) {
        yield return new WaitForSeconds(clipLength);
        source.clip = null;
    }
}
