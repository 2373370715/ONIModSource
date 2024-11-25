﻿using System;

public class PlayAnimsStates : GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>
{
		public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.animating;
		GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.State root = this.root;
		string name = "Unused";
		string tooltip = "Unused";
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, (string str, PlayAnimsStates.Instance smi) => smi.def.statusItemName, (string str, PlayAnimsStates.Instance smi) => smi.def.statusItemTooltip, main);
		this.animating.Enter("PlayAnims", delegate(PlayAnimsStates.Instance smi)
		{
			smi.PlayAnims();
		}).OnAnimQueueComplete(this.done).EventHandler(GameHashes.TagsChanged, delegate(PlayAnimsStates.Instance smi, object obj)
		{
			smi.HandleTagsChanged(obj);
		});
		this.done.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete((PlayAnimsStates.Instance smi) => smi.def.tag, false);
	}

		public GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.State animating;

		public GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.State done;

		public class Def : StateMachine.BaseDef
	{
				public Def(Tag tag, bool loop, string anim, string status_item_name, string status_item_tooltip) : this(tag, loop, new string[]
		{
			anim
		}, status_item_name, status_item_tooltip)
		{
		}

				public Def(Tag tag, bool loop, string[] anims, string status_item_name, string status_item_tooltip)
		{
			this.tag = tag;
			this.loop = loop;
			this.anims = anims;
			this.statusItemName = status_item_name;
			this.statusItemTooltip = status_item_tooltip;
		}

				public override string ToString()
		{
			return this.tag.ToString() + "(PlayAnimsStates)";
		}

				public Tag tag;

				public string[] anims;

				public bool loop;

				public string statusItemName;

				public string statusItemTooltip;
	}

		public new class Instance : GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.GameInstance
	{
				public Instance(Chore<PlayAnimsStates.Instance> chore, PlayAnimsStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, def.tag);
		}

				public void PlayAnims()
		{
			if (base.def.anims == null || base.def.anims.Length == 0)
			{
				return;
			}
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < base.def.anims.Length; i++)
			{
				KAnim.PlayMode mode = KAnim.PlayMode.Once;
				if (base.def.loop && i == base.def.anims.Length - 1)
				{
					mode = KAnim.PlayMode.Loop;
				}
				if (i == 0)
				{
					component.Play(base.def.anims[i], mode, 1f, 0f);
				}
				else
				{
					component.Queue(base.def.anims[i], mode, 1f, 0f);
				}
			}
		}

				public void HandleTagsChanged(object obj)
		{
			if (!base.smi.HasTag(base.smi.def.tag))
			{
				base.smi.GoTo(null);
			}
		}
	}
}
