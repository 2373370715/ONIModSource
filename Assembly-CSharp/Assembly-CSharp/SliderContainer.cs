using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/SliderContainer")]
public class SliderContainer : KMonoBehaviour {
    public bool    isPercentValue = true;
    public LocText nameLabel;
    public KSlider slider;
    public LocText valueLabel;

    protected override void OnSpawn() {
        base.OnSpawn();
        slider.onValueChanged.AddListener(UpdateSliderLabel);
    }

    public void UpdateSliderLabel(float newValue) {
        if (isPercentValue) {
            valueLabel.text = (newValue * 100f).ToString("F0") + "%";
            return;
        }

        valueLabel.text = newValue.ToString();
    }
}