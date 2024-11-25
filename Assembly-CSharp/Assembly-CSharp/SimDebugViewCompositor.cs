using UnityEngine;

public class SimDebugViewCompositor : MonoBehaviour {
    public static SimDebugViewCompositor Instance;
    public        Material               material;
    private       void                   Awake()     { Instance = this; }
    private       void                   OnDestroy() { Instance = null; }

    private void Start() {
        material = new Material(Shader.Find("Klei/PostFX/SimDebugViewCompositor"));
        Toggle(false);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, dest, material);
        if (OverlayScreen.Instance != null) OverlayScreen.Instance.RunPostProcessEffects(src, dest);
    }

    public void Toggle(bool is_on) { enabled = is_on; }
}