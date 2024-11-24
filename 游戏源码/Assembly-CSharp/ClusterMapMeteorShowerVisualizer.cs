using System.Collections.Generic;
using UnityEngine;

public class ClusterMapMeteorShowerVisualizer : ClusterGridEntity
{
	private AnimConfig questionMarkAnimConfig = new AnimConfig
	{
		animFile = Assets.GetAnim("shower_question_mark_kanim"),
		initialAnim = "idle",
		playMode = KAnim.PlayMode.Once
	};

	public string p_name;

	public string clusterAnimName;

	public bool revealed;

	public override string Name => p_name;

	public override EntityLayer Layer => EntityLayer.Craft;

	public override bool IsVisible => true;

	public override ClusterRevealLevel IsVisibleInFOW => ClusterRevealLevel.Peeked;

	public override List<AnimConfig> AnimConfigs => new List<AnimConfig>
	{
		new AnimConfig
		{
			animFile = Assets.GetAnim(clusterAnimName),
			initialAnim = AnimName,
			animPlaySpeedModifier = 0.5f
		},
		new AnimConfig
		{
			animFile = Assets.GetAnim("shower_identify_kanim"),
			initialAnim = "identify_off",
			playMode = KAnim.PlayMode.Once
		},
		questionMarkAnimConfig
	};

	public ClusterRevealLevel clusterCellRevealLevel => ClusterGrid.Instance.GetCellRevealLevel(base.Location);

	public string AnimName
	{
		get
		{
			if (!revealed || clusterCellRevealLevel != ClusterRevealLevel.Visible)
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
			if (!revealed || clusterCellRevealLevel != ClusterRevealLevel.Visible)
			{
				return questionMarkAnimConfig.initialAnim;
			}
			return "off";
		}
	}

	public KBatchedAnimController CreateQuestionMarkInstance(KBatchedAnimController origin, Transform parent)
	{
		KBatchedAnimController kBatchedAnimController = Object.Instantiate(origin, parent);
		kBatchedAnimController.gameObject.SetActive(value: true);
		kBatchedAnimController.SwapAnims(new KAnimFile[1] { questionMarkAnimConfig.animFile });
		kBatchedAnimController.Play(QuestionMarkAnimName);
		kBatchedAnimController.gameObject.AddOrGet<ClusterMapIconFixRotation>();
		return kBatchedAnimController;
	}

	protected override void OnCleanUp()
	{
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
			if (entityVisAnim != null)
			{
				entityVisAnim.gameObject.SetActive(value: false);
			}
		}
		base.OnCleanUp();
	}

	public void SetInitialLocation(AxialI startLocation)
	{
		m_location = startLocation;
		RefreshVisuals();
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
		return m_selectable.IsSelected;
	}

	public override void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
		ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		switch (levelUsed)
		{
		case ClusterRevealLevel.Hidden:
			Deselect();
			break;
		case ClusterRevealLevel.Peeked:
		{
			KBatchedAnimController firstAnimController = entityVisAnim.GetFirstAnimController();
			if (firstAnimController != null)
			{
				firstAnimController.SwapAnims(new KAnimFile[1] { AnimConfigs[0].animFile });
				KBatchedAnimController externalAnimController = CreateQuestionMarkInstance(entityVisAnim.peekControllerPrefab, firstAnimController.transform.parent);
				entityVisAnim.ManualAddAnimController(externalAnimController);
			}
			RefreshVisuals();
			Deselect();
			break;
		}
		case ClusterRevealLevel.Visible:
			RefreshVisuals();
			break;
		}
		KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
		if (animController != null && !revealed)
		{
			animController.gameObject.AddOrGet<ClusterMapIconFixRotation>();
		}
	}

	public void Deselect()
	{
		if (m_selectable.IsSelected)
		{
			m_selectable.Unselect();
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
				firstAnimController.Play(AnimName, KAnim.PlayMode.Loop);
			}
			KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
			if (animController != null)
			{
				animController.Play(QuestionMarkAnimName);
			}
		}
	}

	public void PlayRevealAnimation(bool playIdentifyAnimationIfVisible)
	{
		revealed = true;
		RefreshVisuals();
		if (playIdentifyAnimationIfVisible)
		{
			ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
			KBatchedAnimController animController = entityVisAnim.GetAnimController(1);
			entityVisAnim.GetAnimController(2);
			if (animController != null)
			{
				animController.Play("identify");
			}
		}
	}

	public void PlayHideAnimation()
	{
		revealed = false;
		if (ClusterMapScreen.Instance.GetEntityVisAnim(this) != null)
		{
			RefreshVisuals();
		}
	}
}
