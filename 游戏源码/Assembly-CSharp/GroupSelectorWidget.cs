using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200205C RID: 8284
public class GroupSelectorWidget : MonoBehaviour
{
	// Token: 0x0600B052 RID: 45138 RVA: 0x00112A03 File Offset: 0x00110C03
	public void Initialize(object widget_id, IList<GroupSelectorWidget.ItemData> options, GroupSelectorWidget.ItemCallbacks item_callbacks)
	{
		this.widgetID = widget_id;
		this.options = options;
		this.itemCallbacks = item_callbacks;
		this.addItemButton.onClick += this.OnAddItemClicked;
	}

	// Token: 0x0600B053 RID: 45139 RVA: 0x00423EEC File Offset: 0x004220EC
	public void Reconfigure(IList<int> selected_option_indices)
	{
		this.selectedOptionIndices.Clear();
		this.selectedOptionIndices.AddRange(selected_option_indices);
		this.selectedOptionIndices.Sort();
		this.addItemButton.isInteractable = (this.selectedOptionIndices.Count < this.options.Count);
		this.RebuildSelectedVisualizers();
	}

	// Token: 0x0600B054 RID: 45140 RVA: 0x00423F44 File Offset: 0x00422144
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

	// Token: 0x0600B055 RID: 45141 RVA: 0x00112A31 File Offset: 0x00110C31
	private void OnItemAdded(int option_idx)
	{
		if (this.itemCallbacks.onItemAdded != null)
		{
			this.itemCallbacks.onItemAdded(this.widgetID, this.options[option_idx].userData);
			this.RebuildSubPanelOptions();
		}
	}

	// Token: 0x0600B056 RID: 45142 RVA: 0x00112A6E File Offset: 0x00110C6E
	private void OnItemRemoved(int option_idx)
	{
		if (this.itemCallbacks.onItemRemoved != null)
		{
			this.itemCallbacks.onItemRemoved(this.widgetID, this.options[option_idx].userData);
		}
	}

	// Token: 0x0600B057 RID: 45143 RVA: 0x00423FB0 File Offset: 0x004221B0
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

	// Token: 0x0600B058 RID: 45144 RVA: 0x00424074 File Offset: 0x00422274
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

	// Token: 0x0600B059 RID: 45145 RVA: 0x00112AA4 File Offset: 0x00110CA4
	public bool IsSubPanelOpen()
	{
		return this.unselectedItemsPanel.gameObject.activeSelf;
	}

	// Token: 0x0600B05A RID: 45146 RVA: 0x00112AB6 File Offset: 0x00110CB6
	public void CloseSubPanel()
	{
		this.ClearSubPanelOptions();
		this.unselectedItemsPanel.gameObject.SetActive(false);
	}

	// Token: 0x0600B05B RID: 45147 RVA: 0x00424148 File Offset: 0x00422348
	private void ClearSubPanelOptions()
	{
		foreach (object obj in this.unselectedItemsPanel.transform)
		{
			Util.KDestroyGameObject(((Transform)obj).gameObject);
		}
	}

	// Token: 0x0600B05C RID: 45148 RVA: 0x004241A8 File Offset: 0x004223A8
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

	// Token: 0x04008B1A RID: 35610
	[SerializeField]
	private GameObject itemTemplate;

	// Token: 0x04008B1B RID: 35611
	[SerializeField]
	private RectTransform selectedItemsPanel;

	// Token: 0x04008B1C RID: 35612
	[SerializeField]
	private RectTransform unselectedItemsPanel;

	// Token: 0x04008B1D RID: 35613
	[SerializeField]
	private KButton addItemButton;

	// Token: 0x04008B1E RID: 35614
	[SerializeField]
	private int numExpectedPanelColumns = 3;

	// Token: 0x04008B1F RID: 35615
	private object widgetID;

	// Token: 0x04008B20 RID: 35616
	private GroupSelectorWidget.ItemCallbacks itemCallbacks;

	// Token: 0x04008B21 RID: 35617
	private IList<GroupSelectorWidget.ItemData> options;

	// Token: 0x04008B22 RID: 35618
	private List<int> selectedOptionIndices = new List<int>();

	// Token: 0x04008B23 RID: 35619
	private List<GameObject> selectedVisualizers = new List<GameObject>();

	// Token: 0x0200205D RID: 8285
	[Serializable]
	public struct ItemData
	{
		// Token: 0x0600B05E RID: 45150 RVA: 0x00112AF4 File Offset: 0x00110CF4
		public ItemData(Sprite sprite, object user_data)
		{
			this.sprite = sprite;
			this.userData = user_data;
		}

		// Token: 0x04008B24 RID: 35620
		public Sprite sprite;

		// Token: 0x04008B25 RID: 35621
		public object userData;
	}

	// Token: 0x0200205E RID: 8286
	public struct ItemCallbacks
	{
		// Token: 0x04008B26 RID: 35622
		public Func<object, IList<int>> getSubPanelDisplayIndices;

		// Token: 0x04008B27 RID: 35623
		public Action<object, object> onItemAdded;

		// Token: 0x04008B28 RID: 35624
		public Action<object, object> onItemRemoved;

		// Token: 0x04008B29 RID: 35625
		public Func<object, object, bool, string> getItemHoverText;
	}
}
