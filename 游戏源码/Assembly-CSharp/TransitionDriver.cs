using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B5B RID: 2907
public class TransitionDriver
{
	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06003715 RID: 14101 RVA: 0x000C3BCF File Offset: 0x000C1DCF
	public Navigator.ActiveTransition GetTransition
	{
		get
		{
			return this.transition;
		}
	}

	// Token: 0x06003716 RID: 14102 RVA: 0x000C3BD7 File Offset: 0x000C1DD7
	public TransitionDriver(Navigator navigator)
	{
		this.log = new LoggerFS("TransitionDriver", 35);
	}

	// Token: 0x06003717 RID: 14103 RVA: 0x00215D88 File Offset: 0x00213F88
	public void BeginTransition(Navigator navigator, NavGrid.Transition transition, float defaultSpeed)
	{
		Navigator.ActiveTransition instance = TransitionDriver.TransitionPool.GetInstance();
		instance.Init(transition, defaultSpeed);
		this.BeginTransition(navigator, instance);
	}

	// Token: 0x06003718 RID: 14104 RVA: 0x00215DB0 File Offset: 0x00213FB0
	private void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		bool flag = this.interruptOverrideStack.Count != 0;
		foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
		{
			if (!flag || !(overrideLayer is TransitionDriver.InterruptOverrideLayer))
			{
				overrideLayer.BeginTransition(navigator, transition);
			}
		}
		this.navigator = navigator;
		this.transition = transition;
		this.isComplete = false;
		Grid.SceneLayer sceneLayer = navigator.sceneLayer;
		if (transition.navGridTransition.start == NavType.Tube || transition.navGridTransition.end == NavType.Tube)
		{
			sceneLayer = Grid.SceneLayer.BuildingUse;
		}
		else if (transition.navGridTransition.start == NavType.Solid && transition.navGridTransition.end == NavType.Solid)
		{
			sceneLayer = Grid.SceneLayer.FXFront;
			navigator.animController.SetSceneLayer(sceneLayer);
		}
		else if (transition.navGridTransition.start == NavType.Solid || transition.navGridTransition.end == NavType.Solid)
		{
			navigator.animController.SetSceneLayer(sceneLayer);
		}
		int cell = Grid.OffsetCell(Grid.PosToCell(navigator), transition.x, transition.y);
		this.targetPos = Grid.CellToPosCBC(cell, sceneLayer);
		if (transition.isLooping)
		{
			KAnimControllerBase animController = navigator.animController;
			animController.PlaySpeedMultiplier = transition.animSpeed;
			bool flag2 = transition.preAnim != "";
			bool flag3 = animController.CurrentAnim != null && animController.CurrentAnim.name == transition.anim;
			if (flag2 && animController.CurrentAnim != null && animController.CurrentAnim.name == transition.preAnim)
			{
				animController.ClearQueue();
				animController.Queue(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
			else if (flag3)
			{
				if (animController.PlayMode != KAnim.PlayMode.Loop)
				{
					animController.ClearQueue();
					animController.Queue(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
				}
			}
			else if (flag2)
			{
				animController.Play(transition.preAnim, KAnim.PlayMode.Once, 1f, 0f);
				animController.Queue(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
			else
			{
				animController.Play(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}
		else if (transition.anim != null)
		{
			KBatchedAnimController animController2 = navigator.animController;
			animController2.PlaySpeedMultiplier = transition.animSpeed;
			animController2.Play(transition.anim, KAnim.PlayMode.Once, 1f, 0f);
			navigator.Subscribe(-1061186183, new Action<object>(this.OnAnimComplete));
		}
		if (transition.navGridTransition.y != 0)
		{
			if (transition.navGridTransition.start == NavType.RightWall)
			{
				navigator.facing.SetFacing(transition.navGridTransition.y < 0);
			}
			else if (transition.navGridTransition.start == NavType.LeftWall)
			{
				navigator.facing.SetFacing(transition.navGridTransition.y > 0);
			}
		}
		if (transition.navGridTransition.x != 0)
		{
			if (transition.navGridTransition.start == NavType.Ceiling)
			{
				navigator.facing.SetFacing(transition.navGridTransition.x > 0);
			}
			else if (transition.navGridTransition.start != NavType.LeftWall && transition.navGridTransition.start != NavType.RightWall)
			{
				navigator.facing.SetFacing(transition.navGridTransition.x < 0);
			}
		}
		this.brain = navigator.GetComponent<Brain>();
	}

	// Token: 0x06003719 RID: 14105 RVA: 0x0021613C File Offset: 0x0021433C
	public void UpdateTransition(float dt)
	{
		if (this.navigator == null)
		{
			return;
		}
		foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
		{
			bool flag = this.interruptOverrideStack.Count != 0;
			bool flag2 = overrideLayer is TransitionDriver.InterruptOverrideLayer;
			if (!flag || !flag2 || this.interruptOverrideStack.Peek() == overrideLayer)
			{
				overrideLayer.UpdateTransition(this.navigator, this.transition);
			}
		}
		if (!this.isComplete && this.transition.isCompleteCB != null)
		{
			this.isComplete = this.transition.isCompleteCB();
		}
		if (this.brain != null)
		{
			bool flag3 = this.isComplete;
		}
		if (this.transition.isLooping)
		{
			float speed = this.transition.speed;
			Vector3 position = this.navigator.transform.GetPosition();
			int num = Grid.PosToCell(position);
			if (this.transition.x > 0)
			{
				position.x += dt * speed;
				if (position.x > this.targetPos.x)
				{
					this.isComplete = true;
				}
			}
			else if (this.transition.x < 0)
			{
				position.x -= dt * speed;
				if (position.x < this.targetPos.x)
				{
					this.isComplete = true;
				}
			}
			else
			{
				position.x = this.targetPos.x;
			}
			if (this.transition.y > 0)
			{
				position.y += dt * speed;
				if (position.y > this.targetPos.y)
				{
					this.isComplete = true;
				}
			}
			else if (this.transition.y < 0)
			{
				position.y -= dt * speed;
				if (position.y < this.targetPos.y)
				{
					this.isComplete = true;
				}
			}
			else
			{
				position.y = this.targetPos.y;
			}
			this.navigator.transform.SetPosition(position);
			int num2 = Grid.PosToCell(position);
			if (num2 != num)
			{
				this.navigator.Trigger(915392638, num2);
			}
		}
		if (this.isComplete)
		{
			this.isComplete = false;
			Navigator navigator = this.navigator;
			navigator.SetCurrentNavType(this.transition.end);
			navigator.transform.SetPosition(this.targetPos);
			this.EndTransition();
			navigator.AdvancePath(true);
		}
	}

	// Token: 0x0600371A RID: 14106 RVA: 0x002163E0 File Offset: 0x002145E0
	public void EndTransition()
	{
		if (this.navigator != null)
		{
			this.interruptOverrideStack.Clear();
			foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
			{
				overrideLayer.EndTransition(this.navigator, this.transition);
			}
			this.navigator.animController.PlaySpeedMultiplier = 1f;
			this.navigator.Unsubscribe(-1061186183, new Action<object>(this.OnAnimComplete));
			if (this.brain != null)
			{
				this.brain.Resume("move_handler");
			}
			TransitionDriver.TransitionPool.ReleaseInstance(this.transition);
			this.transition = null;
			this.navigator = null;
			this.brain = null;
		}
	}

	// Token: 0x0600371B RID: 14107 RVA: 0x000C3C08 File Offset: 0x000C1E08
	private void OnAnimComplete(object data)
	{
		if (this.navigator != null)
		{
			this.navigator.Unsubscribe(-1061186183, new Action<object>(this.OnAnimComplete));
		}
		this.isComplete = true;
	}

	// Token: 0x0600371C RID: 14108 RVA: 0x000C3C3B File Offset: 0x000C1E3B
	public static Navigator.ActiveTransition SwapTransitionWithEmpty(Navigator.ActiveTransition src)
	{
		Navigator.ActiveTransition instance = TransitionDriver.TransitionPool.GetInstance();
		instance.Copy(src);
		src.Copy(TransitionDriver.emptyTransition);
		return instance;
	}

	// Token: 0x0400254A RID: 9546
	private static Navigator.ActiveTransition emptyTransition = new Navigator.ActiveTransition();

	// Token: 0x0400254B RID: 9547
	public static ObjectPool<Navigator.ActiveTransition> TransitionPool = new ObjectPool<Navigator.ActiveTransition>(() => new Navigator.ActiveTransition(), 128);

	// Token: 0x0400254C RID: 9548
	private Stack<TransitionDriver.InterruptOverrideLayer> interruptOverrideStack = new Stack<TransitionDriver.InterruptOverrideLayer>(8);

	// Token: 0x0400254D RID: 9549
	private Navigator.ActiveTransition transition;

	// Token: 0x0400254E RID: 9550
	private Navigator navigator;

	// Token: 0x0400254F RID: 9551
	private Vector3 targetPos;

	// Token: 0x04002550 RID: 9552
	private bool isComplete;

	// Token: 0x04002551 RID: 9553
	private Brain brain;

	// Token: 0x04002552 RID: 9554
	public List<TransitionDriver.OverrideLayer> overrideLayers = new List<TransitionDriver.OverrideLayer>();

	// Token: 0x04002553 RID: 9555
	private LoggerFS log;

	// Token: 0x02000B5C RID: 2908
	public class OverrideLayer
	{
		// Token: 0x0600371E RID: 14110 RVA: 0x000A5E2C File Offset: 0x000A402C
		public OverrideLayer(Navigator navigator)
		{
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x000A5E40 File Offset: 0x000A4040
		public virtual void Destroy()
		{
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x000A5E40 File Offset: 0x000A4040
		public virtual void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x000A5E40 File Offset: 0x000A4040
		public virtual void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x000A5E40 File Offset: 0x000A4040
		public virtual void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}
	}

	// Token: 0x02000B5D RID: 2909
	public class InterruptOverrideLayer : TransitionDriver.OverrideLayer
	{
		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06003723 RID: 14115 RVA: 0x000C3C84 File Offset: 0x000C1E84
		protected bool InterruptInProgress
		{
			get
			{
				return this.originalTransition != null;
			}
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x000C3C8F File Offset: 0x000C1E8F
		public InterruptOverrideLayer(Navigator navigator) : base(navigator)
		{
			this.driver = navigator.transitionDriver;
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x000C3CA4 File Offset: 0x000C1EA4
		public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			this.driver.interruptOverrideStack.Push(this);
			this.originalTransition = TransitionDriver.SwapTransitionWithEmpty(transition);
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x002164CC File Offset: 0x002146CC
		public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			if (!this.IsOverrideComplete())
			{
				return;
			}
			this.driver.interruptOverrideStack.Pop();
			transition.Copy(this.originalTransition);
			TransitionDriver.TransitionPool.ReleaseInstance(this.originalTransition);
			this.originalTransition = null;
			this.EndTransition(navigator, transition);
			this.driver.BeginTransition(navigator, transition);
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x000C3CC3 File Offset: 0x000C1EC3
		public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			base.EndTransition(navigator, transition);
			if (this.originalTransition == null)
			{
				return;
			}
			TransitionDriver.TransitionPool.ReleaseInstance(this.originalTransition);
			this.originalTransition = null;
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x000C3CED File Offset: 0x000C1EED
		protected virtual bool IsOverrideComplete()
		{
			return this.originalTransition != null && this.driver.interruptOverrideStack.Count != 0 && this.driver.interruptOverrideStack.Peek() == this;
		}

		// Token: 0x04002554 RID: 9556
		protected Navigator.ActiveTransition originalTransition;

		// Token: 0x04002555 RID: 9557
		protected TransitionDriver driver;
	}
}
