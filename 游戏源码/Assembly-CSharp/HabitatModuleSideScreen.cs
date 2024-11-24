using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F74 RID: 8052
public class HabitatModuleSideScreen : SideScreenContent
{
	// Token: 0x17000AD1 RID: 2769
	// (get) Token: 0x0600A9EB RID: 43499 RVA: 0x0010E6DF File Offset: 0x0010C8DF
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x0600A9EC RID: 43500 RVA: 0x0010E6EC File Offset: 0x0010C8EC
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x0600A9ED RID: 43501 RVA: 0x000FE620 File Offset: 0x000FC820
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x0600A9EE RID: 43502 RVA: 0x0010E6FC File Offset: 0x0010C8FC
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Clustercraft>() != null && this.GetPassengerModule(target.GetComponent<Clustercraft>()) != null;
	}

	// Token: 0x0600A9EF RID: 43503 RVA: 0x00403680 File Offset: 0x00401880
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		PassengerRocketModule passengerModule = this.GetPassengerModule(this.targetCraft);
		this.RefreshModulePanel(passengerModule);
	}

	// Token: 0x0600A9F0 RID: 43504 RVA: 0x004036B4 File Offset: 0x004018B4
	private PassengerRocketModule GetPassengerModule(Clustercraft craft)
	{
		foreach (Ref<RocketModuleCluster> @ref in craft.GetComponent<CraftModuleInterface>().ClusterModules)
		{
			PassengerRocketModule component = @ref.Get().GetComponent<PassengerRocketModule>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x0600A9F1 RID: 43505 RVA: 0x0040371C File Offset: 0x0040191C
	private void RefreshModulePanel(PassengerRocketModule module)
	{
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("icon").sprite = Def.GetUISprite(module.gameObject, "ui", false).first;
		KButton reference = component.GetReference<KButton>("button");
		reference.ClearOnClick();
		reference.onClick += delegate()
		{
			AudioMixer.instance.Start(module.interiorReverbSnapshot);
			AudioMixer.instance.PauseSpaceVisibleSnapshot(true);
			ClusterManager.Instance.SetActiveWorld(module.GetComponent<ClustercraftExteriorDoor>().GetTargetWorld().id);
			ManagementMenu.Instance.CloseAll();
		};
		component.GetReference<LocText>("label").SetText(module.gameObject.GetProperName());
	}

	// Token: 0x040085A3 RID: 34211
	private Clustercraft targetCraft;

	// Token: 0x040085A4 RID: 34212
	public GameObject moduleContentContainer;

	// Token: 0x040085A5 RID: 34213
	public GameObject modulePanelPrefab;
}
