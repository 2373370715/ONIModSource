using System;
using UnityEngine;

public class DevLifeSupport : KMonoBehaviour, ISim200ms
{
		protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.elementConsumer != null)
		{
			this.elementConsumer.EnableConsumption(true);
		}
	}

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

		[MyCmpReq]
	private ElementConsumer elementConsumer;

		public float targetTemperature = 303.15f;

		public int effectRadius = 7;

		private const float temperatureControlK = 0.2f;
}
