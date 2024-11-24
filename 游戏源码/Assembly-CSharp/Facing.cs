using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000A51 RID: 2641
[AddComponentMenu("KMonoBehaviour/scripts/Facing")]
public class Facing : KMonoBehaviour
{
	// Token: 0x0600308B RID: 12427 RVA: 0x000BF8EB File Offset: 0x000BDAEB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.log = new LoggerFS("Facing", 35);
	}

	// Token: 0x0600308C RID: 12428 RVA: 0x000BF905 File Offset: 0x000BDB05
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateMirror();
	}

	// Token: 0x0600308D RID: 12429 RVA: 0x001FC978 File Offset: 0x001FAB78
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

	// Token: 0x0600308E RID: 12430 RVA: 0x001FC9B0 File Offset: 0x001FABB0
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

	// Token: 0x0600308F RID: 12431 RVA: 0x000BF913 File Offset: 0x000BDB13
	[ContextMenu("Flip")]
	public void SwapFacing()
	{
		this.SetFacing(!this.facingLeft);
	}

	// Token: 0x06003090 RID: 12432 RVA: 0x000BF924 File Offset: 0x000BDB24
	private void UpdateMirror()
	{
		if (this.kanimController != null && this.kanimController.FlipX != this.facingLeft)
		{
			this.kanimController.FlipX = this.facingLeft;
			bool flag = this.facingLeft;
		}
	}

	// Token: 0x06003091 RID: 12433 RVA: 0x000BF95F File Offset: 0x000BDB5F
	public bool GetFacing()
	{
		return this.facingLeft;
	}

	// Token: 0x06003092 RID: 12434 RVA: 0x000BF967 File Offset: 0x000BDB67
	public void SetFacing(bool mirror_x)
	{
		this.facingLeft = mirror_x;
		this.UpdateMirror();
	}

	// Token: 0x06003093 RID: 12435 RVA: 0x001FC9F8 File Offset: 0x001FABF8
	public int GetFrontCell()
	{
		int cell = Grid.PosToCell(this);
		if (this.GetFacing())
		{
			return Grid.CellLeft(cell);
		}
		return Grid.CellRight(cell);
	}

	// Token: 0x06003094 RID: 12436 RVA: 0x001FCA24 File Offset: 0x001FAC24
	public int GetBackCell()
	{
		int cell = Grid.PosToCell(this);
		if (!this.GetFacing())
		{
			return Grid.CellLeft(cell);
		}
		return Grid.CellRight(cell);
	}

	// Token: 0x040020CA RID: 8394
	[MyCmpGet]
	private KAnimControllerBase kanimController;

	// Token: 0x040020CB RID: 8395
	private LoggerFS log;

	// Token: 0x040020CC RID: 8396
	[Serialize]
	public bool facingLeft;
}
