using System;
using UnityEngine;

// Token: 0x020017B2 RID: 6066
[AddComponentMenu("KMonoBehaviour/scripts/Reservable")]
public class Reservable : KMonoBehaviour
{
	// Token: 0x170007F0 RID: 2032
	// (get) Token: 0x06007CF4 RID: 31988 RVA: 0x000F242D File Offset: 0x000F062D
	public GameObject ReservedBy
	{
		get
		{
			return this.reservedBy;
		}
	}

	// Token: 0x170007F1 RID: 2033
	// (get) Token: 0x06007CF5 RID: 31989 RVA: 0x000F2435 File Offset: 0x000F0635
	public bool isReserved
	{
		get
		{
			return !(this.reservedBy == null);
		}
	}

	// Token: 0x06007CF6 RID: 31990 RVA: 0x000F2446 File Offset: 0x000F0646
	public bool Reserve(GameObject reserver)
	{
		if (this.reservedBy == null)
		{
			this.reservedBy = reserver;
			return true;
		}
		return false;
	}

	// Token: 0x06007CF7 RID: 31991 RVA: 0x000F2460 File Offset: 0x000F0660
	public void ClearReservation(GameObject reserver)
	{
		if (this.reservedBy == reserver)
		{
			this.reservedBy = null;
		}
	}

	// Token: 0x04005E8C RID: 24204
	private GameObject reservedBy;
}
