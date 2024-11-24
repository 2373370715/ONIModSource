using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001276 RID: 4726
public class Electrobank : KMonoBehaviour, ISim1000ms, IConsumableUIItem, IGameObjectEffectDescriptor
{
	// Token: 0x170005E1 RID: 1505
	// (get) Token: 0x060060D6 RID: 24790 RVA: 0x000DF2DB File Offset: 0x000DD4DB
	// (set) Token: 0x060060D5 RID: 24789 RVA: 0x000DF2D2 File Offset: 0x000DD4D2
	public string ID { get; private set; }

	// Token: 0x170005E2 RID: 1506
	// (get) Token: 0x060060D7 RID: 24791 RVA: 0x000DF2E3 File Offset: 0x000DD4E3
	public bool IsFullyCharged
	{
		get
		{
			return this.charge == Electrobank.capacity;
		}
	}

	// Token: 0x170005E3 RID: 1507
	// (get) Token: 0x060060D8 RID: 24792 RVA: 0x000DF2F2 File Offset: 0x000DD4F2
	public float Charge
	{
		get
		{
			return this.charge;
		}
	}

	// Token: 0x060060D9 RID: 24793 RVA: 0x002B0EBC File Offset: 0x002AF0BC
	protected override void OnPrefabInit()
	{
		this.ID = base.gameObject.PrefabID().ToString();
		base.Subscribe(748399584, new Action<object>(this.OnCraft));
		base.OnPrefabInit();
	}

	// Token: 0x060060DA RID: 24794 RVA: 0x000BFD08 File Offset: 0x000BDF08
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060060DB RID: 24795 RVA: 0x000DF2FA File Offset: 0x000DD4FA
	private void OnCraft(object data)
	{
		WorldResourceAmountTracker<ElectrobankTracker>.Get().RegisterAmountProduced(this.Charge);
	}

	// Token: 0x060060DC RID: 24796 RVA: 0x002B0F08 File Offset: 0x002AF108
	public static GameObject ReplaceEmptyWithCharged(GameObject EmptyElectrobank, bool dropFromStorage = false)
	{
		Vector3 position = EmptyElectrobank.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("Electrobank"), position);
		gameObject.GetComponent<PrimaryElement>().SetElement(EmptyElectrobank.GetComponent<PrimaryElement>().Element.id, true);
		gameObject.SetActive(true);
		Storage storage = EmptyElectrobank.GetComponent<Pickupable>().storage;
		EmptyElectrobank.DeleteObject();
		if (storage != null && !dropFromStorage)
		{
			storage.Store(gameObject, false, false, true, false);
		}
		return gameObject;
	}

	// Token: 0x060060DD RID: 24797 RVA: 0x002B0F88 File Offset: 0x002AF188
	public static GameObject ReplaceChargedWithEmpty(GameObject ChargedElectrobank, bool dropFromStorage = false)
	{
		Vector3 position = ChargedElectrobank.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("EmptyElectrobank"), position);
		gameObject.GetComponent<PrimaryElement>().SetElement(ChargedElectrobank.GetComponent<PrimaryElement>().Element.id, true);
		gameObject.SetActive(true);
		Storage storage = ChargedElectrobank.GetComponent<Pickupable>().storage;
		ChargedElectrobank.DeleteObject();
		if (storage != null && !dropFromStorage)
		{
			storage.Store(gameObject, false, false, true, false);
		}
		return gameObject;
	}

	// Token: 0x060060DE RID: 24798 RVA: 0x002B1008 File Offset: 0x002AF208
	public static GameObject ReplaceEmptyWithGarbage(GameObject ChargedElectrobank, bool dropFromStorage = false)
	{
		Vector3 position = ChargedElectrobank.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("GarbageElectrobank"), position);
		gameObject.GetComponent<PrimaryElement>().SetElement(ChargedElectrobank.GetComponent<PrimaryElement>().Element.id, true);
		gameObject.SetActive(true);
		Storage storage = ChargedElectrobank.GetComponent<Pickupable>().storage;
		ChargedElectrobank.DeleteObject();
		if (storage != null && !dropFromStorage)
		{
			storage.Store(gameObject, false, false, true, false);
		}
		return gameObject;
	}

	// Token: 0x060060DF RID: 24799 RVA: 0x000DF30C File Offset: 0x000DD50C
	public void AddPower(float joules)
	{
		this.charge = Mathf.Clamp(this.charge + joules, 0f, Electrobank.capacity);
	}

	// Token: 0x060060E0 RID: 24800 RVA: 0x002B1088 File Offset: 0x002AF288
	public float RemovePower(float joules, bool dropWhenEmpty)
	{
		float num = Mathf.Min(this.charge, joules);
		this.charge -= num;
		if (this.charge <= 0f)
		{
			if (this.rechargeable)
			{
				Electrobank.ReplaceChargedWithEmpty(base.gameObject, dropWhenEmpty);
			}
			else
			{
				Util.KDestroyGameObject(base.gameObject);
			}
		}
		return num;
	}

	// Token: 0x060060E1 RID: 24801 RVA: 0x000DF32B File Offset: 0x000DD52B
	public void FullyCharge()
	{
		this.charge = Electrobank.capacity;
	}

	// Token: 0x060060E2 RID: 24802 RVA: 0x002B10E0 File Offset: 0x002AF2E0
	public void Explode()
	{
		int num = Grid.PosToCell(base.gameObject.transform.position);
		float num2 = Grid.Temperature[num];
		num2 += this.charge / (Grid.Mass[num] * Grid.Element[num].specificHeatCapacity);
		num2 = Mathf.Clamp(num2, 1f, 9999f);
		SimMessages.ReplaceElement(num, Grid.Element[num].id, CellEventLogger.Instance.SandBoxTool, Grid.Mass[num], num2, Grid.DiseaseIdx[num], Grid.DiseaseCount[num], -1);
		Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactMetal, base.gameObject.transform.position, 0f);
		KFMOD.PlayOneShot(GlobalAssets.GetSound("Battery_explode", false), base.gameObject.transform.position, 1f);
		Electrobank.ReplaceEmptyWithGarbage(base.gameObject, false);
	}

	// Token: 0x060060E3 RID: 24803 RVA: 0x002B11D8 File Offset: 0x002AF3D8
	public void Sim1000ms(float dt)
	{
		if (this.pickupable.KPrefabID.HasTag(GameTags.Stored))
		{
			return;
		}
		if (Grid.IsValidCell(this.pickupable.cachedCell) && Grid.Element[this.pickupable.cachedCell].HasTag(GameTags.AnyWater))
		{
			this.Damage(dt);
		}
	}

	// Token: 0x060060E4 RID: 24804 RVA: 0x002B1234 File Offset: 0x002AF434
	private void Damage(float amount)
	{
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, DUPLICANTS.MODIFIERS.WATERDAMAGE.NAME, base.transform, 1.5f, false);
		Game.Instance.SpawnFX(SpawnFXHashes.BuildingSpark, Grid.PosToCell(base.gameObject), 0f);
		this.health -= amount;
		if (this.health <= 0f)
		{
			this.Explode();
		}
	}

	// Token: 0x060060E5 RID: 24805 RVA: 0x002B12AC File Offset: 0x002AF4AC
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELECTROBANKS, GameUtil.GetFormattedJoules(this.Charge, "F1", GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELECTROBANKS, GameUtil.GetFormattedJoules(this.Charge, "F1", GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x170005E4 RID: 1508
	// (get) Token: 0x060060E6 RID: 24806 RVA: 0x002B1318 File Offset: 0x002AF518
	public string ConsumableId
	{
		get
		{
			return this.PrefabID().Name;
		}
	}

	// Token: 0x170005E5 RID: 1509
	// (get) Token: 0x060060E7 RID: 24807 RVA: 0x000DF338 File Offset: 0x000DD538
	public string ConsumableName
	{
		get
		{
			return this.GetProperName();
		}
	}

	// Token: 0x170005E6 RID: 1510
	// (get) Token: 0x060060E8 RID: 24808 RVA: 0x000DF340 File Offset: 0x000DD540
	public int MajorOrder
	{
		get
		{
			return 500;
		}
	}

	// Token: 0x170005E7 RID: 1511
	// (get) Token: 0x060060E9 RID: 24809 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public int MinorOrder
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x060060EA RID: 24810 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool Display
	{
		get
		{
			return true;
		}
	}

	// Token: 0x040044B7 RID: 17591
	private static float capacity = 120000f;

	// Token: 0x040044B8 RID: 17592
	[Serialize]
	private float charge = Electrobank.capacity;

	// Token: 0x040044B9 RID: 17593
	[Serialize]
	private float health = 10f;

	// Token: 0x040044BA RID: 17594
	public bool rechargeable;

	// Token: 0x040044BB RID: 17595
	[MyCmpGet]
	private Pickupable pickupable;
}
