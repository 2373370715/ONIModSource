using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012A7 RID: 4775
[AddComponentMenu("KMonoBehaviour/scripts/EntombedItemVisualizer")]
public class EntombedItemVisualizer : KMonoBehaviour
{
	// Token: 0x06006235 RID: 25141 RVA: 0x000E00B3 File Offset: 0x000DE2B3
	public void Clear()
	{
		this.cellEntombedCounts.Clear();
	}

	// Token: 0x06006236 RID: 25142 RVA: 0x000E00C0 File Offset: 0x000DE2C0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.entombedItemPool = new GameObjectPool(new Func<GameObject>(this.InstantiateEntombedObject), 32);
	}

	// Token: 0x06006237 RID: 25143 RVA: 0x002B5C60 File Offset: 0x002B3E60
	public bool AddItem(int cell)
	{
		bool result = false;
		if (Grid.Objects[cell, 9] == null)
		{
			result = true;
			EntombedItemVisualizer.Data data;
			this.cellEntombedCounts.TryGetValue(cell, out data);
			if (data.refCount == 0)
			{
				GameObject instance = this.entombedItemPool.GetInstance();
				instance.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront));
				instance.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.value * 360f);
				KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
				int num = UnityEngine.Random.Range(0, EntombedItemVisualizer.EntombedVisualizerAnims.Length);
				string text = EntombedItemVisualizer.EntombedVisualizerAnims[num];
				component.initialAnim = text;
				instance.SetActive(true);
				component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
				data.controller = component;
			}
			data.refCount++;
			this.cellEntombedCounts[cell] = data;
		}
		return result;
	}

	// Token: 0x06006238 RID: 25144 RVA: 0x002B5D50 File Offset: 0x002B3F50
	public void RemoveItem(int cell)
	{
		EntombedItemVisualizer.Data data;
		if (this.cellEntombedCounts.TryGetValue(cell, out data))
		{
			data.refCount--;
			if (data.refCount == 0)
			{
				this.ReleaseVisualizer(cell, data);
				return;
			}
			this.cellEntombedCounts[cell] = data;
		}
	}

	// Token: 0x06006239 RID: 25145 RVA: 0x002B5D98 File Offset: 0x002B3F98
	public void ForceClear(int cell)
	{
		EntombedItemVisualizer.Data data;
		if (this.cellEntombedCounts.TryGetValue(cell, out data))
		{
			this.ReleaseVisualizer(cell, data);
		}
	}

	// Token: 0x0600623A RID: 25146 RVA: 0x002B5DC0 File Offset: 0x002B3FC0
	private void ReleaseVisualizer(int cell, EntombedItemVisualizer.Data data)
	{
		if (data.controller != null)
		{
			data.controller.gameObject.SetActive(false);
			this.entombedItemPool.ReleaseInstance(data.controller.gameObject);
		}
		this.cellEntombedCounts.Remove(cell);
	}

	// Token: 0x0600623B RID: 25147 RVA: 0x000E00E1 File Offset: 0x000DE2E1
	public bool IsEntombedItem(int cell)
	{
		return this.cellEntombedCounts.ContainsKey(cell) && this.cellEntombedCounts[cell].refCount > 0;
	}

	// Token: 0x0600623C RID: 25148 RVA: 0x000E0107 File Offset: 0x000DE307
	private GameObject InstantiateEntombedObject()
	{
		GameObject gameObject = GameUtil.KInstantiate(this.entombedItemPrefab, Grid.SceneLayer.FXFront, null, 0);
		gameObject.SetActive(false);
		return gameObject;
	}

	// Token: 0x040045E8 RID: 17896
	[SerializeField]
	private GameObject entombedItemPrefab;

	// Token: 0x040045E9 RID: 17897
	private static readonly string[] EntombedVisualizerAnims = new string[]
	{
		"idle1",
		"idle2",
		"idle3",
		"idle4"
	};

	// Token: 0x040045EA RID: 17898
	private GameObjectPool entombedItemPool;

	// Token: 0x040045EB RID: 17899
	private Dictionary<int, EntombedItemVisualizer.Data> cellEntombedCounts = new Dictionary<int, EntombedItemVisualizer.Data>();

	// Token: 0x020012A8 RID: 4776
	private struct Data
	{
		// Token: 0x040045EC RID: 17900
		public int refCount;

		// Token: 0x040045ED RID: 17901
		public KBatchedAnimController controller;
	}
}
