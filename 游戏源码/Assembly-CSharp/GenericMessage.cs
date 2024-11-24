using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001DF6 RID: 7670
public class GenericMessage : Message
{
	// Token: 0x0600A075 RID: 41077 RVA: 0x001084E7 File Offset: 0x001066E7
	public GenericMessage(string _title, string _body, string _tooltip, KMonoBehaviour click_focus = null)
	{
		this.title = _title;
		this.body = _body;
		this.tooltip = _tooltip;
		this.clickFocus.Set(click_focus);
	}

	// Token: 0x0600A076 RID: 41078 RVA: 0x0010851C File Offset: 0x0010671C
	public GenericMessage()
	{
	}

	// Token: 0x0600A077 RID: 41079 RVA: 0x000AD332 File Offset: 0x000AB532
	public override string GetSound()
	{
		return null;
	}

	// Token: 0x0600A078 RID: 41080 RVA: 0x0010852F File Offset: 0x0010672F
	public override string GetMessageBody()
	{
		return this.body;
	}

	// Token: 0x0600A079 RID: 41081 RVA: 0x00108537 File Offset: 0x00106737
	public override string GetTooltip()
	{
		return this.tooltip;
	}

	// Token: 0x0600A07A RID: 41082 RVA: 0x0010853F File Offset: 0x0010673F
	public override string GetTitle()
	{
		return this.title;
	}

	// Token: 0x0600A07B RID: 41083 RVA: 0x003D5AD4 File Offset: 0x003D3CD4
	public override void OnClick()
	{
		KMonoBehaviour kmonoBehaviour = this.clickFocus.Get();
		if (kmonoBehaviour == null)
		{
			return;
		}
		Transform transform = kmonoBehaviour.transform;
		if (transform == null)
		{
			return;
		}
		Vector3 position = transform.GetPosition();
		position.z = -40f;
		CameraController.Instance.SetTargetPos(position, 8f, true);
		if (transform.GetComponent<KSelectable>() != null)
		{
			SelectTool.Instance.Select(transform.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x04007D76 RID: 32118
	[Serialize]
	private string title;

	// Token: 0x04007D77 RID: 32119
	[Serialize]
	private string tooltip;

	// Token: 0x04007D78 RID: 32120
	[Serialize]
	private string body;

	// Token: 0x04007D79 RID: 32121
	[Serialize]
	private Ref<KMonoBehaviour> clickFocus = new Ref<KMonoBehaviour>();
}
