﻿using System;

namespace Database
{
		public class AccessorySlots : ResourceSet<AccessorySlot>
	{
				public AccessorySlots(ResourceSet parent) : base("AccessorySlots", parent)
		{
			parent = Db.Get().Accessories;
			KAnimFile anim = Assets.GetAnim("head_swap_kanim");
			KAnimFile anim2 = Assets.GetAnim("body_comp_default_kanim");
			KAnimFile anim3 = Assets.GetAnim("body_swap_kanim");
			KAnimFile anim4 = Assets.GetAnim("hair_swap_kanim");
			KAnimFile anim5 = Assets.GetAnim("hat_swap_kanim");
			this.Eyes = new AccessorySlot("Eyes", this, anim, 0);
			this.Hair = new AccessorySlot("Hair", this, anim4, 0);
			this.HeadShape = new AccessorySlot("HeadShape", this, anim, 0);
			this.Mouth = new AccessorySlot("Mouth", this, anim, 0);
			this.Hat = new AccessorySlot("Hat", this, anim5, 4);
			this.HatHair = new AccessorySlot("Hat_Hair", this, anim4, 0);
			this.HeadEffects = new AccessorySlot("HeadFX", this, anim, 0);
			this.Body = new AccessorySlot("Torso", this, new KAnimHashedString("torso"), anim3, null, 0);
			this.Arm = new AccessorySlot("Arm_Sleeve", this, new KAnimHashedString("arm_sleeve"), anim3, null, 0);
			this.ArmLower = new AccessorySlot("Arm_Lower_Sleeve", this, new KAnimHashedString("arm_lower_sleeve"), anim3, null, 0);
			this.Belt = new AccessorySlot("Belt", this, new KAnimHashedString("belt"), anim2, null, 0);
			this.Neck = new AccessorySlot("Neck", this, new KAnimHashedString("neck"), anim2, null, 0);
			this.Pelvis = new AccessorySlot("Pelvis", this, new KAnimHashedString("pelvis"), anim2, null, 0);
			this.Foot = new AccessorySlot("Foot", this, new KAnimHashedString("foot"), anim2, Assets.GetAnim("shoes_basic_black_kanim"), 0);
			this.Leg = new AccessorySlot("Leg", this, new KAnimHashedString("leg"), anim2, null, 0);
			this.Necklace = new AccessorySlot("Necklace", this, new KAnimHashedString("necklace"), anim2, null, 0);
			this.Cuff = new AccessorySlot("Cuff", this, new KAnimHashedString("cuff"), anim2, null, 0);
			this.Hand = new AccessorySlot("Hand", this, new KAnimHashedString("hand_paint"), anim2, null, 0);
			this.Skirt = new AccessorySlot("Skirt", this, new KAnimHashedString("skirt"), anim3, null, 0);
			this.ArmLowerSkin = new AccessorySlot("Arm_Lower", this, new KAnimHashedString("arm_lower"), anim3, null, 0);
			this.ArmUpperSkin = new AccessorySlot("Arm_Upper", this, new KAnimHashedString("arm_upper"), anim3, null, 0);
			this.LegSkin = new AccessorySlot("Leg_Skin", this, new KAnimHashedString("leg_skin"), anim3, null, 0);
			foreach (AccessorySlot accessorySlot in this.resources)
			{
				accessorySlot.AddAccessories(accessorySlot.AnimFile, parent);
			}
			Db.Get().Accessories.AddCustomAccessories(Assets.GetAnim("body_lonelyminion_kanim"), parent, this);
		}

				public AccessorySlot Find(KAnimHashedString symbol_name)
		{
			foreach (AccessorySlot accessorySlot in Db.Get().AccessorySlots.resources)
			{
				if (symbol_name == accessorySlot.targetSymbolId)
				{
					return accessorySlot;
				}
			}
			return null;
		}

				public AccessorySlot Eyes;

				public AccessorySlot Hair;

				public AccessorySlot HeadShape;

				public AccessorySlot Mouth;

				public AccessorySlot Body;

				public AccessorySlot Arm;

				public AccessorySlot ArmLower;

				public AccessorySlot Hat;

				public AccessorySlot HatHair;

				public AccessorySlot HeadEffects;

				public AccessorySlot Belt;

				public AccessorySlot Neck;

				public AccessorySlot Pelvis;

				public AccessorySlot Leg;

				public AccessorySlot Foot;

				public AccessorySlot Skirt;

				public AccessorySlot Necklace;

				public AccessorySlot Cuff;

				public AccessorySlot Hand;

				public AccessorySlot ArmLowerSkin;

				public AccessorySlot ArmUpperSkin;

				public AccessorySlot LegSkin;
	}
}
