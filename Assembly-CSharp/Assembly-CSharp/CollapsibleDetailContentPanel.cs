﻿using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/CollapsibleDetailContentPanel")]
public class CollapsibleDetailContentPanel : KMonoBehaviour
{
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

		public void SetTitle(string title)
	{
		this.HeaderLabel.text = title;
	}

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

		public void ForceLocTextsMeshRebuild()
	{
		LocText[] componentsInChildren = base.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].ForceMeshUpdate();
		}
	}

		public void SetActive(bool active)
	{
		if (base.gameObject.activeSelf != active)
		{
			base.gameObject.SetActive(active);
		}
	}

		public ImageToggleState ArrowIcon;

		public LocText HeaderLabel;

		public MultiToggle collapseButton;

		public Transform Content;

		public ScalerMask scalerMask;

		[Space(10f)]
	public DetailLabel labelTemplate;

		public DetailLabelWithButton labelWithActionButtonTemplate;

		private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabel>> labels;

		private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabelWithButton>> buttonLabels;

		private LoggerFSS log;

		private class Label<T>
	{
				public T obj;

				public bool used;
	}
}
