using System.Collections.Generic;
using Database;
using UnityEngine;

public class UIMinion : KMonoBehaviour, UIMinionOrMannequin.ITarget
{
	public const float ANIM_SCALE = 0.38f;

	private KBatchedAnimController animController;

	private GameObject spawn;

	private UIMinionOrMannequinReactSource lastReactSource;

	public GameObject SpawnedAvatar
	{
		get
		{
			if (spawn == null)
			{
				TrySpawn();
			}
			return spawn;
		}
	}

	public Option<Personality> Personality { get; private set; }

	protected override void OnSpawn()
	{
		TrySpawn();
	}

	public void TrySpawn()
	{
		if (animController == null)
		{
			animController = Util.KInstantiateUI(Assets.GetPrefab(MinionUIPortrait.ID), base.gameObject).GetComponent<KBatchedAnimController>();
			animController.gameObject.SetActive(value: true);
			animController.animScale = 0.38f;
			animController.Play("idle_default", KAnim.PlayMode.Loop);
			MinionConfig.ConfigureSymbols(animController.gameObject);
			spawn = animController.gameObject;
		}
	}

	public void SetMinion(Personality personality)
	{
		SpawnedAvatar.GetComponent<Accessorizer>().ApplyMinionPersonality(personality);
		Personality = personality;
		base.gameObject.AddOrGet<MinionVoiceProviderMB>().voice = MinionVoice.ByPersonality(personality);
	}

	public void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> outfit)
	{
		outfit = UIMinionOrMannequinITargetExtensions.GetOutfitWithDefaultItems(outfitType, outfit);
		WearableAccessorizer component = SpawnedAvatar.GetComponent<WearableAccessorizer>();
		component.ClearClothingItems();
		component.ApplyClothingItems(outfitType, outfit);
	}

	public MinionVoice GetMinionVoice()
	{
		return MinionVoice.ByObject(SpawnedAvatar).UnwrapOr(MinionVoice.Random());
	}

	public void React(UIMinionOrMannequinReactSource source)
	{
		if (source != UIMinionOrMannequinReactSource.OnPersonalityChanged && lastReactSource == source)
		{
			KAnim.Anim currentAnim = animController.GetCurrentAnim();
			if (currentAnim != null && currentAnim.name != "idle_default")
			{
				return;
			}
		}
		switch (source)
		{
		case UIMinionOrMannequinReactSource.OnHatChanged:
			animController.Play("react_glasses");
			break;
		case UIMinionOrMannequinReactSource.OnTopChanged:
			animController.Play("react_tops");
			break;
		case UIMinionOrMannequinReactSource.OnGlovesChanged:
			animController.Play("react_gloves");
			break;
		case UIMinionOrMannequinReactSource.OnWholeOutfitChanged:
		case UIMinionOrMannequinReactSource.OnBottomChanged:
			animController.Play("react_bottoms");
			break;
		case UIMinionOrMannequinReactSource.OnShoesChanged:
			animController.Play("react_shoes");
			break;
		case UIMinionOrMannequinReactSource.OnPersonalityChanged:
			animController.Play("react");
			break;
		default:
			animController.Play("cheer_pre");
			animController.Queue("cheer_loop");
			animController.Queue("cheer_pst");
			break;
		}
		animController.Queue("idle_default", KAnim.PlayMode.Loop);
		lastReactSource = source;
	}
}
