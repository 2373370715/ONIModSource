﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KToggleMenu : KScreen
{
			public event KToggleMenu.OnSelect onSelect;

	public void Setup(IList<KToggleMenu.ToggleInfo> toggleInfo)
	{
		this.toggleInfo = toggleInfo;
		this.RefreshButtons();
	}

	protected void Setup()
	{
		this.RefreshButtons();
	}

	private void RefreshButtons()
	{
		foreach (KToggle ktoggle in this.toggles)
		{
			if (ktoggle != null)
			{
				UnityEngine.Object.Destroy(ktoggle.gameObject);
			}
		}
		this.toggles.Clear();
		if (this.toggleInfo == null)
		{
			return;
		}
		Transform parent = (this.toggleParent != null) ? this.toggleParent : base.transform;
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			int idx = i;
			KToggleMenu.ToggleInfo toggleInfo = this.toggleInfo[i];
			if (toggleInfo == null)
			{
				this.toggles.Add(null);
			}
			else
			{
				KToggle ktoggle2 = UnityEngine.Object.Instantiate<KToggle>(this.prefab, Vector3.zero, Quaternion.identity);
				ktoggle2.gameObject.name = "Toggle:" + toggleInfo.text;
				ktoggle2.transform.SetParent(parent, false);
				ktoggle2.group = this.group;
				ktoggle2.onClick += delegate()
				{
					this.OnClick(idx);
				};
				ktoggle2.GetComponentsInChildren<Text>(true)[0].text = toggleInfo.text;
				toggleInfo.toggle = ktoggle2;
				this.toggles.Add(ktoggle2);
			}
		}
	}

	public int GetSelected()
	{
		return KToggleMenu.selected;
	}

	private void OnClick(int i)
	{
		UISounds.PlaySound(UISounds.Sound.ClickObject);
		if (this.onSelect == null)
		{
			return;
		}
		this.onSelect(this.toggleInfo[i]);
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.toggles == null)
		{
			return;
		}
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			global::Action hotKey = this.toggleInfo[i].hotKey;
			if (hotKey != global::Action.NumActions && e.TryConsume(hotKey))
			{
				this.toggles[i].Click();
				return;
			}
		}
	}

	[SerializeField]
	private Transform toggleParent;

	[SerializeField]
	private KToggle prefab;

	[SerializeField]
	private ToggleGroup group;

	protected IList<KToggleMenu.ToggleInfo> toggleInfo;

	protected List<KToggle> toggles = new List<KToggle>();

	private static int selected = -1;

		public delegate void OnSelect(KToggleMenu.ToggleInfo toggleInfo);

	public class ToggleInfo
	{
		public ToggleInfo(string text, object user_data = null, global::Action hotKey = global::Action.NumActions)
		{
			this.text = text;
			this.userData = user_data;
			this.hotKey = hotKey;
		}

		public string text;

		public object userData;

		public KToggle toggle;

		public global::Action hotKey;
	}
}
