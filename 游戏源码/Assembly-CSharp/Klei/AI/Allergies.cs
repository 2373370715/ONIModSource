using System;
using System.Collections.Generic;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02003B0C RID: 15116
	public class Allergies : Sickness
	{
		// Token: 0x0600E8B1 RID: 59569 RVA: 0x004C1BD0 File Offset: 0x004BFDD0
		public Allergies() : base("Allergies", Sickness.SicknessType.Pathogen, Sickness.Severity.Minor, 0.00025f, new List<Sickness.InfectionVector>
		{
			Sickness.InfectionVector.Inhalation
		}, 60f, null)
		{
			float value = 0.025f;
			base.AddSicknessComponent(new CommonSickEffectSickness());
			base.AddSicknessComponent(new AnimatedSickness(new HashedString[]
			{
				"anim_idle_allergies_kanim"
			}, Db.Get().Expressions.Pollen));
			base.AddSicknessComponent(new AttributeModifierSickness(new AttributeModifier[]
			{
				new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, value, DUPLICANTS.DISEASES.ALLERGIES.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Sneezyness.Id, 10f, DUPLICANTS.DISEASES.ALLERGIES.NAME, false, false, true)
			}));
		}

		// Token: 0x0400E453 RID: 58451
		public const string ID = "Allergies";

		// Token: 0x0400E454 RID: 58452
		public const float STRESS_PER_CYCLE = 15f;
	}
}
