using System;

// Token: 0x02001936 RID: 6454
[Serializable]
public class RocketModulePerformance
{
	// Token: 0x0600867B RID: 34427 RVA: 0x000F8104 File Offset: 0x000F6304
	public RocketModulePerformance(float burden, float fuelKilogramPerDistance, float enginePower)
	{
		this.burden = burden;
		this.fuelKilogramPerDistance = fuelKilogramPerDistance;
		this.enginePower = enginePower;
	}

	// Token: 0x170008DD RID: 2269
	// (get) Token: 0x0600867C RID: 34428 RVA: 0x000F8121 File Offset: 0x000F6321
	public float Burden
	{
		get
		{
			return this.burden;
		}
	}

	// Token: 0x170008DE RID: 2270
	// (get) Token: 0x0600867D RID: 34429 RVA: 0x000F8129 File Offset: 0x000F6329
	public float FuelKilogramPerDistance
	{
		get
		{
			return this.fuelKilogramPerDistance;
		}
	}

	// Token: 0x170008DF RID: 2271
	// (get) Token: 0x0600867E RID: 34430 RVA: 0x000F8131 File Offset: 0x000F6331
	public float EnginePower
	{
		get
		{
			return this.enginePower;
		}
	}

	// Token: 0x04006591 RID: 26001
	public float burden;

	// Token: 0x04006592 RID: 26002
	public float fuelKilogramPerDistance;

	// Token: 0x04006593 RID: 26003
	public float enginePower;
}
