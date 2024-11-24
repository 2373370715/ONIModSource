using System;
using ProcGen;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/DestinationAsteroid2")]
public class DestinationAsteroid2 : KMonoBehaviour
{
	[SerializeField]
	private Image asteroidImage;

	[SerializeField]
	private KButton button;

	[SerializeField]
	private KBatchedAnimController animController;

	[SerializeField]
	private Image imageDlcFrom;

	private ColonyDestinationAsteroidBeltData asteroidData;

	public event Action<ColonyDestinationAsteroidBeltData> OnClicked;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		button.onClick += OnClickInternal;
	}

	public void SetAsteroid(ColonyDestinationAsteroidBeltData newAsteroidData)
	{
		if (asteroidData != null && !(newAsteroidData.beltPath != asteroidData.beltPath))
		{
			return;
		}
		asteroidData = newAsteroidData;
		ProcGen.World getStartWorld = newAsteroidData.GetStartWorld;
		Assets.TryGetAnim(getStartWorld.asteroidIcon.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : getStartWorld.asteroidIcon, out var anim);
		if (anim != null)
		{
			asteroidImage.gameObject.SetActive(value: false);
			animController.AnimFiles = new KAnimFile[1] { anim };
			animController.initialMode = KAnim.PlayMode.Loop;
			animController.initialAnim = "idle_loop";
			animController.gameObject.SetActive(value: true);
			if (animController.HasAnimation(animController.initialAnim))
			{
				animController.Play(animController.initialAnim, KAnim.PlayMode.Loop);
			}
		}
		else
		{
			animController.gameObject.SetActive(value: false);
			asteroidImage.gameObject.SetActive(value: true);
			asteroidImage.sprite = asteroidData.sprite;
			imageDlcFrom.gameObject.SetActive(value: false);
		}
		Sprite sprite = null;
		if (DlcManager.IsDlcId(asteroidData.Layout.dlcIdFrom))
		{
			sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(asteroidData.Layout.dlcIdFrom));
		}
		if (sprite != null)
		{
			imageDlcFrom.gameObject.SetActive(value: true);
			imageDlcFrom.sprite = sprite;
		}
		else
		{
			imageDlcFrom.gameObject.SetActive(value: false);
			imageDlcFrom.sprite = sprite;
		}
	}

	private void OnClickInternal()
	{
		DebugUtil.LogArgs("Clicked asteroid belt", asteroidData.beltPath);
		this.OnClicked(asteroidData);
	}
}
