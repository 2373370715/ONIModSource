using System;
using UnityEngine;

// Token: 0x02001452 RID: 5202
[AddComponentMenu("KMonoBehaviour/scripts/SelectMarker")]
public class SelectMarker : KMonoBehaviour
{
	// Token: 0x06006BF9 RID: 27641 RVA: 0x000E6EF8 File Offset: 0x000E50F8
	public void SetTargetTransform(Transform target_transform)
	{
		this.targetTransform = target_transform;
		this.LateUpdate();
	}

	// Token: 0x06006BFA RID: 27642 RVA: 0x002E4C28 File Offset: 0x002E2E28
	private void LateUpdate()
	{
		if (this.targetTransform == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		Vector3 position = this.targetTransform.GetPosition();
		KCollider2D component = this.targetTransform.GetComponent<KCollider2D>();
		if (component != null)
		{
			position.x = component.bounds.center.x;
			position.y = component.bounds.center.y + component.bounds.size.y / 2f + 0.1f;
		}
		else
		{
			position.y += 2f;
		}
		Vector3 b = new Vector3(0f, (Mathf.Sin(Time.unscaledTime * 4f) + 1f) * this.animationOffset, 0f);
		base.transform.SetPosition(position + b);
	}

	// Token: 0x04005101 RID: 20737
	public float animationOffset = 0.1f;

	// Token: 0x04005102 RID: 20738
	private Transform targetTransform;
}
