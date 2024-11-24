namespace Database;

public class AccessorySlots : ResourceSet<AccessorySlot>
{
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

	public AccessorySlots(ResourceSet parent)
		: base("AccessorySlots", parent)
	{
		parent = Db.Get().Accessories;
		KAnimFile anim = Assets.GetAnim("head_swap_kanim");
		KAnimFile anim2 = Assets.GetAnim("body_comp_default_kanim");
		KAnimFile anim3 = Assets.GetAnim("body_swap_kanim");
		KAnimFile anim4 = Assets.GetAnim("hair_swap_kanim");
		KAnimFile anim5 = Assets.GetAnim("hat_swap_kanim");
		Eyes = new AccessorySlot("Eyes", this, anim);
		Hair = new AccessorySlot("Hair", this, anim4);
		HeadShape = new AccessorySlot("HeadShape", this, anim);
		Mouth = new AccessorySlot("Mouth", this, anim);
		Hat = new AccessorySlot("Hat", this, anim5, 4);
		HatHair = new AccessorySlot("Hat_Hair", this, anim4);
		HeadEffects = new AccessorySlot("HeadFX", this, anim);
		Body = new AccessorySlot("Torso", this, new KAnimHashedString("torso"), anim3);
		Arm = new AccessorySlot("Arm_Sleeve", this, new KAnimHashedString("arm_sleeve"), anim3);
		ArmLower = new AccessorySlot("Arm_Lower_Sleeve", this, new KAnimHashedString("arm_lower_sleeve"), anim3);
		Belt = new AccessorySlot("Belt", this, new KAnimHashedString("belt"), anim2);
		Neck = new AccessorySlot("Neck", this, new KAnimHashedString("neck"), anim2);
		Pelvis = new AccessorySlot("Pelvis", this, new KAnimHashedString("pelvis"), anim2);
		Foot = new AccessorySlot("Foot", this, new KAnimHashedString("foot"), anim2, Assets.GetAnim("shoes_basic_black_kanim"));
		Leg = new AccessorySlot("Leg", this, new KAnimHashedString("leg"), anim2);
		Necklace = new AccessorySlot("Necklace", this, new KAnimHashedString("necklace"), anim2);
		Cuff = new AccessorySlot("Cuff", this, new KAnimHashedString("cuff"), anim2);
		Hand = new AccessorySlot("Hand", this, new KAnimHashedString("hand_paint"), anim2);
		Skirt = new AccessorySlot("Skirt", this, new KAnimHashedString("skirt"), anim3);
		ArmLowerSkin = new AccessorySlot("Arm_Lower", this, new KAnimHashedString("arm_lower"), anim3);
		ArmUpperSkin = new AccessorySlot("Arm_Upper", this, new KAnimHashedString("arm_upper"), anim3);
		LegSkin = new AccessorySlot("Leg_Skin", this, new KAnimHashedString("leg_skin"), anim3);
		foreach (AccessorySlot resource in resources)
		{
			resource.AddAccessories(resource.AnimFile, parent);
		}
		Db.Get().Accessories.AddCustomAccessories(Assets.GetAnim("body_lonelyminion_kanim"), parent, this);
	}

	public AccessorySlot Find(KAnimHashedString symbol_name)
	{
		foreach (AccessorySlot resource in Db.Get().AccessorySlots.resources)
		{
			if (symbol_name == resource.targetSymbolId)
			{
				return resource;
			}
		}
		return null;
	}
}
