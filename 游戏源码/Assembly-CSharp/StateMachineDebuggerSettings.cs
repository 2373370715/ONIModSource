using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008FA RID: 2298
public class StateMachineDebuggerSettings : ScriptableObject
{
	// Token: 0x060028D1 RID: 10449 RVA: 0x000BA886 File Offset: 0x000B8A86
	public IEnumerator<StateMachineDebuggerSettings.Entry> GetEnumerator()
	{
		return this.entries.GetEnumerator();
	}

	// Token: 0x060028D2 RID: 10450 RVA: 0x000BA898 File Offset: 0x000B8A98
	public static StateMachineDebuggerSettings Get()
	{
		if (StateMachineDebuggerSettings._Instance == null)
		{
			StateMachineDebuggerSettings._Instance = Resources.Load<StateMachineDebuggerSettings>("StateMachineDebuggerSettings");
			StateMachineDebuggerSettings._Instance.Initialize();
		}
		return StateMachineDebuggerSettings._Instance;
	}

	// Token: 0x060028D3 RID: 10451 RVA: 0x001D3A9C File Offset: 0x001D1C9C
	private void Initialize()
	{
		foreach (Type type in App.GetCurrentDomainTypes())
		{
			if (typeof(StateMachine).IsAssignableFrom(type))
			{
				this.CreateEntry(type);
			}
		}
		this.entries.RemoveAll((StateMachineDebuggerSettings.Entry x) => x.type == null);
	}

	// Token: 0x060028D4 RID: 10452 RVA: 0x001D3B2C File Offset: 0x001D1D2C
	public StateMachineDebuggerSettings.Entry CreateEntry(Type type)
	{
		foreach (StateMachineDebuggerSettings.Entry entry in this.entries)
		{
			if (type.FullName == entry.typeName)
			{
				entry.type = type;
				return entry;
			}
		}
		StateMachineDebuggerSettings.Entry entry2 = new StateMachineDebuggerSettings.Entry(type);
		this.entries.Add(entry2);
		return entry2;
	}

	// Token: 0x060028D5 RID: 10453 RVA: 0x000BA8C5 File Offset: 0x000B8AC5
	public void Clear()
	{
		this.entries.Clear();
		this.Initialize();
	}

	// Token: 0x04001B35 RID: 6965
	public List<StateMachineDebuggerSettings.Entry> entries = new List<StateMachineDebuggerSettings.Entry>();

	// Token: 0x04001B36 RID: 6966
	private static StateMachineDebuggerSettings _Instance;

	// Token: 0x020008FB RID: 2299
	[Serializable]
	public class Entry
	{
		// Token: 0x060028D7 RID: 10455 RVA: 0x000BA8EB File Offset: 0x000B8AEB
		public Entry(Type type)
		{
			this.typeName = type.FullName;
			this.type = type;
		}

		// Token: 0x04001B37 RID: 6967
		public Type type;

		// Token: 0x04001B38 RID: 6968
		public string typeName;

		// Token: 0x04001B39 RID: 6969
		public bool breakOnGoTo;

		// Token: 0x04001B3A RID: 6970
		public bool enableConsoleLogging;

		// Token: 0x04001B3B RID: 6971
		public bool saveHistory;
	}
}
