using System;
using STRINGS;

// Token: 0x02000C43 RID: 3139
public class BionicUpgradeComponent : Assignable
{
	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x06003C2A RID: 15402 RVA: 0x000C6DAC File Offset: 0x000C4FAC
	// (set) Token: 0x06003C29 RID: 15401 RVA: 0x000C6DA3 File Offset: 0x000C4FA3
	public BionicUpgradeComponent.IWattageController WattageController { get; private set; }

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x06003C2B RID: 15403 RVA: 0x000C6DB4 File Offset: 0x000C4FB4
	public float CurrentWattage
	{
		get
		{
			if (!this.HasWattageController)
			{
				return 0f;
			}
			return this.WattageController.GetCurrentWattageCost();
		}
	}

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x06003C2C RID: 15404 RVA: 0x000C6DCF File Offset: 0x000C4FCF
	public string CurrentWattageName
	{
		get
		{
			if (!this.HasWattageController)
			{
				return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.STANDARD_INACTIVE_TEMPLATE, this.GetProperName(), GameUtil.GetFormattedWattage(this.PotentialWattage, GameUtil.WattageFormatterUnit.Automatic, true));
			}
			return this.WattageController.GetCurrentWattageCostName();
		}
	}

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06003C2D RID: 15405 RVA: 0x000C6E07 File Offset: 0x000C5007
	public bool HasWattageController
	{
		get
		{
			return this.WattageController != null;
		}
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06003C2E RID: 15406 RVA: 0x000C6E12 File Offset: 0x000C5012
	public float PotentialWattage
	{
		get
		{
			return this.data.WattageCost;
		}
	}

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x06003C2F RID: 15407 RVA: 0x000C6E1F File Offset: 0x000C501F
	public BionicUpgradeComponentConfig.RarityType Rarity
	{
		get
		{
			return this.data.rarity;
		}
	}

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06003C30 RID: 15408 RVA: 0x000C6E2C File Offset: 0x000C502C
	public Func<StateMachine.Instance, StateMachine.Instance> StateMachine
	{
		get
		{
			return this.data.stateMachine;
		}
	}

	// Token: 0x06003C31 RID: 15409 RVA: 0x000C6E39 File Offset: 0x000C5039
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.data = BionicUpgradeComponentConfig.UpgradesData[base.gameObject.PrefabID()];
		base.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.AssignablePrecondition_OnlyOnBionics));
	}

	// Token: 0x06003C32 RID: 15410 RVA: 0x000C6E6E File Offset: 0x000C506E
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06003C33 RID: 15411 RVA: 0x000C6E76 File Offset: 0x000C5076
	public void InformOfWattageChanged()
	{
		System.Action onWattageCostChanged = this.OnWattageCostChanged;
		if (onWattageCostChanged == null)
		{
			return;
		}
		onWattageCostChanged();
	}

	// Token: 0x06003C34 RID: 15412 RVA: 0x000C6E88 File Offset: 0x000C5088
	public void SetWattageController(BionicUpgradeComponent.IWattageController wattageController)
	{
		this.WattageController = wattageController;
	}

	// Token: 0x06003C35 RID: 15413 RVA: 0x0022D660 File Offset: 0x0022B860
	public override void Assign(IAssignableIdentity new_assignee)
	{
		AssignableSlotInstance specificSlotInstance = null;
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (new_assignee != this.assignee && (new_assignee is MinionIdentity || new_assignee is StoredMinionIdentity || new_assignee is MinionAssignablesProxy))
		{
			Ownables soleOwner = new_assignee.GetSoleOwner();
			if (soleOwner != null)
			{
				BionicUpgradesMonitor.Instance smi = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetSMI<BionicUpgradesMonitor.Instance>();
				if (smi != null)
				{
					BionicUpgradesMonitor.UpgradeComponentSlot firstEmptySlot = smi.GetFirstEmptySlot();
					if (firstEmptySlot != null)
					{
						specificSlotInstance = firstEmptySlot.GetAssignableSlotInstance();
					}
				}
			}
		}
		base.Assign(new_assignee, specificSlotInstance);
		base.Trigger(1980521255, null);
	}

	// Token: 0x06003C36 RID: 15414 RVA: 0x000C6E91 File Offset: 0x000C5091
	public override void Unassign()
	{
		base.Unassign();
		base.Trigger(1980521255, null);
	}

	// Token: 0x06003C37 RID: 15415 RVA: 0x0022D6E4 File Offset: 0x0022B8E4
	private bool AssignablePrecondition_OnlyOnBionics(MinionAssignablesProxy worker)
	{
		bool result = false;
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			result = (minionIdentity.GetSMI<BionicUpgradesMonitor.Instance>() != null);
		}
		return result;
	}

	// Token: 0x0400292D RID: 10541
	private BionicUpgradeComponentConfig.BionicUpgradeData data;

	// Token: 0x0400292E RID: 10542
	public System.Action OnWattageCostChanged;

	// Token: 0x02000C44 RID: 3140
	public interface IWattageController
	{
		// Token: 0x06003C39 RID: 15417
		float GetCurrentWattageCost();

		// Token: 0x06003C3A RID: 15418
		string GetCurrentWattageCostName();
	}
}
