using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000C22 RID: 3106
public class ArtifactSelector : KMonoBehaviour
{
	// Token: 0x170002AA RID: 682
	// (get) Token: 0x06003B38 RID: 15160 RVA: 0x000C6391 File Offset: 0x000C4591
	public int AnalyzedArtifactCount
	{
		get
		{
			return this.analyzedArtifactCount;
		}
	}

	// Token: 0x170002AB RID: 683
	// (get) Token: 0x06003B39 RID: 15161 RVA: 0x000C6399 File Offset: 0x000C4599
	public int AnalyzedSpaceArtifactCount
	{
		get
		{
			return this.analyzedSpaceArtifactCount;
		}
	}

	// Token: 0x06003B3A RID: 15162 RVA: 0x000C63A1 File Offset: 0x000C45A1
	public List<string> GetAnalyzedArtifactIDs()
	{
		return this.analyzedArtifatIDs;
	}

	// Token: 0x06003B3B RID: 15163 RVA: 0x0022A1C8 File Offset: 0x002283C8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ArtifactSelector.Instance = this;
		this.placedArtifacts.Add(ArtifactType.Terrestrial, new List<string>());
		this.placedArtifacts.Add(ArtifactType.Space, new List<string>());
		this.placedArtifacts.Add(ArtifactType.Any, new List<string>());
	}

	// Token: 0x06003B3C RID: 15164 RVA: 0x0022A214 File Offset: 0x00228414
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int num = 0;
		int num2 = 0;
		foreach (string artifactID in this.analyzedArtifatIDs)
		{
			ArtifactType artifactType = this.GetArtifactType(artifactID);
			if (artifactType != ArtifactType.Space)
			{
				if (artifactType == ArtifactType.Terrestrial)
				{
					num++;
				}
			}
			else
			{
				num2++;
			}
		}
		if (num > this.analyzedArtifactCount)
		{
			this.analyzedArtifactCount = num;
		}
		if (num2 > this.analyzedSpaceArtifactCount)
		{
			this.analyzedSpaceArtifactCount = num2;
		}
	}

	// Token: 0x06003B3D RID: 15165 RVA: 0x000C63A9 File Offset: 0x000C45A9
	public bool RecordArtifactAnalyzed(string id)
	{
		if (this.analyzedArtifatIDs.Contains(id))
		{
			return false;
		}
		this.analyzedArtifatIDs.Add(id);
		return true;
	}

	// Token: 0x06003B3E RID: 15166 RVA: 0x000C63C8 File Offset: 0x000C45C8
	public void IncrementAnalyzedTerrestrialArtifacts()
	{
		this.analyzedArtifactCount++;
	}

	// Token: 0x06003B3F RID: 15167 RVA: 0x000C63D8 File Offset: 0x000C45D8
	public void IncrementAnalyzedSpaceArtifacts()
	{
		this.analyzedSpaceArtifactCount++;
	}

	// Token: 0x06003B40 RID: 15168 RVA: 0x0022A2A8 File Offset: 0x002284A8
	public string GetUniqueArtifactID(ArtifactType artifactType = ArtifactType.Any)
	{
		List<string> list = new List<string>();
		foreach (string item in ArtifactConfig.artifactItems[artifactType])
		{
			if (!this.placedArtifacts[artifactType].Contains(item))
			{
				list.Add(item);
			}
		}
		string text = "artifact_officemug";
		if (list.Count == 0 && artifactType != ArtifactType.Any)
		{
			foreach (string item2 in ArtifactConfig.artifactItems[ArtifactType.Any])
			{
				if (!this.placedArtifacts[ArtifactType.Any].Contains(item2))
				{
					list.Add(item2);
					artifactType = ArtifactType.Any;
				}
			}
		}
		if (list.Count != 0)
		{
			text = list[UnityEngine.Random.Range(0, list.Count)];
		}
		this.placedArtifacts[artifactType].Add(text);
		return text;
	}

	// Token: 0x06003B41 RID: 15169 RVA: 0x000C63E8 File Offset: 0x000C45E8
	public void ReserveArtifactID(string artifactID, ArtifactType artifactType = ArtifactType.Any)
	{
		if (this.placedArtifacts[artifactType].Contains(artifactID))
		{
			DebugUtil.Assert(true, string.Format("Tried to add {0} to placedArtifacts but it already exists in the list!", artifactID));
		}
		this.placedArtifacts[artifactType].Add(artifactID);
	}

	// Token: 0x06003B42 RID: 15170 RVA: 0x000C6421 File Offset: 0x000C4621
	public ArtifactType GetArtifactType(string artifactID)
	{
		if (this.placedArtifacts[ArtifactType.Terrestrial].Contains(artifactID))
		{
			return ArtifactType.Terrestrial;
		}
		if (this.placedArtifacts[ArtifactType.Space].Contains(artifactID))
		{
			return ArtifactType.Space;
		}
		return ArtifactType.Any;
	}

	// Token: 0x0400287B RID: 10363
	public static ArtifactSelector Instance;

	// Token: 0x0400287C RID: 10364
	[Serialize]
	private Dictionary<ArtifactType, List<string>> placedArtifacts = new Dictionary<ArtifactType, List<string>>();

	// Token: 0x0400287D RID: 10365
	[Serialize]
	private int analyzedArtifactCount;

	// Token: 0x0400287E RID: 10366
	[Serialize]
	private int analyzedSpaceArtifactCount;

	// Token: 0x0400287F RID: 10367
	[Serialize]
	private List<string> analyzedArtifatIDs = new List<string>();

	// Token: 0x04002880 RID: 10368
	private const string DEFAULT_ARTIFACT_ID = "artifact_officemug";
}
