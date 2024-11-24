using System;
using UnityEngine;

// Token: 0x0200170A RID: 5898
public class ClusterCoverPostFX : MonoBehaviour
{
	// Token: 0x06007978 RID: 31096 RVA: 0x000EFF0A File Offset: 0x000EE10A
	private void Awake()
	{
		if (this.shader != null)
		{
			this.material = new Material(this.shader);
		}
	}

	// Token: 0x06007979 RID: 31097 RVA: 0x000EFF2B File Offset: 0x000EE12B
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.SetupUVs();
		Graphics.Blit(source, destination, this.material, 0);
	}

	// Token: 0x0600797A RID: 31098 RVA: 0x0031453C File Offset: 0x0031273C
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
		ray = this.myCamera.ViewportPointToRay(Vector3.one);
		distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		Vector3 point2 = ray.GetPoint(distance);
		Vector4 value;
		value.x = point.x;
		value.y = point.y;
		value.z = point2.x - point.x;
		value.w = point2.y - point.y;
		this.material.SetVector("_CameraCoords", value);
		Vector4 value2;
		if (ClusterManager.Instance != null && !CameraController.Instance.ignoreClusterFX)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			Vector2I worldOffset = activeWorld.WorldOffset;
			Vector2I worldSize = activeWorld.WorldSize;
			value2 = new Vector4((float)worldOffset.x, (float)worldOffset.y, (float)worldSize.x, (float)worldSize.y);
			this.material.SetFloat("_HideSurface", ClusterManager.Instance.activeWorld.FullyEnclosedBorder ? 1f : 0f);
		}
		else
		{
			value2 = new Vector4(0f, 0f, (float)Grid.WidthInCells, (float)Grid.HeightInCells);
			this.material.SetFloat("_HideSurface", 0f);
		}
		this.material.SetVector("_WorldCoords", value2);
	}

	// Token: 0x04005B0F RID: 23311
	[SerializeField]
	private Shader shader;

	// Token: 0x04005B10 RID: 23312
	private Material material;

	// Token: 0x04005B11 RID: 23313
	private Camera myCamera;
}
