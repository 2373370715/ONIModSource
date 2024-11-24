using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02001252 RID: 4690
[AddComponentMenu("KMonoBehaviour/scripts/DupeGreetingManager")]
public class DupeGreetingManager : KMonoBehaviour, ISim200ms
{
	// Token: 0x0600601E RID: 24606 RVA: 0x000DEA4F File Offset: 0x000DCC4F
	protected override void OnPrefabInit()
	{
		this.candidateCells = new Dictionary<int, MinionIdentity>();
		this.activeSetups = new List<DupeGreetingManager.GreetingSetup>();
		this.cooldowns = new Dictionary<MinionIdentity, float>();
	}

	// Token: 0x0600601F RID: 24607 RVA: 0x002ACADC File Offset: 0x002AACDC
	public void Sim200ms(float dt)
	{
		if (GameClock.Instance.GetTime() / 600f < TuningData<DupeGreetingManager.Tuning>.Get().cyclesBeforeFirstGreeting)
		{
			return;
		}
		for (int i = this.activeSetups.Count - 1; i >= 0; i--)
		{
			DupeGreetingManager.GreetingSetup greetingSetup = this.activeSetups[i];
			if (!this.ValidNavigatingMinion(greetingSetup.A.minion) || !this.ValidOppositionalMinion(greetingSetup.A.minion, greetingSetup.B.minion))
			{
				greetingSetup.A.reactable.Cleanup();
				greetingSetup.B.reactable.Cleanup();
				this.activeSetups.RemoveAt(i);
			}
		}
		this.candidateCells.Clear();
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			if ((!this.cooldowns.ContainsKey(minionIdentity) || GameClock.Instance.GetTime() - this.cooldowns[minionIdentity] >= 720f * TuningData<DupeGreetingManager.Tuning>.Get().greetingDelayMultiplier) && this.ValidNavigatingMinion(minionIdentity))
			{
				for (int j = 0; j <= 2; j++)
				{
					int offsetCell = this.GetOffsetCell(minionIdentity, j);
					if (this.candidateCells.ContainsKey(offsetCell) && this.ValidOppositionalMinion(minionIdentity, this.candidateCells[offsetCell]))
					{
						this.BeginNewGreeting(minionIdentity, this.candidateCells[offsetCell], offsetCell);
						break;
					}
					this.candidateCells[offsetCell] = minionIdentity;
				}
			}
		}
	}

	// Token: 0x06006020 RID: 24608 RVA: 0x000DEA72 File Offset: 0x000DCC72
	private int GetOffsetCell(MinionIdentity minion, int offset)
	{
		if (!minion.GetComponent<Facing>().GetFacing())
		{
			return Grid.OffsetCell(Grid.PosToCell(minion), offset, 0);
		}
		return Grid.OffsetCell(Grid.PosToCell(minion), -offset, 0);
	}

	// Token: 0x06006021 RID: 24609 RVA: 0x002ACC84 File Offset: 0x002AAE84
	private bool ValidNavigatingMinion(MinionIdentity minion)
	{
		if (minion == null)
		{
			return false;
		}
		Navigator component = minion.GetComponent<Navigator>();
		return component != null && component.IsMoving() && component.CurrentNavType == NavType.Floor;
	}

	// Token: 0x06006022 RID: 24610 RVA: 0x002ACCC0 File Offset: 0x002AAEC0
	private bool ValidOppositionalMinion(MinionIdentity reference_minion, MinionIdentity minion)
	{
		if (reference_minion == null)
		{
			return false;
		}
		if (minion == null)
		{
			return false;
		}
		Facing component = minion.GetComponent<Facing>();
		Facing component2 = reference_minion.GetComponent<Facing>();
		return this.ValidNavigatingMinion(minion) && component != null && component2 != null && component.GetFacing() != component2.GetFacing();
	}

	// Token: 0x06006023 RID: 24611 RVA: 0x002ACD20 File Offset: 0x002AAF20
	private void BeginNewGreeting(MinionIdentity minion_a, MinionIdentity minion_b, int cell)
	{
		DupeGreetingManager.GreetingSetup greetingSetup = new DupeGreetingManager.GreetingSetup();
		greetingSetup.cell = cell;
		greetingSetup.A = new DupeGreetingManager.GreetingUnit(minion_a, this.GetReactable(minion_a));
		greetingSetup.B = new DupeGreetingManager.GreetingUnit(minion_b, this.GetReactable(minion_b));
		this.activeSetups.Add(greetingSetup);
	}

	// Token: 0x06006024 RID: 24612 RVA: 0x002ACD6C File Offset: 0x002AAF6C
	private Reactable GetReactable(MinionIdentity minion)
	{
		if (DupeGreetingManager.emotes == null)
		{
			DupeGreetingManager.emotes = new List<Emote>
			{
				Db.Get().Emotes.Minion.Wave,
				Db.Get().Emotes.Minion.Wave_Shy,
				Db.Get().Emotes.Minion.FingerGuns
			};
		}
		Emote emote = DupeGreetingManager.emotes[UnityEngine.Random.Range(0, DupeGreetingManager.emotes.Count)];
		SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(minion.gameObject, "NavigatorPassingGreeting", Db.Get().ChoreTypes.Emote, 1000f, 20f, float.PositiveInfinity, 0f);
		selfEmoteReactable.SetEmote(emote).SetThought(Db.Get().Thoughts.Chatty);
		selfEmoteReactable.RegisterEmoteStepCallbacks("react", new Action<GameObject>(this.BeginReacting), null);
		return selfEmoteReactable;
	}

	// Token: 0x06006025 RID: 24613 RVA: 0x002ACE68 File Offset: 0x002AB068
	private void BeginReacting(GameObject minionGO)
	{
		if (minionGO == null)
		{
			return;
		}
		MinionIdentity component = minionGO.GetComponent<MinionIdentity>();
		Vector3 vector = Vector3.zero;
		foreach (DupeGreetingManager.GreetingSetup greetingSetup in this.activeSetups)
		{
			if (greetingSetup.A.minion == component)
			{
				if (greetingSetup.B.minion != null)
				{
					vector = greetingSetup.B.minion.transform.GetPosition();
					greetingSetup.A.minion.Trigger(-594200555, greetingSetup.B.minion);
					greetingSetup.B.minion.Trigger(-594200555, greetingSetup.A.minion);
					break;
				}
				break;
			}
			else if (greetingSetup.B.minion == component)
			{
				if (greetingSetup.A.minion != null)
				{
					vector = greetingSetup.A.minion.transform.GetPosition();
					break;
				}
				break;
			}
		}
		minionGO.GetComponent<Facing>().SetFacing(vector.x < minionGO.transform.GetPosition().x);
		minionGO.GetComponent<Effects>().Add("Greeting", true);
		this.cooldowns[component] = GameClock.Instance.GetTime();
	}

	// Token: 0x0400442A RID: 17450
	private const float COOLDOWN_TIME = 720f;

	// Token: 0x0400442B RID: 17451
	private Dictionary<int, MinionIdentity> candidateCells;

	// Token: 0x0400442C RID: 17452
	private List<DupeGreetingManager.GreetingSetup> activeSetups;

	// Token: 0x0400442D RID: 17453
	private Dictionary<MinionIdentity, float> cooldowns;

	// Token: 0x0400442E RID: 17454
	private static List<Emote> emotes;

	// Token: 0x02001253 RID: 4691
	public class Tuning : TuningData<DupeGreetingManager.Tuning>
	{
		// Token: 0x0400442F RID: 17455
		public float cyclesBeforeFirstGreeting;

		// Token: 0x04004430 RID: 17456
		public float greetingDelayMultiplier;
	}

	// Token: 0x02001254 RID: 4692
	private class GreetingUnit
	{
		// Token: 0x06006028 RID: 24616 RVA: 0x000DEAA5 File Offset: 0x000DCCA5
		public GreetingUnit(MinionIdentity minion, Reactable reactable)
		{
			this.minion = minion;
			this.reactable = reactable;
		}

		// Token: 0x04004431 RID: 17457
		public MinionIdentity minion;

		// Token: 0x04004432 RID: 17458
		public Reactable reactable;
	}

	// Token: 0x02001255 RID: 4693
	private class GreetingSetup
	{
		// Token: 0x04004433 RID: 17459
		public int cell;

		// Token: 0x04004434 RID: 17460
		public DupeGreetingManager.GreetingUnit A;

		// Token: 0x04004435 RID: 17461
		public DupeGreetingManager.GreetingUnit B;
	}
}
