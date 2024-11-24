using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200185C RID: 6236
public class SetLocker : StateMachineComponent<SetLocker.StatesInstance>, ISidescreenButtonControl
{
	// Token: 0x060080DE RID: 32990 RVA: 0x000B2F5A File Offset: 0x000B115A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060080DF RID: 32991 RVA: 0x000F4C85 File Offset: 0x000F2E85
	public void ChooseContents()
	{
		this.contents = this.possible_contents_ids[UnityEngine.Random.Range(0, this.possible_contents_ids.GetLength(0))];
	}

	// Token: 0x060080E0 RID: 32992 RVA: 0x00336370 File Offset: 0x00334570
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.contents == null)
		{
			this.ChooseContents();
		}
		else
		{
			string[] array = this.contents;
			for (int i = 0; i < array.Length; i++)
			{
				if (Assets.GetPrefab(array[i]) == null)
				{
					this.ChooseContents();
					break;
				}
			}
		}
		if (this.pendingRummage)
		{
			this.ActivateChore(null);
		}
	}

	// Token: 0x060080E1 RID: 32993 RVA: 0x003363E0 File Offset: 0x003345E0
	public void DropContents()
	{
		if (this.contents == null)
		{
			return;
		}
		if (DlcManager.IsExpansion1Active() && this.numDataBanks.Length >= 2)
		{
			int num = UnityEngine.Random.Range(this.numDataBanks[0], this.numDataBanks[1]);
			for (int i = 0; i <= num; i++)
			{
				Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), this.dropOffset.x, this.dropOffset.y, "OrbitalResearchDatabank", Grid.SceneLayer.Front).SetActive(true);
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab("OrbitalResearchDatabank".ToTag()).GetProperName(), base.smi.master.transform, 1.5f, false);
			}
		}
		for (int j = 0; j < this.contents.Length; j++)
		{
			GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), this.dropOffset.x, this.dropOffset.y, this.contents[j], Grid.SceneLayer.Front);
			if (gameObject != null)
			{
				gameObject.SetActive(true);
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab(this.contents[j].ToTag()).GetProperName(), base.smi.master.transform, 1.5f, false);
			}
		}
		base.gameObject.Trigger(-372600542, this);
	}

	// Token: 0x060080E2 RID: 32994 RVA: 0x000F4CA6 File Offset: 0x000F2EA6
	private void OnClickOpen()
	{
		this.ActivateChore(null);
	}

	// Token: 0x060080E3 RID: 32995 RVA: 0x000F4CAF File Offset: 0x000F2EAF
	private void OnClickCancel()
	{
		this.CancelChore(null);
	}

	// Token: 0x060080E4 RID: 32996 RVA: 0x00336550 File Offset: 0x00334750
	public void ActivateChore(object param = null)
	{
		if (this.chore != null)
		{
			return;
		}
		Prioritizable.AddRef(base.gameObject);
		base.Trigger(1980521255, null);
		this.pendingRummage = true;
		base.GetComponent<Workable>().SetWorkTime(1.5f);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, false, true, Assets.GetAnim(this.overrideAnim), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x060080E5 RID: 32997 RVA: 0x000F4CB8 File Offset: 0x000F2EB8
	public void CancelChore(object param = null)
	{
		if (this.chore == null)
		{
			return;
		}
		this.pendingRummage = false;
		Prioritizable.RemoveRef(base.gameObject);
		base.Trigger(1980521255, null);
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x060080E6 RID: 32998 RVA: 0x003365DC File Offset: 0x003347DC
	private void CompleteChore()
	{
		this.used = true;
		base.smi.GoTo(base.smi.sm.open);
		this.chore = null;
		this.pendingRummage = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x17000839 RID: 2105
	// (get) Token: 0x060080E7 RID: 32999 RVA: 0x000F4CF8 File Offset: 0x000F2EF8
	public string SidescreenButtonText
	{
		get
		{
			return (this.chore == null) ? UI.USERMENUACTIONS.OPENPOI.NAME : UI.USERMENUACTIONS.OPENPOI.NAME_OFF;
		}
	}

	// Token: 0x1700083A RID: 2106
	// (get) Token: 0x060080E8 RID: 33000 RVA: 0x000F4D13 File Offset: 0x000F2F13
	public string SidescreenButtonTooltip
	{
		get
		{
			return (this.chore == null) ? UI.USERMENUACTIONS.OPENPOI.TOOLTIP : UI.USERMENUACTIONS.OPENPOI.TOOLTIP_OFF;
		}
	}

	// Token: 0x060080E9 RID: 33001 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x060080EA RID: 33002 RVA: 0x000ABC75 File Offset: 0x000A9E75
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x060080EB RID: 33003 RVA: 0x000F4D2E File Offset: 0x000F2F2E
	public void OnSidescreenButtonPressed()
	{
		if (this.chore == null)
		{
			this.OnClickOpen();
			return;
		}
		this.OnClickCancel();
	}

	// Token: 0x060080EC RID: 33004 RVA: 0x000F4D45 File Offset: 0x000F2F45
	public bool SidescreenButtonInteractable()
	{
		return !this.used;
	}

	// Token: 0x060080ED RID: 33005 RVA: 0x000ABCBD File Offset: 0x000A9EBD
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x060080EE RID: 33006 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x040061B5 RID: 25013
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x040061B6 RID: 25014
	public string[][] possible_contents_ids;

	// Token: 0x040061B7 RID: 25015
	public string machineSound;

	// Token: 0x040061B8 RID: 25016
	public string overrideAnim;

	// Token: 0x040061B9 RID: 25017
	public Vector2I dropOffset = Vector2I.zero;

	// Token: 0x040061BA RID: 25018
	public int[] numDataBanks;

	// Token: 0x040061BB RID: 25019
	[Serialize]
	private string[] contents;

	// Token: 0x040061BC RID: 25020
	public bool dropOnDeconstruct;

	// Token: 0x040061BD RID: 25021
	[Serialize]
	private bool pendingRummage;

	// Token: 0x040061BE RID: 25022
	[Serialize]
	private bool used;

	// Token: 0x040061BF RID: 25023
	private Chore chore;

	// Token: 0x0200185D RID: 6237
	public class StatesInstance : GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.GameInstance
	{
		// Token: 0x060080F1 RID: 33009 RVA: 0x000F4D6B File Offset: 0x000F2F6B
		public StatesInstance(SetLocker master) : base(master)
		{
		}

		// Token: 0x060080F2 RID: 33010 RVA: 0x000F4D74 File Offset: 0x000F2F74
		public override void StartSM()
		{
			base.StartSM();
			base.smi.Subscribe(-702296337, delegate(object o)
			{
				if (base.smi.master.dropOnDeconstruct && base.smi.IsInsideState(base.smi.sm.closed))
				{
					base.smi.master.DropContents();
				}
			});
		}
	}

	// Token: 0x0200185E RID: 6238
	public class States : GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker>
	{
		// Token: 0x060080F4 RID: 33012 RVA: 0x00336688 File Offset: 0x00334888
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.closed;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.closed.PlayAnim("on").Enter(delegate(SetLocker.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StartSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
			this.open.PlayAnim("working_pre").QueueAnim("working_loop", false, null).QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.off).Exit(delegate(SetLocker.StatesInstance smi)
			{
				smi.master.DropContents();
			});
			this.off.PlayAnim("off").Enter(delegate(SetLocker.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StopSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
		}

		// Token: 0x040061C0 RID: 25024
		public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State closed;

		// Token: 0x040061C1 RID: 25025
		public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State open;

		// Token: 0x040061C2 RID: 25026
		public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State off;
	}
}
