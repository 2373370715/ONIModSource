using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009AC RID: 2476
public class CancellableMove : Cancellable
{
	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06002D52 RID: 11602 RVA: 0x000BD5F3 File Offset: 0x000BB7F3
	public List<Ref<Movable>> movingObjects
	{
		get
		{
			return this.movables;
		}
	}

	// Token: 0x06002D53 RID: 11603 RVA: 0x001F016C File Offset: 0x001EE36C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (!component.IsPrioritizable())
		{
			component.AddRef();
		}
		if (this.fetchChore == null)
		{
			GameObject nextTarget = this.GetNextTarget();
			if (!(nextTarget != null) || nextTarget.IsNullOrDestroyed())
			{
				global::Debug.LogWarning("MovePickupable spawned with no objects to move. Destroying placer.");
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			this.fetchChore = new MovePickupableChore(this, nextTarget, new Action<Chore>(this.OnChoreEnd));
		}
		base.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
		base.Subscribe(2127324410, new Action<object>(this.OnCancel));
		base.GetComponent<KPrefabID>().AddTag(GameTags.HasChores, false);
		int cell = Grid.PosToCell(this);
		Grid.Objects[cell, 44] = base.gameObject;
	}

	// Token: 0x06002D54 RID: 11604 RVA: 0x001F0244 File Offset: 0x001EE444
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		int cell = Grid.PosToCell(this);
		Grid.Objects[cell, 44] = null;
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x06002D55 RID: 11605 RVA: 0x000BD5FB File Offset: 0x000BB7FB
	public void CancelAll()
	{
		this.OnCancel(null);
	}

	// Token: 0x06002D56 RID: 11606 RVA: 0x001F0278 File Offset: 0x001EE478
	public void OnCancel(Movable cancel_movable = null)
	{
		for (int i = this.movables.Count - 1; i >= 0; i--)
		{
			Ref<Movable> @ref = this.movables[i];
			if (@ref != null)
			{
				Movable movable = @ref.Get();
				if (cancel_movable == null || movable == cancel_movable)
				{
					movable.ClearMove();
					this.movables.RemoveAt(i);
				}
			}
		}
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("CancelMove");
			if (this.fetchChore.driver == null && this.movables.Count <= 0)
			{
				Util.KDestroyGameObject(base.gameObject);
			}
		}
	}

	// Token: 0x06002D57 RID: 11607 RVA: 0x000BD5FB File Offset: 0x000BB7FB
	protected override void OnCancel(object data)
	{
		this.OnCancel(null);
	}

	// Token: 0x06002D58 RID: 11608 RVA: 0x001F031C File Offset: 0x001EE51C
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME_OFF, new System.Action(this.CancelAll), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP_OFF, true), 1f);
	}

	// Token: 0x06002D59 RID: 11609 RVA: 0x001F0378 File Offset: 0x001EE578
	public void SetMovable(Movable movable)
	{
		if (this.fetchChore == null)
		{
			this.fetchChore = new MovePickupableChore(this, movable.gameObject, new Action<Chore>(this.OnChoreEnd));
		}
		if (this.movables.Find((Ref<Movable> move) => move.Get() == movable) == null)
		{
			this.movables.Add(new Ref<Movable>(movable));
		}
	}

	// Token: 0x06002D5A RID: 11610 RVA: 0x001F03EC File Offset: 0x001EE5EC
	public void OnChoreEnd(Chore chore)
	{
		GameObject nextTarget = this.GetNextTarget();
		if (nextTarget == null)
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		this.fetchChore = new MovePickupableChore(this, nextTarget, new Action<Chore>(this.OnChoreEnd));
	}

	// Token: 0x06002D5B RID: 11611 RVA: 0x000BD604 File Offset: 0x000BB804
	public bool IsDeliveryComplete()
	{
		this.ValidateMovables();
		return this.movables.Count <= 0;
	}

	// Token: 0x06002D5C RID: 11612 RVA: 0x001F0430 File Offset: 0x001EE630
	public void RemoveMovable(Movable moved)
	{
		for (int i = this.movables.Count - 1; i >= 0; i--)
		{
			if (this.movables[i].Get() == null || this.movables[i].Get() == moved)
			{
				this.movables.RemoveAt(i);
			}
		}
		if (this.movables.Count <= 0)
		{
			this.OnCancel(null);
		}
	}

	// Token: 0x06002D5D RID: 11613 RVA: 0x000BD61D File Offset: 0x000BB81D
	public GameObject GetNextTarget()
	{
		this.ValidateMovables();
		if (this.movables.Count > 0)
		{
			return this.movables[0].Get().gameObject;
		}
		return null;
	}

	// Token: 0x06002D5E RID: 11614 RVA: 0x001F04A8 File Offset: 0x001EE6A8
	private void ValidateMovables()
	{
		for (int i = this.movables.Count - 1; i >= 0; i--)
		{
			if (this.movables[i] == null)
			{
				this.movables.RemoveAt(i);
			}
			else
			{
				Movable movable = this.movables[i].Get();
				if (movable == null)
				{
					this.movables.RemoveAt(i);
				}
				else if (Grid.PosToCell(movable) == Grid.PosToCell(this))
				{
					movable.ClearMove();
					this.movables.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x04001E80 RID: 7808
	[Serialize]
	private List<Ref<Movable>> movables = new List<Ref<Movable>>();

	// Token: 0x04001E81 RID: 7809
	private MovePickupableChore fetchChore;
}
