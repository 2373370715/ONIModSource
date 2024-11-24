using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EventInfoScreen : KModalScreen
{
	[SerializeField]
	private float baseCharacterScale = 0.0057f;

	[FormerlySerializedAs("midgroundPrefab")]
	[FormerlySerializedAs("mid")]
	[Header("Prefabs")]
	[SerializeField]
	private GameObject animPrefab;

	[SerializeField]
	private GameObject optionPrefab;

	[SerializeField]
	private GameObject optionIconPrefab;

	[SerializeField]
	private GameObject optionTextPrefab;

	[Header("Groups")]
	[SerializeField]
	private Transform artSection;

	[SerializeField]
	private Transform midgroundGroup;

	[SerializeField]
	private GameObject timeGroup;

	[SerializeField]
	private GameObject buttonsGroup;

	[SerializeField]
	private GameObject chainGroup;

	[Header("Text")]
	[SerializeField]
	private LocText eventHeader;

	[SerializeField]
	private LocText eventTimeLabel;

	[SerializeField]
	private LocText eventLocationLabel;

	[SerializeField]
	private LocText eventDescriptionLabel;

	[SerializeField]
	private bool loadMinionFromPersonalities = true;

	[SerializeField]
	private LocText chainCount;

	[Header("Button Colour Styles")]
	[SerializeField]
	private ColorStyleSetting neutralButtonSetting;

	[SerializeField]
	private ColorStyleSetting badButtonSetting;

	[SerializeField]
	private ColorStyleSetting goodButtonSetting;

	private List<KBatchedAnimController> createdAnimations = new List<KBatchedAnimController>();

	public override bool IsModal()
	{
		return true;
	}

	public void SetEventData(EventInfoData data)
	{
		data.FinalizeText();
		eventHeader.text = data.title;
		eventDescriptionLabel.text = data.description;
		eventLocationLabel.text = data.location;
		eventTimeLabel.text = data.whenDescription;
		if (data.location.IsNullOrWhiteSpace() && data.location.IsNullOrWhiteSpace())
		{
			timeGroup.gameObject.SetActive(value: false);
		}
		if (data.options.Count == 0)
		{
			data.AddDefaultOption();
		}
		artSection.gameObject.SetActive(data.animFileName != HashedString.Invalid);
		SetEventDataOptions(data);
		SetEventDataVisuals(data);
	}

	private void SetEventDataOptions(EventInfoData data)
	{
		foreach (EventInfoData.Option option in data.options)
		{
			GameObject gameObject = Util.KInstantiateUI(optionPrefab, buttonsGroup);
			gameObject.name = "Option: " + option.mainText;
			KButton component = gameObject.GetComponent<KButton>();
			component.isInteractable = option.allowed;
			component.onClick += delegate
			{
				if (option.callback != null)
				{
					option.callback();
				}
				Deactivate();
			};
			if (!option.tooltip.IsNullOrWhiteSpace())
			{
				gameObject.GetComponent<ToolTip>().SetSimpleTooltip(option.tooltip);
			}
			else
			{
				gameObject.GetComponent<ToolTip>().enabled = false;
			}
			foreach (EventInfoData.OptionIcon informationIcon in option.informationIcons)
			{
				CreateOptionIcon(gameObject, informationIcon);
			}
			Util.KInstantiateUI(optionTextPrefab, gameObject).GetComponent<LocText>().text = ((option.description == null) ? ("<b>" + option.mainText + "</b>") : ("<b>" + option.mainText + "</b>\n<i>(" + option.description + ")</i>"));
			foreach (EventInfoData.OptionIcon consequenceIcon in option.consequenceIcons)
			{
				CreateOptionIcon(gameObject, consequenceIcon);
			}
			gameObject.SetActive(value: true);
		}
	}

	public override void Deactivate()
	{
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().EventPopupSnapshot);
		base.Deactivate();
	}

	private void CreateOptionIcon(GameObject option, EventInfoData.OptionIcon optionIcon)
	{
		GameObject obj = Util.KInstantiateUI(optionIconPrefab, option);
		obj.GetComponent<ToolTip>().SetSimpleTooltip(optionIcon.tooltip);
		HierarchyReferences component = obj.GetComponent<HierarchyReferences>();
		Image reference = component.GetReference<Image>("Mask");
		Image reference2 = component.GetReference<Image>("Border");
		Image reference3 = component.GetReference<Image>("Icon");
		if (optionIcon.sprite != null)
		{
			reference3.transform.localScale *= optionIcon.scale;
		}
		Color32 color = Color.white;
		switch (optionIcon.containerType)
		{
		case EventInfoData.OptionIcon.ContainerType.Neutral:
			reference.sprite = Assets.GetSprite("container_fill_neutral");
			reference2.sprite = Assets.GetSprite("container_border_neutral");
			if (optionIcon.sprite == null)
			{
				optionIcon.sprite = Assets.GetSprite("knob");
			}
			color = GlobalAssets.Instance.colorSet.eventNeutral;
			break;
		case EventInfoData.OptionIcon.ContainerType.Positive:
			reference.sprite = Assets.GetSprite("container_fill_positive");
			reference2.sprite = Assets.GetSprite("container_border_positive");
			reference3.rectTransform.localPosition += Vector3.down * 1f;
			if (optionIcon.sprite == null)
			{
				optionIcon.sprite = Assets.GetSprite("icon_positive");
			}
			color = GlobalAssets.Instance.colorSet.eventPositive;
			break;
		case EventInfoData.OptionIcon.ContainerType.Negative:
			reference.sprite = Assets.GetSprite("container_fill_negative");
			reference2.sprite = Assets.GetSprite("container_border_negative");
			reference3.rectTransform.localPosition += Vector3.up * 1f;
			color = GlobalAssets.Instance.colorSet.eventNegative;
			if (optionIcon.sprite == null)
			{
				optionIcon.sprite = Assets.GetSprite("cancel");
			}
			break;
		case EventInfoData.OptionIcon.ContainerType.Information:
			reference.sprite = Assets.GetSprite("requirements");
			reference2.enabled = false;
			break;
		}
		reference.color = color;
		reference3.sprite = optionIcon.sprite;
		if (optionIcon.sprite == null)
		{
			reference3.gameObject.SetActive(value: false);
		}
	}

	private void SetEventDataVisuals(EventInfoData data)
	{
		createdAnimations.ForEach(delegate(KBatchedAnimController x)
		{
			Object.Destroy(x);
		});
		createdAnimations.Clear();
		KAnimFile anim = Assets.GetAnim(data.animFileName);
		if (anim == null)
		{
			Debug.LogWarning("Event " + data.title + " has no anim data");
			return;
		}
		KBatchedAnimController component = CreateAnimLayer(midgroundGroup, anim, data.mainAnim).transform.GetComponent<KBatchedAnimController>();
		if (data.minions != null)
		{
			for (int i = 0; i < data.minions.Length; i++)
			{
				if (data.minions[i] == null)
				{
					DebugUtil.LogWarningArgs($"EventInfoScreen unable to display minion {i}");
				}
				string text = $"dupe{i + 1:D2}";
				if (component.HasAnimation(text))
				{
					CreateAnimLayer(midgroundGroup, anim, text, data.minions[i]);
				}
			}
		}
		if (data.artifact != null)
		{
			string text2 = "artifact";
			if (component.HasAnimation(text2))
			{
				CreateAnimLayer(midgroundGroup, anim, text2, null, data.artifact);
			}
		}
	}

	private GameObject CreateAnimLayer(Transform parent, KAnimFile animFile, HashedString animName, GameObject minion = null, GameObject artifact = null, string targetSymbol = null)
	{
		GameObject gameObject = Object.Instantiate(animPrefab, parent);
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		createdAnimations.Add(component);
		if (minion != null)
		{
			component.AnimFiles = new KAnimFile[4]
			{
				Assets.GetAnim("body_comp_default_kanim"),
				Assets.GetAnim("head_swap_kanim"),
				Assets.GetAnim("body_swap_kanim"),
				animFile
			};
		}
		else
		{
			component.AnimFiles = new KAnimFile[1] { animFile };
		}
		gameObject.SetActive(value: true);
		if (minion != null)
		{
			if (loadMinionFromPersonalities)
			{
				component.GetComponent<UIDupeSymbolOverride>().Apply(minion.GetComponent<MinionIdentity>());
			}
			else
			{
				SymbolOverrideController component2 = component.GetComponent<SymbolOverrideController>();
				SymbolOverrideController.SymbolEntry[] getSymbolOverrides = minion.GetComponent<SymbolOverrideController>().GetSymbolOverrides;
				for (int i = 0; i < getSymbolOverrides.Length; i++)
				{
					SymbolOverrideController.SymbolEntry symbolEntry = getSymbolOverrides[i];
					component2.AddSymbolOverride(symbolEntry.targetSymbol, symbolEntry.sourceSymbol, symbolEntry.priority);
				}
			}
			MinionConfig.CopyVisibleSymbols(gameObject, minion);
		}
		if (artifact != null)
		{
			SymbolOverrideController component3 = component.GetComponent<SymbolOverrideController>();
			KBatchedAnimController component4 = artifact.GetComponent<KBatchedAnimController>();
			string initialAnim = component4.initialAnim;
			initialAnim = initialAnim.Replace("idle_", "artifact_");
			initialAnim = initialAnim.Replace("_loop", "");
			KAnim.Build.Symbol symbol = component4.AnimFiles[0].GetData().build.GetSymbol(initialAnim);
			if (symbol != null)
			{
				component3.AddSymbolOverride("snapTo_artifact", symbol);
			}
		}
		if (targetSymbol != null)
		{
			gameObject.AddOrGet<KBatchedAnimTracker>().symbol = targetSymbol;
		}
		component.Play(animName, KAnim.PlayMode.Loop);
		component.animScale = baseCharacterScale;
		return gameObject;
	}

	public static EventInfoScreen ShowPopup(EventInfoData eventInfoData)
	{
		EventInfoScreen obj = (EventInfoScreen)KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.eventInfoScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject);
		obj.SetEventData(eventInfoData);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().EventPopupSnapshot);
		KFMOD.PlayUISound(GlobalAssets.GetSound("StoryTrait_Activation_Popup_short"));
		if (eventInfoData.showCallback != null)
		{
			eventInfoData.showCallback();
		}
		if (eventInfoData.clickFocus != null)
		{
			WorldContainer myWorld = eventInfoData.clickFocus.gameObject.GetMyWorld();
			if (myWorld != null && myWorld.IsDiscovered)
			{
				CameraController.Instance.ActiveWorldStarWipe(myWorld.id, eventInfoData.clickFocus.position);
			}
		}
		return obj;
	}

	public static Notification CreateNotification(EventInfoData eventInfoData, Notification.ClickCallback clickCallback = null)
	{
		if (eventInfoData == null)
		{
			DebugUtil.LogWarningArgs("eventPopup is null in CreateStandardEventNotification");
			return null;
		}
		eventInfoData.FinalizeText();
		Notification notification = new Notification(eventInfoData.title, NotificationType.Event, null, null, expires: false, 0f, null, null, eventInfoData.clickFocus);
		if (clickCallback == null)
		{
			notification.customClickCallback = delegate
			{
				ShowPopup(eventInfoData);
			};
		}
		else
		{
			notification.customClickCallback = clickCallback;
		}
		return notification;
	}
}
