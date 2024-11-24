using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

// Token: 0x02001CBE RID: 7358
public class EntryDevLog
{
	// Token: 0x060099B8 RID: 39352 RVA: 0x003B6404 File Offset: 0x003B4604
	[Conditional("UNITY_EDITOR")]
	public void AddModificationRecord(EntryDevLog.ModificationRecord.ActionType actionType, string target, object newValue)
	{
		string author = this.TrimAuthor();
		this.modificationRecords.Add(new EntryDevLog.ModificationRecord(actionType, target, newValue, author));
	}

	// Token: 0x060099B9 RID: 39353 RVA: 0x003B642C File Offset: 0x003B462C
	[Conditional("UNITY_EDITOR")]
	public void InsertModificationRecord(int index, EntryDevLog.ModificationRecord.ActionType actionType, string target, object newValue)
	{
		string author = this.TrimAuthor();
		this.modificationRecords.Insert(index, new EntryDevLog.ModificationRecord(actionType, target, newValue, author));
	}

	// Token: 0x060099BA RID: 39354 RVA: 0x003B6458 File Offset: 0x003B4658
	private string TrimAuthor()
	{
		string text = "";
		string[] array = new string[]
		{
			"Invoke",
			"CreateInstance",
			"AwakeInternal",
			"Internal",
			"<>",
			"YamlDotNet",
			"Deserialize"
		};
		string[] array2 = new string[]
		{
			".ctor",
			"Trigger",
			"AddContentContainerRange",
			"AddContentContainer",
			"InsertContentContainer",
			"KInstantiateUI",
			"Start",
			"InitializeComponentAwake",
			"TrimAuthor",
			"InsertModificationRecord",
			"AddModificationRecord",
			"SetValue",
			"Write"
		};
		StackTrace stackTrace = new StackTrace();
		int i = 0;
		int num = 0;
		int num2 = 3;
		while (i < num2)
		{
			num++;
			if (stackTrace.FrameCount <= num)
			{
				break;
			}
			MethodBase method = stackTrace.GetFrame(num).GetMethod();
			bool flag = false;
			for (int j = 0; j < array.Length; j++)
			{
				flag = (flag || method.Name.Contains(array[j]));
			}
			for (int k = 0; k < array2.Length; k++)
			{
				flag = (flag || method.Name.Contains(array2[k]));
			}
			if (!flag && !stackTrace.GetFrame(num).GetMethod().Name.StartsWith("set_") && !stackTrace.GetFrame(num).GetMethod().Name.StartsWith("Instantiate"))
			{
				if (i != 0)
				{
					text += " < ";
				}
				i++;
				text += stackTrace.GetFrame(num).GetMethod().Name;
			}
		}
		return text;
	}

	// Token: 0x040077EF RID: 30703
	[SerializeField]
	public List<EntryDevLog.ModificationRecord> modificationRecords = new List<EntryDevLog.ModificationRecord>();

	// Token: 0x02001CBF RID: 7359
	public class ModificationRecord
	{
		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x060099BC RID: 39356 RVA: 0x001040C6 File Offset: 0x001022C6
		// (set) Token: 0x060099BD RID: 39357 RVA: 0x001040CE File Offset: 0x001022CE
		public EntryDevLog.ModificationRecord.ActionType actionType { get; private set; }

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x060099BE RID: 39358 RVA: 0x001040D7 File Offset: 0x001022D7
		// (set) Token: 0x060099BF RID: 39359 RVA: 0x001040DF File Offset: 0x001022DF
		public string target { get; private set; }

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x060099C0 RID: 39360 RVA: 0x001040E8 File Offset: 0x001022E8
		// (set) Token: 0x060099C1 RID: 39361 RVA: 0x001040F0 File Offset: 0x001022F0
		public object newValue { get; private set; }

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x060099C2 RID: 39362 RVA: 0x001040F9 File Offset: 0x001022F9
		// (set) Token: 0x060099C3 RID: 39363 RVA: 0x00104101 File Offset: 0x00102301
		public string author { get; private set; }

		// Token: 0x060099C4 RID: 39364 RVA: 0x0010410A File Offset: 0x0010230A
		public ModificationRecord(EntryDevLog.ModificationRecord.ActionType actionType, string target, object newValue, string author)
		{
			this.target = target;
			this.newValue = newValue;
			this.author = author;
			this.actionType = actionType;
		}

		// Token: 0x02001CC0 RID: 7360
		public enum ActionType
		{
			// Token: 0x040077F5 RID: 30709
			Created,
			// Token: 0x040077F6 RID: 30710
			ChangeSubEntry,
			// Token: 0x040077F7 RID: 30711
			ChangeContent,
			// Token: 0x040077F8 RID: 30712
			ValueChange,
			// Token: 0x040077F9 RID: 30713
			YAMLData
		}
	}
}
