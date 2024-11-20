using System;
using UnityEngine;

public class PlanSubCategoryToggle : KMonoBehaviour
{
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.open = !this.open;
			this.gridContainer.SetActive(this.open);
			this.toggle.ChangeState(this.open ? 0 : 1);
		}));
	}

	[SerializeField]
	private MultiToggle toggle;

	[SerializeField]
	private GameObject gridContainer;

	private bool open = true;
}
