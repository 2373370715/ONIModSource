using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class CodexTemperatureTransitionPanel : CodexWidget<CodexTemperatureTransitionPanel>
{
	public enum TransitionType
	{
		HEAT,
		COOL
	}

	private Element sourceElement;

	private TransitionType transitionType;

	private GameObject materialPrefab;

	private GameObject sourceContainer;

	private GameObject temperaturePanel;

	private GameObject resultsContainer;

	private LocText headerLabel;

	public CodexTemperatureTransitionPanel(Element source, TransitionType type)
	{
		sourceElement = source;
		transitionType = type;
	}

	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		materialPrefab = component.GetReference<RectTransform>("MaterialPrefab").gameObject;
		sourceContainer = component.GetReference<RectTransform>("SourceContainer").gameObject;
		temperaturePanel = component.GetReference<RectTransform>("TemperaturePanel").gameObject;
		resultsContainer = component.GetReference<RectTransform>("ResultsContainer").gameObject;
		headerLabel = component.GetReference<LocText>("HeaderLabel");
		ClearPanel();
		ConfigureSource(contentGameObject, displayPane, textStyles);
		ConfigureTemperature(contentGameObject, displayPane, textStyles);
		ConfigureResults(contentGameObject, displayPane, textStyles);
		ConfigurePreferredLayout(contentGameObject);
	}

	private void ConfigureSource(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = Util.KInstantiateUI(materialPrefab, sourceContainer, force_active: true).GetComponent<HierarchyReferences>();
		Tuple<Sprite, Color> uISprite = Def.GetUISprite(sourceElement);
		component.GetReference<Image>("Icon").sprite = uISprite.first;
		component.GetReference<Image>("Icon").color = uISprite.second;
		component.GetReference<LocText>("Title").text = $"{GameUtil.GetFormattedMass(1f)}";
		component.GetReference<LocText>("Title").color = Color.black;
		component.GetReference<ToolTip>("ToolTip").toolTip = sourceElement.name;
		component.GetReference<KButton>("Button").onClick += delegate
		{
			ManagementMenu.Instance.codexScreen.ChangeArticle(UI.ExtractLinkID(sourceElement.tag.ProperName()));
		};
	}

	private void ConfigureTemperature(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		float temp = ((transitionType == TransitionType.COOL) ? sourceElement.lowTemp : sourceElement.highTemp);
		HierarchyReferences component = temperaturePanel.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Icon").sprite = Assets.GetSprite((transitionType == TransitionType.COOL) ? "crew_state_temp_down" : "crew_state_temp_up");
		component.GetReference<LocText>("Label").text = GameUtil.GetFormattedTemperature(temp);
		component.GetReference<LocText>("Label").color = ((transitionType == TransitionType.COOL) ? Color.blue : Color.red);
		string format = ((transitionType == TransitionType.COOL) ? CODEX.FORMAT_STRINGS.TEMPERATURE_UNDER : CODEX.FORMAT_STRINGS.TEMPERATURE_OVER);
		component.GetReference<ToolTip>("ToolTip").toolTip = string.Format(format, GameUtil.GetFormattedTemperature(temp));
	}

	private void ConfigureResults(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		Element primaryElement = ((transitionType == TransitionType.COOL) ? sourceElement.lowTempTransition : sourceElement.highTempTransition);
		Element secondaryElement = ElementLoader.FindElementByHash((transitionType == TransitionType.COOL) ? sourceElement.lowTempTransitionOreID : sourceElement.highTempTransitionOreID);
		float num = ((transitionType == TransitionType.COOL) ? sourceElement.lowTempTransitionOreMassConversion : sourceElement.highTempTransitionOreMassConversion);
		if (transitionType != TransitionType.COOL)
		{
			_ = sourceElement.highTemp;
		}
		else
		{
			_ = sourceElement.lowTemp;
		}
		HierarchyReferences component = Util.KInstantiateUI(materialPrefab, resultsContainer, force_active: true).GetComponent<HierarchyReferences>();
		Tuple<Sprite, Color> uISprite = Def.GetUISprite(primaryElement);
		component.GetReference<Image>("Icon").sprite = uISprite.first;
		component.GetReference<Image>("Icon").color = uISprite.second;
		string text = $"{GameUtil.GetFormattedMass(1f)}";
		if (secondaryElement != null)
		{
			text = $"{GameUtil.GetFormattedMass(1f - num)}";
		}
		component.GetReference<LocText>("Title").text = text;
		component.GetReference<LocText>("Title").color = Color.black;
		component.GetReference<ToolTip>("ToolTip").toolTip = primaryElement.name;
		component.GetReference<KButton>("Button").onClick += delegate
		{
			ManagementMenu.Instance.codexScreen.ChangeArticle(UI.ExtractLinkID(primaryElement.tag.ProperName()));
		};
		if (secondaryElement != null)
		{
			HierarchyReferences component2 = Util.KInstantiateUI(materialPrefab, resultsContainer, force_active: true).GetComponent<HierarchyReferences>();
			Tuple<Sprite, Color> uISprite2 = Def.GetUISprite(secondaryElement);
			component2.GetReference<Image>("Icon").sprite = uISprite2.first;
			component2.GetReference<Image>("Icon").color = uISprite2.second;
			component2.GetReference<LocText>("Title").text = $"{GameUtil.GetFormattedMass(num)} {secondaryElement.name}";
			component2.GetReference<LocText>("Title").color = Color.black;
			component2.GetReference<ToolTip>("ToolTip").toolTip = secondaryElement.name;
			component2.GetReference<KButton>("Button").onClick += delegate
			{
				ManagementMenu.Instance.codexScreen.ChangeArticle(UI.ExtractLinkID(secondaryElement.tag.ProperName()));
			};
		}
		headerLabel.SetText((secondaryElement == null) ? string.Format(CODEX.FORMAT_STRINGS.TRANSITION_LABEL_TO_ONE_ELEMENT, sourceElement.name, primaryElement.name) : string.Format(CODEX.FORMAT_STRINGS.TRANSITION_LABEL_TO_TWO_ELEMENTS, sourceElement.name, primaryElement.name, secondaryElement.name));
	}

	private void ClearPanel()
	{
		foreach (Transform item in sourceContainer.transform)
		{
			Object.Destroy(item.gameObject);
		}
		foreach (Transform item2 in resultsContainer.transform)
		{
			Object.Destroy(item2.gameObject);
		}
	}
}
