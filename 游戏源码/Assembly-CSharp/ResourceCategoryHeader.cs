using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001ED7 RID: 7895
[AddComponentMenu("KMonoBehaviour/scripts/ResourceCategoryHeader")]
public class ResourceCategoryHeader : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISim4000ms
{
	// Token: 0x0600A60D RID: 42509 RVA: 0x003F115C File Offset: 0x003EF35C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.EntryContainer.SetParent(base.transform.parent);
		this.EntryContainer.SetSiblingIndex(base.transform.GetSiblingIndex() + 1);
		this.EntryContainer.localScale = Vector3.one;
		this.mButton = base.GetComponent<Button>();
		this.mButton.onClick.AddListener(delegate()
		{
			this.ToggleOpen(true);
		});
		this.SetInteractable(this.anyDiscovered);
		this.SetActiveColor(false);
	}

	// Token: 0x0600A60E RID: 42510 RVA: 0x0010BB0B File Offset: 0x00109D0B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tooltip.OnToolTip = new Func<string>(this.OnTooltip);
		this.UpdateContents();
		this.RefreshChart();
	}

	// Token: 0x0600A60F RID: 42511 RVA: 0x0010BB36 File Offset: 0x00109D36
	private void SetInteractable(bool state)
	{
		if (!state)
		{
			this.SetOpen(false);
			this.expandArrow.SetDisabled();
			return;
		}
		if (!this.IsOpen)
		{
			this.expandArrow.SetInactive();
			return;
		}
		this.expandArrow.SetActive();
	}

	// Token: 0x0600A610 RID: 42512 RVA: 0x003F11E8 File Offset: 0x003EF3E8
	private void SetActiveColor(bool state)
	{
		if (state)
		{
			this.elements.QuantityText.color = this.TextColor_Interactable;
			this.elements.LabelText.color = this.TextColor_Interactable;
			this.expandArrow.ActiveColour = this.TextColor_Interactable;
			this.expandArrow.InactiveColour = this.TextColor_Interactable;
			this.expandArrow.TargetImage.color = this.TextColor_Interactable;
			return;
		}
		this.elements.LabelText.color = this.TextColor_NonInteractable;
		this.elements.QuantityText.color = this.TextColor_NonInteractable;
		this.expandArrow.ActiveColour = this.TextColor_NonInteractable;
		this.expandArrow.InactiveColour = this.TextColor_NonInteractable;
		this.expandArrow.TargetImage.color = this.TextColor_NonInteractable;
	}

	// Token: 0x0600A611 RID: 42513 RVA: 0x003F12C4 File Offset: 0x003EF4C4
	public void SetTag(Tag t, GameUtil.MeasureUnit measure)
	{
		this.ResourceCategoryTag = t;
		this.Measure = measure;
		this.elements.LabelText.text = t.ProperName();
		if (SaveGame.Instance.expandedResourceTags.Contains(this.ResourceCategoryTag))
		{
			this.anyDiscovered = true;
			this.ToggleOpen(false);
		}
	}

	// Token: 0x0600A612 RID: 42514 RVA: 0x003F131C File Offset: 0x003EF51C
	private void ToggleOpen(bool play_sound)
	{
		if (!this.anyDiscovered)
		{
			if (play_sound)
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
			}
			return;
		}
		if (!this.IsOpen)
		{
			if (play_sound)
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
			}
			this.SetOpen(true);
			this.elements.LabelText.fontSize = (float)this.maximizedFontSize;
			this.elements.QuantityText.fontSize = (float)this.maximizedFontSize;
			return;
		}
		if (play_sound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		}
		this.SetOpen(false);
		this.elements.LabelText.fontSize = (float)this.minimizedFontSize;
		this.elements.QuantityText.fontSize = (float)this.minimizedFontSize;
	}

	// Token: 0x0600A613 RID: 42515 RVA: 0x003F13E0 File Offset: 0x003EF5E0
	private void Hover(bool is_hovering)
	{
		this.Background.color = (is_hovering ? this.BackgroundHoverColor : new Color(0f, 0f, 0f, 0f));
		ICollection<Pickupable> collection = null;
		if (ClusterManager.Instance.activeWorld.worldInventory != null)
		{
			collection = ClusterManager.Instance.activeWorld.worldInventory.GetPickupables(this.ResourceCategoryTag, false);
		}
		if (collection == null)
		{
			return;
		}
		foreach (Pickupable pickupable in collection)
		{
			if (!(pickupable == null))
			{
				KAnimControllerBase component = pickupable.GetComponent<KAnimControllerBase>();
				if (!(component == null))
				{
					component.HighlightColour = (is_hovering ? this.highlightColour : Color.black);
				}
			}
		}
	}

	// Token: 0x0600A614 RID: 42516 RVA: 0x0010BB6D File Offset: 0x00109D6D
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.Hover(true);
	}

	// Token: 0x0600A615 RID: 42517 RVA: 0x0010BB76 File Offset: 0x00109D76
	public void OnPointerExit(PointerEventData eventData)
	{
		this.Hover(false);
	}

	// Token: 0x0600A616 RID: 42518 RVA: 0x003F14C0 File Offset: 0x003EF6C0
	public void SetOpen(bool open)
	{
		this.IsOpen = open;
		if (open)
		{
			this.expandArrow.SetActive();
			if (!SaveGame.Instance.expandedResourceTags.Contains(this.ResourceCategoryTag))
			{
				SaveGame.Instance.expandedResourceTags.Add(this.ResourceCategoryTag);
			}
		}
		else
		{
			this.expandArrow.SetInactive();
			SaveGame.Instance.expandedResourceTags.Remove(this.ResourceCategoryTag);
		}
		this.EntryContainer.gameObject.SetActive(this.IsOpen);
	}

	// Token: 0x0600A617 RID: 42519 RVA: 0x003F1548 File Offset: 0x003EF748
	private void GetAmounts(bool doExtras, out float available, out float total, out float reserved)
	{
		available = 0f;
		total = 0f;
		reserved = 0f;
		HashSet<Tag> hashSet = null;
		if (!DiscoveredResources.Instance.TryGetDiscoveredResourcesFromTag(this.ResourceCategoryTag, out hashSet))
		{
			return;
		}
		ListPool<Tag, ResourceCategoryHeader>.PooledList pooledList = ListPool<Tag, ResourceCategoryHeader>.Allocate();
		foreach (Tag tag in hashSet)
		{
			EdiblesManager.FoodInfo foodInfo = null;
			if (this.Measure == GameUtil.MeasureUnit.kcal)
			{
				foodInfo = EdiblesManager.GetFoodInfo(tag.Name);
				if (foodInfo == null)
				{
					pooledList.Add(tag);
					continue;
				}
			}
			this.anyDiscovered = true;
			ResourceEntry resourceEntry = null;
			if (!this.ResourcesDiscovered.TryGetValue(tag, out resourceEntry))
			{
				resourceEntry = this.NewResourceEntry(tag, this.Measure);
				this.ResourcesDiscovered.Add(tag, resourceEntry);
			}
			float num;
			float num2;
			float num3;
			resourceEntry.GetAmounts(foodInfo, doExtras, out num, out num2, out num3);
			available += num;
			total += num2;
			reserved += num3;
		}
		foreach (Tag item in pooledList)
		{
			hashSet.Remove(item);
		}
		pooledList.Recycle();
	}

	// Token: 0x0600A618 RID: 42520 RVA: 0x003F1694 File Offset: 0x003EF894
	public void UpdateContents()
	{
		float num;
		float num2;
		float num3;
		this.GetAmounts(false, out num, out num2, out num3);
		if (num != this.cachedAvailable || num2 != this.cachedTotal || num3 != this.cachedReserved)
		{
			if (this.quantityString == null || this.currentQuantity != num)
			{
				switch (this.Measure)
				{
				case GameUtil.MeasureUnit.mass:
					this.quantityString = GameUtil.GetFormattedMass(num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
					break;
				case GameUtil.MeasureUnit.kcal:
					this.quantityString = GameUtil.GetFormattedCalories(num, GameUtil.TimeSlice.None, true);
					break;
				case GameUtil.MeasureUnit.quantity:
					this.quantityString = num.ToString();
					break;
				}
				this.elements.QuantityText.text = this.quantityString;
				this.currentQuantity = num;
			}
			this.cachedAvailable = num;
			this.cachedTotal = num2;
			this.cachedReserved = num3;
		}
		foreach (KeyValuePair<Tag, ResourceEntry> keyValuePair in this.ResourcesDiscovered)
		{
			keyValuePair.Value.UpdateValue();
		}
		this.SetActiveColor(num > 0f);
		this.SetInteractable(this.anyDiscovered);
	}

	// Token: 0x0600A619 RID: 42521 RVA: 0x003F17C4 File Offset: 0x003EF9C4
	private string OnTooltip()
	{
		float quantity;
		float quantity2;
		float quantity3;
		this.GetAmounts(true, out quantity, out quantity2, out quantity3);
		string text = this.elements.LabelText.text + "\n";
		text += string.Format(UI.RESOURCESCREEN.AVAILABLE_TOOLTIP, ResourceCategoryScreen.QuantityTextForMeasure(quantity, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity3, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity2, this.Measure));
		float delta = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.ResourceCategoryTag).GetDelta(150f);
		if (delta != 0f)
		{
			text = text + "\n\n" + string.Format(UI.RESOURCESCREEN.TREND_TOOLTIP, (delta > 0f) ? UI.RESOURCESCREEN.INCREASING_STR : UI.RESOURCESCREEN.DECREASING_STR, GameUtil.GetFormattedMass(Mathf.Abs(delta), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		else
		{
			text = text + "\n\n" + UI.RESOURCESCREEN.TREND_TOOLTIP_NO_CHANGE;
		}
		return text;
	}

	// Token: 0x0600A61A RID: 42522 RVA: 0x0010BB7F File Offset: 0x00109D7F
	private ResourceEntry NewResourceEntry(Tag resourceTag, GameUtil.MeasureUnit measure)
	{
		ResourceEntry component = Util.KInstantiateUI(this.Prefab_ResourceEntry, this.EntryContainer.gameObject, true).GetComponent<ResourceEntry>();
		component.SetTag(resourceTag, measure);
		return component;
	}

	// Token: 0x0600A61B RID: 42523 RVA: 0x0010BBA5 File Offset: 0x00109DA5
	public void Sim4000ms(float dt)
	{
		this.RefreshChart();
	}

	// Token: 0x0600A61C RID: 42524 RVA: 0x003F18C0 File Offset: 0x003EFAC0
	private void RefreshChart()
	{
		if (this.sparkChart != null)
		{
			ResourceTracker resourceStatistic = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.ResourceCategoryTag);
			this.sparkChart.GetComponentInChildren<LineLayer>().RefreshLine(resourceStatistic.ChartableData(3000f), "resourceAmount");
			this.sparkChart.GetComponentInChildren<SparkLayer>().SetColor(Constants.NEUTRAL_COLOR);
		}
	}

	// Token: 0x0400825A RID: 33370
	public GameObject Prefab_ResourceEntry;

	// Token: 0x0400825B RID: 33371
	public Transform EntryContainer;

	// Token: 0x0400825C RID: 33372
	public Tag ResourceCategoryTag;

	// Token: 0x0400825D RID: 33373
	public GameUtil.MeasureUnit Measure;

	// Token: 0x0400825E RID: 33374
	public bool IsOpen;

	// Token: 0x0400825F RID: 33375
	public ImageToggleState expandArrow;

	// Token: 0x04008260 RID: 33376
	private Button mButton;

	// Token: 0x04008261 RID: 33377
	public Dictionary<Tag, ResourceEntry> ResourcesDiscovered = new Dictionary<Tag, ResourceEntry>();

	// Token: 0x04008262 RID: 33378
	public ResourceCategoryHeader.ElementReferences elements;

	// Token: 0x04008263 RID: 33379
	public Color TextColor_Interactable;

	// Token: 0x04008264 RID: 33380
	public Color TextColor_NonInteractable;

	// Token: 0x04008265 RID: 33381
	private string quantityString;

	// Token: 0x04008266 RID: 33382
	private float currentQuantity;

	// Token: 0x04008267 RID: 33383
	private bool anyDiscovered;

	// Token: 0x04008268 RID: 33384
	public const float chartHistoryLength = 3000f;

	// Token: 0x04008269 RID: 33385
	[MyCmpGet]
	private ToolTip tooltip;

	// Token: 0x0400826A RID: 33386
	[SerializeField]
	private int minimizedFontSize;

	// Token: 0x0400826B RID: 33387
	[SerializeField]
	private int maximizedFontSize;

	// Token: 0x0400826C RID: 33388
	[SerializeField]
	private Color highlightColour;

	// Token: 0x0400826D RID: 33389
	[SerializeField]
	private Color BackgroundHoverColor;

	// Token: 0x0400826E RID: 33390
	[SerializeField]
	private Image Background;

	// Token: 0x0400826F RID: 33391
	public GameObject sparkChart;

	// Token: 0x04008270 RID: 33392
	private float cachedAvailable = float.MinValue;

	// Token: 0x04008271 RID: 33393
	private float cachedTotal = float.MinValue;

	// Token: 0x04008272 RID: 33394
	private float cachedReserved = float.MinValue;

	// Token: 0x02001ED8 RID: 7896
	[Serializable]
	public struct ElementReferences
	{
		// Token: 0x04008273 RID: 33395
		public LocText LabelText;

		// Token: 0x04008274 RID: 33396
		public LocText QuantityText;
	}
}
