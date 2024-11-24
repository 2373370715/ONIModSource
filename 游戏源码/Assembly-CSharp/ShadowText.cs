using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;

// Token: 0x02001F1A RID: 7962
public class ShadowText : ShadowRect
{
	// Token: 0x0600A7DE RID: 42974 RVA: 0x003FAAEC File Offset: 0x003F8CEC
	protected override void MatchRect()
	{
		if (this.RectMain == null || this.RectShadow == null)
		{
			return;
		}
		if (this.shadowText == null)
		{
			this.shadowText = this.RectShadow.GetComponent<Text>();
		}
		if (this.mainText == null)
		{
			this.mainText = this.RectMain.GetComponent<Text>();
		}
		if (this.shadowText == null || this.mainText == null)
		{
			return;
		}
		if (this.shadowText.font != this.mainText.font)
		{
			this.shadowText.font = this.mainText.font;
		}
		if (this.shadowText.fontSize != this.mainText.fontSize)
		{
			this.shadowText.fontSize = this.mainText.fontSize;
		}
		if (this.shadowText.alignment != this.mainText.alignment)
		{
			this.shadowText.alignment = this.mainText.alignment;
		}
		if (this.shadowText.lineSpacing != this.mainText.lineSpacing)
		{
			this.shadowText.lineSpacing = this.mainText.lineSpacing;
		}
		string text = this.mainText.text;
		text = Regex.Replace(text, "\\</?color\\b.*?\\>", string.Empty);
		if (this.shadowText.text != text)
		{
			this.shadowText.text = text;
		}
		if (this.shadowText.color != this.shadowColor)
		{
			this.shadowText.color = this.shadowColor;
		}
		if (this.shadowText.horizontalOverflow != this.mainText.horizontalOverflow)
		{
			this.shadowText.horizontalOverflow = this.mainText.horizontalOverflow;
		}
		if (this.shadowText.verticalOverflow != this.mainText.verticalOverflow)
		{
			this.shadowText.verticalOverflow = this.mainText.verticalOverflow;
		}
		base.MatchRect();
	}

	// Token: 0x04008400 RID: 33792
	private Text shadowText;

	// Token: 0x04008401 RID: 33793
	private Text mainText;
}
