using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D2B RID: 7467
public class KIconToggleMenu : KScreen
{
	// Token: 0x1400002D RID: 45
	// (add) Token: 0x06009BD2 RID: 39890 RVA: 0x003C1484 File Offset: 0x003BF684
	// (remove) Token: 0x06009BD3 RID: 39891 RVA: 0x003C14BC File Offset: 0x003BF6BC
	public event KIconToggleMenu.OnSelect onSelect;

	// Token: 0x06009BD4 RID: 39892 RVA: 0x001055E6 File Offset: 0x001037E6
	public void Setup(IList<KIconToggleMenu.ToggleInfo> toggleInfo)
	{
		this.toggleInfo = toggleInfo;
		this.RefreshButtons();
	}

	// Token: 0x06009BD5 RID: 39893 RVA: 0x000FECBA File Offset: 0x000FCEBA
	protected void Setup()
	{
		this.RefreshButtons();
	}

	// Token: 0x06009BD6 RID: 39894 RVA: 0x003C14F4 File Offset: 0x003BF6F4
	protected virtual void RefreshButtons()
	{
		foreach (KToggle ktoggle in this.toggles)
		{
			if (ktoggle != null)
			{
				if (!this.dontDestroyToggles.Contains(ktoggle))
				{
					UnityEngine.Object.Destroy(ktoggle.gameObject);
				}
				else
				{
					ktoggle.ClearOnClick();
				}
			}
		}
		this.toggles.Clear();
		this.dontDestroyToggles.Clear();
		if (this.toggleInfo == null)
		{
			return;
		}
		Transform transform = (this.toggleParent != null) ? this.toggleParent : base.transform;
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			int idx = i;
			KIconToggleMenu.ToggleInfo toggleInfo = this.toggleInfo[i];
			KToggle ktoggle2;
			if (toggleInfo.instanceOverride != null)
			{
				ktoggle2 = toggleInfo.instanceOverride;
				this.dontDestroyToggles.Add(ktoggle2);
			}
			else if (toggleInfo.prefabOverride)
			{
				ktoggle2 = Util.KInstantiateUI<KToggle>(toggleInfo.prefabOverride.gameObject, transform.gameObject, true);
			}
			else
			{
				ktoggle2 = Util.KInstantiateUI<KToggle>(this.prefab.gameObject, transform.gameObject, true);
			}
			ktoggle2.Deselect();
			ktoggle2.gameObject.name = "Toggle:" + toggleInfo.text;
			ktoggle2.group = this.group;
			ktoggle2.onClick += delegate()
			{
				this.OnClick(idx);
			};
			LocText componentInChildren = ktoggle2.transform.GetComponentInChildren<LocText>();
			if (componentInChildren != null)
			{
				componentInChildren.SetText(toggleInfo.text);
			}
			if (toggleInfo.getSpriteCB != null)
			{
				ktoggle2.fgImage.sprite = toggleInfo.getSpriteCB();
			}
			else if (toggleInfo.icon != null)
			{
				ktoggle2.fgImage.sprite = Assets.GetSprite(toggleInfo.icon);
			}
			toggleInfo.SetToggle(ktoggle2);
			this.toggles.Add(ktoggle2);
		}
	}

	// Token: 0x06009BD7 RID: 39895 RVA: 0x003C171C File Offset: 0x003BF91C
	public Sprite GetIcon(string name)
	{
		foreach (Sprite sprite in this.icons)
		{
			if (sprite.name == name)
			{
				return sprite;
			}
		}
		return null;
	}

	// Token: 0x06009BD8 RID: 39896 RVA: 0x003C1754 File Offset: 0x003BF954
	public virtual void ClearSelection()
	{
		if (this.toggles == null)
		{
			return;
		}
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.Deselect();
			ktoggle.ClearAnimState();
		}
		this.selected = -1;
	}

	// Token: 0x06009BD9 RID: 39897 RVA: 0x003C17BC File Offset: 0x003BF9BC
	private void OnClick(int i)
	{
		if (this.onSelect == null)
		{
			return;
		}
		this.selected = i;
		this.onSelect(this.toggleInfo[i]);
		if (!this.toggles[i].isOn)
		{
			this.selected = -1;
		}
		for (int j = 0; j < this.toggles.Count; j++)
		{
			if (j != this.selected)
			{
				this.toggles[j].isOn = false;
			}
		}
	}

	// Token: 0x06009BDA RID: 39898 RVA: 0x003C183C File Offset: 0x003BFA3C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.toggles == null)
		{
			return;
		}
		if (this.toggleInfo == null)
		{
			return;
		}
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			if (this.toggles[i].isActiveAndEnabled)
			{
				global::Action hotKey = this.toggleInfo[i].hotKey;
				if (hotKey != global::Action.NumActions && e.TryConsume(hotKey))
				{
					if (this.selected != i || this.repeatKeyDownToggles)
					{
						this.toggles[i].Click();
						if (this.selected == i)
						{
							this.toggles[i].Deselect();
						}
						this.selected = i;
						return;
					}
					break;
				}
			}
		}
	}

	// Token: 0x06009BDB RID: 39899 RVA: 0x001055F5 File Offset: 0x001037F5
	public virtual void Close()
	{
		this.ClearSelection();
		this.Show(false);
	}

	// Token: 0x04007A1F RID: 31263
	[SerializeField]
	private Transform toggleParent;

	// Token: 0x04007A20 RID: 31264
	[SerializeField]
	private KToggle prefab;

	// Token: 0x04007A21 RID: 31265
	[SerializeField]
	private ToggleGroup group;

	// Token: 0x04007A22 RID: 31266
	[SerializeField]
	private Sprite[] icons;

	// Token: 0x04007A23 RID: 31267
	[SerializeField]
	public TextStyleSetting ToggleToolTipTextStyleSetting;

	// Token: 0x04007A24 RID: 31268
	[SerializeField]
	public TextStyleSetting ToggleToolTipHeaderTextStyleSetting;

	// Token: 0x04007A25 RID: 31269
	[SerializeField]
	protected bool repeatKeyDownToggles = true;

	// Token: 0x04007A26 RID: 31270
	protected KToggle currentlySelectedToggle;

	// Token: 0x04007A28 RID: 31272
	protected IList<KIconToggleMenu.ToggleInfo> toggleInfo;

	// Token: 0x04007A29 RID: 31273
	protected List<KToggle> toggles = new List<KToggle>();

	// Token: 0x04007A2A RID: 31274
	private List<KToggle> dontDestroyToggles = new List<KToggle>();

	// Token: 0x04007A2B RID: 31275
	protected int selected = -1;

	// Token: 0x02001D2C RID: 7468
	// (Invoke) Token: 0x06009BDE RID: 39902
	public delegate void OnSelect(KIconToggleMenu.ToggleInfo toggleInfo);

	// Token: 0x02001D2D RID: 7469
	public class ToggleInfo
	{
		// Token: 0x06009BE1 RID: 39905 RVA: 0x003C18F0 File Offset: 0x003BFAF0
		public ToggleInfo(string text, string icon, object user_data = null, global::Action hotkey = global::Action.NumActions, string tooltip = "", string tooltip_header = "")
		{
			this.text = text;
			this.userData = user_data;
			this.icon = icon;
			this.hotKey = hotkey;
			this.tooltip = tooltip;
			this.tooltipHeader = tooltip_header;
			this.getTooltipText = new ToolTip.ComplexTooltipDelegate(this.DefaultGetTooltipText);
		}

		// Token: 0x06009BE2 RID: 39906 RVA: 0x00105630 File Offset: 0x00103830
		public ToggleInfo(string text, object user_data, global::Action hotkey, Func<Sprite> get_sprite_cb)
		{
			this.text = text;
			this.userData = user_data;
			this.hotKey = hotkey;
			this.getSpriteCB = get_sprite_cb;
		}

		// Token: 0x06009BE3 RID: 39907 RVA: 0x00105655 File Offset: 0x00103855
		public virtual void SetToggle(KToggle toggle)
		{
			this.toggle = toggle;
			toggle.GetComponent<ToolTip>().OnComplexToolTip = this.getTooltipText;
		}

		// Token: 0x06009BE4 RID: 39908 RVA: 0x003C1944 File Offset: 0x003BFB44
		protected virtual List<global::Tuple<string, TextStyleSetting>> DefaultGetTooltipText()
		{
			List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
			if (this.tooltipHeader != null)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(this.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			}
			list.Add(new global::Tuple<string, TextStyleSetting>(this.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			return list;
		}

		// Token: 0x04007A2C RID: 31276
		public string text;

		// Token: 0x04007A2D RID: 31277
		public object userData;

		// Token: 0x04007A2E RID: 31278
		public string icon;

		// Token: 0x04007A2F RID: 31279
		public string tooltip;

		// Token: 0x04007A30 RID: 31280
		public string tooltipHeader;

		// Token: 0x04007A31 RID: 31281
		public KToggle toggle;

		// Token: 0x04007A32 RID: 31282
		public global::Action hotKey;

		// Token: 0x04007A33 RID: 31283
		public ToolTip.ComplexTooltipDelegate getTooltipText;

		// Token: 0x04007A34 RID: 31284
		public Func<Sprite> getSpriteCB;

		// Token: 0x04007A35 RID: 31285
		public KToggle prefabOverride;

		// Token: 0x04007A36 RID: 31286
		public KToggle instanceOverride;
	}
}
