using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E48 RID: 3656
[SkipSaveFileSerialization]
public class LogicGateVisualizer : LogicGateBase
{
	// Token: 0x06004882 RID: 18562 RVA: 0x000CF10B File Offset: 0x000CD30B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
	}

	// Token: 0x06004883 RID: 18563 RVA: 0x000CF119 File Offset: 0x000CD319
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.Unregister();
	}

	// Token: 0x06004884 RID: 18564 RVA: 0x00256D84 File Offset: 0x00254F84
	private void Register()
	{
		this.Unregister();
		this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellOne, false));
		if (base.RequiresFourOutputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellTwo, false));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellThree, false));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellFour, false));
		}
		this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellOne, true));
		if (base.RequiresTwoInputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellTwo, true));
		}
		else if (base.RequiresFourInputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellTwo, true));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellThree, true));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellFour, true));
		}
		if (base.RequiresControlInputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.ControlCellOne, true));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.ControlCellTwo, true));
		}
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		foreach (LogicGateVisualizer.IOVisualizer elem in this.visChildren)
		{
			logicCircuitManager.AddVisElem(elem);
		}
	}

	// Token: 0x06004885 RID: 18565 RVA: 0x00256F08 File Offset: 0x00255108
	private void Unregister()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		foreach (LogicGateVisualizer.IOVisualizer elem in this.visChildren)
		{
			logicCircuitManager.RemoveVisElem(elem);
		}
		this.visChildren.Clear();
	}

	// Token: 0x040032B8 RID: 12984
	private List<LogicGateVisualizer.IOVisualizer> visChildren = new List<LogicGateVisualizer.IOVisualizer>();

	// Token: 0x02000E49 RID: 3657
	private class IOVisualizer : ILogicUIElement, IUniformGridObject
	{
		// Token: 0x06004887 RID: 18567 RVA: 0x000CF13A File Offset: 0x000CD33A
		public IOVisualizer(int cell, bool input)
		{
			this.cell = cell;
			this.input = input;
		}

		// Token: 0x06004888 RID: 18568 RVA: 0x000CF150 File Offset: 0x000CD350
		public int GetLogicUICell()
		{
			return this.cell;
		}

		// Token: 0x06004889 RID: 18569 RVA: 0x000CF158 File Offset: 0x000CD358
		public LogicPortSpriteType GetLogicPortSpriteType()
		{
			if (!this.input)
			{
				return LogicPortSpriteType.Output;
			}
			return LogicPortSpriteType.Input;
		}

		// Token: 0x0600488A RID: 18570 RVA: 0x000CF165 File Offset: 0x000CD365
		public Vector2 PosMin()
		{
			return Grid.CellToPos2D(this.cell);
		}

		// Token: 0x0600488B RID: 18571 RVA: 0x000CF177 File Offset: 0x000CD377
		public Vector2 PosMax()
		{
			return this.PosMin();
		}

		// Token: 0x040032B9 RID: 12985
		private int cell;

		// Token: 0x040032BA RID: 12986
		private bool input;
	}
}
