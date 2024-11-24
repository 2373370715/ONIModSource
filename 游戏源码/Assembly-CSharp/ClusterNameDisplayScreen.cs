using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001C22 RID: 7202
public class ClusterNameDisplayScreen : KScreen
{
	// Token: 0x060095D1 RID: 38353 RVA: 0x00101965 File Offset: 0x000FFB65
	public static void DestroyInstance()
	{
		ClusterNameDisplayScreen.Instance = null;
	}

	// Token: 0x060095D2 RID: 38354 RVA: 0x0010196D File Offset: 0x000FFB6D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ClusterNameDisplayScreen.Instance = this;
	}

	// Token: 0x060095D3 RID: 38355 RVA: 0x0010197B File Offset: 0x000FFB7B
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060095D4 RID: 38356 RVA: 0x0039E590 File Offset: 0x0039C790
	public void AddNewEntry(ClusterGridEntity representedObject)
	{
		if (this.GetEntry(representedObject) != null)
		{
			return;
		}
		ClusterNameDisplayScreen.Entry entry = new ClusterNameDisplayScreen.Entry();
		entry.grid_entity = representedObject;
		GameObject gameObject = Util.KInstantiateUI(this.nameAndBarsPrefab, base.gameObject, true);
		entry.display_go = gameObject;
		gameObject.name = representedObject.name + " cluster overlay";
		entry.Name = representedObject.name;
		entry.refs = gameObject.GetComponent<HierarchyReferences>();
		entry.bars_go = entry.refs.GetReference<RectTransform>("Bars").gameObject;
		this.m_entries.Add(entry);
		if (representedObject.GetComponent<KSelectable>() != null)
		{
			this.UpdateName(representedObject);
			this.UpdateBars(representedObject);
		}
	}

	// Token: 0x060095D5 RID: 38357 RVA: 0x0039E640 File Offset: 0x0039C840
	private void LateUpdate()
	{
		if (App.isLoading || App.IsExiting)
		{
			return;
		}
		int num = this.m_entries.Count;
		int i = 0;
		while (i < num)
		{
			if (this.m_entries[i].grid_entity != null && ClusterMapScreen.GetRevealLevel(this.m_entries[i].grid_entity) == ClusterRevealLevel.Visible)
			{
				Transform gridEntityNameTarget = ClusterMapScreen.Instance.GetGridEntityNameTarget(this.m_entries[i].grid_entity);
				if (gridEntityNameTarget != null)
				{
					Vector3 position = gridEntityNameTarget.GetPosition();
					this.m_entries[i].display_go.GetComponent<RectTransform>().SetPositionAndRotation(position, Quaternion.identity);
					this.m_entries[i].display_go.SetActive(this.m_entries[i].grid_entity.IsVisible && this.m_entries[i].grid_entity.ShowName());
				}
				else if (this.m_entries[i].display_go.activeSelf)
				{
					this.m_entries[i].display_go.SetActive(false);
				}
				this.UpdateBars(this.m_entries[i].grid_entity);
				if (this.m_entries[i].bars_go != null)
				{
					this.m_entries[i].bars_go.GetComponentsInChildren<KCollider2D>(false, this.workingList);
					foreach (KCollider2D kcollider2D in this.workingList)
					{
						kcollider2D.MarkDirty(false);
					}
				}
				i++;
			}
			else
			{
				UnityEngine.Object.Destroy(this.m_entries[i].display_go);
				num--;
				this.m_entries[i] = this.m_entries[num];
			}
		}
		this.m_entries.RemoveRange(num, this.m_entries.Count - num);
	}

	// Token: 0x060095D6 RID: 38358 RVA: 0x0039E858 File Offset: 0x0039CA58
	public void UpdateName(ClusterGridEntity representedObject)
	{
		ClusterNameDisplayScreen.Entry entry = this.GetEntry(representedObject);
		if (entry == null)
		{
			return;
		}
		KSelectable component = representedObject.GetComponent<KSelectable>();
		entry.display_go.name = component.GetProperName() + " cluster overlay";
		LocText componentInChildren = entry.display_go.GetComponentInChildren<LocText>();
		if (componentInChildren != null)
		{
			componentInChildren.text = component.GetProperName();
		}
	}

	// Token: 0x060095D7 RID: 38359 RVA: 0x0039E8B4 File Offset: 0x0039CAB4
	private void UpdateBars(ClusterGridEntity representedObject)
	{
		ClusterNameDisplayScreen.Entry entry = this.GetEntry(representedObject);
		if (entry == null)
		{
			return;
		}
		GenericUIProgressBar componentInChildren = entry.bars_go.GetComponentInChildren<GenericUIProgressBar>(true);
		if (entry.grid_entity.ShowProgressBar())
		{
			if (!componentInChildren.gameObject.activeSelf)
			{
				componentInChildren.gameObject.SetActive(true);
			}
			componentInChildren.SetFillPercentage(entry.grid_entity.GetProgress());
			return;
		}
		if (componentInChildren.gameObject.activeSelf)
		{
			componentInChildren.gameObject.SetActive(false);
		}
	}

	// Token: 0x060095D8 RID: 38360 RVA: 0x0039E92C File Offset: 0x0039CB2C
	private ClusterNameDisplayScreen.Entry GetEntry(ClusterGridEntity entity)
	{
		return this.m_entries.Find((ClusterNameDisplayScreen.Entry entry) => entry.grid_entity == entity);
	}

	// Token: 0x04007469 RID: 29801
	public static ClusterNameDisplayScreen Instance;

	// Token: 0x0400746A RID: 29802
	public GameObject nameAndBarsPrefab;

	// Token: 0x0400746B RID: 29803
	[SerializeField]
	private Color selectedColor;

	// Token: 0x0400746C RID: 29804
	[SerializeField]
	private Color defaultColor;

	// Token: 0x0400746D RID: 29805
	private List<ClusterNameDisplayScreen.Entry> m_entries = new List<ClusterNameDisplayScreen.Entry>();

	// Token: 0x0400746E RID: 29806
	private List<KCollider2D> workingList = new List<KCollider2D>();

	// Token: 0x02001C23 RID: 7203
	private class Entry
	{
		// Token: 0x0400746F RID: 29807
		public string Name;

		// Token: 0x04007470 RID: 29808
		public ClusterGridEntity grid_entity;

		// Token: 0x04007471 RID: 29809
		public GameObject display_go;

		// Token: 0x04007472 RID: 29810
		public GameObject bars_go;

		// Token: 0x04007473 RID: 29811
		public HierarchyReferences refs;
	}
}
