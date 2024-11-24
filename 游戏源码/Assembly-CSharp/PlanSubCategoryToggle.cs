using System;
using UnityEngine;

// Token: 0x02001EAE RID: 7854
public class PlanSubCategoryToggle : KMonoBehaviour
{
	// Token: 0x0600A4EC RID: 42220 RVA: 0x0010AF50 File Offset: 0x00109150
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.open = !this.open;
			this.gridContainer.SetActive(this.open);
			this.toggle.ChangeState(this.open ? 0 : 1);
		}));
	}

	// Token: 0x0400810B RID: 33035
	[SerializeField]
	private MultiToggle toggle;

	// Token: 0x0400810C RID: 33036
	[SerializeField]
	private GameObject gridContainer;

	// Token: 0x0400810D RID: 33037
	private bool open = true;
}
