using System;
using System.Collections.Generic;

// Token: 0x02000A68 RID: 2664
public interface IAssignableIdentity
{
	// Token: 0x06003106 RID: 12550
	string GetProperName();

	// Token: 0x06003107 RID: 12551
	List<Ownables> GetOwners();

	// Token: 0x06003108 RID: 12552
	Ownables GetSoleOwner();

	// Token: 0x06003109 RID: 12553
	bool IsNull();

	// Token: 0x0600310A RID: 12554
	bool HasOwner(Assignables owner);

	// Token: 0x0600310B RID: 12555
	int NumOwners();
}
