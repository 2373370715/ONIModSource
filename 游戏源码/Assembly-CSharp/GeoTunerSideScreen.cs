using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class GeoTunerSideScreen : SideScreenContent
{
	private GeoTuner.Instance targetGeotuner;

	public GameObject rowPrefab;

	public RectTransform rowContainer;

	[SerializeField]
	private TextStyleSetting AnalyzedTextStyle;

	[SerializeField]
	private TextStyleSetting UnanalyzedTextStyle;

	public Dictionary<object, GameObject> rows = new Dictionary<object, GameObject>();

	private int uiRefreshSubHandle = -1;

	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		rowPrefab.SetActive(value: false);
		if (show)
		{
			RefreshOptions();
		}
	}

	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<GeoTuner.Instance>() != null;
	}

	public override void SetTarget(GameObject target)
	{
		targetGeotuner = target.GetSMI<GeoTuner.Instance>();
		RefreshOptions();
		uiRefreshSubHandle = target.Subscribe(1980521255, RefreshOptions);
	}

	public override void ClearTarget()
	{
		if (uiRefreshSubHandle != -1 && targetGeotuner != null)
		{
			targetGeotuner.gameObject.Unsubscribe(uiRefreshSubHandle);
			uiRefreshSubHandle = -1;
		}
	}

	private void RefreshOptions(object data = null)
	{
		int num = 0;
		SetRow(num++, UI.UISIDESCREENS.GEOTUNERSIDESCREEN.NOTHING, Assets.GetSprite("action_building_disabled"), null, studied: true);
		List<Geyser> items = Components.Geysers.GetItems(targetGeotuner.GetMyWorldId());
		foreach (Geyser item in items)
		{
			if (item.GetComponent<Studyable>().Studied)
			{
				SetRow(num++, UI.StripLinkFormatting(item.GetProperName()), Def.GetUISprite(item.gameObject).first, item, studied: true);
			}
		}
		foreach (Geyser item2 in items)
		{
			if (!item2.GetComponent<Studyable>().Studied && Grid.Visible[Grid.PosToCell(item2)] > 0 && item2.GetComponent<Uncoverable>().IsUncovered)
			{
				SetRow(num++, UI.StripLinkFormatting(item2.GetProperName()), Def.GetUISprite(item2.gameObject).first, item2, studied: false);
			}
		}
		for (int i = num; i < rowContainer.childCount; i++)
		{
			rowContainer.GetChild(i).gameObject.SetActive(value: false);
		}
	}

	private void ClearRows()
	{
		for (int num = rowContainer.childCount - 1; num >= 0; num--)
		{
			Util.KDestroyGameObject(rowContainer.GetChild(num));
		}
		rows.Clear();
	}

	private void SetRow(int idx, string name, Sprite icon, Geyser geyser, bool studied)
	{
		bool flag = geyser == null;
		GameObject gameObject = ((idx >= rowContainer.childCount) ? Util.KInstantiateUI(rowPrefab, rowContainer.gameObject, force_active: true) : rowContainer.GetChild(idx).gameObject);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		LocText reference = component.GetReference<LocText>("label");
		reference.text = name;
		reference.textStyleSetting = ((studied || flag) ? AnalyzedTextStyle : UnanalyzedTextStyle);
		reference.ApplySettings();
		Image reference2 = component.GetReference<Image>("icon");
		reference2.sprite = icon;
		reference2.color = (studied ? Color.white : new Color(0f, 0f, 0f, 0.5f));
		if (flag)
		{
			reference2.color = Color.black;
		}
		int count = Components.GeoTuners.GetItems(targetGeotuner.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetFutureGeyser() == geyser);
		int geotunedCount = Components.GeoTuners.GetItems(targetGeotuner.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetFutureGeyser() == geyser || x.GetAssignedGeyser() == geyser);
		ToolTip[] componentsInChildren = gameObject.GetComponentsInChildren<ToolTip>();
		ToolTip toolTip = componentsInChildren.First();
		bool usingStudiedTooltip = geyser != null && (flag || studied);
		toolTip.SetSimpleTooltip(usingStudiedTooltip ? UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP.ToString() : UI.UISIDESCREENS.GEOTUNERSIDESCREEN.UNSTUDIED_TOOLTIP.ToString());
		toolTip.enabled = geyser != null;
		toolTip.OnToolTip = delegate
		{
			if (!usingStudiedTooltip)
			{
				return UI.UISIDESCREENS.GEOTUNERSIDESCREEN.UNSTUDIED_TOOLTIP.ToString();
			}
			if (geyser != targetGeotuner.GetFutureGeyser() && geotunedCount >= 5)
			{
				return UI.UISIDESCREENS.GEOTUNERSIDESCREEN.GEOTUNER_LIMIT_TOOLTIP.ToString();
			}
			Func<float, float> obj = delegate(float emissionPerCycleModifier)
			{
				float num5 = 600f / geyser.configuration.GetIterationLength();
				return emissionPerCycleModifier / num5 / geyser.configuration.GetOnDuration();
			};
			Func<float, float, float, float> func = delegate(float iterationLength, float massPerCycle, float eruptionDuration)
			{
				float num4 = 600f / iterationLength;
				return massPerCycle / num4 / eruptionDuration;
			};
			GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = targetGeotuner.def.GetSettingsForGeyser(geyser);
			float num2 = ((Geyser.temperatureModificationMethod == Geyser.ModificationMethod.Percentages) ? (settingsForGeyser.template.temperatureModifier * geyser.configuration.geyserType.temperature) : settingsForGeyser.template.temperatureModifier);
			float num3 = obj((Geyser.massModificationMethod == Geyser.ModificationMethod.Percentages) ? (settingsForGeyser.template.massPerCycleModifier * geyser.configuration.scaledRate) : settingsForGeyser.template.massPerCycleModifier);
			float temperature = geyser.configuration.geyserType.temperature;
			func(geyser.configuration.scaledIterationLength, geyser.configuration.scaledRate, geyser.configuration.scaledIterationLength * geyser.configuration.scaledIterationPercent);
			_ = count;
			_ = count;
			string text = ((num2 > 0f) ? "+" : "") + GameUtil.GetFormattedTemperature(num2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative);
			string text2 = ((num3 > 0f) ? "+" : "") + GameUtil.GetFormattedMass(num3, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, includeSuffix: true, "{0:0.##}");
			string newValue = settingsForGeyser.material.ProperName();
			return string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP, "\n"), "\n", UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_MATERIAL).Replace("{MATERIAL}", newValue) + "\n" + text, "\n", text2), "\n"), "\n", UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_VISIT_GEYSER);
		};
		if (usingStudiedTooltip && count > 0)
		{
			ToolTip toolTip2 = componentsInChildren.Last();
			toolTip2.SetSimpleTooltip("");
			toolTip2.OnToolTip = () => UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_NUMBER_HOVERED.ToString().Replace("{0}", count.ToString());
		}
		LocText reference3 = component.GetReference<LocText>("amount");
		reference3.SetText(count.ToString());
		reference3.transform.parent.gameObject.SetActive(!flag && count > 0);
		MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
		component2.ChangeState((targetGeotuner.GetFutureGeyser() == geyser) ? 1 : 0);
		component2.onClick = delegate
		{
			if ((geyser == null || geyser.GetComponent<Studyable>().Studied) && !(geyser == targetGeotuner.GetFutureGeyser()))
			{
				int num = Components.GeoTuners.GetItems(targetGeotuner.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetFutureGeyser() == geyser || x.GetAssignedGeyser() == geyser);
				if (!(geyser != null) || num + 1 <= 5)
				{
					targetGeotuner.AssignFutureGeyser(geyser);
					RefreshOptions();
				}
			}
		};
		component2.onDoubleClick = delegate
		{
			if (geyser != null)
			{
				CameraController.Instance.CameraGoTo(geyser.transform.GetPosition());
				return true;
			}
			return false;
		};
	}
}
