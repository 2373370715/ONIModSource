using System;
using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class EventInstanceBase : ISaveLoadable
{
		public EventInstanceBase(EventBase ev)
	{
		this.frame = GameClock.Instance.GetFrame();
		this.eventHash = ev.hash;
		this.ev = ev;
	}

		public override string ToString()
	{
		string str = "[" + this.frame.ToString() + "] ";
		if (this.ev != null)
		{
			return str + this.ev.GetDescription(this);
		}
		return str + "Unknown event";
	}

		[Serialize]
	public int frame;

		[Serialize]
	public int eventHash;

		public EventBase ev;
}
