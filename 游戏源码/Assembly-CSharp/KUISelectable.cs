using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001D37 RID: 7479
[AddComponentMenu("KMonoBehaviour/scripts/KUISelectable")]
public class KUISelectable : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06009C1B RID: 39963 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x06009C1C RID: 39964 RVA: 0x0010585E File Offset: 0x00103A5E
	protected override void OnSpawn()
	{
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x06009C1D RID: 39965 RVA: 0x0010587C File Offset: 0x00103A7C
	public void SetTarget(GameObject target)
	{
		this.target = target;
	}

	// Token: 0x06009C1E RID: 39966 RVA: 0x00105885 File Offset: 0x00103A85
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.target != null)
		{
			SelectTool.Instance.SetHoverOverride(this.target.GetComponent<KSelectable>());
		}
	}

	// Token: 0x06009C1F RID: 39967 RVA: 0x001058AA File Offset: 0x00103AAA
	public void OnPointerExit(PointerEventData eventData)
	{
		SelectTool.Instance.SetHoverOverride(null);
	}

	// Token: 0x06009C20 RID: 39968 RVA: 0x001058B7 File Offset: 0x00103AB7
	private void OnClick()
	{
		if (this.target != null)
		{
			SelectTool.Instance.Select(this.target.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x06009C21 RID: 39969 RVA: 0x001058DD File Offset: 0x00103ADD
	protected override void OnCmpDisable()
	{
		if (SelectTool.Instance != null)
		{
			SelectTool.Instance.SetHoverOverride(null);
		}
	}

	// Token: 0x04007A54 RID: 31316
	private GameObject target;
}
