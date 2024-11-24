using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02001A3F RID: 6719
[AddComponentMenu("KMonoBehaviour/scripts/WaterCubes")]
public class WaterCubes : KMonoBehaviour
{
	// Token: 0x1700092B RID: 2347
	// (get) Token: 0x06008C1D RID: 35869 RVA: 0x000FB950 File Offset: 0x000F9B50
	// (set) Token: 0x06008C1E RID: 35870 RVA: 0x000FB957 File Offset: 0x000F9B57
	public static WaterCubes Instance { get; private set; }

	// Token: 0x06008C1F RID: 35871 RVA: 0x000FB95F File Offset: 0x000F9B5F
	public static void DestroyInstance()
	{
		WaterCubes.Instance = null;
	}

	// Token: 0x06008C20 RID: 35872 RVA: 0x000FB967 File Offset: 0x000F9B67
	protected override void OnPrefabInit()
	{
		WaterCubes.Instance = this;
	}

	// Token: 0x06008C21 RID: 35873 RVA: 0x00361E50 File Offset: 0x00360050
	public void Init()
	{
		this.cubes = Util.NewGameObject(base.gameObject, "WaterCubes");
		GameObject gameObject = new GameObject();
		gameObject.name = "WaterCubesMesh";
		gameObject.transform.parent = this.cubes.transform;
		this.material.renderQueue = RenderQueues.Liquid;
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshRenderer.sharedMaterial = this.material;
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		meshRenderer.receiveShadows = false;
		meshRenderer.lightProbeUsage = LightProbeUsage.Off;
		meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
		meshRenderer.sharedMaterial.SetTexture("_MainTex2", this.waveTexture);
		meshFilter.sharedMesh = this.CreateNewMesh();
		meshRenderer.gameObject.layer = 0;
		meshRenderer.gameObject.transform.parent = base.transform;
		meshRenderer.gameObject.transform.SetPosition(new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.Liquid)));
	}

	// Token: 0x06008C22 RID: 35874 RVA: 0x00361F48 File Offset: 0x00360148
	private Mesh CreateNewMesh()
	{
		Mesh mesh = new Mesh();
		mesh.name = "WaterCubes";
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] array = new Vector2[num];
		Vector3[] normals = new Vector3[num];
		Vector4[] tangents = new Vector4[num];
		int[] triangles = new int[6];
		float layerZ = Grid.GetLayerZ(Grid.SceneLayer.Liquid);
		vertices = new Vector3[]
		{
			new Vector3(0f, 0f, layerZ),
			new Vector3((float)Grid.WidthInCells, 0f, layerZ),
			new Vector3(0f, Grid.HeightInMeters, layerZ),
			new Vector3(Grid.WidthInMeters, Grid.HeightInMeters, layerZ)
		};
		array = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		normals = new Vector3[]
		{
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, -1f)
		};
		tangents = new Vector4[]
		{
			new Vector4(0f, 1f, 0f, -1f),
			new Vector4(0f, 1f, 0f, -1f),
			new Vector4(0f, 1f, 0f, -1f),
			new Vector4(0f, 1f, 0f, -1f)
		};
		triangles = new int[]
		{
			0,
			2,
			1,
			1,
			2,
			3
		};
		mesh.vertices = vertices;
		mesh.uv = array;
		mesh.uv2 = array;
		mesh.normals = normals;
		mesh.tangents = tangents;
		mesh.triangles = triangles;
		mesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, 0f));
		return mesh;
	}

	// Token: 0x04006982 RID: 27010
	public Material material;

	// Token: 0x04006983 RID: 27011
	public Texture2D waveTexture;

	// Token: 0x04006984 RID: 27012
	private GameObject cubes;
}
