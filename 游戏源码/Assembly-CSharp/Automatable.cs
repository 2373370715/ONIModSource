using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000999 RID: 2457
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Automatable")]
public class Automatable : KMonoBehaviour
{
	// Token: 0x06002C98 RID: 11416 RVA: 0x000BCDB0 File Offset: 0x000BAFB0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Automatable>(-905833192, Automatable.OnCopySettingsDelegate);
	}

	// Token: 0x06002C99 RID: 11417 RVA: 0x001EC624 File Offset: 0x001EA824
	private void OnCopySettings(object data)
	{
		Automatable component = ((GameObject)data).GetComponent<Automatable>();
		if (component != null)
		{
			this.automationOnly = component.automationOnly;
		}
	}

	// Token: 0x06002C9A RID: 11418 RVA: 0x000BCDC9 File Offset: 0x000BAFC9
	public bool GetAutomationOnly()
	{
		return this.automationOnly;
	}

	// Token: 0x06002C9B RID: 11419 RVA: 0x000BCDD1 File Offset: 0x000BAFD1
	public void SetAutomationOnly(bool only)
	{
		this.automationOnly = only;
	}

	// Token: 0x06002C9C RID: 11420 RVA: 0x000BCDDA File Offset: 0x000BAFDA
	public bool AllowedByAutomation(bool is_transfer_arm)
	{
		return !this.GetAutomationOnly() || is_transfer_arm;
	}

	// Token: 0x04001DFA RID: 7674
	[Serialize]
	private bool automationOnly = true;

	// Token: 0x04001DFB RID: 7675
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001DFC RID: 7676
	private static readonly EventSystem.IntraObjectHandler<Automatable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Automatable>(delegate(Automatable component, object data)
	{
		component.OnCopySettings(data);
	});
}
