using System;
using System.Collections.Generic;

// Token: 0x020010C2 RID: 4290
public class Faction
{
	// Token: 0x060057F2 RID: 22514 RVA: 0x00289360 File Offset: 0x00287560
	public HashSet<FactionAlignment> HostileTo()
	{
		HashSet<FactionAlignment> hashSet = new HashSet<FactionAlignment>();
		foreach (KeyValuePair<FactionManager.FactionID, FactionManager.Disposition> keyValuePair in this.Dispositions)
		{
			if (keyValuePair.Value == FactionManager.Disposition.Attack)
			{
				hashSet.UnionWith(FactionManager.Instance.GetFaction(keyValuePair.Key).Members);
			}
		}
		return hashSet;
	}

	// Token: 0x060057F3 RID: 22515 RVA: 0x002893DC File Offset: 0x002875DC
	public Faction(FactionManager.FactionID faction)
	{
		this.ID = faction;
		this.ConfigureAlignments(faction);
	}

	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x060057F4 RID: 22516 RVA: 0x000D95B4 File Offset: 0x000D77B4
	// (set) Token: 0x060057F5 RID: 22517 RVA: 0x000D95BC File Offset: 0x000D77BC
	public bool CanAttack { get; private set; }

	// Token: 0x17000530 RID: 1328
	// (get) Token: 0x060057F6 RID: 22518 RVA: 0x000D95C5 File Offset: 0x000D77C5
	// (set) Token: 0x060057F7 RID: 22519 RVA: 0x000D95CD File Offset: 0x000D77CD
	public bool CanAssist { get; private set; }

	// Token: 0x060057F8 RID: 22520 RVA: 0x00289424 File Offset: 0x00287624
	private void ConfigureAlignments(FactionManager.FactionID faction)
	{
		switch (faction)
		{
		case FactionManager.FactionID.Duplicant:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Assist);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Assist);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
			break;
		case FactionManager.FactionID.Friendly:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Assist);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Assist);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
			break;
		case FactionManager.FactionID.Hostile:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Attack);
			break;
		case FactionManager.FactionID.Prey:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
			break;
		case FactionManager.FactionID.Predator:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Attack);
			break;
		case FactionManager.FactionID.Pest:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
			break;
		}
		foreach (KeyValuePair<FactionManager.FactionID, FactionManager.Disposition> keyValuePair in this.Dispositions)
		{
			if (keyValuePair.Value == FactionManager.Disposition.Attack)
			{
				this.CanAttack = true;
			}
			if (keyValuePair.Value == FactionManager.Disposition.Assist)
			{
				this.CanAssist = true;
			}
		}
	}

	// Token: 0x04003D7D RID: 15741
	public HashSet<FactionAlignment> Members = new HashSet<FactionAlignment>();

	// Token: 0x04003D7E RID: 15742
	public FactionManager.FactionID ID;

	// Token: 0x04003D7F RID: 15743
	public Dictionary<FactionManager.FactionID, FactionManager.Disposition> Dispositions = new Dictionary<FactionManager.FactionID, FactionManager.Disposition>(default(Faction.FactionIDComparer));

	// Token: 0x020010C3 RID: 4291
	public struct FactionIDComparer : IEqualityComparer<FactionManager.FactionID>
	{
		// Token: 0x060057F9 RID: 22521 RVA: 0x000D95D6 File Offset: 0x000D77D6
		public bool Equals(FactionManager.FactionID x, FactionManager.FactionID y)
		{
			return x == y;
		}

		// Token: 0x060057FA RID: 22522 RVA: 0x000B1FA8 File Offset: 0x000B01A8
		public int GetHashCode(FactionManager.FactionID obj)
		{
			return (int)obj;
		}
	}
}
