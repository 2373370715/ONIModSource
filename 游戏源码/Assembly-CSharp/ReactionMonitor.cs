using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

public class ReactionMonitor : GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>
{
	public class Def : BaseDef
	{
		public ObjectLayer ReactionLayer;
	}

	public new class Instance : GameInstance
	{
		private KBatchedAnimController animController;

		private float lastReaction = float.NaN;

		private Dictionary<HashedString, float> lastReactTimes;

		private List<Reactable> oneshotReactables;

		public Reactable ImmediateReactable { get; private set; }

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			animController = GetComponent<KBatchedAnimController>();
			lastReactTimes = new Dictionary<HashedString, float>();
			oneshotReactables = new List<Reactable>();
		}

		public bool CanReact(Emote e)
		{
			if (animController != null)
			{
				return e.IsValidForController(animController);
			}
			return false;
		}

		public bool TryReact(Reactable reactable, float clockTime, Navigator.ActiveTransition transition = null)
		{
			if (reactable == null)
			{
				return false;
			}
			if ((lastReactTimes.TryGetValue(reactable.id, out var value) && value == lastReaction) || clockTime - value < reactable.localCooldown)
			{
				return false;
			}
			if (!reactable.CanBegin(base.gameObject, transition))
			{
				return false;
			}
			lastReactTimes[reactable.id] = clockTime;
			base.sm.reactable.Set(reactable, base.smi);
			base.smi.GoTo(base.sm.reacting);
			return true;
		}

		public void PollForReactables(Navigator.ActiveTransition transition)
		{
			if (IsReacting())
			{
				return;
			}
			for (int num = oneshotReactables.Count - 1; num >= 0; num--)
			{
				Reactable reactable = oneshotReactables[num];
				if (reactable.IsExpired())
				{
					reactable.Cleanup();
					oneshotReactables.RemoveAt(num);
				}
			}
			Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(base.smi.gameObject));
			ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(int)base.def.ReactionLayer];
			ListPool<ScenePartitionerEntry, ReactionMonitor>.PooledList pooledList = ListPool<ScenePartitionerEntry, ReactionMonitor>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 1, 1, layer, pooledList);
			float num2 = float.NaN;
			float time = GameClock.Instance.GetTime();
			for (int i = 0; i < pooledList.Count; i++)
			{
				Reactable reactable2 = pooledList[i].obj as Reactable;
				if (TryReact(reactable2, time, transition))
				{
					num2 = time;
					break;
				}
			}
			lastReaction = num2;
			pooledList.Recycle();
		}

		public void ClearLastReaction()
		{
			lastReaction = float.NaN;
		}

		public void StopReaction()
		{
			for (int num = oneshotReactables.Count - 1; num >= 0; num--)
			{
				if (base.sm.reactable.Get(base.smi) == oneshotReactables[num])
				{
					oneshotReactables[num].Cleanup();
					oneshotReactables.RemoveAt(num);
					break;
				}
			}
			base.smi.GoTo(base.sm.idle);
		}

		public bool IsReacting()
		{
			return base.smi.IsInsideState(base.sm.reacting);
		}

		public SelfEmoteReactable AddSelfEmoteReactable(GameObject target, HashedString reactionId, Emote emote, bool isOneShot, ChoreType choreType, float globalCooldown = 0f, float localCooldown = 20f, float lifeSpan = float.NegativeInfinity, float maxInitialDelay = 0f, List<Reactable.ReactablePrecondition> emotePreconditions = null)
		{
			if (!CanReact(emote))
			{
				return null;
			}
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(target, reactionId, choreType, globalCooldown, localCooldown, lifeSpan, maxInitialDelay);
			selfEmoteReactable.SetEmote(emote);
			int num = 0;
			while (emotePreconditions != null && num < emotePreconditions.Count)
			{
				selfEmoteReactable.AddPrecondition(emotePreconditions[num]);
				num++;
			}
			if (isOneShot)
			{
				AddOneshotReactable(selfEmoteReactable);
			}
			return selfEmoteReactable;
		}

		public SelfEmoteReactable AddSelfEmoteReactable(GameObject target, string reactionId, string emoteAnim, bool isOneShot, ChoreType choreType, float globalCooldown = 0f, float localCooldown = 20f, float maxTriggerTime = float.NegativeInfinity, float maxInitialDelay = 0f, List<Reactable.ReactablePrecondition> emotePreconditions = null)
		{
			Emote emote = new Emote(null, reactionId, new EmoteStep[1]
			{
				new EmoteStep
				{
					anim = "react"
				}
			}, emoteAnim);
			return AddSelfEmoteReactable(target, reactionId, emote, isOneShot, choreType, globalCooldown, localCooldown, maxTriggerTime, maxInitialDelay, emotePreconditions);
		}

		public void AddOneshotReactable(SelfEmoteReactable reactable)
		{
			if (reactable != null)
			{
				oneshotReactables.Add(reactable);
			}
		}

		public void CancelOneShotReactable(SelfEmoteReactable cancel_target)
		{
			for (int num = oneshotReactables.Count - 1; num >= 0; num--)
			{
				Reactable reactable = oneshotReactables[num];
				if (cancel_target == reactable)
				{
					reactable.Cleanup();
					oneshotReactables.RemoveAt(num);
					break;
				}
			}
		}

		public void CancelOneShotReactables(Emote reactionEmote)
		{
			for (int num = oneshotReactables.Count - 1; num >= 0; num--)
			{
				if (oneshotReactables[num] is EmoteReactable emoteReactable && emoteReactable.emote == reactionEmote)
				{
					emoteReactable.Cleanup();
					oneshotReactables.RemoveAt(num);
				}
			}
		}
	}

	public State idle;

	public State reacting;

	public State dead;

	public ObjectParameter<Reactable> reactable;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = idle;
		base.serializable = SerializeType.Never;
		root.EventHandler(GameHashes.DestinationReached, delegate(Instance smi)
		{
			smi.ClearLastReaction();
		}).EventHandler(GameHashes.NavigationFailed, delegate(Instance smi)
		{
			smi.ClearLastReaction();
		});
		idle.Enter("ClearReactable", delegate(Instance smi)
		{
			reactable.Set(null, smi);
		}).TagTransition(GameTags.Dead, dead);
		reacting.Enter("Reactable.Begin", delegate(Instance smi)
		{
			reactable.Get(smi).Begin(smi.gameObject);
		}).Enter(delegate(Instance smi)
		{
			smi.master.Trigger(-909573545);
		}).Enter("Reactable.AddChorePreventionTag", delegate(Instance smi)
		{
			if (reactable.Get(smi).preventChoreInterruption)
			{
				smi.GetComponent<KPrefabID>().AddTag(GameTags.PreventChoreInterruption);
			}
		})
			.Update("Reactable.Update", delegate(Instance smi, float dt)
			{
				reactable.Get(smi).Update(dt);
			})
			.Exit(delegate(Instance smi)
			{
				smi.master.Trigger(824899998);
			})
			.Exit("Reactable.End", delegate(Instance smi)
			{
				reactable.Get(smi).End();
			})
			.Exit("Reactable.RemoveChorePreventionTag", delegate(Instance smi)
			{
				if (reactable.Get(smi).preventChoreInterruption)
				{
					smi.GetComponent<KPrefabID>().RemoveTag(GameTags.PreventChoreInterruption);
				}
			})
			.EventTransition(GameHashes.NavigationFailed, idle)
			.TagTransition(GameTags.Dying, dead)
			.TagTransition(GameTags.Dead, dead);
		dead.DoNothing();
	}

	private static bool ShouldReact(Instance smi)
	{
		return smi.ImmediateReactable != null;
	}
}
