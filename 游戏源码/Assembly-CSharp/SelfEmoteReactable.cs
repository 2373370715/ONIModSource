using UnityEngine;

public class SelfEmoteReactable : EmoteReactable
{
	private EmoteChore chore;

	public SelfEmoteReactable(GameObject gameObject, HashedString id, ChoreType chore_type, float globalCooldown = 0f, float localCooldown = 20f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f)
		: base(gameObject, id, chore_type, 3, 3, globalCooldown, localCooldown, lifeSpan, max_initial_delay)
	{
	}

	public override bool InternalCanBegin(GameObject reactor, Navigator.ActiveTransition transition)
	{
		if (reactor != gameObject)
		{
			return false;
		}
		Navigator component = reactor.GetComponent<Navigator>();
		if (component == null || !component.IsMoving())
		{
			return false;
		}
		return true;
	}

	public void PairEmote(EmoteChore emoteChore)
	{
		chore = emoteChore;
	}

	protected override void InternalEnd()
	{
		if (chore != null && chore.driver != null)
		{
			chore.PairReactable(null);
			chore.Cancel("Reactable ended");
			chore = null;
		}
		base.InternalEnd();
	}
}
