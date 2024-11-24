using System;
using FMODUnity;
using UnityEngine;

// Token: 0x020018A6 RID: 6310
[AddComponentMenu("KMonoBehaviour/scripts/Sounds")]
public class Sounds : KMonoBehaviour
{
	// Token: 0x1700085C RID: 2140
	// (get) Token: 0x060082BB RID: 33467 RVA: 0x000F5F23 File Offset: 0x000F4123
	// (set) Token: 0x060082BC RID: 33468 RVA: 0x000F5F2A File Offset: 0x000F412A
	public static Sounds Instance { get; private set; }

	// Token: 0x060082BD RID: 33469 RVA: 0x000F5F32 File Offset: 0x000F4132
	public static void DestroyInstance()
	{
		Sounds.Instance = null;
	}

	// Token: 0x060082BE RID: 33470 RVA: 0x000F5F3A File Offset: 0x000F413A
	protected override void OnPrefabInit()
	{
		Sounds.Instance = this;
	}

	// Token: 0x0400631E RID: 25374
	public FMODAsset BlowUp_Generic;

	// Token: 0x0400631F RID: 25375
	public FMODAsset Build_Generic;

	// Token: 0x04006320 RID: 25376
	public FMODAsset InUse_Fabricator;

	// Token: 0x04006321 RID: 25377
	public FMODAsset InUse_OxygenGenerator;

	// Token: 0x04006322 RID: 25378
	public FMODAsset Place_OreOnSite;

	// Token: 0x04006323 RID: 25379
	public FMODAsset Footstep_rock;

	// Token: 0x04006324 RID: 25380
	public FMODAsset Ice_crack;

	// Token: 0x04006325 RID: 25381
	public FMODAsset BuildingPowerOn;

	// Token: 0x04006326 RID: 25382
	public FMODAsset ElectricGridOverload;

	// Token: 0x04006327 RID: 25383
	public FMODAsset IngameMusic;

	// Token: 0x04006328 RID: 25384
	public FMODAsset[] OreSplashSounds;

	// Token: 0x0400632A RID: 25386
	public EventReference BlowUp_GenericMigrated;

	// Token: 0x0400632B RID: 25387
	public EventReference Build_GenericMigrated;

	// Token: 0x0400632C RID: 25388
	public EventReference InUse_FabricatorMigrated;

	// Token: 0x0400632D RID: 25389
	public EventReference InUse_OxygenGeneratorMigrated;

	// Token: 0x0400632E RID: 25390
	public EventReference Place_OreOnSiteMigrated;

	// Token: 0x0400632F RID: 25391
	public EventReference Footstep_rockMigrated;

	// Token: 0x04006330 RID: 25392
	public EventReference Ice_crackMigrated;

	// Token: 0x04006331 RID: 25393
	public EventReference BuildingPowerOnMigrated;

	// Token: 0x04006332 RID: 25394
	public EventReference ElectricGridOverloadMigrated;

	// Token: 0x04006333 RID: 25395
	public EventReference IngameMusicMigrated;

	// Token: 0x04006334 RID: 25396
	public EventReference[] OreSplashSoundsMigrated;
}
