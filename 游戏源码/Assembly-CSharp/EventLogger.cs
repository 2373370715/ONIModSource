using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x020012D1 RID: 4817
[SerializationConfig(MemberSerialization.OptIn)]
public class EventLogger<EventInstanceType, EventType> : KMonoBehaviour, ISaveLoadable where EventInstanceType : EventInstanceBase where EventType : EventBase
{
	// Token: 0x060062E7 RID: 25319 RVA: 0x000E0949 File Offset: 0x000DEB49
	public IEnumerator<EventInstanceType> GetEnumerator()
	{
		return this.EventInstances.GetEnumerator();
	}

	// Token: 0x060062E8 RID: 25320 RVA: 0x002B8348 File Offset: 0x002B6548
	public EventType AddEvent(EventType ev)
	{
		for (int i = 0; i < this.Events.Count; i++)
		{
			if (this.Events[i].hash == ev.hash)
			{
				this.Events[i] = ev;
				return this.Events[i];
			}
		}
		this.Events.Add(ev);
		return ev;
	}

	// Token: 0x060062E9 RID: 25321 RVA: 0x000E095B File Offset: 0x000DEB5B
	public EventInstanceType Add(EventInstanceType ev)
	{
		if (this.EventInstances.Count > 10000)
		{
			this.EventInstances.RemoveAt(0);
		}
		this.EventInstances.Add(ev);
		return ev;
	}

	// Token: 0x060062EA RID: 25322 RVA: 0x002B83B8 File Offset: 0x002B65B8
	[OnDeserialized]
	protected internal void OnDeserialized()
	{
		if (this.EventInstances.Count > 10000)
		{
			this.EventInstances.RemoveRange(0, this.EventInstances.Count - 10000);
		}
		for (int i = 0; i < this.EventInstances.Count; i++)
		{
			for (int j = 0; j < this.Events.Count; j++)
			{
				if (this.Events[j].hash == this.EventInstances[i].eventHash)
				{
					this.EventInstances[i].ev = this.Events[j];
					break;
				}
			}
		}
	}

	// Token: 0x060062EB RID: 25323 RVA: 0x000E0988 File Offset: 0x000DEB88
	public void Clear()
	{
		this.EventInstances.Clear();
	}

	// Token: 0x04004693 RID: 18067
	private const int MAX_NUM_EVENTS = 10000;

	// Token: 0x04004694 RID: 18068
	private List<EventType> Events = new List<EventType>();

	// Token: 0x04004695 RID: 18069
	[Serialize]
	private List<EventInstanceType> EventInstances = new List<EventInstanceType>();
}
