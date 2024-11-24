using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003B9D RID: 15261
	public class StateGrowthRule : GrowthRule
	{
		// Token: 0x0600EB2B RID: 60203 RVA: 0x0013D398 File Offset: 0x0013B598
		public StateGrowthRule(Element.State state)
		{
			this.state = state;
		}

		// Token: 0x0600EB2C RID: 60204 RVA: 0x0013D3A7 File Offset: 0x0013B5A7
		public override bool Test(Element e)
		{
			return e.IsState(this.state);
		}

		// Token: 0x0600EB2D RID: 60205 RVA: 0x0013D3B5 File Offset: 0x0013B5B5
		public override string Name()
		{
			return Element.GetStateString(this.state);
		}

		// Token: 0x0400E65C RID: 58972
		public Element.State state;
	}
}
