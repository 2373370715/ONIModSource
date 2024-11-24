using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001D31 RID: 7473
public class KPopupMenu : KScreen
{
	// Token: 0x06009C03 RID: 39939 RVA: 0x003C1D34 File Offset: 0x003BFF34
	public void SetOptions(IList<string> options)
	{
		List<KButtonMenu.ButtonInfo> list = new List<KButtonMenu.ButtonInfo>();
		for (int i = 0; i < options.Count; i++)
		{
			int index = i;
			string option = options[i];
			list.Add(new KButtonMenu.ButtonInfo(option, global::Action.NumActions, delegate()
			{
				this.SelectOption(option, index);
			}, null, null));
		}
		this.Buttons = list.ToArray();
	}

	// Token: 0x06009C04 RID: 39940 RVA: 0x003C1DAC File Offset: 0x003BFFAC
	public void OnClick()
	{
		if (this.Buttons != null)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.buttonMenu.SetButtons(this.Buttons);
			this.buttonMenu.RefreshButtons();
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06009C05 RID: 39941 RVA: 0x00105789 File Offset: 0x00103989
	public void SelectOption(string option, int index)
	{
		if (this.OnSelect != null)
		{
			this.OnSelect(option, index);
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06009C06 RID: 39942 RVA: 0x001057AC File Offset: 0x001039AC
	public IList<KButtonMenu.ButtonInfo> GetButtons()
	{
		return this.Buttons;
	}

	// Token: 0x04007A41 RID: 31297
	[SerializeField]
	private KButtonMenu buttonMenu;

	// Token: 0x04007A42 RID: 31298
	private KButtonMenu.ButtonInfo[] Buttons;

	// Token: 0x04007A43 RID: 31299
	public Action<string, int> OnSelect;
}
