using System;

namespace Database
{
	// Token: 0x02002109 RID: 8457
	public class AccessorySlots : ResourceSet<AccessorySlot>
	{
		// Token: 0x0600B3D9 RID: 46041 RVA: 0x0043B088 File Offset: 0x00439288
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

		// Token: 0x0600B3DA RID: 46042 RVA: 0x0043B3C0 File Offset: 0x004395C0
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

		// Token: 0x04008DD6 RID: 36310
		public AccessorySlot Eyes;

		// Token: 0x04008DD7 RID: 36311
		public AccessorySlot Hair;

		// Token: 0x04008DD8 RID: 36312
		public AccessorySlot HeadShape;

		// Token: 0x04008DD9 RID: 36313
		public AccessorySlot Mouth;

		// Token: 0x04008DDA RID: 36314
		public AccessorySlot Body;

		// Token: 0x04008DDB RID: 36315
		public AccessorySlot Arm;

		// Token: 0x04008DDC RID: 36316
		public AccessorySlot ArmLower;

		// Token: 0x04008DDD RID: 36317
		public AccessorySlot Hat;

		// Token: 0x04008DDE RID: 36318
		public AccessorySlot HatHair;

		// Token: 0x04008DDF RID: 36319
		public AccessorySlot HeadEffects;

		// Token: 0x04008DE0 RID: 36320
		public AccessorySlot Belt;

		// Token: 0x04008DE1 RID: 36321
		public AccessorySlot Neck;

		// Token: 0x04008DE2 RID: 36322
		public AccessorySlot Pelvis;

		// Token: 0x04008DE3 RID: 36323
		public AccessorySlot Leg;

		// Token: 0x04008DE4 RID: 36324
		public AccessorySlot Foot;

		// Token: 0x04008DE5 RID: 36325
		public AccessorySlot Skirt;

		// Token: 0x04008DE6 RID: 36326
		public AccessorySlot Necklace;

		// Token: 0x04008DE7 RID: 36327
		public AccessorySlot Cuff;

		// Token: 0x04008DE8 RID: 36328
		public AccessorySlot Hand;

		// Token: 0x04008DE9 RID: 36329
		public AccessorySlot ArmLowerSkin;

		// Token: 0x04008DEA RID: 36330
		public AccessorySlot ArmUpperSkin;

		// Token: 0x04008DEB RID: 36331
		public AccessorySlot LegSkin;
	}
}
