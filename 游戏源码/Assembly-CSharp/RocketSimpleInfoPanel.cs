using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001EEA RID: 7914
public class RocketSimpleInfoPanel : SimpleInfoPanel
{
	// Token: 0x0600A6AD RID: 42669 RVA: 0x0010C037 File Offset: 0x0010A237
	public RocketSimpleInfoPanel(SimpleInfoScreen simpleInfoScreen) : base(simpleInfoScreen)
	{
	}

	// Token: 0x0600A6AE RID: 42670 RVA: 0x003F4AF8 File Offset: 0x003F2CF8
	public override void Refresh(CollapsibleDetailContentPanel rocketStatusContainer, GameObject selectedTarget)
	{
		if (selectedTarget == null)
		{
			this.simpleInfoRoot.StoragePanel.gameObject.SetActive(false);
			return;
		}
		RocketModuleCluster rocketModuleCluster = null;
		Clustercraft clustercraft = null;
		CraftModuleInterface craftModuleInterface = null;
		RocketSimpleInfoPanel.GetRocketStuffFromTarget(selectedTarget, ref rocketModuleCluster, ref clustercraft, ref craftModuleInterface);
		rocketStatusContainer.gameObject.SetActive(craftModuleInterface != null || rocketModuleCluster != null);
		if (craftModuleInterface != null)
		{
			RocketEngineCluster engine = craftModuleInterface.GetEngine();
			string arg;
			string text;
			if (engine != null && engine.GetComponent<HEPFuelTank>() != null)
			{
				arg = GameUtil.GetFormattedHighEnergyParticles(craftModuleInterface.FuelPerHex, GameUtil.TimeSlice.None, true);
				text = GameUtil.GetFormattedHighEnergyParticles(craftModuleInterface.FuelRemaining, GameUtil.TimeSlice.None, true);
			}
			else
			{
				arg = GameUtil.GetFormattedMass(craftModuleInterface.FuelPerHex, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
				text = GameUtil.GetFormattedMass(craftModuleInterface.FuelRemaining, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
			}
			string tooltip = string.Concat(new string[]
			{
				UI.CLUSTERMAP.ROCKETS.RANGE.TOOLTIP,
				"\n    • ",
				string.Format(UI.CLUSTERMAP.ROCKETS.FUEL_PER_HEX.NAME, arg),
				"\n    • ",
				UI.CLUSTERMAP.ROCKETS.FUEL_REMAINING.NAME,
				text,
				"\n    • ",
				UI.CLUSTERMAP.ROCKETS.OXIDIZER_REMAINING.NAME,
				GameUtil.GetFormattedMass(craftModuleInterface.OxidizerPowerRemaining, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
			});
			rocketStatusContainer.SetLabel("RangeRemaining", UI.CLUSTERMAP.ROCKETS.RANGE.NAME + GameUtil.GetFormattedRocketRange(craftModuleInterface.RangeInTiles, true), tooltip);
			string tooltip2 = string.Concat(new string[]
			{
				UI.CLUSTERMAP.ROCKETS.SPEED.TOOLTIP,
				"\n    • ",
				UI.CLUSTERMAP.ROCKETS.POWER_TOTAL.NAME,
				craftModuleInterface.EnginePower.ToString(),
				"\n    • ",
				UI.CLUSTERMAP.ROCKETS.BURDEN_TOTAL.NAME,
				craftModuleInterface.TotalBurden.ToString()
			});
			rocketStatusContainer.SetLabel("Speed", UI.CLUSTERMAP.ROCKETS.SPEED.NAME + GameUtil.GetFormattedRocketRangePerCycle(craftModuleInterface.Speed, true), tooltip2);
			if (craftModuleInterface.GetEngine() != null)
			{
				string tooltip3 = string.Format(UI.CLUSTERMAP.ROCKETS.MAX_HEIGHT.TOOLTIP, craftModuleInterface.GetEngine().GetProperName(), craftModuleInterface.MaxHeight.ToString());
				rocketStatusContainer.SetLabel("MaxHeight", string.Format(UI.CLUSTERMAP.ROCKETS.MAX_HEIGHT.NAME, craftModuleInterface.RocketHeight.ToString(), craftModuleInterface.MaxHeight.ToString()), tooltip3);
			}
			rocketStatusContainer.SetLabel("RocketSpacer2", "", "");
			if (clustercraft != null)
			{
				foreach (KeyValuePair<string, GameObject> keyValuePair in this.artifactModuleLabels)
				{
					keyValuePair.Value.SetActive(false);
				}
				int num = 0;
				foreach (Ref<RocketModuleCluster> @ref in clustercraft.ModuleInterface.ClusterModules)
				{
					ArtifactModule component = @ref.Get().GetComponent<ArtifactModule>();
					if (component != null)
					{
						string text2;
						if (component.Occupant != null)
						{
							text2 = component.GetProperName() + ": " + component.Occupant.GetProperName();
						}
						else
						{
							text2 = string.Format("{0}: {1}", component.GetProperName(), UI.CLUSTERMAP.ROCKETS.ARTIFACT_MODULE.EMPTY);
						}
						rocketStatusContainer.SetLabel("artifactModule_" + num.ToString(), text2, "");
						num++;
					}
				}
				List<CargoBayCluster> allCargoBays = clustercraft.GetAllCargoBays();
				bool flag = allCargoBays != null && allCargoBays.Count > 0;
				foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.cargoBayLabels)
				{
					keyValuePair2.Value.SetActive(false);
				}
				if (flag)
				{
					ListPool<global::Tuple<string, TextStyleSetting>, SimpleInfoScreen>.PooledList pooledList = ListPool<global::Tuple<string, TextStyleSetting>, SimpleInfoScreen>.Allocate();
					int num2 = 0;
					foreach (CargoBayCluster cargoBayCluster in allCargoBays)
					{
						pooledList.Clear();
						Storage storage = cargoBayCluster.storage;
						string text3 = string.Format("{0}: {1}/{2}", cargoBayCluster.GetComponent<KPrefabID>().GetProperName(), GameUtil.GetFormattedMass(storage.MassStored(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedMass(storage.capacityKg, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
						foreach (GameObject gameObject in storage.GetItems())
						{
							KPrefabID component2 = gameObject.GetComponent<KPrefabID>();
							PrimaryElement component3 = gameObject.GetComponent<PrimaryElement>();
							string a = string.Format("{0} : {1}", component2.GetProperName(), GameUtil.GetFormattedMass(component3.Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							pooledList.Add(new global::Tuple<string, TextStyleSetting>(a, PluginAssets.Instance.defaultTextStyleSetting));
						}
						string text4 = "";
						for (int i = 0; i < pooledList.Count; i++)
						{
							text4 += pooledList[i].first;
							if (i != pooledList.Count - 1)
							{
								text4 += "\n";
							}
						}
						rocketStatusContainer.SetLabel("cargoBay_" + num2.ToString(), text3, text4);
						num2++;
					}
					pooledList.Recycle();
				}
			}
		}
		if (rocketModuleCluster != null)
		{
			rocketStatusContainer.SetLabel("ModuleStats", UI.CLUSTERMAP.ROCKETS.MODULE_STATS.NAME + selectedTarget.GetProperName(), UI.CLUSTERMAP.ROCKETS.MODULE_STATS.TOOLTIP);
			float burden = rocketModuleCluster.performanceStats.Burden;
			float enginePower = rocketModuleCluster.performanceStats.EnginePower;
			if (burden != 0f)
			{
				rocketStatusContainer.SetLabel("LocalBurden", "    • " + UI.CLUSTERMAP.ROCKETS.BURDEN_MODULE.NAME + burden.ToString(), string.Format(UI.CLUSTERMAP.ROCKETS.BURDEN_MODULE.TOOLTIP, burden));
			}
			if (enginePower != 0f)
			{
				rocketStatusContainer.SetLabel("LocalPower", "    • " + UI.CLUSTERMAP.ROCKETS.POWER_MODULE.NAME + enginePower.ToString(), string.Format(UI.CLUSTERMAP.ROCKETS.POWER_MODULE.TOOLTIP, enginePower));
			}
		}
		rocketStatusContainer.Commit();
	}

	// Token: 0x0600A6AF RID: 42671 RVA: 0x003F51D4 File Offset: 0x003F33D4
	public static void GetRocketStuffFromTarget(GameObject selectedTarget, ref RocketModuleCluster rocketModuleCluster, ref Clustercraft clusterCraft, ref CraftModuleInterface craftModuleInterface)
	{
		rocketModuleCluster = selectedTarget.GetComponent<RocketModuleCluster>();
		clusterCraft = selectedTarget.GetComponent<Clustercraft>();
		craftModuleInterface = null;
		if (rocketModuleCluster != null)
		{
			craftModuleInterface = rocketModuleCluster.CraftInterface;
			if (clusterCraft == null)
			{
				clusterCraft = craftModuleInterface.GetComponent<Clustercraft>();
				return;
			}
		}
		else if (clusterCraft != null)
		{
			craftModuleInterface = clusterCraft.ModuleInterface;
		}
	}

	// Token: 0x040082F1 RID: 33521
	private Dictionary<string, GameObject> cargoBayLabels = new Dictionary<string, GameObject>();

	// Token: 0x040082F2 RID: 33522
	private Dictionary<string, GameObject> artifactModuleLabels = new Dictionary<string, GameObject>();
}
