using System;

// Token: 0x02000D3D RID: 3389
public interface INavDoor
{
	// Token: 0x17000347 RID: 839
	// (get) Token: 0x06004248 RID: 16968
	bool isSpawned { get; }

	// Token: 0x06004249 RID: 16969
	bool IsOpen();

	// Token: 0x0600424A RID: 16970
	void Open();

	// Token: 0x0600424B RID: 16971
	void Close();
}
