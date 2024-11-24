using System;
using UnityEngine;

// Token: 0x02002053 RID: 8275
[AddComponentMenu("KMonoBehaviour/scripts/CopyTextFieldToClipboard")]
public class CopyTextFieldToClipboard : KMonoBehaviour
{
	// Token: 0x0600B035 RID: 45109 RVA: 0x0011286E File Offset: 0x00110A6E
	protected override void OnPrefabInit()
	{
		this.button.onClick += this.OnClick;
	}

	// Token: 0x0600B036 RID: 45110 RVA: 0x00112887 File Offset: 0x00110A87
	private void OnClick()
	{
		TextEditor textEditor = new TextEditor();
		textEditor.text = this.GetText();
		textEditor.SelectAll();
		textEditor.Copy();
	}

	// Token: 0x04008AF1 RID: 35569
	public KButton button;

	// Token: 0x04008AF2 RID: 35570
	public Func<string> GetText;
}
