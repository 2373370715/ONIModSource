using System;
using System.Collections.Generic;
using Database;
using UnityEngine;
using UnityEngine.UI;

public class ArtableSelectionSideScreen : SideScreenContent
{
	private Artable target;

	public KButton applyButton;

	public KButton clearButton;

	public GameObject stateButtonPrefab;

	private Dictionary<string, MultiToggle> buttons = new Dictionary<string, MultiToggle>();

	[SerializeField]
	private RectTransform scrollTransoform;

	private string selectedStage = "";

	private const int INVALID_SUBSCRIPTION = -1;

	private int workCompleteSub = -1;

	[SerializeField]
	private RectTransform buttonContainer;

	public override bool IsValidForTarget(GameObject target)
	{
		Artable component = target.GetComponent<Artable>();
		if (component == null)
		{
			return false;
		}
		if (component.CurrentStage == "Default")
		{
			return false;
		}
		return true;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		applyButton.onClick += delegate
		{
			target.SetUserChosenTargetState(selectedStage);
			SelectTool.Instance.Select(null, skipSound: true);
		};
		clearButton.onClick += delegate
		{
			selectedStage = "";
			target.SetDefault();
			SelectTool.Instance.Select(null, skipSound: true);
		};
	}

	public override void SetTarget(GameObject target)
	{
		if (workCompleteSub != -1)
		{
			target.Unsubscribe(workCompleteSub);
			workCompleteSub = -1;
		}
		base.SetTarget(target);
		this.target = target.GetComponent<Artable>();
		workCompleteSub = target.Subscribe(-2011693419, OnRefreshTarget);
		OnRefreshTarget();
	}

	public override void ClearTarget()
	{
		target.Unsubscribe(-2011693419);
		workCompleteSub = -1;
		base.ClearTarget();
	}

	private void OnRefreshTarget(object data = null)
	{
		if (!(target == null))
		{
			GenerateStateButtons();
			selectedStage = target.CurrentStage;
			RefreshButtons();
		}
	}

	public void GenerateStateButtons()
	{
		foreach (KeyValuePair<string, MultiToggle> button in buttons)
		{
			Util.KDestroyGameObject(button.Value.gameObject);
		}
		buttons.Clear();
		foreach (ArtableStage prefabStage in Db.GetArtableStages().GetPrefabStages(target.GetComponent<KPrefabID>().PrefabID()))
		{
			if (!(prefabStage.id == "Default"))
			{
				GameObject obj = Util.KInstantiateUI(stateButtonPrefab, buttonContainer.gameObject, force_active: true);
				Sprite sprite = prefabStage.GetPermitPresentationInfo().sprite;
				MultiToggle component = obj.GetComponent<MultiToggle>();
				component.GetComponent<ToolTip>().SetSimpleTooltip(prefabStage.Name);
				component.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = sprite;
				buttons.Add(prefabStage.id, component);
			}
		}
	}

	private void RefreshButtons()
	{
		List<ArtableStage> prefabStages = Db.GetArtableStages().GetPrefabStages(target.GetComponent<KPrefabID>().PrefabID());
		ArtableStage artableStage = prefabStages.Find((ArtableStage match) => match.id == target.CurrentStage);
		int num = 0;
		foreach (KeyValuePair<string, MultiToggle> kvp in buttons)
		{
			ArtableStage stage = prefabStages.Find((ArtableStage match) => match.id == kvp.Key);
			if (stage != null && artableStage != null && stage.statusItem.StatusType != artableStage.statusItem.StatusType)
			{
				kvp.Value.gameObject.SetActive(value: false);
				continue;
			}
			if (!stage.IsUnlocked())
			{
				kvp.Value.gameObject.SetActive(value: false);
				continue;
			}
			num++;
			kvp.Value.gameObject.SetActive(value: true);
			kvp.Value.ChangeState((selectedStage == kvp.Key) ? 1 : 0);
			MultiToggle value = kvp.Value;
			value.onClick = (System.Action)Delegate.Combine(value.onClick, (System.Action)delegate
			{
				selectedStage = stage.id;
				RefreshButtons();
			});
		}
		scrollTransoform.GetComponent<LayoutElement>().preferredHeight = ((num > 3) ? 200 : 100);
	}
}
