using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ScannerNetworkVisualizer")]
public class ScannerNetworkVisualizer : KMonoBehaviour
{
	public Vector2I OriginOffset = new Vector2I(0, 0);

	public int RangeMin;

	public int RangeMax;

	protected override void OnSpawn()
	{
		Components.ScannerVisualizers.Add(base.gameObject.GetMyWorldId(), this);
	}

	protected override void OnCleanUp()
	{
		Components.ScannerVisualizers.Remove(base.gameObject.GetMyWorldId(), this);
	}
}
