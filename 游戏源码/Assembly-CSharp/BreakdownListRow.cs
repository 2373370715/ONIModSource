using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002050 RID: 8272
[AddComponentMenu("KMonoBehaviour/scripts/BreakdownListRow")]
public class BreakdownListRow : KMonoBehaviour
{
	// Token: 0x0600B022 RID: 45090 RVA: 0x004233E4 File Offset: 0x004215E4
	public void ShowData(string name, string value)
	{
		base.gameObject.transform.localScale = Vector3.one;
		this.nameLabel.text = name;
		this.valueLabel.text = value;
		this.dotOutlineImage.gameObject.SetActive(true);
		Vector2 vector = Vector2.one * 0.6f;
		this.dotOutlineImage.rectTransform.localScale.Set(vector.x, vector.y, 1f);
		this.dotInsideImage.gameObject.SetActive(true);
		this.dotInsideImage.color = BreakdownListRow.statusColour[0];
		this.iconImage.gameObject.SetActive(false);
		this.checkmarkImage.gameObject.SetActive(false);
		this.SetHighlighted(false);
		this.SetImportant(false);
	}

	// Token: 0x0600B023 RID: 45091 RVA: 0x004234C0 File Offset: 0x004216C0
	public void ShowStatusData(string name, string value, BreakdownListRow.Status dotColor)
	{
		this.ShowData(name, value);
		this.dotOutlineImage.gameObject.SetActive(true);
		this.dotInsideImage.gameObject.SetActive(true);
		this.iconImage.gameObject.SetActive(false);
		this.checkmarkImage.gameObject.SetActive(false);
		this.SetStatusColor(dotColor);
	}

	// Token: 0x0600B024 RID: 45092 RVA: 0x00423520 File Offset: 0x00421720
	public void SetStatusColor(BreakdownListRow.Status dotColor)
	{
		this.checkmarkImage.gameObject.SetActive(dotColor > BreakdownListRow.Status.Default);
		this.checkmarkImage.color = BreakdownListRow.statusColour[(int)dotColor];
		switch (dotColor)
		{
		case BreakdownListRow.Status.Red:
			this.checkmarkImage.sprite = this.statusFailureIcon;
			return;
		case BreakdownListRow.Status.Green:
			this.checkmarkImage.sprite = this.statusSuccessIcon;
			return;
		case BreakdownListRow.Status.Yellow:
			this.checkmarkImage.sprite = this.statusWarningIcon;
			return;
		default:
			return;
		}
	}

	// Token: 0x0600B025 RID: 45093 RVA: 0x004235A4 File Offset: 0x004217A4
	public void ShowCheckmarkData(string name, string value, BreakdownListRow.Status status)
	{
		this.ShowData(name, value);
		this.dotOutlineImage.gameObject.SetActive(true);
		this.dotOutlineImage.rectTransform.localScale = Vector3.one;
		this.dotInsideImage.gameObject.SetActive(true);
		this.iconImage.gameObject.SetActive(false);
		this.SetStatusColor(status);
	}

	// Token: 0x0600B026 RID: 45094 RVA: 0x00423608 File Offset: 0x00421808
	public void ShowIconData(string name, string value, Sprite sprite)
	{
		this.ShowData(name, value);
		this.dotOutlineImage.gameObject.SetActive(false);
		this.dotInsideImage.gameObject.SetActive(false);
		this.iconImage.gameObject.SetActive(true);
		this.checkmarkImage.gameObject.SetActive(false);
		this.iconImage.sprite = sprite;
		this.iconImage.color = Color.white;
	}

	// Token: 0x0600B027 RID: 45095 RVA: 0x001127C4 File Offset: 0x001109C4
	public void ShowIconData(string name, string value, Sprite sprite, Color spriteColor)
	{
		this.ShowIconData(name, value, sprite);
		this.iconImage.color = spriteColor;
	}

	// Token: 0x0600B028 RID: 45096 RVA: 0x00423680 File Offset: 0x00421880
	public void SetHighlighted(bool highlighted)
	{
		this.isHighlighted = highlighted;
		Vector2 vector = Vector2.one * 0.8f;
		this.dotOutlineImage.rectTransform.localScale.Set(vector.x, vector.y, 1f);
		this.nameLabel.alpha = (this.isHighlighted ? 0.9f : 0.5f);
		this.valueLabel.alpha = (this.isHighlighted ? 0.9f : 0.5f);
	}

	// Token: 0x0600B029 RID: 45097 RVA: 0x0042370C File Offset: 0x0042190C
	public void SetDisabled(bool disabled)
	{
		this.isDisabled = disabled;
		this.nameLabel.alpha = (this.isDisabled ? 0.4f : 0.5f);
		this.valueLabel.alpha = (this.isDisabled ? 0.4f : 0.5f);
	}

	// Token: 0x0600B02A RID: 45098 RVA: 0x00423760 File Offset: 0x00421960
	public void SetImportant(bool important)
	{
		this.isImportant = important;
		this.dotOutlineImage.rectTransform.localScale = Vector3.one;
		this.nameLabel.alpha = (this.isImportant ? 1f : 0.5f);
		this.valueLabel.alpha = (this.isImportant ? 1f : 0.5f);
		this.nameLabel.fontStyle = (this.isImportant ? FontStyles.Bold : FontStyles.Normal);
		this.valueLabel.fontStyle = (this.isImportant ? FontStyles.Bold : FontStyles.Normal);
	}

	// Token: 0x0600B02B RID: 45099 RVA: 0x004237F8 File Offset: 0x004219F8
	public void HideIcon()
	{
		this.dotOutlineImage.gameObject.SetActive(false);
		this.dotInsideImage.gameObject.SetActive(false);
		this.iconImage.gameObject.SetActive(false);
		this.checkmarkImage.gameObject.SetActive(false);
	}

	// Token: 0x0600B02C RID: 45100 RVA: 0x001127DC File Offset: 0x001109DC
	public void AddTooltip(string tooltipText)
	{
		if (this.tooltip == null)
		{
			this.tooltip = base.gameObject.AddComponent<ToolTip>();
		}
		this.tooltip.SetSimpleTooltip(tooltipText);
	}

	// Token: 0x0600B02D RID: 45101 RVA: 0x00112809 File Offset: 0x00110A09
	public void ClearTooltip()
	{
		if (this.tooltip != null)
		{
			this.tooltip.ClearMultiStringTooltip();
		}
	}

	// Token: 0x0600B02E RID: 45102 RVA: 0x00112824 File Offset: 0x00110A24
	public void SetValue(string value)
	{
		this.valueLabel.text = value;
	}

	// Token: 0x04008AD9 RID: 35545
	private static Color[] statusColour = new Color[]
	{
		new Color(0.34117648f, 0.36862746f, 0.45882353f, 1f),
		new Color(0.72156864f, 0.38431373f, 0f, 1f),
		new Color(0.38431373f, 0.72156864f, 0f, 1f),
		new Color(0.72156864f, 0.72156864f, 0f, 1f)
	};

	// Token: 0x04008ADA RID: 35546
	public Image dotOutlineImage;

	// Token: 0x04008ADB RID: 35547
	public Image dotInsideImage;

	// Token: 0x04008ADC RID: 35548
	public Image iconImage;

	// Token: 0x04008ADD RID: 35549
	public Image checkmarkImage;

	// Token: 0x04008ADE RID: 35550
	public LocText nameLabel;

	// Token: 0x04008ADF RID: 35551
	public LocText valueLabel;

	// Token: 0x04008AE0 RID: 35552
	private bool isHighlighted;

	// Token: 0x04008AE1 RID: 35553
	private bool isDisabled;

	// Token: 0x04008AE2 RID: 35554
	private bool isImportant;

	// Token: 0x04008AE3 RID: 35555
	private ToolTip tooltip;

	// Token: 0x04008AE4 RID: 35556
	[SerializeField]
	private Sprite statusSuccessIcon;

	// Token: 0x04008AE5 RID: 35557
	[SerializeField]
	private Sprite statusWarningIcon;

	// Token: 0x04008AE6 RID: 35558
	[SerializeField]
	private Sprite statusFailureIcon;

	// Token: 0x02002051 RID: 8273
	public enum Status
	{
		// Token: 0x04008AE8 RID: 35560
		Default,
		// Token: 0x04008AE9 RID: 35561
		Red,
		// Token: 0x04008AEA RID: 35562
		Green,
		// Token: 0x04008AEB RID: 35563
		Yellow
	}
}
