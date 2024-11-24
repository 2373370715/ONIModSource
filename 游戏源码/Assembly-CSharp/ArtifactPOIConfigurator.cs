using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020018AD RID: 6317
[AddComponentMenu("KMonoBehaviour/scripts/ArtifactPOIConfigurator")]
public class ArtifactPOIConfigurator : KMonoBehaviour
{
	// Token: 0x060082D5 RID: 33493 RVA: 0x0033E1F8 File Offset: 0x0033C3F8
	public static ArtifactPOIConfigurator.ArtifactPOIType FindType(HashedString typeId)
	{
		ArtifactPOIConfigurator.ArtifactPOIType artifactPOIType = null;
		if (typeId != HashedString.Invalid)
		{
			artifactPOIType = ArtifactPOIConfigurator._poiTypes.Find((ArtifactPOIConfigurator.ArtifactPOIType t) => t.id == typeId);
		}
		if (artifactPOIType == null)
		{
			global::Debug.LogError(string.Format("Tried finding a harvestable poi with id {0} but it doesn't exist!", typeId.ToString()));
		}
		return artifactPOIType;
	}

	// Token: 0x060082D6 RID: 33494 RVA: 0x000F5F8B File Offset: 0x000F418B
	public ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration MakeConfiguration()
	{
		return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
	}

	// Token: 0x060082D7 RID: 33495 RVA: 0x0033E264 File Offset: 0x0033C464
	private ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration CreateRandomInstance(HashedString typeId, float min, float max)
	{
		int globalWorldSeed = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
		ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
		Vector3 position = ClusterGrid.Instance.GetPosition(component);
		KRandom randomSource = new KRandom(globalWorldSeed + (int)position.x + (int)position.y);
		return new ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration
		{
			typeId = typeId,
			rechargeRoll = this.Roll(randomSource, min, max)
		};
	}

	// Token: 0x060082D8 RID: 33496 RVA: 0x000E30A9 File Offset: 0x000E12A9
	private float Roll(KRandom randomSource, float min, float max)
	{
		return (float)(randomSource.NextDouble() * (double)(max - min)) + min;
	}

	// Token: 0x04006344 RID: 25412
	private static List<ArtifactPOIConfigurator.ArtifactPOIType> _poiTypes;

	// Token: 0x04006345 RID: 25413
	public static ArtifactPOIConfigurator.ArtifactPOIType defaultArtifactPoiType = new ArtifactPOIConfigurator.ArtifactPOIType("HarvestablePOIArtifacts", null, false, 30000f, 60000f, "EXPANSION1_ID");

	// Token: 0x04006346 RID: 25414
	public HashedString presetType;

	// Token: 0x04006347 RID: 25415
	public float presetMin;

	// Token: 0x04006348 RID: 25416
	public float presetMax = 1f;

	// Token: 0x020018AE RID: 6318
	public class ArtifactPOIType
	{
		// Token: 0x060082DB RID: 33499 RVA: 0x0033E2C4 File Offset: 0x0033C4C4
		public ArtifactPOIType(string id, string harvestableArtifactID = null, bool destroyOnHarvest = false, float poiRechargeTimeMin = 30000f, float poiRechargeTimeMax = 60000f, string dlcID = "EXPANSION1_ID")
		{
			this.id = id;
			this.idHash = id;
			this.harvestableArtifactID = harvestableArtifactID;
			this.destroyOnHarvest = destroyOnHarvest;
			this.poiRechargeTimeMin = poiRechargeTimeMin;
			this.poiRechargeTimeMax = poiRechargeTimeMax;
			this.dlcID = dlcID;
			if (ArtifactPOIConfigurator._poiTypes == null)
			{
				ArtifactPOIConfigurator._poiTypes = new List<ArtifactPOIConfigurator.ArtifactPOIType>();
			}
			ArtifactPOIConfigurator._poiTypes.Add(this);
		}

		// Token: 0x04006349 RID: 25417
		public string id;

		// Token: 0x0400634A RID: 25418
		public HashedString idHash;

		// Token: 0x0400634B RID: 25419
		public string harvestableArtifactID;

		// Token: 0x0400634C RID: 25420
		public bool destroyOnHarvest;

		// Token: 0x0400634D RID: 25421
		public float poiRechargeTimeMin;

		// Token: 0x0400634E RID: 25422
		public float poiRechargeTimeMax;

		// Token: 0x0400634F RID: 25423
		public string dlcID;

		// Token: 0x04006350 RID: 25424
		public List<string> orbitalObject = new List<string>
		{
			Db.Get().OrbitalTypeCategories.gravitas.Id
		};
	}

	// Token: 0x020018AF RID: 6319
	[Serializable]
	public class ArtifactPOIInstanceConfiguration
	{
		// Token: 0x060082DC RID: 33500 RVA: 0x0033E354 File Offset: 0x0033C554
		private void Init()
		{
			if (this.didInit)
			{
				return;
			}
			this.didInit = true;
			this.poiRechargeTime = MathUtil.ReRange(this.rechargeRoll, 0f, 1f, this.poiType.poiRechargeTimeMin, this.poiType.poiRechargeTimeMax);
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x060082DD RID: 33501 RVA: 0x000F5FDA File Offset: 0x000F41DA
		public ArtifactPOIConfigurator.ArtifactPOIType poiType
		{
			get
			{
				return ArtifactPOIConfigurator.FindType(this.typeId);
			}
		}

		// Token: 0x060082DE RID: 33502 RVA: 0x000F5FE7 File Offset: 0x000F41E7
		public bool DestroyOnHarvest()
		{
			this.Init();
			return this.poiType.destroyOnHarvest;
		}

		// Token: 0x060082DF RID: 33503 RVA: 0x000F5FFA File Offset: 0x000F41FA
		public string GetArtifactID()
		{
			this.Init();
			return this.poiType.harvestableArtifactID;
		}

		// Token: 0x060082E0 RID: 33504 RVA: 0x000F600D File Offset: 0x000F420D
		public float GetRechargeTime()
		{
			this.Init();
			return this.poiRechargeTime;
		}

		// Token: 0x04006351 RID: 25425
		public HashedString typeId;

		// Token: 0x04006352 RID: 25426
		private bool didInit;

		// Token: 0x04006353 RID: 25427
		public float rechargeRoll;

		// Token: 0x04006354 RID: 25428
		private float poiRechargeTime;
	}
}
