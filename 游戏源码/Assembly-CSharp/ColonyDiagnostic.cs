using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001220 RID: 4640
public abstract class ColonyDiagnostic : ISim4000ms
{
	// Token: 0x06005F13 RID: 24339 RVA: 0x000DE0FB File Offset: 0x000DC2FB
	public GameObject GetNextClickThroughObject()
	{
		if (this.aggregatedUniqueClickThroughObjects.Count == 0)
		{
			return null;
		}
		this.clickThroughIndex = (this.clickThroughIndex + 1) % this.aggregatedUniqueClickThroughObjects.Count;
		return this.aggregatedUniqueClickThroughObjects[this.clickThroughIndex];
	}

	// Token: 0x06005F14 RID: 24340 RVA: 0x002A7C54 File Offset: 0x002A5E54
	public ColonyDiagnostic(int worldID, string name)
	{
		this.worldID = worldID;
		this.name = name;
		this.id = base.GetType().Name;
		this.IsWorldModuleInterior = ClusterManager.Instance.GetWorld(worldID).IsModuleInterior;
		this.colors = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color>();
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening, Constants.NEGATIVE_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Bad, Constants.NEGATIVE_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Warning, Constants.NEGATIVE_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Concern, Constants.WARNING_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, Constants.NEUTRAL_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion, Constants.NEUTRAL_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial, Constants.NEUTRAL_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Good, Constants.POSITIVE_COLOR);
		SimAndRenderScheduler.instance.Add(this, true);
	}

	// Token: 0x170005BA RID: 1466
	// (get) Token: 0x06005F15 RID: 24341 RVA: 0x000DE137 File Offset: 0x000DC337
	// (set) Token: 0x06005F16 RID: 24342 RVA: 0x000DE13F File Offset: 0x000DC33F
	public int worldID { get; protected set; }

	// Token: 0x170005BB RID: 1467
	// (get) Token: 0x06005F17 RID: 24343 RVA: 0x000DE148 File Offset: 0x000DC348
	// (set) Token: 0x06005F18 RID: 24344 RVA: 0x000DE150 File Offset: 0x000DC350
	public bool IsWorldModuleInterior { get; private set; }

	// Token: 0x06005F19 RID: 24345 RVA: 0x000A6F3E File Offset: 0x000A513E
	public virtual string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06005F1A RID: 24346 RVA: 0x000C09DD File Offset: 0x000BEBDD
	public void OnCleanUp()
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06005F1B RID: 24347 RVA: 0x000DE159 File Offset: 0x000DC359
	public void Sim4000ms(float dt)
	{
		this.SetResult(ColonyDiagnosticUtility.IgnoreFirstUpdate ? ColonyDiagnosticUtility.NoDataResult : this.Evaluate());
	}

	// Token: 0x06005F1C RID: 24348 RVA: 0x002A7D8C File Offset: 0x002A5F8C
	public DiagnosticCriterion[] GetCriteria()
	{
		DiagnosticCriterion[] array = new DiagnosticCriterion[this.criteria.Values.Count];
		this.criteria.Values.CopyTo(array, 0);
		return array;
	}

	// Token: 0x170005BC RID: 1468
	// (get) Token: 0x06005F1D RID: 24349 RVA: 0x000DE175 File Offset: 0x000DC375
	// (set) Token: 0x06005F1E RID: 24350 RVA: 0x000DE17D File Offset: 0x000DC37D
	public ColonyDiagnostic.DiagnosticResult LatestResult
	{
		get
		{
			return this.latestResult;
		}
		private set
		{
			this.latestResult = value;
		}
	}

	// Token: 0x06005F1F RID: 24351 RVA: 0x000DE186 File Offset: 0x000DC386
	public virtual string GetAverageValueString()
	{
		if (this.tracker != null)
		{
			return this.tracker.FormatValueString(Mathf.Round(this.tracker.GetAverageValue(this.trackerSampleCountSeconds)));
		}
		return "";
	}

	// Token: 0x06005F20 RID: 24352 RVA: 0x000CA99D File Offset: 0x000C8B9D
	public virtual string GetCurrentValueString()
	{
		return "";
	}

	// Token: 0x06005F21 RID: 24353 RVA: 0x000DE1B7 File Offset: 0x000DC3B7
	protected void AddCriterion(string id, DiagnosticCriterion criterion)
	{
		if (!this.criteria.ContainsKey(id))
		{
			criterion.SetID(id);
			this.criteria.Add(id, criterion);
		}
	}

	// Token: 0x06005F22 RID: 24354 RVA: 0x002A7DC4 File Offset: 0x002A5FC4
	public virtual ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, "", null);
		bool flag = false;
		if (!ClusterManager.Instance.GetWorld(this.worldID).IsDiscovered)
		{
			return diagnosticResult;
		}
		this.aggregatedUniqueClickThroughObjects.Clear();
		foreach (KeyValuePair<string, DiagnosticCriterion> keyValuePair in this.criteria)
		{
			if (ColonyDiagnosticUtility.Instance.IsCriteriaEnabled(this.worldID, this.id, keyValuePair.Key))
			{
				ColonyDiagnostic.DiagnosticResult diagnosticResult2 = keyValuePair.Value.Evaluate();
				if (diagnosticResult2.opinion < diagnosticResult.opinion || (!flag && diagnosticResult2.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal))
				{
					flag = true;
					diagnosticResult.opinion = diagnosticResult2.opinion;
					diagnosticResult.Message = diagnosticResult2.Message;
					diagnosticResult.clickThroughTarget = diagnosticResult2.clickThroughTarget;
					if (diagnosticResult2.clickThroughObjects != null)
					{
						foreach (GameObject item in diagnosticResult2.clickThroughObjects)
						{
							if (!this.aggregatedUniqueClickThroughObjects.Contains(item))
							{
								this.aggregatedUniqueClickThroughObjects.Add(item);
							}
						}
					}
				}
			}
		}
		return diagnosticResult;
	}

	// Token: 0x06005F23 RID: 24355 RVA: 0x000DE1DB File Offset: 0x000DC3DB
	public void SetResult(ColonyDiagnostic.DiagnosticResult result)
	{
		this.LatestResult = result;
	}

	// Token: 0x170005BD RID: 1469
	// (get) Token: 0x06005F24 RID: 24356 RVA: 0x000DE1E4 File Offset: 0x000DC3E4
	protected string NO_MINIONS
	{
		get
		{
			return this.IsWorldModuleInterior ? UI.COLONY_DIAGNOSTICS.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.NO_MINIONS_PLANETOID;
		}
	}

	// Token: 0x04004392 RID: 17298
	private int clickThroughIndex;

	// Token: 0x04004393 RID: 17299
	private List<GameObject> aggregatedUniqueClickThroughObjects = new List<GameObject>();

	// Token: 0x04004395 RID: 17301
	public string name;

	// Token: 0x04004396 RID: 17302
	public string id;

	// Token: 0x04004398 RID: 17304
	public string icon = "icon_errand_operate";

	// Token: 0x04004399 RID: 17305
	private Dictionary<string, DiagnosticCriterion> criteria = new Dictionary<string, DiagnosticCriterion>();

	// Token: 0x0400439A RID: 17306
	public ColonyDiagnostic.PresentationSetting presentationSetting;

	// Token: 0x0400439B RID: 17307
	private ColonyDiagnostic.DiagnosticResult latestResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.NO_DATA, null);

	// Token: 0x0400439C RID: 17308
	public Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color> colors = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color>();

	// Token: 0x0400439D RID: 17309
	public Tracker tracker;

	// Token: 0x0400439E RID: 17310
	protected float trackerSampleCountSeconds = 4f;

	// Token: 0x02001221 RID: 4641
	public enum PresentationSetting
	{
		// Token: 0x040043A0 RID: 17312
		AverageValue,
		// Token: 0x040043A1 RID: 17313
		CurrentValue
	}

	// Token: 0x02001222 RID: 4642
	public struct DiagnosticResult
	{
		// Token: 0x06005F25 RID: 24357 RVA: 0x000DE1FF File Offset: 0x000DC3FF
		public DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion opinion, string message, global::Tuple<Vector3, GameObject> clickThroughTarget = null)
		{
			this.message = message;
			this.opinion = opinion;
			this.clickThroughTarget = null;
			this.clickThroughObjects = null;
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06005F27 RID: 24359 RVA: 0x000DE226 File Offset: 0x000DC426
		// (set) Token: 0x06005F26 RID: 24358 RVA: 0x000DE21D File Offset: 0x000DC41D
		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
			}
		}

		// Token: 0x06005F28 RID: 24360 RVA: 0x002A7F2C File Offset: 0x002A612C
		public string GetFormattedMessage()
		{
			switch (this.opinion)
			{
			case ColonyDiagnostic.DiagnosticResult.Opinion.Bad:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.NEGATIVE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Warning:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.NEGATIVE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Concern:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.WARNING_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion:
			case ColonyDiagnostic.DiagnosticResult.Opinion.Normal:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.WHITE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Good:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.POSITIVE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			}
			return this.message;
		}

		// Token: 0x040043A2 RID: 17314
		public ColonyDiagnostic.DiagnosticResult.Opinion opinion;

		// Token: 0x040043A3 RID: 17315
		public global::Tuple<Vector3, GameObject> clickThroughTarget;

		// Token: 0x040043A4 RID: 17316
		public List<GameObject> clickThroughObjects;

		// Token: 0x040043A5 RID: 17317
		private string message;

		// Token: 0x02001223 RID: 4643
		public enum Opinion
		{
			// Token: 0x040043A7 RID: 17319
			Unset,
			// Token: 0x040043A8 RID: 17320
			DuplicantThreatening,
			// Token: 0x040043A9 RID: 17321
			Bad,
			// Token: 0x040043AA RID: 17322
			Warning,
			// Token: 0x040043AB RID: 17323
			Concern,
			// Token: 0x040043AC RID: 17324
			Suggestion,
			// Token: 0x040043AD RID: 17325
			Tutorial,
			// Token: 0x040043AE RID: 17326
			Normal,
			// Token: 0x040043AF RID: 17327
			Good
		}
	}
}
