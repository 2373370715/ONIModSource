using System;
using Klei.Actions;

namespace Klei.Input
{
	// Token: 0x02003BAA RID: 15274
	[Action("Clear Cell")]
	public class ClearCellDigAction : DigAction
	{
		// Token: 0x0600EB57 RID: 60247 RVA: 0x0013D55D File Offset: 0x0013B75D
		public override void Dig(int cell, int distFromOrigin)
		{
			if (Grid.Solid[cell] && !Grid.Foundation[cell])
			{
				SimMessages.Dig(cell, -1, true);
			}
		}

		// Token: 0x0600EB58 RID: 60248 RVA: 0x0013D551 File Offset: 0x0013B751
		protected override void EntityDig(IDigActionEntity digActionEntity)
		{
			if (digActionEntity == null)
			{
				return;
			}
			digActionEntity.Dig();
		}
	}
}
