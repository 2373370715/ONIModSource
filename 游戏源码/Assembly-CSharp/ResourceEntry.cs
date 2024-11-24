using System;
using System.Collections;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001EDA RID: 7898
[AddComponentMenu("KMonoBehaviour/scripts/ResourceEntry")]
public class ResourceEntry : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISim4000ms
{
	// Token: 0x0600A627 RID: 42535 RVA: 0x003F1C28 File Offset: 0x003EFE28
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.QuantityLabel.color = this.AvailableColor;
		this.NameLabel.color = this.AvailableColor;
		this.button.onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x0600A628 RID: 42536 RVA: 0x0010BC8D File Offset: 0x00109E8D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tooltip.OnToolTip = new Func<string>(this.OnToolTip);
		this.RefreshChart();
	}

	// Token: 0x0600A629 RID: 42537 RVA: 0x003F1C7C File Offset: 0x003EFE7C
	private void OnClick()
	{
		this.lastClickTime = Time.unscaledTime;
		if (this.cachedPickupables == null)
		{
			this.cachedPickupables = ClusterManager.Instance.activeWorld.worldInventory.CreatePickupablesList(this.Resource);
			base.StartCoroutine(this.ClearCachedPickupablesAfterThreshold());
		}
		if (this.cachedPickupables == null)
		{
			return;
		}
		Pickupable pickupable = null;
		for (int i = 0; i < this.cachedPickupables.Count; i++)
		{
			this.selectionIdx++;
			int index = this.selectionIdx % this.cachedPickupables.Count;
			pickupable = this.cachedPickupables[index];
			if (pickupable != null && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
			{
				break;
			}
		}
		if (pickupable != null)
		{
			Transform transform = pickupable.transform;
			if (pickupable.storage != null)
			{
				transform = pickupable.storage.transform;
			}
			SelectTool.Instance.SelectAndFocus(transform.transform.GetPosition(), transform.GetComponent<KSelectable>(), Vector3.zero);
			for (int j = 0; j < this.cachedPickupables.Count; j++)
			{
				Pickupable pickupable2 = this.cachedPickupables[j];
				if (pickupable2 != null)
				{
					KAnimControllerBase component = pickupable2.GetComponent<KAnimControllerBase>();
					if (component != null)
					{
						component.HighlightColour = this.HighlightColor;
					}
				}
			}
		}
	}

	// Token: 0x0600A62A RID: 42538 RVA: 0x0010BCB2 File Offset: 0x00109EB2
	private IEnumerator ClearCachedPickupablesAfterThreshold()
	{
		while (this.cachedPickupables != null && this.lastClickTime != 0f && Time.unscaledTime - this.lastClickTime < 10f)
		{
			yield return SequenceUtil.WaitForSeconds(1f);
		}
		this.cachedPickupables = null;
		yield break;
	}

	// Token: 0x0600A62B RID: 42539 RVA: 0x003F1DD8 File Offset: 0x003EFFD8
	public void GetAmounts(EdiblesManager.FoodInfo food_info, bool doExtras, out float available, out float total, out float reserved)
	{
		available = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(this.Resource, false);
		total = (doExtras ? ClusterManager.Instance.activeWorld.worldInventory.GetTotalAmount(this.Resource, false) : 0f);
		reserved = (doExtras ? MaterialNeeds.GetAmount(this.Resource, ClusterManager.Instance.activeWorldId, false) : 0f);
		if (food_info != null)
		{
			available *= food_info.CaloriesPerUnit;
			total *= food_info.CaloriesPerUnit;
			reserved *= food_info.CaloriesPerUnit;
		}
	}

	// Token: 0x0600A62C RID: 42540 RVA: 0x003F1E78 File Offset: 0x003F0078
	private void GetAmounts(bool doExtras, out float available, out float total, out float reserved)
	{
		EdiblesManager.FoodInfo food_info = (this.Measure == GameUtil.MeasureUnit.kcal) ? EdiblesManager.GetFoodInfo(this.Resource.Name) : null;
		this.GetAmounts(food_info, doExtras, out available, out total, out reserved);
	}

	// Token: 0x0600A62D RID: 42541 RVA: 0x003F1EB0 File Offset: 0x003F00B0
	public void UpdateValue()
	{
		this.SetName(this.Resource.ProperName());
		bool allowInsufficientMaterialBuild = GenericGameSettings.instance.allowInsufficientMaterialBuild;
		float num;
		float num2;
		float num3;
		this.GetAmounts(allowInsufficientMaterialBuild, out num, out num2, out num3);
		if (this.currentQuantity != num)
		{
			this.currentQuantity = num;
			this.QuantityLabel.text = ResourceCategoryScreen.QuantityTextForMeasure(num, this.Measure);
		}
		Color color = this.AvailableColor;
		if (num3 > num2)
		{
			color = this.OverdrawnColor;
		}
		else if (num == 0f)
		{
			color = this.UnavailableColor;
		}
		if (this.QuantityLabel.color != color)
		{
			this.QuantityLabel.color = color;
		}
		if (this.NameLabel.color != color)
		{
			this.NameLabel.color = color;
		}
	}

	// Token: 0x0600A62E RID: 42542 RVA: 0x003F1F78 File Offset: 0x003F0178
	private string OnToolTip()
	{
		float quantity;
		float quantity2;
		float quantity3;
		this.GetAmounts(true, out quantity, out quantity2, out quantity3);
		string text = this.NameLabel.text + "\n";
		text += string.Format(UI.RESOURCESCREEN.AVAILABLE_TOOLTIP, ResourceCategoryScreen.QuantityTextForMeasure(quantity, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity3, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity2, this.Measure));
		float delta = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.Resource).GetDelta(150f);
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

	// Token: 0x0600A62F RID: 42543 RVA: 0x0010BCC1 File Offset: 0x00109EC1
	public void SetName(string name)
	{
		this.NameLabel.text = name;
	}

	// Token: 0x0600A630 RID: 42544 RVA: 0x0010BCCF File Offset: 0x00109ECF
	public void SetTag(Tag t, GameUtil.MeasureUnit measure)
	{
		this.Resource = t;
		this.Measure = measure;
		this.cachedPickupables = null;
	}

	// Token: 0x0600A631 RID: 42545 RVA: 0x003F2070 File Offset: 0x003F0270
	private void Hover(bool is_hovering)
	{
		if (ClusterManager.Instance.activeWorld.worldInventory == null)
		{
			return;
		}
		if (is_hovering)
		{
			this.Background.color = this.BackgroundHoverColor;
		}
		else
		{
			this.Background.color = new Color(0f, 0f, 0f, 0f);
		}
		ICollection<Pickupable> pickupables = ClusterManager.Instance.activeWorld.worldInventory.GetPickupables(this.Resource, false);
		if (pickupables == null)
		{
			return;
		}
		foreach (Pickupable pickupable in pickupables)
		{
			if (!(pickupable == null))
			{
				KAnimControllerBase component = pickupable.GetComponent<KAnimControllerBase>();
				if (!(component == null))
				{
					if (is_hovering)
					{
						component.HighlightColour = this.HighlightColor;
					}
					else
					{
						component.HighlightColour = Color.black;
					}
				}
			}
		}
	}

	// Token: 0x0600A632 RID: 42546 RVA: 0x0010BCE6 File Offset: 0x00109EE6
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.Hover(true);
	}

	// Token: 0x0600A633 RID: 42547 RVA: 0x0010BCEF File Offset: 0x00109EEF
	public void OnPointerExit(PointerEventData eventData)
	{
		this.Hover(false);
	}

	// Token: 0x0600A634 RID: 42548 RVA: 0x003F2164 File Offset: 0x003F0364
	public void SetSprite(Tag t)
	{
		Element element = ElementLoader.FindElementByName(this.Resource.Name);
		if (element != null)
		{
			Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(element.substance.anim, "ui", false, "");
			if (uispriteFromMultiObjectAnim != null)
			{
				this.image.sprite = uispriteFromMultiObjectAnim;
			}
		}
	}

	// Token: 0x0600A635 RID: 42549 RVA: 0x0010BCF8 File Offset: 0x00109EF8
	public void SetSprite(Sprite sprite)
	{
		this.image.sprite = sprite;
	}

	// Token: 0x0600A636 RID: 42550 RVA: 0x0010BD06 File Offset: 0x00109F06
	public void Sim4000ms(float dt)
	{
		this.RefreshChart();
	}

	// Token: 0x0600A637 RID: 42551 RVA: 0x003F21B8 File Offset: 0x003F03B8
	private void RefreshChart()
	{
		if (this.sparkChart != null)
		{
			ResourceTracker resourceStatistic = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.Resource);
			this.sparkChart.GetComponentInChildren<LineLayer>().RefreshLine(resourceStatistic.ChartableData(3000f), "resourceAmount");
			this.sparkChart.GetComponentInChildren<SparkLayer>().SetColor(Constants.NEUTRAL_COLOR);
		}
	}

	// Token: 0x04008280 RID: 33408
	public Tag Resource;

	// Token: 0x04008281 RID: 33409
	public GameUtil.MeasureUnit Measure;

	// Token: 0x04008282 RID: 33410
	public LocText NameLabel;

	// Token: 0x04008283 RID: 33411
	public LocText QuantityLabel;

	// Token: 0x04008284 RID: 33412
	public Image image;

	// Token: 0x04008285 RID: 33413
	[SerializeField]
	private Color AvailableColor;

	// Token: 0x04008286 RID: 33414
	[SerializeField]
	private Color UnavailableColor;

	// Token: 0x04008287 RID: 33415
	[SerializeField]
	private Color OverdrawnColor;

	// Token: 0x04008288 RID: 33416
	[SerializeField]
	private Color HighlightColor;

	// Token: 0x04008289 RID: 33417
	[SerializeField]
	private Color BackgroundHoverColor;

	// Token: 0x0400828A RID: 33418
	[SerializeField]
	private Image Background;

	// Token: 0x0400828B RID: 33419
	[MyCmpGet]
	private ToolTip tooltip;

	// Token: 0x0400828C RID: 33420
	[MyCmpReq]
	private Button button;

	// Token: 0x0400828D RID: 33421
	public GameObject sparkChart;

	// Token: 0x0400828E RID: 33422
	private const float CLICK_RESET_TIME_THRESHOLD = 10f;

	// Token: 0x0400828F RID: 33423
	private int selectionIdx;

	// Token: 0x04008290 RID: 33424
	private float lastClickTime;

	// Token: 0x04008291 RID: 33425
	private List<Pickupable> cachedPickupables;

	// Token: 0x04008292 RID: 33426
	private float currentQuantity = float.MinValue;
}
