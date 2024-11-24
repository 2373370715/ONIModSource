using System;
using UnityEngine;

// Token: 0x020018FD RID: 6397
public interface ILaunchableRocket
{
	// Token: 0x170008B9 RID: 2233
	// (get) Token: 0x0600852C RID: 34092
	LaunchableRocketRegisterType registerType { get; }

	// Token: 0x170008BA RID: 2234
	// (get) Token: 0x0600852D RID: 34093
	GameObject LaunchableGameObject { get; }

	// Token: 0x170008BB RID: 2235
	// (get) Token: 0x0600852E RID: 34094
	float rocketSpeed { get; }

	// Token: 0x170008BC RID: 2236
	// (get) Token: 0x0600852F RID: 34095
	bool isLanding { get; }
}
