using System;
using UnityEngine;

// Token: 0x020014AF RID: 5295
internal class LogicEventSender : ILogicEventSender, ILogicNetworkConnection, ILogicUIElement, IUniformGridObject
{
	// Token: 0x06006E35 RID: 28213 RVA: 0x000E837F File Offset: 0x000E657F
	public LogicEventSender(HashedString id, int cell, Action<int, int> on_value_changed, Action<int, bool> on_connection_changed, LogicPortSpriteType sprite_type)
	{
		this.id = id;
		this.cell = cell;
		this.onValueChanged = on_value_changed;
		this.onConnectionChanged = on_connection_changed;
		this.spriteType = sprite_type;
	}

	// Token: 0x1700070E RID: 1806
	// (get) Token: 0x06006E36 RID: 28214 RVA: 0x000E83B4 File Offset: 0x000E65B4
	public HashedString ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x06006E37 RID: 28215 RVA: 0x000E83BC File Offset: 0x000E65BC
	public int GetLogicCell()
	{
		return this.cell;
	}

	// Token: 0x06006E38 RID: 28216 RVA: 0x000E83C4 File Offset: 0x000E65C4
	public int GetLogicValue()
	{
		return this.logicValue;
	}

	// Token: 0x06006E39 RID: 28217 RVA: 0x000E83CC File Offset: 0x000E65CC
	public int GetLogicUICell()
	{
		return this.GetLogicCell();
	}

	// Token: 0x06006E3A RID: 28218 RVA: 0x000E83D4 File Offset: 0x000E65D4
	public LogicPortSpriteType GetLogicPortSpriteType()
	{
		return this.spriteType;
	}

	// Token: 0x06006E3B RID: 28219 RVA: 0x000E83DC File Offset: 0x000E65DC
	public Vector2 PosMin()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06006E3C RID: 28220 RVA: 0x000E83DC File Offset: 0x000E65DC
	public Vector2 PosMax()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06006E3D RID: 28221 RVA: 0x002EE3C4 File Offset: 0x002EC5C4
	public void SetValue(int value)
	{
		int arg = this.logicValue;
		this.logicValue = value;
		this.onValueChanged(value, arg);
	}

	// Token: 0x06006E3E RID: 28222 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void LogicTick()
	{
	}

	// Token: 0x06006E3F RID: 28223 RVA: 0x000E83EE File Offset: 0x000E65EE
	public void OnLogicNetworkConnectionChanged(bool connected)
	{
		if (this.onConnectionChanged != null)
		{
			this.onConnectionChanged(this.cell, connected);
		}
	}

	// Token: 0x04005274 RID: 21108
	private HashedString id;

	// Token: 0x04005275 RID: 21109
	private int cell;

	// Token: 0x04005276 RID: 21110
	private int logicValue = -16;

	// Token: 0x04005277 RID: 21111
	private Action<int, int> onValueChanged;

	// Token: 0x04005278 RID: 21112
	private Action<int, bool> onConnectionChanged;

	// Token: 0x04005279 RID: 21113
	private LogicPortSpriteType spriteType;
}
