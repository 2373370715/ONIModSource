using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B00 RID: 15104
	public class Amounts : Modifications<Amount, AmountInstance>
	{
		// Token: 0x0600E84F RID: 59471 RVA: 0x0013B653 File Offset: 0x00139853
		public Amounts(GameObject go) : base(go, null)
		{
		}

		// Token: 0x0600E850 RID: 59472 RVA: 0x0013B65D File Offset: 0x0013985D
		public float GetValue(string amount_id)
		{
			return base.Get(amount_id).value;
		}

		// Token: 0x0600E851 RID: 59473 RVA: 0x0013B66B File Offset: 0x0013986B
		public void SetValue(string amount_id, float value)
		{
			base.Get(amount_id).value = value;
		}

		// Token: 0x0600E852 RID: 59474 RVA: 0x0013B67A File Offset: 0x0013987A
		public override AmountInstance Add(AmountInstance instance)
		{
			instance.Activate();
			return base.Add(instance);
		}

		// Token: 0x0600E853 RID: 59475 RVA: 0x0013B689 File Offset: 0x00139889
		public override void Remove(AmountInstance instance)
		{
			instance.Deactivate();
			base.Remove(instance);
		}

		// Token: 0x0600E854 RID: 59476 RVA: 0x004C0B3C File Offset: 0x004BED3C
		public void Cleanup()
		{
			for (int i = 0; i < base.Count; i++)
			{
				base[i].Deactivate();
			}
		}
	}
}
