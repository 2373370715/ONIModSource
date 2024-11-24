using System;
using Klei.Actions;

namespace Klei.Input
{
	// Token: 0x02003BA9 RID: 15273
	[Action("Immediate")]
	public class ImmediateDigAction : DigAction
	{
		// Token: 0x0600EB54 RID: 60244 RVA: 0x0013D52D File Offset: 0x0013B72D
		public override void Dig(int cell, int distFromOrigin)
		{
			if (Grid.Solid[cell] && !Grid.Foundation[cell])
			{
				SimMessages.Dig(cell, -1, false);
			}
		}

		// Token: 0x0600EB55 RID: 60245 RVA: 0x0013D551 File Offset: 0x0013B751
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
