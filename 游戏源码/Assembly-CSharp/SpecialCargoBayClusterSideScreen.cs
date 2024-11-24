using UnityEngine;
using UnityEngine.UI;

public class SpecialCargoBayClusterSideScreen : ReceptacleSideScreen
{
	public LayoutElement descriptionContent;

	public float descriptionLayoutDefaultSize = -1f;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<SpecialCargoBayClusterReceptacle>() != null;
	}

	protected override bool RequiresAvailableAmountToDeposit()
	{
		return false;
	}

	protected override void UpdateState(object data)
	{
		base.UpdateState(data);
		SetDescriptionSidescreenFoldState(targetReceptacle != null && targetReceptacle.Occupant == null);
	}

	protected override void SetResultDescriptions(GameObject go)
	{
		base.SetResultDescriptions(go);
		if (targetReceptacle != null && targetReceptacle.Occupant != null)
		{
			descriptionLabel.SetText("");
			SetDescriptionSidescreenFoldState(visible: false);
		}
		else
		{
			SetDescriptionSidescreenFoldState(visible: true);
		}
	}

	public void SetDescriptionSidescreenFoldState(bool visible)
	{
		descriptionContent.minHeight = (visible ? descriptionLayoutDefaultSize : 0f);
	}
}
