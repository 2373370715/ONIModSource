using System;
using KSerialization;
using UnityEngine;

// Token: 0x020016CD RID: 5837
[AddComponentMenu("KMonoBehaviour/scripts/HeatBulb")]
public class HeatBulb : KMonoBehaviour, ISim200ms
{
	// Token: 0x0600784B RID: 30795 RVA: 0x000EF0D9 File Offset: 0x000ED2D9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.kanim.Play("off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0600784C RID: 30796 RVA: 0x00310A9C File Offset: 0x0030EC9C
	public void Sim200ms(float dt)
	{
		float num = this.kjConsumptionRate * dt;
		Vector2I vector2I = this.maxCheckOffset - this.minCheckOffset + 1;
		int num2 = vector2I.x * vector2I.y;
		float num3 = num / (float)num2;
		int num4;
		int num5;
		Grid.PosToXY(base.transform.GetPosition(), out num4, out num5);
		for (int i = this.minCheckOffset.y; i <= this.maxCheckOffset.y; i++)
		{
			for (int j = this.minCheckOffset.x; j <= this.maxCheckOffset.x; j++)
			{
				int num6 = Grid.XYToCell(num4 + j, num5 + i);
				if (Grid.IsValidCell(num6) && Grid.Temperature[num6] > this.minTemperature)
				{
					this.kjConsumed += num3;
					SimMessages.ModifyEnergy(num6, -num3, 5000f, SimMessages.EnergySourceID.HeatBulb);
				}
			}
		}
		float num7 = this.lightKJConsumptionRate * dt;
		if (this.kjConsumed > num7)
		{
			if (!this.lightSource.enabled)
			{
				this.kanim.Play("open", KAnim.PlayMode.Once, 1f, 0f);
				this.kanim.Queue("on", KAnim.PlayMode.Once, 1f, 0f);
				this.lightSource.enabled = true;
			}
			this.kjConsumed -= num7;
			return;
		}
		if (this.lightSource.enabled)
		{
			this.kanim.Play("close", KAnim.PlayMode.Once, 1f, 0f);
			this.kanim.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
		}
		this.lightSource.enabled = false;
	}

	// Token: 0x04005A0F RID: 23055
	[SerializeField]
	private float minTemperature;

	// Token: 0x04005A10 RID: 23056
	[SerializeField]
	private float kjConsumptionRate;

	// Token: 0x04005A11 RID: 23057
	[SerializeField]
	private float lightKJConsumptionRate;

	// Token: 0x04005A12 RID: 23058
	[SerializeField]
	private Vector2I minCheckOffset;

	// Token: 0x04005A13 RID: 23059
	[SerializeField]
	private Vector2I maxCheckOffset;

	// Token: 0x04005A14 RID: 23060
	[MyCmpGet]
	private Light2D lightSource;

	// Token: 0x04005A15 RID: 23061
	[MyCmpGet]
	private KBatchedAnimController kanim;

	// Token: 0x04005A16 RID: 23062
	[Serialize]
	private float kjConsumed;
}
