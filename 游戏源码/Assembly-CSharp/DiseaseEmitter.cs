using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x0200123F RID: 4671
[AddComponentMenu("KMonoBehaviour/scripts/DiseaseEmitter")]
public class DiseaseEmitter : KMonoBehaviour
{
	// Token: 0x170005C1 RID: 1473
	// (get) Token: 0x06005FA8 RID: 24488 RVA: 0x000DE530 File Offset: 0x000DC730
	public float EmitRate
	{
		get
		{
			return this.emitRate;
		}
	}

	// Token: 0x06005FA9 RID: 24489 RVA: 0x002AB580 File Offset: 0x002A9780
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.emitDiseases != null)
		{
			this.simHandles = new int[this.emitDiseases.Length];
			for (int i = 0; i < this.simHandles.Length; i++)
			{
				this.simHandles[i] = -1;
			}
		}
		this.SimRegister();
	}

	// Token: 0x06005FAA RID: 24490 RVA: 0x000DE538 File Offset: 0x000DC738
	protected override void OnCleanUp()
	{
		this.SimUnregister();
		base.OnCleanUp();
	}

	// Token: 0x06005FAB RID: 24491 RVA: 0x000DE546 File Offset: 0x000DC746
	public void SetEnable(bool enable)
	{
		if (this.enableEmitter == enable)
		{
			return;
		}
		this.enableEmitter = enable;
		if (this.enableEmitter)
		{
			this.SimRegister();
			return;
		}
		this.SimUnregister();
	}

	// Token: 0x06005FAC RID: 24492 RVA: 0x002AB5D0 File Offset: 0x002A97D0
	private void OnCellChanged()
	{
		if (this.simHandles == null || !this.enableEmitter)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		if (Grid.IsValidCell(cell))
		{
			for (int i = 0; i < this.emitDiseases.Length; i++)
			{
				if (Sim.IsValidHandle(this.simHandles[i]))
				{
					SimMessages.ModifyDiseaseEmitter(this.simHandles[i], cell, this.emitRange, this.emitDiseases[i], this.emitRate, this.emitCount);
				}
			}
		}
	}

	// Token: 0x06005FAD RID: 24493 RVA: 0x002AB648 File Offset: 0x002A9848
	private void SimRegister()
	{
		if (this.simHandles == null || !this.enableEmitter)
		{
			return;
		}
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged), "DiseaseEmitter.Modify");
		for (int i = 0; i < this.simHandles.Length; i++)
		{
			if (this.simHandles[i] == -1)
			{
				this.simHandles[i] = -2;
				SimMessages.AddDiseaseEmitter(Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(DiseaseEmitter.OnSimRegisteredCallback), this, "DiseaseEmitter").index);
			}
		}
	}

	// Token: 0x06005FAE RID: 24494 RVA: 0x002AB6E0 File Offset: 0x002A98E0
	private void SimUnregister()
	{
		if (this.simHandles == null)
		{
			return;
		}
		for (int i = 0; i < this.simHandles.Length; i++)
		{
			if (Sim.IsValidHandle(this.simHandles[i]))
			{
				SimMessages.RemoveDiseaseEmitter(-1, this.simHandles[i]);
			}
			this.simHandles[i] = -1;
		}
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged));
	}

	// Token: 0x06005FAF RID: 24495 RVA: 0x000DE56E File Offset: 0x000DC76E
	private static void OnSimRegisteredCallback(int handle, object data)
	{
		((DiseaseEmitter)data).OnSimRegistered(handle);
	}

	// Token: 0x06005FB0 RID: 24496 RVA: 0x002AB74C File Offset: 0x002A994C
	private void OnSimRegistered(int handle)
	{
		bool flag = false;
		if (this != null)
		{
			for (int i = 0; i < this.simHandles.Length; i++)
			{
				if (this.simHandles[i] == -2)
				{
					this.simHandles[i] = handle;
					flag = true;
					break;
				}
			}
			this.OnCellChanged();
		}
		if (!flag)
		{
			SimMessages.RemoveDiseaseEmitter(-1, handle);
		}
	}

	// Token: 0x06005FB1 RID: 24497 RVA: 0x002AB7A0 File Offset: 0x002A99A0
	public void SetDiseases(List<Disease> diseases)
	{
		this.emitDiseases = new byte[diseases.Count];
		for (int i = 0; i < diseases.Count; i++)
		{
			this.emitDiseases[i] = Db.Get().Diseases.GetIndex(diseases[i].id);
		}
	}

	// Token: 0x040043D8 RID: 17368
	[Serialize]
	public float emitRate = 1f;

	// Token: 0x040043D9 RID: 17369
	[Serialize]
	public byte emitRange;

	// Token: 0x040043DA RID: 17370
	[Serialize]
	public int emitCount;

	// Token: 0x040043DB RID: 17371
	[Serialize]
	public byte[] emitDiseases;

	// Token: 0x040043DC RID: 17372
	public int[] simHandles;

	// Token: 0x040043DD RID: 17373
	[Serialize]
	private bool enableEmitter;
}
