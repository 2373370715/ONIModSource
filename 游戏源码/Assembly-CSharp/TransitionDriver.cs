using System.Collections.Generic;
using UnityEngine;

public class TransitionDriver
{
	public class OverrideLayer
	{
		public OverrideLayer(Navigator navigator)
		{
		}

		public virtual void Destroy()
		{
		}

		public virtual void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}

		public virtual void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}

		public virtual void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}
	}

	public class InterruptOverrideLayer : OverrideLayer
	{
		protected Navigator.ActiveTransition originalTransition;

		protected TransitionDriver driver;

		protected bool InterruptInProgress => originalTransition != null;

		public InterruptOverrideLayer(Navigator navigator)
			: base(navigator)
		{
			driver = navigator.transitionDriver;
		}

		public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			driver.interruptOverrideStack.Push(this);
			originalTransition = SwapTransitionWithEmpty(transition);
		}

		public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			if (IsOverrideComplete())
			{
				driver.interruptOverrideStack.Pop();
				transition.Copy(originalTransition);
				TransitionPool.ReleaseInstance(originalTransition);
				originalTransition = null;
				EndTransition(navigator, transition);
				driver.BeginTransition(navigator, transition);
			}
		}

		public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			base.EndTransition(navigator, transition);
			if (originalTransition != null)
			{
				TransitionPool.ReleaseInstance(originalTransition);
				originalTransition = null;
			}
		}

		protected virtual bool IsOverrideComplete()
		{
			if (originalTransition == null)
			{
				return false;
			}
			if (driver.interruptOverrideStack.Count != 0)
			{
				return driver.interruptOverrideStack.Peek() == this;
			}
			return false;
		}
	}

	private static Navigator.ActiveTransition emptyTransition = new Navigator.ActiveTransition();

	public static ObjectPool<Navigator.ActiveTransition> TransitionPool = new ObjectPool<Navigator.ActiveTransition>(() => new Navigator.ActiveTransition(), 128);

	private Stack<InterruptOverrideLayer> interruptOverrideStack = new Stack<InterruptOverrideLayer>(8);

	private Navigator.ActiveTransition transition;

	private Navigator navigator;

	private Vector3 targetPos;

	private bool isComplete;

	private Brain brain;

	public List<OverrideLayer> overrideLayers = new List<OverrideLayer>();

	private LoggerFS log;

	public Navigator.ActiveTransition GetTransition => transition;

	public TransitionDriver(Navigator navigator)
	{
		log = new LoggerFS("TransitionDriver");
	}

	public void BeginTransition(Navigator navigator, NavGrid.Transition transition, float defaultSpeed)
	{
		Navigator.ActiveTransition instance = TransitionPool.GetInstance();
		instance.Init(transition, defaultSpeed);
		BeginTransition(navigator, instance);
	}

	private void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		bool flag = interruptOverrideStack.Count != 0;
		foreach (OverrideLayer overrideLayer in overrideLayers)
		{
			if (!flag || !(overrideLayer is InterruptOverrideLayer))
			{
				overrideLayer.BeginTransition(navigator, transition);
			}
		}
		this.navigator = navigator;
		this.transition = transition;
		isComplete = false;
		Grid.SceneLayer sceneLayer = navigator.sceneLayer;
		if (transition.navGridTransition.start == NavType.Tube || transition.navGridTransition.end == NavType.Tube)
		{
			sceneLayer = Grid.SceneLayer.BuildingUse;
		}
		else if (transition.navGridTransition.start == NavType.Solid && transition.navGridTransition.end == NavType.Solid)
		{
			KBatchedAnimController component = navigator.GetComponent<KBatchedAnimController>();
			sceneLayer = Grid.SceneLayer.FXFront;
			component.SetSceneLayer(sceneLayer);
		}
		else if (transition.navGridTransition.start == NavType.Solid || transition.navGridTransition.end == NavType.Solid)
		{
			navigator.GetComponent<KBatchedAnimController>().SetSceneLayer(sceneLayer);
		}
		int cell = Grid.OffsetCell(Grid.PosToCell(navigator), transition.x, transition.y);
		targetPos = Grid.CellToPosCBC(cell, sceneLayer);
		if (transition.isLooping)
		{
			KAnimControllerBase component2 = navigator.GetComponent<KAnimControllerBase>();
			component2.PlaySpeedMultiplier = transition.animSpeed;
			bool flag2 = transition.preAnim != "";
			bool flag3 = component2.CurrentAnim != null && component2.CurrentAnim.name == transition.anim;
			if (flag2 && component2.CurrentAnim != null && component2.CurrentAnim.name == transition.preAnim)
			{
				component2.ClearQueue();
				component2.Queue(transition.anim, KAnim.PlayMode.Loop);
			}
			else if (flag3)
			{
				if (component2.PlayMode != 0)
				{
					component2.ClearQueue();
					component2.Queue(transition.anim, KAnim.PlayMode.Loop);
				}
			}
			else if (flag2)
			{
				component2.Play(transition.preAnim);
				component2.Queue(transition.anim, KAnim.PlayMode.Loop);
			}
			else
			{
				component2.Play(transition.anim, KAnim.PlayMode.Loop);
			}
		}
		else if (transition.anim != null)
		{
			KAnimControllerBase component3 = navigator.GetComponent<KAnimControllerBase>();
			component3.PlaySpeedMultiplier = transition.animSpeed;
			component3.Play(transition.anim);
			navigator.Subscribe(-1061186183, OnAnimComplete);
		}
		if (transition.navGridTransition.y != 0)
		{
			if (transition.navGridTransition.start == NavType.RightWall)
			{
				navigator.GetComponent<Facing>().SetFacing(transition.navGridTransition.y < 0);
			}
			else if (transition.navGridTransition.start == NavType.LeftWall)
			{
				navigator.GetComponent<Facing>().SetFacing(transition.navGridTransition.y > 0);
			}
		}
		if (transition.navGridTransition.x != 0)
		{
			if (transition.navGridTransition.start == NavType.Ceiling)
			{
				navigator.GetComponent<Facing>().SetFacing(transition.navGridTransition.x > 0);
			}
			else if (transition.navGridTransition.start != NavType.LeftWall && transition.navGridTransition.start != NavType.RightWall)
			{
				navigator.GetComponent<Facing>().SetFacing(transition.navGridTransition.x < 0);
			}
		}
		brain = navigator.GetComponent<Brain>();
	}

	public void UpdateTransition(float dt)
	{
		if (navigator == null)
		{
			return;
		}
		foreach (OverrideLayer overrideLayer in overrideLayers)
		{
			bool num = interruptOverrideStack.Count != 0;
			bool flag = overrideLayer is InterruptOverrideLayer;
			if (!(num && flag) || interruptOverrideStack.Peek() == overrideLayer)
			{
				overrideLayer.UpdateTransition(navigator, transition);
			}
		}
		if (!isComplete && transition.isCompleteCB != null)
		{
			isComplete = transition.isCompleteCB();
		}
		if (brain != null)
		{
			_ = isComplete;
		}
		if (transition.isLooping)
		{
			float speed = transition.speed;
			Vector3 position = navigator.transform.GetPosition();
			int num2 = Grid.PosToCell(position);
			if (transition.x > 0)
			{
				position.x += dt * speed;
				if (position.x > targetPos.x)
				{
					isComplete = true;
				}
			}
			else if (transition.x < 0)
			{
				position.x -= dt * speed;
				if (position.x < targetPos.x)
				{
					isComplete = true;
				}
			}
			else
			{
				position.x = targetPos.x;
			}
			if (transition.y > 0)
			{
				position.y += dt * speed;
				if (position.y > targetPos.y)
				{
					isComplete = true;
				}
			}
			else if (transition.y < 0)
			{
				position.y -= dt * speed;
				if (position.y < targetPos.y)
				{
					isComplete = true;
				}
			}
			else
			{
				position.y = targetPos.y;
			}
			navigator.transform.SetPosition(position);
			int num3 = Grid.PosToCell(position);
			if (num3 != num2)
			{
				navigator.Trigger(915392638, num3);
			}
		}
		if (isComplete)
		{
			isComplete = false;
			Navigator obj = navigator;
			obj.SetCurrentNavType(transition.end);
			obj.transform.SetPosition(targetPos);
			EndTransition();
			obj.AdvancePath();
		}
	}

	public void EndTransition()
	{
		if (!(navigator != null))
		{
			return;
		}
		interruptOverrideStack.Clear();
		foreach (OverrideLayer overrideLayer in overrideLayers)
		{
			overrideLayer.EndTransition(navigator, transition);
		}
		navigator.GetComponent<KAnimControllerBase>().PlaySpeedMultiplier = 1f;
		navigator.Unsubscribe(-1061186183, OnAnimComplete);
		if (brain != null)
		{
			brain.Resume("move_handler");
		}
		TransitionPool.ReleaseInstance(transition);
		transition = null;
		navigator = null;
		brain = null;
	}

	private void OnAnimComplete(object data)
	{
		if (navigator != null)
		{
			navigator.Unsubscribe(-1061186183, OnAnimComplete);
		}
		isComplete = true;
	}

	public static Navigator.ActiveTransition SwapTransitionWithEmpty(Navigator.ActiveTransition src)
	{
		Navigator.ActiveTransition instance = TransitionPool.GetInstance();
		instance.Copy(src);
		src.Copy(emptyTransition);
		return instance;
	}
}
