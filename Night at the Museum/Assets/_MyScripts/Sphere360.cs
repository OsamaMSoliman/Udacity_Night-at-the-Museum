using UnityEngine;
using UnityEngine.Video;

public class Sphere360 : MonoBehaviour {

    [SerializeField] private Transform returnWaypoint;
    private VideoPlayer videoPlayer;

    private void Start() {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.targetTexture = GameManager.GetNewRenderTexture(videoPlayer.targetTexture, gameObject);
        videoPlayer.SetTargetAudioSource(0, GameManager.Instance.AudioManager.audioSource);
    }

    public void Enter360Sphere() {
        GameManager.Instance.FadingManager.MidFading += () => { MovePlayer(true); };
        GameManager.Instance.FadingManager.EndFading += Play360Video;
        GameManager.Instance.FadingManager.FadeInAndOut();
        GameManager.Instance.ChangeRayLength(false);
    }


    private void MovePlayer(bool inside) {
        GameManager.Instance.MovePlayerTo(inside ? transform.position : returnWaypoint.position);
    }

    private void Play360Video() {
        videoPlayer.Play(); 
        videoPlayer.loopPointReached += (vp) => { Exit360Sphere(); };
    }

    public void Exit360Sphere() {
        if (videoPlayer.isPlaying) videoPlayer.Stop();
        GameManager.Instance.FadingManager.MidFading += () => { MovePlayer(false); };
        GameManager.Instance.FadingManager.FadeInAndOut();
        GameManager.Instance.ChangeRayLength(true);
    }

#if UNITY_EDITOR
    void Awake() {
        if (Application.isEditor)
            Application.runInBackground = true;
    }
#endif
}
