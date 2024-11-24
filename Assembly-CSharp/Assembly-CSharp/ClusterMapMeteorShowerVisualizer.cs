﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class ClusterMapMeteorShowerVisualizer : ClusterGridEntity
{
		public override string Name
	{
		get
		{
			return this.p_name;
		}
	}

		public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Craft;
		}
	}

		public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

		public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

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

		public ClusterRevealLevel clusterCellRevealLevel
	{
		get
		{
			return ClusterGrid.Instance.GetCellRevealLevel(base.Location);
		}
	}

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

	public void SetInitialLocation(AxialI startLocation)
	{
		this.m_location = startLocation;
		this.RefreshVisuals();
	}

	public override bool SpaceOutInSameHex()
	{
		return true;
	}

	public override bool KeepRotationWhenSpacingOutInHex()
	{
		return true;
	}

	public override bool ShowPath()
	{
		return this.m_selectable.IsSelected;
	}

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

	public void Deselect()
	{
		if (this.m_selectable.IsSelected)
		{
			this.m_selectable.Unselect();
		}
	}

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

	public void PlayHideAnimation()
	{
		this.revealed = false;
		if (ClusterMapScreen.Instance.GetEntityVisAnim(this) != null)
		{
			this.RefreshVisuals();
		}
	}

	private ClusterGridEntity.AnimConfig questionMarkAnimConfig = new ClusterGridEntity.AnimConfig
	{
		animFile = Assets.GetAnim("shower_question_mark_kanim"),
		initialAnim = "idle",
		playMode = KAnim.PlayMode.Once
	};

	public string p_name;

	public string clusterAnimName;

	public bool revealed;
}
