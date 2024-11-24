using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000922 RID: 2338
[AddComponentMenu("KMonoBehaviour/scripts/KBatchedAnimEventToggler")]
public class KBatchedAnimEventToggler : KMonoBehaviour
{
	// Token: 0x06002A3C RID: 10812 RVA: 0x001D9114 File Offset: 0x001D7314
	protected override void OnPrefabInit()
	{
		Vector3 position = this.eventSource.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
		int layer = LayerMask.NameToLayer("Default");
		foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
		{
			entry.controller.transform.SetPosition(position);
			entry.controller.SetLayer(layer);
			entry.controller.gameObject.SetActive(false);
		}
		int hash = Hash.SDBMLower(this.enableEvent);
		int hash2 = Hash.SDBMLower(this.disableEvent);
		base.Subscribe(this.eventSource, hash, new Action<object>(this.Enable));
		base.Subscribe(this.eventSource, hash2, new Action<object>(this.Disable));
	}

	// Token: 0x06002A3D RID: 10813 RVA: 0x000BB7DA File Offset: 0x000B99DA
	protected override void OnSpawn()
	{
		this.animEventHandler = base.GetComponentInParent<AnimEventHandler>();
	}

	// Token: 0x06002A3E RID: 10814 RVA: 0x001D9204 File Offset: 0x001D7404
	private void Enable(object data)
	{
		this.StopAll();
		HashedString context = this.animEventHandler.GetContext();
		if (!context.IsValid)
		{
			return;
		}
		foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
		{
			if (entry.context == context)
			{
				entry.controller.gameObject.SetActive(true);
				entry.controller.Play(entry.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}
	}

	// Token: 0x06002A3F RID: 10815 RVA: 0x000BB7E8 File Offset: 0x000B99E8
	private void Disable(object data)
	{
		this.StopAll();
	}

	// Token: 0x06002A40 RID: 10816 RVA: 0x001D92AC File Offset: 0x001D74AC
	private void StopAll()
	{
		foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
		{
			entry.controller.StopAndClear();
			entry.controller.gameObject.SetActive(false);
		}
	}

	// Token: 0x04001C05 RID: 7173
	[SerializeField]
	public GameObject eventSource;

	// Token: 0x04001C06 RID: 7174
	[SerializeField]
	public string enableEvent;

	// Token: 0x04001C07 RID: 7175
	[SerializeField]
	public string disableEvent;

	// Token: 0x04001C08 RID: 7176
	[SerializeField]
	public List<KBatchedAnimEventToggler.Entry> entries;

	// Token: 0x04001C09 RID: 7177
	private AnimEventHandler animEventHandler;

	// Token: 0x02000923 RID: 2339
	[Serializable]
	public struct Entry
	{
		// Token: 0x04001C0A RID: 7178
		public string anim;

		// Token: 0x04001C0B RID: 7179
		public HashedString context;

		// Token: 0x04001C0C RID: 7180
		public KBatchedAnimController controller;
	}
}
