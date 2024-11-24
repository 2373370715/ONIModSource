using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02001971 RID: 6513
public class ClusterMapVisualizer : KMonoBehaviour
{
	// Token: 0x060087CC RID: 34764 RVA: 0x00351A90 File Offset: 0x0034FC90
	public void Init(ClusterGridEntity entity, ClusterMapPathDrawer pathDrawer)
	{
		this.entity = entity;
		this.pathDrawer = pathDrawer;
		this.animControllers = new List<KBatchedAnimController>();
		if (this.animContainer == null)
		{
			GameObject gameObject = new GameObject("AnimContainer", new Type[]
			{
				typeof(RectTransform)
			});
			RectTransform component = base.GetComponent<RectTransform>();
			RectTransform component2 = gameObject.GetComponent<RectTransform>();
			component2.SetParent(component, false);
			component2.SetLocalPosition(new Vector3(0f, 0f, 0f));
			component2.sizeDelta = component.sizeDelta;
			component2.localScale = Vector3.one;
			this.animContainer = component2;
		}
		Vector3 position = ClusterGrid.Instance.GetPosition(entity);
		this.rectTransform().SetLocalPosition(position);
		this.RefreshPathDrawing();
		entity.Subscribe(543433792, new Action<object>(this.OnClusterDestinationChanged));
	}

	// Token: 0x060087CD RID: 34765 RVA: 0x000F8CF9 File Offset: 0x000F6EF9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.doesTransitionAnimation)
		{
			new ClusterMapTravelAnimator.StatesInstance(this, this.entity).StartSM();
		}
	}

	// Token: 0x060087CE RID: 34766 RVA: 0x00351B68 File Offset: 0x0034FD68
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.entity != null)
		{
			if (this.doesTransitionAnimation)
			{
				base.gameObject.GetSMI<ClusterMapTravelAnimator.StatesInstance>().keepRotationOnIdle = this.entity.KeepRotationWhenSpacingOutInHex();
			}
			if (this.entity is Clustercraft)
			{
				new ClusterMapRocketAnimator.StatesInstance(this, this.entity).StartSM();
				return;
			}
			if (this.entity is BallisticClusterGridEntity)
			{
				new ClusterMapBallisticAnimator.StatesInstance(this, this.entity).StartSM();
				return;
			}
			if (this.entity.Layer == EntityLayer.FX)
			{
				new ClusterMapFXAnimator.StatesInstance(this, this.entity).StartSM();
			}
		}
	}

	// Token: 0x060087CF RID: 34767 RVA: 0x000F8D1A File Offset: 0x000F6F1A
	protected override void OnCleanUp()
	{
		if (this.entity != null)
		{
			this.entity.Unsubscribe(543433792, new Action<object>(this.OnClusterDestinationChanged));
		}
		base.OnCleanUp();
	}

	// Token: 0x060087D0 RID: 34768 RVA: 0x000F8D4C File Offset: 0x000F6F4C
	private void OnClusterDestinationChanged(object data)
	{
		this.RefreshPathDrawing();
	}

	// Token: 0x060087D1 RID: 34769 RVA: 0x00351C0C File Offset: 0x0034FE0C
	public void Select(bool selected)
	{
		if (this.animControllers == null || this.animControllers.Count == 0)
		{
			return;
		}
		if (!selected == this.isSelected)
		{
			this.isSelected = selected;
			this.RefreshPathDrawing();
		}
		this.GetFirstAnimController().SetSymbolVisiblity("selected", selected);
	}

	// Token: 0x060087D2 RID: 34770 RVA: 0x000F8D54 File Offset: 0x000F6F54
	public void PlayAnim(string animName, KAnim.PlayMode playMode)
	{
		if (this.animControllers.Count > 0)
		{
			this.GetFirstAnimController().Play(animName, playMode, 1f, 0f);
		}
	}

	// Token: 0x060087D3 RID: 34771 RVA: 0x000F8D80 File Offset: 0x000F6F80
	public KBatchedAnimController GetFirstAnimController()
	{
		return this.GetAnimController(0);
	}

	// Token: 0x060087D4 RID: 34772 RVA: 0x000F8D89 File Offset: 0x000F6F89
	public KBatchedAnimController GetAnimController(int index)
	{
		if (index < this.animControllers.Count)
		{
			return this.animControllers[index];
		}
		return null;
	}

	// Token: 0x060087D5 RID: 34773 RVA: 0x000F8DA7 File Offset: 0x000F6FA7
	public void ManualAddAnimController(KBatchedAnimController externalAnimController)
	{
		this.animControllers.Add(externalAnimController);
	}

	// Token: 0x060087D6 RID: 34774 RVA: 0x00351C60 File Offset: 0x0034FE60
	public void Show(ClusterRevealLevel level)
	{
		if (!this.entity.IsVisible)
		{
			level = ClusterRevealLevel.Hidden;
		}
		if (level == this.lastRevealLevel)
		{
			return;
		}
		this.lastRevealLevel = level;
		switch (level)
		{
		case ClusterRevealLevel.Hidden:
			base.gameObject.SetActive(false);
			break;
		case ClusterRevealLevel.Peeked:
		{
			this.ClearAnimControllers();
			KBatchedAnimController kbatchedAnimController = UnityEngine.Object.Instantiate<KBatchedAnimController>(this.peekControllerPrefab, this.animContainer);
			kbatchedAnimController.gameObject.SetActive(true);
			this.animControllers.Add(kbatchedAnimController);
			base.gameObject.SetActive(true);
			break;
		}
		case ClusterRevealLevel.Visible:
			this.ClearAnimControllers();
			if (this.animControllerPrefab != null && this.entity.AnimConfigs != null)
			{
				foreach (ClusterGridEntity.AnimConfig animConfig in this.entity.AnimConfigs)
				{
					KBatchedAnimController kbatchedAnimController2 = UnityEngine.Object.Instantiate<KBatchedAnimController>(this.animControllerPrefab, this.animContainer);
					kbatchedAnimController2.AnimFiles = new KAnimFile[]
					{
						animConfig.animFile
					};
					kbatchedAnimController2.initialMode = animConfig.playMode;
					kbatchedAnimController2.initialAnim = animConfig.initialAnim;
					kbatchedAnimController2.Offset = animConfig.animOffset;
					kbatchedAnimController2.gameObject.AddComponent<LoopingSounds>();
					if (animConfig.animPlaySpeedModifier != 0f)
					{
						kbatchedAnimController2.PlaySpeedMultiplier = animConfig.animPlaySpeedModifier;
					}
					if (!string.IsNullOrEmpty(animConfig.symbolSwapTarget) && !string.IsNullOrEmpty(animConfig.symbolSwapSymbol))
					{
						SymbolOverrideController component = kbatchedAnimController2.GetComponent<SymbolOverrideController>();
						KAnim.Build.Symbol symbol = kbatchedAnimController2.AnimFiles[0].GetData().build.GetSymbol(animConfig.symbolSwapSymbol);
						component.AddSymbolOverride(animConfig.symbolSwapTarget, symbol, 0);
					}
					kbatchedAnimController2.gameObject.SetActive(true);
					this.animControllers.Add(kbatchedAnimController2);
				}
			}
			base.gameObject.SetActive(true);
			break;
		}
		this.entity.OnClusterMapIconShown(level);
	}

	// Token: 0x060087D7 RID: 34775 RVA: 0x00351E60 File Offset: 0x00350060
	public void RefreshPathDrawing()
	{
		if (this.entity == null)
		{
			return;
		}
		ClusterTraveler component = this.entity.GetComponent<ClusterTraveler>();
		if (component == null)
		{
			return;
		}
		List<AxialI> list = (this.entity.IsVisible && component.IsTraveling()) ? component.CurrentPath : null;
		if (list != null && list.Count > 0)
		{
			if (this.mapPath == null)
			{
				this.mapPath = this.pathDrawer.AddPath();
			}
			this.mapPath.SetPoints(ClusterMapPathDrawer.GetDrawPathList(base.transform.GetLocalPosition(), list));
			Color color;
			if (this.isSelected)
			{
				color = ClusterMapScreen.Instance.rocketSelectedPathColor;
			}
			else if (this.entity.ShowPath())
			{
				color = ClusterMapScreen.Instance.rocketPathColor;
			}
			else
			{
				color = new Color(0f, 0f, 0f, 0f);
			}
			this.mapPath.SetColor(color);
			return;
		}
		if (this.mapPath != null)
		{
			global::Util.KDestroyGameObject(this.mapPath);
			this.mapPath = null;
		}
	}

	// Token: 0x060087D8 RID: 34776 RVA: 0x000F8DB5 File Offset: 0x000F6FB5
	public void SetAnimRotation(float rotation)
	{
		this.animContainer.localRotation = Quaternion.Euler(0f, 0f, rotation);
	}

	// Token: 0x060087D9 RID: 34777 RVA: 0x000F8DD2 File Offset: 0x000F6FD2
	public float GetPathAngle()
	{
		if (this.mapPath == null)
		{
			return 0f;
		}
		return this.mapPath.GetRotationForNextSegment();
	}

	// Token: 0x060087DA RID: 34778 RVA: 0x00351F7C File Offset: 0x0035017C
	private void ClearAnimControllers()
	{
		if (this.animControllers == null)
		{
			return;
		}
		foreach (KBatchedAnimController kbatchedAnimController in this.animControllers)
		{
			global::Util.KDestroyGameObject(kbatchedAnimController.gameObject);
		}
		this.animControllers.Clear();
	}

	// Token: 0x04006669 RID: 26217
	public KBatchedAnimController animControllerPrefab;

	// Token: 0x0400666A RID: 26218
	public KBatchedAnimController peekControllerPrefab;

	// Token: 0x0400666B RID: 26219
	public Transform nameTarget;

	// Token: 0x0400666C RID: 26220
	public AlertVignette alertVignette;

	// Token: 0x0400666D RID: 26221
	public bool doesTransitionAnimation;

	// Token: 0x0400666E RID: 26222
	[HideInInspector]
	public Transform animContainer;

	// Token: 0x0400666F RID: 26223
	private ClusterGridEntity entity;

	// Token: 0x04006670 RID: 26224
	private ClusterMapPathDrawer pathDrawer;

	// Token: 0x04006671 RID: 26225
	private ClusterMapPath mapPath;

	// Token: 0x04006672 RID: 26226
	private List<KBatchedAnimController> animControllers;

	// Token: 0x04006673 RID: 26227
	private bool isSelected;

	// Token: 0x04006674 RID: 26228
	private ClusterRevealLevel lastRevealLevel;

	// Token: 0x02001972 RID: 6514
	private class UpdateXPositionParameter : LoopingSoundParameterUpdater
	{
		// Token: 0x060087DC RID: 34780 RVA: 0x000F8DF3 File Offset: 0x000F6FF3
		public UpdateXPositionParameter() : base("Starmap_Position_X")
		{
		}

		// Token: 0x060087DD RID: 34781 RVA: 0x00351FE8 File Offset: 0x003501E8
		public override void Add(LoopingSoundParameterUpdater.Sound sound)
		{
			ClusterMapVisualizer.UpdateXPositionParameter.Entry item = new ClusterMapVisualizer.UpdateXPositionParameter.Entry
			{
				transform = sound.transform,
				ev = sound.ev,
				parameterId = sound.description.GetParameterId(base.parameter)
			};
			this.entries.Add(item);
		}

		// Token: 0x060087DE RID: 34782 RVA: 0x00352040 File Offset: 0x00350240
		public override void Update(float dt)
		{
			foreach (ClusterMapVisualizer.UpdateXPositionParameter.Entry entry in this.entries)
			{
				if (!(entry.transform == null))
				{
					EventInstance ev = entry.ev;
					ev.setParameterByID(entry.parameterId, entry.transform.GetPosition().x / (float)Screen.width, false);
				}
			}
		}

		// Token: 0x060087DF RID: 34783 RVA: 0x003520C8 File Offset: 0x003502C8
		public override void Remove(LoopingSoundParameterUpdater.Sound sound)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].ev.handle == sound.ev.handle)
				{
					this.entries.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x04006675 RID: 26229
		private List<ClusterMapVisualizer.UpdateXPositionParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateXPositionParameter.Entry>();

		// Token: 0x02001973 RID: 6515
		private struct Entry
		{
			// Token: 0x04006676 RID: 26230
			public Transform transform;

			// Token: 0x04006677 RID: 26231
			public EventInstance ev;

			// Token: 0x04006678 RID: 26232
			public PARAMETER_ID parameterId;
		}
	}

	// Token: 0x02001974 RID: 6516
	private class UpdateYPositionParameter : LoopingSoundParameterUpdater
	{
		// Token: 0x060087E0 RID: 34784 RVA: 0x000F8E10 File Offset: 0x000F7010
		public UpdateYPositionParameter() : base("Starmap_Position_Y")
		{
		}

		// Token: 0x060087E1 RID: 34785 RVA: 0x00352120 File Offset: 0x00350320
		public override void Add(LoopingSoundParameterUpdater.Sound sound)
		{
			ClusterMapVisualizer.UpdateYPositionParameter.Entry item = new ClusterMapVisualizer.UpdateYPositionParameter.Entry
			{
				transform = sound.transform,
				ev = sound.ev,
				parameterId = sound.description.GetParameterId(base.parameter)
			};
			this.entries.Add(item);
		}

		// Token: 0x060087E2 RID: 34786 RVA: 0x00352178 File Offset: 0x00350378
		public override void Update(float dt)
		{
			foreach (ClusterMapVisualizer.UpdateYPositionParameter.Entry entry in this.entries)
			{
				if (!(entry.transform == null))
				{
					EventInstance ev = entry.ev;
					ev.setParameterByID(entry.parameterId, entry.transform.GetPosition().y / (float)Screen.height, false);
				}
			}
		}

		// Token: 0x060087E3 RID: 34787 RVA: 0x00352200 File Offset: 0x00350400
		public override void Remove(LoopingSoundParameterUpdater.Sound sound)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].ev.handle == sound.ev.handle)
				{
					this.entries.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x04006679 RID: 26233
		private List<ClusterMapVisualizer.UpdateYPositionParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateYPositionParameter.Entry>();

		// Token: 0x02001975 RID: 6517
		private struct Entry
		{
			// Token: 0x0400667A RID: 26234
			public Transform transform;

			// Token: 0x0400667B RID: 26235
			public EventInstance ev;

			// Token: 0x0400667C RID: 26236
			public PARAMETER_ID parameterId;
		}
	}

	// Token: 0x02001976 RID: 6518
	private class UpdateZoomPercentageParameter : LoopingSoundParameterUpdater
	{
		// Token: 0x060087E4 RID: 34788 RVA: 0x000F8E2D File Offset: 0x000F702D
		public UpdateZoomPercentageParameter() : base("Starmap_Zoom_Percentage")
		{
		}

		// Token: 0x060087E5 RID: 34789 RVA: 0x00352258 File Offset: 0x00350458
		public override void Add(LoopingSoundParameterUpdater.Sound sound)
		{
			ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry item = new ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry
			{
				ev = sound.ev,
				parameterId = sound.description.GetParameterId(base.parameter)
			};
			this.entries.Add(item);
		}

		// Token: 0x060087E6 RID: 34790 RVA: 0x003522A4 File Offset: 0x003504A4
		public override void Update(float dt)
		{
			foreach (ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry entry in this.entries)
			{
				EventInstance ev = entry.ev;
				ev.setParameterByID(entry.parameterId, ClusterMapScreen.Instance.CurrentZoomPercentage(), false);
			}
		}

		// Token: 0x060087E7 RID: 34791 RVA: 0x00352310 File Offset: 0x00350510
		public override void Remove(LoopingSoundParameterUpdater.Sound sound)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].ev.handle == sound.ev.handle)
				{
					this.entries.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0400667D RID: 26237
		private List<ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry>();

		// Token: 0x02001977 RID: 6519
		private struct Entry
		{
			// Token: 0x0400667E RID: 26238
			public Transform transform;

			// Token: 0x0400667F RID: 26239
			public EventInstance ev;

			// Token: 0x04006680 RID: 26240
			public PARAMETER_ID parameterId;
		}
	}
}
