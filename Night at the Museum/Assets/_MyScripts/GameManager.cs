using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    [HideInInspector] public AudioManager AudioManager;
    [HideInInspector] public FadingManager FadingManager;

    private Transform player;
    private GvrReticlePointer gvr_rp;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        AudioManager = GetComponent<AudioManager>();
        FadingManager = GetComponent<FadingManager>();
        player = Camera.main.transform.parent;
        gvr_rp = Camera.main.transform.GetChild(0).GetComponent<GvrReticlePointer>();
    }

    public void MovePlayerTo(Vector3 pos) { player.position = pos; }

    public void ChangeRayLength(bool elongate) {
        gvr_rp.maxReticleDistance = elongate ? 20 : 5;
    }

    public static RenderTexture GetNewRenderTexture(Material targetMaterial) {
        RenderTexture renderTexture = new RenderTexture((RenderTexture)targetMaterial.GetTexture("_MainTex"));
        targetMaterial.SetTexture("_MainTex", renderTexture);
        targetMaterial.SetTexture("_EmissionMap", renderTexture);
        targetMaterial.SetColor("_EmissionColor", Color.white);
        return renderTexture;
    }
}
