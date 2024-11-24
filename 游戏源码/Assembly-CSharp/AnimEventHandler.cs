using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/AnimEventHandler")]
public class AnimEventHandler : KMonoBehaviour
{
	private delegate void SetPos(Vector3 pos);

	[MyCmpGet]
	private KBatchedAnimController controller;

	[MyCmpGet]
	private KBoxCollider2D animCollider;

	[MyCmpGet]
	private Navigator navigator;

	private Pickupable pickupable;

	private Vector3 targetPos;

	public Transform cachedTransform;

	public Vector2 baseOffset;

	private HashedString context;

	private event SetPos onWorkTargetSet;

	public int GetCachedCell()
	{
		return pickupable.cachedCell;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		cachedTransform = base.transform;
		pickupable = GetComponent<Pickupable>();
		KBatchedAnimTracker[] componentsInChildren = GetComponentsInChildren<KBatchedAnimTracker>(includeInactive: true);
		foreach (KBatchedAnimTracker kBatchedAnimTracker in componentsInChildren)
		{
			if (kBatchedAnimTracker.useTargetPoint)
			{
				onWorkTargetSet += kBatchedAnimTracker.SetTarget;
			}
		}
		baseOffset = animCollider.offset;
		AnimEventHandlerManager.Instance.Add(this);
	}

	protected override void OnCleanUp()
	{
		AnimEventHandlerManager.Instance.Remove(this);
	}

	protected override void OnForcedCleanUp()
	{
		navigator = null;
		base.OnForcedCleanUp();
	}

	public HashedString GetContext()
	{
		return context;
	}

	public void UpdateWorkTarget(Vector3 pos)
	{
		if (this.onWorkTargetSet != null)
		{
			this.onWorkTargetSet(pos);
		}
	}

	public void SetContext(HashedString context)
	{
		this.context = context;
	}

	public void SetTargetPos(Vector3 target_pos)
	{
		targetPos = target_pos;
	}

	public Vector3 GetTargetPos()
	{
		return targetPos;
	}

	public void ClearContext()
	{
		context = default(HashedString);
	}

	public void UpdateOffset()
	{
		Vector3 pivotSymbolPosition = controller.GetPivotSymbolPosition();
		Vector3 vector = navigator.NavGrid.GetNavTypeData(navigator.CurrentNavType).animControllerOffset;
		Vector3 position = cachedTransform.position;
		Vector2 vector2 = new Vector2(baseOffset.x + pivotSymbolPosition.x - position.x - vector.x, baseOffset.y + pivotSymbolPosition.y - position.y + vector.y);
		if (animCollider.offset != vector2)
		{
			animCollider.offset = vector2;
		}
	}
}
