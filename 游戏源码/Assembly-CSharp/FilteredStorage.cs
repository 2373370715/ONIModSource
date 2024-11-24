using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000D6C RID: 3436
public class FilteredStorage
{
	// Token: 0x06004346 RID: 17222 RVA: 0x000CB765 File Offset: 0x000C9965
	public void SetHasMeter(bool has_meter)
	{
		this.hasMeter = has_meter;
	}

	// Token: 0x06004347 RID: 17223 RVA: 0x00244424 File Offset: 0x00242624
	public FilteredStorage(KMonoBehaviour root, Tag[] forbidden_tags, IUserControlledCapacity capacity_control, bool use_logic_meter, ChoreType fetch_chore_type)
	{
		this.root = root;
		this.forbiddenTags = forbidden_tags;
		this.capacityControl = capacity_control;
		this.useLogicMeter = use_logic_meter;
		this.choreType = fetch_chore_type;
		root.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		root.Subscribe(-543130682, new Action<object>(this.OnUserSettingsChanged));
		this.filterable = root.FindOrAdd<TreeFilterable>();
		TreeFilterable treeFilterable = this.filterable;
		treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		this.storage = root.GetComponent<Storage>();
		this.storage.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		this.storage.Subscribe(-1852328367, new Action<object>(this.OnFunctionalChanged));
	}

	// Token: 0x06004348 RID: 17224 RVA: 0x000CB76E File Offset: 0x000C996E
	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		this.OnFilterChanged(this.filterable.GetTags());
	}

	// Token: 0x06004349 RID: 17225 RVA: 0x00244518 File Offset: 0x00242718
	private void CreateMeter()
	{
		if (!this.hasMeter)
		{
			return;
		}
		this.meter = new MeterController(this.root.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_frame",
			"meter_level"
		});
	}

	// Token: 0x0600434A RID: 17226 RVA: 0x000CB781 File Offset: 0x000C9981
	private void CreateLogicMeter()
	{
		if (!this.hasMeter)
		{
			return;
		}
		this.logicMeter = new MeterController(this.root.GetComponent<KBatchedAnimController>(), "logicmeter_target", "logicmeter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x0600434B RID: 17227 RVA: 0x000CB7B4 File Offset: 0x000C99B4
	public void SetMeter(MeterController meter)
	{
		this.hasMeter = true;
		this.meter = meter;
		this.UpdateMeter();
	}

	// Token: 0x0600434C RID: 17228 RVA: 0x00244568 File Offset: 0x00242768
	public void CleanUp()
	{
		if (this.filterable != null)
		{
			TreeFilterable treeFilterable = this.filterable;
			treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		}
		if (this.fetchList != null)
		{
			this.fetchList.Cancel("Parent destroyed");
		}
	}

	// Token: 0x0600434D RID: 17229 RVA: 0x002445C4 File Offset: 0x002427C4
	public void FilterChanged()
	{
		if (this.hasMeter)
		{
			if (this.meter == null)
			{
				this.CreateMeter();
			}
			if (this.logicMeter == null && this.useLogicMeter)
			{
				this.CreateLogicMeter();
			}
		}
		this.OnFilterChanged(this.filterable.GetTags());
		this.UpdateMeter();
	}

	// Token: 0x0600434E RID: 17230 RVA: 0x000CB7CA File Offset: 0x000C99CA
	private void OnUserSettingsChanged(object data)
	{
		this.OnFilterChanged(this.filterable.GetTags());
		this.UpdateMeter();
	}

	// Token: 0x0600434F RID: 17231 RVA: 0x000CB7E3 File Offset: 0x000C99E3
	private void OnStorageChanged(object data)
	{
		if (this.fetchList == null)
		{
			this.OnFilterChanged(this.filterable.GetTags());
		}
		this.UpdateMeter();
	}

	// Token: 0x06004350 RID: 17232 RVA: 0x000CB76E File Offset: 0x000C996E
	private void OnFunctionalChanged(object data)
	{
		this.OnFilterChanged(this.filterable.GetTags());
	}

	// Token: 0x06004351 RID: 17233 RVA: 0x00244614 File Offset: 0x00242814
	private void UpdateMeter()
	{
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float positionPercent = Mathf.Clamp01(this.GetAmountStored() / maxCapacityMinusStorageMargin);
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x06004352 RID: 17234 RVA: 0x0024464C File Offset: 0x0024284C
	public bool IsFull()
	{
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float num = Mathf.Clamp01(this.GetAmountStored() / maxCapacityMinusStorageMargin);
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(num);
		}
		return num >= 1f;
	}

	// Token: 0x06004353 RID: 17235 RVA: 0x000CB76E File Offset: 0x000C996E
	private void OnFetchComplete()
	{
		this.OnFilterChanged(this.filterable.GetTags());
	}

	// Token: 0x06004354 RID: 17236 RVA: 0x00244690 File Offset: 0x00242890
	private float GetMaxCapacity()
	{
		float num = this.storage.capacityKg;
		if (this.capacityControl != null)
		{
			num = Mathf.Min(num, this.capacityControl.UserMaxCapacity);
		}
		return num;
	}

	// Token: 0x06004355 RID: 17237 RVA: 0x000CB804 File Offset: 0x000C9A04
	private float GetMaxCapacityMinusStorageMargin()
	{
		return this.GetMaxCapacity() - this.storage.storageFullMargin;
	}

	// Token: 0x06004356 RID: 17238 RVA: 0x002446C4 File Offset: 0x002428C4
	private float GetAmountStored()
	{
		float result = this.storage.MassStored();
		if (this.capacityControl != null)
		{
			result = this.capacityControl.AmountStored;
		}
		return result;
	}

	// Token: 0x06004357 RID: 17239 RVA: 0x002446F4 File Offset: 0x002428F4
	private bool IsFunctional()
	{
		Operational component = this.storage.GetComponent<Operational>();
		return component == null || component.IsFunctional;
	}

	// Token: 0x06004358 RID: 17240 RVA: 0x00244720 File Offset: 0x00242920
	private void OnFilterChanged(HashSet<Tag> tags)
	{
		bool flag = tags != null && tags.Count != 0;
		if (this.fetchList != null)
		{
			this.fetchList.Cancel("");
			this.fetchList = null;
		}
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float amountStored = this.GetAmountStored();
		float num = Mathf.Max(0f, maxCapacityMinusStorageMargin - amountStored);
		if (num > 0f && flag && this.IsFunctional())
		{
			num = Mathf.Max(0f, this.GetMaxCapacity() - amountStored);
			this.fetchList = new FetchList2(this.storage, this.choreType);
			this.fetchList.ShowStatusItem = false;
			this.fetchList.Add(tags, this.requiredTag, this.forbiddenTags, num, Operational.State.Functional);
			this.fetchList.Submit(new System.Action(this.OnFetchComplete), false);
		}
	}

	// Token: 0x06004359 RID: 17241 RVA: 0x000CB818 File Offset: 0x000C9A18
	public void SetLogicMeter(bool on)
	{
		if (this.logicMeter != null)
		{
			this.logicMeter.SetPositionPercent(on ? 1f : 0f);
		}
	}

	// Token: 0x0600435A RID: 17242 RVA: 0x000CB83C File Offset: 0x000C9A3C
	public void SetRequiredTag(Tag tag)
	{
		if (this.requiredTag != tag)
		{
			this.requiredTag = tag;
			this.OnFilterChanged(this.filterable.GetTags());
		}
	}

	// Token: 0x0600435B RID: 17243 RVA: 0x002447F4 File Offset: 0x002429F4
	public void AddForbiddenTag(Tag forbidden_tag)
	{
		if (this.forbiddenTags == null)
		{
			this.forbiddenTags = new Tag[0];
		}
		if (!this.forbiddenTags.Contains(forbidden_tag))
		{
			this.forbiddenTags = this.forbiddenTags.Append(forbidden_tag);
			this.OnFilterChanged(this.filterable.GetTags());
		}
	}

	// Token: 0x0600435C RID: 17244 RVA: 0x00244848 File Offset: 0x00242A48
	public void RemoveForbiddenTag(Tag forbidden_tag)
	{
		if (this.forbiddenTags != null)
		{
			List<Tag> list = new List<Tag>(this.forbiddenTags);
			list.Remove(forbidden_tag);
			this.forbiddenTags = list.ToArray();
			this.OnFilterChanged(this.filterable.GetTags());
		}
	}

	// Token: 0x04002E12 RID: 11794
	public static readonly HashedString FULL_PORT_ID = "FULL";

	// Token: 0x04002E13 RID: 11795
	private KMonoBehaviour root;

	// Token: 0x04002E14 RID: 11796
	private FetchList2 fetchList;

	// Token: 0x04002E15 RID: 11797
	private IUserControlledCapacity capacityControl;

	// Token: 0x04002E16 RID: 11798
	private TreeFilterable filterable;

	// Token: 0x04002E17 RID: 11799
	private Storage storage;

	// Token: 0x04002E18 RID: 11800
	private MeterController meter;

	// Token: 0x04002E19 RID: 11801
	private MeterController logicMeter;

	// Token: 0x04002E1A RID: 11802
	private Tag requiredTag = Tag.Invalid;

	// Token: 0x04002E1B RID: 11803
	private Tag[] forbiddenTags;

	// Token: 0x04002E1C RID: 11804
	private bool hasMeter = true;

	// Token: 0x04002E1D RID: 11805
	private bool useLogicMeter;

	// Token: 0x04002E1E RID: 11806
	private ChoreType choreType;
}
