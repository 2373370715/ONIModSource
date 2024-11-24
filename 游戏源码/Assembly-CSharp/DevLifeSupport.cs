using System;
using UnityEngine;

// Token: 0x02000D31 RID: 3377
public class DevLifeSupport : KMonoBehaviour, ISim200ms
{
	// Token: 0x060041FD RID: 16893 RVA: 0x000CAA37 File Offset: 0x000C8C37
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.elementConsumer != null)
		{
			this.elementConsumer.EnableConsumption(true);
		}
	}

	// Token: 0x060041FE RID: 16894 RVA: 0x0023FD44 File Offset: 0x0023DF44
	public void Sim200ms(float dt)
	{
		Vector2I vector2I = new Vector2I(-this.effectRadius, -this.effectRadius);
		Vector2I vector2I2 = new Vector2I(this.effectRadius, this.effectRadius);
		int num;
		int num2;
		Grid.PosToXY(base.transform.GetPosition(), out num, out num2);
		int num3 = Grid.XYToCell(num, num2);
		if (Grid.IsValidCell(num3))
		{
			int world = (int)Grid.WorldIdx[num3];
			for (int i = vector2I.y; i <= vector2I2.y; i++)
			{
				for (int j = vector2I.x; j <= vector2I2.x; j++)
				{
					int num4 = Grid.XYToCell(num + j, num2 + i);
					if (Grid.IsValidCellInWorld(num4, world))
					{
						float num5 = (this.targetTemperature - Grid.Temperature[num4]) * Grid.Element[num4].specificHeatCapacity * Grid.Mass[num4];
						if (!Mathf.Approximately(0f, num5))
						{
							SimMessages.ModifyEnergy(num4, num5 * 0.2f, 5000f, (num5 > 0f) ? SimMessages.EnergySourceID.DebugHeat : SimMessages.EnergySourceID.DebugCool);
						}
					}
				}
			}
		}
	}

	// Token: 0x04002D03 RID: 11523
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x04002D04 RID: 11524
	public float targetTemperature = 303.15f;

	// Token: 0x04002D05 RID: 11525
	public int effectRadius = 7;

	// Token: 0x04002D06 RID: 11526
	private const float temperatureControlK = 0.2f;
}
