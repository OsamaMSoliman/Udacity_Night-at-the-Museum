using UnityEngine;
using UnityEngine.UI;

public class SlideShow : MonoBehaviour {

    [SerializeField] private float speed = 0.01f;
    [SerializeField] private Transform content;
    [SerializeField] private Sprite[] pictures;

    private ScrollRect scrollRect;
    private float time;
    private bool flip=true;
    private bool isText;

    private void Start() {
        scrollRect = GetComponentInChildren<ScrollRect>();
        foreach (var pic in pictures) {
            Image img = new GameObject().AddComponent<Image>();
            img.sprite = pic;
            img.preserveAspect = true;
            RectTransform t = (RectTransform)img.transform;
            t.SetParent(content.transform);
            t.localPosition = Vector3.zero;
            t.localScale = Vector3.one;
            t.localRotation = Quaternion.identity;
        }
        //Special case
        //if(content.GetComponentInChildren<TMPro.TextMeshProUGUI> != null)
    }

    private void Update() {
        if (flip) time += Time.deltaTime * speed;
        else time -= Time.deltaTime * speed;
        scrollRect.verticalNormalizedPosition = Mathf.Lerp(0, 1, time);
        if (time > 1 || time < 0) flip = !flip;
    }
}
