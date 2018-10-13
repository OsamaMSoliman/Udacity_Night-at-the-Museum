using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour {

    [SerializeField] private VideoClip[] videos;
    [SerializeField] private TextAsset[] subtitles;
    [SerializeField] private GameObject pressToPlayScreen;
    [SerializeField] private TextMeshProUGUI videoTitle;
    [SerializeField] private TextMeshProUGUI subtitleText;

    private const string NO_SUB = "<color=#999>sorry, <sprite=15> no subtitles for this video yet!";

    private static List<VideoController> vcs = new List<VideoController>();
    private VideoPlayer videoPlayer;
    private AudioSource targetAudio;

    private List<Subtitle> subtitlesList;

    private int currentSubtitleIndex;
    private int _currentVideoIndex;
    private int currentVideoIndex {
        get { return _currentVideoIndex % videos.Length; }
        set { _currentVideoIndex = value; }
    }
    private bool _isPlaying; // I used this instead of VideoPlayer.isPlaying for more accurate performance
    private bool IsPlaying {
        get { return _isPlaying; }
        set {
            _isPlaying = value;
            pressToPlayScreen.SetActive(!_isPlaying);
            subtitleText.text = string.Empty;
        }
    }

    public void SetUp(AudioSource audioSource) {
        vcs.Add(this);
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.targetTexture.Release();
        videoPlayer.targetTexture = GameManager.GetNewRenderTexture(GetComponent<Renderer>().material);
        targetAudio = audioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.clip = videos[currentVideoIndex];
        subtitlesList = XmlSubtitlesParser.Instance.GetSubtitles(subtitles[currentVideoIndex]);
        if (subtitlesList == null) subtitleText.text = NO_SUB;
    }
    

    public void TogglePlay() {
        if (IsPlaying) videoPlayer.Pause();
        else videoPlayer.Play();
        IsPlaying = !IsPlaying;
        videoTitle.text = videoPlayer.clip.name;
        PauseAllOtherVideoPlayers(this);
    }

    public void PlayNext() {
        IsPlaying = true;
        currentSubtitleIndex = 0;
        videoPlayer.Stop();
        currentVideoIndex++;
        videoPlayer.clip = videos[currentVideoIndex];
        subtitlesList = XmlSubtitlesParser.Instance.GetSubtitles(subtitles[currentVideoIndex]);
        if (subtitlesList == null) subtitleText.text = NO_SUB;
        videoPlayer.Play();
        videoTitle.text = videoPlayer.clip.name;
        PauseAllOtherVideoPlayers(this);
    }

    public void VolumeUpDown(bool up) {
        if (up && targetAudio.volume < 1) {
            targetAudio.volume += 0.1f;
        } else if (!up && targetAudio.volume > 0) {
            targetAudio.volume -= 0.1f;
        }
    }

    private void OnValidate() {
        if (videos.Length != subtitles.Length) {
            subtitles = new TextAsset[videos.Length];
            Debug.LogError("Add Subtitles!", gameObject);
        }
    }

    private void Update() {
        if (subtitlesList == null || !videoPlayer.isPlaying) return;
        Subtitle s = subtitlesList[currentSubtitleIndex % subtitlesList.Count];
        if (videoPlayer.time >= s.End)
            subtitleText.text = string.Empty;
        else if (videoPlayer.time >= s.Begin)
            subtitleText.text = s.Text;
        if (videoPlayer.time >= s.End && subtitleText.text == string.Empty)
            currentSubtitleIndex++;
    }

    public static void PauseAllOtherVideoPlayers(VideoController me = null) {
        foreach (var vc in vcs) {
            if (me == vc) continue;
            if (vc.IsPlaying) vc.TogglePlay();
        }
    }
}
