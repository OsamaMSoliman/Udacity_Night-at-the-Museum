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

    private VideoPlayer videoPlayer;

    private List<Subtitle> subtitlesList;

    private int currentVideoIndex;
    private int currentSubtitleIndex;
    private bool _isPlaying; // I used this instead of VideoPlayer.isPlaying for more accurate performance
    private bool IsPlaying {
        get { return _isPlaying; }
        set {
            _isPlaying = value;
            pressToPlayScreen.SetActive(!_isPlaying);
            subtitleText.text = string.Empty;
        }
    }

    private void Start() {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.targetTexture = GameManager.GetNewRenderTexture(videoPlayer.targetTexture, gameObject);
        videoPlayer.SetTargetAudioSource(0, GameManager.Instance.AudioManager.audioSource);
        videoPlayer.clip = videos[currentVideoIndex];
        subtitlesList = XmlSubtitlesParser.Instance.GetSubtitles(subtitles[currentVideoIndex % subtitles.Length]);
        if (subtitlesList == null) subtitleText.text = NO_SUB;
    }
    
    public void TogglePlay() {
        if (IsPlaying) videoPlayer.Pause();
        else videoPlayer.Play();
        IsPlaying = !IsPlaying;
        videoTitle.text = videoPlayer.clip.name;
    }

    public void PlayNext() {
        IsPlaying = true;
        currentSubtitleIndex = 0;
        videoPlayer.Stop();
        videoPlayer.clip = videos[(++currentVideoIndex) % videos.Length];
        subtitlesList = XmlSubtitlesParser.Instance.GetSubtitles(subtitles[currentVideoIndex]);
        if (subtitlesList == null) subtitleText.text = NO_SUB;
        videoPlayer.Play();
        videoTitle.text = videoPlayer.clip.name;
    }

    public void VolumeUpDown(bool up) { GameManager.Instance.AudioManager.VolumeTuning(up); }

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
}
