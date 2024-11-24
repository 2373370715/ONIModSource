using System;

namespace Klei
{
	// Token: 0x02003AE1 RID: 15073
	public struct CallbackInfo
	{
		// Token: 0x0600E7BB RID: 59323 RVA: 0x0013B028 File Offset: 0x00139228
		public CallbackInfo(HandleVector<Game.CallbackInfo>.Handle h)
		{
			this.handle = h;
		}

		// Token: 0x0600E7BC RID: 59324 RVA: 0x004BEFF0 File Offset: 0x004BD1F0
		public void Release()
		{
			if (this.handle.IsValid())
			{
				Game.CallbackInfo item = Game.Instance.callbackManager.GetItem(this.handle);
				System.Action cb = item.cb;
				if (!item.manuallyRelease)
				{
					Game.Instance.callbackManager.Release(this.handle);
				}
				cb();
			}
		}

		// Token: 0x0400E385 RID: 58245
		private HandleVector<Game.CallbackInfo>.Handle handle;
	}
}
