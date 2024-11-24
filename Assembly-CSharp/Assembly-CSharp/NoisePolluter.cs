﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/NoisePolluter")]
public class NoisePolluter : KMonoBehaviour, IPolluter
{
	public static bool IsNoiseableCell(int cell)
	{
		return Grid.IsValidCell(cell) && (Grid.IsGas(cell) || !Grid.IsSubstantialLiquid(cell, 0.35f));
	}

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

	public void SetAttributes(Vector2 pos, int dB, GameObject go, string name)
	{
		this.sourceName = name;
		this.noise = dB;
	}

	public int GetRadius()
	{
		return this.radius;
	}

	public int GetNoise()
	{
		return this.noise;
	}

	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	public void SetSplat(NoiseSplat new_splat)
	{
		this.splat = new_splat;
	}

	public void Clear()
	{
		if (this.splat != null)
		{
			this.splat.Clear();
			this.splat = null;
		}
	}

	public Vector2 GetPosition()
	{
		return base.transform.GetPosition();
	}

			public string sourceName { get; private set; }

			public bool active { get; private set; }

	public void SetActive(bool active = true)
	{
		if (!active && this.splat != null)
		{
			AudioEventManager.Get().ClearNoiseSplat(this.splat);
			this.splat.Clear();
		}
		this.active = active;
	}

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

	private void OnActiveChanged(object data)
	{
		bool isActive = ((Operational)data).IsActive;
		this.SetActive(isActive);
		this.Refresh();
	}

	public void SetValues(EffectorValues values)
	{
		this.noise = values.amount;
		this.radius = values.radius;
	}

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

	private void OnCellChange()
	{
		this.Refresh();
	}

	private void OnCollectNoisePolluters(object data)
	{
		((List<NoisePolluter>)data).Add(this);
	}

	public string GetName()
	{
		if (string.IsNullOrEmpty(this.sourceName))
		{
			this.sourceName = base.GetComponent<KSelectable>().GetName();
		}
		return this.sourceName;
	}

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

	public float GetNoiseForCell(int cell)
	{
		return this.splat.GetDBForCell(cell);
	}

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

	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.GetEffectDescriptions();
	}

	public const string ID = "NoisePolluter";

	public int radius;

	public int noise;

	public AttributeInstance dB;

	public AttributeInstance dBRadius;

	private NoiseSplat splat;

	public System.Action refreshCallback;

	public Action<object> refreshPartionerCallback;

	public Action<object> onCollectNoisePollutersCallback;

	public bool isMovable;

	[MyCmpReq]
	public OccupyArea occupyArea;

	private static readonly EventSystem.IntraObjectHandler<NoisePolluter> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<NoisePolluter>(delegate(NoisePolluter component, object data)
	{
		component.OnActiveChanged(data);
	});
}
