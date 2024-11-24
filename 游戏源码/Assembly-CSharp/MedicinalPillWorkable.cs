using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020014D3 RID: 5331
[AddComponentMenu("KMonoBehaviour/Workable/MedicinalPillWorkable")]
public class MedicinalPillWorkable : Workable, IConsumableUIItem
{
	// Token: 0x06006F11 RID: 28433 RVA: 0x002F1158 File Offset: 0x002EF358
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(10f);
		this.showProgressBar = false;
		this.synchronizeAnims = false;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, null);
		this.CreateChore();
	}

	// Token: 0x06006F12 RID: 28434 RVA: 0x002F11B8 File Offset: 0x002EF3B8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (!string.IsNullOrEmpty(this.pill.info.effect))
		{
			Effects component = worker.GetComponent<Effects>();
			EffectInstance effectInstance = component.Get(this.pill.info.effect);
			if (effectInstance != null)
			{
				effectInstance.timeRemaining = effectInstance.effect.duration;
			}
			else
			{
				component.Add(this.pill.info.effect, true);
			}
		}
		Sicknesses sicknesses = worker.GetSicknesses();
		foreach (string id in this.pill.info.curedSicknesses)
		{
			SicknessInstance sicknessInstance = sicknesses.Get(id);
			if (sicknessInstance != null)
			{
				Game.Instance.savedInfo.curedDisease = true;
				sicknessInstance.Cure();
			}
		}
		base.gameObject.DeleteObject();
	}

	// Token: 0x06006F13 RID: 28435 RVA: 0x000E8C03 File Offset: 0x000E6E03
	private void CreateChore()
	{
		new TakeMedicineChore(this);
	}

	// Token: 0x06006F14 RID: 28436 RVA: 0x002F12AC File Offset: 0x002EF4AC
	public bool CanBeTakenBy(GameObject consumer)
	{
		if (!string.IsNullOrEmpty(this.pill.info.effect))
		{
			Effects component = consumer.GetComponent<Effects>();
			if (component == null || component.HasEffect(this.pill.info.effect))
			{
				return false;
			}
		}
		if (this.pill.info.medicineType == MedicineInfo.MedicineType.Booster)
		{
			return true;
		}
		Sicknesses sicknesses = consumer.GetSicknesses();
		if (this.pill.info.medicineType == MedicineInfo.MedicineType.CureAny && sicknesses.Count > 0)
		{
			return true;
		}
		foreach (SicknessInstance sicknessInstance in sicknesses)
		{
			if (this.pill.info.curedSicknesses.Contains(sicknessInstance.modifier.Id))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x1700071E RID: 1822
	// (get) Token: 0x06006F15 RID: 28437 RVA: 0x002B1318 File Offset: 0x002AF518
	public string ConsumableId
	{
		get
		{
			return this.PrefabID().Name;
		}
	}

	// Token: 0x1700071F RID: 1823
	// (get) Token: 0x06006F16 RID: 28438 RVA: 0x000DF338 File Offset: 0x000DD538
	public string ConsumableName
	{
		get
		{
			return this.GetProperName();
		}
	}

	// Token: 0x17000720 RID: 1824
	// (get) Token: 0x06006F17 RID: 28439 RVA: 0x000E8C0C File Offset: 0x000E6E0C
	public int MajorOrder
	{
		get
		{
			return (int)(this.pill.info.medicineType + 1000);
		}
	}

	// Token: 0x17000721 RID: 1825
	// (get) Token: 0x06006F18 RID: 28440 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public int MinorOrder
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000722 RID: 1826
	// (get) Token: 0x06006F19 RID: 28441 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool Display
	{
		get
		{
			return true;
		}
	}

	// Token: 0x040052F6 RID: 21238
	public MedicinalPill pill;
}
