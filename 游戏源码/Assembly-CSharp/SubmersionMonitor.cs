using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020011E3 RID: 4579
[AddComponentMenu("KMonoBehaviour/scripts/SubmersionMonitor")]
public class SubmersionMonitor : KMonoBehaviour, IGameObjectEffectDescriptor, IWiltCause, ISim1000ms
{
	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x06005D25 RID: 23845 RVA: 0x000DCCF6 File Offset: 0x000DAEF6
	public bool Dry
	{
		get
		{
			return this.dry;
		}
	}

	// Token: 0x06005D26 RID: 23846 RVA: 0x000DCCFE File Offset: 0x000DAEFE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnMove();
		this.CheckDry();
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnMove), "SubmersionMonitor.OnSpawn");
	}

	// Token: 0x06005D27 RID: 23847 RVA: 0x0029D764 File Offset: 0x0029B964
	private void OnMove()
	{
		this.position = Grid.PosToCell(base.gameObject);
		if (this.partitionerEntry.IsValid())
		{
			GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, this.position);
		}
		else
		{
			Vector2I vector2I = Grid.PosToXY(base.transform.GetPosition());
			Extents extents = new Extents(vector2I.x, vector2I.y, 1, 2);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("DrowningMonitor.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
		}
		this.CheckDry();
	}

	// Token: 0x06005D28 RID: 23848 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void OnDrawGizmosSelected()
	{
	}

	// Token: 0x06005D29 RID: 23849 RVA: 0x000DCD34 File Offset: 0x000DAF34
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnMove));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06005D2A RID: 23850 RVA: 0x000DCD68 File Offset: 0x000DAF68
	public void Configure(float _maxStamina, float _staminaRegenRate, float _cellLiquidThreshold = 0.95f)
	{
		this.cellLiquidThreshold = _cellLiquidThreshold;
	}

	// Token: 0x06005D2B RID: 23851 RVA: 0x000DCD71 File Offset: 0x000DAF71
	public void Sim1000ms(float dt)
	{
		this.CheckDry();
	}

	// Token: 0x06005D2C RID: 23852 RVA: 0x0029D808 File Offset: 0x0029BA08
	private void CheckDry()
	{
		if (!this.IsCellSafe())
		{
			if (!this.dry)
			{
				this.dry = true;
				base.Trigger(-2057657673, null);
				return;
			}
		}
		else if (this.dry)
		{
			this.dry = false;
			base.Trigger(1555379996, null);
		}
	}

	// Token: 0x06005D2D RID: 23853 RVA: 0x0029D854 File Offset: 0x0029BA54
	public bool IsCellSafe()
	{
		int cell = Grid.PosToCell(base.gameObject);
		return Grid.IsValidCell(cell) && Grid.IsSubstantialLiquid(cell, this.cellLiquidThreshold);
	}

	// Token: 0x06005D2E RID: 23854 RVA: 0x000DCD71 File Offset: 0x000DAF71
	private void OnLiquidChanged(object data)
	{
		this.CheckDry();
	}

	// Token: 0x17000587 RID: 1415
	// (get) Token: 0x06005D2F RID: 23855 RVA: 0x000DCD79 File Offset: 0x000DAF79
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.DryingOut
			};
		}
	}

	// Token: 0x17000588 RID: 1416
	// (get) Token: 0x06005D30 RID: 23856 RVA: 0x000DCD85 File Offset: 0x000DAF85
	public string WiltStateString
	{
		get
		{
			if (this.Dry)
			{
				return Db.Get().CreatureStatusItems.DryingOut.resolveStringCallback(CREATURES.STATUSITEMS.DRYINGOUT.NAME, this);
			}
			return "";
		}
	}

	// Token: 0x06005D31 RID: 23857 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void SetIncapacitated(bool state)
	{
	}

	// Token: 0x06005D32 RID: 23858 RVA: 0x000DCDB9 File Offset: 0x000DAFB9
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_SUBMERSION, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_SUBMERSION, Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x040041EE RID: 16878
	private int position;

	// Token: 0x040041EF RID: 16879
	private bool dry;

	// Token: 0x040041F0 RID: 16880
	protected float cellLiquidThreshold = 0.2f;

	// Token: 0x040041F1 RID: 16881
	private Extents extents;

	// Token: 0x040041F2 RID: 16882
	private HandleVector<int>.Handle partitionerEntry;
}
