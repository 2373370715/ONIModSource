﻿using System;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Compostable")]
public class Compostable : KMonoBehaviour
{
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.isMarkedForCompost = base.GetComponent<KPrefabID>().HasTag(GameTags.Compostable);
		if (this.isMarkedForCompost)
		{
			this.MarkForCompost(false);
		}
		base.Subscribe<Compostable>(493375141, Compostable.OnRefreshUserMenuDelegate);
		base.Subscribe<Compostable>(856640610, Compostable.OnStoreDelegate);
	}

	private void MarkForCompost(bool force = false)
	{
		this.RefreshStatusItem();
		Storage storage = base.GetComponent<Pickupable>().storage;
		if (storage != null)
		{
			storage.Drop(base.gameObject, true);
		}
	}

	private void OnToggleCompost()
	{
		if (!this.isMarkedForCompost)
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component.storage != null)
			{
				component.storage.Drop(base.gameObject, true);
			}
			Pickupable pickupable = EntitySplitter.Split(component, component.TotalAmount, this.compostPrefab);
			if (pickupable != null)
			{
				SelectTool.Instance.SelectNextFrame(pickupable.GetComponent<KSelectable>(), true);
				return;
			}
		}
		else
		{
			Pickupable component2 = base.GetComponent<Pickupable>();
			Pickupable pickupable2 = EntitySplitter.Split(component2, component2.TotalAmount, this.originalPrefab);
			SelectTool.Instance.SelectNextFrame(pickupable2.GetComponent<KSelectable>(), true);
		}
	}

	private void RefreshStatusItem()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForCompost, false);
		component.RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForCompostInStorage, false);
		if (this.isMarkedForCompost)
		{
			if (base.GetComponent<Pickupable>() != null && base.GetComponent<Pickupable>().storage == null)
			{
				component.AddStatusItem(Db.Get().MiscStatusItems.MarkedForCompost, null);
				return;
			}
			component.AddStatusItem(Db.Get().MiscStatusItems.MarkedForCompostInStorage, null);
		}
	}

	private void OnStore(object data)
	{
		this.RefreshStatusItem();
	}

	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button;
		if (!this.isMarkedForCompost)
		{
			button = new KIconButtonMenu.ButtonInfo("action_compost", UI.USERMENUACTIONS.COMPOST.NAME, new System.Action(this.OnToggleCompost), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.COMPOST.TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_compost", UI.USERMENUACTIONS.COMPOST.NAME_OFF, new System.Action(this.OnToggleCompost), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.COMPOST.TOOLTIP_OFF, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	[SerializeField]
	public bool isMarkedForCompost;

	public GameObject originalPrefab;

	public GameObject compostPrefab;

	private static readonly EventSystem.IntraObjectHandler<Compostable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Compostable>(delegate(Compostable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	private static readonly EventSystem.IntraObjectHandler<Compostable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Compostable>(delegate(Compostable component, object data)
	{
		component.OnStore(data);
	});
}
