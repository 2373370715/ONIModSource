using System;
using UnityEngine;

// Token: 0x02001664 RID: 5732
public class Polluter : IPolluter
{
	// Token: 0x17000776 RID: 1910
	// (get) Token: 0x0600764A RID: 30282 RVA: 0x000EDB57 File Offset: 0x000EBD57
	// (set) Token: 0x0600764B RID: 30283 RVA: 0x000EDB5F File Offset: 0x000EBD5F
	public int radius
	{
		get
		{
			return this._radius;
		}
		private set
		{
			this._radius = value;
			if (this._radius == 0)
			{
				global::Debug.LogFormat("[{0}] has a 0 radius noise, this will disable it", new object[]
				{
					this.GetName()
				});
				return;
			}
		}
	}

	// Token: 0x0600764C RID: 30284 RVA: 0x000EDB8A File Offset: 0x000EBD8A
	public void SetAttributes(Vector2 pos, int dB, GameObject go, string name)
	{
		this.position = pos;
		this.sourceName = name;
		this.decibels = dB;
		this.gameObject = go;
	}

	// Token: 0x0600764D RID: 30285 RVA: 0x000EDBA9 File Offset: 0x000EBDA9
	public string GetName()
	{
		return this.sourceName;
	}

	// Token: 0x0600764E RID: 30286 RVA: 0x000EDBB1 File Offset: 0x000EBDB1
	public int GetRadius()
	{
		return this.radius;
	}

	// Token: 0x0600764F RID: 30287 RVA: 0x000EDBB9 File Offset: 0x000EBDB9
	public int GetNoise()
	{
		return this.decibels;
	}

	// Token: 0x06007650 RID: 30288 RVA: 0x000EDBC1 File Offset: 0x000EBDC1
	public GameObject GetGameObject()
	{
		return this.gameObject;
	}

	// Token: 0x06007651 RID: 30289 RVA: 0x000EDBC9 File Offset: 0x000EBDC9
	public Polluter(int radius)
	{
		this.radius = radius;
	}

	// Token: 0x06007652 RID: 30290 RVA: 0x000EDBD8 File Offset: 0x000EBDD8
	public void SetSplat(NoiseSplat new_splat)
	{
		if (new_splat == null && this.splat != null)
		{
			this.Clear();
		}
		this.splat = new_splat;
		if (this.splat != null)
		{
			AudioEventManager.Get().AddSplat(this.splat);
		}
	}

	// Token: 0x06007653 RID: 30291 RVA: 0x000EDC0A File Offset: 0x000EBE0A
	public void Clear()
	{
		if (this.splat != null)
		{
			AudioEventManager.Get().ClearNoiseSplat(this.splat);
			this.splat.Clear();
			this.splat = null;
		}
	}

	// Token: 0x06007654 RID: 30292 RVA: 0x000EDC36 File Offset: 0x000EBE36
	public Vector2 GetPosition()
	{
		return this.position;
	}

	// Token: 0x04005892 RID: 22674
	private int _radius;

	// Token: 0x04005893 RID: 22675
	private int decibels;

	// Token: 0x04005894 RID: 22676
	private Vector2 position;

	// Token: 0x04005895 RID: 22677
	private string sourceName;

	// Token: 0x04005896 RID: 22678
	private GameObject gameObject;

	// Token: 0x04005897 RID: 22679
	private NoiseSplat splat;
}
