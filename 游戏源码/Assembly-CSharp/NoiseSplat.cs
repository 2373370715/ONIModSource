using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000966 RID: 2406
public class NoiseSplat : IUniformGridObject
{
	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06002B55 RID: 11093 RVA: 0x000BC2C1 File Offset: 0x000BA4C1
	// (set) Token: 0x06002B56 RID: 11094 RVA: 0x000BC2C9 File Offset: 0x000BA4C9
	public int dB { get; private set; }

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06002B57 RID: 11095 RVA: 0x000BC2D2 File Offset: 0x000BA4D2
	// (set) Token: 0x06002B58 RID: 11096 RVA: 0x000BC2DA File Offset: 0x000BA4DA
	public float deathTime { get; private set; }

	// Token: 0x06002B59 RID: 11097 RVA: 0x000BC2E3 File Offset: 0x000BA4E3
	public string GetName()
	{
		return this.provider.GetName();
	}

	// Token: 0x06002B5A RID: 11098 RVA: 0x000BC2F0 File Offset: 0x000BA4F0
	public IPolluter GetProvider()
	{
		return this.provider;
	}

	// Token: 0x06002B5B RID: 11099 RVA: 0x000BC2F8 File Offset: 0x000BA4F8
	public Vector2 PosMin()
	{
		return new Vector2(this.position.x - (float)this.radius, this.position.y - (float)this.radius);
	}

	// Token: 0x06002B5C RID: 11100 RVA: 0x000BC325 File Offset: 0x000BA525
	public Vector2 PosMax()
	{
		return new Vector2(this.position.x + (float)this.radius, this.position.y + (float)this.radius);
	}

	// Token: 0x06002B5D RID: 11101 RVA: 0x001DF3A4 File Offset: 0x001DD5A4
	public NoiseSplat(NoisePolluter setProvider, float death_time = 0f)
	{
		this.deathTime = death_time;
		this.dB = 0;
		this.radius = 5;
		if (setProvider.dB != null)
		{
			this.dB = (int)setProvider.dB.GetTotalValue();
		}
		int cell = Grid.PosToCell(setProvider.gameObject);
		if (!NoisePolluter.IsNoiseableCell(cell))
		{
			this.dB = 0;
		}
		if (this.dB == 0)
		{
			return;
		}
		setProvider.Clear();
		OccupyArea occupyArea = setProvider.occupyArea;
		this.baseExtents = occupyArea.GetExtents();
		this.provider = setProvider;
		this.position = setProvider.transform.GetPosition();
		if (setProvider.dBRadius != null)
		{
			this.radius = (int)setProvider.dBRadius.GetTotalValue();
		}
		if (this.radius == 0)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int widthInCells = occupyArea.GetWidthInCells();
		int heightInCells = occupyArea.GetHeightInCells();
		Vector2I vector2I = new Vector2I(num - this.radius, num2 - this.radius);
		Vector2I vector2I2 = vector2I + new Vector2I(this.radius * 2 + widthInCells, this.radius * 2 + heightInCells);
		vector2I = Vector2I.Max(vector2I, Vector2I.zero);
		vector2I2 = Vector2I.Min(vector2I2, new Vector2I(Grid.WidthInCells - 1, Grid.HeightInCells - 1));
		this.effectExtents = new Extents(vector2I.x, vector2I.y, vector2I2.x - vector2I.x, vector2I2.y - vector2I.y);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("NoiseSplat.SplatCollectNoisePolluters", setProvider.gameObject, this.effectExtents, GameScenePartitioner.Instance.noisePolluterLayer, setProvider.onCollectNoisePollutersCallback);
		this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("NoiseSplat.SplatSolidCheck", setProvider.gameObject, this.effectExtents, GameScenePartitioner.Instance.solidChangedLayer, setProvider.refreshPartionerCallback);
	}

	// Token: 0x06002B5E RID: 11102 RVA: 0x001DF58C File Offset: 0x001DD78C
	public NoiseSplat(IPolluter setProvider, float death_time = 0f)
	{
		this.deathTime = death_time;
		this.provider = setProvider;
		this.provider.Clear();
		this.position = this.provider.GetPosition();
		this.dB = this.provider.GetNoise();
		int cell = Grid.PosToCell(this.position);
		if (!NoisePolluter.IsNoiseableCell(cell))
		{
			this.dB = 0;
		}
		if (this.dB == 0)
		{
			return;
		}
		this.radius = this.provider.GetRadius();
		if (this.radius == 0)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		Vector2I vector2I = new Vector2I(num - this.radius, num2 - this.radius);
		Vector2I vector2I2 = vector2I + new Vector2I(this.radius * 2, this.radius * 2);
		vector2I = Vector2I.Max(vector2I, Vector2I.zero);
		vector2I2 = Vector2I.Min(vector2I2, new Vector2I(Grid.WidthInCells - 1, Grid.HeightInCells - 1));
		this.effectExtents = new Extents(vector2I.x, vector2I.y, vector2I2.x - vector2I.x, vector2I2.y - vector2I.y);
		this.baseExtents = new Extents(num, num2, 1, 1);
		this.AddNoise();
	}

	// Token: 0x06002B5F RID: 11103 RVA: 0x000BC352 File Offset: 0x000BA552
	public void Clear()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
		this.RemoveNoise();
	}

	// Token: 0x06002B60 RID: 11104 RVA: 0x001DF6D8 File Offset: 0x001DD8D8
	private void AddNoise()
	{
		int cell = Grid.PosToCell(this.position);
		int num = this.effectExtents.x + this.effectExtents.width;
		int num2 = this.effectExtents.y + this.effectExtents.height;
		int num3 = this.effectExtents.x;
		int num4 = this.effectExtents.y;
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		num = Math.Min(num, Grid.WidthInCells);
		num2 = Math.Min(num2, Grid.HeightInCells);
		num3 = Math.Max(0, num3);
		num4 = Math.Max(0, num4);
		for (int i = num4; i < num2; i++)
		{
			for (int j = num3; j < num; j++)
			{
				if (Grid.VisibilityTest(x, y, j, i, false))
				{
					int num5 = Grid.XYToCell(j, i);
					float dbforCell = this.GetDBForCell(num5);
					if (dbforCell > 0f)
					{
						float num6 = AudioEventManager.DBToLoudness(dbforCell);
						Grid.Loudness[num5] += num6;
						Pair<int, float> item = new Pair<int, float>(num5, num6);
						this.decibels.Add(item);
					}
				}
			}
		}
	}

	// Token: 0x06002B61 RID: 11105 RVA: 0x001DF7F0 File Offset: 0x001DD9F0
	public float GetDBForCell(int cell)
	{
		Vector2 vector = Grid.CellToPos2D(cell);
		float num = Mathf.Floor(Vector2.Distance(this.position, vector));
		if (vector.x >= (float)this.baseExtents.x && vector.x < (float)(this.baseExtents.x + this.baseExtents.width) && vector.y >= (float)this.baseExtents.y && vector.y < (float)(this.baseExtents.y + this.baseExtents.height))
		{
			num = 0f;
		}
		return Mathf.Round((float)this.dB - (float)this.dB * num * 0.05f);
	}

	// Token: 0x06002B62 RID: 11106 RVA: 0x001DF8A8 File Offset: 0x001DDAA8
	private void RemoveNoise()
	{
		for (int i = 0; i < this.decibels.Count; i++)
		{
			Pair<int, float> pair = this.decibels[i];
			float num = Math.Max(0f, Grid.Loudness[pair.first] - pair.second);
			Grid.Loudness[pair.first] = ((num < 1f) ? 0f : num);
		}
		this.decibels.Clear();
	}

	// Token: 0x06002B63 RID: 11107 RVA: 0x001DF920 File Offset: 0x001DDB20
	public float GetLoudness(int cell)
	{
		float result = 0f;
		for (int i = 0; i < this.decibels.Count; i++)
		{
			Pair<int, float> pair = this.decibels[i];
			if (pair.first == cell)
			{
				result = pair.second;
				break;
			}
		}
		return result;
	}

	// Token: 0x04001D21 RID: 7457
	public const float noiseFalloff = 0.05f;

	// Token: 0x04001D24 RID: 7460
	private IPolluter provider;

	// Token: 0x04001D25 RID: 7461
	private Vector2 position;

	// Token: 0x04001D26 RID: 7462
	private int radius;

	// Token: 0x04001D27 RID: 7463
	private Extents effectExtents;

	// Token: 0x04001D28 RID: 7464
	private Extents baseExtents;

	// Token: 0x04001D29 RID: 7465
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001D2A RID: 7466
	private HandleVector<int>.Handle solidChangedPartitionerEntry;

	// Token: 0x04001D2B RID: 7467
	private List<Pair<int, float>> decibels = new List<Pair<int, float>>();
}
