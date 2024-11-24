using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020019F5 RID: 6645
[AddComponentMenu("KMonoBehaviour/scripts/Trappable")]
public class Trappable : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06008A76 RID: 35446 RVA: 0x000FAA1C File Offset: 0x000F8C1C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
		this.OnCellChange();
	}

	// Token: 0x06008A77 RID: 35447 RVA: 0x000FAA30 File Offset: 0x000F8C30
	protected override void OnCleanUp()
	{
		this.Unregister();
		base.OnCleanUp();
	}

	// Token: 0x06008A78 RID: 35448 RVA: 0x0035B770 File Offset: 0x00359970
	private void OnCellChange()
	{
		int cell = Grid.PosToCell(this);
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.trapsLayer, this);
	}

	// Token: 0x06008A79 RID: 35449 RVA: 0x000FAA3E File Offset: 0x000F8C3E
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.Register();
	}

	// Token: 0x06008A7A RID: 35450 RVA: 0x000FAA4C File Offset: 0x000F8C4C
	protected override void OnCmpDisable()
	{
		this.Unregister();
		base.OnCmpDisable();
	}

	// Token: 0x06008A7B RID: 35451 RVA: 0x0035B79C File Offset: 0x0035999C
	private void Register()
	{
		if (this.registered)
		{
			return;
		}
		base.Subscribe<Trappable>(856640610, Trappable.OnStoreDelegate);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "Trappable.Register");
		this.registered = true;
	}

	// Token: 0x06008A7C RID: 35452 RVA: 0x000FAA5A File Offset: 0x000F8C5A
	private void Unregister()
	{
		if (!this.registered)
		{
			return;
		}
		base.Unsubscribe<Trappable>(856640610, Trappable.OnStoreDelegate, false);
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		this.registered = false;
	}

	// Token: 0x06008A7D RID: 35453 RVA: 0x000FAA99 File Offset: 0x000F8C99
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.BUILDINGEFFECTS.CAPTURE_METHOD_TRAP, UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_TRAP, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x06008A7E RID: 35454 RVA: 0x0035B7EC File Offset: 0x003599EC
	public void OnStore(object data)
	{
		Storage storage = data as Storage;
		if (storage && (storage.GetComponent<Trap>() != null || storage.GetSMI<ReusableTrap.Instance>() != null))
		{
			base.gameObject.AddTag(GameTags.Trapped);
			return;
		}
		base.gameObject.RemoveTag(GameTags.Trapped);
	}

	// Token: 0x04006841 RID: 26689
	private bool registered;

	// Token: 0x04006842 RID: 26690
	private static readonly EventSystem.IntraObjectHandler<Trappable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Trappable>(delegate(Trappable component, object data)
	{
		component.OnStore(data);
	});
}
