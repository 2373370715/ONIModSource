using System;
using UnityEngine;

// Token: 0x020010BF RID: 4287
[AddComponentMenu("KMonoBehaviour/scripts/FactionManager")]
public class FactionManager : KMonoBehaviour
{
	// Token: 0x060057EC RID: 22508 RVA: 0x000D956C File Offset: 0x000D776C
	public static void DestroyInstance()
	{
		FactionManager.Instance = null;
	}

	// Token: 0x060057ED RID: 22509 RVA: 0x000D9574 File Offset: 0x000D7774
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		FactionManager.Instance = this;
	}

	// Token: 0x060057EE RID: 22510 RVA: 0x000BFD08 File Offset: 0x000BDF08
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060057EF RID: 22511 RVA: 0x002892AC File Offset: 0x002874AC
	public Faction GetFaction(FactionManager.FactionID faction)
	{
		switch (faction)
		{
		case FactionManager.FactionID.Duplicant:
			return this.Duplicant;
		case FactionManager.FactionID.Friendly:
			return this.Friendly;
		case FactionManager.FactionID.Hostile:
			return this.Hostile;
		case FactionManager.FactionID.Prey:
			return this.Prey;
		case FactionManager.FactionID.Predator:
			return this.Predator;
		case FactionManager.FactionID.Pest:
			return this.Pest;
		default:
			return null;
		}
	}

	// Token: 0x060057F0 RID: 22512 RVA: 0x000D9582 File Offset: 0x000D7782
	public FactionManager.Disposition GetDisposition(FactionManager.FactionID of_faction, FactionManager.FactionID to_faction)
	{
		if (FactionManager.Instance.GetFaction(of_faction).Dispositions.ContainsKey(to_faction))
		{
			return FactionManager.Instance.GetFaction(of_faction).Dispositions[to_faction];
		}
		return FactionManager.Disposition.Neutral;
	}

	// Token: 0x04003D6A RID: 15722
	public static FactionManager Instance;

	// Token: 0x04003D6B RID: 15723
	public Faction Duplicant = new Faction(FactionManager.FactionID.Duplicant);

	// Token: 0x04003D6C RID: 15724
	public Faction Friendly = new Faction(FactionManager.FactionID.Friendly);

	// Token: 0x04003D6D RID: 15725
	public Faction Hostile = new Faction(FactionManager.FactionID.Hostile);

	// Token: 0x04003D6E RID: 15726
	public Faction Predator = new Faction(FactionManager.FactionID.Predator);

	// Token: 0x04003D6F RID: 15727
	public Faction Prey = new Faction(FactionManager.FactionID.Prey);

	// Token: 0x04003D70 RID: 15728
	public Faction Pest = new Faction(FactionManager.FactionID.Pest);

	// Token: 0x020010C0 RID: 4288
	public enum FactionID
	{
		// Token: 0x04003D72 RID: 15730
		Duplicant,
		// Token: 0x04003D73 RID: 15731
		Friendly,
		// Token: 0x04003D74 RID: 15732
		Hostile,
		// Token: 0x04003D75 RID: 15733
		Prey,
		// Token: 0x04003D76 RID: 15734
		Predator,
		// Token: 0x04003D77 RID: 15735
		Pest,
		// Token: 0x04003D78 RID: 15736
		NumberOfFactions
	}

	// Token: 0x020010C1 RID: 4289
	public enum Disposition
	{
		// Token: 0x04003D7A RID: 15738
		Assist,
		// Token: 0x04003D7B RID: 15739
		Neutral,
		// Token: 0x04003D7C RID: 15740
		Attack
	}
}
