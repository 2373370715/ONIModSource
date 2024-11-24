﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupSelectorWidget : MonoBehaviour
{
	public void Initialize(object widget_id, IList<GroupSelectorWidget.ItemData> options, GroupSelectorWidget.ItemCallbacks item_callbacks)
	{
		this.widgetID = widget_id;
		this.options = options;
		this.itemCallbacks = item_callbacks;
		this.addItemButton.onClick += this.OnAddItemClicked;
	}

	public void Reconfigure(IList<int> selected_option_indices)
	{
		this.selectedOptionIndices.Clear();
		this.selectedOptionIndices.AddRange(selected_option_indices);
		this.selectedOptionIndices.Sort();
		this.addItemButton.isInteractable = (this.selectedOptionIndices.Count < this.options.Count);
		this.RebuildSelectedVisualizers();
	}

	private void OnAddItemClicked()
	{
		if (!this.IsSubPanelOpen())
		{
			if (this.RebuildSubPanelOptions() > 0)
			{
				this.unselectedItemsPanel.GetComponent<GridLayoutGroup>().constraintCount = Mathf.Min(this.numExpectedPanelColumns, this.unselectedItemsPanel.childCount);
				this.unselectedItemsPanel.gameObject.SetActive(true);
				this.unselectedItemsPanel.GetComponent<Selectable>().Select();
				return;
			}
		}
		else
		{
			this.CloseSubPanel();
		}
	}

	private void OnItemAdded(int option_idx)
	{
		if (this.itemCallbacks.onItemAdded != null)
		{
			this.itemCallbacks.onItemAdded(this.widgetID, this.options[option_idx].userData);
			this.RebuildSubPanelOptions();
		}
	}

	private void OnItemRemoved(int option_idx)
	{
		if (this.itemCallbacks.onItemRemoved != null)
		{
			this.itemCallbacks.onItemRemoved(this.widgetID, this.options[option_idx].userData);
		}
	}

	private void RebuildSelectedVisualizers()
	{
		foreach (GameObject original in this.selectedVisualizers)
		{
			Util.KDestroyGameObject(original);
		}
		this.selectedVisualizers.Clear();
		foreach (int idx in this.selectedOptionIndices)
		{
			GameObject item = this.CreateItem(idx, new Action<int>(this.OnItemRemoved), this.selectedItemsPanel.gameObject, true);
			this.selectedVisualizers.Add(item);
		}
	}

	private GameObject CreateItem(int idx, Action<int> on_click, GameObject parent, bool is_selected_item)
	{
		GameObject gameObject = Util.KInstantiateUI(this.itemTemplate, parent, true);
		KButton component = gameObject.GetComponent<KButton>();
		component.onClick += delegate()
		{
			on_click(idx);
		};
		component.fgImage.sprite = this.options[idx].sprite;
		if (parent == this.selectedItemsPanel.gameObject)
		{
			HierarchyReferences component2 = component.GetComponent<HierarchyReferences>();
			if (component2 != null)
			{
				Component reference = component2.GetReference("CancelImg");
				if (reference != null)
				{
					reference.gameObject.SetActive(true);
				}
			}
		}
		gameObject.GetComponent<ToolTip>().OnToolTip = (() => this.itemCallbacks.getItemHoverText(this.widgetID, this.options[idx].userData, is_selected_item));
		return gameObject;
	}

	public bool IsSubPanelOpen()
	{
		return this.unselectedItemsPanel.gameObject.activeSelf;
	}

	public void CloseSubPanel()
	{
		this.ClearSubPanelOptions();
		this.unselectedItemsPanel.gameObject.SetActive(false);
	}

	private void ClearSubPanelOptions()
	{
		foreach (object obj in this.unselectedItemsPanel.transform)
		{
			Util.KDestroyGameObject(((Transform)obj).gameObject);
		}
	}

	private int RebuildSubPanelOptions()
	{
		IList<int> list = this.itemCallbacks.getSubPanelDisplayIndices(this.widgetID);
		if (list.Count > 0)
		{
			this.ClearSubPanelOptions();
			using (IEnumerator<int> enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int num = enumerator.Current;
					if (!this.selectedOptionIndices.Contains(num))
					{
						this.CreateItem(num, new Action<int>(this.OnItemAdded), this.unselectedItemsPanel.gameObject, false);
					}
				}
				goto IL_7E;
			}
		}
		this.CloseSubPanel();
		IL_7E:
		return list.Count;
	}

	[SerializeField]
	private GameObject itemTemplate;

	[SerializeField]
	private RectTransform selectedItemsPanel;

	[SerializeField]
	private RectTransform unselectedItemsPanel;

	[SerializeField]
	private KButton addItemButton;

	[SerializeField]
	private int numExpectedPanelColumns = 3;

	private object widgetID;

	private GroupSelectorWidget.ItemCallbacks itemCallbacks;

	private IList<GroupSelectorWidget.ItemData> options;

	private List<int> selectedOptionIndices = new List<int>();

	private List<GameObject> selectedVisualizers = new List<GameObject>();

	[Serializable]
	public struct ItemData
	{
		public ItemData(Sprite sprite, object user_data)
		{
			this.sprite = sprite;
			this.userData = user_data;
		}

		public Sprite sprite;

		public object userData;
	}

	public struct ItemCallbacks
	{
		public Func<object, IList<int>> getSubPanelDisplayIndices;

		public Action<object, object> onItemAdded;

		public Action<object, object> onItemRemoved;

		public Func<object, object, bool, string> getItemHoverText;
	}
}
