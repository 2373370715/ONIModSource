using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x0200187E RID: 6270
public abstract class SlicedUpdaterSim1000ms<T> : KMonoBehaviour, ISim200ms where T : KMonoBehaviour, ISlicedSim1000ms
{
	// Token: 0x060081BB RID: 33211 RVA: 0x000F5501 File Offset: 0x000F3701
	protected override void OnPrefabInit()
	{
		this.InitializeSlices();
		base.OnPrefabInit();
		SlicedUpdaterSim1000ms<T>.instance = this;
	}

	// Token: 0x060081BC RID: 33212 RVA: 0x000F5515 File Offset: 0x000F3715
	protected override void OnForcedCleanUp()
	{
		SlicedUpdaterSim1000ms<T>.instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060081BD RID: 33213 RVA: 0x0033A6DC File Offset: 0x003388DC
	private void InitializeSlices()
	{
		int num = SlicedUpdaterSim1000ms<T>.NUM_200MS_BUCKETS * this.numSlicesPer200ms;
		this.m_slices = new List<SlicedUpdaterSim1000ms<T>.Slice>();
		for (int i = 0; i < num; i++)
		{
			this.m_slices.Add(new SlicedUpdaterSim1000ms<T>.Slice());
		}
		this.m_nextSliceIdx = 0;
	}

	// Token: 0x060081BE RID: 33214 RVA: 0x000F5523 File Offset: 0x000F3723
	private int GetSliceIdx(T toBeUpdated)
	{
		return toBeUpdated.GetComponent<KPrefabID>().InstanceID % this.m_slices.Count;
	}

	// Token: 0x060081BF RID: 33215 RVA: 0x0033A724 File Offset: 0x00338924
	public void RegisterUpdate1000ms(T toBeUpdated)
	{
		SlicedUpdaterSim1000ms<T>.Slice slice = this.m_slices[this.GetSliceIdx(toBeUpdated)];
		slice.Register(toBeUpdated);
		DebugUtil.DevAssert(slice.Count < this.maxUpdatesPer200ms, string.Format("The SlicedUpdaterSim1000ms for {0} wants to update no more than {1} instances per 200ms tick, but a slice has grown more than the SlicedUpdaterSim1000ms can support.", typeof(T).Name, this.maxUpdatesPer200ms), null);
	}

	// Token: 0x060081C0 RID: 33216 RVA: 0x000F5541 File Offset: 0x000F3741
	public void UnregisterUpdate1000ms(T toBeUpdated)
	{
		this.m_slices[this.GetSliceIdx(toBeUpdated)].Unregister(toBeUpdated);
	}

	// Token: 0x060081C1 RID: 33217 RVA: 0x0033A784 File Offset: 0x00338984
	public void Sim200ms(float dt)
	{
		foreach (SlicedUpdaterSim1000ms<T>.Slice slice in this.m_slices)
		{
			slice.IncrementDt(dt);
		}
		int num = 0;
		int i = 0;
		while (i < this.numSlicesPer200ms)
		{
			SlicedUpdaterSim1000ms<T>.Slice slice2 = this.m_slices[this.m_nextSliceIdx];
			num += slice2.Count;
			if (num > this.maxUpdatesPer200ms && i > 0)
			{
				break;
			}
			slice2.Update();
			i++;
			this.m_nextSliceIdx = (this.m_nextSliceIdx + 1) % this.m_slices.Count;
		}
	}

	// Token: 0x0400627C RID: 25212
	private static int NUM_200MS_BUCKETS = 5;

	// Token: 0x0400627D RID: 25213
	public static SlicedUpdaterSim1000ms<T> instance;

	// Token: 0x0400627E RID: 25214
	[Serialize]
	public int maxUpdatesPer200ms = 300;

	// Token: 0x0400627F RID: 25215
	[Serialize]
	public int numSlicesPer200ms = 3;

	// Token: 0x04006280 RID: 25216
	private List<SlicedUpdaterSim1000ms<T>.Slice> m_slices;

	// Token: 0x04006281 RID: 25217
	private int m_nextSliceIdx;

	// Token: 0x0200187F RID: 6271
	private class Slice
	{
		// Token: 0x060081C4 RID: 33220 RVA: 0x000F557D File Offset: 0x000F377D
		public void Register(T toBeUpdated)
		{
			if (this.m_timeSinceLastUpdate == 0f)
			{
				this.m_updateList.Add(toBeUpdated);
				return;
			}
			this.m_recentlyAdded[toBeUpdated] = 0f;
		}

		// Token: 0x060081C5 RID: 33221 RVA: 0x000F55AA File Offset: 0x000F37AA
		public void Unregister(T toBeUpdated)
		{
			if (!this.m_updateList.Remove(toBeUpdated))
			{
				this.m_recentlyAdded.Remove(toBeUpdated);
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x060081C6 RID: 33222 RVA: 0x000F55C7 File Offset: 0x000F37C7
		public int Count
		{
			get
			{
				return this.m_updateList.Count + this.m_recentlyAdded.Count;
			}
		}

		// Token: 0x060081C7 RID: 33223 RVA: 0x000F55E0 File Offset: 0x000F37E0
		public List<T> GetUpdateList()
		{
			List<T> list = new List<T>();
			list.AddRange(this.m_updateList);
			list.AddRange(this.m_recentlyAdded.Keys);
			return list;
		}

		// Token: 0x060081C8 RID: 33224 RVA: 0x0033A830 File Offset: 0x00338A30
		public void Update()
		{
			foreach (T t in this.m_updateList)
			{
				t.SlicedSim1000ms(this.m_timeSinceLastUpdate);
			}
			foreach (KeyValuePair<T, float> keyValuePair in this.m_recentlyAdded)
			{
				keyValuePair.Key.SlicedSim1000ms(keyValuePair.Value);
				this.m_updateList.Add(keyValuePair.Key);
			}
			this.m_recentlyAdded.Clear();
			this.m_timeSinceLastUpdate = 0f;
		}

		// Token: 0x060081C9 RID: 33225 RVA: 0x0033A908 File Offset: 0x00338B08
		public void IncrementDt(float dt)
		{
			this.m_timeSinceLastUpdate += dt;
			if (this.m_recentlyAdded.Count > 0)
			{
				foreach (T t in new List<T>(this.m_recentlyAdded.Keys))
				{
					Dictionary<T, float> recentlyAdded = this.m_recentlyAdded;
					T key = t;
					recentlyAdded[key] += dt;
				}
			}
		}

		// Token: 0x04006282 RID: 25218
		private float m_timeSinceLastUpdate;

		// Token: 0x04006283 RID: 25219
		private List<T> m_updateList = new List<T>();

		// Token: 0x04006284 RID: 25220
		private Dictionary<T, float> m_recentlyAdded = new Dictionary<T, float>();
	}
}
