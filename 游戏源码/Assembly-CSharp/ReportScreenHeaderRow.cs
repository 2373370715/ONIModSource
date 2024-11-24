using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenHeaderRow")]
public class ReportScreenHeaderRow : KMonoBehaviour
{
	[SerializeField]
	public new LocText name;

	[SerializeField]
	private LayoutElement spacer;

	[SerializeField]
	private Image bgImage;

	public float groupSpacerWidth;

	private float nameWidth = 164f;

	[SerializeField]
	private Color oddRowColor;

	public void SetLine(ReportManager.ReportGroup reportGroup)
	{
		LayoutElement component = name.GetComponent<LayoutElement>();
		float minWidth = (component.preferredWidth = nameWidth);
		component.minWidth = minWidth;
		spacer.minWidth = groupSpacerWidth;
		name.text = reportGroup.stringKey;
	}
}
