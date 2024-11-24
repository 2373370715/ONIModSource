using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Facing")]
public class Facing : KMonoBehaviour
{
	[MyCmpGet]
	private KAnimControllerBase kanimController;

	private LoggerFS log;

	[Serialize]
	public bool facingLeft;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		log = new LoggerFS("Facing");
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		UpdateMirror();
	}

	public void Face(float target_x)
	{
		float x = base.transform.GetLocalPosition().x;
		if (target_x < x)
		{
			SetFacing(mirror_x: true);
		}
		else if (target_x > x)
		{
			SetFacing(mirror_x: false);
		}
	}

	public void Face(Vector3 target_pos)
	{
		int num = Grid.CellColumn(Grid.PosToCell(base.transform.GetLocalPosition()));
		int num2 = Grid.CellColumn(Grid.PosToCell(target_pos));
		if (num > num2)
		{
			SetFacing(mirror_x: true);
		}
		else if (num2 > num)
		{
			SetFacing(mirror_x: false);
		}
	}

	[ContextMenu("Flip")]
	public void SwapFacing()
	{
		SetFacing(!facingLeft);
	}

	private void UpdateMirror()
	{
		if (kanimController != null && kanimController.FlipX != facingLeft)
		{
			kanimController.FlipX = facingLeft;
			_ = facingLeft;
		}
	}

	public bool GetFacing()
	{
		return facingLeft;
	}

	public void SetFacing(bool mirror_x)
	{
		facingLeft = mirror_x;
		UpdateMirror();
	}

	public int GetFrontCell()
	{
		int cell = Grid.PosToCell(this);
		if (GetFacing())
		{
			return Grid.CellLeft(cell);
		}
		return Grid.CellRight(cell);
	}

	public int GetBackCell()
	{
		int cell = Grid.PosToCell(this);
		if (!GetFacing())
		{
			return Grid.CellLeft(cell);
		}
		return Grid.CellRight(cell);
	}
}
