using System;
using UnityEngine;

// Token: 0x02000812 RID: 2066
public class SelfEmoteReactable : EmoteReactable
{
	// Token: 0x06002505 RID: 9477 RVA: 0x001CB574 File Offset: 0x001C9774
	public SelfEmoteReactable(GameObject gameObject, HashedString id, ChoreType chore_type, float globalCooldown = 0f, float localCooldown = 20f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f) : base(gameObject, id, chore_type, 3, 3, globalCooldown, localCooldown, lifeSpan, max_initial_delay)
	{
	}

	// Token: 0x06002506 RID: 9478 RVA: 0x001CB594 File Offset: 0x001C9794
	public override bool InternalCanBegin(GameObject reactor, Navigator.ActiveTransition transition)
	{
		if (reactor != this.gameObject)
		{
			return false;
		}
		Navigator component = reactor.GetComponent<Navigator>();
		return !(component == null) && component.IsMoving();
	}

	// Token: 0x06002507 RID: 9479 RVA: 0x000B8217 File Offset: 0x000B6417
	public void PairEmote(EmoteChore emoteChore)
	{
		this.chore = emoteChore;
	}

	// Token: 0x06002508 RID: 9480 RVA: 0x001CB5CC File Offset: 0x001C97CC
	protected override void InternalEnd()
	{
		if (this.chore != null && this.chore.driver != null)
		{
			this.chore.PairReactable(null);
			this.chore.Cancel("Reactable ended");
			this.chore = null;
		}
		base.InternalEnd();
	}

	// Token: 0x0400190D RID: 6413
	private EmoteChore chore;
}
