using UnityEngine;

public abstract class TargetPanel : KMonoBehaviour
{
	protected GameObject selectedTarget;

	public abstract bool IsValidForTarget(GameObject target);

	public virtual void SetTarget(GameObject target)
	{
		if (selectedTarget != target)
		{
			if (selectedTarget != null)
			{
				OnDeselectTarget(selectedTarget);
			}
			selectedTarget = target;
			if (selectedTarget != null)
			{
				OnSelectTarget(selectedTarget);
			}
		}
	}

	protected virtual void OnSelectTarget(GameObject target)
	{
		target.Subscribe(1502190696, OnTargetDestroyed);
	}

	public virtual void OnDeselectTarget(GameObject target)
	{
		target.Unsubscribe(1502190696, OnTargetDestroyed);
	}

	private void OnTargetDestroyed(object data)
	{
		DetailsScreen.Instance.Show(show: false);
		SetTarget(null);
	}
}
