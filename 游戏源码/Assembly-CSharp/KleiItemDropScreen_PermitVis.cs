using System.Collections;
using UnityEngine;

public class KleiItemDropScreen_PermitVis : KMonoBehaviour
{
	[SerializeField]
	private RectTransform root;

	[Header("Different Permit Visualizers")]
	[SerializeField]
	private KleiItemDropScreen_PermitVis_Fallback fallbackVis;

	[SerializeField]
	private KleiItemDropScreen_PermitVis_DupeEquipment equipmentVis;

	public void ConfigureWith(DropScreenPresentationInfo info)
	{
		ResetState();
		equipmentVis.gameObject.SetActive(value: false);
		fallbackVis.gameObject.SetActive(value: false);
		if (info.UseEquipmentVis)
		{
			equipmentVis.gameObject.SetActive(value: true);
			equipmentVis.ConfigureWith(info);
		}
		else
		{
			fallbackVis.gameObject.SetActive(value: true);
			fallbackVis.ConfigureWith(info);
		}
	}

	public Promise AnimateIn()
	{
		return Updater.RunRoutine(this, AnimateInRoutine());
	}

	public Promise AnimateOut()
	{
		return Updater.RunRoutine(this, AnimateOutRoutine());
	}

	private IEnumerator AnimateInRoutine()
	{
		root.gameObject.SetActive(value: true);
		yield return Updater.Ease(delegate(Vector3 v3)
		{
			root.transform.localScale = v3;
		}, root.transform.localScale, Vector3.one, 0.5f, Easing.EaseOutBack);
	}

	private IEnumerator AnimateOutRoutine()
	{
		yield return Updater.Ease(delegate(Vector3 v3)
		{
			root.transform.localScale = v3;
		}, root.transform.localScale, Vector3.zero, 0.25f);
		root.gameObject.SetActive(value: true);
	}

	public void ResetState()
	{
		root.transform.localScale = Vector3.zero;
	}
}
