using UnityEngine;

public abstract class VisualizerEffect : MonoBehaviour
{
	protected Material material;

	protected Camera myCamera;

	protected Texture2D OcclusionTex;

	protected abstract void SetupMaterial();

	protected abstract void SetupOcclusionTex();

	protected abstract void OnPostRender();

	protected virtual void Start()
	{
		SetupMaterial();
		SetupOcclusionTex();
		myCamera = GetComponent<Camera>();
	}
}
