using System;
using STRINGS;
using UnityEngine;

[Serializable]
public class SaveConfigurationScreen {
    [SerializeField]
    private LocText autosaveDescriptionLabel;

    [SerializeField]
    private KSlider autosaveFrequencySlider;

    [SerializeField]
    private GameObject disabledContentPanel;

    [SerializeField]
    private GameObject disabledContentWarning;

    [SerializeField]
    private GameObject perSaveWarning;

    private int[] sliderValueToCycleCount = {
        -1,
        50,
        20,
        10,
        5,
        2,
        1
    };

    private Vector2I[] sliderValueToResolution = {
        new Vector2I(-1,   -1),
        new Vector2I(256,  384),
        new Vector2I(512,  768),
        new Vector2I(1024, 1536),
        new Vector2I(2048, 3072),
        new Vector2I(4096, 6144),
        new Vector2I(8192, 12288)
    };

    [SerializeField]
    private LocText timelapseDescriptionLabel;

    [SerializeField]
    private KSlider timelapseResolutionSlider;

    public void ToggleDisabledContent(bool enable) {
        if (enable) {
            disabledContentPanel.SetActive(true);
            disabledContentWarning.SetActive(false);
            perSaveWarning.SetActive(true);
            return;
        }

        disabledContentPanel.SetActive(false);
        disabledContentWarning.SetActive(true);
        perSaveWarning.SetActive(false);
    }

    public void Init() {
        autosaveFrequencySlider.minValue = 0f;
        autosaveFrequencySlider.maxValue = sliderValueToCycleCount.Length - 1;
        autosaveFrequencySlider.onValueChanged.AddListener(delegate(float val) {
                                                               OnAutosaveValueChanged(Mathf.FloorToInt(val));
                                                           });

        autosaveFrequencySlider.value      = CycleCountToSlider(SaveGame.Instance.AutoSaveCycleInterval);
        timelapseResolutionSlider.minValue = 0f;
        timelapseResolutionSlider.maxValue = sliderValueToResolution.Length - 1;
        timelapseResolutionSlider.onValueChanged.AddListener(delegate(float val) {
                                                                 OnTimelapseValueChanged(Mathf.FloorToInt(val));
                                                             });

        timelapseResolutionSlider.value = ResolutionToSliderValue(SaveGame.Instance.TimelapseResolution);
        OnTimelapseValueChanged(Mathf.FloorToInt(timelapseResolutionSlider.value));
    }

    public void Show(bool show) {
        if (show) {
            autosaveFrequencySlider.value   = CycleCountToSlider(SaveGame.Instance.AutoSaveCycleInterval);
            timelapseResolutionSlider.value = ResolutionToSliderValue(SaveGame.Instance.TimelapseResolution);
            OnAutosaveValueChanged(Mathf.FloorToInt(autosaveFrequencySlider.value));
            OnTimelapseValueChanged(Mathf.FloorToInt(timelapseResolutionSlider.value));
        }
    }

    private void OnTimelapseValueChanged(int sliderValue) {
        var vector2I = SliderValueToResolution(sliderValue);
        if (vector2I.x <= 0)
            timelapseDescriptionLabel.SetText(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.TIMELAPSE_DISABLED_DESCRIPTION);
        else
            timelapseDescriptionLabel.SetText(string.Format(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN
                                                              .TIMELAPSE_RESOLUTION_DESCRIPTION,
                                                            vector2I.x,
                                                            vector2I.y));

        SaveGame.Instance.TimelapseResolution = vector2I;
        Game.Instance.Trigger(75424175);
    }

    private void OnAutosaveValueChanged(int sliderValue) {
        var num = SliderValueToCycleCount(sliderValue);
        if (sliderValue == 0)
            autosaveDescriptionLabel.SetText(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.AUTOSAVE_NEVER);
        else
            autosaveDescriptionLabel.SetText(string.Format(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN
                                                             .AUTOSAVE_FREQUENCY_DESCRIPTION,
                                                           num));

        SaveGame.Instance.AutoSaveCycleInterval = num;
    }

    private int SliderValueToCycleCount(int sliderValue) { return sliderValueToCycleCount[sliderValue]; }

    private int CycleCountToSlider(int count) {
        for (var i = 0; i < sliderValueToCycleCount.Length; i++)
            if (sliderValueToCycleCount[i] == count)
                return i;

        return 0;
    }

    private Vector2I SliderValueToResolution(int sliderValue) { return sliderValueToResolution[sliderValue]; }

    private int ResolutionToSliderValue(Vector2I resolution) {
        for (var i = 0; i < sliderValueToResolution.Length; i++)
            if (sliderValueToResolution[i] == resolution)
                return i;

        return 0;
    }
}