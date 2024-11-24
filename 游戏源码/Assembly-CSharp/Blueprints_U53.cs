using System;
using Database;

// Token: 0x0200097F RID: 2431
public class Blueprints_U53 : BlueprintProvider
{
	// Token: 0x06002BFF RID: 11263 RVA: 0x000A6F3E File Offset: 0x000A513E
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06002C00 RID: 11264 RVA: 0x000BC82B File Offset: 0x000BAA2B
	public override void SetupBlueprints()
	{
		base.AddBuilding("LuxuryBed", PermitRarity.Loyalty, "permit_elegantbed_hatch", "elegantbed_hatch_kanim");
		base.AddBuilding("LuxuryBed", PermitRarity.Loyalty, "permit_elegantbed_pipsqueak", "elegantbed_pipsqueak_kanim");
	}
}
