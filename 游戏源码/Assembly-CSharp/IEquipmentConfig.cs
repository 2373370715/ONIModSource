using System;
using UnityEngine;

// Token: 0x020012AD RID: 4781
public interface IEquipmentConfig
{
	// Token: 0x06006254 RID: 25172
	EquipmentDef CreateEquipmentDef();

	// Token: 0x06006255 RID: 25173
	void DoPostConfigure(GameObject go);

	// Token: 0x06006256 RID: 25174
	string[] GetDlcIds();
}
