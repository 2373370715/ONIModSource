using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/RationTracker")]
public class RationTracker : WorldResourceAmountTracker<RationTracker>, ISaveLoadable
{
		protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.itemTag = GameTags.Edible;
	}

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

		public float GetAmountConsumed()
	{
		float num = 0f;
		foreach (KeyValuePair<string, float> keyValuePair in this.amountsConsumedByID)
		{
			num += keyValuePair.Value;
		}
		return num;
	}

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

		[Serialize]
	public Dictionary<string, float> caloriesConsumedByFood = new Dictionary<string, float>();
}
