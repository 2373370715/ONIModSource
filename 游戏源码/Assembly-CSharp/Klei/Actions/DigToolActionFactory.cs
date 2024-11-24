using System;
using Klei.Input;

namespace Klei.Actions
{
	// Token: 0x02003BAE RID: 15278
	public class DigToolActionFactory : ActionFactory<DigToolActionFactory, DigAction, DigToolActionFactory.Actions>
	{
		// Token: 0x0600EB65 RID: 60261 RVA: 0x0013D61A File Offset: 0x0013B81A
		protected override DigAction CreateAction(DigToolActionFactory.Actions action)
		{
			if (action == DigToolActionFactory.Actions.Immediate)
			{
				return new ImmediateDigAction();
			}
			if (action == DigToolActionFactory.Actions.ClearCell)
			{
				return new ClearCellDigAction();
			}
			if (action == DigToolActionFactory.Actions.MarkCell)
			{
				return new MarkCellDigAction();
			}
			throw new InvalidOperationException("Can not create DigAction 'Count'. Please provide a valid action.");
		}

		// Token: 0x02003BAF RID: 15279
		public enum Actions
		{
			// Token: 0x0400E678 RID: 59000
			MarkCell = 145163119,
			// Token: 0x0400E679 RID: 59001
			Immediate = -1044758767,
			// Token: 0x0400E67A RID: 59002
			ClearCell = -1011242513,
			// Token: 0x0400E67B RID: 59003
			Count = -1427607121
		}
	}
}
