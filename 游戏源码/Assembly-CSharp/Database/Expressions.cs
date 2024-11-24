using System;

namespace Database
{
	// Token: 0x0200213B RID: 8507
	public class Expressions : ResourceSet<Expression>
	{
		// Token: 0x0600B552 RID: 46418 RVA: 0x0044E8E0 File Offset: 0x0044CAE0
		public Expressions(ResourceSet parent) : base("Expressions", parent)
		{
			Faces faces = Db.Get().Faces;
			this.Angry = new Expression("Angry", this, faces.Angry);
			this.Suffocate = new Expression("Suffocate", this, faces.Suffocate);
			this.RecoverBreath = new Expression("RecoverBreath", this, faces.Uncomfortable);
			this.RedAlert = new Expression("RedAlert", this, faces.Hot);
			this.Hungry = new Expression("Hungry", this, faces.Hungry);
			this.Radiation1 = new Expression("Radiation1", this, faces.Radiation1);
			this.Radiation2 = new Expression("Radiation2", this, faces.Radiation2);
			this.Radiation3 = new Expression("Radiation3", this, faces.Radiation3);
			this.Radiation4 = new Expression("Radiation4", this, faces.Radiation4);
			this.SickSpores = new Expression("SickSpores", this, faces.SickSpores);
			this.Zombie = new Expression("Zombie", this, faces.Zombie);
			this.SickFierySkin = new Expression("SickFierySkin", this, faces.SickFierySkin);
			this.SickCold = new Expression("SickCold", this, faces.SickCold);
			this.Pollen = new Expression("Pollen", this, faces.Pollen);
			this.Sick = new Expression("Sick", this, faces.Sick);
			this.Cold = new Expression("Cold", this, faces.Cold);
			this.Hot = new Expression("Hot", this, faces.Hot);
			this.FullBladder = new Expression("FullBladder", this, faces.Uncomfortable);
			this.Tired = new Expression("Tired", this, faces.Tired);
			this.Unhappy = new Expression("Unhappy", this, faces.Uncomfortable);
			this.Uncomfortable = new Expression("Uncomfortable", this, faces.Uncomfortable);
			this.Productive = new Expression("Productive", this, faces.Productive);
			this.Determined = new Expression("Determined", this, faces.Determined);
			this.Sticker = new Expression("Sticker", this, faces.Sticker);
			this.Balloon = new Expression("Sticker", this, faces.Balloon);
			this.Sparkle = new Expression("Sticker", this, faces.Sparkle);
			this.Music = new Expression("Music", this, faces.Music);
			this.Tickled = new Expression("Tickled", this, faces.Tickled);
			this.Happy = new Expression("Happy", this, faces.Happy);
			this.Relief = new Expression("Relief", this, faces.Happy);
			this.Neutral = new Expression("Neutral", this, faces.Neutral);
			for (int i = this.Count - 1; i >= 0; i--)
			{
				this.resources[i].priority = 100 * (this.Count - i);
			}
		}

		// Token: 0x0400927C RID: 37500
		public Expression Neutral;

		// Token: 0x0400927D RID: 37501
		public Expression Happy;

		// Token: 0x0400927E RID: 37502
		public Expression Uncomfortable;

		// Token: 0x0400927F RID: 37503
		public Expression Cold;

		// Token: 0x04009280 RID: 37504
		public Expression Hot;

		// Token: 0x04009281 RID: 37505
		public Expression FullBladder;

		// Token: 0x04009282 RID: 37506
		public Expression Tired;

		// Token: 0x04009283 RID: 37507
		public Expression Hungry;

		// Token: 0x04009284 RID: 37508
		public Expression Angry;

		// Token: 0x04009285 RID: 37509
		public Expression Unhappy;

		// Token: 0x04009286 RID: 37510
		public Expression RedAlert;

		// Token: 0x04009287 RID: 37511
		public Expression Suffocate;

		// Token: 0x04009288 RID: 37512
		public Expression RecoverBreath;

		// Token: 0x04009289 RID: 37513
		public Expression Sick;

		// Token: 0x0400928A RID: 37514
		public Expression SickSpores;

		// Token: 0x0400928B RID: 37515
		public Expression Zombie;

		// Token: 0x0400928C RID: 37516
		public Expression SickFierySkin;

		// Token: 0x0400928D RID: 37517
		public Expression SickCold;

		// Token: 0x0400928E RID: 37518
		public Expression Pollen;

		// Token: 0x0400928F RID: 37519
		public Expression Relief;

		// Token: 0x04009290 RID: 37520
		public Expression Productive;

		// Token: 0x04009291 RID: 37521
		public Expression Determined;

		// Token: 0x04009292 RID: 37522
		public Expression Sticker;

		// Token: 0x04009293 RID: 37523
		public Expression Balloon;

		// Token: 0x04009294 RID: 37524
		public Expression Sparkle;

		// Token: 0x04009295 RID: 37525
		public Expression Music;

		// Token: 0x04009296 RID: 37526
		public Expression Tickled;

		// Token: 0x04009297 RID: 37527
		public Expression Radiation1;

		// Token: 0x04009298 RID: 37528
		public Expression Radiation2;

		// Token: 0x04009299 RID: 37529
		public Expression Radiation3;

		// Token: 0x0400929A RID: 37530
		public Expression Radiation4;
	}
}
