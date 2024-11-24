using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

public class UIDupeRandomizer : MonoBehaviour
{
	[Serializable]
	public struct AnimChoice
	{
		public string anim_name;

		public List<KBatchedAnimController> minions;

		public float minSecondsBetweenAction;

		public float maxSecondsBetweenAction;

		public float lastWaitTime;

		public KAnimFile curBody;
	}

	[Tooltip("Enable this to allow for a chance for skill hats to appear")]
	public bool applyHat = true;

	[Tooltip("Enable this to allow for a chance for suit helmets to appear (ie. atmosuit and leadsuit)")]
	public bool applySuit = true;

	public AnimChoice[] anims;

	private AccessorySlots slots;

	protected virtual void Start()
	{
		slots = Db.Get().AccessorySlots;
		for (int i = 0; i < anims.Length; i++)
		{
			anims[i].curBody = null;
			GetNewBody(i);
		}
	}

	protected void GetNewBody(int minion_idx)
	{
		Personality random = Db.Get().Personalities.GetRandom(onlyEnabledMinions: true, onlyStartingMinions: false);
		foreach (KBatchedAnimController minion in anims[minion_idx].minions)
		{
			Apply(minion, random);
		}
	}

	private void Apply(KBatchedAnimController dupe, Personality personality)
	{
		KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(personality);
		SymbolOverrideController component = dupe.GetComponent<SymbolOverrideController>();
		component.RemoveAllSymbolOverrides();
		AddAccessory(dupe, slots.Hair.Lookup(bodyData.hair));
		AddAccessory(dupe, slots.HatHair.Lookup("hat_" + HashCache.Get().Get(bodyData.hair)));
		AddAccessory(dupe, slots.Eyes.Lookup(bodyData.eyes));
		AddAccessory(dupe, slots.HeadShape.Lookup(bodyData.headShape));
		AddAccessory(dupe, slots.Mouth.Lookup(bodyData.mouth));
		AddAccessory(dupe, slots.Body.Lookup(bodyData.body));
		AddAccessory(dupe, slots.Arm.Lookup(bodyData.arms));
		AddAccessory(dupe, slots.ArmLower.Lookup(bodyData.armslower));
		AddAccessory(dupe, slots.Belt.Lookup(bodyData.belt));
		if (applySuit && UnityEngine.Random.value < 0.15f)
		{
			component.AddBuildOverride(Assets.GetAnim("body_oxygen_kanim").GetData(), 6);
			dupe.SetSymbolVisiblity("snapto_neck", is_visible: true);
			dupe.SetSymbolVisiblity("belt", is_visible: false);
		}
		else
		{
			dupe.SetSymbolVisiblity("snapto_neck", is_visible: false);
		}
		if (applyHat && UnityEngine.Random.value < 0.5f)
		{
			List<string> list = new List<string>();
			foreach (Skill resource in Db.Get().Skills.resources)
			{
				list.Add(resource.hat);
			}
			string id = list[UnityEngine.Random.Range(0, list.Count)];
			AddAccessory(dupe, slots.Hat.Lookup(id));
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, is_visible: false);
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, is_visible: true);
		}
		else
		{
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, is_visible: true);
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, is_visible: false);
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, is_visible: false);
		}
		dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, is_visible: false);
		dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Necklace.targetSymbolId, is_visible: false);
	}

	public static KAnimHashedString AddAccessory(KBatchedAnimController minion, Accessory accessory)
	{
		if (accessory != null)
		{
			SymbolOverrideController component = minion.GetComponent<SymbolOverrideController>();
			DebugUtil.Assert(component != null, minion.name + " is missing symbol override controller");
			component.TryRemoveSymbolOverride(accessory.slot.targetSymbolId);
			component.AddSymbolOverride(accessory.slot.targetSymbolId, accessory.symbol);
			minion.SetSymbolVisiblity(accessory.slot.targetSymbolId, is_visible: true);
			return accessory.slot.targetSymbolId;
		}
		return HashedString.Invalid;
	}

	public KAnimHashedString AddRandomAccessory(KBatchedAnimController minion, List<Accessory> choices)
	{
		Accessory accessory = choices[UnityEngine.Random.Range(1, choices.Count)];
		return AddAccessory(minion, accessory);
	}

	public void Randomize()
	{
		if (slots != null)
		{
			for (int i = 0; i < anims.Length; i++)
			{
				GetNewBody(i);
			}
		}
	}

	protected virtual void Update()
	{
	}
}
