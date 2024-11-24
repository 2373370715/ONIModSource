using System;
using UnityEngine;

// Token: 0x0200170B RID: 5899
public class FogOfWarPostFX : MonoBehaviour
{
	// Token: 0x0600797C RID: 31100 RVA: 0x000EFF41 File Offset: 0x000EE141
	private void Awake()
	{
		if (this.shader != null)
		{
			this.material = new Material(this.shader);
		}
	}

	// Token: 0x0600797D RID: 31101 RVA: 0x000EFF62 File Offset: 0x000EE162
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.SetupUVs();
		Graphics.Blit(source, destination, this.material, 0);
	}

	// Token: 0x0600797E RID: 31102 RVA: 0x00314704 File Offset: 0x00312904
	private void SetupUVs()
	{
		if (this.myCamera == null)
		{
			this.myCamera = base.GetComponent<Camera>();
			if (this.myCamera == null)
			{
				return;
			}
		}
		Ray ray = this.myCamera.ViewportPointToRay(Vector3.zero);
		float distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		Vector3 point = ray.GetPoint(distance);
		Vector4 vector;
		vector.x = point.x / Grid.WidthInMeters;
		vector.y = point.y / Grid.HeightInMeters;
		ray = this.myCamera.ViewportPointToRay(Vector3.one);
		distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		point = ray.GetPoint(distance);
		vector.z = point.x / Grid.WidthInMeters - vector.x;
		vector.w = point.y / Grid.HeightInMeters - vector.y;
		this.material.SetVector("_UVOffsetScale", vector);
	}

	// Token: 0x04005B12 RID: 23314
	[SerializeField]
	private Shader shader;

	// Token: 0x04005B13 RID: 23315
	private Material material;

	// Token: 0x04005B14 RID: 23316
	private Camera myCamera;
}
