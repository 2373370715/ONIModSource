using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Accessorizer")]
public class Accessorizer : KMonoBehaviour
{
	[Serialize]
	private List<ResourceRef<Accessory>> accessories = new List<ResourceRef<Accessory>>();

	[MyCmpReq]
	private KAnimControllerBase animController;

	[Serialize]
	private List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();

	public KCompBuilder.BodyData bodyData { get; set; }

	public List<ResourceRef<Accessory>> GetAccessories()
	{
		return accessories;
	}

	public void SetAccessories(List<ResourceRef<Accessory>> data)
	{
		accessories = data;
	}

	[OnDeserialized]
	private void OnDeserialized()
	{
		MinionIdentity component = GetComponent<MinionIdentity>();
		if (clothingItems.Count > 0 || (component != null && component.nameStringKey == "JORGE") || SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 30))
		{
			if (component != null)
			{
				bodyData = UpdateAccessorySlots(component.nameStringKey, ref accessories);
			}
			accessories.RemoveAll((ResourceRef<Accessory> x) => x.Get() == null);
		}
		if (clothingItems.Count > 0)
		{
			GetComponent<WearableAccessorizer>().ApplyClothingItems(ClothingOutfitUtility.OutfitType.Clothing, clothingItems.Select((ResourceRef<ClothingItemResource> i) => i.Get()));
			clothingItems.Clear();
		}
		ApplyAccessories();
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		MinionIdentity component = GetComponent<MinionIdentity>();
		if (component != null)
		{
			bodyData = MinionStartingStats.CreateBodyData(Db.Get().Personalities.Get(component.personalityResourceId));
		}
	}

	public void AddAccessory(Accessory accessory)
	{
		if (accessory == null)
		{
			return;
		}
		if (animController == null)
		{
			animController = GetComponent<KAnimControllerBase>();
		}
		animController.GetComponent<SymbolOverrideController>().AddSymbolOverride(accessory.slot.targetSymbolId, accessory.symbol, accessory.slot.overrideLayer);
		if (!HasAccessory(accessory))
		{
			ResourceRef<Accessory> resourceRef = new ResourceRef<Accessory>(accessory);
			if (resourceRef != null)
			{
				accessories.Add(resourceRef);
			}
		}
	}

	public void RemoveAccessory(Accessory accessory)
	{
		accessories.RemoveAll((ResourceRef<Accessory> x) => x.Get() == accessory);
		animController.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride(accessory.slot.targetSymbolId, accessory.slot.overrideLayer);
	}

	public void ApplyAccessories()
	{
		foreach (ResourceRef<Accessory> accessory2 in accessories)
		{
			Accessory accessory = accessory2.Get();
			if (accessory != null)
			{
				AddAccessory(accessory);
			}
		}
	}

	public static KCompBuilder.BodyData UpdateAccessorySlots(string nameString, ref List<ResourceRef<Accessory>> accessories)
	{
		accessories.RemoveAll((ResourceRef<Accessory> acc) => acc.Get() == null);
		Personality personalityFromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(nameString);
		if (personalityFromNameStringKey != null)
		{
			KCompBuilder.BodyData result = MinionStartingStats.CreateBodyData(personalityFromNameStringKey);
			{
				foreach (AccessorySlot resource in Db.Get().AccessorySlots.resources)
				{
					if (resource.accessories.Count == 0)
					{
						continue;
					}
					Accessory accessory = null;
					if (resource == Db.Get().AccessorySlots.Body)
					{
						accessory = resource.Lookup(result.body);
					}
					else if (resource == Db.Get().AccessorySlots.Arm)
					{
						accessory = resource.Lookup(result.arms);
					}
					else if (resource == Db.Get().AccessorySlots.ArmLower)
					{
						accessory = resource.Lookup(result.armslower);
					}
					else if (resource == Db.Get().AccessorySlots.ArmLowerSkin)
					{
						accessory = resource.Lookup(result.armLowerSkin);
					}
					else if (resource == Db.Get().AccessorySlots.ArmUpperSkin)
					{
						accessory = resource.Lookup(result.armUpperSkin);
					}
					else if (resource == Db.Get().AccessorySlots.LegSkin)
					{
						accessory = resource.Lookup(result.legSkin);
					}
					else if (resource == Db.Get().AccessorySlots.Leg)
					{
						accessory = resource.Lookup(result.legs);
					}
					else if (resource == Db.Get().AccessorySlots.Belt)
					{
						accessory = resource.Lookup(result.belt);
					}
					else if (resource == Db.Get().AccessorySlots.Neck)
					{
						accessory = resource.Lookup(result.neck);
					}
					else if (resource == Db.Get().AccessorySlots.Pelvis)
					{
						accessory = resource.Lookup(result.pelvis);
					}
					else if (resource == Db.Get().AccessorySlots.Foot)
					{
						accessory = resource.Lookup(result.foot);
					}
					else if (resource == Db.Get().AccessorySlots.Cuff)
					{
						accessory = resource.Lookup(result.cuff);
					}
					else if (resource == Db.Get().AccessorySlots.Hand)
					{
						accessory = resource.Lookup(result.hand);
					}
					if (accessory != null)
					{
						ResourceRef<Accessory> item = new ResourceRef<Accessory>(accessory);
						accessories.RemoveAll((ResourceRef<Accessory> old_acc) => old_acc.Get().slot == accessory.slot);
						accessories.Add(item);
					}
				}
				return result;
			}
		}
		return default(KCompBuilder.BodyData);
	}

	public bool HasAccessory(Accessory accessory)
	{
		return accessories.Exists((ResourceRef<Accessory> x) => x.Get() == accessory);
	}

	public Accessory GetAccessory(AccessorySlot slot)
	{
		for (int i = 0; i < accessories.Count; i++)
		{
			if (accessories[i].Get() != null && accessories[i].Get().slot == slot)
			{
				return accessories[i].Get();
			}
		}
		return null;
	}

	public void ApplyMinionPersonality(Personality personality)
	{
		bodyData = MinionStartingStats.CreateBodyData(personality);
		accessories.Clear();
		if (animController == null)
		{
			animController = GetComponent<KAnimControllerBase>();
		}
		string[] array = new string[9] { "snapTo_hat", "snapTo_hat_hair", "snapTo_goggles", "snapTo_headFX", "snapTo_neck", "snapTo_chest", "snapTo_pivot", "skirt", "necklace" };
		foreach (string text in array)
		{
			animController.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(text);
			animController.SetSymbolVisiblity(text, is_visible: false);
		}
		AddAccessory(Db.Get().AccessorySlots.Eyes.Lookup(bodyData.eyes));
		AddAccessory(Db.Get().AccessorySlots.Hair.Lookup(bodyData.hair));
		AddAccessory(Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(bodyData.hair)));
		AddAccessory(Db.Get().AccessorySlots.HeadShape.Lookup(bodyData.headShape));
		AddAccessory(Db.Get().AccessorySlots.Mouth.Lookup(bodyData.mouth));
		AddAccessory(Db.Get().AccessorySlots.Body.Lookup(bodyData.body));
		AddAccessory(Db.Get().AccessorySlots.Arm.Lookup(bodyData.arms));
		AddAccessory(Db.Get().AccessorySlots.ArmLower.Lookup(bodyData.armslower));
		AddAccessory(Db.Get().AccessorySlots.Neck.Lookup(bodyData.neck));
		AddAccessory(Db.Get().AccessorySlots.Pelvis.Lookup(bodyData.pelvis));
		AddAccessory(Db.Get().AccessorySlots.Leg.Lookup(bodyData.legs));
		AddAccessory(Db.Get().AccessorySlots.Foot.Lookup(bodyData.foot));
		AddAccessory(Db.Get().AccessorySlots.Hand.Lookup(bodyData.hand));
		AddAccessory(Db.Get().AccessorySlots.Cuff.Lookup(bodyData.cuff));
		AddAccessory(Db.Get().AccessorySlots.Belt.Lookup(bodyData.belt));
		AddAccessory(Db.Get().AccessorySlots.ArmLowerSkin.Lookup(bodyData.armLowerSkin));
		AddAccessory(Db.Get().AccessorySlots.ArmUpperSkin.Lookup(bodyData.armUpperSkin));
		AddAccessory(Db.Get().AccessorySlots.LegSkin.Lookup(bodyData.legSkin));
		UpdateHairBasedOnHat();
	}

	public void UpdateHairBasedOnHat()
	{
		if (!GetAccessory(Db.Get().AccessorySlots.Hat).IsNullOrDestroyed())
		{
			animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, is_visible: false);
			animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, is_visible: true);
		}
		else
		{
			animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, is_visible: true);
			animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, is_visible: false);
			animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, is_visible: false);
		}
	}

	public void GetBodySlots(ref KCompBuilder.BodyData fd)
	{
		fd.eyes = HashedString.Invalid;
		fd.hair = HashedString.Invalid;
		fd.headShape = HashedString.Invalid;
		fd.mouth = HashedString.Invalid;
		fd.neck = HashedString.Invalid;
		fd.body = HashedString.Invalid;
		fd.arms = HashedString.Invalid;
		fd.armslower = HashedString.Invalid;
		fd.hat = HashedString.Invalid;
		fd.faceFX = HashedString.Invalid;
		fd.armLowerSkin = HashedString.Invalid;
		fd.armUpperSkin = HashedString.Invalid;
		fd.legSkin = HashedString.Invalid;
		fd.belt = HashedString.Invalid;
		fd.pelvis = HashedString.Invalid;
		fd.foot = HashedString.Invalid;
		fd.skirt = HashedString.Invalid;
		fd.necklace = HashedString.Invalid;
		fd.cuff = HashedString.Invalid;
		fd.hand = HashedString.Invalid;
		for (int i = 0; i < accessories.Count; i++)
		{
			Accessory accessory = accessories[i].Get();
			if (accessory != null)
			{
				if (accessory.slot.Id == "Eyes")
				{
					fd.eyes = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Hair")
				{
					fd.hair = accessory.IdHash;
				}
				else if (accessory.slot.Id == "HeadShape")
				{
					fd.headShape = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Mouth")
				{
					fd.mouth = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Neck")
				{
					fd.neck = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Torso")
				{
					fd.body = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Arm_Sleeve")
				{
					fd.arms = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Arm_Lower_Sleeve")
				{
					fd.armslower = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Hat")
				{
					fd.hat = HashedString.Invalid;
				}
				else if (accessory.slot.Id == "FaceEffect")
				{
					fd.faceFX = HashedString.Invalid;
				}
				else if (accessory.slot.Id == "Arm_Lower")
				{
					fd.armLowerSkin = accessory.Id;
				}
				else if (accessory.slot.Id == "Arm_Upper")
				{
					fd.armUpperSkin = accessory.Id;
				}
				else if (accessory.slot.Id == "Leg_Skin")
				{
					fd.legSkin = accessory.Id;
				}
				else if (accessory.slot.Id == "Leg")
				{
					fd.legs = accessory.Id;
				}
				else if (accessory.slot.Id == "Belt")
				{
					fd.belt = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Pelvis")
				{
					fd.pelvis = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Foot")
				{
					fd.foot = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Cuff")
				{
					fd.cuff = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Skirt")
				{
					fd.skirt = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Hand")
				{
					fd.hand = accessory.IdHash;
				}
			}
		}
	}
}
