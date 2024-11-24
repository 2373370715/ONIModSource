using System;

namespace FoodRehydrator
{
	// Token: 0x020020E9 RID: 8425
	public class ResourceRequirementMonitor : KMonoBehaviour
	{
		// Token: 0x0600B363 RID: 45923 RVA: 0x0043A540 File Offset: 0x00438740
		protected override void OnSpawn()
		{
			base.OnSpawn();
			Storage[] components = base.GetComponents<Storage>();
			DebugUtil.DevAssert(components.Length == 2, "Incorrect number of storages on foodrehydrator", null);
			this.packages = components[0];
			this.water = components[1];
			base.Subscribe<ResourceRequirementMonitor>(-1697596308, ResourceRequirementMonitor.OnStorageChangedDelegate);
		}

		// Token: 0x0600B364 RID: 45924 RVA: 0x001145FC File Offset: 0x001127FC
		protected float GetAvailableWater()
		{
			return this.water.GetMassAvailable(GameTags.Water);
		}

		// Token: 0x0600B365 RID: 45925 RVA: 0x0011460E File Offset: 0x0011280E
		protected bool HasSufficientResources()
		{
			return this.packages.items.Count > 0 && this.GetAvailableWater() > 1f;
		}

		// Token: 0x0600B366 RID: 45926 RVA: 0x00114632 File Offset: 0x00112832
		protected void OnStorageChanged(object _)
		{
			this.operational.SetFlag(ResourceRequirementMonitor.flag, this.HasSufficientResources());
		}

		// Token: 0x04008DBC RID: 36284
		[MyCmpReq]
		private Operational operational;

		// Token: 0x04008DBD RID: 36285
		private Storage packages;

		// Token: 0x04008DBE RID: 36286
		private Storage water;

		// Token: 0x04008DBF RID: 36287
		private static readonly Operational.Flag flag = new Operational.Flag("HasSufficientResources", Operational.Flag.Type.Requirement);

		// Token: 0x04008DC0 RID: 36288
		private static readonly EventSystem.IntraObjectHandler<ResourceRequirementMonitor> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ResourceRequirementMonitor>(delegate(ResourceRequirementMonitor component, object data)
		{
			component.OnStorageChanged(data);
		});
	}
}
