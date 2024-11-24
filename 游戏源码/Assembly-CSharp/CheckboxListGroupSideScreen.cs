using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckboxListGroupSideScreen : SideScreenContent
{
	public class CheckboxContainer
	{
		public HierarchyReferences container;

		public List<HierarchyReferences> checkboxUIItems = new List<HierarchyReferences>();

		public CheckboxContainer(HierarchyReferences container)
		{
			this.container = container;
		}
	}

	public const int DefaultCheckboxListSideScreenSortOrder = 20;

	private ObjectPool<CheckboxContainer> checkboxContainerPool;

	private GameObjectPool checkboxPool;

	[SerializeField]
	private GameObject checkboxGroupPrefab;

	[SerializeField]
	private GameObject checkboxPrefab;

	[SerializeField]
	private RectTransform groupParent;

	[SerializeField]
	private RectTransform checkboxParent;

	[SerializeField]
	private LocText descriptionLabel;

	private List<ICheckboxListGroupControl> targets;

	private GameObject currentBuildTarget;

	private int uiRefreshSubHandle = -1;

	private List<CheckboxContainer> activeChecklistGroups = new List<CheckboxContainer>();

	private CheckboxContainer InstantiateCheckboxContainer()
	{
		return new CheckboxContainer(Util.KInstantiateUI(checkboxGroupPrefab, groupParent.gameObject, force_active: true).GetComponent<HierarchyReferences>());
	}

	private GameObject InstantiateCheckbox()
	{
		return Util.KInstantiateUI(checkboxPrefab, checkboxParent.gameObject);
	}

	protected override void OnSpawn()
	{
		checkboxPrefab.SetActive(value: false);
		checkboxGroupPrefab.SetActive(value: false);
		base.OnSpawn();
	}

	public override bool IsValidForTarget(GameObject target)
	{
		ICheckboxListGroupControl[] components = target.GetComponents<ICheckboxListGroupControl>();
		if (components != null)
		{
			ICheckboxListGroupControl[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].SidescreenEnabled())
				{
					return true;
				}
			}
		}
		foreach (ICheckboxListGroupControl item in target.GetAllSMI<ICheckboxListGroupControl>())
		{
			if (item.SidescreenEnabled())
			{
				return true;
			}
		}
		return false;
	}

	public override int GetSideScreenSortOrder()
	{
		if (targets == null)
		{
			return 20;
		}
		return targets[0].CheckboxSideScreenSortOrder();
	}

	public override void SetTarget(GameObject target)
	{
		if (target == null)
		{
			Debug.LogError("Invalid gameObject received");
			return;
		}
		targets = target.GetAllSMI<ICheckboxListGroupControl>();
		targets.AddRange(target.GetComponents<ICheckboxListGroupControl>());
		Rebuild(target);
		uiRefreshSubHandle = currentBuildTarget.Subscribe(1980521255, Refresh);
	}

	public override void ClearTarget()
	{
		if (uiRefreshSubHandle != -1 && currentBuildTarget != null)
		{
			currentBuildTarget.Unsubscribe(uiRefreshSubHandle);
			uiRefreshSubHandle = -1;
		}
		ReleaseContainers(activeChecklistGroups.Count);
	}

	public override string GetTitle()
	{
		if (targets != null && targets.Count > 0 && targets[0] != null)
		{
			return targets[0].Title;
		}
		return base.GetTitle();
	}

	private void Rebuild(GameObject buildTarget)
	{
		if (checkboxContainerPool == null)
		{
			checkboxContainerPool = new ObjectPool<CheckboxContainer>(InstantiateCheckboxContainer);
			checkboxPool = new GameObjectPool(InstantiateCheckbox);
		}
		descriptionLabel.enabled = !targets[0].Description.IsNullOrWhiteSpace();
		if (!targets[0].Description.IsNullOrWhiteSpace())
		{
			descriptionLabel.SetText(targets[0].Description);
		}
		if (buildTarget == currentBuildTarget)
		{
			Refresh();
			return;
		}
		currentBuildTarget = buildTarget;
		foreach (ICheckboxListGroupControl target in targets)
		{
			ICheckboxListGroupControl.ListGroup[] data = target.GetData();
			foreach (ICheckboxListGroupControl.ListGroup group in data)
			{
				CheckboxContainer instance = checkboxContainerPool.GetInstance();
				InitContainer(target, group, instance);
			}
		}
	}

	[ContextMenu("Force refresh")]
	private void Test()
	{
		Refresh();
	}

	private void Refresh(object data = null)
	{
		int num = 0;
		foreach (ICheckboxListGroupControl target in targets)
		{
			ICheckboxListGroupControl.ListGroup[] data2 = target.GetData();
			for (int i = 0; i < data2.Length; i++)
			{
				ICheckboxListGroupControl.ListGroup group = data2[i];
				if (++num > activeChecklistGroups.Count)
				{
					InitContainer(target, group, checkboxContainerPool.GetInstance());
				}
				CheckboxContainer checkboxContainer = activeChecklistGroups[num - 1];
				if (group.resolveTitleCallback != null)
				{
					checkboxContainer.container.GetReference<LocText>("Text").SetText(group.resolveTitleCallback(group.title));
				}
				for (int j = 0; j < group.checkboxItems.Length; j++)
				{
					ICheckboxListGroupControl.CheckboxItem data3 = group.checkboxItems[j];
					if (checkboxContainer.checkboxUIItems.Count <= j)
					{
						CreateSingleCheckBoxForGroupUI(checkboxContainer);
					}
					HierarchyReferences checkboxUI = checkboxContainer.checkboxUIItems[j];
					SetCheckboxData(checkboxUI, data3, target);
				}
				while (checkboxContainer.checkboxUIItems.Count > group.checkboxItems.Length)
				{
					HierarchyReferences checkbox = checkboxContainer.checkboxUIItems[checkboxContainer.checkboxUIItems.Count - 1];
					RemoveSingleCheckboxFromContainer(checkbox, checkboxContainer);
				}
			}
		}
		ReleaseContainers(activeChecklistGroups.Count - num);
	}

	private void ReleaseContainers(int count)
	{
		int count2 = activeChecklistGroups.Count;
		for (int i = 1; i <= count; i++)
		{
			int index = count2 - i;
			CheckboxContainer checkboxContainer = activeChecklistGroups[index];
			activeChecklistGroups.RemoveAt(index);
			for (int num = checkboxContainer.checkboxUIItems.Count - 1; num >= 0; num--)
			{
				HierarchyReferences checkbox = checkboxContainer.checkboxUIItems[num];
				RemoveSingleCheckboxFromContainer(checkbox, checkboxContainer);
			}
			checkboxContainer.container.gameObject.SetActive(value: false);
			checkboxContainerPool.ReleaseInstance(checkboxContainer);
		}
	}

	private void InitContainer(ICheckboxListGroupControl target, ICheckboxListGroupControl.ListGroup group, CheckboxContainer groupUI)
	{
		activeChecklistGroups.Add(groupUI);
		groupUI.container.gameObject.SetActive(value: true);
		string text = group.title;
		if (group.resolveTitleCallback != null)
		{
			text = group.resolveTitleCallback(text);
		}
		groupUI.container.GetReference<LocText>("Text").SetText(text);
		ICheckboxListGroupControl.CheckboxItem[] checkboxItems = group.checkboxItems;
		foreach (ICheckboxListGroupControl.CheckboxItem data in checkboxItems)
		{
			CreateSingleCheckBoxForGroupUI(data, target, groupUI);
		}
	}

	public void RemoveSingleCheckboxFromContainer(HierarchyReferences checkbox, CheckboxContainer container)
	{
		container.checkboxUIItems.Remove(checkbox);
		checkbox.gameObject.SetActive(value: false);
		checkbox.transform.SetParent(checkboxParent);
		checkboxPool.ReleaseInstance(checkbox.gameObject);
	}

	public HierarchyReferences CreateSingleCheckBoxForGroupUI(CheckboxContainer container)
	{
		HierarchyReferences component = checkboxPool.GetInstance().GetComponent<HierarchyReferences>();
		component.gameObject.SetActive(value: true);
		container.checkboxUIItems.Add(component);
		component.transform.SetParent(container.container.transform);
		return component;
	}

	public HierarchyReferences CreateSingleCheckBoxForGroupUI(ICheckboxListGroupControl.CheckboxItem data, ICheckboxListGroupControl target, CheckboxContainer container)
	{
		HierarchyReferences hierarchyReferences = CreateSingleCheckBoxForGroupUI(container);
		SetCheckboxData(hierarchyReferences, data, target);
		return hierarchyReferences;
	}

	public void SetCheckboxData(HierarchyReferences checkboxUI, ICheckboxListGroupControl.CheckboxItem data, ICheckboxListGroupControl target)
	{
		LocText reference = checkboxUI.GetReference<LocText>("Text");
		reference.SetText(data.text);
		reference.SetLinkOverrideAction(data.overrideLinkActions);
		checkboxUI.GetReference<Image>("Check").enabled = data.isOn;
		ToolTip reference2 = checkboxUI.GetReference<ToolTip>("Tooltip");
		reference2.SetSimpleTooltip(data.tooltip);
		reference2.refreshWhileHovering = data.resolveTooltipCallback != null;
		reference2.OnToolTip = () => (data.resolveTooltipCallback == null) ? data.tooltip : data.resolveTooltipCallback(data.tooltip, target);
	}
}
