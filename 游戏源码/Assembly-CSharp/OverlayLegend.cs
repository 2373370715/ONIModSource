using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E8D RID: 7821
public class OverlayLegend : KScreen
{
	// Token: 0x0600A3FF RID: 41983 RVA: 0x003E42F8 File Offset: 0x003E24F8
	[ContextMenu("Set all fonts color")]
	public void SetAllFontsColor()
	{
		foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
		{
			for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
			{
				if (overlayInfo.infoUnits[i].fontColor == Color.clear)
				{
					overlayInfo.infoUnits[i].fontColor = Color.white;
				}
			}
		}
	}

	// Token: 0x0600A400 RID: 41984 RVA: 0x003E4390 File Offset: 0x003E2590
	[ContextMenu("Set all tooltips")]
	public void SetAllTooltips()
	{
		foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
		{
			string text = overlayInfo.name;
			text = text.Replace("NAME", "");
			for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
			{
				string text2 = overlayInfo.infoUnits[i].description;
				text2 = text2.Replace(text, "");
				text2 = text + "TOOLTIPS." + text2;
				overlayInfo.infoUnits[i].tooltip = text2;
			}
		}
	}

	// Token: 0x0600A401 RID: 41985 RVA: 0x003E4454 File Offset: 0x003E2654
	[ContextMenu("Set Sliced for empty icons")]
	public void SetSlicedForEmptyIcons()
	{
		foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
		{
			for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
			{
				if (overlayInfo.infoUnits[i].icon == this.emptySprite)
				{
					overlayInfo.infoUnits[i].sliceIcon = true;
				}
			}
		}
	}

	// Token: 0x0600A402 RID: 41986 RVA: 0x003E44E8 File Offset: 0x003E26E8
	protected override void OnSpawn()
	{
		base.ConsumeMouseScroll = true;
		base.OnSpawn();
		if (OverlayLegend.Instance == null)
		{
			OverlayLegend.Instance = this;
			this.activeUnitObjs = new List<GameObject>();
			this.inactiveUnitObjs = new List<GameObject>();
			foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
			{
				overlayInfo.name = Strings.Get(overlayInfo.name);
				for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
				{
					overlayInfo.infoUnits[i].description = Strings.Get(overlayInfo.infoUnits[i].description);
					if (!string.IsNullOrEmpty(overlayInfo.infoUnits[i].tooltip))
					{
						overlayInfo.infoUnits[i].tooltip = Strings.Get(overlayInfo.infoUnits[i].tooltip);
					}
				}
			}
			base.GetComponent<LayoutElement>().minWidth = (float)(DlcManager.FeatureClusterSpaceEnabled() ? 322 : 288);
			this.ClearLegend();
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600A403 RID: 41987 RVA: 0x0010A608 File Offset: 0x00108808
	protected override void OnLoadLevel()
	{
		OverlayLegend.Instance = null;
		this.activeDiagrams.Clear();
		UnityEngine.Object.Destroy(base.gameObject);
		base.OnLoadLevel();
	}

	// Token: 0x0600A404 RID: 41988 RVA: 0x003E4644 File Offset: 0x003E2844
	private void SetLegend(OverlayLegend.OverlayInfo overlayInfo)
	{
		if (overlayInfo == null)
		{
			this.ClearLegend();
			return;
		}
		if (!overlayInfo.isProgrammaticallyPopulated && (overlayInfo.infoUnits == null || overlayInfo.infoUnits.Count == 0))
		{
			this.ClearLegend();
			return;
		}
		this.Show(true);
		this.title.text = overlayInfo.name;
		if (overlayInfo.isProgrammaticallyPopulated)
		{
			this.PopulateGeneratedLegend(overlayInfo, false);
		}
		else
		{
			this.PopulateOverlayInfoUnits(overlayInfo, false);
			this.PopulateOverlayDiagrams(overlayInfo, false);
		}
		this.ConfigureUIHeight();
	}

	// Token: 0x0600A405 RID: 41989 RVA: 0x003E46C0 File Offset: 0x003E28C0
	public void SetLegend(OverlayModes.Mode mode, bool refreshing = false)
	{
		if (this.currentMode != null && this.currentMode.ViewMode() == mode.ViewMode() && !refreshing)
		{
			return;
		}
		this.ClearLegend();
		OverlayLegend.OverlayInfo legend = this.overlayInfoList.Find((OverlayLegend.OverlayInfo ol) => ol.mode == mode.ViewMode());
		this.currentMode = mode;
		this.SetLegend(legend);
	}

	// Token: 0x0600A406 RID: 41990 RVA: 0x003E4734 File Offset: 0x003E2934
	public GameObject GetFreeUnitObject()
	{
		if (this.inactiveUnitObjs.Count == 0)
		{
			this.inactiveUnitObjs.Add(Util.KInstantiateUI(this.unitPrefab, this.inactiveUnitsParent, false));
		}
		GameObject gameObject = this.inactiveUnitObjs[0];
		this.inactiveUnitObjs.RemoveAt(0);
		this.activeUnitObjs.Add(gameObject);
		return gameObject;
	}

	// Token: 0x0600A407 RID: 41991 RVA: 0x003E4794 File Offset: 0x003E2994
	private void RemoveActiveObjects()
	{
		while (this.activeUnitObjs.Count > 0)
		{
			this.activeUnitObjs[0].transform.Find("Icon").GetComponent<Image>().enabled = false;
			this.activeUnitObjs[0].GetComponentInChildren<LocText>().enabled = false;
			this.activeUnitObjs[0].transform.SetParent(this.inactiveUnitsParent.transform);
			this.activeUnitObjs[0].SetActive(false);
			this.inactiveUnitObjs.Add(this.activeUnitObjs[0]);
			this.activeUnitObjs.RemoveAt(0);
		}
	}

	// Token: 0x0600A408 RID: 41992 RVA: 0x0010A62C File Offset: 0x0010882C
	public void ClearLegend()
	{
		this.RemoveActiveObjects();
		this.ClearFilters();
		this.ClearDiagrams();
		this.Show(false);
	}

	// Token: 0x0600A409 RID: 41993 RVA: 0x0010A647 File Offset: 0x00108847
	public void ClearFilters()
	{
		if (this.filterMenu != null)
		{
			UnityEngine.Object.Destroy(this.filterMenu.gameObject);
		}
		this.filterMenu = null;
	}

	// Token: 0x0600A40A RID: 41994 RVA: 0x003E484C File Offset: 0x003E2A4C
	public void ClearDiagrams()
	{
		for (int i = 0; i < this.activeDiagrams.Count; i++)
		{
			if (this.activeDiagrams[i] != null)
			{
				UnityEngine.Object.Destroy(this.activeDiagrams[i]);
			}
		}
		this.activeDiagrams.Clear();
		Vector2 sizeDelta = this.diagramsParent.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.y = 0f;
		this.diagramsParent.GetComponent<RectTransform>().sizeDelta = sizeDelta;
	}

	// Token: 0x0600A40B RID: 41995 RVA: 0x003E48D0 File Offset: 0x003E2AD0
	public OverlayLegend.OverlayInfo GetOverlayInfo(OverlayModes.Mode mode)
	{
		for (int i = 0; i < this.overlayInfoList.Count; i++)
		{
			if (this.overlayInfoList[i].mode == mode.ViewMode())
			{
				return this.overlayInfoList[i];
			}
		}
		return null;
	}

	// Token: 0x0600A40C RID: 41996 RVA: 0x003E4920 File Offset: 0x003E2B20
	private void PopulateOverlayInfoUnits(OverlayLegend.OverlayInfo overlayInfo, bool isRefresh = false)
	{
		if (overlayInfo.infoUnits != null && overlayInfo.infoUnits.Count > 0)
		{
			this.activeUnitsParent.SetActive(true);
			using (List<OverlayLegend.OverlayInfoUnit>.Enumerator enumerator = overlayInfo.infoUnits.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					OverlayLegend.OverlayInfoUnit overlayInfoUnit = enumerator.Current;
					GameObject freeUnitObject = this.GetFreeUnitObject();
					if (overlayInfoUnit.icon != null)
					{
						Image component = freeUnitObject.transform.Find("Icon").GetComponent<Image>();
						component.gameObject.SetActive(true);
						component.sprite = overlayInfoUnit.icon;
						component.color = overlayInfoUnit.color;
						component.enabled = true;
						component.type = (overlayInfoUnit.sliceIcon ? Image.Type.Sliced : Image.Type.Simple);
					}
					else
					{
						freeUnitObject.transform.Find("Icon").gameObject.SetActive(false);
					}
					if (!string.IsNullOrEmpty(overlayInfoUnit.description))
					{
						LocText componentInChildren = freeUnitObject.GetComponentInChildren<LocText>();
						componentInChildren.text = string.Format(overlayInfoUnit.description, overlayInfoUnit.formatData);
						componentInChildren.color = overlayInfoUnit.fontColor;
						componentInChildren.enabled = true;
					}
					ToolTip component2 = freeUnitObject.GetComponent<ToolTip>();
					if (!string.IsNullOrEmpty(overlayInfoUnit.tooltip))
					{
						component2.toolTip = string.Format(overlayInfoUnit.tooltip, overlayInfoUnit.tooltipFormatData);
						component2.enabled = true;
					}
					else
					{
						component2.enabled = false;
					}
					freeUnitObject.SetActive(true);
					freeUnitObject.transform.SetParent(this.activeUnitsParent.transform);
				}
				return;
			}
		}
		this.activeUnitsParent.SetActive(false);
	}

	// Token: 0x0600A40D RID: 41997 RVA: 0x003E4ACC File Offset: 0x003E2CCC
	private void PopulateOverlayDiagrams(OverlayLegend.OverlayInfo overlayInfo, bool isRefresh = false)
	{
		if (!isRefresh)
		{
			if (overlayInfo.mode == OverlayModes.Temperature.ID)
			{
				Game.TemperatureOverlayModes temperatureOverlayMode = Game.Instance.temperatureOverlayMode;
				if (temperatureOverlayMode != Game.TemperatureOverlayModes.AbsoluteTemperature)
				{
					if (temperatureOverlayMode == Game.TemperatureOverlayModes.RelativeTemperature)
					{
						this.ClearDiagrams();
						overlayInfo = this.overlayInfoList.Find((OverlayLegend.OverlayInfo match) => match.name == UI.OVERLAYS.RELATIVETEMPERATURE.NAME);
					}
				}
				else
				{
					SimDebugView.Instance.user_temperatureThresholds[0] = 0f;
					SimDebugView.Instance.user_temperatureThresholds[1] = 2073f;
				}
			}
			if (overlayInfo.diagrams != null && overlayInfo.diagrams.Count > 0)
			{
				this.diagramsParent.SetActive(true);
				using (List<GameObject>.Enumerator enumerator = overlayInfo.diagrams.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameObject original = enumerator.Current;
						GameObject item = Util.KInstantiateUI(original, this.diagramsParent, false);
						this.activeDiagrams.Add(item);
					}
					return;
				}
			}
			this.diagramsParent.SetActive(false);
		}
	}

	// Token: 0x0600A40E RID: 41998 RVA: 0x003E4BE8 File Offset: 0x003E2DE8
	private void PopulateGeneratedLegend(OverlayLegend.OverlayInfo info, bool isRefresh = false)
	{
		if (isRefresh)
		{
			this.RemoveActiveObjects();
			this.ClearDiagrams();
		}
		if (info.infoUnits != null && info.infoUnits.Count > 0)
		{
			this.PopulateOverlayInfoUnits(info, isRefresh);
		}
		this.PopulateOverlayDiagrams(info, false);
		List<LegendEntry> customLegendData = this.currentMode.GetCustomLegendData();
		if (customLegendData != null)
		{
			this.activeUnitsParent.SetActive(true);
			using (List<LegendEntry>.Enumerator enumerator = customLegendData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LegendEntry legendEntry = enumerator.Current;
					GameObject freeUnitObject = this.GetFreeUnitObject();
					Image component = freeUnitObject.transform.Find("Icon").GetComponent<Image>();
					component.gameObject.SetActive(legendEntry.displaySprite);
					component.sprite = legendEntry.sprite;
					component.color = legendEntry.colour;
					component.enabled = true;
					component.type = Image.Type.Simple;
					LocText componentInChildren = freeUnitObject.GetComponentInChildren<LocText>();
					componentInChildren.text = legendEntry.name;
					componentInChildren.color = Color.white;
					componentInChildren.enabled = true;
					ToolTip component2 = freeUnitObject.GetComponent<ToolTip>();
					component2.enabled = (legendEntry.desc != null || legendEntry.desc_arg != null);
					component2.toolTip = ((legendEntry.desc_arg == null) ? legendEntry.desc : string.Format(legendEntry.desc, legendEntry.desc_arg));
					freeUnitObject.SetActive(true);
					freeUnitObject.transform.SetParent(this.activeUnitsParent.transform);
				}
				goto IL_165;
			}
		}
		this.activeUnitsParent.SetActive(false);
		IL_165:
		if (!isRefresh && this.currentMode.legendFilters != null)
		{
			GameObject gameObject = Util.KInstantiateUI(this.toolParameterMenuPrefab, this.diagramsParent.transform.parent.gameObject, false);
			gameObject.transform.SetAsFirstSibling();
			this.filterMenu = gameObject.GetComponent<ToolParameterMenu>();
			this.filterMenu.PopulateMenu(this.currentMode.legendFilters);
			this.filterMenu.onParametersChanged += this.OnFiltersChanged;
			this.OnFiltersChanged();
		}
		this.ConfigureUIHeight();
	}

	// Token: 0x0600A40F RID: 41999 RVA: 0x0010A66E File Offset: 0x0010886E
	private void OnFiltersChanged()
	{
		this.currentMode.OnFiltersChanged();
		this.PopulateGeneratedLegend(this.GetOverlayInfo(this.currentMode), true);
		Game.Instance.ForceOverlayUpdate(false);
	}

	// Token: 0x0600A410 RID: 42000 RVA: 0x0010A699 File Offset: 0x00108899
	private void DisableOverlay()
	{
		this.filterMenu.onParametersChanged -= this.OnFiltersChanged;
		this.filterMenu.ClearMenu();
		this.filterMenu.gameObject.SetActive(false);
		this.filterMenu = null;
	}

	// Token: 0x0600A411 RID: 42001 RVA: 0x003E4DEC File Offset: 0x003E2FEC
	private void ConfigureUIHeight()
	{
		this.scrollRectLayout.enabled = false;
		this.scrollRectLayout.GetComponent<VerticalLayoutGroup>().enabled = true;
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
		this.scrollRectLayout.preferredWidth = this.scrollRectLayout.rectTransform().sizeDelta.x;
		float y = this.scrollRectLayout.rectTransform().sizeDelta.y;
		this.scrollRectLayout.preferredHeight = Mathf.Min(y, 512f);
		this.scrollRectLayout.GetComponent<VerticalLayoutGroup>().enabled = false;
		this.scrollRectLayout.enabled = true;
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
	}

	// Token: 0x04008024 RID: 32804
	public static OverlayLegend Instance;

	// Token: 0x04008025 RID: 32805
	[SerializeField]
	private LocText title;

	// Token: 0x04008026 RID: 32806
	[SerializeField]
	private Sprite emptySprite;

	// Token: 0x04008027 RID: 32807
	[SerializeField]
	private List<OverlayLegend.OverlayInfo> overlayInfoList;

	// Token: 0x04008028 RID: 32808
	[SerializeField]
	private GameObject unitPrefab;

	// Token: 0x04008029 RID: 32809
	[SerializeField]
	private GameObject activeUnitsParent;

	// Token: 0x0400802A RID: 32810
	[SerializeField]
	private GameObject diagramsParent;

	// Token: 0x0400802B RID: 32811
	[SerializeField]
	private GameObject inactiveUnitsParent;

	// Token: 0x0400802C RID: 32812
	[SerializeField]
	private GameObject toolParameterMenuPrefab;

	// Token: 0x0400802D RID: 32813
	[SerializeField]
	private LayoutElement scrollRectLayout;

	// Token: 0x0400802E RID: 32814
	private ToolParameterMenu filterMenu;

	// Token: 0x0400802F RID: 32815
	private OverlayModes.Mode currentMode;

	// Token: 0x04008030 RID: 32816
	private List<GameObject> inactiveUnitObjs;

	// Token: 0x04008031 RID: 32817
	private List<GameObject> activeUnitObjs;

	// Token: 0x04008032 RID: 32818
	private List<GameObject> activeDiagrams = new List<GameObject>();

	// Token: 0x02001E8E RID: 7822
	[Serializable]
	public class OverlayInfoUnit
	{
		// Token: 0x0600A413 RID: 42003 RVA: 0x0010A6E8 File Offset: 0x001088E8
		public OverlayInfoUnit(Sprite icon, string description, Color color, Color fontColor, object formatData = null, bool sliceIcon = false)
		{
			this.icon = icon;
			this.description = description;
			this.color = color;
			this.fontColor = fontColor;
			this.formatData = formatData;
			this.sliceIcon = sliceIcon;
		}

		// Token: 0x04008033 RID: 32819
		public Sprite icon;

		// Token: 0x04008034 RID: 32820
		public string description;

		// Token: 0x04008035 RID: 32821
		public string tooltip;

		// Token: 0x04008036 RID: 32822
		public Color color;

		// Token: 0x04008037 RID: 32823
		public Color fontColor;

		// Token: 0x04008038 RID: 32824
		public object formatData;

		// Token: 0x04008039 RID: 32825
		public object tooltipFormatData;

		// Token: 0x0400803A RID: 32826
		public bool sliceIcon;
	}

	// Token: 0x02001E8F RID: 7823
	[Serializable]
	public class OverlayInfo
	{
		// Token: 0x0400803B RID: 32827
		public string name;

		// Token: 0x0400803C RID: 32828
		public HashedString mode;

		// Token: 0x0400803D RID: 32829
		public List<OverlayLegend.OverlayInfoUnit> infoUnits;

		// Token: 0x0400803E RID: 32830
		public List<GameObject> diagrams;

		// Token: 0x0400803F RID: 32831
		public bool isProgrammaticallyPopulated;
	}
}
