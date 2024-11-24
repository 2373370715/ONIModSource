using Database;
using UnityEngine;

[RequireComponent(typeof(SymbolOverrideController))]
public class UIDupeSymbolOverride : MonoBehaviour
{
	private KBatchedAnimController animController;

	private AccessorySlots slots;

	private SymbolOverrideController symbolOverrideController;

	public void Apply(MinionIdentity minionIdentity)
	{
		if (slots == null)
		{
			slots = new AccessorySlots(null);
		}
		if (symbolOverrideController == null)
		{
			symbolOverrideController = GetComponent<SymbolOverrideController>();
		}
		if (animController == null)
		{
			animController = GetComponent<KBatchedAnimController>();
		}
		Personality personalityFromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(minionIdentity.nameStringKey);
		DebugUtil.DevAssert(personalityFromNameStringKey != null, "Personality is not found");
		KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(personalityFromNameStringKey);
		symbolOverrideController.RemoveAllSymbolOverrides();
		SetAccessory(animController, slots.Hair.Lookup(bodyData.hair));
		SetAccessory(animController, slots.HatHair.Lookup("hat_" + HashCache.Get().Get(bodyData.hair)));
		SetAccessory(animController, slots.Eyes.Lookup(bodyData.eyes));
		SetAccessory(animController, slots.HeadShape.Lookup(bodyData.headShape));
		SetAccessory(animController, slots.Mouth.Lookup(bodyData.mouth));
		SetAccessory(animController, slots.Neck.Lookup(bodyData.neck));
		SetAccessory(animController, slots.Body.Lookup(bodyData.body));
		SetAccessory(animController, slots.Leg.Lookup(bodyData.legs));
		SetAccessory(animController, slots.Arm.Lookup(bodyData.arms));
		SetAccessory(animController, slots.ArmLower.Lookup(bodyData.armslower));
		SetAccessory(animController, slots.Pelvis.Lookup(bodyData.pelvis));
		SetAccessory(animController, slots.Belt.Lookup(bodyData.belt));
		SetAccessory(animController, slots.Foot.Lookup(bodyData.foot));
		SetAccessory(animController, slots.Cuff.Lookup(bodyData.cuff));
		SetAccessory(animController, slots.Hand.Lookup(bodyData.hand));
	}

	private KAnimHashedString SetAccessory(KBatchedAnimController minion, Accessory accessory)
	{
		if (accessory != null)
		{
			symbolOverrideController.TryRemoveSymbolOverride(accessory.slot.targetSymbolId);
			symbolOverrideController.AddSymbolOverride(accessory.slot.targetSymbolId, accessory.symbol);
			minion.SetSymbolVisiblity(accessory.slot.targetSymbolId, is_visible: true);
			return accessory.slot.targetSymbolId;
		}
		return HashedString.Invalid;
	}
}
