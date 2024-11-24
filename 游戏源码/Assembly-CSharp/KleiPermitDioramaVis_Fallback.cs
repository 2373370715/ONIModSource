using Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KleiPermitDioramaVis_Fallback : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	[SerializeField]
	private Image sprite;

	[SerializeField]
	private RectTransform editorOnlyErrorMessageParent;

	[SerializeField]
	private TextMeshProUGUI editorOnlyErrorMessageText;

	private Option<string> error;

	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	public void ConfigureSetup()
	{
	}

	public void ConfigureWith(PermitResource permit)
	{
		sprite.sprite = PermitPresentationInfo.GetUnknownSprite();
		editorOnlyErrorMessageParent.gameObject.SetActive(value: false);
	}

	public KleiPermitDioramaVis_Fallback WithError(string error)
	{
		this.error = error;
		Debug.Log("[KleiInventoryScreen Error] Had to use fallback vis. " + error);
		return this;
	}
}
