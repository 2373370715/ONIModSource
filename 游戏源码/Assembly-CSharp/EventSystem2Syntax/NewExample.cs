using System;

namespace EventSystem2Syntax
{
	// Token: 0x020020DE RID: 8414
	internal class NewExample : KMonoBehaviour2
	{
		// Token: 0x0600B2FF RID: 45823 RVA: 0x0043A21C File Offset: 0x0043841C
		protected override void OnPrefabInit()
		{
			base.Subscribe<NewExample, NewExample.ObjectDestroyedEvent>(new Action<NewExample, NewExample.ObjectDestroyedEvent>(NewExample.OnObjectDestroyed));
			base.Trigger<NewExample.ObjectDestroyedEvent>(new NewExample.ObjectDestroyedEvent
			{
				parameter = false
			});
		}

		// Token: 0x0600B300 RID: 45824 RVA: 0x000A5E40 File Offset: 0x000A4040
		private static void OnObjectDestroyed(NewExample example, NewExample.ObjectDestroyedEvent evt)
		{
		}

		// Token: 0x020020DF RID: 8415
		private struct ObjectDestroyedEvent : IEventData
		{
			// Token: 0x04008D90 RID: 36240
			public bool parameter;
		}
	}
}
