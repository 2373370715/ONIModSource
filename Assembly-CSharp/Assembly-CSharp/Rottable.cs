﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class Rottable : GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>
{
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Fresh;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.TagTransition(GameTags.Preserved, this.Preserved, false).TagTransition(GameTags.Entombed, this.Preserved, false);
		this.Fresh.ToggleStatusItem(Db.Get().CreatureStatusItems.Fresh, (Rottable.Instance smi) => smi).ParamTransition<float>(this.rotParameter, this.Stale_Pre, (Rottable.Instance smi, float p) => p <= smi.def.spoilTime - (smi.def.spoilTime - smi.def.staleTime)).Update(delegate(Rottable.Instance smi, float dt)
		{
			smi.sm.rotParameter.Set(smi.RotValue, smi, false);
		}, UpdateRate.SIM_1000ms, true).FastUpdate("Rot", Rottable.rotCB, UpdateRate.SIM_1000ms, true);
		this.Preserved.TagTransition(Rottable.PRESERVED_TAGS, this.Fresh, true).Enter("RefreshModifiers", delegate(Rottable.Instance smi)
		{
			smi.RefreshModifiers(0f);
		});
		this.Stale_Pre.Enter(delegate(Rottable.Instance smi)
		{
			smi.GoTo(this.Stale);
		});
		this.Stale.ToggleStatusItem(Db.Get().CreatureStatusItems.Stale, (Rottable.Instance smi) => smi).ParamTransition<float>(this.rotParameter, this.Fresh, (Rottable.Instance smi, float p) => p > smi.def.spoilTime - (smi.def.spoilTime - smi.def.staleTime)).ParamTransition<float>(this.rotParameter, this.Spoiled, GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.IsLTEZero).Update(delegate(Rottable.Instance smi, float dt)
		{
			smi.sm.rotParameter.Set(smi.RotValue, smi, false);
		}, UpdateRate.SIM_1000ms, false).FastUpdate("Rot", Rottable.rotCB, UpdateRate.SIM_1000ms, false);
		this.Spoiled.Enter(delegate(Rottable.Instance smi)
		{
			GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(smi.master.gameObject), 0, 0, "RotPile", Grid.SceneLayer.Ore);
			gameObject.gameObject.GetComponent<KSelectable>().SetName(UI.GAMEOBJECTEFFECTS.ROTTEN + " " + smi.master.gameObject.GetProperName());
			gameObject.transform.SetPosition(smi.master.transform.GetPosition());
			gameObject.GetComponent<PrimaryElement>().Mass = smi.master.GetComponent<PrimaryElement>().Mass;
			gameObject.GetComponent<PrimaryElement>().Temperature = smi.master.GetComponent<PrimaryElement>().Temperature;
			gameObject.SetActive(true);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, ITEMS.FOOD.ROTPILE.NAME, gameObject.transform, 1.5f, false);
			Edible component = smi.GetComponent<Edible>();
			if (component != null)
			{
				if (component.worker != null)
				{
					ChoreDriver component2 = component.worker.GetComponent<ChoreDriver>();
					if (component2 != null && component2.GetCurrentChore() != null)
					{
						component2.GetCurrentChore().Fail("food rotted");
					}
				}
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.ROTTED, "{0}", smi.gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.ROTTED_CONTEXT);
			}
			Util.KDestroyGameObject(smi.gameObject);
		});
	}

	private static string OnStaleTooltip(List<Notification> notifications, object data)
	{
		string text = "\n";
		foreach (Notification notification in notifications)
		{
			if (notification.tooltipData != null)
			{
				GameObject gameObject = (GameObject)notification.tooltipData;
				if (gameObject != null)
				{
					text = text + "\n" + gameObject.GetProperName();
				}
			}
		}
		return string.Format(MISC.NOTIFICATIONS.FOODSTALE.TOOLTIP, text);
	}

	public static void SetStatusItems(IRottable rottable)
	{
		Grid.PosToCell(rottable.gameObject);
		KSelectable component = rottable.gameObject.GetComponent<KSelectable>();
		Rottable.RotRefrigerationLevel rotRefrigerationLevel = Rottable.RefrigerationLevel(rottable);
		if (rotRefrigerationLevel != Rottable.RotRefrigerationLevel.Refrigerated)
		{
			if (rotRefrigerationLevel == Rottable.RotRefrigerationLevel.Frozen)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature, Db.Get().CreatureStatusItems.RefrigeratedFrozen, rottable);
			}
			else
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature, Db.Get().CreatureStatusItems.Unrefrigerated, rottable);
			}
		}
		else
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature, Db.Get().CreatureStatusItems.Refrigerated, rottable);
		}
		Rottable.RotAtmosphereQuality rotAtmosphereQuality = Rottable.AtmosphereQuality(rottable);
		if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Sterilizing)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, Db.Get().CreatureStatusItems.SterilizingAtmosphere, null);
			return;
		}
		if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Contaminating)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, Db.Get().CreatureStatusItems.ContaminatedAtmosphere, null);
			return;
		}
		component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, null, null);
	}

	public static bool IsInActiveFridge(IRottable rottable)
	{
		Pickupable component = rottable.gameObject.GetComponent<Pickupable>();
		if (component != null && component.storage != null)
		{
			Refrigerator component2 = component.storage.GetComponent<Refrigerator>();
			return component2 != null && component2.IsActive();
		}
		return false;
	}

	public static Rottable.RotRefrigerationLevel RefrigerationLevel(IRottable rottable)
	{
		int num = Grid.PosToCell(rottable.gameObject);
		Rottable.Instance smi = rottable.gameObject.GetSMI<Rottable.Instance>();
		PrimaryElement component = rottable.gameObject.GetComponent<PrimaryElement>();
		float num2 = component.Temperature;
		bool flag = false;
		if (!Grid.IsValidCell(num))
		{
			if (!smi.IsRottableInSpace())
			{
				return Rottable.RotRefrigerationLevel.Normal;
			}
			flag = true;
		}
		if (!flag && Grid.Element[num].id != SimHashes.Vacuum)
		{
			num2 = Mathf.Min(Grid.Temperature[num], component.Temperature);
		}
		if (num2 < rottable.PreserveTemperature)
		{
			return Rottable.RotRefrigerationLevel.Frozen;
		}
		if (num2 < rottable.RotTemperature || Rottable.IsInActiveFridge(rottable))
		{
			return Rottable.RotRefrigerationLevel.Refrigerated;
		}
		return Rottable.RotRefrigerationLevel.Normal;
	}

	public static Rottable.RotAtmosphereQuality AtmosphereQuality(IRottable rottable)
	{
		int num = Grid.PosToCell(rottable.gameObject);
		int num2 = Grid.CellAbove(num);
		if (!Grid.IsValidCell(num))
		{
			if (rottable.gameObject.GetSMI<Rottable.Instance>().IsRottableInSpace())
			{
				return Rottable.RotAtmosphereQuality.Sterilizing;
			}
			return Rottable.RotAtmosphereQuality.Normal;
		}
		else
		{
			SimHashes id = Grid.Element[num].id;
			Rottable.RotAtmosphereQuality rotAtmosphereQuality = Rottable.RotAtmosphereQuality.Normal;
			Rottable.AtmosphereModifier.TryGetValue((int)id, out rotAtmosphereQuality);
			Rottable.RotAtmosphereQuality rotAtmosphereQuality2 = Rottable.RotAtmosphereQuality.Normal;
			if (Grid.IsValidCell(num2))
			{
				SimHashes id2 = Grid.Element[num2].id;
				if (!Rottable.AtmosphereModifier.TryGetValue((int)id2, out rotAtmosphereQuality2))
				{
					rotAtmosphereQuality2 = rotAtmosphereQuality;
				}
			}
			else
			{
				rotAtmosphereQuality2 = rotAtmosphereQuality;
			}
			if (rotAtmosphereQuality == rotAtmosphereQuality2)
			{
				return rotAtmosphereQuality;
			}
			if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Contaminating || rotAtmosphereQuality2 == Rottable.RotAtmosphereQuality.Contaminating)
			{
				return Rottable.RotAtmosphereQuality.Contaminating;
			}
			if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Normal || rotAtmosphereQuality2 == Rottable.RotAtmosphereQuality.Normal)
			{
				return Rottable.RotAtmosphereQuality.Normal;
			}
			return Rottable.RotAtmosphereQuality.Sterilizing;
		}
	}

	public StateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.FloatParameter rotParameter;

	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Preserved;

	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Fresh;

	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Stale_Pre;

	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Stale;

	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Spoiled;

	private static readonly Tag[] PRESERVED_TAGS = new Tag[]
	{
		GameTags.Preserved,
		GameTags.Dehydrated,
		GameTags.Entombed
	};

	private static readonly Rottable.RotCB rotCB = new Rottable.RotCB();

	public static Dictionary<int, Rottable.RotAtmosphereQuality> AtmosphereModifier = new Dictionary<int, Rottable.RotAtmosphereQuality>
	{
		{
			721531317,
			Rottable.RotAtmosphereQuality.Contaminating
		},
		{
			1887387588,
			Rottable.RotAtmosphereQuality.Contaminating
		},
		{
			-1528777920,
			Rottable.RotAtmosphereQuality.Normal
		},
		{
			1836671383,
			Rottable.RotAtmosphereQuality.Normal
		},
		{
			1960575215,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-899515856,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1554872654,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1858722091,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			758759285,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1046145888,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1324664829,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1406916018,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-432557516,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-805366663,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			1966552544,
			Rottable.RotAtmosphereQuality.Sterilizing
		}
	};

	public class Def : StateMachine.BaseDef
	{
		public float spoilTime;

		public float staleTime;

		public float preserveTemperature = 255.15f;

		public float rotTemperature = 277.15f;
	}

	private class RotCB : UpdateBucketWithUpdater<Rottable.Instance>.IUpdater
	{
		public void Update(Rottable.Instance smi, float dt)
		{
			smi.Rot(smi, dt);
		}
	}

	public new class Instance : GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.GameInstance, IRottable
	{
						public float RotValue
		{
			get
			{
				return this.rotAmountInstance.value;
			}
			set
			{
				base.sm.rotParameter.Set(value, this, false);
				this.rotAmountInstance.SetValue(value);
			}
		}

				public float RotConstitutionPercentage
		{
			get
			{
				return this.RotValue / base.def.spoilTime;
			}
		}

				public float RotTemperature
		{
			get
			{
				return base.def.rotTemperature;
			}
		}

				public float PreserveTemperature
		{
			get
			{
				return base.def.preserveTemperature;
			}
		}

		public Instance(IStateMachineTarget master, Rottable.Def def) : base(master, def)
		{
			this.pickupable = base.gameObject.RequireComponent<Pickupable>();
			base.master.Subscribe(-2064133523, new Action<object>(this.OnAbsorb));
			base.master.Subscribe(1335436905, new Action<object>(this.OnSplitFromChunk));
			this.primaryElement = base.gameObject.GetComponent<PrimaryElement>();
			Amounts amounts = master.gameObject.GetAmounts();
			this.rotAmountInstance = amounts.Add(new AmountInstance(Db.Get().Amounts.Rot, master.gameObject));
			this.rotAmountInstance.maxAttribute.Add(new AttributeModifier("Rot", def.spoilTime, null, false, false, true));
			this.rotAmountInstance.SetValue(def.spoilTime);
			base.sm.rotParameter.Set(this.rotAmountInstance.value, base.smi, false);
			if (Rottable.Instance.unrefrigeratedModifier == null)
			{
				Rottable.Instance.unrefrigeratedModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -0.7f, DUPLICANTS.MODIFIERS.ROTTEMPERATURE.UNREFRIGERATED, false, false, true);
				Rottable.Instance.refrigeratedModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -0.2f, DUPLICANTS.MODIFIERS.ROTTEMPERATURE.REFRIGERATED, false, false, true);
				Rottable.Instance.frozenModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, --0f, DUPLICANTS.MODIFIERS.ROTTEMPERATURE.FROZEN, false, false, true);
				Rottable.Instance.contaminatedAtmosphereModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -1f, DUPLICANTS.MODIFIERS.ROTATMOSPHERE.CONTAMINATED, false, false, true);
				Rottable.Instance.normalAtmosphereModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -0.3f, DUPLICANTS.MODIFIERS.ROTATMOSPHERE.NORMAL, false, false, true);
				Rottable.Instance.sterileAtmosphereModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, --0f, DUPLICANTS.MODIFIERS.ROTATMOSPHERE.STERILE, false, false, true);
			}
			this.RefreshModifiers(0f);
		}

		[OnDeserialized]
		private void OnDeserialized()
		{
			if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 23))
			{
				this.rotAmountInstance.SetValue(this.rotAmountInstance.value * 2f);
			}
		}

		public string StateString()
		{
			string result = "";
			if (base.smi.GetCurrentState() == base.sm.Fresh)
			{
				result = Db.Get().CreatureStatusItems.Fresh.resolveStringCallback(CREATURES.STATUSITEMS.FRESH.NAME, this);
			}
			if (base.smi.GetCurrentState() == base.sm.Stale)
			{
				result = Db.Get().CreatureStatusItems.Fresh.resolveStringCallback(CREATURES.STATUSITEMS.STALE.NAME, this);
			}
			return result;
		}

		public void Rot(Rottable.Instance smi, float deltaTime)
		{
			this.RefreshModifiers(deltaTime);
			if (smi.pickupable.storage != null)
			{
				smi.pickupable.storage.Trigger(-1197125120, null);
			}
		}

		public bool IsRottableInSpace()
		{
			if (base.gameObject.GetMyWorld() == null)
			{
				Pickupable component = base.GetComponent<Pickupable>();
				if (component != null && component.storage && (component.storage.GetComponent<RocketModuleCluster>() || component.storage.GetComponent<ClusterTraveler>()))
				{
					return true;
				}
			}
			return false;
		}

		public void RefreshModifiers(float dt)
		{
			if (this.GetMaster().isNull)
			{
				return;
			}
			if (!Grid.IsValidCell(Grid.PosToCell(base.gameObject)) && !this.IsRottableInSpace())
			{
				return;
			}
			this.rotAmountInstance.deltaAttribute.ClearModifiers();
			KPrefabID component = base.GetComponent<KPrefabID>();
			if (!component.HasAnyTags(Rottable.PRESERVED_TAGS))
			{
				Rottable.RotRefrigerationLevel rotRefrigerationLevel = Rottable.RefrigerationLevel(this);
				if (rotRefrigerationLevel != Rottable.RotRefrigerationLevel.Refrigerated)
				{
					if (rotRefrigerationLevel == Rottable.RotRefrigerationLevel.Frozen)
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.frozenModifier);
					}
					else
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.unrefrigeratedModifier);
					}
				}
				else
				{
					this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.refrigeratedModifier);
				}
				Rottable.RotAtmosphereQuality rotAtmosphereQuality = Rottable.AtmosphereQuality(this);
				if (rotAtmosphereQuality != Rottable.RotAtmosphereQuality.Sterilizing)
				{
					if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Contaminating)
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.contaminatedAtmosphereModifier);
					}
					else
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.normalAtmosphereModifier);
					}
				}
				else
				{
					this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.sterileAtmosphereModifier);
				}
			}
			if (component.HasTag(Db.Get().Spices.PreservingSpice.Id))
			{
				this.rotAmountInstance.deltaAttribute.Add(Db.Get().Spices.PreservingSpice.FoodModifier);
			}
			Rottable.SetStatusItems(this);
		}

		private void OnAbsorb(object data)
		{
			Pickupable pickupable = (Pickupable)data;
			if (pickupable != null)
			{
				PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
				PrimaryElement primaryElement = pickupable.PrimaryElement;
				Rottable.Instance smi = pickupable.gameObject.GetSMI<Rottable.Instance>();
				if (component != null && primaryElement != null && smi != null)
				{
					float num = component.Units * base.sm.rotParameter.Get(base.smi);
					float num2 = primaryElement.Units * base.sm.rotParameter.Get(smi);
					float value = (num + num2) / (component.Units + primaryElement.Units);
					base.sm.rotParameter.Set(value, base.smi, false);
				}
			}
		}

		public bool IsRotLevelStackable(Rottable.Instance other)
		{
			return Mathf.Abs(this.RotConstitutionPercentage - other.RotConstitutionPercentage) < 0.1f;
		}

		public string GetToolTip()
		{
			return this.rotAmountInstance.GetTooltip();
		}

		private void OnSplitFromChunk(object data)
		{
			Pickupable pickupable = (Pickupable)data;
			if (pickupable != null)
			{
				Rottable.Instance smi = pickupable.GetSMI<Rottable.Instance>();
				if (smi != null)
				{
					this.RotValue = smi.RotValue;
				}
			}
		}

		public void OnPreserved(object data)
		{
			if ((bool)data)
			{
				base.smi.GoTo(base.sm.Preserved);
				return;
			}
			base.smi.GoTo(base.sm.Fresh);
		}

		private AmountInstance rotAmountInstance;

		private static AttributeModifier unrefrigeratedModifier;

		private static AttributeModifier refrigeratedModifier;

		private static AttributeModifier frozenModifier;

		private static AttributeModifier contaminatedAtmosphereModifier;

		private static AttributeModifier normalAtmosphereModifier;

		private static AttributeModifier sterileAtmosphereModifier;

		public PrimaryElement primaryElement;

		public Pickupable pickupable;
	}

	public enum RotAtmosphereQuality
	{
		Normal,
		Sterilizing,
		Contaminating
	}

	public enum RotRefrigerationLevel
	{
		Normal,
		Refrigerated,
		Frozen
	}
}
