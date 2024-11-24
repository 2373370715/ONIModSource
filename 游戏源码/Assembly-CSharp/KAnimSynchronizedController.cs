using System;
using UnityEngine;

// Token: 0x0200091F RID: 2335
public class KAnimSynchronizedController
{
	// Token: 0x1700014A RID: 330
	// (get) Token: 0x060029DE RID: 10718 RVA: 0x000BB3E0 File Offset: 0x000B95E0
	// (set) Token: 0x060029DF RID: 10719 RVA: 0x000BB3E8 File Offset: 0x000B95E8
	public string Postfix
	{
		get
		{
			return this.postfix;
		}
		set
		{
			this.postfix = value;
		}
	}

	// Token: 0x060029E0 RID: 10720 RVA: 0x001D7398 File Offset: 0x001D5598
	public KAnimSynchronizedController(KAnimControllerBase controller, Grid.SceneLayer layer, string postfix)
	{
		this.controller = controller;
		this.Postfix = postfix;
		GameObject gameObject = Util.KInstantiate(EntityPrefabs.Instance.ForegroundLayer, controller.gameObject, null);
		gameObject.name = controller.name + postfix;
		this.synchronizedController = gameObject.GetComponent<KAnimControllerBase>();
		this.synchronizedController.AnimFiles = controller.AnimFiles;
		gameObject.SetActive(true);
		this.synchronizedController.initialAnim = controller.initialAnim + postfix;
		this.synchronizedController.defaultAnim = this.synchronizedController.initialAnim;
		Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(layer) - 0.1f);
		gameObject.transform.SetLocalPosition(position);
		this.link = new KAnimLink(controller, this.synchronizedController);
		this.Dirty();
		KAnimSynchronizer synchronizer = controller.GetSynchronizer();
		synchronizer.Add(this);
		synchronizer.SyncController(this);
	}

	// Token: 0x060029E1 RID: 10721 RVA: 0x000BB3F1 File Offset: 0x000B95F1
	public void Enable(bool enable)
	{
		this.synchronizedController.enabled = enable;
	}

	// Token: 0x060029E2 RID: 10722 RVA: 0x000BB3FF File Offset: 0x000B95FF
	public void Play(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		if (this.synchronizedController.enabled && this.synchronizedController.HasAnimation(anim_name))
		{
			this.synchronizedController.Play(anim_name, mode, speed, time_offset);
		}
	}

	// Token: 0x060029E3 RID: 10723 RVA: 0x001D7488 File Offset: 0x001D5688
	public void Dirty()
	{
		if (this.synchronizedController == null)
		{
			return;
		}
		this.synchronizedController.Offset = this.controller.Offset;
		this.synchronizedController.Pivot = this.controller.Pivot;
		this.synchronizedController.Rotation = this.controller.Rotation;
		this.synchronizedController.FlipX = this.controller.FlipX;
		this.synchronizedController.FlipY = this.controller.FlipY;
	}

	// Token: 0x04001BE4 RID: 7140
	private KAnimControllerBase controller;

	// Token: 0x04001BE5 RID: 7141
	public KAnimControllerBase synchronizedController;

	// Token: 0x04001BE6 RID: 7142
	private KAnimLink link;

	// Token: 0x04001BE7 RID: 7143
	private string postfix;
}
