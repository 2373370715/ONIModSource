using System;
using System.Collections.Generic;

// Token: 0x02000920 RID: 2336
public class KAnimSynchronizer
{
	// Token: 0x1700014B RID: 331
	// (get) Token: 0x060029E4 RID: 10724 RVA: 0x000BB42C File Offset: 0x000B962C
	// (set) Token: 0x060029E5 RID: 10725 RVA: 0x000BB434 File Offset: 0x000B9634
	public string IdleAnim
	{
		get
		{
			return this.idle_anim;
		}
		set
		{
			this.idle_anim = value;
		}
	}

	// Token: 0x060029E6 RID: 10726 RVA: 0x000BB43D File Offset: 0x000B963D
	public KAnimSynchronizer(KAnimControllerBase master_controller)
	{
		this.masterController = master_controller;
	}

	// Token: 0x060029E7 RID: 10727 RVA: 0x000BB46D File Offset: 0x000B966D
	private void Clear(KAnimControllerBase controller)
	{
		controller.Play(this.IdleAnim, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x060029E8 RID: 10728 RVA: 0x000BB48B File Offset: 0x000B968B
	public void Add(KAnimControllerBase controller)
	{
		this.Targets.Add(controller);
	}

	// Token: 0x060029E9 RID: 10729 RVA: 0x000BB499 File Offset: 0x000B9699
	public void Remove(KAnimControllerBase controller)
	{
		this.Clear(controller);
		this.Targets.Remove(controller);
	}

	// Token: 0x060029EA RID: 10730 RVA: 0x000BB4AF File Offset: 0x000B96AF
	public void RemoveWithoutIdleAnim(KAnimControllerBase controller)
	{
		this.Targets.Remove(controller);
	}

	// Token: 0x060029EB RID: 10731 RVA: 0x000BB4BE File Offset: 0x000B96BE
	private void Clear(KAnimSynchronizedController controller)
	{
		controller.Play(this.IdleAnim, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x060029EC RID: 10732 RVA: 0x000BB4DC File Offset: 0x000B96DC
	public void Add(KAnimSynchronizedController controller)
	{
		this.SyncedControllers.Add(controller);
	}

	// Token: 0x060029ED RID: 10733 RVA: 0x000BB4EA File Offset: 0x000B96EA
	public void Remove(KAnimSynchronizedController controller)
	{
		this.Clear(controller);
		this.SyncedControllers.Remove(controller);
	}

	// Token: 0x060029EE RID: 10734 RVA: 0x001D7514 File Offset: 0x001D5714
	public void Clear()
	{
		foreach (KAnimControllerBase kanimControllerBase in this.Targets)
		{
			if (!(kanimControllerBase == null) && kanimControllerBase.AnimFiles != null)
			{
				this.Clear(kanimControllerBase);
			}
		}
		this.Targets.Clear();
		foreach (KAnimSynchronizedController kanimSynchronizedController in this.SyncedControllers)
		{
			if (!(kanimSynchronizedController.synchronizedController == null) && kanimSynchronizedController.synchronizedController.AnimFiles != null)
			{
				this.Clear(kanimSynchronizedController);
			}
		}
		this.SyncedControllers.Clear();
	}

	// Token: 0x060029EF RID: 10735 RVA: 0x001D75EC File Offset: 0x001D57EC
	public void Sync(KAnimControllerBase controller)
	{
		if (this.masterController == null)
		{
			return;
		}
		if (controller == null)
		{
			return;
		}
		KAnim.Anim currentAnim = this.masterController.GetCurrentAnim();
		if (currentAnim != null && !string.IsNullOrEmpty(controller.defaultAnim) && !controller.HasAnimation(currentAnim.name))
		{
			controller.Play(controller.defaultAnim, KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		if (currentAnim == null)
		{
			return;
		}
		KAnim.PlayMode mode = this.masterController.GetMode();
		float playSpeed = this.masterController.GetPlaySpeed();
		float elapsedTime = this.masterController.GetElapsedTime();
		controller.Play(currentAnim.name, mode, playSpeed, elapsedTime);
		Facing component = controller.GetComponent<Facing>();
		if (component != null)
		{
			float num = component.transform.GetPosition().x;
			num += (this.masterController.FlipX ? -0.5f : 0.5f);
			component.Face(num);
			return;
		}
		controller.FlipX = this.masterController.FlipX;
		controller.FlipY = this.masterController.FlipY;
	}

	// Token: 0x060029F0 RID: 10736 RVA: 0x001D770C File Offset: 0x001D590C
	public void SyncController(KAnimSynchronizedController controller)
	{
		if (this.masterController == null)
		{
			return;
		}
		if (controller == null)
		{
			return;
		}
		KAnim.Anim currentAnim = this.masterController.GetCurrentAnim();
		string s = (currentAnim != null) ? (currentAnim.name + controller.Postfix) : string.Empty;
		if (!string.IsNullOrEmpty(controller.synchronizedController.defaultAnim) && !controller.synchronizedController.HasAnimation(s))
		{
			controller.Play(controller.synchronizedController.defaultAnim, KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		if (currentAnim == null)
		{
			return;
		}
		KAnim.PlayMode mode = this.masterController.GetMode();
		float playSpeed = this.masterController.GetPlaySpeed();
		float elapsedTime = this.masterController.GetElapsedTime();
		controller.Play(s, mode, playSpeed, elapsedTime);
		Facing component = controller.synchronizedController.GetComponent<Facing>();
		if (component != null)
		{
			float num = component.transform.GetPosition().x;
			num += (this.masterController.FlipX ? -0.5f : 0.5f);
			component.Face(num);
			return;
		}
		controller.synchronizedController.FlipX = this.masterController.FlipX;
		controller.synchronizedController.FlipY = this.masterController.FlipY;
	}

	// Token: 0x060029F1 RID: 10737 RVA: 0x001D7854 File Offset: 0x001D5A54
	public void Sync()
	{
		for (int i = 0; i < this.Targets.Count; i++)
		{
			KAnimControllerBase controller = this.Targets[i];
			this.Sync(controller);
		}
		for (int j = 0; j < this.SyncedControllers.Count; j++)
		{
			KAnimSynchronizedController controller2 = this.SyncedControllers[j];
			this.SyncController(controller2);
		}
	}

	// Token: 0x060029F2 RID: 10738 RVA: 0x001D78B8 File Offset: 0x001D5AB8
	public void SyncTime()
	{
		float elapsedTime = this.masterController.GetElapsedTime();
		for (int i = 0; i < this.Targets.Count; i++)
		{
			this.Targets[i].SetElapsedTime(elapsedTime);
		}
		for (int j = 0; j < this.SyncedControllers.Count; j++)
		{
			this.SyncedControllers[j].synchronizedController.SetElapsedTime(elapsedTime);
		}
	}

	// Token: 0x04001BE8 RID: 7144
	private string idle_anim = "idle_default";

	// Token: 0x04001BE9 RID: 7145
	private KAnimControllerBase masterController;

	// Token: 0x04001BEA RID: 7146
	private List<KAnimControllerBase> Targets = new List<KAnimControllerBase>();

	// Token: 0x04001BEB RID: 7147
	private List<KAnimSynchronizedController> SyncedControllers = new List<KAnimSynchronizedController>();
}
