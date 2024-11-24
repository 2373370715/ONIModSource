using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A6B RID: 2667
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/InOrbitRequired")]
public class InOrbitRequired : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x0600311C RID: 12572 RVA: 0x001FE0CC File Offset: 0x001FC2CC
	protected override void OnSpawn()
	{
		WorldContainer myWorld = this.GetMyWorld();
		this.craftModuleInterface = myWorld.GetComponent<CraftModuleInterface>();
		base.OnSpawn();
		bool newInOrbit = this.craftModuleInterface.HasTag(GameTags.RocketNotOnGround);
		this.UpdateFlag(newInOrbit);
		this.craftModuleInterface.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
	}

	// Token: 0x0600311D RID: 12573 RVA: 0x000BFE46 File Offset: 0x000BE046
	protected override void OnCleanUp()
	{
		if (this.craftModuleInterface != null)
		{
			this.craftModuleInterface.Unsubscribe(-1582839653, new Action<object>(this.OnTagsChanged));
		}
	}

	// Token: 0x0600311E RID: 12574 RVA: 0x001FE128 File Offset: 0x001FC328
	private void OnTagsChanged(object data)
	{
		TagChangedEventData tagChangedEventData = (TagChangedEventData)data;
		if (tagChangedEventData.tag == GameTags.RocketNotOnGround)
		{
			this.UpdateFlag(tagChangedEventData.added);
		}
	}

	// Token: 0x0600311F RID: 12575 RVA: 0x000BFE72 File Offset: 0x000BE072
	private void UpdateFlag(bool newInOrbit)
	{
		this.operational.SetFlag(InOrbitRequired.inOrbitFlag, newInOrbit);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.InOrbitRequired, !newInOrbit, this);
	}

	// Token: 0x06003120 RID: 12576 RVA: 0x000BFEA5 File Offset: 0x000BE0A5
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.BUILDINGEFFECTS.IN_ORBIT_REQUIRED, UI.BUILDINGEFFECTS.TOOLTIPS.IN_ORBIT_REQUIRED, Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x0400211D RID: 8477
	[MyCmpReq]
	private Building building;

	// Token: 0x0400211E RID: 8478
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400211F RID: 8479
	public static readonly Operational.Flag inOrbitFlag = new Operational.Flag("in_orbit", Operational.Flag.Type.Requirement);

	// Token: 0x04002120 RID: 8480
	private CraftModuleInterface craftModuleInterface;
}
