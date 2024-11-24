using UnityEngine;

public abstract class DetailScreenTab : TargetPanel
{
	public abstract override bool IsValidForTarget(GameObject target);

	protected override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
	}

	protected CollapsibleDetailContentPanel CreateCollapsableSection(string title = null)
	{
		CollapsibleDetailContentPanel collapsibleDetailContentPanel = Util.KInstantiateUI<CollapsibleDetailContentPanel>(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject);
		if (!string.IsNullOrEmpty(title))
		{
			collapsibleDetailContentPanel.SetTitle(title);
		}
		return collapsibleDetailContentPanel;
	}

	private void Update()
	{
		Refresh();
	}

	protected virtual void Refresh(bool force = false)
	{
	}
}
