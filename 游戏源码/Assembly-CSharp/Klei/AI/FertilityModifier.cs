using System;

namespace Klei.AI
{
	// Token: 0x02003B7A RID: 15226
	public class FertilityModifier : Resource
	{
		// Token: 0x0600EA63 RID: 60003 RVA: 0x0013CAB9 File Offset: 0x0013ACB9
		public FertilityModifier(string id, Tag targetTag, string name, string description, Func<string, string> tooltipCB, FertilityModifier.FertilityModFn applyFunction) : base(id, name)
		{
			this.Description = description;
			this.TargetTag = targetTag;
			this.TooltipCB = tooltipCB;
			this.ApplyFunction = applyFunction;
		}

		// Token: 0x0600EA64 RID: 60004 RVA: 0x0013CAE2 File Offset: 0x0013ACE2
		public string GetTooltip()
		{
			if (this.TooltipCB != null)
			{
				return this.TooltipCB(this.Description);
			}
			return this.Description;
		}

		// Token: 0x0400E5BF RID: 58815
		public string Description;

		// Token: 0x0400E5C0 RID: 58816
		public Tag TargetTag;

		// Token: 0x0400E5C1 RID: 58817
		public Func<string, string> TooltipCB;

		// Token: 0x0400E5C2 RID: 58818
		public FertilityModifier.FertilityModFn ApplyFunction;

		// Token: 0x02003B7B RID: 15227
		// (Invoke) Token: 0x0600EA66 RID: 60006
		public delegate void FertilityModFn(FertilityMonitor.Instance inst, Tag eggTag);
	}
}
