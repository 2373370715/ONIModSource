using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/MinionGroupProber")]
public class MinionGroupProber : KMonoBehaviour, IGroupProber
{
	public static void DestroyInstance()
	{
		MinionGroupProber.Instance = null;
	}

	public static MinionGroupProber Get()
	{
		return MinionGroupProber.Instance;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MinionGroupProber.Instance = this;
		this.cells = new Dictionary<object, short>[Grid.CellCount];
	}

	private bool IsReachable_AssumeLock(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		Dictionary<object, short> dictionary = this.cells[cell];
		if (dictionary == null)
		{
			return false;
		}
		bool result = false;
		foreach (KeyValuePair<object, short> keyValuePair in dictionary)
		{
			object key = keyValuePair.Key;
			short value = keyValuePair.Value;
			KeyValuePair<short, short> keyValuePair2;
			if (this.valid_serial_nos.TryGetValue(key, out keyValuePair2) && (value == keyValuePair2.Key || value == keyValuePair2.Value))
			{
				result = true;
				break;
			}
			this.pending_removals.Add(key);
		}
		foreach (object key2 in this.pending_removals)
		{
			dictionary.Remove(key2);
			if (dictionary.Count == 0)
			{
				this.cells[cell] = null;
			}
		}
		this.pending_removals.Clear();
		return result;
	}

	public bool IsReachable(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		bool result = false;
		object obj = this.access;
		lock (obj)
		{
			result = this.IsReachable_AssumeLock(cell);
		}
		return result;
	}

	public bool IsReachable(int cell, CellOffset[] offsets)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		bool result = false;
		object obj = this.access;
		lock (obj)
		{
			foreach (CellOffset offset in offsets)
			{
				if (this.IsReachable_AssumeLock(Grid.OffsetCell(cell, offset)))
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	public bool IsAllReachable(int cell, CellOffset[] offsets)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		bool result = false;
		object obj = this.access;
		lock (obj)
		{
			if (this.IsReachable_AssumeLock(cell))
			{
				result = true;
			}
			else
			{
				foreach (CellOffset offset in offsets)
				{
					if (this.IsReachable_AssumeLock(Grid.OffsetCell(cell, offset)))
					{
						result = true;
						break;
					}
				}
			}
		}
		return result;
	}

	public bool IsReachable(Workable workable)
	{
		return this.IsReachable(Grid.PosToCell(workable), workable.GetOffsets());
	}

	public void Occupy(object prober, short serial_no, IEnumerable<int> cells)
	{
		object obj = this.access;
		lock (obj)
		{
			foreach (int num in cells)
			{
				if (this.cells[num] == null)
				{
					this.cells[num] = new Dictionary<object, short>();
				}
				this.cells[num][prober] = serial_no;
			}
		}
	}

	public void SetValidSerialNos(object prober, short previous_serial_no, short serial_no)
	{
		object obj = this.access;
		lock (obj)
		{
			this.valid_serial_nos[prober] = new KeyValuePair<short, short>(previous_serial_no, serial_no);
		}
	}

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

	private static MinionGroupProber Instance;

	private Dictionary<object, short>[] cells;

	private Dictionary<object, KeyValuePair<short, short>> valid_serial_nos = new Dictionary<object, KeyValuePair<short, short>>();

	private List<object> pending_removals = new List<object>();

	private readonly object access = new object();
}
