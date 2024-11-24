using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

public class CancellableMove : Cancellable
{
	[Serialize]
	private List<Ref<Movable>> movables = new List<Ref<Movable>>();

	private MovePickupableChore fetchChore;

	public List<Ref<Movable>> movingObjects => movables;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable component = GetComponent<Prioritizable>();
		if (!component.IsPrioritizable())
		{
			component.AddRef();
		}
		if (fetchChore == null)
		{
			GameObject nextTarget = GetNextTarget();
			if (!(nextTarget != null) || nextTarget.IsNullOrDestroyed())
			{
				Debug.LogWarning("MovePickupable spawned with no objects to move. Destroying placer.");
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			fetchChore = new MovePickupableChore(this, nextTarget, OnChoreEnd);
		}
		Subscribe(493375141, OnRefreshUserMenu);
		Subscribe(2127324410, OnCancel);
		GetComponent<KPrefabID>().AddTag(GameTags.HasChores);
		int cell = Grid.PosToCell(this);
		Grid.Objects[cell, 44] = base.gameObject;
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		int cell = Grid.PosToCell(this);
		Grid.Objects[cell, 44] = null;
		Prioritizable.RemoveRef(base.gameObject);
	}

	public void CancelAll()
	{
		OnCancel();
	}

	public void OnCancel(Movable cancel_movable = null)
	{
		for (int num = movables.Count - 1; num >= 0; num--)
		{
			Ref<Movable> @ref = movables[num];
			if (@ref != null)
			{
				Movable movable = @ref.Get();
				if (cancel_movable == null || movable == cancel_movable)
				{
					movable.ClearMove();
					movables.RemoveAt(num);
				}
			}
		}
		if (fetchChore != null)
		{
			fetchChore.Cancel("CancelMove");
			if (fetchChore.driver == null && movables.Count <= 0)
			{
				Util.KDestroyGameObject(base.gameObject);
			}
		}
	}

	protected override void OnCancel(object data)
	{
		OnCancel();
	}

	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME_OFF, CancelAll, Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP_OFF));
	}

	public void SetMovable(Movable movable)
	{
		if (fetchChore == null)
		{
			fetchChore = new MovePickupableChore(this, movable.gameObject, OnChoreEnd);
		}
		if (movables.Find((Ref<Movable> move) => move.Get() == movable) == null)
		{
			movables.Add(new Ref<Movable>(movable));
		}
	}

	public void OnChoreEnd(Chore chore)
	{
		GameObject nextTarget = GetNextTarget();
		if (nextTarget == null)
		{
			Util.KDestroyGameObject(base.gameObject);
		}
		else
		{
			fetchChore = new MovePickupableChore(this, nextTarget, OnChoreEnd);
		}
	}

	public bool IsDeliveryComplete()
	{
		ValidateMovables();
		return movables.Count <= 0;
	}

	public void RemoveMovable(Movable moved)
	{
		for (int num = movables.Count - 1; num >= 0; num--)
		{
			if (movables[num].Get() == null || movables[num].Get() == moved)
			{
				movables.RemoveAt(num);
			}
		}
		if (movables.Count <= 0)
		{
			OnCancel();
		}
	}

	public GameObject GetNextTarget()
	{
		ValidateMovables();
		if (movables.Count > 0)
		{
			return movables[0].Get().gameObject;
		}
		return null;
	}

	private void ValidateMovables()
	{
		for (int num = movables.Count - 1; num >= 0; num--)
		{
			if (movables[num] == null)
			{
				movables.RemoveAt(num);
			}
			else
			{
				Movable movable = movables[num].Get();
				if (movable == null)
				{
					movables.RemoveAt(num);
				}
				else if (Grid.PosToCell(movable) == Grid.PosToCell(this))
				{
					movable.ClearMove();
					movables.RemoveAt(num);
				}
			}
		}
	}
}
