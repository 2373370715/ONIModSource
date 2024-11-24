using System;

namespace Database
{
	// Token: 0x0200216A RID: 8554
	public class StateMachineCategories : ResourceSet<StateMachine.Category>
	{
		// Token: 0x0600B603 RID: 46595 RVA: 0x00456628 File Offset: 0x00454828
		public StateMachineCategories()
		{
			this.Ai = base.Add(new StateMachine.Category("Ai"));
			this.Monitor = base.Add(new StateMachine.Category("Monitor"));
			this.Chore = base.Add(new StateMachine.Category("Chore"));
			this.Misc = base.Add(new StateMachine.Category("Misc"));
		}

		// Token: 0x0400945C RID: 37980
		public StateMachine.Category Ai;

		// Token: 0x0400945D RID: 37981
		public StateMachine.Category Monitor;

		// Token: 0x0400945E RID: 37982
		public StateMachine.Category Chore;

		// Token: 0x0400945F RID: 37983
		public StateMachine.Category Misc;
	}
}
