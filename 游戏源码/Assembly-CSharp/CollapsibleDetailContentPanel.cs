using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001CA2 RID: 7330
[AddComponentMenu("KMonoBehaviour/scripts/CollapsibleDetailContentPanel")]
public class CollapsibleDetailContentPanel : KMonoBehaviour
{
	// Token: 0x060098FD RID: 39165 RVA: 0x003B2E4C File Offset: 0x003B104C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.collapseButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.ToggleOpen));
		this.ArrowIcon.SetActive();
		this.log = new LoggerFSS("detailpanel", 35);
		this.labels = new Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabel>>();
		this.buttonLabels = new Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabelWithButton>>();
		this.Commit();
	}

	// Token: 0x060098FE RID: 39166 RVA: 0x001038D4 File Offset: 0x00101AD4
	public void SetTitle(string title)
	{
		this.HeaderLabel.text = title;
	}

	// Token: 0x060098FF RID: 39167 RVA: 0x003B2EC0 File Offset: 0x003B10C0
	public void Commit()
	{
		int num = 0;
		foreach (CollapsibleDetailContentPanel.Label<DetailLabel> label in this.labels.Values)
		{
			if (label.used)
			{
				num++;
				if (!label.obj.gameObject.activeSelf)
				{
					label.obj.gameObject.SetActive(true);
				}
			}
			else if (!label.used && label.obj.gameObject.activeSelf)
			{
				label.obj.gameObject.SetActive(false);
			}
			label.used = false;
		}
		foreach (CollapsibleDetailContentPanel.Label<DetailLabelWithButton> label2 in this.buttonLabels.Values)
		{
			if (label2.used)
			{
				num++;
				if (!label2.obj.gameObject.activeSelf)
				{
					label2.obj.gameObject.SetActive(true);
				}
			}
			else if (!label2.used && label2.obj.gameObject.activeSelf)
			{
				label2.obj.gameObject.SetActive(false);
			}
			label2.used = false;
		}
		if (base.gameObject.activeSelf && num == 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (!base.gameObject.activeSelf && num > 0)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06009900 RID: 39168 RVA: 0x003B305C File Offset: 0x003B125C
	public void SetLabel(string id, string text, string tooltip)
	{
		CollapsibleDetailContentPanel.Label<DetailLabel> label;
		if (!this.labels.TryGetValue(id, out label))
		{
			label = new CollapsibleDetailContentPanel.Label<DetailLabel>
			{
				used = true,
				obj = Util.KInstantiateUI(this.labelTemplate.gameObject, this.Content.gameObject, false).GetComponent<DetailLabel>()
			};
			label.obj.gameObject.name = id;
			this.labels[id] = label;
		}
		label.obj.label.AllowLinks = true;
		label.obj.label.text = text;
		label.obj.toolTip.toolTip = tooltip;
		label.used = true;
	}

	// Token: 0x06009901 RID: 39169 RVA: 0x003B3108 File Offset: 0x003B1308
	public void SetLabelWithButton(string id, string text, string tooltip, System.Action buttonCb)
	{
		CollapsibleDetailContentPanel.Label<DetailLabelWithButton> label;
		if (!this.buttonLabels.TryGetValue(id, out label))
		{
			label = new CollapsibleDetailContentPanel.Label<DetailLabelWithButton>
			{
				used = true,
				obj = Util.KInstantiateUI(this.labelWithActionButtonTemplate.gameObject, this.Content.gameObject, false).GetComponent<DetailLabelWithButton>()
			};
			label.obj.gameObject.name = id;
			this.buttonLabels[id] = label;
		}
		label.obj.label.AllowLinks = false;
		label.obj.label.raycastTarget = false;
		label.obj.label.text = text;
		label.obj.toolTip.toolTip = tooltip;
		label.obj.button.ClearOnClick();
		label.obj.button.onClick += buttonCb;
		label.used = true;
	}

	// Token: 0x06009902 RID: 39170 RVA: 0x003B31E4 File Offset: 0x003B13E4
	private void ToggleOpen()
	{
		bool flag = this.scalerMask.gameObject.activeSelf;
		flag = !flag;
		this.scalerMask.gameObject.SetActive(flag);
		if (flag)
		{
			this.ArrowIcon.SetActive();
			this.ForceLocTextsMeshRebuild();
			return;
		}
		this.ArrowIcon.SetInactive();
	}

	// Token: 0x06009903 RID: 39171 RVA: 0x003B3238 File Offset: 0x003B1438
	public void ForceLocTextsMeshRebuild()
	{
		LocText[] componentsInChildren = base.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].ForceMeshUpdate();
		}
	}

	// Token: 0x06009904 RID: 39172 RVA: 0x001038E2 File Offset: 0x00101AE2
	public void SetActive(bool active)
	{
		if (base.gameObject.activeSelf != active)
		{
			base.gameObject.SetActive(active);
		}
	}

	// Token: 0x04007738 RID: 30520
	public ImageToggleState ArrowIcon;

	// Token: 0x04007739 RID: 30521
	public LocText HeaderLabel;

	// Token: 0x0400773A RID: 30522
	public MultiToggle collapseButton;

	// Token: 0x0400773B RID: 30523
	public Transform Content;

	// Token: 0x0400773C RID: 30524
	public ScalerMask scalerMask;

	// Token: 0x0400773D RID: 30525
	[Space(10f)]
	public DetailLabel labelTemplate;

	// Token: 0x0400773E RID: 30526
	public DetailLabelWithButton labelWithActionButtonTemplate;

	// Token: 0x0400773F RID: 30527
	private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabel>> labels;

	// Token: 0x04007740 RID: 30528
	private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabelWithButton>> buttonLabels;

	// Token: 0x04007741 RID: 30529
	private LoggerFSS log;

	// Token: 0x02001CA3 RID: 7331
	private class Label<T>
	{
		// Token: 0x04007742 RID: 30530
		public T obj;

		// Token: 0x04007743 RID: 30531
		public bool used;
	}
}
