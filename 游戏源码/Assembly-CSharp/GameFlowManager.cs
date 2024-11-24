using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001320 RID: 4896
[SerializationConfig(MemberSerialization.OptIn)]
public class GameFlowManager : StateMachineComponent<GameFlowManager.StatesInstance>, ISaveLoadable
{
	// Token: 0x06006496 RID: 25750 RVA: 0x000E1A4F File Offset: 0x000DFC4F
	public static void DestroyInstance()
	{
		GameFlowManager.Instance = null;
	}

	// Token: 0x06006497 RID: 25751 RVA: 0x000E1A57 File Offset: 0x000DFC57
	protected override void OnPrefabInit()
	{
		GameFlowManager.Instance = this;
	}

	// Token: 0x06006498 RID: 25752 RVA: 0x000E1A5F File Offset: 0x000DFC5F
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06006499 RID: 25753 RVA: 0x000E1A6C File Offset: 0x000DFC6C
	public bool IsGameOver()
	{
		return base.smi.IsInsideState(base.smi.sm.gameover);
	}

	// Token: 0x04004856 RID: 18518
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04004857 RID: 18519
	public static GameFlowManager Instance;

	// Token: 0x02001321 RID: 4897
	public class StatesInstance : GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.GameInstance
	{
		// Token: 0x0600649B RID: 25755 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public bool IsIncapacitated(GameObject go)
		{
			return false;
		}

		// Token: 0x0600649C RID: 25756 RVA: 0x002C0D40 File Offset: 0x002BEF40
		public void CheckForGameOver()
		{
			if (!Game.Instance.GameStarted())
			{
				return;
			}
			if (GenericGameSettings.instance.disableGameOver)
			{
				return;
			}
			bool flag = false;
			if (Components.LiveMinionIdentities.Count == 0)
			{
				flag = true;
			}
			else
			{
				flag = true;
				foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
				{
					if (!this.IsIncapacitated(minionIdentity.gameObject))
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				this.GoTo(base.sm.gameover.pending);
			}
		}

		// Token: 0x0600649D RID: 25757 RVA: 0x002C0DEC File Offset: 0x002BEFEC
		public StatesInstance(GameFlowManager smi) : base(smi)
		{
		}

		// Token: 0x04004858 RID: 18520
		public Notification colonyLostNotification = new Notification(MISC.NOTIFICATIONS.COLONYLOST.NAME, NotificationType.Bad, null, null, false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x02001322 RID: 4898
	public class States : GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager>
	{
		// Token: 0x0600649E RID: 25758 RVA: 0x002C0E24 File Offset: 0x002BF024
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.loading;
			this.loading.ScheduleGoTo(4f, this.running);
			this.running.Update("CheckForGameOver", delegate(GameFlowManager.StatesInstance smi, float dt)
			{
				smi.CheckForGameOver();
			}, UpdateRate.SIM_200ms, false);
			this.gameover.TriggerOnEnter(GameHashes.GameOver, null).ToggleNotification((GameFlowManager.StatesInstance smi) => smi.colonyLostNotification);
			this.gameover.pending.Enter("Goto(gameover.active)", delegate(GameFlowManager.StatesInstance smi)
			{
				UIScheduler.Instance.Schedule("Goto(gameover.active)", 4f, delegate(object d)
				{
					smi.GoTo(this.gameover.active);
				}, null, null);
			});
			this.gameover.active.Enter(delegate(GameFlowManager.StatesInstance smi)
			{
				if (GenericGameSettings.instance.demoMode)
				{
					DemoTimer.Instance.EndDemo();
					return;
				}
				GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.GameOverScreen, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<KScreen>().Show(true);
			});
		}

		// Token: 0x04004859 RID: 18521
		public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State loading;

		// Token: 0x0400485A RID: 18522
		public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State running;

		// Token: 0x0400485B RID: 18523
		public GameFlowManager.States.GameOverState gameover;

		// Token: 0x02001323 RID: 4899
		public class GameOverState : GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State
		{
			// Token: 0x0400485C RID: 18524
			public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State pending;

			// Token: 0x0400485D RID: 18525
			public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State active;
		}
	}
}
