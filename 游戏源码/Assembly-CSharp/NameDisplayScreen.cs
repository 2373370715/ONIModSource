using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class NameDisplayScreen : KScreen
{
	[Serializable]
	public class Entry
	{
		public string Name;

		public bool visible;

		public GameObject world_go;

		public GameObject display_go;

		public GameObject bars_go;

		public KPrefabID kprfabID;

		public KBoxCollider2D collider;

		public KAnimControllerBase world_go_anim_controller;

		public RectTransform display_go_rect;

		public LocText nameLabel;

		public HealthBar healthBar;

		public ProgressBar breathBar;

		public ProgressBar suitBar;

		public ProgressBar suitFuelBar;

		public ProgressBar suitBatteryBar;

		public DreamBubble dreamBubble;

		public HierarchyReferences thoughtBubble;

		public HierarchyReferences thoughtBubbleConvo;

		public HierarchyReferences gameplayEventDisplay;

		public HierarchyReferences refs;
	}

	public class TextEntry
	{
		public Guid guid;

		public GameObject display_go;
	}

	[SerializeField]
	private float HideDistance;

	public static NameDisplayScreen Instance;

	[SerializeField]
	private Canvas nameDisplayCanvas;

	[SerializeField]
	private Canvas areaTextDisplayCanvas;

	public GameObject nameAndBarsPrefab;

	public GameObject barsPrefab;

	public TextStyleSetting ToolTipStyle_Property;

	[SerializeField]
	private Color selectedColor;

	[SerializeField]
	private Color defaultColor;

	public int fontsize_min = 14;

	public int fontsize_max = 32;

	public float cameraDistance_fontsize_min = 6f;

	public float cameraDistance_fontsize_max = 4f;

	public List<Entry> entries = new List<Entry>();

	public List<TextEntry> textEntries = new List<TextEntry>();

	public bool worldSpace = true;

	private bool isOverlayChangeBound;

	private HashedString lastKnownOverlayID = OverlayModes.None.ID;

	private int currentUpdateIndex;

	public static void DestroyInstance()
	{
		Instance = null;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Instance = this;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Health.Register(OnHealthAdded, null);
		Components.Equipment.Register(OnEquipmentAdded, null);
		BindOnOverlayChange();
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (isOverlayChangeBound && OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(OnOverlayChanged));
			isOverlayChangeBound = false;
		}
	}

	private void BindOnOverlayChange()
	{
		if (!isOverlayChangeBound && OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(OnOverlayChanged));
			isOverlayChangeBound = true;
		}
	}

	public void RemoveWorldEntries(int worldId)
	{
		entries.RemoveAll((Entry entry) => entry.world_go.IsNullOrDestroyed() || entry.world_go.GetMyWorldId() == worldId);
	}

	private void OnOverlayChanged(HashedString new_mode)
	{
		_ = lastKnownOverlayID;
		lastKnownOverlayID = new_mode;
		nameDisplayCanvas.enabled = lastKnownOverlayID == OverlayModes.None.ID;
	}

	private void OnHealthAdded(Health health)
	{
		RegisterComponent(health.gameObject, health);
	}

	private void OnEquipmentAdded(Equipment equipment)
	{
		MinionAssignablesProxy component = equipment.GetComponent<MinionAssignablesProxy>();
		GameObject targetGameObject = component.GetTargetGameObject();
		if ((bool)targetGameObject)
		{
			RegisterComponent(targetGameObject, equipment);
			return;
		}
		Debug.LogWarningFormat("OnEquipmentAdded proxy target {0} was null.", component.TargetInstanceID);
	}

	private bool ShouldShowName(GameObject representedObject)
	{
		CharacterOverlay component = representedObject.GetComponent<CharacterOverlay>();
		if (component != null)
		{
			return component.shouldShowName;
		}
		return false;
	}

	public Guid AddAreaText(string initialText, GameObject prefab)
	{
		TextEntry textEntry = new TextEntry();
		textEntry.guid = Guid.NewGuid();
		textEntry.display_go = Util.KInstantiateUI(prefab, areaTextDisplayCanvas.gameObject, force_active: true);
		textEntry.display_go.GetComponentInChildren<LocText>().text = initialText;
		textEntries.Add(textEntry);
		return textEntry.guid;
	}

	public GameObject GetWorldText(Guid guid)
	{
		GameObject result = null;
		foreach (TextEntry textEntry in textEntries)
		{
			if (textEntry.guid == guid)
			{
				result = textEntry.display_go;
				break;
			}
		}
		return result;
	}

	public void RemoveWorldText(Guid guid)
	{
		int num = -1;
		for (int i = 0; i < textEntries.Count; i++)
		{
			if (textEntries[i].guid == guid)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			UnityEngine.Object.Destroy(textEntries[num].display_go);
			textEntries.RemoveAt(num);
		}
	}

	public void AddNewEntry(GameObject representedObject)
	{
		Entry entry = new Entry();
		entry.world_go = representedObject;
		entry.world_go_anim_controller = representedObject.GetComponent<KAnimControllerBase>();
		GameObject original = (ShouldShowName(representedObject) ? nameAndBarsPrefab : barsPrefab);
		entry.kprfabID = representedObject.GetComponent<KPrefabID>();
		entry.collider = representedObject.GetComponent<KBoxCollider2D>();
		GameObject gameObject = (entry.display_go = Util.KInstantiateUI(original, nameDisplayCanvas.gameObject, force_active: true));
		entry.display_go_rect = gameObject.GetComponent<RectTransform>();
		entry.nameLabel = entry.display_go.GetComponentInChildren<LocText>();
		entry.display_go.SetActive(value: false);
		if (worldSpace)
		{
			entry.display_go.transform.localScale = Vector3.one * 0.01f;
		}
		gameObject.name = representedObject.name + " character overlay";
		entry.Name = representedObject.name;
		entry.refs = gameObject.GetComponent<HierarchyReferences>();
		entries.Add(entry);
		KSelectable component = representedObject.GetComponent<KSelectable>();
		FactionAlignment component2 = representedObject.GetComponent<FactionAlignment>();
		if (!(component != null))
		{
			return;
		}
		if (component2 != null)
		{
			if (component2.Alignment == FactionManager.FactionID.Friendly || component2.Alignment == FactionManager.FactionID.Duplicant)
			{
				UpdateName(representedObject);
			}
		}
		else
		{
			UpdateName(representedObject);
		}
	}

	public void RegisterComponent(GameObject representedObject, object component, bool force_new_entry = false)
	{
		Entry entry = (force_new_entry ? null : GetEntry(representedObject));
		if (entry == null)
		{
			CharacterOverlay component2 = representedObject.GetComponent<CharacterOverlay>();
			if (component2 != null)
			{
				component2.Register();
				entry = GetEntry(representedObject);
			}
		}
		if (entry == null)
		{
			return;
		}
		Transform reference = entry.refs.GetReference<Transform>("Bars");
		entry.bars_go = reference.gameObject;
		if (component is Health)
		{
			if (!entry.healthBar)
			{
				Health health = (Health)component;
				GameObject gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.healthBarPrefab, reference.gameObject);
				gameObject.name = "Health Bar";
				health.healthBar = gameObject.GetComponent<HealthBar>();
				health.healthBar.GetComponent<KSelectable>().entityName = UI.METERS.HEALTH.TOOLTIP;
				health.healthBar.GetComponent<KSelectableHealthBar>().IsSelectable = representedObject.GetComponent<MinionBrain>() != null;
				entry.healthBar = health.healthBar;
				entry.healthBar.autoHide = false;
				gameObject.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
			}
			else
			{
				Debug.LogWarningFormat("Health added twice {0}", component);
			}
		}
		else if (component is OxygenBreather)
		{
			if (!entry.breathBar)
			{
				GameObject gameObject2 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject);
				entry.breathBar = gameObject2.GetComponent<ProgressBar>();
				entry.breathBar.autoHide = false;
				gameObject2.gameObject.GetComponent<ToolTip>().AddMultiStringTooltip("Breath", ToolTipStyle_Property);
				gameObject2.name = "Breath Bar";
				gameObject2.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("BreathBar");
				gameObject2.GetComponent<KSelectable>().entityName = UI.METERS.BREATH.TOOLTIP;
			}
			else
			{
				Debug.LogWarningFormat("OxygenBreather added twice {0}", component);
			}
		}
		else if (component is Equipment)
		{
			if (!entry.suitBar)
			{
				GameObject gameObject3 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject);
				entry.suitBar = gameObject3.GetComponent<ProgressBar>();
				entry.suitBar.autoHide = false;
				gameObject3.name = "Suit Tank Bar";
				gameObject3.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("OxygenTankBar");
				gameObject3.GetComponent<KSelectable>().entityName = UI.METERS.BREATH.TOOLTIP;
			}
			else
			{
				Debug.LogWarningFormat("SuitBar added twice {0}", component);
			}
			if (!entry.suitFuelBar)
			{
				GameObject gameObject4 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject);
				entry.suitFuelBar = gameObject4.GetComponent<ProgressBar>();
				entry.suitFuelBar.autoHide = false;
				gameObject4.name = "Suit Fuel Bar";
				gameObject4.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("FuelTankBar");
				gameObject4.GetComponent<KSelectable>().entityName = UI.METERS.FUEL.TOOLTIP;
			}
			else
			{
				Debug.LogWarningFormat("FuelBar added twice {0}", component);
			}
			if (!entry.suitBatteryBar)
			{
				GameObject gameObject5 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject);
				entry.suitBatteryBar = gameObject5.GetComponent<ProgressBar>();
				entry.suitBatteryBar.autoHide = false;
				gameObject5.name = "Suit Battery Bar";
				gameObject5.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("BatteryBar");
				gameObject5.GetComponent<KSelectable>().entityName = UI.METERS.BATTERY.TOOLTIP;
			}
			else
			{
				Debug.LogWarningFormat("CoolantBar added twice {0}", component);
			}
		}
		else if (component is ThoughtGraph.Instance || component is CreatureThoughtGraph.Instance)
		{
			if (!entry.thoughtBubble)
			{
				GameObject gameObject6 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubble, entry.display_go);
				entry.thoughtBubble = gameObject6.GetComponent<HierarchyReferences>();
				gameObject6.name = ((component is CreatureThoughtGraph.Instance) ? "Creature " : "") + "Thought Bubble";
				GameObject gameObject7 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubbleConvo, entry.display_go);
				entry.thoughtBubbleConvo = gameObject7.GetComponent<HierarchyReferences>();
				gameObject7.name = ((component is CreatureThoughtGraph.Instance) ? "Creature " : "") + "Thought Bubble Convo";
			}
			else
			{
				Debug.LogWarningFormat("ThoughtGraph added twice {0}", component);
			}
		}
		else if (component is GameplayEventMonitor.Instance)
		{
			if (!entry.gameplayEventDisplay)
			{
				GameObject gameObject8 = Util.KInstantiateUI(EffectPrefabs.Instance.GameplayEventDisplay, entry.display_go);
				entry.gameplayEventDisplay = gameObject8.GetComponent<HierarchyReferences>();
				gameObject8.name = "Gameplay Event Display";
			}
			else
			{
				Debug.LogWarningFormat("GameplayEventDisplay added twice {0}", component);
			}
		}
		else if (component is Dreamer.Instance && !entry.dreamBubble)
		{
			GameObject gameObject9 = Util.KInstantiateUI(EffectPrefabs.Instance.DreamBubble, entry.display_go);
			gameObject9.name = "Dream Bubble";
			entry.dreamBubble = gameObject9.GetComponent<DreamBubble>();
		}
	}

	public bool IsVisibleToZoom()
	{
		if (Game.MainCamera == null)
		{
			return false;
		}
		return Game.MainCamera.orthographicSize < HideDistance;
	}

	private void LateUpdate()
	{
		if (App.isLoading || App.IsExiting)
		{
			return;
		}
		BindOnOverlayChange();
		if (!(Game.MainCamera == null) && !(lastKnownOverlayID != OverlayModes.None.ID))
		{
			_ = entries.Count;
			bool num = IsVisibleToZoom();
			bool flag = num && lastKnownOverlayID == OverlayModes.None.ID;
			if (nameDisplayCanvas.enabled != flag)
			{
				nameDisplayCanvas.enabled = flag;
			}
			if (num)
			{
				RemoveDestroyedEntries();
				Culling();
				UpdatePos();
				HideDeadProgressBars();
			}
		}
	}

	private void Culling()
	{
		if (entries.Count == 0)
		{
			return;
		}
		Grid.GetVisibleCellRangeInActiveWorld(out var min, out var max);
		int num = Mathf.Min(500, entries.Count);
		for (int i = 0; i < num; i++)
		{
			int index = (currentUpdateIndex + i) % entries.Count;
			Entry entry = entries[index];
			Vector3 position = entry.world_go.transform.GetPosition();
			bool flag = position.x >= (float)min.x && position.y >= (float)min.y && position.x < (float)max.x && position.y < (float)max.y;
			if (entry.visible != flag)
			{
				entry.display_go.SetActive(flag);
			}
			entry.visible = flag;
		}
		currentUpdateIndex = (currentUpdateIndex + num) % entries.Count;
	}

	private void UpdatePos()
	{
		CameraController instance = CameraController.Instance;
		Transform followTarget = instance.followTarget;
		int count = entries.Count;
		for (int i = 0; i < count; i++)
		{
			Entry entry = entries[i];
			if (!entry.visible)
			{
				continue;
			}
			GameObject world_go = entry.world_go;
			if (!(world_go == null))
			{
				Vector3 vector = world_go.transform.GetPosition();
				if (followTarget == world_go.transform)
				{
					vector = instance.followTargetPos;
				}
				else if (entry.world_go_anim_controller != null && entry.collider != null)
				{
					vector.x += entry.collider.offset.x;
					vector.y += entry.collider.offset.y - entry.collider.size.y / 2f;
				}
				entry.display_go_rect.anchoredPosition = (worldSpace ? vector : WorldToScreen(vector));
			}
		}
	}

	private void RemoveDestroyedEntries()
	{
		int num = entries.Count;
		int num2 = 0;
		while (num2 < num)
		{
			if (entries[num2].world_go == null)
			{
				UnityEngine.Object.Destroy(entries[num2].display_go);
				num--;
				entries[num2] = entries[num];
			}
			else
			{
				num2++;
			}
		}
		entries.RemoveRange(num, entries.Count - num);
	}

	private void HideDeadProgressBars()
	{
		int count = entries.Count;
		for (int i = 0; i < count; i++)
		{
			if (entries[i].visible && !(entries[i].world_go == null) && entries[i].kprfabID.HasTag(GameTags.Dead) && entries[i].bars_go.activeSelf)
			{
				entries[i].bars_go.SetActive(value: false);
			}
		}
	}

	public void UpdateName(GameObject representedObject)
	{
		Entry entry = GetEntry(representedObject);
		if (entry == null)
		{
			return;
		}
		KSelectable component = representedObject.GetComponent<KSelectable>();
		entry.display_go.name = component.GetProperName() + " character overlay";
		if (entry.nameLabel != null)
		{
			entry.nameLabel.text = component.GetProperName();
			if (representedObject.GetComponent<RocketModule>() != null)
			{
				entry.nameLabel.text = representedObject.GetComponent<RocketModule>().GetParentRocketName();
			}
		}
	}

	public void SetDream(GameObject minion_go, Dream dream)
	{
		Entry entry = GetEntry(minion_go);
		if (entry != null && !(entry.dreamBubble == null))
		{
			entry.dreamBubble.SetDream(dream);
			entry.dreamBubble.GetComponent<KSelectable>().entityName = "Dreaming";
			entry.dreamBubble.gameObject.SetActive(value: true);
			entry.dreamBubble.SetVisibility(visible: true);
		}
	}

	public void StopDreaming(GameObject minion_go)
	{
		Entry entry = GetEntry(minion_go);
		if (entry != null && !(entry.dreamBubble == null))
		{
			entry.dreamBubble.StopDreaming();
			entry.dreamBubble.gameObject.SetActive(value: false);
		}
	}

	public void DreamTick(GameObject minion_go, float dt)
	{
		Entry entry = GetEntry(minion_go);
		if (entry != null && !(entry.dreamBubble == null))
		{
			entry.dreamBubble.Tick(dt);
		}
	}

	public void SetThoughtBubbleDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite bubble_sprite, Sprite topic_sprite)
	{
		Entry entry = GetEntry(minion_go);
		if (entry != null && !(entry.thoughtBubble == null))
		{
			ApplyThoughtSprite(entry.thoughtBubble, bubble_sprite, "bubble_sprite");
			ApplyThoughtSprite(entry.thoughtBubble, topic_sprite, "icon_sprite");
			entry.thoughtBubble.GetComponent<KSelectable>().entityName = hover_text;
			entry.thoughtBubble.gameObject.SetActive(bVisible);
		}
	}

	public void SetThoughtBubbleConvoDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite bubble_sprite, Sprite topic_sprite, Sprite mode_sprite)
	{
		Entry entry = GetEntry(minion_go);
		if (entry != null && !(entry.thoughtBubble == null))
		{
			ApplyThoughtSprite(entry.thoughtBubbleConvo, bubble_sprite, "bubble_sprite");
			ApplyThoughtSprite(entry.thoughtBubbleConvo, topic_sprite, "icon_sprite");
			ApplyThoughtSprite(entry.thoughtBubbleConvo, mode_sprite, "icon_sprite_mode");
			entry.thoughtBubbleConvo.GetComponent<KSelectable>().entityName = hover_text;
			entry.thoughtBubbleConvo.gameObject.SetActive(bVisible);
		}
	}

	private void ApplyThoughtSprite(HierarchyReferences active_bubble, Sprite sprite, string target)
	{
		active_bubble.GetReference<Image>(target).sprite = sprite;
	}

	public void SetGameplayEventDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite sprite)
	{
		Entry entry = GetEntry(minion_go);
		if (entry != null && !(entry.gameplayEventDisplay == null))
		{
			entry.gameplayEventDisplay.GetReference<Image>("icon_sprite").sprite = sprite;
			entry.gameplayEventDisplay.GetComponent<KSelectable>().entityName = hover_text;
			entry.gameplayEventDisplay.gameObject.SetActive(bVisible);
		}
	}

	public void SetBreathDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		Entry entry = GetEntry(minion_go);
		if (entry != null && !(entry.breathBar == null))
		{
			entry.breathBar.SetUpdateFunc(updatePercentFull);
			entry.breathBar.SetVisibility(bVisible);
		}
	}

	public void SetHealthDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		Entry entry = GetEntry(minion_go);
		if (entry != null && !(entry.healthBar == null))
		{
			entry.healthBar.OnChange();
			entry.healthBar.SetUpdateFunc(updatePercentFull);
			if (entry.healthBar.gameObject.activeSelf != bVisible)
			{
				entry.healthBar.SetVisibility(bVisible);
			}
		}
	}

	public void SetSuitTankDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		Entry entry = GetEntry(minion_go);
		if (entry != null && !(entry.suitBar == null))
		{
			entry.suitBar.SetUpdateFunc(updatePercentFull);
			entry.suitBar.SetVisibility(bVisible);
		}
	}

	public void SetSuitFuelDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		Entry entry = GetEntry(minion_go);
		if (entry != null && !(entry.suitFuelBar == null))
		{
			entry.suitFuelBar.SetUpdateFunc(updatePercentFull);
			entry.suitFuelBar.SetVisibility(bVisible);
		}
	}

	public void SetSuitBatteryDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		Entry entry = GetEntry(minion_go);
		if (entry != null && !(entry.suitBatteryBar == null))
		{
			entry.suitBatteryBar.SetUpdateFunc(updatePercentFull);
			entry.suitBatteryBar.SetVisibility(bVisible);
		}
	}

	private Entry GetEntry(GameObject worldObject)
	{
		return entries.Find((Entry entry) => entry.world_go == worldObject);
	}
}
