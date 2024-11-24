using System;
using UnityEngine;

// Token: 0x0200140F RID: 5135
public class CarePackageInfo : ITelepadDeliverable
{
	// Token: 0x060069D4 RID: 27092 RVA: 0x000E56E9 File Offset: 0x000E38E9
	public CarePackageInfo(string ID, float amount, Func<bool> requirement)
	{
		this.id = ID;
		this.quantity = amount;
		this.requirement = requirement;
	}

	// Token: 0x060069D5 RID: 27093 RVA: 0x000E5706 File Offset: 0x000E3906
	public CarePackageInfo(string ID, float amount, Func<bool> requirement, string facadeID)
	{
		this.id = ID;
		this.quantity = amount;
		this.requirement = requirement;
		this.facadeID = facadeID;
	}

	// Token: 0x060069D6 RID: 27094 RVA: 0x002DBE80 File Offset: 0x002DA080
	public GameObject Deliver(Vector3 location)
	{
		location += Vector3.right / 2f;
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(CarePackageConfig.ID), location);
		gameObject.SetActive(true);
		gameObject.GetComponent<CarePackage>().SetInfo(this);
		return gameObject;
	}

	// Token: 0x04004FED RID: 20461
	public readonly string id;

	// Token: 0x04004FEE RID: 20462
	public readonly float quantity;

	// Token: 0x04004FEF RID: 20463
	public readonly Func<bool> requirement;

	// Token: 0x04004FF0 RID: 20464
	public readonly string facadeID;
}
