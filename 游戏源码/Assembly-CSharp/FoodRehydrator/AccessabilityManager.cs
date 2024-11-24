using System;
using UnityEngine;

namespace FoodRehydrator
{
	// Token: 0x020020E8 RID: 8424
	public class AccessabilityManager : KMonoBehaviour
	{
		// Token: 0x0600B35A RID: 45914 RVA: 0x00114515 File Offset: 0x00112715
		protected override void OnSpawn()
		{
			base.OnSpawn();
			Components.FoodRehydrators.Add(base.gameObject);
			base.Subscribe(824508782, new Action<object>(this.ActiveChangedHandler));
		}

		// Token: 0x0600B35B RID: 45915 RVA: 0x00114545 File Offset: 0x00112745
		protected override void OnCleanUp()
		{
			Components.FoodRehydrators.Remove(base.gameObject);
			base.OnCleanUp();
		}

		// Token: 0x0600B35C RID: 45916 RVA: 0x0011455D File Offset: 0x0011275D
		public void Reserve(GameObject reserver)
		{
			this.reserver = reserver;
			global::Debug.Assert(reserver != null && reserver.GetComponent<MinionResume>() != null);
		}

		// Token: 0x0600B35D RID: 45917 RVA: 0x00114583 File Offset: 0x00112783
		public void Unreserve()
		{
			this.activeWorkable = null;
			this.reserver = null;
		}

		// Token: 0x0600B35E RID: 45918 RVA: 0x0043A4F0 File Offset: 0x004386F0
		public void SetActiveWorkable(Workable work)
		{
			DebugUtil.DevAssert(this.activeWorkable == null || work == null, "FoodRehydrator::AccessabilityManager activating a second workable", null);
			this.activeWorkable = work;
			this.operational.SetActive(this.activeWorkable != null, false);
		}

		// Token: 0x0600B35F RID: 45919 RVA: 0x00114593 File Offset: 0x00112793
		public bool CanAccess(GameObject worker)
		{
			return this.operational.IsOperational && (this.reserver == null || this.reserver == worker);
		}

		// Token: 0x0600B360 RID: 45920 RVA: 0x001145C0 File Offset: 0x001127C0
		protected void ActiveChangedHandler(object obj)
		{
			if (!this.operational.IsActive)
			{
				this.CancelActiveWorkable();
			}
		}

		// Token: 0x0600B361 RID: 45921 RVA: 0x001145D5 File Offset: 0x001127D5
		public void CancelActiveWorkable()
		{
			if (this.activeWorkable != null)
			{
				this.activeWorkable.StopWork(this.activeWorkable.worker, true);
			}
		}

		// Token: 0x04008DB9 RID: 36281
		[MyCmpReq]
		private Operational operational;

		// Token: 0x04008DBA RID: 36282
		private GameObject reserver;

		// Token: 0x04008DBB RID: 36283
		private Workable activeWorkable;
	}
}
