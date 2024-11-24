using System;

// Token: 0x02001C8B RID: 7307
public class CustomGameSettingWidget : KMonoBehaviour
{
	// Token: 0x14000026 RID: 38
	// (add) Token: 0x06009856 RID: 38998 RVA: 0x003AF924 File Offset: 0x003ADB24
	// (remove) Token: 0x06009857 RID: 38999 RVA: 0x003AF95C File Offset: 0x003ADB5C
	public event Action<CustomGameSettingWidget> onSettingChanged;

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x06009858 RID: 39000 RVA: 0x003AF994 File Offset: 0x003ADB94
	// (remove) Token: 0x06009859 RID: 39001 RVA: 0x003AF9CC File Offset: 0x003ADBCC
	public event System.Action onRefresh;

	// Token: 0x14000028 RID: 40
	// (add) Token: 0x0600985A RID: 39002 RVA: 0x003AFA04 File Offset: 0x003ADC04
	// (remove) Token: 0x0600985B RID: 39003 RVA: 0x003AFA3C File Offset: 0x003ADC3C
	public event System.Action onDestroy;

	// Token: 0x0600985C RID: 39004 RVA: 0x0010319B File Offset: 0x0010139B
	public virtual void Refresh()
	{
		if (this.onRefresh != null)
		{
			this.onRefresh();
		}
	}

	// Token: 0x0600985D RID: 39005 RVA: 0x001031B0 File Offset: 0x001013B0
	public void Notify()
	{
		if (this.onSettingChanged != null)
		{
			this.onSettingChanged(this);
		}
	}

	// Token: 0x0600985E RID: 39006 RVA: 0x001031C6 File Offset: 0x001013C6
	protected override void OnForcedCleanUp()
	{
		base.OnForcedCleanUp();
		if (this.onDestroy != null)
		{
			this.onDestroy();
		}
	}
}
