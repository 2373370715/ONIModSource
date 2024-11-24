using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02001A5E RID: 6750
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class WorldResourceAmountTracker<T> : KMonoBehaviour where T : KMonoBehaviour
{
	// Token: 0x06008D38 RID: 36152 RVA: 0x000FC3C5 File Offset: 0x000FA5C5
	public static void DestroyInstance()
	{
		WorldResourceAmountTracker<T>.instance = default(T);
	}

	// Token: 0x06008D39 RID: 36153 RVA: 0x000FC3D2 File Offset: 0x000FA5D2
	public static T Get()
	{
		return WorldResourceAmountTracker<T>.instance;
	}

	// Token: 0x06008D3A RID: 36154 RVA: 0x00366E40 File Offset: 0x00365040
	protected override void OnPrefabInit()
	{
		Debug.Assert(WorldResourceAmountTracker<T>.instance == null, "Error, WorldResourceAmountTracker of type T has already been initialize and another instance is attempting to initialize. this isn't allowed because T is meant to be a singleton, ensure only one instance exist. existing instance GameObject: " + ((WorldResourceAmountTracker<T>.instance == null) ? "" : WorldResourceAmountTracker<T>.instance.gameObject.name) + ". Error triggered by instance of T in GameObject: " + base.gameObject.name);
		WorldResourceAmountTracker<T>.instance = (this as T);
		this.itemTag = GameTags.Edible;
	}

	// Token: 0x06008D3B RID: 36155 RVA: 0x000FC3D9 File Offset: 0x000FA5D9
	protected override void OnSpawn()
	{
		base.Subscribe(631075836, new Action<object>(this.OnNewDay));
	}

	// Token: 0x06008D3C RID: 36156 RVA: 0x000FC3F3 File Offset: 0x000FA5F3
	private void OnNewDay(object data)
	{
		this.previousFrame = this.currentFrame;
		this.currentFrame = default(WorldResourceAmountTracker<T>.Frame);
	}

	// Token: 0x06008D3D RID: 36157
	protected abstract WorldResourceAmountTracker<T>.ItemData GetItemData(Pickupable item);

	// Token: 0x06008D3E RID: 36158 RVA: 0x00366EC4 File Offset: 0x003650C4
	public float CountAmount(Dictionary<string, float> unitCountByID, WorldInventory inventory, bool excludeUnreachable = true)
	{
		float num = 0f;
		ICollection<Pickupable> pickupables = inventory.GetPickupables(this.itemTag, false);
		if (pickupables != null)
		{
			foreach (Pickupable pickupable in pickupables)
			{
				if (!pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
				{
					WorldResourceAmountTracker<T>.ItemData itemData = this.GetItemData(pickupable);
					num += itemData.amountValue;
					if (unitCountByID != null)
					{
						if (!unitCountByID.ContainsKey(itemData.ID))
						{
							unitCountByID[itemData.ID] = 0f;
						}
						string id = itemData.ID;
						unitCountByID[id] += itemData.units;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06008D3F RID: 36159 RVA: 0x000FC40D File Offset: 0x000FA60D
	public void RegisterAmountProduced(float val)
	{
		this.currentFrame.amountProduced = this.currentFrame.amountProduced + val;
	}

	// Token: 0x06008D40 RID: 36160 RVA: 0x00366F90 File Offset: 0x00365190
	public void RegisterAmountConsumed(string ID, float valueConsumed)
	{
		this.currentFrame.amountConsumed = this.currentFrame.amountConsumed + valueConsumed;
		if (!this.amountsConsumedByID.ContainsKey(ID))
		{
			this.amountsConsumedByID.Add(ID, valueConsumed);
			return;
		}
		Dictionary<string, float> dictionary = this.amountsConsumedByID;
		dictionary[ID] += valueConsumed;
	}

	// Token: 0x04006A18 RID: 27160
	private static T instance;

	// Token: 0x04006A19 RID: 27161
	[Serialize]
	public WorldResourceAmountTracker<T>.Frame currentFrame;

	// Token: 0x04006A1A RID: 27162
	[Serialize]
	public WorldResourceAmountTracker<T>.Frame previousFrame;

	// Token: 0x04006A1B RID: 27163
	[Serialize]
	public Dictionary<string, float> amountsConsumedByID = new Dictionary<string, float>();

	// Token: 0x04006A1C RID: 27164
	protected Tag itemTag;

	// Token: 0x02001A5F RID: 6751
	protected struct ItemData
	{
		// Token: 0x04006A1D RID: 27165
		public string ID;

		// Token: 0x04006A1E RID: 27166
		public float amountValue;

		// Token: 0x04006A1F RID: 27167
		public float units;
	}

	// Token: 0x02001A60 RID: 6752
	public struct Frame
	{
		// Token: 0x04006A20 RID: 27168
		public float amountProduced;

		// Token: 0x04006A21 RID: 27169
		public float amountConsumed;
	}
}
