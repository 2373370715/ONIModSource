using System;

// Token: 0x02001667 RID: 5735
public class NonEssentialEnergyConsumer : EnergyConsumer
{
	// Token: 0x17000779 RID: 1913
	// (get) Token: 0x06007674 RID: 30324 RVA: 0x000EDDCC File Offset: 0x000EBFCC
	// (set) Token: 0x06007675 RID: 30325 RVA: 0x000EDDD4 File Offset: 0x000EBFD4
	public override bool IsPowered
	{
		get
		{
			return this.isPowered;
		}
		protected set
		{
			if (value == this.isPowered)
			{
				return;
			}
			this.isPowered = value;
			Action<bool> poweredStateChanged = this.PoweredStateChanged;
			if (poweredStateChanged == null)
			{
				return;
			}
			poweredStateChanged(this.isPowered);
		}
	}

	// Token: 0x040058A7 RID: 22695
	public Action<bool> PoweredStateChanged;

	// Token: 0x040058A8 RID: 22696
	private bool isPowered;
}
