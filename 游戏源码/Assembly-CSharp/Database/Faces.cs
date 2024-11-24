using System;

namespace Database
{
	// Token: 0x0200213C RID: 8508
	public class Faces : ResourceSet<Face>
	{
		// Token: 0x0600B553 RID: 46419 RVA: 0x0044EBFC File Offset: 0x0044CDFC
		public Faces()
		{
			this.Neutral = base.Add(new Face("Neutral", null));
			this.Happy = base.Add(new Face("Happy", null));
			this.Uncomfortable = base.Add(new Face("Uncomfortable", null));
			this.Cold = base.Add(new Face("Cold", null));
			this.Hot = base.Add(new Face("Hot", "headfx_sweat"));
			this.Tired = base.Add(new Face("Tired", null));
			this.Sleep = base.Add(new Face("Sleep", null));
			this.Hungry = base.Add(new Face("Hungry", null));
			this.Angry = base.Add(new Face("Angry", null));
			this.Suffocate = base.Add(new Face("Suffocate", null));
			this.Sick = base.Add(new Face("Sick", "headfx_sick"));
			this.SickSpores = base.Add(new Face("Spores", "headfx_spores"));
			this.Zombie = base.Add(new Face("Zombie", null));
			this.SickFierySkin = base.Add(new Face("Fiery", "headfx_fiery"));
			this.SickCold = base.Add(new Face("SickCold", "headfx_sickcold"));
			this.Pollen = base.Add(new Face("Pollen", "headfx_pollen"));
			this.Dead = base.Add(new Face("Death", null));
			this.Productive = base.Add(new Face("Productive", null));
			this.Determined = base.Add(new Face("Determined", null));
			this.Sticker = base.Add(new Face("Sticker", null));
			this.Sparkle = base.Add(new Face("Sparkle", null));
			this.Balloon = base.Add(new Face("Balloon", null));
			this.Tickled = base.Add(new Face("Tickled", null));
			this.Music = base.Add(new Face("Music", null));
			this.Radiation1 = base.Add(new Face("Radiation1", "headfx_radiation1"));
			this.Radiation2 = base.Add(new Face("Radiation2", "headfx_radiation2"));
			this.Radiation3 = base.Add(new Face("Radiation3", "headfx_radiation3"));
			this.Radiation4 = base.Add(new Face("Radiation4", "headfx_radiation4"));
		}

		// Token: 0x0400929B RID: 37531
		public Face Neutral;

		// Token: 0x0400929C RID: 37532
		public Face Happy;

		// Token: 0x0400929D RID: 37533
		public Face Uncomfortable;

		// Token: 0x0400929E RID: 37534
		public Face Cold;

		// Token: 0x0400929F RID: 37535
		public Face Hot;

		// Token: 0x040092A0 RID: 37536
		public Face Tired;

		// Token: 0x040092A1 RID: 37537
		public Face Sleep;

		// Token: 0x040092A2 RID: 37538
		public Face Hungry;

		// Token: 0x040092A3 RID: 37539
		public Face Angry;

		// Token: 0x040092A4 RID: 37540
		public Face Suffocate;

		// Token: 0x040092A5 RID: 37541
		public Face Dead;

		// Token: 0x040092A6 RID: 37542
		public Face Sick;

		// Token: 0x040092A7 RID: 37543
		public Face SickSpores;

		// Token: 0x040092A8 RID: 37544
		public Face Zombie;

		// Token: 0x040092A9 RID: 37545
		public Face SickFierySkin;

		// Token: 0x040092AA RID: 37546
		public Face SickCold;

		// Token: 0x040092AB RID: 37547
		public Face Pollen;

		// Token: 0x040092AC RID: 37548
		public Face Productive;

		// Token: 0x040092AD RID: 37549
		public Face Determined;

		// Token: 0x040092AE RID: 37550
		public Face Sticker;

		// Token: 0x040092AF RID: 37551
		public Face Balloon;

		// Token: 0x040092B0 RID: 37552
		public Face Sparkle;

		// Token: 0x040092B1 RID: 37553
		public Face Tickled;

		// Token: 0x040092B2 RID: 37554
		public Face Music;

		// Token: 0x040092B3 RID: 37555
		public Face Radiation1;

		// Token: 0x040092B4 RID: 37556
		public Face Radiation2;

		// Token: 0x040092B5 RID: 37557
		public Face Radiation3;

		// Token: 0x040092B6 RID: 37558
		public Face Radiation4;
	}
}
