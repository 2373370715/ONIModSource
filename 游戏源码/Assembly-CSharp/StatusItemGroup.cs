using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B16 RID: 2838
public class StatusItemGroup
{
	// Token: 0x06003552 RID: 13650 RVA: 0x000C2C27 File Offset: 0x000C0E27
	public IEnumerator<StatusItemGroup.Entry> GetEnumerator()
	{
		return this.items.GetEnumerator();
	}

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06003553 RID: 13651 RVA: 0x000C2C39 File Offset: 0x000C0E39
	// (set) Token: 0x06003554 RID: 13652 RVA: 0x000C2C41 File Offset: 0x000C0E41
	public GameObject gameObject { get; private set; }

	// Token: 0x06003555 RID: 13653 RVA: 0x000C2C4A File Offset: 0x000C0E4A
	public StatusItemGroup(GameObject go)
	{
		this.gameObject = go;
	}

	// Token: 0x06003556 RID: 13654 RVA: 0x000C2C7E File Offset: 0x000C0E7E
	public void SetOffset(Vector3 offset)
	{
		this.offset = offset;
		Game.Instance.SetStatusItemOffset(this.gameObject.transform, offset);
	}

	// Token: 0x06003557 RID: 13655 RVA: 0x0020E3CC File Offset: 0x0020C5CC
	public StatusItemGroup.Entry GetStatusItem(StatusItemCategory category)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].category == category)
			{
				return this.items[i];
			}
		}
		return StatusItemGroup.Entry.EmptyEntry;
	}

	// Token: 0x06003558 RID: 13656 RVA: 0x0020E418 File Offset: 0x0020C618
	public Guid SetStatusItem(StatusItemCategory category, StatusItem item, object data = null)
	{
		if (item != null && item.allowMultiples)
		{
			throw new ArgumentException(item.Name + " allows multiple instances of itself to be active so you must access it via its handle");
		}
		if (category == null)
		{
			throw new ArgumentException("SetStatusItem requires a category.");
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].category == category)
			{
				if (this.items[i].item == item)
				{
					this.Log("Set (exists in category)", item, this.items[i].id, category);
					return this.items[i].id;
				}
				this.Log("Set->Remove existing in category", item, this.items[i].id, category);
				this.RemoveStatusItem(this.items[i].id, false);
			}
		}
		if (item != null)
		{
			Guid guid = this.AddStatusItem(item, data, category);
			this.Log("Set (new)", item, guid, category);
			return guid;
		}
		this.Log("Set (failed)", item, Guid.Empty, category);
		return Guid.Empty;
	}

	// Token: 0x06003559 RID: 13657 RVA: 0x000C2C9D File Offset: 0x000C0E9D
	public void SetStatusItem(Guid guid, StatusItemCategory category, StatusItem new_item, object data = null)
	{
		this.RemoveStatusItem(guid, false);
		if (new_item != null)
		{
			this.AddStatusItem(new_item, data, category);
		}
	}

	// Token: 0x0600355A RID: 13658 RVA: 0x0020E534 File Offset: 0x0020C734
	public bool HasStatusItem(StatusItem status_item)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].item.Id == status_item.Id)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600355B RID: 13659 RVA: 0x0020E580 File Offset: 0x0020C780
	public bool HasStatusItemID(string status_item_id)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].item.Id == status_item_id)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600355C RID: 13660 RVA: 0x0020E5C4 File Offset: 0x0020C7C4
	public Guid AddStatusItem(StatusItem item, object data = null, StatusItemCategory category = null)
	{
		if (this.gameObject == null || (!item.allowMultiples && this.HasStatusItem(item)))
		{
			return Guid.Empty;
		}
		if (!item.allowMultiples)
		{
			using (List<StatusItemGroup.Entry>.Enumerator enumerator = this.items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.item.Id == item.Id)
					{
						throw new ArgumentException("Tried to add " + item.Id + " multiples times which is not permitted.");
					}
				}
			}
		}
		StatusItemGroup.Entry entry = new StatusItemGroup.Entry(item, category, data);
		if (item.shouldNotify)
		{
			entry.notification = new Notification(item.notificationText, item.notificationType, new Func<List<Notification>, object, string>(StatusItemGroup.OnToolTip), item, false, 0f, item.notificationClickCallback, data, null, true, false, false);
			this.gameObject.AddOrGet<Notifier>().Add(entry.notification, "");
		}
		if (item.ShouldShowIcon())
		{
			Game.Instance.AddStatusItem(this.gameObject.transform, item);
			Game.Instance.SetStatusItemOffset(this.gameObject.transform, this.offset);
		}
		this.items.Add(entry);
		if (this.OnAddStatusItem != null)
		{
			this.OnAddStatusItem(entry, category);
		}
		return entry.id;
	}

	// Token: 0x0600355D RID: 13661 RVA: 0x0020E734 File Offset: 0x0020C934
	public Guid RemoveStatusItem(StatusItem status_item, bool immediate = false)
	{
		if (status_item.allowMultiples)
		{
			throw new ArgumentException(status_item.Name + " allows multiple instances of itself to be active so it must be released via an instance handle");
		}
		int i = 0;
		while (i < this.items.Count)
		{
			if (this.items[i].item.Id == status_item.Id)
			{
				Guid id = this.items[i].id;
				if (id == Guid.Empty)
				{
					return id;
				}
				this.RemoveStatusItemInternal(id, i, immediate);
				return id;
			}
			else
			{
				i++;
			}
		}
		return Guid.Empty;
	}

	// Token: 0x0600355E RID: 13662 RVA: 0x0020E7CC File Offset: 0x0020C9CC
	public Guid RemoveStatusItem(Guid guid, bool immediate = false)
	{
		if (guid == Guid.Empty)
		{
			return guid;
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].id == guid)
			{
				this.RemoveStatusItemInternal(guid, i, immediate);
				return guid;
			}
		}
		return Guid.Empty;
	}

	// Token: 0x0600355F RID: 13663 RVA: 0x0020E828 File Offset: 0x0020CA28
	private void RemoveStatusItemInternal(Guid guid, int itemIdx, bool immediate)
	{
		StatusItemGroup.Entry entry = this.items[itemIdx];
		this.items.RemoveAt(itemIdx);
		if (entry.notification != null)
		{
			this.gameObject.GetComponent<Notifier>().Remove(entry.notification);
		}
		if (entry.item.ShouldShowIcon() && Game.Instance != null)
		{
			Game.Instance.RemoveStatusItem(this.gameObject.transform, entry.item);
		}
		if (this.OnRemoveStatusItem != null)
		{
			this.OnRemoveStatusItem(entry, immediate);
		}
	}

	// Token: 0x06003560 RID: 13664 RVA: 0x000C2CB6 File Offset: 0x000C0EB6
	private static string OnToolTip(List<Notification> notifications, object data)
	{
		return ((StatusItem)data).notificationTooltipText + notifications.ReduceMessages(true);
	}

	// Token: 0x06003561 RID: 13665 RVA: 0x000C2CCF File Offset: 0x000C0ECF
	public void Destroy()
	{
		if (Game.IsQuitting())
		{
			return;
		}
		while (this.items.Count > 0)
		{
			this.RemoveStatusItem(this.items[0].id, false);
		}
	}

	// Token: 0x06003562 RID: 13666 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("ENABLE_LOGGER")]
	private void Log(string action, StatusItem item, Guid guid)
	{
	}

	// Token: 0x06003563 RID: 13667 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void Log(string action, StatusItem item, Guid guid, StatusItemCategory category)
	{
	}

	// Token: 0x04002449 RID: 9289
	private List<StatusItemGroup.Entry> items = new List<StatusItemGroup.Entry>();

	// Token: 0x0400244A RID: 9290
	public Action<StatusItemGroup.Entry, StatusItemCategory> OnAddStatusItem;

	// Token: 0x0400244B RID: 9291
	public Action<StatusItemGroup.Entry, bool> OnRemoveStatusItem;

	// Token: 0x0400244D RID: 9293
	private Vector3 offset = new Vector3(0f, 0f, 0f);

	// Token: 0x02000B17 RID: 2839
	public struct Entry : IComparable<StatusItemGroup.Entry>, IEquatable<StatusItemGroup.Entry>
	{
		// Token: 0x06003564 RID: 13668 RVA: 0x000C2D00 File Offset: 0x000C0F00
		public Entry(StatusItem item, StatusItemCategory category, object data)
		{
			this.id = Guid.NewGuid();
			this.item = item;
			this.data = data;
			this.category = category;
			this.notification = null;
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x000C2D29 File Offset: 0x000C0F29
		public string GetName()
		{
			return this.item.GetName(this.data);
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x000C2D3C File Offset: 0x000C0F3C
		public void ShowToolTip(ToolTip tooltip_widget, TextStyleSetting property_style)
		{
			this.item.ShowToolTip(tooltip_widget, this.data, property_style);
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x000C2D51 File Offset: 0x000C0F51
		public void SetIcon(Image image)
		{
			this.item.SetIcon(image, this.data);
		}

		// Token: 0x06003568 RID: 13672 RVA: 0x000C2D65 File Offset: 0x000C0F65
		public int CompareTo(StatusItemGroup.Entry other)
		{
			return this.id.CompareTo(other.id);
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x000C2D78 File Offset: 0x000C0F78
		public bool Equals(StatusItemGroup.Entry other)
		{
			return this.id == other.id;
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x000C2D8B File Offset: 0x000C0F8B
		public void OnClick()
		{
			this.item.OnClick(this.data);
		}

		// Token: 0x0400244E RID: 9294
		public static StatusItemGroup.Entry EmptyEntry = new StatusItemGroup.Entry
		{
			id = Guid.Empty
		};

		// Token: 0x0400244F RID: 9295
		public Guid id;

		// Token: 0x04002450 RID: 9296
		public StatusItem item;

		// Token: 0x04002451 RID: 9297
		public object data;

		// Token: 0x04002452 RID: 9298
		public Notification notification;

		// Token: 0x04002453 RID: 9299
		public StatusItemCategory category;
	}
}
