using System;
using UnityEngine;

public class ClusterCoverPostFX : MonoBehaviour
{
	private void Awake()
	{
		if (this.shader != null)
		{
			this.material = new Material(this.shader);
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.SetupUVs();
		Graphics.Blit(source, destination, this.material, 0);
	}

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

	[SerializeField]
	private Shader shader;

	private Material material;

	private Camera myCamera;
}
