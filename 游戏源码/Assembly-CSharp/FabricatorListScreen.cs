using System;
using System.Collections.Generic;

// Token: 0x02001CC8 RID: 7368
public class FabricatorListScreen : KToggleMenu
{
	// Token: 0x060099DC RID: 39388 RVA: 0x003B70A4 File Offset: 0x003B52A4
	private void Refresh()
	{
		List<KToggleMenu.ToggleInfo> list = new List<KToggleMenu.ToggleInfo>();
		foreach (Fabricator fabricator in Components.Fabricators.Items)
		{
			KSelectable component = fabricator.GetComponent<KSelectable>();
			list.Add(new KToggleMenu.ToggleInfo(component.GetName(), fabricator, global::Action.NumActions));
		}
		base.Setup(list);
	}

	// Token: 0x060099DD RID: 39389 RVA: 0x001041E6 File Offset: 0x001023E6
	protected override void OnSpawn()
	{
		base.onSelect += this.OnClickFabricator;
	}

	// Token: 0x060099DE RID: 39390 RVA: 0x001041FA File Offset: 0x001023FA
	protected override void OnActivate()
	{
		base.OnActivate();
		this.Refresh();
	}

	// Token: 0x060099DF RID: 39391 RVA: 0x003B7120 File Offset: 0x003B5320
	private void OnClickFabricator(KToggleMenu.ToggleInfo toggle_info)
	{
		Fabricator fabricator = (Fabricator)toggle_info.userData;
		SelectTool.Instance.Select(fabricator.GetComponent<KSelectable>(), false);
	}
}
