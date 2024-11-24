using System;
using ProcGen;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C9F RID: 7327
[AddComponentMenu("KMonoBehaviour/scripts/DestinationAsteroid2")]
public class DestinationAsteroid2 : KMonoBehaviour
{
	// Token: 0x14000029 RID: 41
	// (add) Token: 0x060098D8 RID: 39128 RVA: 0x003B1EA0 File Offset: 0x003B00A0
	// (remove) Token: 0x060098D9 RID: 39129 RVA: 0x003B1ED8 File Offset: 0x003B00D8
	public event Action<ColonyDestinationAsteroidBeltData> OnClicked;

	// Token: 0x060098DA RID: 39130 RVA: 0x001037B9 File Offset: 0x001019B9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.button.onClick += this.OnClickInternal;
	}

	// Token: 0x060098DB RID: 39131 RVA: 0x003B1F10 File Offset: 0x003B0110
	public void SetAsteroid(ColonyDestinationAsteroidBeltData newAsteroidData)
	{
		if (this.asteroidData == null || newAsteroidData.beltPath != this.asteroidData.beltPath)
		{
			this.asteroidData = newAsteroidData;
			ProcGen.World getStartWorld = newAsteroidData.GetStartWorld;
			KAnimFile kanimFile;
			Assets.TryGetAnim(getStartWorld.asteroidIcon.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : getStartWorld.asteroidIcon, out kanimFile);
			if (kanimFile != null)
			{
				this.asteroidImage.gameObject.SetActive(false);
				this.animController.AnimFiles = new KAnimFile[]
				{
					kanimFile
				};
				this.animController.initialMode = KAnim.PlayMode.Loop;
				this.animController.initialAnim = "idle_loop";
				this.animController.gameObject.SetActive(true);
				if (this.animController.HasAnimation(this.animController.initialAnim))
				{
					this.animController.Play(this.animController.initialAnim, KAnim.PlayMode.Loop, 1f, 0f);
				}
			}
			else
			{
				this.animController.gameObject.SetActive(false);
				this.asteroidImage.gameObject.SetActive(true);
				this.asteroidImage.sprite = this.asteroidData.sprite;
				this.imageDlcFrom.gameObject.SetActive(false);
			}
			Sprite sprite = null;
			if (DlcManager.IsDlcId(this.asteroidData.Layout.dlcIdFrom))
			{
				sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(this.asteroidData.Layout.dlcIdFrom));
			}
			if (sprite != null)
			{
				this.imageDlcFrom.gameObject.SetActive(true);
				this.imageDlcFrom.sprite = sprite;
				return;
			}
			this.imageDlcFrom.gameObject.SetActive(false);
			this.imageDlcFrom.sprite = sprite;
		}
	}

	// Token: 0x060098DC RID: 39132 RVA: 0x001037D8 File Offset: 0x001019D8
	private void OnClickInternal()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Clicked asteroid belt",
			this.asteroidData.beltPath
		});
		this.OnClicked(this.asteroidData);
	}

	// Token: 0x04007718 RID: 30488
	[SerializeField]
	private Image asteroidImage;

	// Token: 0x04007719 RID: 30489
	[SerializeField]
	private KButton button;

	// Token: 0x0400771A RID: 30490
	[SerializeField]
	private KBatchedAnimController animController;

	// Token: 0x0400771B RID: 30491
	[SerializeField]
	private Image imageDlcFrom;

	// Token: 0x0400771D RID: 30493
	private ColonyDestinationAsteroidBeltData asteroidData;
}
