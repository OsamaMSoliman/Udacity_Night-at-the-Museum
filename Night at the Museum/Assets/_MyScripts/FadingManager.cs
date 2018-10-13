using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadingManager : MonoBehaviour {

    [SerializeField] private Image fadingImg;
    private void Awake() { fadingImg = GetComponentInChildren<Image>(); }


    public event Action BeginFading; // nothing
    public event Action MidFading; // move to position
    public event Action EndFading; // start playing video

    public void FadeInAndOut() {
        if (BeginFading != null) { BeginFading(); BeginFading = null; }
        StartCoroutine(FadeIn());
    }

    private WaitForSeconds delay = new WaitForSeconds(0.1f);
    private IEnumerator FadeIn() {
        for (float t = 0; fadingImg.color.a < 1; t += 0.1f) {
            fadingImg.color = Color.Lerp(Color.clear, Color.black, t);
            yield return delay;
        }
        if (MidFading != null) { MidFading(); MidFading = null; }
        StartCoroutine(FadeOut());
    }
    private IEnumerator FadeOut() {
        for (float t = 0; fadingImg.color.a > 0; t += 0.1f) {
            fadingImg.color = Color.Lerp(Color.black, Color.clear, t);
            yield return delay;
        }
        if (EndFading != null) { EndFading(); EndFading = null; }
    }
}
