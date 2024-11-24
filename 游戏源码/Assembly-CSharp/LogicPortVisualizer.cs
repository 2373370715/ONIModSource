using System;
using UnityEngine;

// Token: 0x020014B8 RID: 5304
public class LogicPortVisualizer : ILogicUIElement, IUniformGridObject
{
	// Token: 0x06006E88 RID: 28296 RVA: 0x000E86F4 File Offset: 0x000E68F4
	public LogicPortVisualizer(int cell, LogicPortSpriteType sprite_type)
	{
		this.cell = cell;
		this.spriteType = sprite_type;
	}

	// Token: 0x06006E89 RID: 28297 RVA: 0x000E870A File Offset: 0x000E690A
	public int GetLogicUICell()
	{
		return this.cell;
	}

	// Token: 0x06006E8A RID: 28298 RVA: 0x000E8712 File Offset: 0x000E6912
	public Vector2 PosMin()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06006E8B RID: 28299 RVA: 0x000E8712 File Offset: 0x000E6912
	public Vector2 PosMax()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06006E8C RID: 28300 RVA: 0x000E8724 File Offset: 0x000E6924
	public LogicPortSpriteType GetLogicPortSpriteType()
	{
		return this.spriteType;
	}

	// Token: 0x040052A9 RID: 21161
	private int cell;

	// Token: 0x040052AA RID: 21162
	private LogicPortSpriteType spriteType;
}
