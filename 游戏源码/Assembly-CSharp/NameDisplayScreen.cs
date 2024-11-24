using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E48 RID: 7752
public class NameDisplayScreen : KScreen
{
	// Token: 0x0600A25D RID: 41565 RVA: 0x001095AA File Offset: 0x001077AA
	public static void DestroyInstance()
	{
		NameDisplayScreen.Instance = null;
	}

	// Token: 0x0600A25E RID: 41566 RVA: 0x001095B2 File Offset: 0x001077B2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		NameDisplayScreen.Instance = this;
	}

	// Token: 0x0600A25F RID: 41567 RVA: 0x001095C0 File Offset: 0x001077C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Health.Register(new Action<Health>(this.OnHealthAdded), null);
		Components.Equipment.Register(new Action<Equipment>(this.OnEquipmentAdded), null);
		this.BindOnOverlayChange();
	}

	// Token: 0x0600A260 RID: 41568 RVA: 0x003DCFEC File Offset: 0x003DB1EC
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.isOverlayChangeBound && OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			this.isOverlayChangeBound = false;
		}
	}

	// Token: 0x0600A261 RID: 41569 RVA: 0x003DD044 File Offset: 0x003DB244
	private void BindOnOverlayChange()
	{
		if (this.isOverlayChangeBound)
		{
			return;
		}
		if (OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			this.isOverlayChangeBound = true;
		}
	}

	// Token: 0x0600A262 RID: 41570 RVA: 0x003DD094 File Offset: 0x003DB294
	public void RemoveWorldEntries(int worldId)
	{
		this.entries.RemoveAll((NameDisplayScreen.Entry entry) => entry.world_go.IsNullOrDestroyed() || entry.world_go.GetMyWorldId() == worldId);
	}

	// Token: 0x0600A263 RID: 41571 RVA: 0x001095FC File Offset: 0x001077FC
	private void OnOverlayChanged(HashedString new_mode)
	{
		HashedString hashedString = this.lastKnownOverlayID;
		this.lastKnownOverlayID = new_mode;
		this.nameDisplayCanvas.enabled = (this.lastKnownOverlayID == OverlayModes.None.ID);
	}

	// Token: 0x0600A264 RID: 41572 RVA: 0x00109627 File Offset: 0x00107827
	private void OnHealthAdded(Health health)
	{
		this.RegisterComponent(health.gameObject, health, false);
	}

	// Token: 0x0600A265 RID: 41573 RVA: 0x003DD0C8 File Offset: 0x003DB2C8
	private void OnEquipmentAdded(Equipment equipment)
	{
		MinionAssignablesProxy component = equipment.GetComponent<MinionAssignablesProxy>();
		GameObject targetGameObject = component.GetTargetGameObject();
		if (targetGameObject)
		{
			this.RegisterComponent(targetGameObject, equipment, false);
			return;
		}
		global::Debug.LogWarningFormat("OnEquipmentAdded proxy target {0} was null.", new object[]
		{
			component.TargetInstanceID
		});
	}

	// Token: 0x0600A266 RID: 41574 RVA: 0x003DD114 File Offset: 0x003DB314
	private bool ShouldShowName(GameObject representedObject)
	{
		CharacterOverlay component = representedObject.GetComponent<CharacterOverlay>();
		return component != null && component.shouldShowName;
	}

	// Token: 0x0600A267 RID: 41575 RVA: 0x003DD13C File Offset: 0x003DB33C
	public Guid AddAreaText(string initialText, GameObject prefab)
	{
		NameDisplayScreen.TextEntry textEntry = new NameDisplayScreen.TextEntry();
		textEntry.guid = Guid.NewGuid();
		textEntry.display_go = Util.KInstantiateUI(prefab, this.areaTextDisplayCanvas.gameObject, true);
		textEntry.display_go.GetComponentInChildren<LocText>().text = initialText;
		this.textEntries.Add(textEntry);
		return textEntry.guid;
	}

	// Token: 0x0600A268 RID: 41576 RVA: 0x003DD198 File Offset: 0x003DB398
	public GameObject GetWorldText(Guid guid)
	{
		GameObject result = null;
		foreach (NameDisplayScreen.TextEntry textEntry in this.textEntries)
		{
			if (textEntry.guid == guid)
			{
				result = textEntry.display_go;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600A269 RID: 41577 RVA: 0x003DD200 File Offset: 0x003DB400
	public void RemoveWorldText(Guid guid)
	{
		int num = -1;
		for (int i = 0; i < this.textEntries.Count; i++)
		{
			if (this.textEntries[i].guid == guid)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			UnityEngine.Object.Destroy(this.textEntries[num].display_go);
			this.textEntries.RemoveAt(num);
		}
	}

	// Token: 0x0600A26A RID: 41578 RVA: 0x003DD268 File Offset: 0x003DB468
	public void AddNewEntry(GameObject representedObject)
	{
		NameDisplayScreen.Entry entry = new NameDisplayScreen.Entry();
		entry.world_go = representedObject;
		entry.world_go_anim_controller = representedObject.GetComponent<KAnimControllerBase>();
		GameObject original = this.ShouldShowName(representedObject) ? this.nameAndBarsPrefab : this.barsPrefab;
		entry.kprfabID = representedObject.GetComponent<KPrefabID>();
		entry.collider = representedObject.GetComponent<KBoxCollider2D>();
		GameObject gameObject = Util.KInstantiateUI(original, this.nameDisplayCanvas.gameObject, true);
		entry.display_go = gameObject;
		entry.display_go_rect = gameObject.GetComponent<RectTransform>();
		entry.nameLabel = entry.display_go.GetComponentInChildren<LocText>();
		entry.display_go.SetActive(false);
		if (this.worldSpace)
		{
			entry.display_go.transform.localScale = Vector3.one * 0.01f;
		}
		gameObject.name = representedObject.name + " character overlay";
		entry.Name = representedObject.name;
		entry.refs = gameObject.GetComponent<HierarchyReferences>();
		this.entries.Add(entry);
		UnityEngine.Object component = representedObject.GetComponent<KSelectable>();
		FactionAlignment component2 = representedObject.GetComponent<FactionAlignment>();
		if (component != null)
		{
			if (component2 != null)
			{
				if (component2.Alignment == FactionManager.FactionID.Friendly || component2.Alignment == FactionManager.FactionID.Duplicant)
				{
					this.UpdateName(representedObject);
					return;
				}
			}
			else
			{
				this.UpdateName(representedObject);
			}
		}
	}

	// Token: 0x0600A26B RID: 41579 RVA: 0x003DD3A0 File Offset: 0x003DB5A0
	public void RegisterComponent(GameObject representedObject, object component, bool force_new_entry = false)
	{
		NameDisplayScreen.Entry entry = force_new_entry ? null : this.GetEntry(representedObject);
		if (entry == null)
		{
			CharacterOverlay component2 = representedObject.GetComponent<CharacterOverlay>();
			if (component2 != null)
			{
				component2.Register();
				entry = this.GetEntry(representedObject);
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
				GameObject gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.healthBarPrefab, reference.gameObject, false);
				gameObject.name = "Health Bar";
				health.healthBar = gameObject.GetComponent<HealthBar>();
				health.healthBar.GetComponent<KSelectable>().entityName = UI.METERS.HEALTH.TOOLTIP;
				health.healthBar.GetComponent<KSelectableHealthBar>().IsSelectable = (representedObject.GetComponent<MinionBrain>() != null);
				entry.healthBar = health.healthBar;
				entry.healthBar.autoHide = false;
				gameObject.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
				return;
			}
			global::Debug.LogWarningFormat("Health added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is OxygenBreather)
		{
			if (!entry.breathBar)
			{
				GameObject gameObject2 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.breathBar = gameObject2.GetComponent<ProgressBar>();
				entry.breathBar.autoHide = false;
				gameObject2.gameObject.GetComponent<ToolTip>().AddMultiStringTooltip("Breath", this.ToolTipStyle_Property);
				gameObject2.name = "Breath Bar";
				gameObject2.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("BreathBar");
				gameObject2.GetComponent<KSelectable>().entityName = UI.METERS.BREATH.TOOLTIP;
				return;
			}
			global::Debug.LogWarningFormat("OxygenBreather added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is Equipment)
		{
			if (!entry.suitBar)
			{
				GameObject gameObject3 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.suitBar = gameObject3.GetComponent<ProgressBar>();
				entry.suitBar.autoHide = false;
				gameObject3.name = "Suit Tank Bar";
				gameObject3.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("OxygenTankBar");
				gameObject3.GetComponent<KSelectable>().entityName = UI.METERS.BREATH.TOOLTIP;
			}
			else
			{
				global::Debug.LogWarningFormat("SuitBar added twice {0}", new object[]
				{
					component
				});
			}
			if (!entry.suitFuelBar)
			{
				GameObject gameObject4 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.suitFuelBar = gameObject4.GetComponent<ProgressBar>();
				entry.suitFuelBar.autoHide = false;
				gameObject4.name = "Suit Fuel Bar";
				gameObject4.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("FuelTankBar");
				gameObject4.GetComponent<KSelectable>().entityName = UI.METERS.FUEL.TOOLTIP;
			}
			else
			{
				global::Debug.LogWarningFormat("FuelBar added twice {0}", new object[]
				{
					component
				});
			}
			if (!entry.suitBatteryBar)
			{
				GameObject gameObject5 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.suitBatteryBar = gameObject5.GetComponent<ProgressBar>();
				entry.suitBatteryBar.autoHide = false;
				gameObject5.name = "Suit Battery Bar";
				gameObject5.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("BatteryBar");
				gameObject5.GetComponent<KSelectable>().entityName = UI.METERS.BATTERY.TOOLTIP;
				return;
			}
			global::Debug.LogWarningFormat("CoolantBar added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is ThoughtGraph.Instance || component is CreatureThoughtGraph.Instance)
		{
			if (!entry.thoughtBubble)
			{
				GameObject gameObject6 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubble, entry.display_go, false);
				entry.thoughtBubble = gameObject6.GetComponent<HierarchyReferences>();
				gameObject6.name = ((component is CreatureThoughtGraph.Instance) ? "Creature " : "") + "Thought Bubble";
				GameObject gameObject7 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubbleConvo, entry.display_go, false);
				entry.thoughtBubbleConvo = gameObject7.GetComponent<HierarchyReferences>();
				gameObject7.name = ((component is CreatureThoughtGraph.Instance) ? "Creature " : "") + "Thought Bubble Convo";
				return;
			}
			global::Debug.LogWarningFormat("ThoughtGraph added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else
		{
			if (!(component is GameplayEventMonitor.Instance))
			{
				if (component is Dreamer.Instance && !entry.dreamBubble)
				{
					GameObject gameObject8 = Util.KInstantiateUI(EffectPrefabs.Instance.DreamBubble, entry.display_go, false);
					gameObject8.name = "Dream Bubble";
					entry.dreamBubble = gameObject8.GetComponent<DreamBubble>();
				}
				return;
			}
			if (!entry.gameplayEventDisplay)
			{
				GameObject gameObject9 = Util.KInstantiateUI(EffectPrefabs.Instance.GameplayEventDisplay, entry.display_go, false);
				entry.gameplayEventDisplay = gameObject9.GetComponent<HierarchyReferences>();
				gameObject9.name = "Gameplay Event Display";
				return;
			}
			global::Debug.LogWarningFormat("GameplayEventDisplay added twice {0}", new object[]
			{
				component
			});
			return;
		}
	}

	// Token: 0x0600A26C RID: 41580 RVA: 0x00109637 File Offset: 0x00107837
	public bool IsVisibleToZoom()
	{
		return !(Game.MainCamera == null) && Game.MainCamera.orthographicSize < this.HideDistance;
	}

	// Token: 0x0600A26D RID: 41581 RVA: 0x003DD904 File Offset: 0x003DBB04
	private void LateUpdate()
	{
		if (App.isLoading || App.IsExiting)
		{
			return;
		}
		this.BindOnOverlayChange();
		if (Game.MainCamera == null)
		{
			return;
		}
		if (this.lastKnownOverlayID != OverlayModes.None.ID)
		{
			return;
		}
		int count = this.entries.Count;
		bool flag = this.IsVisibleToZoom();
		bool flag2 = flag && this.lastKnownOverlayID == OverlayModes.None.ID;
		if (this.nameDisplayCanvas.enabled != flag2)
		{
			this.nameDisplayCanvas.enabled = flag2;
		}
		if (flag)
		{
			this.RemoveDestroyedEntries();
			this.Culling();
			this.UpdatePos();
			this.HideDeadProgressBars();
		}
	}

	// Token: 0x0600A26E RID: 41582 RVA: 0x003DD9A4 File Offset: 0x003DBBA4
	private void Culling()
	{
		if (this.entries.Count == 0)
		{
			return;
		}
		Vector2I vector2I;
		Vector2I vector2I2;
		Grid.GetVisibleCellRangeInActiveWorld(out vector2I, out vector2I2, 4, 1.5f);
		int num = Mathf.Min(500, this.entries.Count);
		for (int i = 0; i < num; i++)
		{
			int index = (this.currentUpdateIndex + i) % this.entries.Count;
			NameDisplayScreen.Entry entry = this.entries[index];
			Vector3 position = entry.world_go.transform.GetPosition();
			bool flag = position.x >= (float)vector2I.x && position.y >= (float)vector2I.y && position.x < (float)vector2I2.x && position.y < (float)vector2I2.y;
			if (entry.visible != flag)
			{
				entry.display_go.SetActive(flag);
			}
			entry.visible = flag;
		}
		this.currentUpdateIndex = (this.currentUpdateIndex + num) % this.entries.Count;
	}

	// Token: 0x0600A26F RID: 41583 RVA: 0x003DDAB0 File Offset: 0x003DBCB0
	private void UpdatePos()
	{
		CameraController instance = CameraController.Instance;
		Transform followTarget = instance.followTarget;
		int count = this.entries.Count;
		for (int i = 0; i < count; i++)
		{
			NameDisplayScreen.Entry entry = this.entries[i];
			if (entry.visible)
			{
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
					entry.display_go_rect.anchoredPosition = (this.worldSpace ? vector : base.WorldToScreen(vector));
				}
			}
		}
	}

	// Token: 0x0600A270 RID: 41584 RVA: 0x003DDBD4 File Offset: 0x003DBDD4
	private void RemoveDestroyedEntries()
	{
		int num = this.entries.Count;
		int i = 0;
		while (i < num)
		{
			if (this.entries[i].world_go == null)
			{
				UnityEngine.Object.Destroy(this.entries[i].display_go);
				num--;
				this.entries[i] = this.entries[num];
			}
			else
			{
				i++;
			}
		}
		this.entries.RemoveRange(num, this.entries.Count - num);
	}

	// Token: 0x0600A271 RID: 41585 RVA: 0x003DDC60 File Offset: 0x003DBE60
	private void HideDeadProgressBars()
	{
		int count = this.entries.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.entries[i].visible && !(this.entries[i].world_go == null) && this.entries[i].kprfabID.HasTag(GameTags.Dead) && this.entries[i].bars_go.activeSelf)
			{
				this.entries[i].bars_go.SetActive(false);
			}
		}
	}

	// Token: 0x0600A272 RID: 41586 RVA: 0x003DDD00 File Offset: 0x003DBF00
	public void UpdateName(GameObject representedObject)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(representedObject);
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

	// Token: 0x0600A273 RID: 41587 RVA: 0x003DDD80 File Offset: 0x003DBF80
	public void SetDream(GameObject minion_go, Dream dream)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.dreamBubble == null)
		{
			return;
		}
		entry.dreamBubble.SetDream(dream);
		entry.dreamBubble.GetComponent<KSelectable>().entityName = "Dreaming";
		entry.dreamBubble.gameObject.SetActive(true);
		entry.dreamBubble.SetVisibility(true);
	}

	// Token: 0x0600A274 RID: 41588 RVA: 0x003DDDE8 File Offset: 0x003DBFE8
	public void StopDreaming(GameObject minion_go)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.dreamBubble == null)
		{
			return;
		}
		entry.dreamBubble.StopDreaming();
		entry.dreamBubble.gameObject.SetActive(false);
	}

	// Token: 0x0600A275 RID: 41589 RVA: 0x003DDE2C File Offset: 0x003DC02C
	public void DreamTick(GameObject minion_go, float dt)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.dreamBubble == null)
		{
			return;
		}
		entry.dreamBubble.Tick(dt);
	}

	// Token: 0x0600A276 RID: 41590 RVA: 0x003DDE60 File Offset: 0x003DC060
	public void SetThoughtBubbleDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite bubble_sprite, Sprite topic_sprite)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.thoughtBubble == null)
		{
			return;
		}
		this.ApplyThoughtSprite(entry.thoughtBubble, bubble_sprite, "bubble_sprite");
		this.ApplyThoughtSprite(entry.thoughtBubble, topic_sprite, "icon_sprite");
		entry.thoughtBubble.GetComponent<KSelectable>().entityName = hover_text;
		entry.thoughtBubble.gameObject.SetActive(bVisible);
	}

	// Token: 0x0600A277 RID: 41591 RVA: 0x003DDED0 File Offset: 0x003DC0D0
	public void SetThoughtBubbleConvoDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite bubble_sprite, Sprite topic_sprite, Sprite mode_sprite)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.thoughtBubble == null)
		{
			return;
		}
		this.ApplyThoughtSprite(entry.thoughtBubbleConvo, bubble_sprite, "bubble_sprite");
		this.ApplyThoughtSprite(entry.thoughtBubbleConvo, topic_sprite, "icon_sprite");
		this.ApplyThoughtSprite(entry.thoughtBubbleConvo, mode_sprite, "icon_sprite_mode");
		entry.thoughtBubbleConvo.GetComponent<KSelectable>().entityName = hover_text;
		entry.thoughtBubbleConvo.gameObject.SetActive(bVisible);
	}

	// Token: 0x0600A278 RID: 41592 RVA: 0x0010965A File Offset: 0x0010785A
	private void ApplyThoughtSprite(HierarchyReferences active_bubble, Sprite sprite, string target)
	{
		active_bubble.GetReference<Image>(target).sprite = sprite;
	}

	// Token: 0x0600A279 RID: 41593 RVA: 0x003DDF54 File Offset: 0x003DC154
	public void SetGameplayEventDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite sprite)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.gameplayEventDisplay == null)
		{
			return;
		}
		entry.gameplayEventDisplay.GetReference<Image>("icon_sprite").sprite = sprite;
		entry.gameplayEventDisplay.GetComponent<KSelectable>().entityName = hover_text;
		entry.gameplayEventDisplay.gameObject.SetActive(bVisible);
	}

	// Token: 0x0600A27A RID: 41594 RVA: 0x003DDFB4 File Offset: 0x003DC1B4
	public void SetBreathDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.breathBar == null)
		{
			return;
		}
		entry.breathBar.SetUpdateFunc(updatePercentFull);
		entry.breathBar.SetVisibility(bVisible);
	}

	// Token: 0x0600A27B RID: 41595 RVA: 0x003DDFF4 File Offset: 0x003DC1F4
	public void SetHealthDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.healthBar == null)
		{
			return;
		}
		entry.healthBar.OnChange();
		entry.healthBar.SetUpdateFunc(updatePercentFull);
		if (entry.healthBar.gameObject.activeSelf != bVisible)
		{
			entry.healthBar.SetVisibility(bVisible);
		}
	}

	// Token: 0x0600A27C RID: 41596 RVA: 0x003DE054 File Offset: 0x003DC254
	public void SetSuitTankDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.suitBar == null)
		{
			return;
		}
		entry.suitBar.SetUpdateFunc(updatePercentFull);
		entry.suitBar.SetVisibility(bVisible);
	}

	// Token: 0x0600A27D RID: 41597 RVA: 0x003DE094 File Offset: 0x003DC294
	public void SetSuitFuelDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.suitFuelBar == null)
		{
			return;
		}
		entry.suitFuelBar.SetUpdateFunc(updatePercentFull);
		entry.suitFuelBar.SetVisibility(bVisible);
	}

	// Token: 0x0600A27E RID: 41598 RVA: 0x003DE0D4 File Offset: 0x003DC2D4
	public void SetSuitBatteryDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.suitBatteryBar == null)
		{
			return;
		}
		entry.suitBatteryBar.SetUpdateFunc(updatePercentFull);
		entry.suitBatteryBar.SetVisibility(bVisible);
	}

	// Token: 0x0600A27F RID: 41599 RVA: 0x003DE114 File Offset: 0x003DC314
	private NameDisplayScreen.Entry GetEntry(GameObject worldObject)
	{
		return this.entries.Find((NameDisplayScreen.Entry entry) => entry.world_go == worldObject);
	}

	// Token: 0x04007EAF RID: 32431
	[SerializeField]
	private float HideDistance;

	// Token: 0x04007EB0 RID: 32432
	public static NameDisplayScreen Instance;

	// Token: 0x04007EB1 RID: 32433
	[SerializeField]
	private Canvas nameDisplayCanvas;

	// Token: 0x04007EB2 RID: 32434
	[SerializeField]
	private Canvas areaTextDisplayCanvas;

	// Token: 0x04007EB3 RID: 32435
	public GameObject nameAndBarsPrefab;

	// Token: 0x04007EB4 RID: 32436
	public GameObject barsPrefab;

	// Token: 0x04007EB5 RID: 32437
	public TextStyleSetting ToolTipStyle_Property;

	// Token: 0x04007EB6 RID: 32438
	[SerializeField]
	private Color selectedColor;

	// Token: 0x04007EB7 RID: 32439
	[SerializeField]
	private Color defaultColor;

	// Token: 0x04007EB8 RID: 32440
	public int fontsize_min = 14;

	// Token: 0x04007EB9 RID: 32441
	public int fontsize_max = 32;

	// Token: 0x04007EBA RID: 32442
	public float cameraDistance_fontsize_min = 6f;

	// Token: 0x04007EBB RID: 32443
	public float cameraDistance_fontsize_max = 4f;

	// Token: 0x04007EBC RID: 32444
	public List<NameDisplayScreen.Entry> entries = new List<NameDisplayScreen.Entry>();

	// Token: 0x04007EBD RID: 32445
	public List<NameDisplayScreen.TextEntry> textEntries = new List<NameDisplayScreen.TextEntry>();

	// Token: 0x04007EBE RID: 32446
	public bool worldSpace = true;

	// Token: 0x04007EBF RID: 32447
	private bool isOverlayChangeBound;

	// Token: 0x04007EC0 RID: 32448
	private HashedString lastKnownOverlayID = OverlayModes.None.ID;

	// Token: 0x04007EC1 RID: 32449
	private int currentUpdateIndex;

	// Token: 0x02001E49 RID: 7753
	[Serializable]
	public class Entry
	{
		// Token: 0x04007EC2 RID: 32450
		public string Name;

		// Token: 0x04007EC3 RID: 32451
		public bool visible;

		// Token: 0x04007EC4 RID: 32452
		public GameObject world_go;

		// Token: 0x04007EC5 RID: 32453
		public GameObject display_go;

		// Token: 0x04007EC6 RID: 32454
		public GameObject bars_go;

		// Token: 0x04007EC7 RID: 32455
		public KPrefabID kprfabID;

		// Token: 0x04007EC8 RID: 32456
		public KBoxCollider2D collider;

		// Token: 0x04007EC9 RID: 32457
		public KAnimControllerBase world_go_anim_controller;

		// Token: 0x04007ECA RID: 32458
		public RectTransform display_go_rect;

		// Token: 0x04007ECB RID: 32459
		public LocText nameLabel;

		// Token: 0x04007ECC RID: 32460
		public HealthBar healthBar;

		// Token: 0x04007ECD RID: 32461
		public ProgressBar breathBar;

		// Token: 0x04007ECE RID: 32462
		public ProgressBar suitBar;

		// Token: 0x04007ECF RID: 32463
		public ProgressBar suitFuelBar;

		// Token: 0x04007ED0 RID: 32464
		public ProgressBar suitBatteryBar;

		// Token: 0x04007ED1 RID: 32465
		public DreamBubble dreamBubble;

		// Token: 0x04007ED2 RID: 32466
		public HierarchyReferences thoughtBubble;

		// Token: 0x04007ED3 RID: 32467
		public HierarchyReferences thoughtBubbleConvo;

		// Token: 0x04007ED4 RID: 32468
		public HierarchyReferences gameplayEventDisplay;

		// Token: 0x04007ED5 RID: 32469
		public HierarchyReferences refs;
	}

	// Token: 0x02001E4A RID: 7754
	public class TextEntry
	{
		// Token: 0x04007ED6 RID: 32470
		public Guid guid;

		// Token: 0x04007ED7 RID: 32471
		public GameObject display_go;
	}
}
