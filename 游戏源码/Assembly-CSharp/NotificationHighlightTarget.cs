using System;

// Token: 0x02001E5E RID: 7774
public class NotificationHighlightTarget : KMonoBehaviour
{
	// Token: 0x0600A2FD RID: 41725 RVA: 0x00109BB5 File Offset: 0x00107DB5
	protected void OnEnable()
	{
		this.controller = base.GetComponentInParent<NotificationHighlightController>();
		if (this.controller != null)
		{
			this.controller.AddTarget(this);
		}
	}

	// Token: 0x0600A2FE RID: 41726 RVA: 0x00109BDD File Offset: 0x00107DDD
	protected override void OnDisable()
	{
		if (this.controller != null)
		{
			this.controller.RemoveTarget(this);
		}
	}

	// Token: 0x0600A2FF RID: 41727 RVA: 0x00109BF9 File Offset: 0x00107DF9
	public void View()
	{
		base.GetComponentInParent<NotificationHighlightController>().TargetViewed(this);
	}

	// Token: 0x04007F36 RID: 32566
	public string targetKey;

	// Token: 0x04007F37 RID: 32567
	private NotificationHighlightController controller;
}
