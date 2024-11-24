using System;
using Database;
using UnityEngine;

// Token: 0x02001D54 RID: 7508
public interface IKleiPermitDioramaVisTarget
{
	// Token: 0x06009CD8 RID: 40152
	GameObject GetGameObject();

	// Token: 0x06009CD9 RID: 40153
	void ConfigureSetup();

	// Token: 0x06009CDA RID: 40154
	void ConfigureWith(PermitResource permit);
}
