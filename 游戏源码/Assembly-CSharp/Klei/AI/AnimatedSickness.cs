using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B2A RID: 15146
	public class AnimatedSickness : Sickness.SicknessComponent
	{
		// Token: 0x0600E920 RID: 59680 RVA: 0x004C5450 File Offset: 0x004C3650
		public AnimatedSickness(HashedString[] kanim_filenames, Expression expression)
		{
			this.kanims = new KAnimFile[kanim_filenames.Length];
			for (int i = 0; i < kanim_filenames.Length; i++)
			{
				this.kanims[i] = Assets.GetAnim(kanim_filenames[i]);
			}
			this.expression = expression;
		}

		// Token: 0x0600E921 RID: 59681 RVA: 0x004C549C File Offset: 0x004C369C
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			for (int i = 0; i < this.kanims.Length; i++)
			{
				go.GetComponent<KAnimControllerBase>().AddAnimOverrides(this.kanims[i], 10f);
			}
			if (this.expression != null)
			{
				go.GetComponent<FaceGraph>().AddExpression(this.expression);
			}
			return null;
		}

		// Token: 0x0600E922 RID: 59682 RVA: 0x004C54F0 File Offset: 0x004C36F0
		public override void OnCure(GameObject go, object instace_data)
		{
			if (this.expression != null)
			{
				go.GetComponent<FaceGraph>().RemoveExpression(this.expression);
			}
			for (int i = 0; i < this.kanims.Length; i++)
			{
				go.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(this.kanims[i]);
			}
		}

		// Token: 0x0400E4BC RID: 58556
		private KAnimFile[] kanims;

		// Token: 0x0400E4BD RID: 58557
		private Expression expression;
	}
}
