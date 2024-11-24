using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020018E3 RID: 6371
[AddComponentMenu("KMonoBehaviour/scripts/HarvestablePOIConfigurator")]
public class HarvestablePOIConfigurator : KMonoBehaviour
{
	// Token: 0x060084AA RID: 33962 RVA: 0x00344CE4 File Offset: 0x00342EE4
	public static HarvestablePOIConfigurator.HarvestablePOIType FindType(HashedString typeId)
	{
		HarvestablePOIConfigurator.HarvestablePOIType harvestablePOIType = null;
		if (typeId != HashedString.Invalid)
		{
			harvestablePOIType = HarvestablePOIConfigurator._poiTypes.Find((HarvestablePOIConfigurator.HarvestablePOIType t) => t.id == typeId);
		}
		if (harvestablePOIType == null)
		{
			global::Debug.LogError(string.Format("Tried finding a harvestable poi with id {0} but it doesn't exist!", typeId.ToString()));
		}
		return harvestablePOIType;
	}

	// Token: 0x060084AB RID: 33963 RVA: 0x000F71F1 File Offset: 0x000F53F1
	public HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration MakeConfiguration()
	{
		return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
	}

	// Token: 0x060084AC RID: 33964 RVA: 0x00344D50 File Offset: 0x00342F50
	private HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration CreateRandomInstance(HashedString typeId, float min, float max)
	{
		int globalWorldSeed = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
		ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
		Vector3 position = ClusterGrid.Instance.GetPosition(component);
		KRandom randomSource = new KRandom(globalWorldSeed + (int)position.x + (int)position.y);
		return new HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration
		{
			typeId = typeId,
			capacityRoll = this.Roll(randomSource, min, max),
			rechargeRoll = this.Roll(randomSource, min, max)
		};
	}

	// Token: 0x060084AD RID: 33965 RVA: 0x000E30A9 File Offset: 0x000E12A9
	private float Roll(KRandom randomSource, float min, float max)
	{
		return (float)(randomSource.NextDouble() * (double)(max - min)) + min;
	}

	// Token: 0x04006435 RID: 25653
	private static List<HarvestablePOIConfigurator.HarvestablePOIType> _poiTypes;

	// Token: 0x04006436 RID: 25654
	public HashedString presetType;

	// Token: 0x04006437 RID: 25655
	public float presetMin;

	// Token: 0x04006438 RID: 25656
	public float presetMax = 1f;

	// Token: 0x020018E4 RID: 6372
	public class HarvestablePOIType
	{
		// Token: 0x060084AF RID: 33967 RVA: 0x00344DC0 File Offset: 0x00342FC0
		public HarvestablePOIType(string id, Dictionary<SimHashes, float> harvestableElements, float poiCapacityMin = 54000f, float poiCapacityMax = 81000f, float poiRechargeMin = 30000f, float poiRechargeMax = 60000f, bool canProvideArtifacts = true, List<string> orbitalObject = null, int maxNumOrbitingObjects = 20, string dlcID = "EXPANSION1_ID")
		{
			this.id = id;
			this.idHash = id;
			this.harvestableElements = harvestableElements;
			this.poiCapacityMin = poiCapacityMin;
			this.poiCapacityMax = poiCapacityMax;
			this.poiRechargeMin = poiRechargeMin;
			this.poiRechargeMax = poiRechargeMax;
			this.canProvideArtifacts = canProvideArtifacts;
			this.orbitalObject = orbitalObject;
			this.maxNumOrbitingObjects = maxNumOrbitingObjects;
			this.dlcID = dlcID;
			if (HarvestablePOIConfigurator._poiTypes == null)
			{
				HarvestablePOIConfigurator._poiTypes = new List<HarvestablePOIConfigurator.HarvestablePOIType>();
			}
			HarvestablePOIConfigurator._poiTypes.Add(this);
		}

		// Token: 0x04006439 RID: 25657
		public string id;

		// Token: 0x0400643A RID: 25658
		public HashedString idHash;

		// Token: 0x0400643B RID: 25659
		public Dictionary<SimHashes, float> harvestableElements;

		// Token: 0x0400643C RID: 25660
		public float poiCapacityMin;

		// Token: 0x0400643D RID: 25661
		public float poiCapacityMax;

		// Token: 0x0400643E RID: 25662
		public float poiRechargeMin;

		// Token: 0x0400643F RID: 25663
		public float poiRechargeMax;

		// Token: 0x04006440 RID: 25664
		public bool canProvideArtifacts;

		// Token: 0x04006441 RID: 25665
		public string dlcID;

		// Token: 0x04006442 RID: 25666
		public List<string> orbitalObject;

		// Token: 0x04006443 RID: 25667
		public int maxNumOrbitingObjects;
	}

	// Token: 0x020018E5 RID: 6373
	[Serializable]
	public class HarvestablePOIInstanceConfiguration
	{
		// Token: 0x060084B0 RID: 33968 RVA: 0x00344E48 File Offset: 0x00343048
		private void Init()
		{
			if (this.didInit)
			{
				return;
			}
			this.didInit = true;
			this.poiTotalCapacity = MathUtil.ReRange(this.capacityRoll, 0f, 1f, this.poiType.poiCapacityMin, this.poiType.poiCapacityMax);
			this.poiRecharge = MathUtil.ReRange(this.rechargeRoll, 0f, 1f, this.poiType.poiRechargeMin, this.poiType.poiRechargeMax);
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x060084B1 RID: 33969 RVA: 0x000F721E File Offset: 0x000F541E
		public HarvestablePOIConfigurator.HarvestablePOIType poiType
		{
			get
			{
				return HarvestablePOIConfigurator.FindType(this.typeId);
			}
		}

		// Token: 0x060084B2 RID: 33970 RVA: 0x000F722B File Offset: 0x000F542B
		public Dictionary<SimHashes, float> GetElementsWithWeights()
		{
			this.Init();
			return this.poiType.harvestableElements;
		}

		// Token: 0x060084B3 RID: 33971 RVA: 0x000F723E File Offset: 0x000F543E
		public bool CanProvideArtifacts()
		{
			this.Init();
			return this.poiType.canProvideArtifacts;
		}

		// Token: 0x060084B4 RID: 33972 RVA: 0x000F7251 File Offset: 0x000F5451
		public float GetMaxCapacity()
		{
			this.Init();
			return this.poiTotalCapacity;
		}

		// Token: 0x060084B5 RID: 33973 RVA: 0x000F725F File Offset: 0x000F545F
		public float GetRechargeTime()
		{
			this.Init();
			return this.poiRecharge;
		}

		// Token: 0x04006444 RID: 25668
		public HashedString typeId;

		// Token: 0x04006445 RID: 25669
		private bool didInit;

		// Token: 0x04006446 RID: 25670
		public float capacityRoll;

		// Token: 0x04006447 RID: 25671
		public float rechargeRoll;

		// Token: 0x04006448 RID: 25672
		private float poiTotalCapacity;

		// Token: 0x04006449 RID: 25673
		private float poiRecharge;
	}
}
