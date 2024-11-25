using System;
using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Facing")]
public class Facing : KMonoBehaviour
{
		protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.log = new LoggerFS("Facing", 35);
	}

		protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateMirror();
	}

		public void Face(float target_x)
	{
		float x = base.transform.GetLocalPosition().x;
		if (target_x < x)
		{
			this.SetFacing(true);
			return;
		}
		if (target_x > x)
		{
			this.SetFacing(false);
		}
	}

		public void Face(Vector3 target_pos)
	{
		int num = Grid.CellColumn(Grid.PosToCell(base.transform.GetLocalPosition()));
		int num2 = Grid.CellColumn(Grid.PosToCell(target_pos));
		if (num > num2)
		{
			this.SetFacing(true);
			return;
		}
		if (num2 > num)
		{
			this.SetFacing(false);
		}
	}

		[ContextMenu("Flip")]
	public void SwapFacing()
	{
		this.SetFacing(!this.facingLeft);
	}

		private void UpdateMirror()
	{
		if (this.kanimController != null && this.kanimController.FlipX != this.facingLeft)
		{
			this.kanimController.FlipX = this.facingLeft;
			bool flag = this.facingLeft;
		}
	}

		public bool GetFacing()
	{
		return this.facingLeft;
	}

		public void SetFacing(bool mirror_x)
	{
		this.facingLeft = mirror_x;
		this.UpdateMirror();
	}

		public int GetFrontCell()
	{
		int cell = Grid.PosToCell(this);
		if (this.GetFacing())
		{
			return Grid.CellLeft(cell);
		}
		return Grid.CellRight(cell);
	}

		public int GetBackCell()
	{
		int cell = Grid.PosToCell(this);
		if (!this.GetFacing())
		{
			return Grid.CellLeft(cell);
		}
		return Grid.CellRight(cell);
	}

		[MyCmpGet]
	private KAnimControllerBase kanimController;

		private LoggerFS log;

		[Serialize]
	public bool facingLeft;
}
