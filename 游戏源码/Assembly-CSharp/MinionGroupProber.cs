using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020014E9 RID: 5353
[AddComponentMenu("KMonoBehaviour/scripts/MinionGroupProber")]
public class MinionGroupProber : KMonoBehaviour, IGroupProber, ISim200ms
{
	// Token: 0x06006F77 RID: 28535 RVA: 0x000E8F21 File Offset: 0x000E7121
	public static void DestroyInstance()
	{
		MinionGroupProber.Instance = null;
	}

	// Token: 0x06006F78 RID: 28536 RVA: 0x000E8F29 File Offset: 0x000E7129
	public static MinionGroupProber Get()
	{
		return MinionGroupProber.Instance;
	}

	// Token: 0x06006F79 RID: 28537 RVA: 0x002F3C08 File Offset: 0x002F1E08
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MinionGroupProber.Instance = this;
		this.cells = new Dictionary<object, short>[Grid.CellCount];
		for (int i = 0; i < Grid.CellCount; i++)
		{
			this.cells[i] = new Dictionary<object, short>();
		}
		this.cell_cleanup_index = 0;
		this.cell_checks_per_frame = Grid.CellCount / 500;
	}

	// Token: 0x06006F7A RID: 28538 RVA: 0x002F3C68 File Offset: 0x002F1E68
	public bool IsReachable(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		foreach (KeyValuePair<object, short> keyValuePair in this.cells[cell])
		{
			object key = keyValuePair.Key;
			short value = keyValuePair.Value;
			KeyValuePair<short, short> keyValuePair2;
			if (this.valid_serial_nos.TryGetValue(key, out keyValuePair2) && (value == keyValuePair2.Key || value == keyValuePair2.Value))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006F7B RID: 28539 RVA: 0x002F3D00 File Offset: 0x002F1F00
	public bool IsReachable(int cell, CellOffset[] offsets)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		foreach (CellOffset offset in offsets)
		{
			if (this.IsReachable(Grid.OffsetCell(cell, offset)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006F7C RID: 28540 RVA: 0x002F3D44 File Offset: 0x002F1F44
	public bool IsAllReachable(int cell, CellOffset[] offsets)
	{
		if (this.IsReachable(cell))
		{
			return true;
		}
		foreach (CellOffset offset in offsets)
		{
			if (this.IsReachable(Grid.OffsetCell(cell, offset)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006F7D RID: 28541 RVA: 0x000E8F30 File Offset: 0x000E7130
	public bool IsReachable(Workable workable)
	{
		return this.IsReachable(Grid.PosToCell(workable), workable.GetOffsets());
	}

	// Token: 0x06006F7E RID: 28542 RVA: 0x002F3D88 File Offset: 0x002F1F88
	public void Occupy(object prober, short serial_no, IEnumerable<int> cells)
	{
		foreach (int num in cells)
		{
			Dictionary<object, short> obj = this.cells[num];
			lock (obj)
			{
				this.cells[num][prober] = serial_no;
			}
		}
	}

	// Token: 0x06006F7F RID: 28543 RVA: 0x002F3E04 File Offset: 0x002F2004
	public void SetValidSerialNos(object prober, short previous_serial_no, short serial_no)
	{
		object obj = this.access;
		lock (obj)
		{
			this.valid_serial_nos[prober] = new KeyValuePair<short, short>(previous_serial_no, serial_no);
		}
	}

	// Token: 0x06006F80 RID: 28544 RVA: 0x002F3E54 File Offset: 0x002F2054
	public bool ReleaseProber(object prober)
	{
		object obj = this.access;
		bool result;
		lock (obj)
		{
			result = this.valid_serial_nos.Remove(prober);
		}
		return result;
	}

	// Token: 0x06006F81 RID: 28545 RVA: 0x002F3E9C File Offset: 0x002F209C
	public void Sim200ms(float dt)
	{
		int i = 0;
		while (i < this.cell_checks_per_frame)
		{
			this.pending_removals.Clear();
			foreach (KeyValuePair<object, short> keyValuePair in this.cells[this.cell_cleanup_index])
			{
				KeyValuePair<short, short> keyValuePair2;
				if (!this.valid_serial_nos.TryGetValue(keyValuePair.Key, out keyValuePair2) || (keyValuePair2.Key != keyValuePair.Value && keyValuePair2.Value != keyValuePair.Value))
				{
					this.pending_removals.Add(keyValuePair.Key);
				}
			}
			foreach (object key in this.pending_removals)
			{
				this.cells[this.cell_cleanup_index].Remove(key);
			}
			i++;
			this.cell_cleanup_index = (this.cell_cleanup_index + 1) % this.cells.Length;
		}
	}

	// Token: 0x0400535A RID: 21338
	private static MinionGroupProber Instance;

	// Token: 0x0400535B RID: 21339
	private Dictionary<object, short>[] cells;

	// Token: 0x0400535C RID: 21340
	private Dictionary<object, KeyValuePair<short, short>> valid_serial_nos = new Dictionary<object, KeyValuePair<short, short>>();

	// Token: 0x0400535D RID: 21341
	private List<object> pending_removals = new List<object>();

	// Token: 0x0400535E RID: 21342
	private int cell_cleanup_index;

	// Token: 0x0400535F RID: 21343
	private int cell_checks_per_frame;

	// Token: 0x04005360 RID: 21344
	private readonly object access = new object();
}
