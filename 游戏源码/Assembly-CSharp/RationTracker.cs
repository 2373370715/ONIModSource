using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02001735 RID: 5941
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/RationTracker")]
public class RationTracker : WorldResourceAmountTracker<RationTracker>, ISaveLoadable
{
	// Token: 0x06007A4C RID: 31308 RVA: 0x000F0580 File Offset: 0x000EE780
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.itemTag = GameTags.Edible;
	}

	// Token: 0x06007A4D RID: 31309 RVA: 0x0031806C File Offset: 0x0031626C
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.caloriesConsumedByFood != null && this.caloriesConsumedByFood.Count > 0)
		{
			foreach (string key in this.caloriesConsumedByFood.Keys)
			{
				float num = this.caloriesConsumedByFood[key];
				float num2 = 0f;
				if (this.amountsConsumedByID.TryGetValue(key, out num2))
				{
					this.amountsConsumedByID[key] = num2 + num;
				}
				else
				{
					this.amountsConsumedByID.Add(key, num);
				}
			}
		}
		this.caloriesConsumedByFood = null;
	}

	// Token: 0x06007A4E RID: 31310 RVA: 0x00318120 File Offset: 0x00316320
	protected override WorldResourceAmountTracker<RationTracker>.ItemData GetItemData(Pickupable item)
	{
		Edible component = item.GetComponent<Edible>();
		return new WorldResourceAmountTracker<RationTracker>.ItemData
		{
			ID = component.FoodID,
			amountValue = component.Calories,
			units = component.Units
		};
	}

	// Token: 0x06007A4F RID: 31311 RVA: 0x00318164 File Offset: 0x00316364
	public float GetAmountConsumed()
	{
		float num = 0f;
		foreach (KeyValuePair<string, float> keyValuePair in this.amountsConsumedByID)
		{
			num += keyValuePair.Value;
		}
		return num;
	}

	// Token: 0x06007A50 RID: 31312 RVA: 0x003181C4 File Offset: 0x003163C4
	public float GetAmountConsumedForIDs(List<string> itemIDs)
	{
		float num = 0f;
		foreach (string key in itemIDs)
		{
			if (this.amountsConsumedByID.ContainsKey(key))
			{
				num += this.amountsConsumedByID[key];
			}
		}
		return num;
	}

	// Token: 0x06007A51 RID: 31313 RVA: 0x00318230 File Offset: 0x00316430
	public float CountAmountForItemWithID(string ID, WorldInventory inventory, bool excludeUnreachable = true)
	{
		float num = 0f;
		ICollection<Pickupable> pickupables = inventory.GetPickupables(this.itemTag, false);
		if (pickupables != null)
		{
			foreach (Pickupable pickupable in pickupables)
			{
				if (!pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
				{
					WorldResourceAmountTracker<RationTracker>.ItemData itemData = this.GetItemData(pickupable);
					if (itemData.ID == ID)
					{
						num += itemData.amountValue;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x04005BCD RID: 23501
	[Serialize]
	public Dictionary<string, float> caloriesConsumedByFood = new Dictionary<string, float>();
}
