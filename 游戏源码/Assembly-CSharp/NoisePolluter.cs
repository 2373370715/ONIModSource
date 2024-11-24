using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001665 RID: 5733
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/NoisePolluter")]
public class NoisePolluter : KMonoBehaviour, IPolluter
{
	// Token: 0x06007655 RID: 30293 RVA: 0x000EDC3E File Offset: 0x000EBE3E
	public static bool IsNoiseableCell(int cell)
	{
		return Grid.IsValidCell(cell) && (Grid.IsGas(cell) || !Grid.IsSubstantialLiquid(cell, 0.35f));
	}

	// Token: 0x06007656 RID: 30294 RVA: 0x000EDC62 File Offset: 0x000EBE62
	public void ResetCells()
	{
		if (this.radius == 0)
		{
			global::Debug.LogFormat("[{0}] has a 0 radius noise, this will disable it", new object[]
			{
				this.GetName()
			});
			return;
		}
	}

	// Token: 0x06007657 RID: 30295 RVA: 0x000EDC86 File Offset: 0x000EBE86
	public void SetAttributes(Vector2 pos, int dB, GameObject go, string name)
	{
		this.sourceName = name;
		this.noise = dB;
	}

	// Token: 0x06007658 RID: 30296 RVA: 0x000EDC97 File Offset: 0x000EBE97
	public int GetRadius()
	{
		return this.radius;
	}

	// Token: 0x06007659 RID: 30297 RVA: 0x000EDC9F File Offset: 0x000EBE9F
	public int GetNoise()
	{
		return this.noise;
	}

	// Token: 0x0600765A RID: 30298 RVA: 0x000C9F3A File Offset: 0x000C813A
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0600765B RID: 30299 RVA: 0x000EDCA7 File Offset: 0x000EBEA7
	public void SetSplat(NoiseSplat new_splat)
	{
		this.splat = new_splat;
	}

	// Token: 0x0600765C RID: 30300 RVA: 0x000EDCB0 File Offset: 0x000EBEB0
	public void Clear()
	{
		if (this.splat != null)
		{
			this.splat.Clear();
			this.splat = null;
		}
	}

	// Token: 0x0600765D RID: 30301 RVA: 0x000EDCCC File Offset: 0x000EBECC
	public Vector2 GetPosition()
	{
		return base.transform.GetPosition();
	}

	// Token: 0x17000777 RID: 1911
	// (get) Token: 0x0600765E RID: 30302 RVA: 0x000EDCDE File Offset: 0x000EBEDE
	// (set) Token: 0x0600765F RID: 30303 RVA: 0x000EDCE6 File Offset: 0x000EBEE6
	public string sourceName { get; private set; }

	// Token: 0x17000778 RID: 1912
	// (get) Token: 0x06007660 RID: 30304 RVA: 0x000EDCEF File Offset: 0x000EBEEF
	// (set) Token: 0x06007661 RID: 30305 RVA: 0x000EDCF7 File Offset: 0x000EBEF7
	public bool active { get; private set; }

	// Token: 0x06007662 RID: 30306 RVA: 0x000EDD00 File Offset: 0x000EBF00
	public void SetActive(bool active = true)
	{
		if (!active && this.splat != null)
		{
			AudioEventManager.Get().ClearNoiseSplat(this.splat);
			this.splat.Clear();
		}
		this.active = active;
	}

	// Token: 0x06007663 RID: 30307 RVA: 0x00309028 File Offset: 0x00307228
	public void Refresh()
	{
		if (this.active)
		{
			if (this.splat != null)
			{
				AudioEventManager.Get().ClearNoiseSplat(this.splat);
				this.splat.Clear();
			}
			KSelectable component = base.GetComponent<KSelectable>();
			string name = (component != null) ? component.GetName() : base.name;
			GameObject gameObject = base.GetComponent<KMonoBehaviour>().gameObject;
			this.splat = AudioEventManager.Get().CreateNoiseSplat(this.GetPosition(), this.noise, this.radius, name, gameObject);
		}
	}

	// Token: 0x06007664 RID: 30308 RVA: 0x003090B0 File Offset: 0x003072B0
	private void OnActiveChanged(object data)
	{
		bool isActive = ((Operational)data).IsActive;
		this.SetActive(isActive);
		this.Refresh();
	}

	// Token: 0x06007665 RID: 30309 RVA: 0x000EDD2F File Offset: 0x000EBF2F
	public void SetValues(EffectorValues values)
	{
		this.noise = values.amount;
		this.radius = values.radius;
	}

	// Token: 0x06007666 RID: 30310 RVA: 0x003090D8 File Offset: 0x003072D8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.radius == 0 || this.noise == 0)
		{
			global::Debug.LogWarning(string.Concat(new string[]
			{
				"Noisepollutor::OnSpawn [",
				this.GetName(),
				"] noise: [",
				this.noise.ToString(),
				"] radius: [",
				this.radius.ToString(),
				"]"
			}));
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.ResetCells();
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			base.Subscribe<NoisePolluter>(824508782, NoisePolluter.OnActiveChangedDelegate);
		}
		this.refreshCallback = new System.Action(this.Refresh);
		this.refreshPartionerCallback = delegate(object data)
		{
			this.Refresh();
		};
		this.onCollectNoisePollutersCallback = new Action<object>(this.OnCollectNoisePolluters);
		Attributes attributes = this.GetAttributes();
		Db db = Db.Get();
		this.dB = attributes.Add(db.BuildingAttributes.NoisePollution);
		this.dBRadius = attributes.Add(db.BuildingAttributes.NoisePollutionRadius);
		if (this.noise != 0 && this.radius != 0)
		{
			AttributeModifier modifier = new AttributeModifier(db.BuildingAttributes.NoisePollution.Id, (float)this.noise, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			AttributeModifier modifier2 = new AttributeModifier(db.BuildingAttributes.NoisePollutionRadius.Id, (float)this.radius, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			attributes.Add(modifier);
			attributes.Add(modifier2);
		}
		else
		{
			global::Debug.LogWarning(string.Concat(new string[]
			{
				"Noisepollutor::OnSpawn [",
				this.GetName(),
				"] radius: [",
				this.radius.ToString(),
				"] noise: [",
				this.noise.ToString(),
				"]"
			}));
		}
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		this.isMovable = (component2 != null && component2.isMovable);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "NoisePolluter.OnSpawn");
		AttributeInstance attributeInstance = this.dB;
		attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, this.refreshCallback);
		AttributeInstance attributeInstance2 = this.dBRadius;
		attributeInstance2.OnDirty = (System.Action)Delegate.Combine(attributeInstance2.OnDirty, this.refreshCallback);
		if (component != null)
		{
			this.OnActiveChanged(component.IsActive);
		}
	}

	// Token: 0x06007667 RID: 30311 RVA: 0x000EDD49 File Offset: 0x000EBF49
	private void OnCellChange()
	{
		this.Refresh();
	}

	// Token: 0x06007668 RID: 30312 RVA: 0x000EDD51 File Offset: 0x000EBF51
	private void OnCollectNoisePolluters(object data)
	{
		((List<NoisePolluter>)data).Add(this);
	}

	// Token: 0x06007669 RID: 30313 RVA: 0x000EDD5F File Offset: 0x000EBF5F
	public string GetName()
	{
		if (string.IsNullOrEmpty(this.sourceName))
		{
			this.sourceName = base.GetComponent<KSelectable>().GetName();
		}
		return this.sourceName;
	}

	// Token: 0x0600766A RID: 30314 RVA: 0x0030935C File Offset: 0x0030755C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (base.isSpawned)
		{
			if (this.dB != null)
			{
				AttributeInstance attributeInstance = this.dB;
				attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, this.refreshCallback);
				AttributeInstance attributeInstance2 = this.dBRadius;
				attributeInstance2.OnDirty = (System.Action)Delegate.Remove(attributeInstance2.OnDirty, this.refreshCallback);
			}
			if (this.isMovable)
			{
				Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
			}
		}
		if (this.splat != null)
		{
			AudioEventManager.Get().ClearNoiseSplat(this.splat);
			this.splat.Clear();
		}
	}

	// Token: 0x0600766B RID: 30315 RVA: 0x000EDD85 File Offset: 0x000EBF85
	public float GetNoiseForCell(int cell)
	{
		return this.splat.GetDBForCell(cell);
	}

	// Token: 0x0600766C RID: 30316 RVA: 0x00309408 File Offset: 0x00307608
	public List<Descriptor> GetEffectDescriptions()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.dB != null && this.dBRadius != null)
		{
			float totalValue = this.dB.GetTotalValue();
			float totalValue2 = this.dBRadius.GetTotalValue();
			string text = (this.noise > 0) ? UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_INCREASE : UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_DECREASE;
			text = text + "\n\n" + this.dB.GetAttributeValueTooltip();
			string arg = GameUtil.AddPositiveSign(totalValue.ToString(), totalValue > 0f);
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.NOISE_CREATED, arg, totalValue2), string.Format(text, arg, totalValue2), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else if (this.noise != 0)
		{
			string format = (this.noise >= 0) ? UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_INCREASE : UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_DECREASE;
			string arg2 = GameUtil.AddPositiveSign(this.noise.ToString(), this.noise > 0);
			Descriptor item2 = new Descriptor(string.Format(UI.BUILDINGEFFECTS.NOISE_CREATED, arg2, this.radius), string.Format(format, arg2, this.radius), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x0600766D RID: 30317 RVA: 0x000EDD93 File Offset: 0x000EBF93
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.GetEffectDescriptions();
	}

	// Token: 0x04005898 RID: 22680
	public const string ID = "NoisePolluter";

	// Token: 0x04005899 RID: 22681
	public int radius;

	// Token: 0x0400589A RID: 22682
	public int noise;

	// Token: 0x0400589B RID: 22683
	public AttributeInstance dB;

	// Token: 0x0400589C RID: 22684
	public AttributeInstance dBRadius;

	// Token: 0x0400589D RID: 22685
	private NoiseSplat splat;

	// Token: 0x0400589F RID: 22687
	public System.Action refreshCallback;

	// Token: 0x040058A0 RID: 22688
	public Action<object> refreshPartionerCallback;

	// Token: 0x040058A1 RID: 22689
	public Action<object> onCollectNoisePollutersCallback;

	// Token: 0x040058A2 RID: 22690
	public bool isMovable;

	// Token: 0x040058A3 RID: 22691
	[MyCmpReq]
	public OccupyArea occupyArea;

	// Token: 0x040058A5 RID: 22693
	private static readonly EventSystem.IntraObjectHandler<NoisePolluter> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<NoisePolluter>(delegate(NoisePolluter component, object data)
	{
		component.OnActiveChanged(data);
	});
}
