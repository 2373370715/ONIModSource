using System;
using Klei.AI;
using UnityEngine;

public class EmoteReactable : Reactable
{
	private KBatchedAnimController kbac;

	public Expression expression;

	public Thought thought;

	public Emote emote;

	private HandleVector<EmoteStep.Callbacks>.Handle[] callbackHandles;

	protected KAnimFile overrideAnimSet;

	private int currentStep = -1;

	private float elapsed;

	public EmoteReactable(GameObject gameObject, HashedString id, ChoreType chore_type, int range_width = 15, int range_height = 8, float globalCooldown = 0f, float localCooldown = 20f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f)
		: base(gameObject, id, chore_type, range_width, range_height, follow_transform: true, globalCooldown, localCooldown, lifeSpan, max_initial_delay)
	{
	}

	public EmoteReactable SetEmote(Emote emote)
	{
		this.emote = emote;
		return this;
	}

	public EmoteReactable RegisterEmoteStepCallbacks(HashedString stepName, Action<GameObject> startedCb, Action<GameObject> finishedCb)
	{
		if (callbackHandles == null)
		{
			callbackHandles = new HandleVector<EmoteStep.Callbacks>.Handle[emote.StepCount];
		}
		int stepIndex = emote.GetStepIndex(stepName);
		callbackHandles[stepIndex] = emote[stepIndex].RegisterCallbacks(startedCb, finishedCb);
		return this;
	}

	public EmoteReactable SetExpression(Expression expression)
	{
		this.expression = expression;
		return this;
	}

	public EmoteReactable SetThought(Thought thought)
	{
		this.thought = thought;
		return this;
	}

	public EmoteReactable SetOverideAnimSet(string animSet)
	{
		overrideAnimSet = Assets.GetAnim(animSet);
		return this;
	}

	public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
	{
		if (reactor != null || new_reactor == null)
		{
			return false;
		}
		Navigator component = new_reactor.GetComponent<Navigator>();
		if (component == null || !component.IsMoving())
		{
			return false;
		}
		if ((0xFFFFFEFFu & (uint)(1 << (int)component.CurrentNavType)) != 0)
		{
			return gameObject != new_reactor;
		}
		return false;
	}

	public override void Update(float dt)
	{
		if (emote == null || !emote.IsValidStep(currentStep))
		{
			return;
		}
		if (gameObject != null && reactor != null)
		{
			Facing component = reactor.GetComponent<Facing>();
			if (component != null)
			{
				component.Face(gameObject.transform.GetPosition());
			}
		}
		float timeout = emote[currentStep].timeout;
		if (timeout > 0f && timeout < elapsed)
		{
			NextStep(null);
		}
		else
		{
			elapsed += dt;
		}
	}

	protected override void InternalBegin()
	{
		kbac = reactor.GetComponent<KBatchedAnimController>();
		emote.ApplyAnimOverrides(kbac, overrideAnimSet);
		if (expression != null)
		{
			reactor.GetComponent<FaceGraph>().AddExpression(expression);
		}
		if (thought != null)
		{
			reactor.GetSMI<ThoughtGraph.Instance>().AddThought(thought);
		}
		NextStep(null);
	}

	protected override void InternalEnd()
	{
		if (kbac != null)
		{
			kbac.onAnimComplete -= NextStep;
			emote.RemoveAnimOverrides(kbac, overrideAnimSet);
			kbac = null;
		}
		if (reactor != null)
		{
			if (expression != null)
			{
				reactor.GetComponent<FaceGraph>().RemoveExpression(expression);
			}
			if (thought != null)
			{
				reactor.GetSMI<ThoughtGraph.Instance>().RemoveThought(thought);
			}
		}
		currentStep = -1;
	}

	protected override void InternalCleanup()
	{
		if (emote != null && callbackHandles != null)
		{
			for (int i = 0; emote.IsValidStep(i); i++)
			{
				emote[i].UnregisterCallbacks(callbackHandles[i]);
			}
		}
	}

	private void NextStep(HashedString finishedAnim)
	{
		if (emote.IsValidStep(currentStep) && emote[currentStep].timeout <= 0f)
		{
			kbac.onAnimComplete -= NextStep;
			if (callbackHandles != null)
			{
				emote[currentStep].OnStepFinished(callbackHandles[currentStep], reactor);
			}
		}
		currentStep++;
		if (!emote.IsValidStep(currentStep) || kbac == null)
		{
			End();
			return;
		}
		EmoteStep emoteStep = emote[currentStep];
		if (emoteStep.anim != HashedString.Invalid)
		{
			kbac.Play(emoteStep.anim, emoteStep.mode);
			if (kbac.IsStopped())
			{
				emoteStep.timeout = 0.25f;
			}
		}
		if (emoteStep.timeout <= 0f)
		{
			kbac.onAnimComplete += NextStep;
		}
		else
		{
			elapsed = 0f;
		}
		if (callbackHandles != null)
		{
			emoteStep.OnStepStarted(callbackHandles[currentStep], reactor);
		}
	}
}
