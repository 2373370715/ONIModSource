using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class AirBorneCritterCondoConfig : IBuildingConfig
{
	public const string ID = "AirBorneCritterCondo";

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("AirBorneCritterCondo", 3, 3, "critter_condo_airborne_kanim", 100, 120f, new float[1] { 200f }, MATERIALS.PLASTICS, 1600f, BuildLocationRule.OnCeiling, noise: NOISE_POLLUTION.NONE, decor: TUNING.BUILDINGS.DECOR.BONUS.TIER3);
		obj.AudioCategory = "Metal";
		obj.PermittedRotations = PermittedRotations.FlipH;
		return obj;
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		Effect effect = new Effect("InteractedWithAirborneCondo", STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME, STRINGS.CREATURES.MODIFIERS.AIRBORNECRITTERCONDOINTERACTEFFECT.TOOLTIP, 600f, show_in_ui: true, trigger_floating_text: true, is_bad: false);
		effect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 1f, STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME));
		Db.Get().effects.Add(effect);
		CritterCondo.Def def = go.AddOrGetDef<CritterCondo.Def>();
		def.IsCritterCondoOperationalCb = (CritterCondo.Instance condo_smi) => condo_smi.GetComponent<RoomTracker>().IsInCorrectRoom();
		def.moveToStatusItem = new StatusItem("AIRBORNECRITTERCONDO.MOVINGTO", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		def.interactStatusItem = new StatusItem("AIRBORNECRITTERCONDO.INTERACTING", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		def.condoTag = "AirBorneCritterCondo";
		def.effectId = effect.Id;
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType);
	}

	public override void ConfigurePost(BuildingDef def)
	{
	}
}
