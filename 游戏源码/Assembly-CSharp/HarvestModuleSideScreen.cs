using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F76 RID: 8054
public class HarvestModuleSideScreen : SideScreenContent, ISimEveryTick
{
	// Token: 0x17000AD2 RID: 2770
	// (get) Token: 0x0600A9F5 RID: 43509 RVA: 0x0010E720 File Offset: 0x0010C920
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x0600A9F6 RID: 43510 RVA: 0x0010E6EC File Offset: 0x0010C8EC
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x0600A9F7 RID: 43511 RVA: 0x000FE620 File Offset: 0x000FC820
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x0600A9F8 RID: 43512 RVA: 0x0010E72D File Offset: 0x0010C92D
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Clustercraft>() != null && this.GetResourceHarvestModule(target.GetComponent<Clustercraft>()) != null;
	}

	// Token: 0x0600A9F9 RID: 43513 RVA: 0x00403800 File Offset: 0x00401A00
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		ResourceHarvestModule.StatesInstance resourceHarvestModule = this.GetResourceHarvestModule(this.targetCraft);
		this.RefreshModulePanel(resourceHarvestModule);
	}

	// Token: 0x0600A9FA RID: 43514 RVA: 0x00403834 File Offset: 0x00401A34
	private ResourceHarvestModule.StatesInstance GetResourceHarvestModule(Clustercraft craft)
	{
		foreach (Ref<RocketModuleCluster> @ref in craft.GetComponent<CraftModuleInterface>().ClusterModules)
		{
			GameObject gameObject = @ref.Get().gameObject;
			if (gameObject.GetDef<ResourceHarvestModule.Def>() != null)
			{
				return gameObject.GetSMI<ResourceHarvestModule.StatesInstance>();
			}
		}
		return null;
	}

	// Token: 0x0600A9FB RID: 43515 RVA: 0x004038A0 File Offset: 0x00401AA0
	private void RefreshModulePanel(StateMachine.Instance module)
	{
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("icon").sprite = Def.GetUISprite(module.gameObject, "ui", false).first;
		component.GetReference<LocText>("label").SetText(module.gameObject.GetProperName());
	}

	// Token: 0x0600A9FC RID: 43516 RVA: 0x004038F4 File Offset: 0x00401AF4
	public void SimEveryTick(float dt)
	{
		if (this.targetCraft.IsNullOrDestroyed())
		{
			return;
		}
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		ResourceHarvestModule.StatesInstance resourceHarvestModule = this.GetResourceHarvestModule(this.targetCraft);
		if (resourceHarvestModule == null)
		{
			return;
		}
		GenericUIProgressBar reference = component.GetReference<GenericUIProgressBar>("progressBar");
		float num = 4f;
		float num2 = resourceHarvestModule.timeinstate % num;
		if (resourceHarvestModule.sm.canHarvest.Get(resourceHarvestModule))
		{
			reference.SetFillPercentage(num2 / num);
			reference.label.SetText(UI.UISIDESCREENS.HARVESTMODULESIDESCREEN.MINING_IN_PROGRESS);
		}
		else
		{
			reference.SetFillPercentage(0f);
			reference.label.SetText(UI.UISIDESCREENS.HARVESTMODULESIDESCREEN.MINING_STOPPED);
		}
		GenericUIProgressBar reference2 = component.GetReference<GenericUIProgressBar>("diamondProgressBar");
		Storage component2 = resourceHarvestModule.GetComponent<Storage>();
		float fillPercentage = component2.MassStored() / component2.Capacity();
		reference2.SetFillPercentage(fillPercentage);
		reference2.label.SetText(ElementLoader.GetElement(SimHashes.Diamond.CreateTag()).name + ": " + GameUtil.GetFormattedMass(component2.MassStored(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x040085A7 RID: 34215
	private Clustercraft targetCraft;

	// Token: 0x040085A8 RID: 34216
	public GameObject moduleContentContainer;

	// Token: 0x040085A9 RID: 34217
	public GameObject modulePanelPrefab;
}
