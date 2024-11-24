using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FB4 RID: 8116
public class RelatedEntitiesSideScreen : SideScreenContent, ISim1000ms
{
	// Token: 0x0600AB98 RID: 43928 RVA: 0x0010F709 File Offset: 0x0010D909
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.rowPrefab.SetActive(false);
		if (show)
		{
			this.RefreshOptions(null);
		}
	}

	// Token: 0x0600AB99 RID: 43929 RVA: 0x0010F728 File Offset: 0x0010D928
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IRelatedEntities>() != null;
	}

	// Token: 0x0600AB9A RID: 43930 RVA: 0x0010F733 File Offset: 0x0010D933
	public override void SetTarget(GameObject target)
	{
		this.target = target;
		this.targetRelatedEntitiesComponent = target.GetComponent<IRelatedEntities>();
		this.RefreshOptions(null);
		this.uiRefreshSubHandle = Game.Instance.Subscribe(1980521255, new Action<object>(this.RefreshOptions));
	}

	// Token: 0x0600AB9B RID: 43931 RVA: 0x0010F770 File Offset: 0x0010D970
	public override void ClearTarget()
	{
		if (this.uiRefreshSubHandle != -1 && this.targetRelatedEntitiesComponent != null)
		{
			Game.Instance.Unsubscribe(this.uiRefreshSubHandle);
			this.uiRefreshSubHandle = -1;
		}
	}

	// Token: 0x0600AB9C RID: 43932 RVA: 0x0040AF9C File Offset: 0x0040919C
	private void RefreshOptions(object data = null)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.ClearRows();
		foreach (KSelectable entity in this.targetRelatedEntitiesComponent.GetRelatedEntities())
		{
			this.AddRow(entity);
		}
	}

	// Token: 0x0600AB9D RID: 43933 RVA: 0x0040B008 File Offset: 0x00409208
	private void ClearRows()
	{
		for (int i = this.rowContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.rowContainer.GetChild(i));
		}
		this.rows.Clear();
	}

	// Token: 0x0600AB9E RID: 43934 RVA: 0x0040B04C File Offset: 0x0040924C
	private void AddRow(KSelectable entity)
	{
		GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
		gameObject.GetComponent<KButton>().onClick += delegate()
		{
			SelectTool.Instance.SelectAndFocus(entity.transform.position, entity);
		};
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("label").SetText((SelectTool.Instance.selected == entity) ? ("<b>" + entity.GetProperName() + "</b>") : entity.GetProperName());
		component.GetReference<Image>("icon").sprite = Def.GetUISprite(entity.gameObject, "ui", false).first;
		this.rows.Add(entity, gameObject);
		this.RefreshMainStatus(entity);
	}

	// Token: 0x0600AB9F RID: 43935 RVA: 0x0040B134 File Offset: 0x00409334
	private void RefreshMainStatus(KSelectable entity)
	{
		if (entity.IsNullOrDestroyed())
		{
			return;
		}
		if (!this.rows.ContainsKey(entity))
		{
			return;
		}
		HierarchyReferences component = this.rows[entity].GetComponent<HierarchyReferences>();
		StatusItemGroup.Entry statusItem = entity.GetStatusItem(Db.Get().StatusItemCategories.Main);
		LocText reference = component.GetReference<LocText>("status");
		if (statusItem.data != null)
		{
			reference.gameObject.SetActive(true);
			reference.SetText(statusItem.item.GetName(statusItem.data));
			return;
		}
		reference.gameObject.SetActive(false);
		reference.SetText("");
	}

	// Token: 0x0600ABA0 RID: 43936 RVA: 0x0040B1D0 File Offset: 0x004093D0
	public void Sim1000ms(float dt)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		foreach (KeyValuePair<KSelectable, GameObject> keyValuePair in this.rows)
		{
			this.RefreshMainStatus(keyValuePair.Key);
		}
	}

	// Token: 0x040086E5 RID: 34533
	private GameObject target;

	// Token: 0x040086E6 RID: 34534
	private IRelatedEntities targetRelatedEntitiesComponent;

	// Token: 0x040086E7 RID: 34535
	public GameObject rowPrefab;

	// Token: 0x040086E8 RID: 34536
	public RectTransform rowContainer;

	// Token: 0x040086E9 RID: 34537
	public Dictionary<KSelectable, GameObject> rows = new Dictionary<KSelectable, GameObject>();

	// Token: 0x040086EA RID: 34538
	private int uiRefreshSubHandle = -1;
}
