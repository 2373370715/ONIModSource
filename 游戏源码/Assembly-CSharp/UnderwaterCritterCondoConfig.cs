using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class UnderwaterCritterCondoConfig : IBuildingConfig
{
	public const string ID = "UnderwaterCritterCondo";

	public static readonly Operational.Flag Submerged = new Operational.Flag("Submerged", Operational.Flag.Type.Requirement);

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("UnderwaterCritterCondo", 3, 3, "underwater_critter_condo_kanim", 100, 120f, new float[1] { 200f }, MATERIALS.PLASTICS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NONE, decor: TUNING.BUILDINGS.DECOR.BONUS.TIER3);
		obj.AudioCategory = "Metal";
		obj.PermittedRotations = PermittedRotations.FlipH;
		obj.Floodable = false;
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
		go.AddOrGet<Submergable>();
		Effect effect = new Effect("InteractedWithUnderwaterCondo", STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME, STRINGS.CREATURES.MODIFIERS.UNDERWATERCRITTERCONDOINTERACTEFFECT.TOOLTIP, 600f, show_in_ui: true, trigger_floating_text: true, is_bad: false);
		effect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 1f, STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME));
		Db.Get().effects.Add(effect);
		CritterCondo.Def def = go.AddOrGetDef<CritterCondo.Def>();
		def.IsCritterCondoOperationalCb = delegate(CritterCondo.Instance condo_smi)
		{
			Building component = condo_smi.GetComponent<Building>();
			for (int i = 0; i < component.PlacementCells.Length; i++)
			{
				if (!Grid.IsLiquid(component.PlacementCells[i]))
				{
					return false;
				}
			}
			return true;
		};
		def.moveToStatusItem = new StatusItem("UNDERWATERCRITTERCONDO.MOVINGTO", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		def.interactStatusItem = new StatusItem("UNDERWATERCRITTERCONDO.INTERACTING", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		def.condoTag = "UnderwaterCritterCondo";
		def.effectId = effect.Id;
	}

	public override void ConfigurePost(BuildingDef def)
	{
	}
}
