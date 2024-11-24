﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using UnityEngine;

// Token: 0x02000980 RID: 2432
[AddComponentMenu("KMonoBehaviour/scripts/Accessorizer")]
public class Accessorizer : KMonoBehaviour
{
	// Token: 0x06002C02 RID: 11266 RVA: 0x000BC859 File Offset: 0x000BAA59
	public List<ResourceRef<Accessory>> GetAccessories()
	{
		return this.accessories;
	}

	// Token: 0x06002C03 RID: 11267 RVA: 0x000BC861 File Offset: 0x000BAA61
	public void SetAccessories(List<ResourceRef<Accessory>> data)
	{
		this.accessories = data;
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x06002C04 RID: 11268 RVA: 0x000BC86A File Offset: 0x000BAA6A
	// (set) Token: 0x06002C05 RID: 11269 RVA: 0x000BC872 File Offset: 0x000BAA72
	public KCompBuilder.BodyData bodyData { get; set; }

	// Token: 0x06002C06 RID: 11270 RVA: 0x001EA014 File Offset: 0x001E8214
	[OnDeserialized]
	private void OnDeserialized()
	{
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		if (this.clothingItems.Count > 0 || (component != null && component.nameStringKey == "JORGE") || SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 30))
		{
			if (component != null)
			{
				this.bodyData = Accessorizer.UpdateAccessorySlots(component.nameStringKey, ref this.accessories);
			}
			this.accessories.RemoveAll((ResourceRef<Accessory> x) => x.Get() == null);
		}
		if (this.clothingItems.Count > 0)
		{
			base.GetComponent<WearableAccessorizer>().ApplyClothingItems(ClothingOutfitUtility.OutfitType.Clothing, from i in this.clothingItems
			select i.Get());
			this.clothingItems.Clear();
		}
		this.ApplyAccessories();
	}

	// Token: 0x06002C07 RID: 11271 RVA: 0x001EA110 File Offset: 0x001E8310
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		if (component != null)
		{
			this.bodyData = MinionStartingStats.CreateBodyData(Db.Get().Personalities.Get(component.personalityResourceId));
		}
	}

	// Token: 0x06002C08 RID: 11272 RVA: 0x001EA154 File Offset: 0x001E8354
	public void AddAccessory(Accessory accessory)
	{
		if (accessory != null)
		{
			if (this.animController == null)
			{
				this.animController = base.GetComponent<KAnimControllerBase>();
			}
			this.animController.GetComponent<SymbolOverrideController>().AddSymbolOverride(accessory.slot.targetSymbolId, accessory.symbol, accessory.slot.overrideLayer);
			if (!this.HasAccessory(accessory))
			{
				ResourceRef<Accessory> resourceRef = new ResourceRef<Accessory>(accessory);
				if (resourceRef != null)
				{
					this.accessories.Add(resourceRef);
				}
			}
		}
	}

	// Token: 0x06002C09 RID: 11273 RVA: 0x001EA1D0 File Offset: 0x001E83D0
	public void RemoveAccessory(Accessory accessory)
	{
		this.accessories.RemoveAll((ResourceRef<Accessory> x) => x.Get() == accessory);
		this.animController.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride(accessory.slot.targetSymbolId, accessory.slot.overrideLayer);
	}

	// Token: 0x06002C0A RID: 11274 RVA: 0x001EA238 File Offset: 0x001E8438
	public void ApplyAccessories()
	{
		foreach (ResourceRef<Accessory> resourceRef in this.accessories)
		{
			Accessory accessory = resourceRef.Get();
			if (accessory != null)
			{
				this.AddAccessory(accessory);
			}
		}
	}

	// Token: 0x06002C0B RID: 11275 RVA: 0x001EA294 File Offset: 0x001E8494
	public static KCompBuilder.BodyData UpdateAccessorySlots(string nameString, ref List<ResourceRef<Accessory>> accessories)
	{
		accessories.RemoveAll((ResourceRef<Accessory> acc) => acc.Get() == null);
		Personality personalityFromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(nameString);
		if (personalityFromNameStringKey != null)
		{
			KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(personalityFromNameStringKey);
			foreach (AccessorySlot accessorySlot in Db.Get().AccessorySlots.resources)
			{
				if (accessorySlot.accessories.Count != 0)
				{
					Accessory accessory = null;
					if (accessorySlot == Db.Get().AccessorySlots.Body)
					{
						accessory = accessorySlot.Lookup(bodyData.body);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Arm)
					{
						accessory = accessorySlot.Lookup(bodyData.arms);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.ArmLower)
					{
						accessory = accessorySlot.Lookup(bodyData.armslower);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.ArmLowerSkin)
					{
						accessory = accessorySlot.Lookup(bodyData.armLowerSkin);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.ArmUpperSkin)
					{
						accessory = accessorySlot.Lookup(bodyData.armUpperSkin);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.LegSkin)
					{
						accessory = accessorySlot.Lookup(bodyData.legSkin);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Leg)
					{
						accessory = accessorySlot.Lookup(bodyData.legs);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Belt)
					{
						accessory = accessorySlot.Lookup(bodyData.belt);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Neck)
					{
						accessory = accessorySlot.Lookup(bodyData.neck);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Pelvis)
					{
						accessory = accessorySlot.Lookup(bodyData.pelvis);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Foot)
					{
						accessory = accessorySlot.Lookup(bodyData.foot);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Cuff)
					{
						accessory = accessorySlot.Lookup(bodyData.cuff);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Hand)
					{
						accessory = accessorySlot.Lookup(bodyData.hand);
					}
					if (accessory != null)
					{
						ResourceRef<Accessory> item = new ResourceRef<Accessory>(accessory);
						accessories.RemoveAll((ResourceRef<Accessory> old_acc) => old_acc.Get().slot == accessory.slot);
						accessories.Add(item);
					}
				}
			}
			return bodyData;
		}
		return default(KCompBuilder.BodyData);
	}

	// Token: 0x06002C0C RID: 11276 RVA: 0x001EA5B4 File Offset: 0x001E87B4
	public bool HasAccessory(Accessory accessory)
	{
		return this.accessories.Exists((ResourceRef<Accessory> x) => x.Get() == accessory);
	}

	// Token: 0x06002C0D RID: 11277 RVA: 0x001EA5E8 File Offset: 0x001E87E8
	public Accessory GetAccessory(AccessorySlot slot)
	{
		for (int i = 0; i < this.accessories.Count; i++)
		{
			if (this.accessories[i].Get() != null && this.accessories[i].Get().slot == slot)
			{
				return this.accessories[i].Get();
			}
		}
		return null;
	}

	// Token: 0x06002C0E RID: 11278 RVA: 0x001EA64C File Offset: 0x001E884C
	public void ApplyMinionPersonality(Personality personality)
	{
		this.bodyData = MinionStartingStats.CreateBodyData(personality);
		this.accessories.Clear();
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
		}
		foreach (string text in new string[]
		{
			"snapTo_hat",
			"snapTo_hat_hair",
			"snapTo_goggles",
			"snapTo_headFX",
			"snapTo_neck",
			"snapTo_chest",
			"snapTo_pivot",
			"skirt",
			"necklace"
		})
		{
			this.animController.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(text, 0);
			this.animController.SetSymbolVisiblity(text, false);
		}
		this.AddAccessory(Db.Get().AccessorySlots.Eyes.Lookup(this.bodyData.eyes));
		this.AddAccessory(Db.Get().AccessorySlots.Hair.Lookup(this.bodyData.hair));
		this.AddAccessory(Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(this.bodyData.hair)));
		this.AddAccessory(Db.Get().AccessorySlots.HeadShape.Lookup(this.bodyData.headShape));
		this.AddAccessory(Db.Get().AccessorySlots.Mouth.Lookup(this.bodyData.mouth));
		this.AddAccessory(Db.Get().AccessorySlots.Body.Lookup(this.bodyData.body));
		this.AddAccessory(Db.Get().AccessorySlots.Arm.Lookup(this.bodyData.arms));
		this.AddAccessory(Db.Get().AccessorySlots.ArmLower.Lookup(this.bodyData.armslower));
		this.AddAccessory(Db.Get().AccessorySlots.Neck.Lookup(this.bodyData.neck));
		this.AddAccessory(Db.Get().AccessorySlots.Pelvis.Lookup(this.bodyData.pelvis));
		this.AddAccessory(Db.Get().AccessorySlots.Leg.Lookup(this.bodyData.legs));
		this.AddAccessory(Db.Get().AccessorySlots.Foot.Lookup(this.bodyData.foot));
		this.AddAccessory(Db.Get().AccessorySlots.Hand.Lookup(this.bodyData.hand));
		this.AddAccessory(Db.Get().AccessorySlots.Cuff.Lookup(this.bodyData.cuff));
		this.AddAccessory(Db.Get().AccessorySlots.Belt.Lookup(this.bodyData.belt));
		this.AddAccessory(Db.Get().AccessorySlots.ArmLowerSkin.Lookup(this.bodyData.armLowerSkin));
		this.AddAccessory(Db.Get().AccessorySlots.ArmUpperSkin.Lookup(this.bodyData.armUpperSkin));
		this.AddAccessory(Db.Get().AccessorySlots.LegSkin.Lookup(this.bodyData.legSkin));
		this.UpdateHairBasedOnHat();
	}

	// Token: 0x06002C0F RID: 11279 RVA: 0x001EA9CC File Offset: 0x001E8BCC
	public void ApplyBodyData(KCompBuilder.BodyData bodyData)
	{
		this.accessories.Clear();
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
		}
		foreach (string text in new string[]
		{
			"snapTo_hat",
			"snapTo_hat_hair",
			"snapTo_goggles",
			"snapTo_headFX",
			"snapTo_neck",
			"snapTo_chest",
			"snapTo_pivot",
			"skirt",
			"necklace"
		})
		{
			this.animController.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(text, 0);
			this.animController.SetSymbolVisiblity(text, false);
		}
		this.AddAccessory(Db.Get().AccessorySlots.Eyes.Lookup(bodyData.eyes));
		this.AddAccessory(Db.Get().AccessorySlots.Hair.Lookup(bodyData.hair));
		this.AddAccessory(Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(bodyData.hair)));
		this.AddAccessory(Db.Get().AccessorySlots.HeadShape.Lookup(bodyData.headShape));
		this.AddAccessory(Db.Get().AccessorySlots.Mouth.Lookup(bodyData.mouth));
		this.AddAccessory(Db.Get().AccessorySlots.Body.Lookup(bodyData.body));
		this.AddAccessory(Db.Get().AccessorySlots.Arm.Lookup(bodyData.arms));
		this.AddAccessory(Db.Get().AccessorySlots.ArmLower.Lookup(bodyData.armslower));
		this.AddAccessory(Db.Get().AccessorySlots.Neck.Lookup(bodyData.neck));
		this.AddAccessory(Db.Get().AccessorySlots.Pelvis.Lookup(bodyData.pelvis));
		this.AddAccessory(Db.Get().AccessorySlots.Leg.Lookup(bodyData.legs));
		this.AddAccessory(Db.Get().AccessorySlots.Foot.Lookup(bodyData.foot));
		this.AddAccessory(Db.Get().AccessorySlots.Hand.Lookup(bodyData.hand));
		this.AddAccessory(Db.Get().AccessorySlots.Cuff.Lookup(bodyData.cuff));
		this.AddAccessory(Db.Get().AccessorySlots.Belt.Lookup(bodyData.belt));
		this.AddAccessory(Db.Get().AccessorySlots.ArmLowerSkin.Lookup(bodyData.armLowerSkin));
		this.AddAccessory(Db.Get().AccessorySlots.ArmUpperSkin.Lookup(bodyData.armUpperSkin));
		this.AddAccessory(Db.Get().AccessorySlots.LegSkin.Lookup(bodyData.legSkin));
		this.UpdateHairBasedOnHat();
	}

	// Token: 0x06002C10 RID: 11280 RVA: 0x001EACE4 File Offset: 0x001E8EE4
	public void UpdateHairBasedOnHat()
	{
		if (!this.GetAccessory(Db.Get().AccessorySlots.Hat).IsNullOrDestroyed())
		{
			this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
			this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
			return;
		}
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
	}

	// Token: 0x06002C11 RID: 11281 RVA: 0x001EADB4 File Offset: 0x001E8FB4
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
		for (int i = 0; i < this.accessories.Count; i++)
		{
			Accessory accessory = this.accessories[i].Get();
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

	// Token: 0x04001D9C RID: 7580
	[Serialize]
	private List<ResourceRef<Accessory>> accessories = new List<ResourceRef<Accessory>>();

	// Token: 0x04001D9D RID: 7581
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x04001D9E RID: 7582
	[Serialize]
	private List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();
}
