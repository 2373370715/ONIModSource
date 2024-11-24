using System;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input
{
	// Token: 0x02003BA8 RID: 15272
	[Action("Mark Cell")]
	public class MarkCellDigAction : DigAction
	{
		// Token: 0x0600EB51 RID: 60241 RVA: 0x004CCCDC File Offset: 0x004CAEDC
		public override void Dig(int cell, int distFromOrigin)
		{
			GameObject gameObject = DigTool.PlaceDig(cell, distFromOrigin);
			if (gameObject != null)
			{
				Prioritizable component = gameObject.GetComponent<Prioritizable>();
				if (component != null)
				{
					component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
				}
			}
		}

		// Token: 0x0600EB52 RID: 60242 RVA: 0x0013D518 File Offset: 0x0013B718
		protected override void EntityDig(IDigActionEntity digActionEntity)
		{
			if (digActionEntity == null)
			{
				return;
			}
			digActionEntity.MarkForDig(true);
		}
	}
}
