using System;

namespace EventSystem2Syntax
{
	// Token: 0x020020DD RID: 8413
	internal class OldExample : KMonoBehaviour2
	{
		// Token: 0x0600B2FC RID: 45820 RVA: 0x0043A1E4 File Offset: 0x004383E4
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			base.Subscribe(0, new Action<object>(this.OnObjectDestroyed));
			bool flag = false;
			base.Trigger(0, flag);
		}

		// Token: 0x0600B2FD RID: 45821 RVA: 0x0011424F File Offset: 0x0011244F
		private void OnObjectDestroyed(object data)
		{
			Debug.Log((bool)data);
		}
	}
}
