using System;
using UnityEngine;

// Token: 0x02002043 RID: 8259
public class URLOpenFunction : MonoBehaviour
{
	// Token: 0x0600AFD1 RID: 45009 RVA: 0x0011242A File Offset: 0x0011062A
	private void Start()
	{
		if (this.triggerButton != null)
		{
			this.triggerButton.ClearOnClick();
			this.triggerButton.onClick += delegate()
			{
				this.OpenUrl(this.fixedURL);
			};
		}
	}

	// Token: 0x0600AFD2 RID: 45010 RVA: 0x0011245C File Offset: 0x0011065C
	public void OpenUrl(string url)
	{
		if (url == "blueprints")
		{
			if (LockerMenuScreen.Instance != null)
			{
				LockerMenuScreen.Instance.ShowInventoryScreen();
				return;
			}
		}
		else
		{
			App.OpenWebURL(url);
		}
	}

	// Token: 0x0600AFD3 RID: 45011 RVA: 0x00112489 File Offset: 0x00110689
	public void SetURL(string url)
	{
		this.fixedURL = url;
	}

	// Token: 0x04008A8B RID: 35467
	[SerializeField]
	private KButton triggerButton;

	// Token: 0x04008A8C RID: 35468
	[SerializeField]
	private string fixedURL;
}
