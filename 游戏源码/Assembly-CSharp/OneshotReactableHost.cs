using System;
using UnityEngine;

// Token: 0x02001681 RID: 5761
[AddComponentMenu("KMonoBehaviour/scripts/OneshotReactableHost")]
public class OneshotReactableHost : KMonoBehaviour
{
	// Token: 0x06007706 RID: 30470 RVA: 0x000EE2F9 File Offset: 0x000EC4F9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("CleanupOneshotReactable", this.lifetime, new Action<object>(this.OnExpire), null, null);
	}

	// Token: 0x06007707 RID: 30471 RVA: 0x000EE325 File Offset: 0x000EC525
	public void SetReactable(Reactable reactable)
	{
		this.reactable = reactable;
	}

	// Token: 0x06007708 RID: 30472 RVA: 0x0030C624 File Offset: 0x0030A824
	private void OnExpire(object obj)
	{
		if (!this.reactable.IsReacting)
		{
			this.reactable.Cleanup();
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		GameScheduler.Instance.Schedule("CleanupOneshotReactable", 0.5f, new Action<object>(this.OnExpire), null, null);
	}

	// Token: 0x04005903 RID: 22787
	private Reactable reactable;

	// Token: 0x04005904 RID: 22788
	public float lifetime = 1f;
}
