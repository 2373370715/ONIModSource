using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

namespace Database
{
	// Token: 0x02002134 RID: 8500
	public class DuplicantStatusItems : StatusItems
	{
		// Token: 0x0600B515 RID: 46357 RVA: 0x00114F53 File Offset: 0x00113153
		public DuplicantStatusItems(ResourceSet parent) : base("DuplicantStatusItems", parent)
		{
			this.CreateStatusItems();
		}

		// Token: 0x0600B516 RID: 46358 RVA: 0x0043DD94 File Offset: 0x0043BF94
		private StatusItem CreateStatusItem(string id, string prefix, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool showWorldIcon = true, int status_overlays = 2)
		{
			return base.Add(new StatusItem(id, prefix, icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, null));
		}

		// Token: 0x0600B517 RID: 46359 RVA: 0x0043DDBC File Offset: 0x0043BFBC
		private StatusItem CreateStatusItem(string id, string name, string tooltip, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, int status_overlays = 2)
		{
			return base.Add(new StatusItem(id, name, tooltip, icon, icon_type, notification_type, allow_multiples, render_overlay, status_overlays, true, null));
		}

		// Token: 0x0600B518 RID: 46360 RVA: 0x0044B4B8 File Offset: 0x004496B8
		private void CreateStatusItems()
		{
			Func<string, object, string> resolveStringCallback = delegate(string str, object data)
			{
				Workable workable = (Workable)data;
				if (workable != null && workable.GetComponent<KSelectable>() != null)
				{
					str = str.Replace("{Target}", workable.GetComponent<KSelectable>().GetName());
				}
				return str;
			};
			Func<string, object, string> resolveStringCallback2 = delegate(string str, object data)
			{
				Workable workable = (Workable)data;
				if (workable != null)
				{
					str = str.Replace("{Target}", workable.GetComponent<KSelectable>().GetName());
					ComplexFabricatorWorkable complexFabricatorWorkable = workable as ComplexFabricatorWorkable;
					if (complexFabricatorWorkable != null)
					{
						ComplexRecipe currentWorkingOrder = complexFabricatorWorkable.CurrentWorkingOrder;
						if (currentWorkingOrder != null)
						{
							str = str.Replace("{Item}", currentWorkingOrder.FirstResult.ProperName());
						}
					}
				}
				return str;
			};
			this.BedUnreachable = this.CreateStatusItem("BedUnreachable", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.BedUnreachable.AddNotification(null, null, null);
			this.DailyRationLimitReached = this.CreateStatusItem("DailyRationLimitReached", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.DailyRationLimitReached.AddNotification(null, null, null);
			this.HoldingBreath = this.CreateStatusItem("HoldingBreath", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Hungry = this.CreateStatusItem("Hungry", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Slippering = this.CreateStatusItem("Slippering", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Unhappy = this.CreateStatusItem("Unhappy", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Unhappy.AddNotification(null, null, null);
			this.NervousBreakdown = this.CreateStatusItem("NervousBreakdown", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.NervousBreakdown.AddNotification(null, null, null);
			this.NoRationsAvailable = this.CreateStatusItem("NoRationsAvailable", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.PendingPacification = this.CreateStatusItem("PendingPacification", "DUPLICANTS", "status_item_pending_pacification", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.QuarantineAreaUnassigned = this.CreateStatusItem("QuarantineAreaUnassigned", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.QuarantineAreaUnassigned.AddNotification(null, null, null);
			this.QuarantineAreaUnreachable = this.CreateStatusItem("QuarantineAreaUnreachable", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.QuarantineAreaUnreachable.AddNotification(null, null, null);
			this.Quarantined = this.CreateStatusItem("Quarantined", "DUPLICANTS", "status_item_quarantined", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.RationsUnreachable = this.CreateStatusItem("RationsUnreachable", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.RationsUnreachable.AddNotification(null, null, null);
			this.RationsNotPermitted = this.CreateStatusItem("RationsNotPermitted", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.RationsNotPermitted.AddNotification(null, null, null);
			this.Rotten = this.CreateStatusItem("Rotten", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Starving = this.CreateStatusItem("Starving", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.Starving.AddNotification(null, null, null);
			this.Suffocating = this.CreateStatusItem("Suffocating", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.DuplicantThreatening, false, OverlayModes.None.ID, true, 2);
			this.Suffocating.AddNotification(null, null, null);
			this.Tired = this.CreateStatusItem("Tired", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Idle = this.CreateStatusItem("Idle", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.IdleInRockets = this.CreateStatusItem("IdleInRockets", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Pacified = this.CreateStatusItem("Pacified", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Dead = this.CreateStatusItem("Dead", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Dead.resolveStringCallback = delegate(string str, object data)
			{
				Death death = (Death)data;
				return str.Replace("{Death}", death.Name);
			};
			this.MoveToSuitNotRequired = this.CreateStatusItem("MoveToSuitNotRequired", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.DroppingUnusedInventory = this.CreateStatusItem("DroppingUnusedInventory", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.MovingToSafeArea = this.CreateStatusItem("MovingToSafeArea", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.ToiletUnreachable = this.CreateStatusItem("ToiletUnreachable", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.ToiletUnreachable.AddNotification(null, null, null);
			this.NoUsableToilets = this.CreateStatusItem("NoUsableToilets", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.NoUsableToilets.AddNotification(null, null, null);
			this.NoToilets = this.CreateStatusItem("NoToilets", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.NoToilets.AddNotification(null, null, null);
			this.BreathingO2 = this.CreateStatusItem("BreathingO2", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 130);
			this.BreathingO2.resolveStringCallback = delegate(string str, object data)
			{
				OxygenBreather oxygenBreather = (OxygenBreather)data;
				float averageRate = Game.Instance.accumulators.GetAverageRate(oxygenBreather.O2Accumulator);
				return str.Replace("{ConsumptionRate}", GameUtil.GetFormattedMass(-averageRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.EmittingCO2 = this.CreateStatusItem("EmittingCO2", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 130);
			this.EmittingCO2.resolveStringCallback = delegate(string str, object data)
			{
				OxygenBreather oxygenBreather = (OxygenBreather)data;
				return str.Replace("{EmittingRate}", GameUtil.GetFormattedMass(oxygenBreather.CO2EmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.Vomiting = this.CreateStatusItem("Vomiting", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Coughing = this.CreateStatusItem("Coughing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.LowOxygen = this.CreateStatusItem("LowOxygen", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.LowOxygen.AddNotification(null, null, null);
			this.RedAlert = this.CreateStatusItem("RedAlert", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Dreaming = this.CreateStatusItem("Dreaming", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Sleeping = this.CreateStatusItem("Sleeping", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Sleeping.resolveTooltipCallback = delegate(string str, object data)
			{
				if (data is SleepChore.StatesInstance)
				{
					string stateChangeNoiseSource = ((SleepChore.StatesInstance)data).stateChangeNoiseSource;
					if (!string.IsNullOrEmpty(stateChangeNoiseSource))
					{
						string text = DUPLICANTS.STATUSITEMS.SLEEPING.TOOLTIP;
						text = text.Replace("{Disturber}", stateChangeNoiseSource);
						str += text;
					}
				}
				return str;
			};
			this.SleepingExhausted = this.CreateStatusItem("SleepingExhausted", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.SleepingInterruptedByNoise = this.CreateStatusItem("SleepingInterruptedByNoise", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.SleepingInterruptedByLight = this.CreateStatusItem("SleepingInterruptedByLight", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.SleepingInterruptedByFearOfDark = this.CreateStatusItem("SleepingInterruptedByFearOfDark", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.SleepingInterruptedByMovement = this.CreateStatusItem("SleepingInterruptedByMovement", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.SleepingInterruptedByCold = this.CreateStatusItem("SleepingInterruptedByCold", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Eating = this.CreateStatusItem("Eating", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Eating.resolveStringCallback = resolveStringCallback;
			this.Digging = this.CreateStatusItem("Digging", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Cleaning = this.CreateStatusItem("Cleaning", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Cleaning.resolveStringCallback = resolveStringCallback;
			this.PickingUp = this.CreateStatusItem("PickingUp", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.PickingUp.resolveStringCallback = resolveStringCallback;
			this.Mopping = this.CreateStatusItem("Mopping", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Cooking = this.CreateStatusItem("Cooking", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Cooking.resolveStringCallback = resolveStringCallback2;
			this.Mushing = this.CreateStatusItem("Mushing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Mushing.resolveStringCallback = resolveStringCallback2;
			this.Researching = this.CreateStatusItem("Researching", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Researching.resolveStringCallback = delegate(string str, object data)
			{
				TechInstance activeResearch = Research.Instance.GetActiveResearch();
				if (activeResearch != null)
				{
					return str.Replace("{Tech}", activeResearch.tech.Name);
				}
				return str;
			};
			this.ResearchingFromPOI = this.CreateStatusItem("ResearchingFromPOI", DUPLICANTS.STATUSITEMS.RESEARCHING_FROM_POI.NAME, DUPLICANTS.STATUSITEMS.RESEARCHING_FROM_POI.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.MissionControlling = this.CreateStatusItem("MissionControlling", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Tinkering = this.CreateStatusItem("Tinkering", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Tinkering.resolveStringCallback = delegate(string str, object data)
			{
				Tinkerable tinkerable = (Tinkerable)data;
				if (tinkerable != null)
				{
					return string.Format(str, tinkerable.tinkerMaterialTag.ProperName());
				}
				return str;
			};
			this.Storing = this.CreateStatusItem("Storing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Storing.resolveStringCallback = delegate(string str, object data)
			{
				Workable workable = (Workable)data;
				if (workable != null && workable.worker as StandardWorker != null)
				{
					KSelectable component = workable.GetComponent<KSelectable>();
					if (component)
					{
						str = str.Replace("{Target}", component.GetName());
					}
					Pickupable pickupable = (workable.worker as StandardWorker).workCompleteData as Pickupable;
					if (workable.worker != null && pickupable)
					{
						KSelectable component2 = pickupable.GetComponent<KSelectable>();
						if (component2)
						{
							str = str.Replace("{Item}", component2.GetName());
						}
					}
				}
				return str;
			};
			this.Building = this.CreateStatusItem("Building", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Building.resolveStringCallback = resolveStringCallback;
			this.Equipping = this.CreateStatusItem("Equipping", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Equipping.resolveStringCallback = resolveStringCallback;
			this.WarmingUp = this.CreateStatusItem("WarmingUp", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.WarmingUp.resolveStringCallback = resolveStringCallback;
			this.GeneratingPower = this.CreateStatusItem("GeneratingPower", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.GeneratingPower.resolveStringCallback = resolveStringCallback;
			this.Harvesting = this.CreateStatusItem("Harvesting", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Ranching = this.CreateStatusItem("Ranching", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Harvesting.resolveStringCallback = resolveStringCallback;
			this.Uprooting = this.CreateStatusItem("Uprooting", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Uprooting.resolveStringCallback = resolveStringCallback;
			this.Emptying = this.CreateStatusItem("Emptying", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Emptying.resolveStringCallback = resolveStringCallback;
			this.Toggling = this.CreateStatusItem("Toggling", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Toggling.resolveStringCallback = resolveStringCallback;
			this.Deconstructing = this.CreateStatusItem("Deconstructing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Deconstructing.resolveStringCallback = resolveStringCallback;
			this.Disinfecting = this.CreateStatusItem("Disinfecting", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Disinfecting.resolveStringCallback = resolveStringCallback;
			this.Upgrading = this.CreateStatusItem("Upgrading", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Upgrading.resolveStringCallback = resolveStringCallback;
			this.Fabricating = this.CreateStatusItem("Fabricating", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Fabricating.resolveStringCallback = resolveStringCallback2;
			this.Processing = this.CreateStatusItem("Processing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Processing.resolveStringCallback = resolveStringCallback2;
			this.Spicing = this.CreateStatusItem("Spicing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Clearing = this.CreateStatusItem("Clearing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Clearing.resolveStringCallback = resolveStringCallback;
			this.GeneratingPower = this.CreateStatusItem("GeneratingPower", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.GeneratingPower.resolveStringCallback = resolveStringCallback;
			this.Cold = this.CreateStatusItem("Cold", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Cold.resolveTooltipCallback = delegate(string str, object data)
			{
				ExternalTemperatureMonitor.Instance smi = ((ColdImmunityMonitor.Instance)data).GetSMI<ExternalTemperatureMonitor.Instance>();
				str = str.Replace("{StressModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("ColdAir").SelfModifiers[0].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{StaminaModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("ColdAir").SelfModifiers[1].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{AthleticsModification}", Db.Get().effects.Get("ColdAir").SelfModifiers[2].Value.ToString());
				float dtu_s = smi.temperatureTransferer.average_kilowatts_exchanged.GetUnweightedAverage * 1000f;
				str = str.Replace("{currentTransferWattage}", GameUtil.GetFormattedHeatEnergyRate(dtu_s, GameUtil.HeatEnergyFormatterUnit.Automatic));
				AttributeInstance attributeInstance = smi.attributes.Get("ThermalConductivityBarrier");
				string text = "<b>" + attributeInstance.GetFormattedValue() + "</b>";
				for (int num = 0; num != attributeInstance.Modifiers.Count; num++)
				{
					AttributeModifier attributeModifier = attributeInstance.Modifiers[num];
					text += "\n";
					text = string.Concat(new string[]
					{
						text,
						"    • ",
						attributeModifier.GetDescription(),
						" <b>",
						attributeModifier.GetFormattedString(),
						"</b>"
					});
				}
				str = str.Replace("{conductivityBarrier}", text);
				return str;
			};
			this.ExitingCold = this.CreateStatusItem("ExitingCold", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.ExitingCold.resolveTooltipCallback = delegate(string str, object data)
			{
				ColdImmunityMonitor.Instance instance = (ColdImmunityMonitor.Instance)data;
				str = str.Replace("{0}", GameUtil.GetFormattedTime(instance.ColdCountdown, "F0"));
				str = str.Replace("{StressModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("ColdAir").SelfModifiers[0].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{StaminaModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("ColdAir").SelfModifiers[1].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{AthleticsModification}", Db.Get().effects.Get("ColdAir").SelfModifiers[2].Value.ToString());
				return str;
			};
			this.Hot = this.CreateStatusItem("Hot", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Hot.resolveTooltipCallback = delegate(string str, object data)
			{
				ExternalTemperatureMonitor.Instance smi = ((HeatImmunityMonitor.Instance)data).GetSMI<ExternalTemperatureMonitor.Instance>();
				str = str.Replace("{StressModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("WarmAir").SelfModifiers[0].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{StaminaModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("WarmAir").SelfModifiers[1].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{AthleticsModification}", Db.Get().effects.Get("WarmAir").SelfModifiers[2].Value.ToString());
				float dtu_s = smi.temperatureTransferer.average_kilowatts_exchanged.GetUnweightedAverage * 1000f;
				str = str.Replace("{currentTransferWattage}", GameUtil.GetFormattedHeatEnergyRate(dtu_s, GameUtil.HeatEnergyFormatterUnit.Automatic));
				AttributeInstance attributeInstance = smi.attributes.Get("ThermalConductivityBarrier");
				string text = "<b>" + attributeInstance.GetFormattedValue() + "</b>";
				for (int num = 0; num != attributeInstance.Modifiers.Count; num++)
				{
					AttributeModifier attributeModifier = attributeInstance.Modifiers[num];
					text += "\n";
					text = string.Concat(new string[]
					{
						text,
						"    • ",
						attributeModifier.GetDescription(),
						" <b>",
						attributeModifier.GetFormattedString(),
						"</b>"
					});
				}
				str = str.Replace("{conductivityBarrier}", text);
				return str;
			};
			this.ExitingHot = this.CreateStatusItem("ExitingHot", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.ExitingHot.resolveTooltipCallback = delegate(string str, object data)
			{
				HeatImmunityMonitor.Instance instance = (HeatImmunityMonitor.Instance)data;
				str = str.Replace("{0}", GameUtil.GetFormattedTime(instance.HeatCountdown, "F0"));
				str = str.Replace("{StressModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("WarmAir").SelfModifiers[0].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{StaminaModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("WarmAir").SelfModifiers[1].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{AthleticsModification}", Db.Get().effects.Get("WarmAir").SelfModifiers[2].Value.ToString());
				return str;
			};
			this.BodyRegulatingHeating = this.CreateStatusItem("BodyRegulatingHeating", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BodyRegulatingHeating.resolveStringCallback = delegate(string str, object data)
			{
				WarmBlooded.StatesInstance statesInstance = (WarmBlooded.StatesInstance)data;
				return str.Replace("{TempDelta}", GameUtil.GetFormattedTemperature(statesInstance.TemperatureDelta, GameUtil.TimeSlice.PerSecond, GameUtil.TemperatureInterpretation.Relative, true, false));
			};
			this.BodyRegulatingCooling = this.CreateStatusItem("BodyRegulatingCooling", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BodyRegulatingCooling.resolveStringCallback = this.BodyRegulatingHeating.resolveStringCallback;
			this.EntombedChore = this.CreateStatusItem("EntombedChore", "DUPLICANTS", "status_item_entombed", StatusItem.IconType.Custom, NotificationType.DuplicantThreatening, false, OverlayModes.None.ID, true, 2);
			this.EntombedChore.AddNotification(null, null, null);
			this.EarlyMorning = this.CreateStatusItem("EarlyMorning", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.NightTime = this.CreateStatusItem("NightTime", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.PoorDecor = this.CreateStatusItem("PoorDecor", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.PoorQualityOfLife = this.CreateStatusItem("PoorQualityOfLife", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.PoorFoodQuality = this.CreateStatusItem("PoorFoodQuality", DUPLICANTS.STATUSITEMS.POOR_FOOD_QUALITY.NAME, DUPLICANTS.STATUSITEMS.POOR_FOOD_QUALITY.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.GoodFoodQuality = this.CreateStatusItem("GoodFoodQuality", DUPLICANTS.STATUSITEMS.GOOD_FOOD_QUALITY.NAME, DUPLICANTS.STATUSITEMS.GOOD_FOOD_QUALITY.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.Arting = this.CreateStatusItem("Arting", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Arting.resolveStringCallback = resolveStringCallback;
			this.SevereWounds = this.CreateStatusItem("SevereWounds", "DUPLICANTS", "status_item_broken", StatusItem.IconType.Custom, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.SevereWounds.AddNotification(null, null, null);
			this.BionicOfflineIncapacitated = this.CreateStatusItem("BionicOfflineIncapacitated", "DUPLICANTS", "status_electrobank", StatusItem.IconType.Custom, NotificationType.DuplicantThreatening, false, OverlayModes.None.ID, true, 2);
			this.BionicOfflineIncapacitated.AddNotification(null, null, null);
			this.BionicWantsOilChange = this.CreateStatusItem("BionicWantsOilChange", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BionicWaitingForReboot = this.CreateStatusItem("BionicWaitingForReboot", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BionicBeingRebooted = this.CreateStatusItem("BionicBeingRebooted", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BionicRequiresSkillPerk = this.CreateStatusItem("BionicRequiresSkillPerk", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BionicRequiresSkillPerk.resolveStringCallback = delegate(string str, object data)
			{
				string id = (string)data;
				SkillPerk perk = Db.Get().SkillPerks.Get(id);
				List<Skill> skillsWithPerk = Db.Get().Skills.GetSkillsWithPerk(perk);
				List<string> list = new List<string>();
				foreach (Skill skill in skillsWithPerk)
				{
					if (!skill.deprecated)
					{
						list.Add(skill.Name);
					}
				}
				str = str.Replace("{Skills}", string.Join(", ", list.ToArray()));
				return str;
			};
			this.Incapacitated = this.CreateStatusItem("Incapacitated", "DUPLICANTS", "status_item_broken", StatusItem.IconType.Custom, NotificationType.DuplicantThreatening, false, OverlayModes.None.ID, true, 2);
			this.Incapacitated.AddNotification(null, null, null);
			this.Incapacitated.resolveStringCallback = delegate(string str, object data)
			{
				IncapacitationMonitor.Instance instance = (IncapacitationMonitor.Instance)data;
				float bleedLifeTime = instance.GetBleedLifeTime(instance);
				str = str.Replace("{CauseOfIncapacitation}", instance.GetCauseOfIncapacitation().Name);
				return str.Replace("{TimeUntilDeath}", GameUtil.GetFormattedTime(bleedLifeTime, "F0"));
			};
			this.Relocating = this.CreateStatusItem("Relocating", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Relocating.resolveStringCallback = resolveStringCallback;
			this.Fighting = this.CreateStatusItem("Fighting", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.Fighting.AddNotification(null, null, null);
			this.Fleeing = this.CreateStatusItem("Fleeing", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.Fleeing.AddNotification(null, null, null);
			this.Stressed = this.CreateStatusItem("Stressed", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Stressed.AddNotification(null, null, null);
			this.LashingOut = this.CreateStatusItem("LashingOut", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.LashingOut.AddNotification(null, null, null);
			this.LowImmunity = this.CreateStatusItem("LowImmunity", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.LowImmunity.AddNotification(null, null, null);
			this.Studying = this.CreateStatusItem("Studying", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.InstallingElectrobank = this.CreateStatusItem("InstallingElectrobank", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Socializing = this.CreateStatusItem("Socializing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.Mingling = this.CreateStatusItem("Mingling", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.BionicExplorerBooster = this.CreateStatusItem("BionicExplorerBooster", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 2);
			this.BionicExplorerBooster.resolveStringCallback = delegate(string str, object data)
			{
				BionicUpgrade_ExplorerBoosterMonitor.Instance instance = (BionicUpgrade_ExplorerBoosterMonitor.Instance)data;
				str = string.Format(str, GameUtil.GetFormattedPercent(instance.CurrentProgress * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.ContactWithGerms = this.CreateStatusItem("ContactWithGerms", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.Disease.ID, true, 2);
			this.ContactWithGerms.resolveStringCallback = delegate(string str, object data)
			{
				GermExposureMonitor.ExposureStatusData exposureStatusData = (GermExposureMonitor.ExposureStatusData)data;
				string name = Db.Get().Sicknesses.Get(exposureStatusData.exposure_type.sickness_id).Name;
				str = str.Replace("{Sickness}", name);
				return str;
			};
			this.ContactWithGerms.statusItemClickCallback = delegate(object data)
			{
				GermExposureMonitor.ExposureStatusData exposureStatusData = (GermExposureMonitor.ExposureStatusData)data;
				Vector3 lastExposurePosition = exposureStatusData.owner.GetLastExposurePosition(exposureStatusData.exposure_type.germ_id);
				CameraController.Instance.CameraGoTo(lastExposurePosition, 2f, true);
				if (OverlayScreen.Instance.mode == OverlayModes.None.ID)
				{
					OverlayScreen.Instance.ToggleOverlay(OverlayModes.Disease.ID, true);
				}
			};
			this.ExposedToGerms = this.CreateStatusItem("ExposedToGerms", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.Disease.ID, true, 2);
			this.ExposedToGerms.resolveStringCallback = delegate(string str, object data)
			{
				GermExposureMonitor.ExposureStatusData exposureStatusData = (GermExposureMonitor.ExposureStatusData)data;
				string name = Db.Get().Sicknesses.Get(exposureStatusData.exposure_type.sickness_id).Name;
				AttributeInstance attributeInstance = Db.Get().Attributes.GermResistance.Lookup(exposureStatusData.owner.gameObject);
				string lastDiseaseSource = exposureStatusData.owner.GetLastDiseaseSource(exposureStatusData.exposure_type.germ_id);
				GermExposureMonitor.Instance smi = exposureStatusData.owner.GetSMI<GermExposureMonitor.Instance>();
				float num = (float)exposureStatusData.exposure_type.base_resistance + GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[0];
				float totalValue = attributeInstance.GetTotalValue();
				float resistanceToExposureType = smi.GetResistanceToExposureType(exposureStatusData.exposure_type, -1f);
				float contractionChance = GermExposureMonitor.GetContractionChance(resistanceToExposureType);
				float exposureTier = smi.GetExposureTier(exposureStatusData.exposure_type.germ_id);
				float num2 = GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[(int)exposureTier - 1] - GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[0];
				str = str.Replace("{Severity}", DUPLICANTS.STATUSITEMS.EXPOSEDTOGERMS.EXPOSURE_TIERS[(int)exposureTier - 1].ToString());
				str = str.Replace("{Sickness}", name);
				str = str.Replace("{Source}", lastDiseaseSource);
				str = str.Replace("{Base}", GameUtil.GetFormattedSimple(num, GameUtil.TimeSlice.None, null));
				str = str.Replace("{Dupe}", GameUtil.GetFormattedSimple(totalValue, GameUtil.TimeSlice.None, null));
				str = str.Replace("{Total}", GameUtil.GetFormattedSimple(resistanceToExposureType, GameUtil.TimeSlice.None, null));
				str = str.Replace("{ExposureLevelBonus}", GameUtil.GetFormattedSimple(num2, GameUtil.TimeSlice.None, null));
				str = str.Replace("{Chance}", GameUtil.GetFormattedPercent(contractionChance * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.ExposedToGerms.statusItemClickCallback = delegate(object data)
			{
				GermExposureMonitor.ExposureStatusData exposureStatusData = (GermExposureMonitor.ExposureStatusData)data;
				Vector3 lastExposurePosition = exposureStatusData.owner.GetLastExposurePosition(exposureStatusData.exposure_type.germ_id);
				CameraController.Instance.CameraGoTo(lastExposurePosition, 2f, true);
				if (OverlayScreen.Instance.mode == OverlayModes.None.ID)
				{
					OverlayScreen.Instance.ToggleOverlay(OverlayModes.Disease.ID, true);
				}
			};
			this.LightWorkEfficiencyBonus = this.CreateStatusItem("LightWorkEfficiencyBonus", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.LightWorkEfficiencyBonus.resolveStringCallback = delegate(string str, object data)
			{
				string arg = string.Format(DUPLICANTS.STATUSITEMS.LIGHTWORKEFFICIENCYBONUS.NO_BUILDING_WORK_ATTRIBUTE, GameUtil.AddPositiveSign(GameUtil.GetFormattedPercent(DUPLICANTSTATS.STANDARD.Light.LIGHT_WORK_EFFICIENCY_BONUS * 100f, GameUtil.TimeSlice.None), true));
				return string.Format(str, arg);
			};
			this.LaboratoryWorkEfficiencyBonus = this.CreateStatusItem("LaboratoryWorkEfficiencyBonus", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.LaboratoryWorkEfficiencyBonus.resolveStringCallback = delegate(string str, object data)
			{
				string arg = string.Format(DUPLICANTS.STATUSITEMS.LABORATORYWORKEFFICIENCYBONUS.NO_BUILDING_WORK_ATTRIBUTE, GameUtil.AddPositiveSign(GameUtil.GetFormattedPercent(10f, GameUtil.TimeSlice.None), true));
				return string.Format(str, arg);
			};
			this.BeingProductive = this.CreateStatusItem("BeingProductive", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BalloonArtistPlanning = this.CreateStatusItem("BalloonArtistPlanning", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BalloonArtistHandingOut = this.CreateStatusItem("BalloonArtistHandingOut", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Partying = this.CreateStatusItem("Partying", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.DataRainerPlanning = this.CreateStatusItem("DataRainerPlanning", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.DataRainerRaining = this.CreateStatusItem("DataRainerRaining", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.RoboDancerPlanning = this.CreateStatusItem("RoboDancerPlanning", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.RoboDancerDancing = this.CreateStatusItem("RoboDancerDancing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.GasLiquidIrritation = this.CreateStatusItem("GasLiquidIrritated", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.GasLiquidIrritation.resolveStringCallback = ((string str, object data) => ((GasLiquidExposureMonitor.Instance)data).IsMajorIrritation() ? DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.NAME_MAJOR : DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.NAME_MINOR);
			this.GasLiquidIrritation.resolveTooltipCallback = delegate(string str, object data)
			{
				GasLiquidExposureMonitor.Instance instance = (GasLiquidExposureMonitor.Instance)data;
				string text = DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP;
				string text2 = "";
				Effect appliedEffect = instance.sm.GetAppliedEffect(instance);
				if (appliedEffect != null)
				{
					text2 = Effect.CreateTooltip(appliedEffect, false, "\n    • ", true);
				}
				string text3 = DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP_EXPOSED.Replace("{element}", instance.CurrentlyExposedToElement().name);
				float currentExposure = instance.sm.GetCurrentExposure(instance);
				if (currentExposure < 0f)
				{
					text3 = text3.Replace("{rate}", DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP_RATE_DECREASE);
				}
				else if (currentExposure > 0f)
				{
					text3 = text3.Replace("{rate}", DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP_RATE_INCREASE);
				}
				else
				{
					text3 = text3.Replace("{rate}", DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP_RATE_STAYS);
				}
				float seconds = (instance.exposure - instance.minorIrritationThreshold) / Math.Abs(instance.exposureRate);
				string text4 = DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP_EXPOSURE_LEVEL.Replace("{time}", GameUtil.GetFormattedTime(seconds, "F0"));
				return string.Concat(new string[]
				{
					text,
					"\n\n",
					text2,
					"\n\n",
					text3,
					"\n\n",
					text4
				});
			};
			this.ExpellingRads = this.CreateStatusItem("ExpellingRads", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.AnalyzingGenes = this.CreateStatusItem("AnalyzingGenes", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.AnalyzingArtifact = this.CreateStatusItem("AnalyzingArtifact", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.MegaBrainTank_Pajamas_Wearing = this.CreateStatusItem("MegaBrainTank_Pajamas_Wearing", DUPLICANTS.STATUSITEMS.WEARING_PAJAMAS.NAME, DUPLICANTS.STATUSITEMS.WEARING_PAJAMAS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.MegaBrainTank_Pajamas_Wearing.resolveTooltipCallback_shouldStillCallIfDataIsNull = true;
			this.MegaBrainTank_Pajamas_Wearing.resolveTooltipCallback = delegate(string str, object data)
			{
				string str2 = DUPLICANTS.STATUSITEMS.WEARING_PAJAMAS.TOOLTIP;
				Effect effect = Db.Get().effects.Get("SleepClinic");
				string str3;
				if (effect != null)
				{
					str3 = Effect.CreateTooltip(effect, false, "\n    • ", true);
				}
				else
				{
					str3 = "";
				}
				return str2 + "\n\n" + str3;
			};
			this.MegaBrainTank_Pajamas_Sleeping = this.CreateStatusItem("MegaBrainTank_Pajamas_Sleeping", DUPLICANTS.STATUSITEMS.DREAMING.NAME, DUPLICANTS.STATUSITEMS.DREAMING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.MegaBrainTank_Pajamas_Sleeping.resolveTooltipCallback = delegate(string str, object data)
			{
				ClinicDreamable clinicDreamable = (ClinicDreamable)data;
				return str.Replace("{time}", GameUtil.GetFormattedTime(clinicDreamable.WorkTimeRemaining, "F0"));
			};
			this.FossilHunt_WorkerExcavating = this.CreateStatusItem("FossilHunt_WorkerExcavating", DUPLICANTS.STATUSITEMS.FOSSILHUNT.WORKEREXCAVATING.NAME, DUPLICANTS.STATUSITEMS.FOSSILHUNT.WORKEREXCAVATING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.MorbRoverMakerWorkingOnRevealing = this.CreateStatusItem("MorbRoverMakerWorkingOnRevealing", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.BUILDING_REVEALING.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.BUILDING_REVEALING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.MorbRoverMakerDoctorWorking = this.CreateStatusItem("MorbRoverMakerDoctorWorking", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.DOCTOR_WORKING_BUILDING.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.DOCTOR_WORKING_BUILDING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.ArmingTrap = this.CreateStatusItem("ArmingTrap", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.WaxedForTransitTube = this.CreateStatusItem("WaxedForTransitTube", "DUPLICANTS", "action_speed_up", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.WaxedForTransitTube.resolveTooltipCallback = delegate(string str, object data)
			{
				float percent = (float)data * 100f;
				return str.Replace("{0}", GameUtil.GetFormattedPercent(percent, GameUtil.TimeSlice.None));
			};
			this.JoyResponse_HasBalloon = this.CreateStatusItem("JoyResponse_HasBalloon", DUPLICANTS.MODIFIERS.HASBALLOON.NAME, DUPLICANTS.MODIFIERS.HASBALLOON.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.JoyResponse_HasBalloon.resolveTooltipCallback = delegate(string str, object data)
			{
				EquippableBalloon.StatesInstance statesInstance = (EquippableBalloon.StatesInstance)data;
				return str + "\n\n" + DUPLICANTS.MODIFIERS.TIME_REMAINING.Replace("{0}", GameUtil.GetFormattedCycles(statesInstance.transitionTime - GameClock.Instance.GetTime(), "F1", false));
			};
			this.JoyResponse_HeardJoySinger = this.CreateStatusItem("JoyResponse_HeardJoySinger", DUPLICANTS.MODIFIERS.HEARDJOYSINGER.NAME, DUPLICANTS.MODIFIERS.HEARDJOYSINGER.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.JoyResponse_HeardJoySinger.resolveTooltipCallback = delegate(string str, object data)
			{
				InspirationEffectMonitor.Instance instance = (InspirationEffectMonitor.Instance)data;
				return str + "\n\n" + DUPLICANTS.MODIFIERS.TIME_REMAINING.Replace("{0}", GameUtil.GetFormattedCycles(instance.sm.inspirationTimeRemaining.Get(instance), "F1", false));
			};
			this.JoyResponse_StickerBombing = this.CreateStatusItem("JoyResponse_StickerBombing", DUPLICANTS.MODIFIERS.ISSTICKERBOMBING.NAME, DUPLICANTS.MODIFIERS.ISSTICKERBOMBING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.Meteorphile = this.CreateStatusItem("Meteorphile", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.EnteringDock = this.CreateStatusItem("EnteringDock", DUPLICANTS.STATUSITEMS.REMOTEWORKER.ENTERINGDOCK.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.ENTERINGDOCK.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.UnreachableDock = this.CreateStatusItem("UnreachableDock", DUPLICANTS.STATUSITEMS.REMOTEWORKER.UNREACHABLEDOCK.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.UNREACHABLEDOCK.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 2);
			this.NoHomeDock = this.CreateStatusItem("UnreachableDock", DUPLICANTS.STATUSITEMS.REMOTEWORKER.NOHOMEDOCK.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.NOHOMEDOCK.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerCapacitorStatus = this.CreateStatusItem("RemoteWorkerCapacitorStatus", DUPLICANTS.STATUSITEMS.REMOTEWORKER.POWERSTATUS.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.POWERSTATUS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerCapacitorStatus.resolveStringCallback = delegate(string str, object obj)
			{
				RemoteWorkerCapacitor remoteWorkerCapacitor = obj as RemoteWorkerCapacitor;
				float joules = 0f;
				float percent = 0f;
				if (remoteWorkerCapacitor != null)
				{
					joules = remoteWorkerCapacitor.Charge;
					percent = remoteWorkerCapacitor.ChargeRatio * 100f;
				}
				return str.Replace("{CHARGE}", GameUtil.GetFormattedJoules(joules, "F1", GameUtil.TimeSlice.None)).Replace("{RATIO}", GameUtil.GetFormattedPercent(percent, GameUtil.TimeSlice.None));
			};
			this.RemoteWorkerLowPower = this.CreateStatusItem("RemoteWorkerLowPower", DUPLICANTS.STATUSITEMS.REMOTEWORKER.LOWPOWER.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.LOWPOWER.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerOutOfPower = this.CreateStatusItem("RemoteWorkerOutOfPower", DUPLICANTS.STATUSITEMS.REMOTEWORKER.OUTOFPOWER.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.OUTOFPOWER.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerHighGunkLevel = this.CreateStatusItem("RemoteWorkerHighGunkLevel", DUPLICANTS.STATUSITEMS.REMOTEWORKER.HIGHGUNK.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.HIGHGUNK.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerFullGunkLevel = this.CreateStatusItem("RemoteWorkerFullGunkLevel", DUPLICANTS.STATUSITEMS.REMOTEWORKER.FULLGUNK.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.FULLGUNK.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerLowOil = this.CreateStatusItem("RemoteWorkerLowOil", DUPLICANTS.STATUSITEMS.REMOTEWORKER.LOWOIL.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.LOWOIL.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerOutOfOil = this.CreateStatusItem("RemoteWorkerOutOfOil", DUPLICANTS.STATUSITEMS.REMOTEWORKER.OUTOFOIL.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.OUTOFOIL.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerRecharging = this.CreateStatusItem("RemoteWorkerRecharging", DUPLICANTS.STATUSITEMS.REMOTEWORKER.RECHARGING.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.RECHARGING.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerOiling = this.CreateStatusItem("RemoteWorkerOiling", DUPLICANTS.STATUSITEMS.REMOTEWORKER.OILING.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.OILING.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerDraining = this.CreateStatusItem("RemoteWorkerDraining", DUPLICANTS.STATUSITEMS.REMOTEWORKER.DRAINING.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.DRAINING.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.BionicCriticalBattery = this.CreateStatusItem("BionicCriticalBattery", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.BionicCriticalBattery.AddNotification(null, null, null);
		}

		// Token: 0x040091A1 RID: 37281
		public StatusItem Idle;

		// Token: 0x040091A2 RID: 37282
		public StatusItem IdleInRockets;

		// Token: 0x040091A3 RID: 37283
		public StatusItem Pacified;

		// Token: 0x040091A4 RID: 37284
		public StatusItem PendingPacification;

		// Token: 0x040091A5 RID: 37285
		public StatusItem Dead;

		// Token: 0x040091A6 RID: 37286
		public StatusItem MoveToSuitNotRequired;

		// Token: 0x040091A7 RID: 37287
		public StatusItem DroppingUnusedInventory;

		// Token: 0x040091A8 RID: 37288
		public StatusItem MovingToSafeArea;

		// Token: 0x040091A9 RID: 37289
		public StatusItem BedUnreachable;

		// Token: 0x040091AA RID: 37290
		public StatusItem Hungry;

		// Token: 0x040091AB RID: 37291
		public StatusItem Starving;

		// Token: 0x040091AC RID: 37292
		public StatusItem Rotten;

		// Token: 0x040091AD RID: 37293
		public StatusItem Quarantined;

		// Token: 0x040091AE RID: 37294
		public StatusItem NoRationsAvailable;

		// Token: 0x040091AF RID: 37295
		public StatusItem RationsUnreachable;

		// Token: 0x040091B0 RID: 37296
		public StatusItem RationsNotPermitted;

		// Token: 0x040091B1 RID: 37297
		public StatusItem DailyRationLimitReached;

		// Token: 0x040091B2 RID: 37298
		public StatusItem Scalding;

		// Token: 0x040091B3 RID: 37299
		public StatusItem Hot;

		// Token: 0x040091B4 RID: 37300
		public StatusItem Cold;

		// Token: 0x040091B5 RID: 37301
		public StatusItem ExitingCold;

		// Token: 0x040091B6 RID: 37302
		public StatusItem ExitingHot;

		// Token: 0x040091B7 RID: 37303
		public StatusItem QuarantineAreaUnassigned;

		// Token: 0x040091B8 RID: 37304
		public StatusItem QuarantineAreaUnreachable;

		// Token: 0x040091B9 RID: 37305
		public StatusItem Tired;

		// Token: 0x040091BA RID: 37306
		public StatusItem NervousBreakdown;

		// Token: 0x040091BB RID: 37307
		public StatusItem Unhappy;

		// Token: 0x040091BC RID: 37308
		public StatusItem Suffocating;

		// Token: 0x040091BD RID: 37309
		public StatusItem HoldingBreath;

		// Token: 0x040091BE RID: 37310
		public StatusItem ToiletUnreachable;

		// Token: 0x040091BF RID: 37311
		public StatusItem NoUsableToilets;

		// Token: 0x040091C0 RID: 37312
		public StatusItem NoToilets;

		// Token: 0x040091C1 RID: 37313
		public StatusItem Vomiting;

		// Token: 0x040091C2 RID: 37314
		public StatusItem Coughing;

		// Token: 0x040091C3 RID: 37315
		public StatusItem Slippering;

		// Token: 0x040091C4 RID: 37316
		public StatusItem BreathingO2;

		// Token: 0x040091C5 RID: 37317
		public StatusItem EmittingCO2;

		// Token: 0x040091C6 RID: 37318
		public StatusItem LowOxygen;

		// Token: 0x040091C7 RID: 37319
		public StatusItem RedAlert;

		// Token: 0x040091C8 RID: 37320
		public StatusItem Digging;

		// Token: 0x040091C9 RID: 37321
		public StatusItem Eating;

		// Token: 0x040091CA RID: 37322
		public StatusItem Dreaming;

		// Token: 0x040091CB RID: 37323
		public StatusItem Sleeping;

		// Token: 0x040091CC RID: 37324
		public StatusItem SleepingExhausted;

		// Token: 0x040091CD RID: 37325
		public StatusItem SleepingInterruptedByLight;

		// Token: 0x040091CE RID: 37326
		public StatusItem SleepingInterruptedByNoise;

		// Token: 0x040091CF RID: 37327
		public StatusItem SleepingInterruptedByFearOfDark;

		// Token: 0x040091D0 RID: 37328
		public StatusItem SleepingInterruptedByMovement;

		// Token: 0x040091D1 RID: 37329
		public StatusItem SleepingInterruptedByCold;

		// Token: 0x040091D2 RID: 37330
		public StatusItem SleepingPeacefully;

		// Token: 0x040091D3 RID: 37331
		public StatusItem SleepingBadly;

		// Token: 0x040091D4 RID: 37332
		public StatusItem SleepingTerribly;

		// Token: 0x040091D5 RID: 37333
		public StatusItem Cleaning;

		// Token: 0x040091D6 RID: 37334
		public StatusItem PickingUp;

		// Token: 0x040091D7 RID: 37335
		public StatusItem Mopping;

		// Token: 0x040091D8 RID: 37336
		public StatusItem Cooking;

		// Token: 0x040091D9 RID: 37337
		public StatusItem Arting;

		// Token: 0x040091DA RID: 37338
		public StatusItem Mushing;

		// Token: 0x040091DB RID: 37339
		public StatusItem Researching;

		// Token: 0x040091DC RID: 37340
		public StatusItem ResearchingFromPOI;

		// Token: 0x040091DD RID: 37341
		public StatusItem MissionControlling;

		// Token: 0x040091DE RID: 37342
		public StatusItem Tinkering;

		// Token: 0x040091DF RID: 37343
		public StatusItem Storing;

		// Token: 0x040091E0 RID: 37344
		public StatusItem Building;

		// Token: 0x040091E1 RID: 37345
		public StatusItem Equipping;

		// Token: 0x040091E2 RID: 37346
		public StatusItem WarmingUp;

		// Token: 0x040091E3 RID: 37347
		public StatusItem GeneratingPower;

		// Token: 0x040091E4 RID: 37348
		public StatusItem Ranching;

		// Token: 0x040091E5 RID: 37349
		public StatusItem Harvesting;

		// Token: 0x040091E6 RID: 37350
		public StatusItem Uprooting;

		// Token: 0x040091E7 RID: 37351
		public StatusItem Emptying;

		// Token: 0x040091E8 RID: 37352
		public StatusItem Toggling;

		// Token: 0x040091E9 RID: 37353
		public StatusItem Deconstructing;

		// Token: 0x040091EA RID: 37354
		public StatusItem Disinfecting;

		// Token: 0x040091EB RID: 37355
		public StatusItem Relocating;

		// Token: 0x040091EC RID: 37356
		public StatusItem Upgrading;

		// Token: 0x040091ED RID: 37357
		public StatusItem Fabricating;

		// Token: 0x040091EE RID: 37358
		public StatusItem Processing;

		// Token: 0x040091EF RID: 37359
		public StatusItem Spicing;

		// Token: 0x040091F0 RID: 37360
		public StatusItem Clearing;

		// Token: 0x040091F1 RID: 37361
		public StatusItem BodyRegulatingHeating;

		// Token: 0x040091F2 RID: 37362
		public StatusItem BodyRegulatingCooling;

		// Token: 0x040091F3 RID: 37363
		public StatusItem EntombedChore;

		// Token: 0x040091F4 RID: 37364
		public StatusItem EarlyMorning;

		// Token: 0x040091F5 RID: 37365
		public StatusItem NightTime;

		// Token: 0x040091F6 RID: 37366
		public StatusItem PoorDecor;

		// Token: 0x040091F7 RID: 37367
		public StatusItem PoorQualityOfLife;

		// Token: 0x040091F8 RID: 37368
		public StatusItem PoorFoodQuality;

		// Token: 0x040091F9 RID: 37369
		public StatusItem GoodFoodQuality;

		// Token: 0x040091FA RID: 37370
		public StatusItem SevereWounds;

		// Token: 0x040091FB RID: 37371
		public StatusItem Incapacitated;

		// Token: 0x040091FC RID: 37372
		public StatusItem BionicOfflineIncapacitated;

		// Token: 0x040091FD RID: 37373
		public StatusItem BionicWaitingForReboot;

		// Token: 0x040091FE RID: 37374
		public StatusItem BionicBeingRebooted;

		// Token: 0x040091FF RID: 37375
		public StatusItem BionicRequiresSkillPerk;

		// Token: 0x04009200 RID: 37376
		public StatusItem BionicWantsOilChange;

		// Token: 0x04009201 RID: 37377
		public StatusItem InstallingElectrobank;

		// Token: 0x04009202 RID: 37378
		public StatusItem Fighting;

		// Token: 0x04009203 RID: 37379
		public StatusItem Fleeing;

		// Token: 0x04009204 RID: 37380
		public StatusItem Stressed;

		// Token: 0x04009205 RID: 37381
		public StatusItem LashingOut;

		// Token: 0x04009206 RID: 37382
		public StatusItem LowImmunity;

		// Token: 0x04009207 RID: 37383
		public StatusItem Studying;

		// Token: 0x04009208 RID: 37384
		public StatusItem Socializing;

		// Token: 0x04009209 RID: 37385
		public StatusItem Mingling;

		// Token: 0x0400920A RID: 37386
		public StatusItem ContactWithGerms;

		// Token: 0x0400920B RID: 37387
		public StatusItem ExposedToGerms;

		// Token: 0x0400920C RID: 37388
		public StatusItem LightWorkEfficiencyBonus;

		// Token: 0x0400920D RID: 37389
		public StatusItem LaboratoryWorkEfficiencyBonus;

		// Token: 0x0400920E RID: 37390
		public StatusItem BeingProductive;

		// Token: 0x0400920F RID: 37391
		public StatusItem BalloonArtistPlanning;

		// Token: 0x04009210 RID: 37392
		public StatusItem BalloonArtistHandingOut;

		// Token: 0x04009211 RID: 37393
		public StatusItem Partying;

		// Token: 0x04009212 RID: 37394
		public StatusItem GasLiquidIrritation;

		// Token: 0x04009213 RID: 37395
		public StatusItem ExpellingRads;

		// Token: 0x04009214 RID: 37396
		public StatusItem AnalyzingGenes;

		// Token: 0x04009215 RID: 37397
		public StatusItem AnalyzingArtifact;

		// Token: 0x04009216 RID: 37398
		public StatusItem MegaBrainTank_Pajamas_Wearing;

		// Token: 0x04009217 RID: 37399
		public StatusItem MegaBrainTank_Pajamas_Sleeping;

		// Token: 0x04009218 RID: 37400
		public StatusItem JoyResponse_HasBalloon;

		// Token: 0x04009219 RID: 37401
		public StatusItem JoyResponse_HeardJoySinger;

		// Token: 0x0400921A RID: 37402
		public StatusItem JoyResponse_StickerBombing;

		// Token: 0x0400921B RID: 37403
		public StatusItem Meteorphile;

		// Token: 0x0400921C RID: 37404
		public StatusItem FossilHunt_WorkerExcavating;

		// Token: 0x0400921D RID: 37405
		public StatusItem MorbRoverMakerDoctorWorking;

		// Token: 0x0400921E RID: 37406
		public StatusItem MorbRoverMakerWorkingOnRevealing;

		// Token: 0x0400921F RID: 37407
		public StatusItem ArmingTrap;

		// Token: 0x04009220 RID: 37408
		public StatusItem WaxedForTransitTube;

		// Token: 0x04009221 RID: 37409
		public StatusItem DataRainerPlanning;

		// Token: 0x04009222 RID: 37410
		public StatusItem DataRainerRaining;

		// Token: 0x04009223 RID: 37411
		public StatusItem RoboDancerPlanning;

		// Token: 0x04009224 RID: 37412
		public StatusItem RoboDancerDancing;

		// Token: 0x04009225 RID: 37413
		public StatusItem BionicExplorerBooster;

		// Token: 0x04009226 RID: 37414
		public StatusItem EnteringDock;

		// Token: 0x04009227 RID: 37415
		public StatusItem UnreachableDock;

		// Token: 0x04009228 RID: 37416
		public StatusItem NoHomeDock;

		// Token: 0x04009229 RID: 37417
		public StatusItem RemoteWorkerCapacitorStatus;

		// Token: 0x0400922A RID: 37418
		public StatusItem RemoteWorkerLowPower;

		// Token: 0x0400922B RID: 37419
		public StatusItem RemoteWorkerOutOfPower;

		// Token: 0x0400922C RID: 37420
		public StatusItem RemoteWorkerHighGunkLevel;

		// Token: 0x0400922D RID: 37421
		public StatusItem RemoteWorkerFullGunkLevel;

		// Token: 0x0400922E RID: 37422
		public StatusItem RemoteWorkerLowOil;

		// Token: 0x0400922F RID: 37423
		public StatusItem RemoteWorkerOutOfOil;

		// Token: 0x04009230 RID: 37424
		public StatusItem RemoteWorkerRecharging;

		// Token: 0x04009231 RID: 37425
		public StatusItem RemoteWorkerOiling;

		// Token: 0x04009232 RID: 37426
		public StatusItem RemoteWorkerDraining;

		// Token: 0x04009233 RID: 37427
		public StatusItem BionicCriticalBattery;

		// Token: 0x04009234 RID: 37428
		private const int NONE_OVERLAY = 0;
	}
}
