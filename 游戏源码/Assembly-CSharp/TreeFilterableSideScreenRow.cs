using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterableSideScreenRow")]
public class TreeFilterableSideScreenRow : KMonoBehaviour
{
	public enum State
	{
		Off,
		Mixed,
		On
	}

	public bool visualDirty;

	public bool standardCommodity = true;

	[SerializeField]
	private LocText elementName;

	[SerializeField]
	private GameObject elementGroup;

	[SerializeField]
	private MultiToggle checkBoxToggle;

	[SerializeField]
	private MultiToggle arrowToggle;

	[SerializeField]
	private KImage bgImg;

	private List<Tag> subTags = new List<Tag>();

	private List<TreeFilterableSideScreenElement> rowElements = new List<TreeFilterableSideScreenElement>();

	private TreeFilterableSideScreen parent;

	public bool ArrowExpanded { get; private set; }

	public TreeFilterableSideScreen Parent
	{
		get
		{
			return parent;
		}
		set
		{
			parent = value;
		}
	}

	public State GetState()
	{
		bool flag = false;
		bool flag2 = false;
		foreach (TreeFilterableSideScreenElement rowElement in rowElements)
		{
			if (parent.GetElementTagAcceptedState(rowElement.GetElementTag()))
			{
				flag = true;
			}
			else
			{
				flag2 = true;
			}
		}
		if (flag && !flag2)
		{
			return State.On;
		}
		if (!flag && flag2)
		{
			return State.Off;
		}
		if (flag && flag2)
		{
			return State.Mixed;
		}
		if (rowElements.Count <= 0)
		{
			return State.Off;
		}
		return State.On;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = checkBoxToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, (System.Action)delegate
		{
			if (parent.CurrentSearchValue == "")
			{
				switch (GetState())
				{
				case State.On:
					ChangeCheckBoxState(State.Off);
					break;
				case State.Off:
				case State.Mixed:
					ChangeCheckBoxState(State.On);
					break;
				}
			}
		});
	}

	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		SetArrowToggleState(GetState() != State.Off);
	}

	protected override void OnCmpDisable()
	{
		SetArrowToggleState(state: false);
		base.OnCmpDisable();
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	public void UpdateCheckBoxVisualState()
	{
		checkBoxToggle.ChangeState((int)GetState());
		visualDirty = false;
	}

	public void ChangeCheckBoxState(State newState)
	{
		switch (newState)
		{
		case State.Off:
		{
			for (int j = 0; j < rowElements.Count; j++)
			{
				rowElements[j].SetCheckBox(checkBoxState: false);
			}
			break;
		}
		case State.On:
		{
			for (int i = 0; i < rowElements.Count; i++)
			{
				rowElements[i].SetCheckBox(checkBoxState: true);
			}
			break;
		}
		}
		visualDirty = true;
	}

	private void ArrowToggleClicked()
	{
		SetArrowToggleState(!ArrowExpanded);
		RefreshArrowToggleState();
	}

	public void SetArrowToggleState(bool state)
	{
		ArrowExpanded = state;
		RefreshArrowToggleState();
	}

	private void RefreshArrowToggleState()
	{
		arrowToggle.ChangeState(ArrowExpanded ? 1 : 0);
		elementGroup.SetActive(ArrowExpanded);
		bgImg.enabled = ArrowExpanded;
	}

	private void ArrowToggleDisabledClick()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative"));
	}

	public void ShowToggleBox(bool show)
	{
		checkBoxToggle.gameObject.SetActive(show);
	}

	private void OnElementSelectionChanged(Tag t, bool state)
	{
		if (state)
		{
			parent.AddTag(t);
		}
		else
		{
			parent.RemoveTag(t);
		}
		visualDirty = true;
	}

	public void SetElement(Tag mainElementTag, bool state, Dictionary<Tag, bool> filterMap)
	{
		subTags.Clear();
		rowElements.Clear();
		elementName.text = mainElementTag.ProperName();
		bgImg.enabled = false;
		string simpleTooltip = string.Format(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.CATEGORYBUTTONTOOLTIP, mainElementTag.ProperName());
		checkBoxToggle.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
		if (filterMap.Count == 0)
		{
			if (elementGroup.activeInHierarchy)
			{
				elementGroup.SetActive(value: false);
			}
			arrowToggle.onClick = ArrowToggleDisabledClick;
			arrowToggle.ChangeState(0);
		}
		else
		{
			arrowToggle.onClick = ArrowToggleClicked;
			arrowToggle.ChangeState(0);
			foreach (KeyValuePair<Tag, bool> item in filterMap)
			{
				TreeFilterableSideScreenElement freeElement = parent.elementPool.GetFreeElement(elementGroup, forceActive: true);
				freeElement.Parent = parent;
				freeElement.SetTag(item.Key);
				freeElement.SetCheckBox(item.Value);
				freeElement.OnSelectionChanged = OnElementSelectionChanged;
				freeElement.SetCheckBox(parent.IsTagAllowed(item.Key));
				rowElements.Add(freeElement);
				subTags.Add(item.Key);
			}
		}
		UpdateCheckBoxVisualState();
	}

	public void RefreshRowElements()
	{
		foreach (TreeFilterableSideScreenElement rowElement in rowElements)
		{
			rowElement.SetCheckBox(parent.IsTagAllowed(rowElement.GetElementTag()));
		}
	}

	public void FilterAgainstSearch(Tag thisCategoryTag, string search)
	{
		bool flag = false;
		bool flag2 = thisCategoryTag.ProperNameStripLink().ToUpper().Contains(search.ToUpper());
		search = search.ToUpper();
		foreach (TreeFilterableSideScreenElement rowElement in rowElements)
		{
			bool flag3 = flag2 || rowElement.GetElementTag().ProperNameStripLink().ToUpper()
				.Contains(search.ToUpper());
			rowElement.gameObject.SetActive(flag3);
			flag = flag || flag3;
		}
		base.gameObject.SetActive(flag);
		if (search != "" && flag && arrowToggle.CurrentState == 0)
		{
			SetArrowToggleState(state: true);
		}
	}
}
