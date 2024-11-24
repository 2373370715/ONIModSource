using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D71 RID: 7537
public class KScrollbarVisibility : MonoBehaviour
{
	// Token: 0x06009D7B RID: 40315 RVA: 0x00106703 File Offset: 0x00104903
	private void Start()
	{
		this.Update();
	}

	// Token: 0x06009D7C RID: 40316 RVA: 0x003C7710 File Offset: 0x003C5910
	private void Update()
	{
		if (this.content.content == null)
		{
			return;
		}
		bool flag = false;
		Vector2 vector = new Vector2(this.parent.rect.width, this.parent.rect.height);
		Vector2 sizeDelta = this.content.content.GetComponent<RectTransform>().sizeDelta;
		if ((sizeDelta.x >= vector.x && this.checkWidth) || (sizeDelta.y >= vector.y && this.checkHeight))
		{
			flag = true;
		}
		if (this.scrollbar.gameObject.activeSelf != flag)
		{
			this.scrollbar.gameObject.SetActive(flag);
			if (this.others != null)
			{
				GameObject[] array = this.others;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(flag);
				}
			}
		}
	}

	// Token: 0x04007B52 RID: 31570
	[SerializeField]
	private ScrollRect content;

	// Token: 0x04007B53 RID: 31571
	[SerializeField]
	private RectTransform parent;

	// Token: 0x04007B54 RID: 31572
	[SerializeField]
	private bool checkWidth = true;

	// Token: 0x04007B55 RID: 31573
	[SerializeField]
	private bool checkHeight = true;

	// Token: 0x04007B56 RID: 31574
	[SerializeField]
	private Scrollbar scrollbar;

	// Token: 0x04007B57 RID: 31575
	[SerializeField]
	private GameObject[] others;
}
