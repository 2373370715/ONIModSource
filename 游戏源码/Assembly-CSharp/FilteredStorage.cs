using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FilteredStorage
{
	public static readonly HashedString FULL_PORT_ID = "FULL";

	private KMonoBehaviour root;

	private FetchList2 fetchList;

	private IUserControlledCapacity capacityControl;

	private TreeFilterable filterable;

	private Storage storage;

	private MeterController meter;

	private MeterController logicMeter;

	private Tag requiredTag = Tag.Invalid;

	private Tag[] forbiddenTags;

	private bool hasMeter = true;

	private bool useLogicMeter;

	private ChoreType choreType;

	public void SetHasMeter(bool has_meter)
	{
		hasMeter = has_meter;
	}

	public FilteredStorage(KMonoBehaviour root, Tag[] forbidden_tags, IUserControlledCapacity capacity_control, bool use_logic_meter, ChoreType fetch_chore_type)
	{
		this.root = root;
		forbiddenTags = forbidden_tags;
		capacityControl = capacity_control;
		useLogicMeter = use_logic_meter;
		choreType = fetch_chore_type;
		root.Subscribe(-1697596308, OnStorageChanged);
		root.Subscribe(-543130682, OnUserSettingsChanged);
		filterable = root.FindOrAdd<TreeFilterable>();
		TreeFilterable treeFilterable = filterable;
		treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(OnFilterChanged));
		storage = root.GetComponent<Storage>();
		storage.Subscribe(644822890, OnOnlyFetchMarkedItemsSettingChanged);
		storage.Subscribe(-1852328367, OnFunctionalChanged);
	}

	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		OnFilterChanged(filterable.GetTags());
	}

	private void CreateMeter()
	{
		if (hasMeter)
		{
			meter = new MeterController(root.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, "meter_frame", "meter_level");
		}
	}

	private void CreateLogicMeter()
	{
		if (hasMeter)
		{
			logicMeter = new MeterController(root.GetComponent<KBatchedAnimController>(), "logicmeter_target", "logicmeter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer);
		}
	}

	public void SetMeter(MeterController meter)
	{
		hasMeter = true;
		this.meter = meter;
		UpdateMeter();
	}

	public void CleanUp()
	{
		if (filterable != null)
		{
			TreeFilterable treeFilterable = filterable;
			treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(OnFilterChanged));
		}
		if (fetchList != null)
		{
			fetchList.Cancel("Parent destroyed");
		}
	}

	public void FilterChanged()
	{
		if (hasMeter)
		{
			if (meter == null)
			{
				CreateMeter();
			}
			if (logicMeter == null && useLogicMeter)
			{
				CreateLogicMeter();
			}
		}
		OnFilterChanged(filterable.GetTags());
		UpdateMeter();
	}

	private void OnUserSettingsChanged(object data)
	{
		OnFilterChanged(filterable.GetTags());
		UpdateMeter();
	}

	private void OnStorageChanged(object data)
	{
		if (fetchList == null)
		{
			OnFilterChanged(filterable.GetTags());
		}
		UpdateMeter();
	}

	private void OnFunctionalChanged(object data)
	{
		OnFilterChanged(filterable.GetTags());
	}

	private void UpdateMeter()
	{
		float maxCapacityMinusStorageMargin = GetMaxCapacityMinusStorageMargin();
		float positionPercent = Mathf.Clamp01(GetAmountStored() / maxCapacityMinusStorageMargin);
		if (meter != null)
		{
			meter.SetPositionPercent(positionPercent);
		}
	}

	public bool IsFull()
	{
		float maxCapacityMinusStorageMargin = GetMaxCapacityMinusStorageMargin();
		float num = Mathf.Clamp01(GetAmountStored() / maxCapacityMinusStorageMargin);
		if (meter != null)
		{
			meter.SetPositionPercent(num);
		}
		if (!(num >= 1f))
		{
			return false;
		}
		return true;
	}

	private void OnFetchComplete()
	{
		OnFilterChanged(filterable.GetTags());
	}

	private float GetMaxCapacity()
	{
		float num = storage.capacityKg;
		if (capacityControl != null)
		{
			num = Mathf.Min(num, capacityControl.UserMaxCapacity);
		}
		return num;
	}

	private float GetMaxCapacityMinusStorageMargin()
	{
		return GetMaxCapacity() - storage.storageFullMargin;
	}

	private float GetAmountStored()
	{
		float result = storage.MassStored();
		if (capacityControl != null)
		{
			result = capacityControl.AmountStored;
		}
		return result;
	}

	private bool IsFunctional()
	{
		Operational component = storage.GetComponent<Operational>();
		if (!(component == null))
		{
			return component.IsFunctional;
		}
		return true;
	}

	private void OnFilterChanged(HashSet<Tag> tags)
	{
		bool flag = tags != null && tags.Count != 0;
		if (fetchList != null)
		{
			fetchList.Cancel("");
			fetchList = null;
		}
		float maxCapacityMinusStorageMargin = GetMaxCapacityMinusStorageMargin();
		float amountStored = GetAmountStored();
		float num = Mathf.Max(0f, maxCapacityMinusStorageMargin - amountStored);
		if (num > 0f && flag && IsFunctional())
		{
			num = Mathf.Max(0f, GetMaxCapacity() - amountStored);
			fetchList = new FetchList2(storage, choreType);
			fetchList.ShowStatusItem = false;
			fetchList.Add(tags, requiredTag, forbiddenTags, num, Operational.State.Functional);
			fetchList.Submit(OnFetchComplete, check_storage_contents: false);
		}
	}

	public void SetLogicMeter(bool on)
	{
		if (logicMeter != null)
		{
			logicMeter.SetPositionPercent(on ? 1f : 0f);
		}
	}

	public void SetRequiredTag(Tag tag)
	{
		if (requiredTag != tag)
		{
			requiredTag = tag;
			OnFilterChanged(filterable.GetTags());
		}
	}

	public void AddForbiddenTag(Tag forbidden_tag)
	{
		if (forbiddenTags == null)
		{
			forbiddenTags = new Tag[0];
		}
		if (!forbiddenTags.Contains(forbidden_tag))
		{
			forbiddenTags = forbiddenTags.Append(forbidden_tag);
			OnFilterChanged(filterable.GetTags());
		}
	}

	public void RemoveForbiddenTag(Tag forbidden_tag)
	{
		if (forbiddenTags != null)
		{
			List<Tag> list = new List<Tag>(forbiddenTags);
			list.Remove(forbidden_tag);
			forbiddenTags = list.ToArray();
			OnFilterChanged(filterable.GetTags());
		}
	}
}
