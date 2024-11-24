using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200094B RID: 2379
[AddComponentMenu("KMonoBehaviour/scripts/AudioEventManager")]
public class AudioEventManager : KMonoBehaviour
{
	// Token: 0x06002AFC RID: 11004 RVA: 0x001DD424 File Offset: 0x001DB624
	public static AudioEventManager Get()
	{
		if (AudioEventManager.instance == null)
		{
			if (App.IsExiting)
			{
				return null;
			}
			GameObject gameObject = GameObject.Find("/AudioEventManager");
			if (gameObject == null)
			{
				gameObject = new GameObject();
				gameObject.name = "AudioEventManager";
			}
			AudioEventManager.instance = gameObject.GetComponent<AudioEventManager>();
			if (AudioEventManager.instance == null)
			{
				AudioEventManager.instance = gameObject.AddComponent<AudioEventManager>();
			}
		}
		return AudioEventManager.instance;
	}

	// Token: 0x06002AFD RID: 11005 RVA: 0x000BBF41 File Offset: 0x000BA141
	protected override void OnSpawn()
	{
		base.OnPrefabInit();
		this.spatialSplats.Reset(Grid.WidthInCells, Grid.HeightInCells, 16, 16);
	}

	// Token: 0x06002AFE RID: 11006 RVA: 0x000BBF62 File Offset: 0x000BA162
	public static float LoudnessToDB(float loudness)
	{
		if (loudness <= 0f)
		{
			return 0f;
		}
		return 10f * Mathf.Log10(loudness);
	}

	// Token: 0x06002AFF RID: 11007 RVA: 0x000BBF7E File Offset: 0x000BA17E
	public static float DBToLoudness(float src_db)
	{
		return Mathf.Pow(10f, src_db / 10f);
	}

	// Token: 0x06002B00 RID: 11008 RVA: 0x000BBF91 File Offset: 0x000BA191
	public float GetDecibelsAtCell(int cell)
	{
		return Mathf.Round(AudioEventManager.LoudnessToDB(Grid.Loudness[cell]) * 2f) / 2f;
	}

	// Token: 0x06002B01 RID: 11009 RVA: 0x001DD494 File Offset: 0x001DB694
	public static string GetLoudestNoisePolluterAtCell(int cell)
	{
		float negativeInfinity = float.NegativeInfinity;
		string result = null;
		AudioEventManager audioEventManager = AudioEventManager.Get();
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2 pos = new Vector2((float)vector2I.x, (float)vector2I.y);
		foreach (object obj in audioEventManager.spatialSplats.GetAllIntersecting(pos))
		{
			NoiseSplat noiseSplat = (NoiseSplat)obj;
			if (noiseSplat.GetLoudness(cell) > negativeInfinity)
			{
				result = noiseSplat.GetProvider().GetName();
			}
		}
		return result;
	}

	// Token: 0x06002B02 RID: 11010 RVA: 0x000BBFB0 File Offset: 0x000BA1B0
	public void ClearNoiseSplat(NoiseSplat splat)
	{
		if (this.splats.Contains(splat))
		{
			this.splats.Remove(splat);
			this.spatialSplats.Remove(splat);
		}
	}

	// Token: 0x06002B03 RID: 11011 RVA: 0x000BBFD9 File Offset: 0x000BA1D9
	public void AddSplat(NoiseSplat splat)
	{
		this.splats.Add(splat);
		this.spatialSplats.Add(splat);
	}

	// Token: 0x06002B04 RID: 11012 RVA: 0x001DD538 File Offset: 0x001DB738
	public NoiseSplat CreateNoiseSplat(Vector2 pos, int dB, int radius, string name, GameObject go)
	{
		Polluter polluter = this.GetPolluter(radius);
		polluter.SetAttributes(pos, dB, go, name);
		NoiseSplat noiseSplat = new NoiseSplat(polluter, 0f);
		polluter.SetSplat(noiseSplat);
		return noiseSplat;
	}

	// Token: 0x06002B05 RID: 11013 RVA: 0x001DD56C File Offset: 0x001DB76C
	public List<AudioEventManager.PolluterDisplay> GetPollutersForCell(int cell)
	{
		this.polluters.Clear();
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2 pos = new Vector2((float)vector2I.x, (float)vector2I.y);
		foreach (object obj in this.spatialSplats.GetAllIntersecting(pos))
		{
			NoiseSplat noiseSplat = (NoiseSplat)obj;
			float loudness = noiseSplat.GetLoudness(cell);
			if (loudness > 0f)
			{
				AudioEventManager.PolluterDisplay item = default(AudioEventManager.PolluterDisplay);
				item.name = noiseSplat.GetName();
				item.value = AudioEventManager.LoudnessToDB(loudness);
				item.provider = noiseSplat.GetProvider();
				this.polluters.Add(item);
			}
		}
		return this.polluters;
	}

	// Token: 0x06002B06 RID: 11014 RVA: 0x001DD644 File Offset: 0x001DB844
	private void RemoveExpiredSplats()
	{
		if (this.removeTime.Count > 1)
		{
			this.removeTime.Sort((Pair<float, NoiseSplat> a, Pair<float, NoiseSplat> b) => a.first.CompareTo(b.first));
		}
		int num = -1;
		int num2 = 0;
		while (num2 < this.removeTime.Count && this.removeTime[num2].first <= Time.time)
		{
			NoiseSplat second = this.removeTime[num2].second;
			if (second != null)
			{
				IPolluter provider = second.GetProvider();
				this.FreePolluter(provider as Polluter);
			}
			num = num2;
			num2++;
		}
		for (int i = num; i >= 0; i--)
		{
			this.removeTime.RemoveAt(i);
		}
	}

	// Token: 0x06002B07 RID: 11015 RVA: 0x000BBFF4 File Offset: 0x000BA1F4
	private void Update()
	{
		this.RemoveExpiredSplats();
	}

	// Token: 0x06002B08 RID: 11016 RVA: 0x001DD700 File Offset: 0x001DB900
	private Polluter GetPolluter(int radius)
	{
		if (!this.freePool.ContainsKey(radius))
		{
			this.freePool.Add(radius, new List<Polluter>());
		}
		Polluter polluter;
		if (this.freePool[radius].Count > 0)
		{
			polluter = this.freePool[radius][0];
			this.freePool[radius].RemoveAt(0);
		}
		else
		{
			polluter = new Polluter(radius);
		}
		if (!this.inusePool.ContainsKey(radius))
		{
			this.inusePool.Add(radius, new List<Polluter>());
		}
		this.inusePool[radius].Add(polluter);
		return polluter;
	}

	// Token: 0x06002B09 RID: 11017 RVA: 0x001DD7A4 File Offset: 0x001DB9A4
	private void FreePolluter(Polluter pol)
	{
		if (pol != null)
		{
			pol.Clear();
			global::Debug.Assert(this.inusePool[pol.radius].Contains(pol));
			this.inusePool[pol.radius].Remove(pol);
			this.freePool[pol.radius].Add(pol);
		}
	}

	// Token: 0x06002B0A RID: 11018 RVA: 0x001DD808 File Offset: 0x001DBA08
	public void PlayTimedOnceOff(Vector2 pos, int dB, int radius, string name, GameObject go, float time = 1f)
	{
		if (dB > 0 && radius > 0 && time > 0f)
		{
			Polluter polluter = this.GetPolluter(radius);
			polluter.SetAttributes(pos, dB, go, name);
			this.AddTimedInstance(polluter, time);
		}
	}

	// Token: 0x06002B0B RID: 11019 RVA: 0x001DD844 File Offset: 0x001DBA44
	private void AddTimedInstance(Polluter p, float time)
	{
		NoiseSplat noiseSplat = new NoiseSplat(p, time + Time.time);
		p.SetSplat(noiseSplat);
		this.removeTime.Add(new Pair<float, NoiseSplat>(time + Time.time, noiseSplat));
	}

	// Token: 0x06002B0C RID: 11020 RVA: 0x000BBFFC File Offset: 0x000BA1FC
	private static void SoundLog(long itemId, string message)
	{
		global::Debug.Log(" [" + itemId.ToString() + "] \t" + message);
	}

	// Token: 0x04001CBC RID: 7356
	public const float NO_NOISE_EFFECTORS = 0f;

	// Token: 0x04001CBD RID: 7357
	public const float MIN_LOUDNESS_THRESHOLD = 1f;

	// Token: 0x04001CBE RID: 7358
	private static AudioEventManager instance;

	// Token: 0x04001CBF RID: 7359
	private List<Pair<float, NoiseSplat>> removeTime = new List<Pair<float, NoiseSplat>>();

	// Token: 0x04001CC0 RID: 7360
	private Dictionary<int, List<Polluter>> freePool = new Dictionary<int, List<Polluter>>();

	// Token: 0x04001CC1 RID: 7361
	private Dictionary<int, List<Polluter>> inusePool = new Dictionary<int, List<Polluter>>();

	// Token: 0x04001CC2 RID: 7362
	private HashSet<NoiseSplat> splats = new HashSet<NoiseSplat>();

	// Token: 0x04001CC3 RID: 7363
	private UniformGrid<NoiseSplat> spatialSplats = new UniformGrid<NoiseSplat>();

	// Token: 0x04001CC4 RID: 7364
	private List<AudioEventManager.PolluterDisplay> polluters = new List<AudioEventManager.PolluterDisplay>();

	// Token: 0x0200094C RID: 2380
	public enum NoiseEffect
	{
		// Token: 0x04001CC6 RID: 7366
		Peaceful,
		// Token: 0x04001CC7 RID: 7367
		Quiet = 36,
		// Token: 0x04001CC8 RID: 7368
		TossAndTurn = 45,
		// Token: 0x04001CC9 RID: 7369
		WakeUp = 60,
		// Token: 0x04001CCA RID: 7370
		Passive = 80,
		// Token: 0x04001CCB RID: 7371
		Active = 106
	}

	// Token: 0x0200094D RID: 2381
	public struct PolluterDisplay
	{
		// Token: 0x04001CCC RID: 7372
		public string name;

		// Token: 0x04001CCD RID: 7373
		public float value;

		// Token: 0x04001CCE RID: 7374
		public IPolluter provider;
	}
}
