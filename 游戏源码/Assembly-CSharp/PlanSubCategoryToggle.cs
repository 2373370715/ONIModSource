using System;
using UnityEngine;

public class PlanSubCategoryToggle : KMonoBehaviour
{
	[SerializeField]
	private MultiToggle toggle;

	[SerializeField]
	private GameObject gridContainer;

	private bool open = true;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		MultiToggle multiToggle = toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, (System.Action)delegate
		{
			open = !open;
			gridContainer.SetActive(open);
			toggle.ChangeState((!open) ? 1 : 0);
		});
	}
}
