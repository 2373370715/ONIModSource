using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020018CB RID: 6347
public class ClusterMapMeteorShowerVisualizer : ClusterGridEntity
{
	// Token: 0x17000878 RID: 2168
	// (get) Token: 0x0600838D RID: 33677 RVA: 0x000F6738 File Offset: 0x000F4938
	public override string Name
	{
		get
		{
			return this.p_name;
		}
	}

	// Token: 0x17000879 RID: 2169
	// (get) Token: 0x0600838E RID: 33678 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Craft;
		}
	}

	// Token: 0x1700087A RID: 2170
	// (get) Token: 0x0600838F RID: 33679 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700087B RID: 2171
	// (get) Token: 0x06008390 RID: 33680 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x1700087C RID: 2172
	// (get) Token: 0x06008391 RID: 33681 RVA: 0x003402D0 File Offset: 0x0033E4D0
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim(this.clusterAnimName),
					initialAnim = this.AnimName,
					animPlaySpeedModifier = 0.5f
				},
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("shower_identify_kanim"),
					initialAnim = "identify_off",
					playMode = KAnim.PlayMode.Once
				},
				this.questionMarkAnimConfig
			};
		}
	}

	// Token: 0x1700087D RID: 2173
	// (get) Token: 0x06008392 RID: 33682 RVA: 0x000F6740 File Offset: 0x000F4940
	public ClusterRevealLevel clusterCellRevealLevel
	{
		get
		{
			return ClusterGrid.Instance.GetCellRevealLevel(base.Location);
		}
	}

	// Token: 0x1700087E RID: 2174
	// (get) Token: 0x06008393 RID: 33683 RVA: 0x000F6752 File Offset: 0x000F4952
	public string AnimName
	{
		get
		{
			if (!this.revealed || this.clusterCellRevealLevel != ClusterRevealLevel.Visible)
			{
				return "unknown";
			}
			return "idle_loop";
		}
	}

	// Token: 0x1700087F RID: 2175
	// (get) Token: 0x06008394 RID: 33684 RVA: 0x000F6770 File Offset: 0x000F4970
	public string QuestionMarkAnimName
	{
		get
		{
			if (!this.revealed || this.clusterCellRevealLevel != ClusterRevealLevel.Visible)
			{
				return this.questionMarkAnimConfig.initialAnim;
			}
			return "off";
		}
	}

	// Token: 0x06008395 RID: 33685 RVA: 0x00340368 File Offset: 0x0033E568
	public KBatchedAnimController CreateQuestionMarkInstance(KBatchedAnimController origin, Transform parent)
	{
		KBatchedAnimController kbatchedAnimController = UnityEngine.Object.Instantiate<KBatchedAnimController>(origin, parent);
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController.SwapAnims(new KAnimFile[]
		{
			this.questionMarkAnimConfig.animFile
		});
		kbatchedAnimController.Play(this.QuestionMarkAnimName, KAnim.PlayMode.Once, 1f, 0f);
		kbatchedAnimController.gameObject.AddOrGet<ClusterMapIconFixRotation>();
		return kbatchedAnimController;
	}

	// Token: 0x06008396 RID: 33686 RVA: 0x003403CC File Offset: 0x0033E5CC
	protected override void OnCleanUp()
	{
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
			if (entityVisAnim != null)
			{
				entityVisAnim.gameObject.SetActive(false);
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x06008397 RID: 33687 RVA: 0x000F6794 File Offset: 0x000F4994
	public void SetInitialLocation(AxialI startLocation)
	{
		this.m_location = startLocation;
		this.RefreshVisuals();
	}

	// Token: 0x06008398 RID: 33688 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool SpaceOutInSameHex()
	{
		return true;
	}

	// Token: 0x06008399 RID: 33689 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool KeepRotationWhenSpacingOutInHex()
	{
		return true;
	}

	// Token: 0x0600839A RID: 33690 RVA: 0x000AE329 File Offset: 0x000AC529
	public override bool ShowPath()
	{
		return this.m_selectable.IsSelected;
	}

	// Token: 0x0600839B RID: 33691 RVA: 0x00340410 File Offset: 0x0033E610
	public override void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
		ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		switch (levelUsed)
		{
		case ClusterRevealLevel.Hidden:
			this.Deselect();
			break;
		case ClusterRevealLevel.Peeked:
		{
			KBatchedAnimController firstAnimController = entityVisAnim.GetFirstAnimController();
			if (firstAnimController != null)
			{
				firstAnimController.SwapAnims(new KAnimFile[]
				{
					this.AnimConfigs[0].animFile
				});
				KBatchedAnimController externalAnimController = this.CreateQuestionMarkInstance(entityVisAnim.peekControllerPrefab, firstAnimController.transform.parent);
				entityVisAnim.ManualAddAnimController(externalAnimController);
			}
			this.RefreshVisuals();
			this.Deselect();
			break;
		}
		case ClusterRevealLevel.Visible:
			this.RefreshVisuals();
			break;
		}
		KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
		if (animController != null && !this.revealed)
		{
			animController.gameObject.AddOrGet<ClusterMapIconFixRotation>();
		}
	}

	// Token: 0x0600839C RID: 33692 RVA: 0x000F67A3 File Offset: 0x000F49A3
	public void Deselect()
	{
		if (this.m_selectable.IsSelected)
		{
			this.m_selectable.Unselect();
		}
	}

	// Token: 0x0600839D RID: 33693 RVA: 0x003404D0 File Offset: 0x0033E6D0
	public void RefreshVisuals()
	{
		ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		if (entityVisAnim != null)
		{
			KBatchedAnimController firstAnimController = entityVisAnim.GetFirstAnimController();
			if (firstAnimController != null)
			{
				firstAnimController.Play(this.AnimName, KAnim.PlayMode.Loop, 1f, 0f);
			}
			KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
			if (animController != null)
			{
				animController.Play(this.QuestionMarkAnimName, KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x0600839E RID: 33694 RVA: 0x0034054C File Offset: 0x0033E74C
	public void PlayRevealAnimation(bool playIdentifyAnimationIfVisible)
	{
		this.revealed = true;
		this.RefreshVisuals();
		if (playIdentifyAnimationIfVisible)
		{
			ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
			KBatchedAnimController animController = entityVisAnim.GetAnimController(1);
			entityVisAnim.GetAnimController(2);
			if (animController != null)
			{
				animController.Play("identify", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x0600839F RID: 33695 RVA: 0x000F67BD File Offset: 0x000F49BD
	public void PlayHideAnimation()
	{
		this.revealed = false;
		if (ClusterMapScreen.Instance.GetEntityVisAnim(this) != null)
		{
			this.RefreshVisuals();
		}
	}

	// Token: 0x040063CB RID: 25547
	private ClusterGridEntity.AnimConfig questionMarkAnimConfig = new ClusterGridEntity.AnimConfig
	{
		animFile = Assets.GetAnim("shower_question_mark_kanim"),
		initialAnim = "idle",
		playMode = KAnim.PlayMode.Once
	};

	// Token: 0x040063CC RID: 25548
	public string p_name;

	// Token: 0x040063CD RID: 25549
	public string clusterAnimName;

	// Token: 0x040063CE RID: 25550
	public bool revealed;
}
