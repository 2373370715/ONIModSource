using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelatedEntitiesSideScreen : SideScreenContent, ISim1000ms
{
	private GameObject target;

	private IRelatedEntities targetRelatedEntitiesComponent;

	public GameObject rowPrefab;

	public RectTransform rowContainer;

	public Dictionary<KSelectable, GameObject> rows = new Dictionary<KSelectable, GameObject>();

	private int uiRefreshSubHandle = -1;

	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		rowPrefab.SetActive(value: false);
		if (show)
		{
			RefreshOptions();
		}
	}

	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IRelatedEntities>() != null;
	}

	public override void SetTarget(GameObject target)
	{
		this.target = target;
		targetRelatedEntitiesComponent = target.GetComponent<IRelatedEntities>();
		RefreshOptions();
		uiRefreshSubHandle = Game.Instance.Subscribe(1980521255, RefreshOptions);
	}

	public override void ClearTarget()
	{
		if (uiRefreshSubHandle != -1 && targetRelatedEntitiesComponent != null)
		{
			Game.Instance.Unsubscribe(uiRefreshSubHandle);
			uiRefreshSubHandle = -1;
		}
	}

	private void RefreshOptions(object data = null)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		ClearRows();
		foreach (KSelectable relatedEntity in targetRelatedEntitiesComponent.GetRelatedEntities())
		{
			AddRow(relatedEntity);
		}
	}

	private void ClearRows()
	{
		for (int num = rowContainer.childCount - 1; num >= 0; num--)
		{
			Util.KDestroyGameObject(rowContainer.GetChild(num));
		}
		rows.Clear();
	}

	private void AddRow(KSelectable entity)
	{
		GameObject gameObject = Util.KInstantiateUI(rowPrefab, rowContainer.gameObject, force_active: true);
		gameObject.GetComponent<KButton>().onClick += delegate
		{
			SelectTool.Instance.SelectAndFocus(entity.transform.position, entity);
		};
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("label").SetText((SelectTool.Instance.selected == entity) ? ("<b>" + entity.GetProperName() + "</b>") : entity.GetProperName());
		component.GetReference<Image>("icon").sprite = Def.GetUISprite(entity.gameObject).first;
		rows.Add(entity, gameObject);
		RefreshMainStatus(entity);
	}

	private void RefreshMainStatus(KSelectable entity)
	{
		if (!entity.IsNullOrDestroyed() && rows.ContainsKey(entity))
		{
			HierarchyReferences component = rows[entity].GetComponent<HierarchyReferences>();
			StatusItemGroup.Entry statusItem = entity.GetStatusItem(Db.Get().StatusItemCategories.Main);
			LocText reference = component.GetReference<LocText>("status");
			if (statusItem.data != null)
			{
				reference.gameObject.SetActive(value: true);
				reference.SetText(statusItem.item.GetName(statusItem.data));
			}
			else
			{
				reference.gameObject.SetActive(value: false);
				reference.SetText("");
			}
		}
	}

	public void Sim1000ms(float dt)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		foreach (KeyValuePair<KSelectable, GameObject> row in rows)
		{
			RefreshMainStatus(row.Key);
		}
	}
}
