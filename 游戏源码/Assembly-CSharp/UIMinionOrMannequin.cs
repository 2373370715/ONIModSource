using System;
using System.Collections.Generic;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200203D RID: 8253
public class UIMinionOrMannequin : KMonoBehaviour
{
	// Token: 0x17000B32 RID: 2866
	// (get) Token: 0x0600AFA9 RID: 44969 RVA: 0x00112235 File Offset: 0x00110435
	// (set) Token: 0x0600AFAA RID: 44970 RVA: 0x0011223D File Offset: 0x0011043D
	public UIMinionOrMannequin.ITarget current { get; private set; }

	// Token: 0x0600AFAB RID: 44971 RVA: 0x00112246 File Offset: 0x00110446
	protected override void OnSpawn()
	{
		this.TrySpawn();
	}

	// Token: 0x0600AFAC RID: 44972 RVA: 0x00421D40 File Offset: 0x0041FF40
	public bool TrySpawn()
	{
		bool flag = false;
		if (this.mannequin.IsNullOrDestroyed())
		{
			GameObject gameObject = new GameObject("UIMannequin");
			gameObject.AddOrGet<RectTransform>().Fill(Padding.All(10f));
			gameObject.transform.SetParent(base.transform, false);
			AspectRatioFitter aspectRatioFitter = gameObject.AddOrGet<AspectRatioFitter>();
			aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
			aspectRatioFitter.aspectRatio = 1f;
			this.mannequin = gameObject.AddOrGet<UIMannequin>();
			this.mannequin.TrySpawn();
			gameObject.SetActive(false);
			flag = true;
		}
		if (this.minion.IsNullOrDestroyed())
		{
			GameObject gameObject2 = new GameObject("UIMinion");
			gameObject2.AddOrGet<RectTransform>().Fill(Padding.All(10f));
			gameObject2.transform.SetParent(base.transform, false);
			AspectRatioFitter aspectRatioFitter2 = gameObject2.AddOrGet<AspectRatioFitter>();
			aspectRatioFitter2.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
			aspectRatioFitter2.aspectRatio = 1f;
			this.minion = gameObject2.AddOrGet<UIMinion>();
			this.minion.TrySpawn();
			gameObject2.SetActive(false);
			flag = true;
		}
		if (flag)
		{
			this.SetAsMannequin();
		}
		return flag;
	}

	// Token: 0x0600AFAD RID: 44973 RVA: 0x0011224F File Offset: 0x0011044F
	public UIMinionOrMannequin.ITarget SetFrom(Option<Personality> personality)
	{
		if (personality.IsSome())
		{
			return this.SetAsMinion(personality.Unwrap());
		}
		return this.SetAsMannequin();
	}

	// Token: 0x0600AFAE RID: 44974 RVA: 0x00421E48 File Offset: 0x00420048
	public UIMinion SetAsMinion(Personality personality)
	{
		this.mannequin.gameObject.SetActive(false);
		this.minion.gameObject.SetActive(true);
		this.minion.SetMinion(personality);
		this.current = this.minion;
		return this.minion;
	}

	// Token: 0x0600AFAF RID: 44975 RVA: 0x0011226E File Offset: 0x0011046E
	public UIMannequin SetAsMannequin()
	{
		this.minion.gameObject.SetActive(false);
		this.mannequin.gameObject.SetActive(true);
		this.current = this.mannequin;
		return this.mannequin;
	}

	// Token: 0x0600AFB0 RID: 44976 RVA: 0x00421E98 File Offset: 0x00420098
	public MinionVoice GetMinionVoice()
	{
		return MinionVoice.ByObject(this.current.SpawnedAvatar).UnwrapOr(MinionVoice.Random(), null);
	}

	// Token: 0x04008A82 RID: 35458
	public UIMinion minion;

	// Token: 0x04008A83 RID: 35459
	public UIMannequin mannequin;

	// Token: 0x0200203E RID: 8254
	public interface ITarget
	{
		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x0600AFB2 RID: 44978
		GameObject SpawnedAvatar { get; }

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x0600AFB3 RID: 44979
		Option<Personality> Personality { get; }

		// Token: 0x0600AFB4 RID: 44980
		void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> clothingItems);

		// Token: 0x0600AFB5 RID: 44981
		void React(UIMinionOrMannequinReactSource source);
	}
}
