using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000ADA RID: 2778
[AddComponentMenu("KMonoBehaviour/scripts/ReportManager")]
public class ReportManager : KMonoBehaviour
{
	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06003416 RID: 13334 RVA: 0x000C1FEF File Offset: 0x000C01EF
	public List<ReportManager.DailyReport> reports
	{
		get
		{
			return this.dailyReports;
		}
	}

	// Token: 0x06003417 RID: 13335 RVA: 0x000C1FF7 File Offset: 0x000C01F7
	public static void DestroyInstance()
	{
		ReportManager.Instance = null;
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06003418 RID: 13336 RVA: 0x000C1FFF File Offset: 0x000C01FF
	// (set) Token: 0x06003419 RID: 13337 RVA: 0x000C2006 File Offset: 0x000C0206
	public static ReportManager Instance { get; private set; }

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x0600341A RID: 13338 RVA: 0x000C200E File Offset: 0x000C020E
	public ReportManager.DailyReport TodaysReport
	{
		get
		{
			return this.todaysReport;
		}
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x0600341B RID: 13339 RVA: 0x000C2016 File Offset: 0x000C0216
	public ReportManager.DailyReport YesterdaysReport
	{
		get
		{
			if (this.dailyReports.Count <= 1)
			{
				return null;
			}
			return this.dailyReports[this.dailyReports.Count - 1];
		}
	}

	// Token: 0x0600341C RID: 13340 RVA: 0x000C2040 File Offset: 0x000C0240
	protected override void OnPrefabInit()
	{
		ReportManager.Instance = this;
		base.Subscribe(Game.Instance.gameObject, -1917495436, new Action<object>(this.OnSaveGameReady));
		this.noteStorage = new ReportManager.NoteStorage();
	}

	// Token: 0x0600341D RID: 13341 RVA: 0x000C1FF7 File Offset: 0x000C01F7
	protected override void OnCleanUp()
	{
		ReportManager.Instance = null;
	}

	// Token: 0x0600341E RID: 13342 RVA: 0x000C2075 File Offset: 0x000C0275
	[CustomSerialize]
	private void CustomSerialize(BinaryWriter writer)
	{
		writer.Write(0);
		this.noteStorage.Serialize(writer);
	}

	// Token: 0x0600341F RID: 13343 RVA: 0x00208E7C File Offset: 0x0020707C
	[CustomDeserialize]
	private void CustomDeserialize(IReader reader)
	{
		if (this.noteStorageBytes == null)
		{
			global::Debug.Assert(reader.ReadInt32() == 0);
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(reader.RawBytes()));
			binaryReader.BaseStream.Position = (long)reader.Position;
			this.noteStorage.Deserialize(binaryReader);
			reader.SkipBytes((int)binaryReader.BaseStream.Position - reader.Position);
		}
	}

	// Token: 0x06003420 RID: 13344 RVA: 0x000C208A File Offset: 0x000C028A
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.noteStorageBytes != null)
		{
			this.noteStorage.Deserialize(new BinaryReader(new MemoryStream(this.noteStorageBytes)));
			this.noteStorageBytes = null;
		}
	}

	// Token: 0x06003421 RID: 13345 RVA: 0x00208EE8 File Offset: 0x002070E8
	private void OnSaveGameReady(object data)
	{
		base.Subscribe(GameClock.Instance.gameObject, -722330267, new Action<object>(this.OnNightTime));
		if (this.todaysReport == null)
		{
			this.todaysReport = new ReportManager.DailyReport(this);
			this.todaysReport.day = GameUtil.GetCurrentCycle();
		}
	}

	// Token: 0x06003422 RID: 13346 RVA: 0x000C20B6 File Offset: 0x000C02B6
	public void ReportValue(ReportManager.ReportType reportType, float value, string note = null, string context = null)
	{
		this.TodaysReport.AddData(reportType, value, note, context);
	}

	// Token: 0x06003423 RID: 13347 RVA: 0x00208F3C File Offset: 0x0020713C
	private void OnNightTime(object data)
	{
		this.dailyReports.Add(this.todaysReport);
		int day = this.todaysReport.day;
		ManagementMenuNotification notification = new ManagementMenuNotification(global::Action.ManageReport, NotificationValence.Good, null, string.Format(UI.ENDOFDAYREPORT.NOTIFICATION_TITLE, day), NotificationType.Good, (List<Notification> n, object d) => string.Format(UI.ENDOFDAYREPORT.NOTIFICATION_TOOLTIP, day), null, true, 0f, delegate(object d)
		{
			ManagementMenu.Instance.OpenReports(day);
		}, null, null, true);
		if (this.notifier == null)
		{
			global::Debug.LogError("Cant notify, null notifier");
		}
		else
		{
			this.notifier.Add(notification, "");
		}
		this.todaysReport = new ReportManager.DailyReport(this);
		this.todaysReport.day = GameUtil.GetCurrentCycle() + 1;
	}

	// Token: 0x06003424 RID: 13348 RVA: 0x00209004 File Offset: 0x00207204
	public ReportManager.DailyReport FindReport(int day)
	{
		foreach (ReportManager.DailyReport dailyReport in this.dailyReports)
		{
			if (dailyReport.day == day)
			{
				return dailyReport;
			}
		}
		if (this.todaysReport.day == day)
		{
			return this.todaysReport;
		}
		return null;
	}

	// Token: 0x06003425 RID: 13349 RVA: 0x00209078 File Offset: 0x00207278
	public ReportManager()
	{
		Dictionary<ReportManager.ReportType, ReportManager.ReportGroup> dictionary = new Dictionary<ReportManager.ReportType, ReportManager.ReportGroup>();
		dictionary.Add(ReportManager.ReportType.DuplicantHeader, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.DUPLICANT_DETAILS_HEADER, "", "", ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, null));
		dictionary.Add(ReportManager.ReportType.CaloriesCreated, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedCalories(v, GameUtil.TimeSlice.None, true), true, 1, UI.ENDOFDAYREPORT.CALORIES_CREATED.NAME, UI.ENDOFDAYREPORT.CALORIES_CREATED.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CALORIES_CREATED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.StressDelta, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v, GameUtil.TimeSlice.None), true, 1, UI.ENDOFDAYREPORT.STRESS_DELTA.NAME, UI.ENDOFDAYREPORT.STRESS_DELTA.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.STRESS_DELTA.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.DiseaseAdded, new ReportManager.ReportGroup(null, false, 1, UI.ENDOFDAYREPORT.DISEASE_ADDED.NAME, UI.ENDOFDAYREPORT.DISEASE_ADDED.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.DISEASE_ADDED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.DiseaseStatus, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedDiseaseAmount((int)v, GameUtil.TimeSlice.None), true, 1, UI.ENDOFDAYREPORT.DISEASE_STATUS.NAME, UI.ENDOFDAYREPORT.DISEASE_STATUS.TOOLTIP, UI.ENDOFDAYREPORT.DISEASE_STATUS.TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.LevelUp, new ReportManager.ReportGroup(null, false, 1, UI.ENDOFDAYREPORT.LEVEL_UP.NAME, UI.ENDOFDAYREPORT.LEVEL_UP.TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ToiletIncident, new ReportManager.ReportGroup(null, false, 1, UI.ENDOFDAYREPORT.TOILET_INCIDENT.NAME, UI.ENDOFDAYREPORT.TOILET_INCIDENT.TOOLTIP, UI.ENDOFDAYREPORT.TOILET_INCIDENT.TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ChoreStatus, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.CHORE_STATUS.NAME, UI.ENDOFDAYREPORT.CHORE_STATUS.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CHORE_STATUS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.DomesticatedCritters, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.NAME, UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.WildCritters, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.NAME, UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.RocketsInFlight, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.NAME, UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.TimeSpentHeader, new ReportManager.ReportGroup(null, true, 2, UI.ENDOFDAYREPORT.TIME_DETAILS_HEADER, "", "", ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, null));
		dictionary.Add(ReportManager.ReportType.WorkTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.WORK_TIME.NAME, UI.ENDOFDAYREPORT.WORK_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.TravelTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.TRAVEL_TIME.NAME, UI.ENDOFDAYREPORT.TRAVEL_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.PersonalTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.PERSONAL_TIME.NAME, UI.ENDOFDAYREPORT.PERSONAL_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.IdleTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.IDLE_TIME.NAME, UI.ENDOFDAYREPORT.IDLE_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.BaseHeader, new ReportManager.ReportGroup(null, true, 3, UI.ENDOFDAYREPORT.BASE_DETAILS_HEADER, "", "", ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, null));
		dictionary.Add(ReportManager.ReportType.OxygenCreated, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), true, 3, UI.ENDOFDAYREPORT.OXYGEN_CREATED.NAME, UI.ENDOFDAYREPORT.OXYGEN_CREATED.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.OXYGEN_CREATED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.EnergyCreated, new ReportManager.ReportGroup(new ReportManager.FormattingFn(GameUtil.GetFormattedRoundedJoules), true, 3, UI.ENDOFDAYREPORT.ENERGY_USAGE.NAME, UI.ENDOFDAYREPORT.ENERGY_USAGE.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.ENERGY_USAGE.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.EnergyWasted, new ReportManager.ReportGroup(new ReportManager.FormattingFn(GameUtil.GetFormattedRoundedJoules), true, 3, UI.ENDOFDAYREPORT.ENERGY_WASTED.NAME, UI.ENDOFDAYREPORT.NONE, UI.ENDOFDAYREPORT.ENERGY_WASTED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ContaminatedOxygenToilet, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), false, 3, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.NAME, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ContaminatedOxygenSublimation, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), false, 3, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.NAME, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		this.ReportGroups = dictionary;
		this.dailyReports = new List<ReportManager.DailyReport>();
		base..ctor();
	}

	// Token: 0x04002312 RID: 8978
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04002313 RID: 8979
	private ReportManager.NoteStorage noteStorage;

	// Token: 0x04002314 RID: 8980
	public Dictionary<ReportManager.ReportType, ReportManager.ReportGroup> ReportGroups;

	// Token: 0x04002315 RID: 8981
	[Serialize]
	private List<ReportManager.DailyReport> dailyReports;

	// Token: 0x04002316 RID: 8982
	[Serialize]
	private ReportManager.DailyReport todaysReport;

	// Token: 0x04002317 RID: 8983
	[Serialize]
	private byte[] noteStorageBytes;

	// Token: 0x02000ADB RID: 2779
	// (Invoke) Token: 0x06003427 RID: 13351
	public delegate string FormattingFn(float v);

	// Token: 0x02000ADC RID: 2780
	// (Invoke) Token: 0x0600342B RID: 13355
	public delegate string GroupFormattingFn(float v, float numEntries);

	// Token: 0x02000ADD RID: 2781
	public enum ReportType
	{
		// Token: 0x0400231A RID: 8986
		DuplicantHeader,
		// Token: 0x0400231B RID: 8987
		CaloriesCreated,
		// Token: 0x0400231C RID: 8988
		StressDelta,
		// Token: 0x0400231D RID: 8989
		LevelUp,
		// Token: 0x0400231E RID: 8990
		DiseaseStatus,
		// Token: 0x0400231F RID: 8991
		DiseaseAdded,
		// Token: 0x04002320 RID: 8992
		ToiletIncident,
		// Token: 0x04002321 RID: 8993
		ChoreStatus,
		// Token: 0x04002322 RID: 8994
		TimeSpentHeader,
		// Token: 0x04002323 RID: 8995
		TimeSpent,
		// Token: 0x04002324 RID: 8996
		WorkTime,
		// Token: 0x04002325 RID: 8997
		TravelTime,
		// Token: 0x04002326 RID: 8998
		PersonalTime,
		// Token: 0x04002327 RID: 8999
		IdleTime,
		// Token: 0x04002328 RID: 9000
		BaseHeader,
		// Token: 0x04002329 RID: 9001
		ContaminatedOxygenFlatulence,
		// Token: 0x0400232A RID: 9002
		ContaminatedOxygenToilet,
		// Token: 0x0400232B RID: 9003
		ContaminatedOxygenSublimation,
		// Token: 0x0400232C RID: 9004
		OxygenCreated,
		// Token: 0x0400232D RID: 9005
		EnergyCreated,
		// Token: 0x0400232E RID: 9006
		EnergyWasted,
		// Token: 0x0400232F RID: 9007
		DomesticatedCritters,
		// Token: 0x04002330 RID: 9008
		WildCritters,
		// Token: 0x04002331 RID: 9009
		RocketsInFlight
	}

	// Token: 0x02000ADE RID: 2782
	public struct ReportGroup
	{
		// Token: 0x0600342E RID: 13358 RVA: 0x00209680 File Offset: 0x00207880
		public ReportGroup(ReportManager.FormattingFn formatfn, bool reportIfZero, int group, string stringKey, string positiveTooltip, string negativeTooltip, ReportManager.ReportEntry.Order pos_note_order = ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order neg_note_order = ReportManager.ReportEntry.Order.Unordered, bool is_header = false, ReportManager.GroupFormattingFn group_format_fn = null)
		{
			ReportManager.FormattingFn formattingFn;
			if (formatfn == null)
			{
				formattingFn = ((float v) => v.ToString());
			}
			else
			{
				formattingFn = formatfn;
			}
			this.formatfn = formattingFn;
			this.groupFormatfn = group_format_fn;
			this.stringKey = stringKey;
			this.positiveTooltip = positiveTooltip;
			this.negativeTooltip = negativeTooltip;
			this.reportIfZero = reportIfZero;
			this.group = group;
			this.posNoteOrder = pos_note_order;
			this.negNoteOrder = neg_note_order;
			this.isHeader = is_header;
		}

		// Token: 0x04002332 RID: 9010
		public ReportManager.FormattingFn formatfn;

		// Token: 0x04002333 RID: 9011
		public ReportManager.GroupFormattingFn groupFormatfn;

		// Token: 0x04002334 RID: 9012
		public string stringKey;

		// Token: 0x04002335 RID: 9013
		public string positiveTooltip;

		// Token: 0x04002336 RID: 9014
		public string negativeTooltip;

		// Token: 0x04002337 RID: 9015
		public bool reportIfZero;

		// Token: 0x04002338 RID: 9016
		public int group;

		// Token: 0x04002339 RID: 9017
		public bool isHeader;

		// Token: 0x0400233A RID: 9018
		public ReportManager.ReportEntry.Order posNoteOrder;

		// Token: 0x0400233B RID: 9019
		public ReportManager.ReportEntry.Order negNoteOrder;
	}

	// Token: 0x02000AE0 RID: 2784
	[SerializationConfig(MemberSerialization.OptIn)]
	public class ReportEntry
	{
		// Token: 0x06003432 RID: 13362 RVA: 0x00209700 File Offset: 0x00207900
		public ReportEntry(ReportManager.ReportType reportType, int note_storage_id, string context, bool is_child = false)
		{
			this.reportType = reportType;
			this.context = context;
			this.isChild = is_child;
			this.accumulate = 0f;
			this.accPositive = 0f;
			this.accNegative = 0f;
			this.noteStorageId = note_storage_id;
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06003433 RID: 13363 RVA: 0x000C20DD File Offset: 0x000C02DD
		public float Positive
		{
			get
			{
				return this.accPositive;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06003434 RID: 13364 RVA: 0x000C20E5 File Offset: 0x000C02E5
		public float Negative
		{
			get
			{
				return this.accNegative;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06003435 RID: 13365 RVA: 0x000C20ED File Offset: 0x000C02ED
		public float Net
		{
			get
			{
				return this.accPositive + this.accNegative;
			}
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x000C20FC File Offset: 0x000C02FC
		[OnDeserializing]
		private void OnDeserialize()
		{
			this.contextEntries.Clear();
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x000C2109 File Offset: 0x000C0309
		public void IterateNotes(Action<ReportManager.ReportEntry.Note> callback)
		{
			ReportManager.Instance.noteStorage.IterateNotes(this.noteStorageId, callback);
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x000C2121 File Offset: 0x000C0321
		[OnDeserialized]
		private void OnDeserialized()
		{
			if (this.gameHash != -1)
			{
				this.reportType = (ReportManager.ReportType)this.gameHash;
				this.gameHash = -1;
			}
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x00209758 File Offset: 0x00207958
		public void AddData(ReportManager.NoteStorage note_storage, float value, string note = null, string dataContext = null)
		{
			this.AddActualData(note_storage, value, note);
			if (dataContext != null)
			{
				ReportManager.ReportEntry reportEntry = null;
				for (int i = 0; i < this.contextEntries.Count; i++)
				{
					if (this.contextEntries[i].context == dataContext)
					{
						reportEntry = this.contextEntries[i];
						break;
					}
				}
				if (reportEntry == null)
				{
					reportEntry = new ReportManager.ReportEntry(this.reportType, note_storage.GetNewNoteId(), dataContext, true);
					this.contextEntries.Add(reportEntry);
				}
				reportEntry.AddActualData(note_storage, value, note);
			}
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x002097E4 File Offset: 0x002079E4
		private void AddActualData(ReportManager.NoteStorage note_storage, float value, string note = null)
		{
			this.accumulate += value;
			if (value > 0f)
			{
				this.accPositive += value;
			}
			else
			{
				this.accNegative += value;
			}
			if (note != null)
			{
				note_storage.Add(this.noteStorageId, value, note);
			}
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x000C213F File Offset: 0x000C033F
		public bool HasContextEntries()
		{
			return this.contextEntries.Count > 0;
		}

		// Token: 0x0400233E RID: 9022
		[Serialize]
		public int noteStorageId;

		// Token: 0x0400233F RID: 9023
		[Serialize]
		public int gameHash = -1;

		// Token: 0x04002340 RID: 9024
		[Serialize]
		public ReportManager.ReportType reportType;

		// Token: 0x04002341 RID: 9025
		[Serialize]
		public string context;

		// Token: 0x04002342 RID: 9026
		[Serialize]
		public float accumulate;

		// Token: 0x04002343 RID: 9027
		[Serialize]
		public float accPositive;

		// Token: 0x04002344 RID: 9028
		[Serialize]
		public float accNegative;

		// Token: 0x04002345 RID: 9029
		[Serialize]
		public ArrayRef<ReportManager.ReportEntry> contextEntries;

		// Token: 0x04002346 RID: 9030
		public bool isChild;

		// Token: 0x02000AE1 RID: 2785
		public struct Note
		{
			// Token: 0x0600343C RID: 13372 RVA: 0x000C214F File Offset: 0x000C034F
			public Note(float value, string note)
			{
				this.value = value;
				this.note = note;
			}

			// Token: 0x04002347 RID: 9031
			public float value;

			// Token: 0x04002348 RID: 9032
			public string note;
		}

		// Token: 0x02000AE2 RID: 2786
		public enum Order
		{
			// Token: 0x0400234A RID: 9034
			Unordered,
			// Token: 0x0400234B RID: 9035
			Ascending,
			// Token: 0x0400234C RID: 9036
			Descending
		}
	}

	// Token: 0x02000AE3 RID: 2787
	public class DailyReport
	{
		// Token: 0x0600343D RID: 13373 RVA: 0x00209838 File Offset: 0x00207A38
		public DailyReport(ReportManager manager)
		{
			foreach (KeyValuePair<ReportManager.ReportType, ReportManager.ReportGroup> keyValuePair in manager.ReportGroups)
			{
				this.reportEntries.Add(new ReportManager.ReportEntry(keyValuePair.Key, this.noteStorage.GetNewNoteId(), null, false));
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600343E RID: 13374 RVA: 0x000C215F File Offset: 0x000C035F
		private ReportManager.NoteStorage noteStorage
		{
			get
			{
				return ReportManager.Instance.noteStorage;
			}
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x002098BC File Offset: 0x00207ABC
		public ReportManager.ReportEntry GetEntry(ReportManager.ReportType reportType)
		{
			for (int i = 0; i < this.reportEntries.Count; i++)
			{
				ReportManager.ReportEntry reportEntry = this.reportEntries[i];
				if (reportEntry.reportType == reportType)
				{
					return reportEntry;
				}
			}
			ReportManager.ReportEntry reportEntry2 = new ReportManager.ReportEntry(reportType, this.noteStorage.GetNewNoteId(), null, false);
			this.reportEntries.Add(reportEntry2);
			return reportEntry2;
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x000C216B File Offset: 0x000C036B
		public void AddData(ReportManager.ReportType reportType, float value, string note = null, string context = null)
		{
			this.GetEntry(reportType).AddData(this.noteStorage, value, note, context);
		}

		// Token: 0x0400234D RID: 9037
		[Serialize]
		public int day;

		// Token: 0x0400234E RID: 9038
		[Serialize]
		public List<ReportManager.ReportEntry> reportEntries = new List<ReportManager.ReportEntry>();
	}

	// Token: 0x02000AE4 RID: 2788
	public class NoteStorage
	{
		// Token: 0x06003441 RID: 13377 RVA: 0x000C2183 File Offset: 0x000C0383
		public NoteStorage()
		{
			this.noteEntries = new ReportManager.NoteStorage.NoteEntries();
			this.stringTable = new ReportManager.NoteStorage.StringTable();
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x00209918 File Offset: 0x00207B18
		public void Add(int report_entry_id, float value, string note)
		{
			int note_id = this.stringTable.AddString(note, 6);
			this.noteEntries.Add(report_entry_id, value, note_id);
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x00209944 File Offset: 0x00207B44
		public int GetNewNoteId()
		{
			int result = this.nextNoteId + 1;
			this.nextNoteId = result;
			return result;
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x000C21A1 File Offset: 0x000C03A1
		public void IterateNotes(int report_entry_id, Action<ReportManager.ReportEntry.Note> callback)
		{
			this.noteEntries.IterateNotes(this.stringTable, report_entry_id, callback);
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x000C21B6 File Offset: 0x000C03B6
		public void Serialize(BinaryWriter writer)
		{
			writer.Write(6);
			writer.Write(this.nextNoteId);
			this.stringTable.Serialize(writer);
			this.noteEntries.Serialize(writer);
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x00209964 File Offset: 0x00207B64
		public void Deserialize(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			if (num < 5)
			{
				return;
			}
			this.nextNoteId = reader.ReadInt32();
			this.stringTable.Deserialize(reader, num);
			this.noteEntries.Deserialize(reader, num);
		}

		// Token: 0x0400234F RID: 9039
		public const int SERIALIZATION_VERSION = 6;

		// Token: 0x04002350 RID: 9040
		private int nextNoteId;

		// Token: 0x04002351 RID: 9041
		private ReportManager.NoteStorage.NoteEntries noteEntries;

		// Token: 0x04002352 RID: 9042
		private ReportManager.NoteStorage.StringTable stringTable;

		// Token: 0x02000AE5 RID: 2789
		private class StringTable
		{
			// Token: 0x06003447 RID: 13383 RVA: 0x002099A4 File Offset: 0x00207BA4
			public int AddString(string str, int version = 6)
			{
				int num = Hash.SDBMLower(str);
				this.strings[num] = str;
				return num;
			}

			// Token: 0x06003448 RID: 13384 RVA: 0x002099C8 File Offset: 0x00207BC8
			public string GetStringByHash(int hash)
			{
				string result = "";
				this.strings.TryGetValue(hash, out result);
				return result;
			}

			// Token: 0x06003449 RID: 13385 RVA: 0x002099EC File Offset: 0x00207BEC
			public void Serialize(BinaryWriter writer)
			{
				writer.Write(this.strings.Count);
				foreach (KeyValuePair<int, string> keyValuePair in this.strings)
				{
					writer.Write(keyValuePair.Value);
				}
			}

			// Token: 0x0600344A RID: 13386 RVA: 0x00209A58 File Offset: 0x00207C58
			public void Deserialize(BinaryReader reader, int version)
			{
				int num = reader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					string str = reader.ReadString();
					this.AddString(str, version);
				}
			}

			// Token: 0x04002353 RID: 9043
			private Dictionary<int, string> strings = new Dictionary<int, string>();
		}

		// Token: 0x02000AE6 RID: 2790
		private class NoteEntries
		{
			// Token: 0x0600344C RID: 13388 RVA: 0x00209A88 File Offset: 0x00207C88
			public void Add(int report_entry_id, float value, int note_id)
			{
				Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary;
				if (!this.entries.TryGetValue(report_entry_id, out dictionary))
				{
					dictionary = new Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>(ReportManager.NoteStorage.NoteEntries.sKeyComparer);
					this.entries[report_entry_id] = dictionary;
				}
				ReportManager.NoteStorage.NoteEntries.NoteEntryKey noteEntryKey = new ReportManager.NoteStorage.NoteEntries.NoteEntryKey
				{
					noteHash = note_id,
					isPositive = (value > 0f)
				};
				if (dictionary.ContainsKey(noteEntryKey))
				{
					Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary2 = dictionary;
					ReportManager.NoteStorage.NoteEntries.NoteEntryKey key = noteEntryKey;
					dictionary2[key] += value;
					return;
				}
				dictionary[noteEntryKey] = value;
			}

			// Token: 0x0600344D RID: 13389 RVA: 0x00209B04 File Offset: 0x00207D04
			public void Serialize(BinaryWriter writer)
			{
				writer.Write(this.entries.Count);
				foreach (KeyValuePair<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>> keyValuePair in this.entries)
				{
					writer.Write(keyValuePair.Key);
					writer.Write(keyValuePair.Value.Count);
					foreach (KeyValuePair<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> keyValuePair2 in keyValuePair.Value)
					{
						writer.Write(keyValuePair2.Key.noteHash);
						writer.Write(keyValuePair2.Key.isPositive);
						writer.WriteSingleFast(keyValuePair2.Value);
					}
				}
			}

			// Token: 0x0600344E RID: 13390 RVA: 0x00209BF4 File Offset: 0x00207DF4
			public void Deserialize(BinaryReader reader, int version)
			{
				if (version < 6)
				{
					OldNoteEntriesV5 oldNoteEntriesV = new OldNoteEntriesV5();
					oldNoteEntriesV.Deserialize(reader);
					foreach (OldNoteEntriesV5.NoteStorageBlock noteStorageBlock in oldNoteEntriesV.storageBlocks)
					{
						for (int i = 0; i < noteStorageBlock.entryCount; i++)
						{
							OldNoteEntriesV5.NoteEntry noteEntry = noteStorageBlock.entries.structs[i];
							this.Add(noteEntry.reportEntryId, noteEntry.value, noteEntry.noteHash);
						}
					}
					return;
				}
				int num = reader.ReadInt32();
				this.entries = new Dictionary<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>>(num);
				for (int j = 0; j < num; j++)
				{
					int key = reader.ReadInt32();
					int num2 = reader.ReadInt32();
					Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary = new Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>(num2, ReportManager.NoteStorage.NoteEntries.sKeyComparer);
					this.entries[key] = dictionary;
					for (int k = 0; k < num2; k++)
					{
						ReportManager.NoteStorage.NoteEntries.NoteEntryKey key2 = new ReportManager.NoteStorage.NoteEntries.NoteEntryKey
						{
							noteHash = reader.ReadInt32(),
							isPositive = reader.ReadBoolean()
						};
						dictionary[key2] = reader.ReadSingle();
					}
				}
			}

			// Token: 0x0600344F RID: 13391 RVA: 0x00209D28 File Offset: 0x00207F28
			public void IterateNotes(ReportManager.NoteStorage.StringTable string_table, int report_entry_id, Action<ReportManager.ReportEntry.Note> callback)
			{
				Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary;
				if (this.entries.TryGetValue(report_entry_id, out dictionary))
				{
					foreach (KeyValuePair<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> keyValuePair in dictionary)
					{
						string stringByHash = string_table.GetStringByHash(keyValuePair.Key.noteHash);
						ReportManager.ReportEntry.Note obj = new ReportManager.ReportEntry.Note(keyValuePair.Value, stringByHash);
						callback(obj);
					}
				}
			}

			// Token: 0x04002354 RID: 9044
			private static ReportManager.NoteStorage.NoteEntries.NoteEntryKeyComparer sKeyComparer = new ReportManager.NoteStorage.NoteEntries.NoteEntryKeyComparer();

			// Token: 0x04002355 RID: 9045
			private Dictionary<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>> entries = new Dictionary<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>>();

			// Token: 0x02000AE7 RID: 2791
			public struct NoteEntryKey
			{
				// Token: 0x04002356 RID: 9046
				public int noteHash;

				// Token: 0x04002357 RID: 9047
				public bool isPositive;
			}

			// Token: 0x02000AE8 RID: 2792
			public class NoteEntryKeyComparer : IEqualityComparer<ReportManager.NoteStorage.NoteEntries.NoteEntryKey>
			{
				// Token: 0x06003452 RID: 13394 RVA: 0x000C2215 File Offset: 0x000C0415
				public bool Equals(ReportManager.NoteStorage.NoteEntries.NoteEntryKey a, ReportManager.NoteStorage.NoteEntries.NoteEntryKey b)
				{
					return a.noteHash == b.noteHash && a.isPositive == b.isPositive;
				}

				// Token: 0x06003453 RID: 13395 RVA: 0x000C2235 File Offset: 0x000C0435
				public int GetHashCode(ReportManager.NoteStorage.NoteEntries.NoteEntryKey a)
				{
					return a.noteHash * (a.isPositive ? 1 : -1);
				}
			}
		}
	}
}
