using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x020012A5 RID: 4773
[AddComponentMenu("KMonoBehaviour/scripts/EntombedItemManager")]
public class EntombedItemManager : KMonoBehaviour, ISim33ms
{
	// Token: 0x06006229 RID: 25129 RVA: 0x000E0091 File Offset: 0x000DE291
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.SpawnUncoveredObjects();
		this.AddMassToWorldIfPossible();
		this.PopulateEntombedItemVisualizers();
	}

	// Token: 0x0600622A RID: 25130 RVA: 0x002B5558 File Offset: 0x002B3758
	public static bool CanEntomb(Pickupable pickupable)
	{
		if (pickupable == null)
		{
			return false;
		}
		if (pickupable.storage != null)
		{
			return false;
		}
		int num = Grid.PosToCell(pickupable);
		return Grid.IsValidCell(num) && Grid.Solid[num] && !(Grid.Objects[num, 9] != null) && (pickupable.PrimaryElement.Element.IsSolid && pickupable.GetComponent<ElementChunk>() != null);
	}

	// Token: 0x0600622B RID: 25131 RVA: 0x000E00A5 File Offset: 0x000DE2A5
	public void Add(Pickupable pickupable)
	{
		this.pickupables.Add(pickupable);
	}

	// Token: 0x0600622C RID: 25132 RVA: 0x002B55DC File Offset: 0x002B37DC
	public void Sim33ms(float dt)
	{
		EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
		HashSetPool<Pickupable, EntombedItemManager>.PooledHashSet pooledHashSet = HashSetPool<Pickupable, EntombedItemManager>.Allocate();
		foreach (Pickupable pickupable in this.pickupables)
		{
			if (EntombedItemManager.CanEntomb(pickupable))
			{
				pooledHashSet.Add(pickupable);
			}
		}
		this.pickupables.Clear();
		foreach (Pickupable pickupable2 in pooledHashSet)
		{
			int num = Grid.PosToCell(pickupable2);
			PrimaryElement primaryElement = pickupable2.PrimaryElement;
			SimHashes elementID = primaryElement.ElementID;
			float mass = primaryElement.Mass;
			float temperature = primaryElement.Temperature;
			byte diseaseIdx = primaryElement.DiseaseIdx;
			int diseaseCount = primaryElement.DiseaseCount;
			Element element = Grid.Element[num];
			if (elementID == element.id && mass > 0.010000001f && Grid.Mass[num] + mass < element.maxMass)
			{
				SimMessages.AddRemoveSubstance(num, ElementLoader.FindElementByHash(elementID).idx, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature, diseaseIdx, diseaseCount, true, -1);
			}
			else
			{
				component.AddItem(num);
				this.cells.Add(num);
				this.elementIds.Add((int)elementID);
				this.masses.Add(mass);
				this.temperatures.Add(temperature);
				this.diseaseIndices.Add(diseaseIdx);
				this.diseaseCounts.Add(diseaseCount);
			}
			Util.KDestroyGameObject(pickupable2.gameObject);
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x0600622D RID: 25133 RVA: 0x002B57A4 File Offset: 0x002B39A4
	public void OnSolidChanged(List<int> solid_changed_cells)
	{
		ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
		foreach (int num in solid_changed_cells)
		{
			if (!Grid.Solid[num])
			{
				pooledList.Add(num);
			}
		}
		ListPool<int, EntombedItemManager>.PooledList pooledList2 = ListPool<int, EntombedItemManager>.Allocate();
		for (int i = 0; i < this.cells.Count; i++)
		{
			int num2 = this.cells[i];
			foreach (int num3 in pooledList)
			{
				if (num2 == num3)
				{
					pooledList2.Add(i);
					break;
				}
			}
		}
		pooledList.Recycle();
		this.SpawnObjects(pooledList2);
		pooledList2.Recycle();
	}

	// Token: 0x0600622E RID: 25134 RVA: 0x002B5890 File Offset: 0x002B3A90
	private void SpawnUncoveredObjects()
	{
		ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
		for (int i = 0; i < this.cells.Count; i++)
		{
			int i2 = this.cells[i];
			if (!Grid.Solid[i2])
			{
				pooledList.Add(i);
			}
		}
		this.SpawnObjects(pooledList);
		pooledList.Recycle();
	}

	// Token: 0x0600622F RID: 25135 RVA: 0x002B58E8 File Offset: 0x002B3AE8
	private void AddMassToWorldIfPossible()
	{
		ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
		for (int i = 0; i < this.cells.Count; i++)
		{
			int num = this.cells[i];
			if (Grid.Solid[num] && Grid.Element[num].id == (SimHashes)this.elementIds[i])
			{
				pooledList.Add(i);
			}
		}
		pooledList.Sort();
		pooledList.Reverse();
		foreach (int item_idx in pooledList)
		{
			EntombedItemManager.Item item = this.GetItem(item_idx);
			this.RemoveItem(item_idx);
			if (item.mass > 1E-45f)
			{
				SimMessages.AddRemoveSubstance(item.cell, ElementLoader.FindElementByHash((SimHashes)item.elementId).idx, CellEventLogger.Instance.ElementConsumerSimUpdate, item.mass, item.temperature, item.diseaseIdx, item.diseaseCount, false, -1);
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06006230 RID: 25136 RVA: 0x002B5A00 File Offset: 0x002B3C00
	private void RemoveItem(int item_idx)
	{
		this.cells.RemoveAt(item_idx);
		this.elementIds.RemoveAt(item_idx);
		this.masses.RemoveAt(item_idx);
		this.temperatures.RemoveAt(item_idx);
		this.diseaseIndices.RemoveAt(item_idx);
		this.diseaseCounts.RemoveAt(item_idx);
	}

	// Token: 0x06006231 RID: 25137 RVA: 0x002B5A58 File Offset: 0x002B3C58
	private EntombedItemManager.Item GetItem(int item_idx)
	{
		return new EntombedItemManager.Item
		{
			cell = this.cells[item_idx],
			elementId = this.elementIds[item_idx],
			mass = this.masses[item_idx],
			temperature = this.temperatures[item_idx],
			diseaseIdx = this.diseaseIndices[item_idx],
			diseaseCount = this.diseaseCounts[item_idx]
		};
	}

	// Token: 0x06006232 RID: 25138 RVA: 0x002B5AE0 File Offset: 0x002B3CE0
	private void SpawnObjects(List<int> uncovered_item_indices)
	{
		uncovered_item_indices.Sort();
		uncovered_item_indices.Reverse();
		EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
		foreach (int item_idx in uncovered_item_indices)
		{
			EntombedItemManager.Item item = this.GetItem(item_idx);
			component.RemoveItem(item.cell);
			this.RemoveItem(item_idx);
			Element element = ElementLoader.FindElementByHash((SimHashes)item.elementId);
			if (element != null)
			{
				element.substance.SpawnResource(Grid.CellToPosCCC(item.cell, Grid.SceneLayer.Ore), item.mass, item.temperature, item.diseaseIdx, item.diseaseCount, false, false, false);
			}
		}
	}

	// Token: 0x06006233 RID: 25139 RVA: 0x002B5BA0 File Offset: 0x002B3DA0
	private void PopulateEntombedItemVisualizers()
	{
		EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
		foreach (int cell in this.cells)
		{
			component.AddItem(cell);
		}
	}

	// Token: 0x040045DB RID: 17883
	[Serialize]
	private List<int> cells = new List<int>();

	// Token: 0x040045DC RID: 17884
	[Serialize]
	private List<int> elementIds = new List<int>();

	// Token: 0x040045DD RID: 17885
	[Serialize]
	private List<float> masses = new List<float>();

	// Token: 0x040045DE RID: 17886
	[Serialize]
	private List<float> temperatures = new List<float>();

	// Token: 0x040045DF RID: 17887
	[Serialize]
	private List<byte> diseaseIndices = new List<byte>();

	// Token: 0x040045E0 RID: 17888
	[Serialize]
	private List<int> diseaseCounts = new List<int>();

	// Token: 0x040045E1 RID: 17889
	private List<Pickupable> pickupables = new List<Pickupable>();

	// Token: 0x020012A6 RID: 4774
	private struct Item
	{
		// Token: 0x040045E2 RID: 17890
		public int cell;

		// Token: 0x040045E3 RID: 17891
		public int elementId;

		// Token: 0x040045E4 RID: 17892
		public float mass;

		// Token: 0x040045E5 RID: 17893
		public float temperature;

		// Token: 0x040045E6 RID: 17894
		public byte diseaseIdx;

		// Token: 0x040045E7 RID: 17895
		public int diseaseCount;
	}
}
