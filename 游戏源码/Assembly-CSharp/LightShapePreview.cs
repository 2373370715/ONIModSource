using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/LightShapePreview")]
public class LightShapePreview : KMonoBehaviour
{
	public float radius;

	public int lux;

	public int width;

	public DiscreteShadowCaster.Direction direction;

	public LightShape shape;

	public CellOffset offset;

	private int previousCell = -1;

	private void Update()
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (num != previousCell)
		{
			previousCell = num;
			LightGridManager.DestroyPreview();
			LightGridManager.CreatePreview(Grid.OffsetCell(num, offset), radius, shape, lux, width, direction);
		}
	}

	protected override void OnCleanUp()
	{
		LightGridManager.DestroyPreview();
	}
}
