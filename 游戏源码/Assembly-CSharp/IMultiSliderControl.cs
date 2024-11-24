public interface IMultiSliderControl
{
	string SidescreenTitleKey { get; }

	ISliderControl[] sliderControls { get; }

	bool SidescreenEnabled();
}
