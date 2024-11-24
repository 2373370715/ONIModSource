using System;

namespace Database
{
	// Token: 0x0200217C RID: 8572
	public class Urges : ResourceSet<Urge>
	{
		// Token: 0x0600B63A RID: 46650 RVA: 0x004598E0 File Offset: 0x00457AE0
		public Urges()
		{
			this.HealCritical = base.Add(new Urge("HealCritical"));
			this.BeOffline = base.Add(new Urge("BeOffline"));
			this.BeIncapacitated = base.Add(new Urge("BeIncapacitated"));
			this.PacifyEat = base.Add(new Urge("PacifyEat"));
			this.PacifySleep = base.Add(new Urge("PacifySleep"));
			this.PacifyIdle = base.Add(new Urge("PacifyIdle"));
			this.EmoteHighPriority = base.Add(new Urge("EmoteHighPriority"));
			this.RecoverBreath = base.Add(new Urge("RecoverBreath"));
			this.RecoverWarmth = base.Add(new Urge("RecoverWarmth"));
			this.Aggression = base.Add(new Urge("Aggression"));
			this.MoveToQuarantine = base.Add(new Urge("MoveToQuarantine"));
			this.WashHands = base.Add(new Urge("WashHands"));
			this.Shower = base.Add(new Urge("Shower"));
			this.Eat = base.Add(new Urge("Eat"));
			this.ReloadElectrobank = base.Add(new Urge("ReloadElectrobank"));
			this.RemoveDischargedElectrobank = base.Add(new Urge("RemoveDischargedElectrobank"));
			this.Pee = base.Add(new Urge("Pee"));
			this.RestDueToDisease = base.Add(new Urge("RestDueToDisease"));
			this.Sleep = base.Add(new Urge("Sleep"));
			this.Narcolepsy = base.Add(new Urge("Narcolepsy"));
			this.Doctor = base.Add(new Urge("Doctor"));
			this.Heal = base.Add(new Urge("Heal"));
			this.Feed = base.Add(new Urge("Feed"));
			this.PacifyRelocate = base.Add(new Urge("PacifyRelocate"));
			this.Emote = base.Add(new Urge("Emote"));
			this.MoveToSafety = base.Add(new Urge("MoveToSafety"));
			this.WarmUp = base.Add(new Urge("WarmUp"));
			this.CoolDown = base.Add(new Urge("CoolDown"));
			this.LearnSkill = base.Add(new Urge("LearnSkill"));
			this.EmoteIdle = base.Add(new Urge("EmoteIdle"));
			this.OilRefill = base.Add(new Urge("OilRefill"));
			this.GunkPee = base.Add(new Urge("GunkPee"));
			this.FindOxygenRefill = base.Add(new Urge("FindOxygenRefill"));
		}

		// Token: 0x040094C7 RID: 38087
		public Urge BeIncapacitated;

		// Token: 0x040094C8 RID: 38088
		public Urge BeOffline;

		// Token: 0x040094C9 RID: 38089
		public Urge Sleep;

		// Token: 0x040094CA RID: 38090
		public Urge Narcolepsy;

		// Token: 0x040094CB RID: 38091
		public Urge Eat;

		// Token: 0x040094CC RID: 38092
		public Urge RemoveDischargedElectrobank;

		// Token: 0x040094CD RID: 38093
		public Urge ReloadElectrobank;

		// Token: 0x040094CE RID: 38094
		public Urge WashHands;

		// Token: 0x040094CF RID: 38095
		public Urge Shower;

		// Token: 0x040094D0 RID: 38096
		public Urge Pee;

		// Token: 0x040094D1 RID: 38097
		public Urge MoveToQuarantine;

		// Token: 0x040094D2 RID: 38098
		public Urge HealCritical;

		// Token: 0x040094D3 RID: 38099
		public Urge RecoverBreath;

		// Token: 0x040094D4 RID: 38100
		public Urge FindOxygenRefill;

		// Token: 0x040094D5 RID: 38101
		public Urge RecoverWarmth;

		// Token: 0x040094D6 RID: 38102
		public Urge Emote;

		// Token: 0x040094D7 RID: 38103
		public Urge Feed;

		// Token: 0x040094D8 RID: 38104
		public Urge Doctor;

		// Token: 0x040094D9 RID: 38105
		public Urge Flee;

		// Token: 0x040094DA RID: 38106
		public Urge Heal;

		// Token: 0x040094DB RID: 38107
		public Urge PacifyIdle;

		// Token: 0x040094DC RID: 38108
		public Urge PacifyEat;

		// Token: 0x040094DD RID: 38109
		public Urge PacifySleep;

		// Token: 0x040094DE RID: 38110
		public Urge PacifyRelocate;

		// Token: 0x040094DF RID: 38111
		public Urge RestDueToDisease;

		// Token: 0x040094E0 RID: 38112
		public Urge EmoteHighPriority;

		// Token: 0x040094E1 RID: 38113
		public Urge Aggression;

		// Token: 0x040094E2 RID: 38114
		public Urge MoveToSafety;

		// Token: 0x040094E3 RID: 38115
		public Urge WarmUp;

		// Token: 0x040094E4 RID: 38116
		public Urge CoolDown;

		// Token: 0x040094E5 RID: 38117
		public Urge LearnSkill;

		// Token: 0x040094E6 RID: 38118
		public Urge EmoteIdle;

		// Token: 0x040094E7 RID: 38119
		public Urge OilRefill;

		// Token: 0x040094E8 RID: 38120
		public Urge GunkPee;
	}
}
