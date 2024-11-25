using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ReportManager")]
public class ReportManager : KMonoBehaviour {
    public delegate string FormattingFn(float v);

    public delegate string GroupFormattingFn(float v, float numEntries);

    public enum ReportType {
        DuplicantHeader,
        CaloriesCreated,
        StressDelta,
        LevelUp,
        DiseaseStatus,
        DiseaseAdded,
        ToiletIncident,
        ChoreStatus,
        TimeSpentHeader,
        TimeSpent,
        WorkTime,
        TravelTime,
        PersonalTime,
        IdleTime,
        BaseHeader,
        ContaminatedOxygenFlatulence,
        ContaminatedOxygenToilet,
        ContaminatedOxygenSublimation,
        OxygenCreated,
        EnergyCreated,
        EnergyWasted,
        DomesticatedCritters,
        WildCritters,
        RocketsInFlight
    }

    private NoteStorage noteStorage;

    [Serialize]
    private byte[] noteStorageBytes;

    [MyCmpAdd]
    private Notifier notifier;

    public Dictionary<ReportType, ReportGroup> ReportGroups;

    public ReportManager() {
        var dictionary = new Dictionary<ReportType, ReportGroup>();
        dictionary.Add(ReportType.DuplicantHeader,
                       new ReportGroup(null,
                                       true,
                                       1,
                                       UI.ENDOFDAYREPORT.DUPLICANT_DETAILS_HEADER,
                                       "",
                                       "",
                                       ReportEntry.Order.Unordered,
                                       ReportEntry.Order.Unordered,
                                       true));

        dictionary.Add(ReportType.CaloriesCreated,
                       new ReportGroup(v => GameUtil.GetFormattedCalories(v),
                                       true,
                                       1,
                                       UI.ENDOFDAYREPORT.CALORIES_CREATED.NAME,
                                       UI.ENDOFDAYREPORT.CALORIES_CREATED.POSITIVE_TOOLTIP,
                                       UI.ENDOFDAYREPORT.CALORIES_CREATED.NEGATIVE_TOOLTIP,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending));

        dictionary.Add(ReportType.StressDelta,
                       new ReportGroup(v => GameUtil.GetFormattedPercent(v),
                                       true,
                                       1,
                                       UI.ENDOFDAYREPORT.STRESS_DELTA.NAME,
                                       UI.ENDOFDAYREPORT.STRESS_DELTA.POSITIVE_TOOLTIP,
                                       UI.ENDOFDAYREPORT.STRESS_DELTA.NEGATIVE_TOOLTIP,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending));

        dictionary.Add(ReportType.DiseaseAdded,
                       new ReportGroup(null,
                                       false,
                                       1,
                                       UI.ENDOFDAYREPORT.DISEASE_ADDED.NAME,
                                       UI.ENDOFDAYREPORT.DISEASE_ADDED.POSITIVE_TOOLTIP,
                                       UI.ENDOFDAYREPORT.DISEASE_ADDED.NEGATIVE_TOOLTIP,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending));

        dictionary.Add(ReportType.DiseaseStatus,
                       new ReportGroup(v => GameUtil.GetFormattedDiseaseAmount((int)v),
                                       true,
                                       1,
                                       UI.ENDOFDAYREPORT.DISEASE_STATUS.NAME,
                                       UI.ENDOFDAYREPORT.DISEASE_STATUS.TOOLTIP,
                                       UI.ENDOFDAYREPORT.DISEASE_STATUS.TOOLTIP,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending));

        dictionary.Add(ReportType.LevelUp,
                       new ReportGroup(null,
                                       false,
                                       1,
                                       UI.ENDOFDAYREPORT.LEVEL_UP.NAME,
                                       UI.ENDOFDAYREPORT.LEVEL_UP.TOOLTIP,
                                       UI.ENDOFDAYREPORT.NONE,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending));

        dictionary.Add(ReportType.ToiletIncident,
                       new ReportGroup(null,
                                       false,
                                       1,
                                       UI.ENDOFDAYREPORT.TOILET_INCIDENT.NAME,
                                       UI.ENDOFDAYREPORT.TOILET_INCIDENT.TOOLTIP,
                                       UI.ENDOFDAYREPORT.TOILET_INCIDENT.TOOLTIP,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending));

        dictionary.Add(ReportType.ChoreStatus,
                       new ReportGroup(null,
                                       true,
                                       1,
                                       UI.ENDOFDAYREPORT.CHORE_STATUS.NAME,
                                       UI.ENDOFDAYREPORT.CHORE_STATUS.POSITIVE_TOOLTIP,
                                       UI.ENDOFDAYREPORT.CHORE_STATUS.NEGATIVE_TOOLTIP,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending));

        dictionary.Add(ReportType.DomesticatedCritters,
                       new ReportGroup(null,
                                       true,
                                       1,
                                       UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.NAME,
                                       UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.POSITIVE_TOOLTIP,
                                       UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.NEGATIVE_TOOLTIP,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending));

        dictionary.Add(ReportType.WildCritters,
                       new ReportGroup(null,
                                       true,
                                       1,
                                       UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.NAME,
                                       UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.POSITIVE_TOOLTIP,
                                       UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.NEGATIVE_TOOLTIP,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending));

        dictionary.Add(ReportType.RocketsInFlight,
                       new ReportGroup(null,
                                       true,
                                       1,
                                       UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.NAME,
                                       UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.POSITIVE_TOOLTIP,
                                       UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.NEGATIVE_TOOLTIP,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending));

        dictionary.Add(ReportType.TimeSpentHeader,
                       new ReportGroup(null,
                                       true,
                                       2,
                                       UI.ENDOFDAYREPORT.TIME_DETAILS_HEADER,
                                       "",
                                       "",
                                       ReportEntry.Order.Unordered,
                                       ReportEntry.Order.Unordered,
                                       true));

        dictionary.Add(ReportType.WorkTime,
                       new ReportGroup(v => GameUtil.GetFormattedPercent(v / 600f * 100f),
                                       true,
                                       2,
                                       UI.ENDOFDAYREPORT.WORK_TIME.NAME,
                                       UI.ENDOFDAYREPORT.WORK_TIME.POSITIVE_TOOLTIP,
                                       UI.ENDOFDAYREPORT.NONE,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending,
                                       false,
                                       (v, num_entries) =>
                                           GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries)));

        dictionary.Add(ReportType.TravelTime,
                       new ReportGroup(v => GameUtil.GetFormattedPercent(v / 600f * 100f),
                                       true,
                                       2,
                                       UI.ENDOFDAYREPORT.TRAVEL_TIME.NAME,
                                       UI.ENDOFDAYREPORT.TRAVEL_TIME.POSITIVE_TOOLTIP,
                                       UI.ENDOFDAYREPORT.NONE,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending,
                                       false,
                                       (v, num_entries) =>
                                           GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries)));

        dictionary.Add(ReportType.PersonalTime,
                       new ReportGroup(v => GameUtil.GetFormattedPercent(v / 600f * 100f),
                                       true,
                                       2,
                                       UI.ENDOFDAYREPORT.PERSONAL_TIME.NAME,
                                       UI.ENDOFDAYREPORT.PERSONAL_TIME.POSITIVE_TOOLTIP,
                                       UI.ENDOFDAYREPORT.NONE,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending,
                                       false,
                                       (v, num_entries) =>
                                           GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries)));

        dictionary.Add(ReportType.IdleTime,
                       new ReportGroup(v => GameUtil.GetFormattedPercent(v / 600f * 100f),
                                       true,
                                       2,
                                       UI.ENDOFDAYREPORT.IDLE_TIME.NAME,
                                       UI.ENDOFDAYREPORT.IDLE_TIME.POSITIVE_TOOLTIP,
                                       UI.ENDOFDAYREPORT.NONE,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending,
                                       false,
                                       (v, num_entries) =>
                                           GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries)));

        dictionary.Add(ReportType.BaseHeader,
                       new ReportGroup(null,
                                       true,
                                       3,
                                       UI.ENDOFDAYREPORT.BASE_DETAILS_HEADER,
                                       "",
                                       "",
                                       ReportEntry.Order.Unordered,
                                       ReportEntry.Order.Unordered,
                                       true));

        dictionary.Add(ReportType.OxygenCreated,
                       new ReportGroup(v => GameUtil.GetFormattedMass(v),
                                       true,
                                       3,
                                       UI.ENDOFDAYREPORT.OXYGEN_CREATED.NAME,
                                       UI.ENDOFDAYREPORT.OXYGEN_CREATED.POSITIVE_TOOLTIP,
                                       UI.ENDOFDAYREPORT.OXYGEN_CREATED.NEGATIVE_TOOLTIP,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending));

        dictionary.Add(ReportType.EnergyCreated,
                       new ReportGroup(GameUtil.GetFormattedRoundedJoules,
                                       true,
                                       3,
                                       UI.ENDOFDAYREPORT.ENERGY_USAGE.NAME,
                                       UI.ENDOFDAYREPORT.ENERGY_USAGE.POSITIVE_TOOLTIP,
                                       UI.ENDOFDAYREPORT.ENERGY_USAGE.NEGATIVE_TOOLTIP,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending));

        dictionary.Add(ReportType.EnergyWasted,
                       new ReportGroup(GameUtil.GetFormattedRoundedJoules,
                                       true,
                                       3,
                                       UI.ENDOFDAYREPORT.ENERGY_WASTED.NAME,
                                       UI.ENDOFDAYREPORT.NONE,
                                       UI.ENDOFDAYREPORT.ENERGY_WASTED.NEGATIVE_TOOLTIP,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending));

        dictionary.Add(ReportType.ContaminatedOxygenToilet,
                       new ReportGroup(v => GameUtil.GetFormattedMass(v),
                                       false,
                                       3,
                                       UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.NAME,
                                       UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.POSITIVE_TOOLTIP,
                                       UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.NEGATIVE_TOOLTIP,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending));

        dictionary.Add(ReportType.ContaminatedOxygenSublimation,
                       new ReportGroup(v => GameUtil.GetFormattedMass(v),
                                       false,
                                       3,
                                       UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.NAME,
                                       UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.POSITIVE_TOOLTIP,
                                       UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.NEGATIVE_TOOLTIP,
                                       ReportEntry.Order.Descending,
                                       ReportEntry.Order.Descending));

        ReportGroups = dictionary;
        reports      = new List<DailyReport>();
        base..ctor();
    }

    [field: Serialize]
    public List<DailyReport> reports { get; }

    public static ReportManager Instance { get; private set; }

    [field: Serialize]
    public DailyReport TodaysReport { get; private set; }

    public DailyReport YesterdaysReport {
        get {
            if (reports.Count <= 1) return null;

            return reports[reports.Count - 1];
        }
    }

    public static void DestroyInstance() { Instance = null; }

    protected override void OnPrefabInit() {
        Instance = this;
        Subscribe(Game.Instance.gameObject, -1917495436, OnSaveGameReady);
        noteStorage = new NoteStorage();
    }

    protected override void OnCleanUp() { Instance = null; }

    [CustomSerialize]
    private void CustomSerialize(BinaryWriter writer) {
        writer.Write(0);
        noteStorage.Serialize(writer);
    }

    [CustomDeserialize]
    private void CustomDeserialize(IReader reader) {
        if (noteStorageBytes == null) {
            Debug.Assert(reader.ReadInt32() == 0);
            var binaryReader = new BinaryReader(new MemoryStream(reader.RawBytes()));
            binaryReader.BaseStream.Position = reader.Position;
            noteStorage.Deserialize(binaryReader);
            reader.SkipBytes((int)binaryReader.BaseStream.Position - reader.Position);
        }
    }

    [OnDeserialized]
    private void OnDeserialized() {
        if (noteStorageBytes != null) {
            noteStorage.Deserialize(new BinaryReader(new MemoryStream(noteStorageBytes)));
            noteStorageBytes = null;
        }
    }

    private void OnSaveGameReady(object data) {
        Subscribe(GameClock.Instance.gameObject, -722330267, OnNightTime);
        if (TodaysReport == null) {
            TodaysReport     = new DailyReport(this);
            TodaysReport.day = GameUtil.GetCurrentCycle();
        }
    }

    public void ReportValue(ReportType reportType, float value, string note = null, string context = null) {
        TodaysReport.AddData(reportType, value, note, context);
    }

    private void OnNightTime(object data) {
        reports.Add(TodaysReport);
        var day = TodaysReport.day;
        var notification = new ManagementMenuNotification(Action.ManageReport,
                                                          NotificationValence.Good,
                                                          null,
                                                          string.Format(UI.ENDOFDAYREPORT.NOTIFICATION_TITLE, day),
                                                          NotificationType.Good,
                                                          (n, d) =>
                                                              string.Format(UI.ENDOFDAYREPORT.NOTIFICATION_TOOLTIP,
                                                                            day),
                                                          null,
                                                          true,
                                                          0f,
                                                          delegate { ManagementMenu.Instance.OpenReports(day); });

        if (notifier == null)
            Debug.LogError("Cant notify, null notifier");
        else
            notifier.Add(notification);

        TodaysReport     = new DailyReport(this);
        TodaysReport.day = GameUtil.GetCurrentCycle() + 1;
    }

    public DailyReport FindReport(int day) {
        foreach (var dailyReport in reports)
            if (dailyReport.day == day)
                return dailyReport;

        if (TodaysReport.day == day) return TodaysReport;

        return null;
    }

    public struct ReportGroup {
        public ReportGroup(FormattingFn      formatfn,
                           bool              reportIfZero,
                           int               group,
                           string            stringKey,
                           string            positiveTooltip,
                           string            negativeTooltip,
                           ReportEntry.Order pos_note_order  = ReportEntry.Order.Unordered,
                           ReportEntry.Order neg_note_order  = ReportEntry.Order.Unordered,
                           bool              is_header       = false,
                           GroupFormattingFn group_format_fn = null) {
            FormattingFn formattingFn;
            if (formatfn == null)
                formattingFn = v => v.ToString();
            else
                formattingFn = formatfn;

            this.formatfn        = formattingFn;
            groupFormatfn        = group_format_fn;
            this.stringKey       = stringKey;
            this.positiveTooltip = positiveTooltip;
            this.negativeTooltip = negativeTooltip;
            this.reportIfZero    = reportIfZero;
            this.group           = group;
            posNoteOrder         = pos_note_order;
            negNoteOrder         = neg_note_order;
            isHeader             = is_header;
        }

        public FormattingFn      formatfn;
        public GroupFormattingFn groupFormatfn;
        public string            stringKey;
        public string            positiveTooltip;
        public string            negativeTooltip;
        public bool              reportIfZero;
        public int               group;
        public bool              isHeader;
        public ReportEntry.Order posNoteOrder;
        public ReportEntry.Order negNoteOrder;
    }

    [SerializationConfig(MemberSerialization.OptIn)]
    public class ReportEntry {
        public enum Order {
            Unordered,
            Ascending,
            Descending
        }

        [Serialize]
        public float accNegative;

        [Serialize]
        public float accPositive;

        [Serialize]
        public float accumulate;

        [Serialize]
        public string context;

        [Serialize]
        public ArrayRef<ReportEntry> contextEntries;

        [Serialize]
        public int gameHash = -1;

        public bool isChild;

        [Serialize]
        public int noteStorageId;

        [Serialize]
        public ReportType reportType;

        public ReportEntry(ReportType reportType, int note_storage_id, string context, bool is_child = false) {
            this.reportType = reportType;
            this.context    = context;
            isChild         = is_child;
            accumulate      = 0f;
            accPositive     = 0f;
            accNegative     = 0f;
            noteStorageId   = note_storage_id;
        }

        public float Positive => accPositive;
        public float Negative => accNegative;
        public float Net      => accPositive + accNegative;

        [OnDeserializing]
        private void OnDeserialize() { contextEntries.Clear(); }

        public void IterateNotes(Action<Note> callback) { Instance.noteStorage.IterateNotes(noteStorageId, callback); }

        [OnDeserialized]
        private void OnDeserialized() {
            if (gameHash != -1) {
                reportType = (ReportType)gameHash;
                gameHash   = -1;
            }
        }

        public void AddData(NoteStorage note_storage, float value, string note = null, string dataContext = null) {
            AddActualData(note_storage, value, note);
            if (dataContext != null) {
                ReportEntry reportEntry = null;
                for (var i = 0; i < contextEntries.Count; i++)
                    if (contextEntries[i].context == dataContext) {
                        reportEntry = contextEntries[i];
                        break;
                    }

                if (reportEntry == null) {
                    reportEntry = new ReportEntry(reportType, note_storage.GetNewNoteId(), dataContext, true);
                    contextEntries.Add(reportEntry);
                }

                reportEntry.AddActualData(note_storage, value, note);
            }
        }

        private void AddActualData(NoteStorage note_storage, float value, string note = null) {
            accumulate += value;
            if (value > 0f)
                accPositive += value;
            else
                accNegative += value;

            if (note != null) note_storage.Add(noteStorageId, value, note);
        }

        public bool HasContextEntries() { return contextEntries.Count > 0; }

        public struct Note {
            public Note(float value, string note) {
                this.value = value;
                this.note  = note;
            }

            public float  value;
            public string note;
        }
    }

    public class DailyReport {
        [Serialize]
        public int day;

        [Serialize]
        public List<ReportEntry> reportEntries = new List<ReportEntry>();

        public DailyReport(ReportManager manager) {
            foreach (var keyValuePair in manager.ReportGroups)
                reportEntries.Add(new ReportEntry(keyValuePair.Key, noteStorage.GetNewNoteId(), null));
        }

        private NoteStorage noteStorage => Instance.noteStorage;

        public ReportEntry GetEntry(ReportType reportType) {
            for (var i = 0; i < reportEntries.Count; i++) {
                var reportEntry = reportEntries[i];
                if (reportEntry.reportType == reportType) return reportEntry;
            }

            var reportEntry2 = new ReportEntry(reportType, noteStorage.GetNewNoteId(), null);
            reportEntries.Add(reportEntry2);
            return reportEntry2;
        }

        public void AddData(ReportType reportType, float value, string note = null, string context = null) {
            GetEntry(reportType).AddData(noteStorage, value, note, context);
        }
    }

    public class NoteStorage {
        public const     int         SERIALIZATION_VERSION = 6;
        private          int         nextNoteId;
        private readonly NoteEntries noteEntries;
        private readonly StringTable stringTable;

        public NoteStorage() {
            noteEntries = new NoteEntries();
            stringTable = new StringTable();
        }

        public void Add(int report_entry_id, float value, string note) {
            var note_id = stringTable.AddString(note);
            noteEntries.Add(report_entry_id, value, note_id);
        }

        public int GetNewNoteId() {
            var result = nextNoteId + 1;
            nextNoteId = result;
            return result;
        }

        public void IterateNotes(int report_entry_id, Action<ReportEntry.Note> callback) {
            noteEntries.IterateNotes(stringTable, report_entry_id, callback);
        }

        public void Serialize(BinaryWriter writer) {
            writer.Write(6);
            writer.Write(nextNoteId);
            stringTable.Serialize(writer);
            noteEntries.Serialize(writer);
        }

        public void Deserialize(BinaryReader reader) {
            var num = reader.ReadInt32();
            if (num < 5) return;

            nextNoteId = reader.ReadInt32();
            stringTable.Deserialize(reader, num);
            noteEntries.Deserialize(reader, num);
        }

        private class StringTable {
            private readonly Dictionary<int, string> strings = new Dictionary<int, string>();

            public int AddString(string str, int version = 6) {
                var num = Hash.SDBMLower(str);
                strings[num] = str;
                return num;
            }

            public string GetStringByHash(int hash) {
                var result = "";
                strings.TryGetValue(hash, out result);
                return result;
            }

            public void Serialize(BinaryWriter writer) {
                writer.Write(strings.Count);
                foreach (var keyValuePair in strings) writer.Write(keyValuePair.Value);
            }

            public void Deserialize(BinaryReader reader, int version) {
                var num = reader.ReadInt32();
                for (var i = 0; i < num; i++) {
                    var str = reader.ReadString();
                    AddString(str, version);
                }
            }
        }

        private class NoteEntries {
            private static readonly NoteEntryKeyComparer sKeyComparer = new NoteEntryKeyComparer();

            private Dictionary<int, Dictionary<NoteEntryKey, float>> entries
                = new Dictionary<int, Dictionary<NoteEntryKey, float>>();

            public void Add(int report_entry_id, float value, int note_id) {
                Dictionary<NoteEntryKey, float> dictionary;
                if (!entries.TryGetValue(report_entry_id, out dictionary)) {
                    dictionary               = new Dictionary<NoteEntryKey, float>(sKeyComparer);
                    entries[report_entry_id] = dictionary;
                }

                var noteEntryKey = new NoteEntryKey { noteHash = note_id, isPositive = value > 0f };
                if (dictionary.ContainsKey(noteEntryKey)) {
                    var dictionary2 = dictionary;
                    var key         = noteEntryKey;
                    dictionary2[key] += value;
                    return;
                }

                dictionary[noteEntryKey] = value;
            }

            public void Serialize(BinaryWriter writer) {
                writer.Write(entries.Count);
                foreach (var keyValuePair in entries) {
                    writer.Write(keyValuePair.Key);
                    writer.Write(keyValuePair.Value.Count);
                    foreach (var keyValuePair2 in keyValuePair.Value) {
                        writer.Write(keyValuePair2.Key.noteHash);
                        writer.Write(keyValuePair2.Key.isPositive);
                        writer.WriteSingleFast(keyValuePair2.Value);
                    }
                }
            }

            public void Deserialize(BinaryReader reader, int version) {
                if (version < 6) {
                    var oldNoteEntriesV = new OldNoteEntriesV5();
                    oldNoteEntriesV.Deserialize(reader);
                    foreach (var noteStorageBlock in oldNoteEntriesV.storageBlocks)
                        for (var i = 0; i < noteStorageBlock.entryCount; i++) {
                            var noteEntry = noteStorageBlock.entries.structs[i];
                            Add(noteEntry.reportEntryId, noteEntry.value, noteEntry.noteHash);
                        }

                    return;
                }

                var num = reader.ReadInt32();
                entries = new Dictionary<int, Dictionary<NoteEntryKey, float>>(num);
                for (var j = 0; j < num; j++) {
                    var key        = reader.ReadInt32();
                    var num2       = reader.ReadInt32();
                    var dictionary = new Dictionary<NoteEntryKey, float>(num2, sKeyComparer);
                    entries[key] = dictionary;
                    for (var k = 0; k < num2; k++) {
                        var key2 = new NoteEntryKey {
                            noteHash = reader.ReadInt32(), isPositive = reader.ReadBoolean()
                        };

                        dictionary[key2] = reader.ReadSingle();
                    }
                }
            }

            public void IterateNotes(StringTable string_table, int report_entry_id, Action<ReportEntry.Note> callback) {
                Dictionary<NoteEntryKey, float> dictionary;
                if (entries.TryGetValue(report_entry_id, out dictionary))
                    foreach (var keyValuePair in dictionary) {
                        var stringByHash = string_table.GetStringByHash(keyValuePair.Key.noteHash);
                        var obj          = new ReportEntry.Note(keyValuePair.Value, stringByHash);
                        callback(obj);
                    }
            }

            public struct NoteEntryKey {
                public int  noteHash;
                public bool isPositive;
            }

            public class NoteEntryKeyComparer : IEqualityComparer<NoteEntryKey> {
                public bool Equals(NoteEntryKey a, NoteEntryKey b) {
                    return a.noteHash == b.noteHash && a.isPositive == b.isPositive;
                }

                public int GetHashCode(NoteEntryKey a) { return a.noteHash * (a.isPositive ? 1 : -1); }
            }
        }
    }
}