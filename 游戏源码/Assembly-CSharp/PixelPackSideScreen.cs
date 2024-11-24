using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FA1 RID: 8097
public class PixelPackSideScreen : SideScreenContent
{
	// Token: 0x0600AB0E RID: 43790 RVA: 0x00407E20 File Offset: 0x00406020
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.swatch_object_by_color.Count == 0)
		{
			this.InitializeColorSwatch();
		}
		this.PopulateColorSelections();
		this.copyActiveToStandbyButton.onClick += this.CopyActiveToStandby;
		this.copyStandbyToActiveButton.onClick += this.CopyStandbyToActive;
		this.swapColorsButton.onClick += this.SwapColors;
	}

	// Token: 0x0600AB0F RID: 43791 RVA: 0x0010F263 File Offset: 0x0010D463
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<PixelPack>() != null;
	}

	// Token: 0x0600AB10 RID: 43792 RVA: 0x0010F271 File Offset: 0x0010D471
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetPixelPack = target.GetComponent<PixelPack>();
		this.PopulateColorSelections();
		this.HighlightUsedColors();
	}

	// Token: 0x0600AB11 RID: 43793 RVA: 0x00407E94 File Offset: 0x00406094
	private void HighlightUsedColors()
	{
		if (this.swatch_object_by_color.Count == 0)
		{
			this.InitializeColorSwatch();
		}
		for (int i = 0; i < this.highlightedSwatchGameObjects.Count; i++)
		{
			this.highlightedSwatchGameObjects[i].GetComponent<HierarchyReferences>().GetReference("used").GetComponentInChildren<Image>().gameObject.SetActive(false);
		}
		this.highlightedSwatchGameObjects.Clear();
		for (int j = 0; j < this.targetPixelPack.colorSettings.Count; j++)
		{
			this.swatch_object_by_color[this.targetPixelPack.colorSettings[j].activeColor].GetComponent<HierarchyReferences>().GetReference("used").gameObject.SetActive(true);
			this.swatch_object_by_color[this.targetPixelPack.colorSettings[j].standbyColor].GetComponent<HierarchyReferences>().GetReference("used").gameObject.SetActive(true);
			this.highlightedSwatchGameObjects.Add(this.swatch_object_by_color[this.targetPixelPack.colorSettings[j].activeColor]);
			this.highlightedSwatchGameObjects.Add(this.swatch_object_by_color[this.targetPixelPack.colorSettings[j].standbyColor]);
		}
	}

	// Token: 0x0600AB12 RID: 43794 RVA: 0x00407FF0 File Offset: 0x004061F0
	private void PopulateColorSelections()
	{
		for (int i = 0; i < this.targetPixelPack.colorSettings.Count; i++)
		{
			int current_id = i;
			this.activeColors[i].GetComponent<Image>().color = this.targetPixelPack.colorSettings[i].activeColor;
			this.activeColors[i].GetComponent<KButton>().onClick += delegate()
			{
				PixelPack.ColorPair value = this.targetPixelPack.colorSettings[current_id];
				this.activeColors[current_id].GetComponent<Image>().color = this.paintingColor;
				value.activeColor = this.paintingColor;
				this.targetPixelPack.colorSettings[current_id] = value;
				this.targetPixelPack.UpdateColors();
				this.HighlightUsedColors();
			};
			this.standbyColors[i].GetComponent<Image>().color = this.targetPixelPack.colorSettings[i].standbyColor;
			this.standbyColors[i].GetComponent<KButton>().onClick += delegate()
			{
				PixelPack.ColorPair value = this.targetPixelPack.colorSettings[current_id];
				this.standbyColors[current_id].GetComponent<Image>().color = this.paintingColor;
				value.standbyColor = this.paintingColor;
				this.targetPixelPack.colorSettings[current_id] = value;
				this.targetPixelPack.UpdateColors();
				this.HighlightUsedColors();
			};
		}
	}

	// Token: 0x0600AB13 RID: 43795 RVA: 0x004080D0 File Offset: 0x004062D0
	private void InitializeColorSwatch()
	{
		bool flag = false;
		for (int i = 0; i < this.colorSwatch.Count; i++)
		{
			GameObject swatch = Util.KInstantiateUI(this.swatchEntry, this.colorSwatchContainer, true);
			Image component = swatch.GetComponent<Image>();
			component.color = this.colorSwatch[i];
			KButton component2 = swatch.GetComponent<KButton>();
			Color color = this.colorSwatch[i];
			if (component.color == Color.black)
			{
				swatch.GetComponent<HierarchyReferences>().GetReference("selected").GetComponentInChildren<Image>().color = Color.white;
			}
			if (!flag)
			{
				this.SelectColor(color, swatch);
				flag = true;
			}
			component2.onClick += delegate()
			{
				this.SelectColor(color, swatch);
			};
			this.swatch_object_by_color[color] = swatch;
		}
	}

	// Token: 0x0600AB14 RID: 43796 RVA: 0x004081D0 File Offset: 0x004063D0
	private void SelectColor(Color color, GameObject swatchEntry)
	{
		this.paintingColor = color;
		swatchEntry.GetComponent<HierarchyReferences>().GetReference("selected").gameObject.SetActive(true);
		if (this.selectedSwatchEntry != null && this.selectedSwatchEntry != swatchEntry)
		{
			this.selectedSwatchEntry.GetComponent<HierarchyReferences>().GetReference("selected").gameObject.SetActive(false);
		}
		this.selectedSwatchEntry = swatchEntry;
	}

	// Token: 0x0600AB15 RID: 43797 RVA: 0x00408244 File Offset: 0x00406444
	private void CopyActiveToStandby()
	{
		for (int i = 0; i < this.targetPixelPack.colorSettings.Count; i++)
		{
			PixelPack.ColorPair colorPair = this.targetPixelPack.colorSettings[i];
			colorPair.standbyColor = colorPair.activeColor;
			this.targetPixelPack.colorSettings[i] = colorPair;
			this.standbyColors[i].GetComponent<Image>().color = colorPair.activeColor;
		}
		this.HighlightUsedColors();
		this.targetPixelPack.UpdateColors();
	}

	// Token: 0x0600AB16 RID: 43798 RVA: 0x004082CC File Offset: 0x004064CC
	private void CopyStandbyToActive()
	{
		for (int i = 0; i < this.targetPixelPack.colorSettings.Count; i++)
		{
			PixelPack.ColorPair colorPair = this.targetPixelPack.colorSettings[i];
			colorPair.activeColor = colorPair.standbyColor;
			this.targetPixelPack.colorSettings[i] = colorPair;
			this.activeColors[i].GetComponent<Image>().color = colorPair.standbyColor;
		}
		this.HighlightUsedColors();
		this.targetPixelPack.UpdateColors();
	}

	// Token: 0x0600AB17 RID: 43799 RVA: 0x00408354 File Offset: 0x00406554
	private void SwapColors()
	{
		for (int i = 0; i < this.targetPixelPack.colorSettings.Count; i++)
		{
			PixelPack.ColorPair colorPair = default(PixelPack.ColorPair);
			colorPair.activeColor = this.targetPixelPack.colorSettings[i].standbyColor;
			colorPair.standbyColor = this.targetPixelPack.colorSettings[i].activeColor;
			this.targetPixelPack.colorSettings[i] = colorPair;
			this.activeColors[i].GetComponent<Image>().color = colorPair.activeColor;
			this.standbyColors[i].GetComponent<Image>().color = colorPair.standbyColor;
		}
		this.HighlightUsedColors();
		this.targetPixelPack.UpdateColors();
	}

	// Token: 0x0400866A RID: 34410
	public PixelPack targetPixelPack;

	// Token: 0x0400866B RID: 34411
	public KButton copyActiveToStandbyButton;

	// Token: 0x0400866C RID: 34412
	public KButton copyStandbyToActiveButton;

	// Token: 0x0400866D RID: 34413
	public KButton swapColorsButton;

	// Token: 0x0400866E RID: 34414
	public GameObject colorSwatchContainer;

	// Token: 0x0400866F RID: 34415
	public GameObject swatchEntry;

	// Token: 0x04008670 RID: 34416
	public GameObject activeColorsContainer;

	// Token: 0x04008671 RID: 34417
	public GameObject standbyColorsContainer;

	// Token: 0x04008672 RID: 34418
	public List<GameObject> activeColors = new List<GameObject>();

	// Token: 0x04008673 RID: 34419
	public List<GameObject> standbyColors = new List<GameObject>();

	// Token: 0x04008674 RID: 34420
	public Color paintingColor;

	// Token: 0x04008675 RID: 34421
	public GameObject selectedSwatchEntry;

	// Token: 0x04008676 RID: 34422
	private Dictionary<Color, GameObject> swatch_object_by_color = new Dictionary<Color, GameObject>();

	// Token: 0x04008677 RID: 34423
	private List<GameObject> highlightedSwatchGameObjects = new List<GameObject>();

	// Token: 0x04008678 RID: 34424
	private List<Color> colorSwatch = new List<Color>
	{
		new Color(0.4862745f, 0.4862745f, 0.4862745f),
		new Color(0f, 0f, 0.9882353f),
		new Color(0f, 0f, 0.7372549f),
		new Color(0.26666668f, 0.15686275f, 0.7372549f),
		new Color(0.5803922f, 0f, 0.5176471f),
		new Color(0.65882355f, 0f, 0.1254902f),
		new Color(0.65882355f, 0.0627451f, 0f),
		new Color(0.53333336f, 0.078431375f, 0f),
		new Color(0.3137255f, 0.1882353f, 0f),
		new Color(0f, 0.47058824f, 0f),
		new Color(0f, 0.40784314f, 0f),
		new Color(0f, 0.34509805f, 0f),
		new Color(0f, 0.2509804f, 0.34509805f),
		new Color(0f, 0f, 0f),
		new Color(0.7372549f, 0.7372549f, 0.7372549f),
		new Color(0f, 0.47058824f, 0.972549f),
		new Color(0f, 0.34509805f, 0.972549f),
		new Color(0.40784314f, 0.26666668f, 0.9882353f),
		new Color(0.84705883f, 0f, 0.8f),
		new Color(0.89411765f, 0f, 0.34509805f),
		new Color(0.972549f, 0.21960784f, 0f),
		new Color(0.89411765f, 0.36078432f, 0.0627451f),
		new Color(0.6745098f, 0.4862745f, 0f),
		new Color(0f, 0.72156864f, 0f),
		new Color(0f, 0.65882355f, 0f),
		new Color(0f, 0.65882355f, 0.26666668f),
		new Color(0f, 0.53333336f, 0.53333336f),
		new Color(0f, 0f, 0f),
		new Color(0.972549f, 0.972549f, 0.972549f),
		new Color(0.23529412f, 0.7372549f, 0.9882353f),
		new Color(0.40784314f, 0.53333336f, 0.9882353f),
		new Color(0.59607846f, 0.47058824f, 0.972549f),
		new Color(0.972549f, 0.47058824f, 0.972549f),
		new Color(0.972549f, 0.34509805f, 0.59607846f),
		new Color(0.972549f, 0.47058824f, 0.34509805f),
		new Color(0.9882353f, 0.627451f, 0.26666668f),
		new Color(0.972549f, 0.72156864f, 0f),
		new Color(0.72156864f, 0.972549f, 0.09411765f),
		new Color(0.34509805f, 0.84705883f, 0.32941177f),
		new Color(0.34509805f, 0.972549f, 0.59607846f),
		new Color(0f, 0.9098039f, 0.84705883f),
		new Color(0.47058824f, 0.47058824f, 0.47058824f),
		new Color(0.9882353f, 0.9882353f, 0.9882353f),
		new Color(0.6431373f, 0.89411765f, 0.9882353f),
		new Color(0.72156864f, 0.72156864f, 0.972549f),
		new Color(0.84705883f, 0.72156864f, 0.972549f),
		new Color(0.972549f, 0.72156864f, 0.972549f),
		new Color(0.972549f, 0.72156864f, 0.7529412f),
		new Color(0.9411765f, 0.8156863f, 0.6901961f),
		new Color(0.9882353f, 0.8784314f, 0.65882355f),
		new Color(0.972549f, 0.84705883f, 0.47058824f),
		new Color(0.84705883f, 0.972549f, 0.47058824f),
		new Color(0.72156864f, 0.972549f, 0.72156864f),
		new Color(0.72156864f, 0.972549f, 0.84705883f),
		new Color(0f, 0.9882353f, 0.9882353f),
		new Color(0.84705883f, 0.84705883f, 0.84705883f)
	};
}
