using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B84 RID: 15236
	public class EmoteStep
	{
		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x0600EAAE RID: 60078 RVA: 0x0013CEAF File Offset: 0x0013B0AF
		public int Id
		{
			get
			{
				return this.anim.HashValue;
			}
		}

		// Token: 0x0600EAAF RID: 60079 RVA: 0x004CAA04 File Offset: 0x004C8C04
		public HandleVector<EmoteStep.Callbacks>.Handle RegisterCallbacks(Action<GameObject> startedCb, Action<GameObject> finishedCb)
		{
			if (startedCb == null && finishedCb == null)
			{
				return HandleVector<EmoteStep.Callbacks>.InvalidHandle;
			}
			EmoteStep.Callbacks item = new EmoteStep.Callbacks
			{
				StartedCb = startedCb,
				FinishedCb = finishedCb
			};
			return this.callbacks.Add(item);
		}

		// Token: 0x0600EAB0 RID: 60080 RVA: 0x0013CEBC File Offset: 0x0013B0BC
		public void UnregisterCallbacks(HandleVector<EmoteStep.Callbacks>.Handle callbackHandle)
		{
			this.callbacks.Release(callbackHandle);
		}

		// Token: 0x0600EAB1 RID: 60081 RVA: 0x0013CECB File Offset: 0x0013B0CB
		public void UnregisterAllCallbacks()
		{
			this.callbacks = new HandleVector<EmoteStep.Callbacks>(64);
		}

		// Token: 0x0600EAB2 RID: 60082 RVA: 0x004CAA44 File Offset: 0x004C8C44
		public void OnStepStarted(HandleVector<EmoteStep.Callbacks>.Handle callbackHandle, GameObject parameter)
		{
			if (callbackHandle == HandleVector<EmoteStep.Callbacks>.Handle.InvalidHandle)
			{
				return;
			}
			EmoteStep.Callbacks item = this.callbacks.GetItem(callbackHandle);
			if (item.StartedCb != null)
			{
				item.StartedCb(parameter);
			}
		}

		// Token: 0x0600EAB3 RID: 60083 RVA: 0x004CAA80 File Offset: 0x004C8C80
		public void OnStepFinished(HandleVector<EmoteStep.Callbacks>.Handle callbackHandle, GameObject parameter)
		{
			if (callbackHandle == HandleVector<EmoteStep.Callbacks>.Handle.InvalidHandle)
			{
				return;
			}
			EmoteStep.Callbacks item = this.callbacks.GetItem(callbackHandle);
			if (item.FinishedCb != null)
			{
				item.FinishedCb(parameter);
			}
		}

		// Token: 0x0400E5D5 RID: 58837
		public HashedString anim = HashedString.Invalid;

		// Token: 0x0400E5D6 RID: 58838
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;

		// Token: 0x0400E5D7 RID: 58839
		public float timeout = -1f;

		// Token: 0x0400E5D8 RID: 58840
		private HandleVector<EmoteStep.Callbacks> callbacks = new HandleVector<EmoteStep.Callbacks>(64);

		// Token: 0x02003B85 RID: 15237
		public struct Callbacks
		{
			// Token: 0x0400E5D9 RID: 58841
			public Action<GameObject> StartedCb;

			// Token: 0x0400E5DA RID: 58842
			public Action<GameObject> FinishedCb;
		}
	}
}
