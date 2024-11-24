using UnityEngine;

public static class StampToolPreviewUtil
{
	public static readonly Color COLOR_OK = Color.white;

	public static readonly Color COLOR_ERROR = Color.red;

	public const float SOLID_VIS_ALPHA = 1f;

	public const float LIQUID_VIS_ALPHA = 1f;

	public const float GAS_VIS_ALPHA = 1f;

	public const float BACKGROUND_ALPHA = 1f;

	public static Material MakeMaterial(Texture texture)
	{
		Material material = new Material(Shader.Find("Sprites/Default"));
		material.SetTexture("_MainTex", texture);
		return material;
	}

	public static void MakeQuad(out GameObject gameObject, out MeshRenderer meshRenderer, float mesh_size, Vector4? uvBox = null)
	{
		gameObject = new GameObject();
		gameObject.layer = LayerMask.NameToLayer("Place");
		float num = mesh_size / 2f;
		float num2 = mesh_size / 2f;
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[4]
		{
			new Vector3(0f - num, 0f - num2, 0f),
			new Vector3(num, 0f - num2, 0f),
			new Vector3(0f - num, num2, 0f),
			new Vector3(num, num2, 0f)
		};
		mesh.triangles = new int[6] { 0, 2, 1, 2, 3, 1 };
		mesh.normals = new Vector3[4]
		{
			-Vector3.forward,
			-Vector3.forward,
			-Vector3.forward,
			-Vector3.forward
		};
		mesh.uv = (uvBox.HasValue ? new Vector2[4]
		{
			new Vector2(uvBox.Value.x, uvBox.Value.w),
			new Vector2(uvBox.Value.z, uvBox.Value.w),
			new Vector2(uvBox.Value.x, uvBox.Value.y),
			new Vector2(uvBox.Value.z, uvBox.Value.y)
		} : new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		});
		Mesh mesh2 = mesh;
		gameObject.AddComponent<MeshFilter>().mesh = mesh2;
		meshRenderer = gameObject.AddComponent<MeshRenderer>();
	}
}
