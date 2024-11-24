using System.Collections.Generic;
using Database;
using UnityEngine;
using UnityEngine.UI;

public class UIMinionOrMannequin : KMonoBehaviour
{
	public interface ITarget
	{
		GameObject SpawnedAvatar { get; }

		Option<Personality> Personality { get; }

		void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> clothingItems);

		void React(UIMinionOrMannequinReactSource source);
	}

	public UIMinion minion;

	public UIMannequin mannequin;

	public ITarget current { get; private set; }

	protected override void OnSpawn()
	{
		TrySpawn();
	}

	public bool TrySpawn()
	{
		bool flag = false;
		if (mannequin.IsNullOrDestroyed())
		{
			GameObject gameObject = new GameObject("UIMannequin");
			gameObject.AddOrGet<RectTransform>().Fill(Padding.All(10f));
			gameObject.transform.SetParent(base.transform, worldPositionStays: false);
			AspectRatioFitter aspectRatioFitter = gameObject.AddOrGet<AspectRatioFitter>();
			aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
			aspectRatioFitter.aspectRatio = 1f;
			mannequin = gameObject.AddOrGet<UIMannequin>();
			mannequin.TrySpawn();
			gameObject.SetActive(value: false);
			flag = true;
		}
		if (minion.IsNullOrDestroyed())
		{
			GameObject gameObject2 = new GameObject("UIMinion");
			gameObject2.AddOrGet<RectTransform>().Fill(Padding.All(10f));
			gameObject2.transform.SetParent(base.transform, worldPositionStays: false);
			AspectRatioFitter aspectRatioFitter2 = gameObject2.AddOrGet<AspectRatioFitter>();
			aspectRatioFitter2.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
			aspectRatioFitter2.aspectRatio = 1f;
			minion = gameObject2.AddOrGet<UIMinion>();
			minion.TrySpawn();
			gameObject2.SetActive(value: false);
			flag = true;
		}
		if (flag)
		{
			SetAsMannequin();
		}
		return flag;
	}

	public ITarget SetFrom(Option<Personality> personality)
	{
		if (personality.IsSome())
		{
			return SetAsMinion(personality.Unwrap());
		}
		return SetAsMannequin();
	}

	public UIMinion SetAsMinion(Personality personality)
	{
		mannequin.gameObject.SetActive(value: false);
		minion.gameObject.SetActive(value: true);
		minion.SetMinion(personality);
		current = minion;
		return minion;
	}

	public UIMannequin SetAsMannequin()
	{
		minion.gameObject.SetActive(value: false);
		mannequin.gameObject.SetActive(value: true);
		current = mannequin;
		return mannequin;
	}

	public MinionVoice GetMinionVoice()
	{
		return MinionVoice.ByObject(current.SpawnedAvatar).UnwrapOr(MinionVoice.Random());
	}
}
