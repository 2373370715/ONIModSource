using System;

namespace EventSystem2Syntax
{
	// Token: 0x020020E0 RID: 8416
	internal class KMonoBehaviour2
	{
		// Token: 0x0600B302 RID: 45826 RVA: 0x000A5E40 File Offset: 0x000A4040
		protected virtual void OnPrefabInit()
		{
		}

		// Token: 0x0600B303 RID: 45827 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void Subscribe(int evt, Action<object> cb)
		{
		}

		// Token: 0x0600B304 RID: 45828 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void Trigger(int evt, object data)
		{
		}

		// Token: 0x0600B305 RID: 45829 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void Subscribe<ListenerType, EventType>(Action<ListenerType, EventType> cb) where EventType : IEventData
		{
		}

		// Token: 0x0600B306 RID: 45830 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void Trigger<EventType>(EventType evt) where EventType : IEventData
		{
		}
	}
}
