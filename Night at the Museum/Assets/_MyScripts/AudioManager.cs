using UnityEngine;

public class AudioManager : MonoBehaviour {

    [HideInInspector]public AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    internal void VolumeTuning(bool up) {
        if (up && audioSource.volume < 1) {
            audioSource.volume += 0.1f;
        } else if (!up && audioSource.volume > 0) {
            audioSource.volume -= 0.1f;
        }
    }
}
