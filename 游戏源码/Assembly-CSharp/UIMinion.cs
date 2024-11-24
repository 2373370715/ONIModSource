using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x0200203B RID: 8251
public class UIMinion : KMonoBehaviour, UIMinionOrMannequin.ITarget
{
	// Token: 0x17000B30 RID: 2864
	// (get) Token: 0x0600AF9F RID: 44959 RVA: 0x001121C6 File Offset: 0x001103C6
	public GameObject SpawnedAvatar
	{
		get
		{
			if (this.spawn == null)
			{
				this.TrySpawn();
			}
			return this.spawn;
		}
	}

	// Token: 0x17000B31 RID: 2865
	// (get) Token: 0x0600AFA0 RID: 44960 RVA: 0x001121E2 File Offset: 0x001103E2
	// (set) Token: 0x0600AFA1 RID: 44961 RVA: 0x001121EA File Offset: 0x001103EA
	public Option<Personality> Personality { get; private set; }

	// Token: 0x0600AFA2 RID: 44962 RVA: 0x001121F3 File Offset: 0x001103F3
	protected override void OnSpawn()
	{
		this.TrySpawn();
	}

	// Token: 0x0600AFA3 RID: 44963 RVA: 0x00421A6C File Offset: 0x0041FC6C
	public void TrySpawn()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(MinionUIPortrait.ID), base.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = 0.38f;
			this.animController.Play("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
			BaseMinionConfig.ConfigureSymbols(this.animController.gameObject, true);
			this.spawn = this.animController.gameObject;
		}
	}

	// Token: 0x0600AFA4 RID: 44964 RVA: 0x001121FB File Offset: 0x001103FB
	public void SetMinion(Personality personality)
	{
		this.SpawnedAvatar.GetComponent<Accessorizer>().ApplyMinionPersonality(personality);
		this.Personality = personality;
		base.gameObject.AddOrGet<MinionVoiceProviderMB>().voice = MinionVoice.ByPersonality(personality);
	}

	// Token: 0x0600AFA5 RID: 44965 RVA: 0x00421B14 File Offset: 0x0041FD14
	public void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> outfit)
	{
		outfit = UIMinionOrMannequinITargetExtensions.GetOutfitWithDefaultItems(outfitType, outfit);
		WearableAccessorizer component = this.SpawnedAvatar.GetComponent<WearableAccessorizer>();
		component.ClearClothingItems(null);
		component.ApplyClothingItems(outfitType, outfit);
	}

	// Token: 0x0600AFA6 RID: 44966 RVA: 0x00421B4C File Offset: 0x0041FD4C
	public MinionVoice GetMinionVoice()
	{
		return MinionVoice.ByObject(this.SpawnedAvatar).UnwrapOr(MinionVoice.Random(), null);
	}

	// Token: 0x0600AFA7 RID: 44967 RVA: 0x00421B74 File Offset: 0x0041FD74
	public void React(UIMinionOrMannequinReactSource source)
	{
		if (source != UIMinionOrMannequinReactSource.OnPersonalityChanged && this.lastReactSource == source)
		{
			KAnim.Anim currentAnim = this.animController.GetCurrentAnim();
			if (currentAnim != null && currentAnim.name != "idle_default")
			{
				return;
			}
		}
		switch (source)
		{
		case UIMinionOrMannequinReactSource.OnPersonalityChanged:
			this.animController.Play("react", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnWholeOutfitChanged:
		case UIMinionOrMannequinReactSource.OnBottomChanged:
			this.animController.Play("react_bottoms", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnHatChanged:
			this.animController.Play("react_glasses", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnTopChanged:
			this.animController.Play("react_tops", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnGlovesChanged:
			this.animController.Play("react_gloves", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnShoesChanged:
			this.animController.Play("react_shoes", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		}
		this.animController.Play("cheer_pre", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Queue("cheer_loop", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Queue("cheer_pst", KAnim.PlayMode.Once, 1f, 0f);
		IL_195:
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
		this.lastReactSource = source;
	}

	// Token: 0x04008A74 RID: 35444
	public const float ANIM_SCALE = 0.38f;

	// Token: 0x04008A75 RID: 35445
	private KBatchedAnimController animController;

	// Token: 0x04008A76 RID: 35446
	private GameObject spawn;

	// Token: 0x04008A78 RID: 35448
	private UIMinionOrMannequinReactSource lastReactSource;
}
