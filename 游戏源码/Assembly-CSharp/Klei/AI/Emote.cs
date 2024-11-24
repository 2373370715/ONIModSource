using System;
using System.Collections.Generic;

namespace Klei.AI
{
	// Token: 0x02003B83 RID: 15235
	public class Emote : Resource
	{
		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x0600EAA4 RID: 60068 RVA: 0x0013CDF5 File Offset: 0x0013AFF5
		public int StepCount
		{
			get
			{
				if (this.emoteSteps != null)
				{
					return this.emoteSteps.Count;
				}
				return 0;
			}
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x0600EAA5 RID: 60069 RVA: 0x0013CE0C File Offset: 0x0013B00C
		public KAnimFile AnimSet
		{
			get
			{
				if (this.animSetName != HashedString.Invalid && this.animSet == null)
				{
					this.animSet = Assets.GetAnim(this.animSetName);
				}
				return this.animSet;
			}
		}

		// Token: 0x0600EAA6 RID: 60070 RVA: 0x0013CE45 File Offset: 0x0013B045
		public Emote(ResourceSet parent, string emoteId, EmoteStep[] defaultSteps, string animSetName = null) : base(emoteId, parent, null)
		{
			this.emoteSteps.AddRange(defaultSteps);
			this.animSetName = animSetName;
		}

		// Token: 0x0600EAA7 RID: 60071 RVA: 0x004CA818 File Offset: 0x004C8A18
		public bool IsValidForController(KBatchedAnimController animController)
		{
			bool flag = true;
			int num = 0;
			while (flag && num < this.StepCount)
			{
				flag = animController.HasAnimation(this.emoteSteps[num].anim);
				num++;
			}
			KAnimFileData kanimFileData = (this.animSet == null) ? null : this.animSet.GetData();
			int num2 = 0;
			while (kanimFileData != null && flag && num2 < this.StepCount)
			{
				bool flag2 = false;
				int num3 = 0;
				while (!flag2 && num3 < kanimFileData.animCount)
				{
					flag2 = (kanimFileData.GetAnim(num2).id == this.emoteSteps[num2].anim);
					num3++;
				}
				flag = flag2;
				num2++;
			}
			return flag;
		}

		// Token: 0x0600EAA8 RID: 60072 RVA: 0x004CA8D0 File Offset: 0x004C8AD0
		public void ApplyAnimOverrides(KBatchedAnimController animController, KAnimFile overrideSet)
		{
			KAnimFile kanimFile = (overrideSet != null) ? overrideSet : this.AnimSet;
			if (kanimFile == null || animController == null)
			{
				return;
			}
			animController.AddAnimOverrides(kanimFile, 0f);
		}

		// Token: 0x0600EAA9 RID: 60073 RVA: 0x004CA910 File Offset: 0x004C8B10
		public void RemoveAnimOverrides(KBatchedAnimController animController, KAnimFile overrideSet)
		{
			KAnimFile kanimFile = (overrideSet != null) ? overrideSet : this.AnimSet;
			if (kanimFile == null || animController == null)
			{
				return;
			}
			animController.RemoveAnimOverrides(kanimFile);
		}

		// Token: 0x0600EAAA RID: 60074 RVA: 0x004CA94C File Offset: 0x004C8B4C
		public void CollectStepAnims(out HashedString[] emoteAnims, int iterations)
		{
			emoteAnims = new HashedString[this.emoteSteps.Count * iterations];
			for (int i = 0; i < emoteAnims.Length; i++)
			{
				emoteAnims[i] = this.emoteSteps[i % this.emoteSteps.Count].anim;
			}
		}

		// Token: 0x0600EAAB RID: 60075 RVA: 0x0013CE80 File Offset: 0x0013B080
		public bool IsValidStep(int stepIdx)
		{
			return stepIdx >= 0 && stepIdx < this.emoteSteps.Count;
		}

		// Token: 0x17000C33 RID: 3123
		public EmoteStep this[int stepIdx]
		{
			get
			{
				if (!this.IsValidStep(stepIdx))
				{
					return null;
				}
				return this.emoteSteps[stepIdx];
			}
		}

		// Token: 0x0600EAAD RID: 60077 RVA: 0x004CA9A4 File Offset: 0x004C8BA4
		public int GetStepIndex(HashedString animName)
		{
			int i = 0;
			bool condition = false;
			while (i < this.emoteSteps.Count)
			{
				if (this.emoteSteps[i].anim == animName)
				{
					condition = true;
					break;
				}
				i++;
			}
			Debug.Assert(condition, string.Format("Could not find emote step {0} for emote {1}!", animName, this.Id));
			return i;
		}

		// Token: 0x0400E5D2 RID: 58834
		private HashedString animSetName = null;

		// Token: 0x0400E5D3 RID: 58835
		private KAnimFile animSet;

		// Token: 0x0400E5D4 RID: 58836
		private List<EmoteStep> emoteSteps = new List<EmoteStep>();
	}
}
