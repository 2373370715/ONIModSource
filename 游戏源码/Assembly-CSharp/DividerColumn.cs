using System;
using UnityEngine;

// Token: 0x02001DD3 RID: 7635
public class DividerColumn : TableColumn
{
	// Token: 0x06009F96 RID: 40854 RVA: 0x003D1C94 File Offset: 0x003CFE94
	public DividerColumn(Func<bool> revealed = null, string scrollerID = "") : base(delegate(IAssignableIdentity minion, GameObject widget_go)
	{
		if (revealed != null)
		{
			if (revealed())
			{
				if (!widget_go.activeSelf)
				{
					widget_go.SetActive(true);
					return;
				}
			}
			else if (widget_go.activeSelf)
			{
				widget_go.SetActive(false);
				return;
			}
		}
		else
		{
			widget_go.SetActive(true);
		}
	}, null, null, null, revealed, false, scrollerID)
	{
	}

	// Token: 0x06009F97 RID: 40855 RVA: 0x00107CCB File Offset: 0x00105ECB
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
	}

	// Token: 0x06009F98 RID: 40856 RVA: 0x00107CCB File Offset: 0x00105ECB
	public override GameObject GetMinionWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
	}

	// Token: 0x06009F99 RID: 40857 RVA: 0x00107CCB File Offset: 0x00105ECB
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
	}
}
