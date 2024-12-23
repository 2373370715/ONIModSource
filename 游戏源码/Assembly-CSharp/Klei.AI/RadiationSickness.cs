using System.Collections.Generic;
using STRINGS;

namespace Klei.AI;

public class RadiationSickness : Sickness
{
	public const string ID = "RadiationSickness";

	public const string RECOVERY_ID = "RadiationSicknessRecovery";

	public const int ATTRIBUTE_PENALTY = -10;

	public RadiationSickness()
		: base("RadiationSickness", SicknessType.Pathogen, Severity.Major, 0.00025f, new List<InfectionVector>
		{
			InfectionVector.Inhalation,
			InfectionVector.Contact
		}, 10800f, "RadiationSicknessRecovery")
	{
		AddSicknessComponent(new CustomSickEffectSickness("spore_fx_kanim", "working_loop"));
		AddSicknessComponent(new AnimatedSickness(new HashedString[2] { "anim_idle_spores_kanim", "anim_loco_spore_kanim" }, Db.Get().Expressions.Zombie));
		AddSicknessComponent(new AttributeModifierSickness(new AttributeModifier[11]
		{
			new AttributeModifier(Db.Get().Attributes.Athletics.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME),
			new AttributeModifier(Db.Get().Attributes.Strength.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME),
			new AttributeModifier(Db.Get().Attributes.Digging.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME),
			new AttributeModifier(Db.Get().Attributes.Construction.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME),
			new AttributeModifier(Db.Get().Attributes.Art.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME),
			new AttributeModifier(Db.Get().Attributes.Caring.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME),
			new AttributeModifier(Db.Get().Attributes.Learning.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME),
			new AttributeModifier(Db.Get().Attributes.Machinery.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME),
			new AttributeModifier(Db.Get().Attributes.Cooking.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME),
			new AttributeModifier(Db.Get().Attributes.Botanist.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME),
			new AttributeModifier(Db.Get().Attributes.Ranching.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME)
		}));
	}
}
