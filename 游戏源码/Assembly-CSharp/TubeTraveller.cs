using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001618 RID: 5656
public class TubeTraveller : GameStateMachine<TubeTraveller, TubeTraveller.Instance>
{
	// Token: 0x0600750C RID: 29964 RVA: 0x0030535C File Offset: 0x0030355C
	public void InitModifiers()
	{
		this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.Insulation.Id, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_INSULATION, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.ThermalConductivityBarrier.Id, TUNING.EQUIPMENT.SUITS.ATMOSUIT_THERMAL_CONDUCTIVITY_BARRIER, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.modifiers.Add(new AttributeModifier(Db.Get().Amounts.Bladder.deltaAttribute.Id, TUNING.EQUIPMENT.SUITS.ATMOSUIT_BLADDER, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.ScaldingThreshold.Id, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_SCALDING, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.ScoldingThreshold.Id, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_SCOLDING, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.waxSpeedBoostModifier = new AttributeModifier(Db.Get().Attributes.TransitTubeTravelSpeed.Id, DUPLICANTSTATS.STANDARD.BaseStats.TRANSIT_TUBE_TRAVEL_SPEED * 0.25f, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true);
		this.immunities.Add(Db.Get().effects.Get("SoakingWet"));
		this.immunities.Add(Db.Get().effects.Get("WetFeet"));
		this.immunities.Add(Db.Get().effects.Get("PoppedEarDrums"));
		this.immunities.Add(Db.Get().effects.Get("MinorIrritation"));
		this.immunities.Add(Db.Get().effects.Get("MajorIrritation"));
	}

	// Token: 0x0600750D RID: 29965 RVA: 0x000ECE4B File Offset: 0x000EB04B
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		this.InitModifiers();
		default_state = this.root;
		this.root.DoNothing();
	}

	// Token: 0x0600750E RID: 29966 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x0600750F RID: 29967 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06007510 RID: 29968 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
	{
		return false;
	}

	// Token: 0x06007511 RID: 29969 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool ShouldEmitCO2()
	{
		return false;
	}

	// Token: 0x06007512 RID: 29970 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool ShouldStoreCO2()
	{
		return false;
	}

	// Token: 0x040057A3 RID: 22435
	private List<Effect> immunities = new List<Effect>();

	// Token: 0x040057A4 RID: 22436
	private List<AttributeModifier> modifiers = new List<AttributeModifier>();

	// Token: 0x040057A5 RID: 22437
	private AttributeModifier waxSpeedBoostModifier;

	// Token: 0x040057A6 RID: 22438
	private const float WaxSpeedBoost = 0.25f;

	// Token: 0x02001619 RID: 5657
	public new class Instance : GameStateMachine<TubeTraveller, TubeTraveller.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06007514 RID: 29972 RVA: 0x000ECE85 File Offset: 0x000EB085
		public int prefabInstanceID
		{
			get
			{
				return base.GetComponent<Navigator>().gameObject.GetComponent<KPrefabID>().InstanceID;
			}
		}

		// Token: 0x06007515 RID: 29973 RVA: 0x000ECE9C File Offset: 0x000EB09C
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06007516 RID: 29974 RVA: 0x000ECEB0 File Offset: 0x000EB0B0
		public void OnPathAdvanced(object data)
		{
			this.UnreserveEntrances();
			this.ReserveEntrances();
		}

		// Token: 0x06007517 RID: 29975 RVA: 0x0030555C File Offset: 0x0030375C
		public void ReserveEntrances()
		{
			PathFinder.Path path = base.GetComponent<Navigator>().path;
			if (path.nodes == null)
			{
				return;
			}
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				if (path.nodes[i].navType == NavType.Floor && path.nodes[i + 1].navType == NavType.Tube)
				{
					int cell = path.nodes[i].cell;
					if (Grid.HasUsableTubeEntrance(cell, this.prefabInstanceID))
					{
						GameObject gameObject = Grid.Objects[cell, 1];
						if (gameObject)
						{
							TravelTubeEntrance component = gameObject.GetComponent<TravelTubeEntrance>();
							if (component)
							{
								component.Reserve(this, this.prefabInstanceID);
								this.reservations.Add(component);
							}
						}
					}
				}
			}
		}

		// Token: 0x06007518 RID: 29976 RVA: 0x00305628 File Offset: 0x00303828
		public void UnreserveEntrances()
		{
			foreach (TravelTubeEntrance travelTubeEntrance in this.reservations)
			{
				if (!(travelTubeEntrance == null))
				{
					travelTubeEntrance.Unreserve(this, this.prefabInstanceID);
				}
			}
			this.reservations.Clear();
		}

		// Token: 0x06007519 RID: 29977 RVA: 0x00305698 File Offset: 0x00303898
		public void ApplyEnteringTubeEffects()
		{
			Effects component = base.GetComponent<Effects>();
			Attributes attributes = base.gameObject.GetAttributes();
			base.gameObject.AddTag(GameTags.InTransitTube);
			string name = GameTags.InTransitTube.Name;
			foreach (Effect effect in base.sm.immunities)
			{
				component.AddImmunity(effect, name, true);
			}
			foreach (AttributeModifier modifier in base.sm.modifiers)
			{
				attributes.Add(modifier);
			}
			if (this.isWaxed)
			{
				attributes.Add(base.sm.waxSpeedBoostModifier);
			}
			CreatureSimTemperatureTransfer component2 = base.gameObject.GetComponent<CreatureSimTemperatureTransfer>();
			if (component2 != null)
			{
				component2.RefreshRegistration();
			}
		}

		// Token: 0x0600751A RID: 29978 RVA: 0x003057A8 File Offset: 0x003039A8
		public void ClearAllEffects()
		{
			Effects component = base.GetComponent<Effects>();
			Attributes attributes = base.gameObject.GetAttributes();
			base.gameObject.RemoveTag(GameTags.InTransitTube);
			string name = GameTags.InTransitTube.Name;
			foreach (Effect effect in base.sm.immunities)
			{
				component.RemoveImmunity(effect, name);
			}
			foreach (AttributeModifier modifier in base.sm.modifiers)
			{
				attributes.Remove(modifier);
			}
			this.SetWaxState(false);
			attributes.Remove(base.sm.waxSpeedBoostModifier);
			CreatureSimTemperatureTransfer component2 = base.gameObject.GetComponent<CreatureSimTemperatureTransfer>();
			if (component2 != null)
			{
				component2.RefreshRegistration();
			}
		}

		// Token: 0x0600751B RID: 29979 RVA: 0x003058B4 File Offset: 0x00303AB4
		public void SetWaxState(bool isWaxed)
		{
			this.isWaxed = isWaxed;
			KSelectable component = base.GetComponent<KSelectable>();
			if (component != null)
			{
				if (isWaxed)
				{
					component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.WaxedForTransitTube, 0.25f);
					return;
				}
				component.RemoveStatusItem(Db.Get().DuplicantStatusItems.WaxedForTransitTube, false);
			}
		}

		// Token: 0x0600751C RID: 29980 RVA: 0x000ECEBE File Offset: 0x000EB0BE
		public void OnTubeTransition(bool nowInTube)
		{
			if (nowInTube != this.inTube)
			{
				this.inTube = nowInTube;
				base.GetComponent<Effects>();
				base.gameObject.GetAttributes();
				if (nowInTube)
				{
					this.ApplyEnteringTubeEffects();
					return;
				}
				this.ClearAllEffects();
			}
		}

		// Token: 0x040057A7 RID: 22439
		private List<TravelTubeEntrance> reservations = new List<TravelTubeEntrance>();

		// Token: 0x040057A8 RID: 22440
		public bool inTube;

		// Token: 0x040057A9 RID: 22441
		public bool isWaxed;
	}
}
