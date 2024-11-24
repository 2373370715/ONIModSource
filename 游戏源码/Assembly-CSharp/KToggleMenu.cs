using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D33 RID: 7475
public class KToggleMenu : KScreen
{
	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06009C0A RID: 39946 RVA: 0x003C1E04 File Offset: 0x003C0004
	// (remove) Token: 0x06009C0B RID: 39947 RVA: 0x003C1E3C File Offset: 0x003C003C
	public event KToggleMenu.OnSelect onSelect;

	// Token: 0x06009C0C RID: 39948 RVA: 0x001057CD File Offset: 0x001039CD
	public void Setup(IList<KToggleMenu.ToggleInfo> toggleInfo)
	{
		this.toggleInfo = toggleInfo;
		this.RefreshButtons();
	}

	// Token: 0x06009C0D RID: 39949 RVA: 0x001057DC File Offset: 0x001039DC
	protected void Setup()
	{
		this.RefreshButtons();
	}

	// Token: 0x06009C0E RID: 39950 RVA: 0x003C1E74 File Offset: 0x003C0074
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

	// Token: 0x06009C0F RID: 39951 RVA: 0x001057E4 File Offset: 0x001039E4
	public int GetSelected()
	{
		return KToggleMenu.selected;
	}

	// Token: 0x06009C10 RID: 39952 RVA: 0x001057EB File Offset: 0x001039EB
	private void OnClick(int i)
	{
		UISounds.PlaySound(UISounds.Sound.ClickObject);
		if (this.onSelect == null)
		{
			return;
		}
		this.onSelect(this.toggleInfo[i]);
	}

	// Token: 0x06009C11 RID: 39953 RVA: 0x003C1FEC File Offset: 0x003C01EC
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

	// Token: 0x04007A47 RID: 31303
	[SerializeField]
	private Transform toggleParent;

	// Token: 0x04007A48 RID: 31304
	[SerializeField]
	private KToggle prefab;

	// Token: 0x04007A49 RID: 31305
	[SerializeField]
	private ToggleGroup group;

	// Token: 0x04007A4B RID: 31307
	protected IList<KToggleMenu.ToggleInfo> toggleInfo;

	// Token: 0x04007A4C RID: 31308
	protected List<KToggle> toggles = new List<KToggle>();

	// Token: 0x04007A4D RID: 31309
	private static int selected = -1;

	// Token: 0x02001D34 RID: 7476
	// (Invoke) Token: 0x06009C15 RID: 39957
	public delegate void OnSelect(KToggleMenu.ToggleInfo toggleInfo);

	// Token: 0x02001D35 RID: 7477
	public class ToggleInfo
	{
		// Token: 0x06009C18 RID: 39960 RVA: 0x0010582E File Offset: 0x00103A2E
		public ToggleInfo(string text, object user_data = null, global::Action hotKey = global::Action.NumActions)
		{
			this.text = text;
			this.userData = user_data;
			this.hotKey = hotKey;
		}

		// Token: 0x04007A4E RID: 31310
		public string text;

		// Token: 0x04007A4F RID: 31311
		public object userData;

		// Token: 0x04007A50 RID: 31312
		public KToggle toggle;

		// Token: 0x04007A51 RID: 31313
		public global::Action hotKey;
	}
}
