using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/KBatchedAnimEventToggler")]
public class KBatchedAnimEventToggler : KMonoBehaviour
{
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

	protected override void OnSpawn()
	{
		this.animEventHandler = base.GetComponentInParent<AnimEventHandler>();
	}

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

	private void Disable(object data)
	{
		this.StopAll();
	}

	private void StopAll()
	{
		foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
		{
			entry.controller.StopAndClear();
			entry.controller.gameObject.SetActive(false);
		}
	}

	[SerializeField]
	public GameObject eventSource;

	[SerializeField]
	public string enableEvent;

	[SerializeField]
	public string disableEvent;

	[SerializeField]
	public List<KBatchedAnimEventToggler.Entry> entries;

	private AnimEventHandler animEventHandler;

	[Serializable]
	public struct Entry
	{
		public string anim;

		public HashedString context;

		public KBatchedAnimController controller;
	}
}
