using UnityEngine;
using UnityEngine.UI;

public class TileScreen : KScreen {
    public Image gasIcon;
    public Text  gasText;
    public Image liquidIcon;
    public Text  massAmtLabel;
    public Image massIcon;
    public Text  massTitleLabel;
    public Text  nameLabel;
    public Image solidIcon;
    public Text  solidText;
    public Text  symbolLabel;

    [SerializeField]
    private Color temperatureDefaultColour;

    public MinMaxSlider temperatureSlider;
    public Image        temperatureSliderIcon;
    public Text         temperatureSliderText;

    [SerializeField]
    private Color temperatureTransitionColour;

    private bool SetSliderColour(float temperature, float transition_temperature) {
        if (Mathf.Abs(temperature - transition_temperature) < 5f) {
            temperatureSliderText.color = temperatureTransitionColour;
            temperatureSliderIcon.color = temperatureTransitionColour;
            return true;
        }

        temperatureSliderText.color = temperatureDefaultColour;
        temperatureSliderIcon.color = temperatureDefaultColour;
        return false;
    }

    private void DisplayTileInfo() {
        var mousePos = KInputManager.GetMousePos();
        mousePos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
        var num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(mousePos));
        if (Grid.IsValidCell(num) && Grid.IsVisible(num)) {
            var element = Grid.Element[num];
            nameLabel.text = element.name;
            var num2 = Grid.Mass[num];
            var arg  = "kg";
            if (num2 < 5f) {
                num2 *= 1000f;
                arg  =  "g";
            }

            if (num2 < 5f) {
                num2 *= 1000f;
                arg  =  "mg";
            }

            if (num2 < 5f) {
                num2 *= 1000f;
                arg  =  "mcg";
                num2 =  Mathf.Floor(num2);
            }

            massAmtLabel.text   = string.Format("{0:0.0} {1}", num2, arg);
            massTitleLabel.text = "mass";
            var num3 = Grid.Temperature[num];
            if (element.IsSolid) {
                solidIcon.gameObject.transform.parent.gameObject.SetActive(true);
                gasIcon.gameObject.transform.parent.gameObject.SetActive(false);
                massIcon.sprite = solidIcon.sprite;
                solidText.text  = ((int)element.highTemp).ToString();
                gasText.text    = "";
                liquidIcon.rectTransform.SetParent(solidIcon.transform.parent, true);
                liquidIcon.rectTransform.SetLocalPosition(new Vector3(0f, 64f));
                SetSliderColour(num3, element.highTemp);
                temperatureSlider.SetMinMaxValue(element.highTemp,
                                                 Mathf.Min(element.highTemp + 100f, 4000f),
                                                 Mathf.Max(element.highTemp - 100f, 0f),
                                                 Mathf.Min(element.highTemp + 100f, 4000f));
            } else if (element.IsLiquid) {
                solidIcon.gameObject.transform.parent.gameObject.SetActive(true);
                gasIcon.gameObject.transform.parent.gameObject.SetActive(true);
                massIcon.sprite = liquidIcon.sprite;
                solidText.text  = ((int)element.lowTemp).ToString();
                gasText.text    = ((int)element.highTemp).ToString();
                liquidIcon.rectTransform.SetParent(temperatureSlider.transform.parent, true);
                liquidIcon.rectTransform.SetLocalPosition(new Vector3(-80f, 0f));
                if (!SetSliderColour(num3, element.lowTemp)) SetSliderColour(num3, element.highTemp);
                temperatureSlider.SetMinMaxValue(element.lowTemp,
                                                 element.highTemp,
                                                 Mathf.Max(element.lowTemp  - 100f, 0f),
                                                 Mathf.Min(element.highTemp + 100f, 5200f));
            } else if (element.IsGas) {
                solidText.text = "";
                gasText.text   = ((int)element.lowTemp).ToString();
                solidIcon.gameObject.transform.parent.gameObject.SetActive(false);
                gasIcon.gameObject.transform.parent.gameObject.SetActive(true);
                massIcon.sprite = gasIcon.sprite;
                SetSliderColour(num3, element.lowTemp);
                liquidIcon.rectTransform.SetParent(gasIcon.transform.parent, true);
                liquidIcon.rectTransform.SetLocalPosition(new Vector3(0f, -64f));
                temperatureSlider.SetMinMaxValue(0f, Mathf.Max(element.lowTemp - 100f, 0f), 0f, element.lowTemp + 100f);
            }

            temperatureSlider.SetExtraValue(num3);
            temperatureSliderText.text = GameUtil.GetFormattedTemperature((int)num3);
            var info = FallingWater.instance.GetInfo(num);
            if (info.Count <= 0) return;

            var elements = ElementLoader.elements;
            using (var enumerator = info.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    var keyValuePair = enumerator.Current;
                    var element2     = elements[keyValuePair.Key];
                    var text         = nameLabel;
                    text.text = text.text + "\n" + element2.name + string.Format(" {0:0.00} kg", keyValuePair.Value);
                }

                return;
            }
        }

        nameLabel.text = "Unknown";
    }

    private void DisplayConduitFlowInfo() {
        var mode = OverlayScreen.Instance.GetMode();
        var utilityNetworkManager = mode == OverlayModes.GasConduits.ID
                                        ? Game.Instance.gasConduitSystem
                                        : Game.Instance.liquidConduitSystem;

        var conduitFlow = mode == OverlayModes.LiquidConduits.ID
                              ? Game.Instance.gasConduitFlow
                              : Game.Instance.liquidConduitFlow;

        var mousePos = KInputManager.GetMousePos();
        mousePos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
        var cell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(mousePos));
        if (Grid.IsValidCell(cell) && utilityNetworkManager.GetConnections(cell, true) != 0) {
            var contents    = conduitFlow.GetContents(cell);
            var element     = ElementLoader.FindElementByHash(contents.element);
            var num         = contents.mass;
            var temperature = contents.temperature;
            nameLabel.text = element.name;
            var arg = "kg";
            if (num < 5f) {
                num *= 1000f;
                arg =  "g";
            }

            massAmtLabel.text   = string.Format("{0:0.0} {1}", num, arg);
            massTitleLabel.text = "mass";
            if (element.IsLiquid) {
                solidIcon.gameObject.transform.parent.gameObject.SetActive(true);
                gasIcon.gameObject.transform.parent.gameObject.SetActive(true);
                massIcon.sprite = liquidIcon.sprite;
                solidText.text  = ((int)element.lowTemp).ToString();
                gasText.text    = ((int)element.highTemp).ToString();
                liquidIcon.rectTransform.SetParent(temperatureSlider.transform.parent, true);
                liquidIcon.rectTransform.SetLocalPosition(new Vector3(-80f, 0f));
                if (!SetSliderColour(temperature, element.lowTemp)) SetSliderColour(temperature, element.highTemp);
                temperatureSlider.SetMinMaxValue(element.lowTemp,
                                                 element.highTemp,
                                                 Mathf.Max(element.lowTemp  - 100f, 0f),
                                                 Mathf.Min(element.highTemp + 100f, 5200f));
            } else if (element.IsGas) {
                solidText.text = "";
                gasText.text   = ((int)element.lowTemp).ToString();
                solidIcon.gameObject.transform.parent.gameObject.SetActive(false);
                gasIcon.gameObject.transform.parent.gameObject.SetActive(true);
                massIcon.sprite = gasIcon.sprite;
                SetSliderColour(temperature, element.lowTemp);
                liquidIcon.rectTransform.SetParent(gasIcon.transform.parent, true);
                liquidIcon.rectTransform.SetLocalPosition(new Vector3(0f, -64f));
                temperatureSlider.SetMinMaxValue(0f, Mathf.Max(element.lowTemp - 100f, 0f), 0f, element.lowTemp + 100f);
            }

            temperatureSlider.SetExtraValue(temperature);
            temperatureSliderText.text = GameUtil.GetFormattedTemperature((int)temperature);
            return;
        }

        nameLabel.text      = "No Conduit";
        symbolLabel.text    = "";
        massAmtLabel.text   = "";
        massTitleLabel.text = "";
    }

    private void Update() {
        transform.SetPosition(KInputManager.GetMousePos());
        var mode = OverlayScreen.Instance.GetMode();
        if (mode == OverlayModes.GasConduits.ID || mode == OverlayModes.LiquidConduits.ID) {
            DisplayConduitFlowInfo();
            return;
        }

        DisplayTileInfo();
    }
}