using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class RocketSimpleInfoPanel : SimpleInfoPanel
{
	private Dictionary<string, GameObject> cargoBayLabels = new Dictionary<string, GameObject>();

	private Dictionary<string, GameObject> artifactModuleLabels = new Dictionary<string, GameObject>();

	public RocketSimpleInfoPanel(SimpleInfoScreen simpleInfoScreen)
		: base(simpleInfoScreen)
	{
	}

	public override void Refresh(CollapsibleDetailContentPanel rocketStatusContainer, GameObject selectedTarget)
	{
		if (selectedTarget == null)
		{
			simpleInfoRoot.StoragePanel.gameObject.SetActive(value: false);
			return;
		}
		RocketModuleCluster rocketModuleCluster = null;
		Clustercraft clusterCraft = null;
		CraftModuleInterface craftModuleInterface = null;
		GetRocketStuffFromTarget(selectedTarget, ref rocketModuleCluster, ref clusterCraft, ref craftModuleInterface);
		rocketStatusContainer.gameObject.SetActive(craftModuleInterface != null || rocketModuleCluster != null);
		if (craftModuleInterface != null)
		{
			RocketEngineCluster engine = craftModuleInterface.GetEngine();
			string arg;
			string text;
			if (engine != null && engine.GetComponent<HEPFuelTank>() != null)
			{
				arg = GameUtil.GetFormattedHighEnergyParticles(craftModuleInterface.FuelPerHex);
				text = GameUtil.GetFormattedHighEnergyParticles(craftModuleInterface.FuelRemaining);
			}
			else
			{
				arg = GameUtil.GetFormattedMass(craftModuleInterface.FuelPerHex);
				text = GameUtil.GetFormattedMass(craftModuleInterface.FuelRemaining);
			}
			string tooltip = string.Concat(UI.CLUSTERMAP.ROCKETS.RANGE.TOOLTIP, "\n    • ", string.Format(UI.CLUSTERMAP.ROCKETS.FUEL_PER_HEX.NAME, arg), "\n    • ", UI.CLUSTERMAP.ROCKETS.FUEL_REMAINING.NAME, text, "\n    • ", UI.CLUSTERMAP.ROCKETS.OXIDIZER_REMAINING.NAME, GameUtil.GetFormattedMass(craftModuleInterface.OxidizerPowerRemaining));
			rocketStatusContainer.SetLabel("RangeRemaining", string.Concat(UI.CLUSTERMAP.ROCKETS.RANGE.NAME, GameUtil.GetFormattedRocketRange(craftModuleInterface.RangeInTiles)), tooltip);
			string tooltip2 = string.Concat(UI.CLUSTERMAP.ROCKETS.SPEED.TOOLTIP, "\n    • ", UI.CLUSTERMAP.ROCKETS.POWER_TOTAL.NAME, craftModuleInterface.EnginePower.ToString(), "\n    • ", UI.CLUSTERMAP.ROCKETS.BURDEN_TOTAL.NAME, craftModuleInterface.TotalBurden.ToString());
			rocketStatusContainer.SetLabel("Speed", string.Concat(UI.CLUSTERMAP.ROCKETS.SPEED.NAME, GameUtil.GetFormattedRocketRangePerCycle(craftModuleInterface.Speed)), tooltip2);
			if (craftModuleInterface.GetEngine() != null)
			{
				string tooltip3 = string.Format(UI.CLUSTERMAP.ROCKETS.MAX_HEIGHT.TOOLTIP, craftModuleInterface.GetEngine().GetProperName(), craftModuleInterface.MaxHeight.ToString());
				rocketStatusContainer.SetLabel("MaxHeight", string.Format(UI.CLUSTERMAP.ROCKETS.MAX_HEIGHT.NAME, craftModuleInterface.RocketHeight.ToString(), craftModuleInterface.MaxHeight.ToString()), tooltip3);
			}
			rocketStatusContainer.SetLabel("RocketSpacer2", "", "");
			if (clusterCraft != null)
			{
				foreach (KeyValuePair<string, GameObject> artifactModuleLabel in artifactModuleLabels)
				{
					artifactModuleLabel.Value.SetActive(value: false);
				}
				int num = 0;
				foreach (Ref<RocketModuleCluster> clusterModule in clusterCraft.ModuleInterface.ClusterModules)
				{
					ArtifactModule component = clusterModule.Get().GetComponent<ArtifactModule>();
					if (component != null)
					{
						string text2 = "";
						rocketStatusContainer.SetLabel(text: (!(component.Occupant != null)) ? $"{component.GetProperName()}: {UI.CLUSTERMAP.ROCKETS.ARTIFACT_MODULE.EMPTY}" : (component.GetProperName() + ": " + component.Occupant.GetProperName()), id: "artifactModule_" + num, tooltip: "");
						num++;
					}
				}
				List<CargoBayCluster> allCargoBays = clusterCraft.GetAllCargoBays();
				bool flag = allCargoBays != null && allCargoBays.Count > 0;
				foreach (KeyValuePair<string, GameObject> cargoBayLabel in cargoBayLabels)
				{
					cargoBayLabel.Value.SetActive(value: false);
				}
				if (flag)
				{
					ListPool<Tuple<string, TextStyleSetting>, SimpleInfoScreen>.PooledList pooledList = ListPool<Tuple<string, TextStyleSetting>, SimpleInfoScreen>.Allocate();
					int num2 = 0;
					foreach (CargoBayCluster item in allCargoBays)
					{
						pooledList.Clear();
						Storage storage = item.storage;
						string text4 = $"{item.GetComponent<KPrefabID>().GetProperName()}: {GameUtil.GetFormattedMass(storage.MassStored())}/{GameUtil.GetFormattedMass(storage.capacityKg)}";
						foreach (GameObject item2 in storage.GetItems())
						{
							KPrefabID component2 = item2.GetComponent<KPrefabID>();
							PrimaryElement component3 = item2.GetComponent<PrimaryElement>();
							string a = $"{component2.GetProperName()} : {GameUtil.GetFormattedMass(component3.Mass)}";
							pooledList.Add(new Tuple<string, TextStyleSetting>(a, PluginAssets.Instance.defaultTextStyleSetting));
						}
						string text5 = "";
						for (int i = 0; i < pooledList.Count; i++)
						{
							text5 += pooledList[i].first;
							if (i != pooledList.Count - 1)
							{
								text5 += "\n";
							}
						}
						rocketStatusContainer.SetLabel("cargoBay_" + num2, text4, text5);
						num2++;
					}
					pooledList.Recycle();
				}
			}
		}
		if (rocketModuleCluster != null)
		{
			rocketStatusContainer.SetLabel("ModuleStats", string.Concat(UI.CLUSTERMAP.ROCKETS.MODULE_STATS.NAME, selectedTarget.GetProperName()), UI.CLUSTERMAP.ROCKETS.MODULE_STATS.TOOLTIP);
			float burden = rocketModuleCluster.performanceStats.Burden;
			float enginePower = rocketModuleCluster.performanceStats.EnginePower;
			if (burden != 0f)
			{
				rocketStatusContainer.SetLabel("LocalBurden", string.Concat("    • ", UI.CLUSTERMAP.ROCKETS.BURDEN_MODULE.NAME, burden.ToString()), string.Format(UI.CLUSTERMAP.ROCKETS.BURDEN_MODULE.TOOLTIP, burden));
			}
			if (enginePower != 0f)
			{
				rocketStatusContainer.SetLabel("LocalPower", string.Concat("    • ", UI.CLUSTERMAP.ROCKETS.POWER_MODULE.NAME, enginePower.ToString()), string.Format(UI.CLUSTERMAP.ROCKETS.POWER_MODULE.TOOLTIP, enginePower));
			}
		}
		rocketStatusContainer.Commit();
	}

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
			}
		}
		else if (clusterCraft != null)
		{
			craftModuleInterface = clusterCraft.ModuleInterface;
		}
	}
}
