using System.Collections;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/ResourceEntry")]
public class
    ResourceEntry : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISim4000ms {
    private const float CLICK_RESET_TIME_THRESHOLD = 10f;

    [SerializeField]
    private Color AvailableColor;

    [SerializeField]
    private Image Background;

    [SerializeField]
    private Color BackgroundHoverColor;

    [MyCmpReq]
    private Button button;

    private List<Pickupable> cachedPickupables;
    private float            currentQuantity = float.MinValue;

    [SerializeField]
    private Color HighlightColor;

    public  Image                image;
    private float                lastClickTime;
    public  GameUtil.MeasureUnit Measure;
    public  LocText              NameLabel;

    [SerializeField]
    private Color OverdrawnColor;

    public  LocText    QuantityLabel;
    public  Tag        Resource;
    private int        selectionIdx;
    public  GameObject sparkChart;

    [MyCmpGet]
    private ToolTip tooltip;

    [SerializeField]
    private Color UnavailableColor;

    public void OnPointerEnter(PointerEventData eventData) { Hover(true); }
    public void OnPointerExit(PointerEventData  eventData) { Hover(false); }
    public void Sim4000ms(float                 dt)        { RefreshChart(); }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        QuantityLabel.color = AvailableColor;
        NameLabel.color     = AvailableColor;
        button.onClick.AddListener(OnClick);
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        tooltip.OnToolTip = OnToolTip;
        RefreshChart();
    }

    private void OnClick() {
        lastClickTime = Time.unscaledTime;
        if (cachedPickupables == null) {
            cachedPickupables = ClusterManager.Instance.activeWorld.worldInventory.CreatePickupablesList(Resource);
            StartCoroutine(ClearCachedPickupablesAfterThreshold());
        }

        if (cachedPickupables == null) return;

        Pickupable pickupable = null;
        for (var i = 0; i < cachedPickupables.Count; i++) {
            selectionIdx++;
            var index = selectionIdx % cachedPickupables.Count;
            pickupable = cachedPickupables[index];
            if (pickupable != null && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate)) break;
        }

        if (pickupable != null) {
            var transform                             = pickupable.transform;
            if (pickupable.storage != null) transform = pickupable.storage.transform;
            SelectTool.Instance.SelectAndFocus(transform.transform.GetPosition(),
                                               transform.GetComponent<KSelectable>(),
                                               Vector3.zero);

            for (var j = 0; j < cachedPickupables.Count; j++) {
                var pickupable2 = cachedPickupables[j];
                if (pickupable2 != null) {
                    var component                                    = pickupable2.GetComponent<KAnimControllerBase>();
                    if (component != null) component.HighlightColour = HighlightColor;
                }
            }
        }
    }

    private IEnumerator ClearCachedPickupablesAfterThreshold() {
        while (cachedPickupables != null && lastClickTime != 0f && Time.unscaledTime - lastClickTime < 10f)
            yield return SequenceUtil.WaitForSeconds(1f);

        cachedPickupables = null;
    }

    public void GetAmounts(EdiblesManager.FoodInfo food_info,
                           bool                    doExtras,
                           out float               available,
                           out float               total,
                           out float               reserved) {
        available = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(Resource, false);
        total     = doExtras ? ClusterManager.Instance.activeWorld.worldInventory.GetTotalAmount(Resource, false) : 0f;
        reserved  = doExtras ? MaterialNeeds.GetAmount(Resource, ClusterManager.Instance.activeWorldId, false) : 0f;
        if (food_info != null) {
            available *= food_info.CaloriesPerUnit;
            total     *= food_info.CaloriesPerUnit;
            reserved  *= food_info.CaloriesPerUnit;
        }
    }

    private void GetAmounts(bool doExtras, out float available, out float total, out float reserved) {
        var food_info = Measure == GameUtil.MeasureUnit.kcal ? EdiblesManager.GetFoodInfo(Resource.Name) : null;
        GetAmounts(food_info, doExtras, out available, out total, out reserved);
    }

    public void UpdateValue() {
        SetName(Resource.ProperName());
        var   allowInsufficientMaterialBuild = GenericGameSettings.instance.allowInsufficientMaterialBuild;
        float num;
        float num2;
        float num3;
        GetAmounts(allowInsufficientMaterialBuild, out num, out num2, out num3);
        if (currentQuantity != num) {
            currentQuantity    = num;
            QuantityLabel.text = ResourceCategoryScreen.QuantityTextForMeasure(num, Measure);
        }

        var color = AvailableColor;
        if (num3 > num2)
            color                 = OverdrawnColor;
        else if (num == 0f) color = UnavailableColor;

        if (QuantityLabel.color != color) QuantityLabel.color = color;
        if (NameLabel.color     != color) NameLabel.color     = color;
    }

    private string OnToolTip() {
        float quantity;
        float quantity2;
        float quantity3;
        GetAmounts(true, out quantity, out quantity2, out quantity3);
        var text = NameLabel.text + "\n";
        text += string.Format(UI.RESOURCESCREEN.AVAILABLE_TOOLTIP,
                              ResourceCategoryScreen.QuantityTextForMeasure(quantity,  Measure),
                              ResourceCategoryScreen.QuantityTextForMeasure(quantity3, Measure),
                              ResourceCategoryScreen.QuantityTextForMeasure(quantity2, Measure));

        var delta = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, Resource)
                               .GetDelta(150f);

        if (delta != 0f)
            text = text   +
                   "\n\n" +
                   string.Format(UI.RESOURCESCREEN.TREND_TOOLTIP,
                                 delta > 0f ? UI.RESOURCESCREEN.INCREASING_STR : UI.RESOURCESCREEN.DECREASING_STR,
                                 GameUtil.GetFormattedMass(Mathf.Abs(delta)));
        else
            text = text + "\n\n" + UI.RESOURCESCREEN.TREND_TOOLTIP_NO_CHANGE;

        return text;
    }

    public void SetName(string name) { NameLabel.text = name; }

    public void SetTag(Tag t, GameUtil.MeasureUnit measure) {
        Resource          = t;
        Measure           = measure;
        cachedPickupables = null;
    }

    private void Hover(bool is_hovering) {
        if (ClusterManager.Instance.activeWorld.worldInventory == null) return;

        if (is_hovering)
            Background.color = BackgroundHoverColor;
        else
            Background.color = new Color(0f, 0f, 0f, 0f);

        var pickupables = ClusterManager.Instance.activeWorld.worldInventory.GetPickupables(Resource);
        if (pickupables == null) return;

        foreach (var pickupable in pickupables)
            if (!(pickupable == null)) {
                var component = pickupable.GetComponent<KAnimControllerBase>();
                if (!(component == null)) {
                    if (is_hovering)
                        component.HighlightColour = HighlightColor;
                    else
                        component.HighlightColour = Color.black;
                }
            }
    }

    public void SetSprite(Tag t) {
        var element = ElementLoader.FindElementByName(Resource.Name);
        if (element != null) {
            var uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(element.substance.anim);
            if (uispriteFromMultiObjectAnim != null) image.sprite = uispriteFromMultiObjectAnim;
        }
    }

    public void SetSprite(Sprite sprite) { image.sprite = sprite; }

    private void RefreshChart() {
        if (sparkChart != null) {
            var resourceStatistic
                = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, Resource);

            sparkChart.GetComponentInChildren<LineLayer>()
                      .RefreshLine(resourceStatistic.ChartableData(3000f), "resourceAmount");

            sparkChart.GetComponentInChildren<SparkLayer>().SetColor(Constants.NEUTRAL_COLOR);
        }
    }
}