using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001D1B RID: 7451
public class KButtonMenu : KScreen
{
	// Token: 0x06009B91 RID: 39825 RVA: 0x00105409 File Offset: 0x00103609
	protected override void OnActivate()
	{
		base.ConsumeMouseScroll = this.ShouldConsumeMouseScroll;
		this.RefreshButtons();
	}

	// Token: 0x06009B92 RID: 39826 RVA: 0x0010541D File Offset: 0x0010361D
	public void SetButtons(IList<KButtonMenu.ButtonInfo> buttons)
	{
		this.buttons = buttons;
		if (this.activateOnSpawn)
		{
			this.RefreshButtons();
		}
	}

	// Token: 0x06009B93 RID: 39827 RVA: 0x003C0384 File Offset: 0x003BE584
	public virtual void RefreshButtons()
	{
		if (this.buttonObjects != null)
		{
			for (int i = 0; i < this.buttonObjects.Length; i++)
			{
				UnityEngine.Object.Destroy(this.buttonObjects[i]);
			}
			this.buttonObjects = null;
		}
		if (this.buttons == null)
		{
			return;
		}
		this.buttonObjects = new GameObject[this.buttons.Count];
		for (int j = 0; j < this.buttons.Count; j++)
		{
			KButtonMenu.ButtonInfo binfo = this.buttons[j];
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab, Vector3.zero, Quaternion.identity);
			this.buttonObjects[j] = gameObject;
			Transform parent = (this.buttonParent != null) ? this.buttonParent : base.transform;
			gameObject.transform.SetParent(parent, false);
			gameObject.SetActive(true);
			gameObject.name = binfo.text + "Button";
			LocText[] componentsInChildren = gameObject.GetComponentsInChildren<LocText>(true);
			if (componentsInChildren != null)
			{
				foreach (LocText locText in componentsInChildren)
				{
					locText.text = ((locText.name == "Hotkey") ? GameUtil.GetActionString(binfo.shortcutKey) : binfo.text);
					locText.color = (binfo.isEnabled ? new Color(1f, 1f, 1f) : new Color(0.5f, 0.5f, 0.5f));
				}
			}
			ToolTip componentInChildren = gameObject.GetComponentInChildren<ToolTip>();
			if (binfo.toolTip != null && binfo.toolTip != "" && componentInChildren != null)
			{
				componentInChildren.toolTip = binfo.toolTip;
			}
			KButtonMenu screen = this;
			KButton button = gameObject.GetComponent<KButton>();
			button.isInteractable = binfo.isEnabled;
			if (binfo.popupOptions == null && binfo.onPopulatePopup == null)
			{
				UnityAction onClick = binfo.onClick;
				System.Action value = delegate()
				{
					onClick();
					if (!this.keepMenuOpen && screen != null)
					{
						screen.Deactivate();
					}
				};
				button.onClick += value;
			}
			else
			{
				button.onClick += delegate()
				{
					this.SetupPopupMenu(binfo, button);
				};
			}
			binfo.uibutton = button;
			KButtonMenu.ButtonInfo.HoverCallback onHover = binfo.onHover;
		}
		this.Update();
	}

	// Token: 0x06009B94 RID: 39828 RVA: 0x003C0638 File Offset: 0x003BE838
	protected Button.ButtonClickedEvent SetupPopupMenu(KButtonMenu.ButtonInfo binfo, KButton button)
	{
		Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
		UnityAction unityAction = delegate()
		{
			List<KButtonMenu.ButtonInfo> list = new List<KButtonMenu.ButtonInfo>();
			if (binfo.onPopulatePopup != null)
			{
				binfo.popupOptions = binfo.onPopulatePopup();
			}
			string[] popupOptions = binfo.popupOptions;
			for (int i = 0; i < popupOptions.Length; i++)
			{
				string delegate_str2 = popupOptions[i];
				string delegate_str = delegate_str2;
				list.Add(new KButtonMenu.ButtonInfo(delegate_str, delegate()
				{
					binfo.onPopupClick(delegate_str);
					if (!this.keepMenuOpen)
					{
						this.Deactivate();
					}
				}, global::Action.NumActions, null, null, null, true, null, null, null));
			}
			KButtonMenu component = Util.KInstantiate(ScreenPrefabs.Instance.ButtonGrid.gameObject, null, null).GetComponent<KButtonMenu>();
			component.SetButtons(list.ToArray());
			RootMenu.Instance.AddSubMenu(component);
			Game.Instance.LocalPlayer.ScreenManager.ActivateScreen(component.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
			Vector3 b = default(Vector3);
			if (Util.IsOnLeftSideOfScreen(button.transform.GetPosition()))
			{
				b.x = button.GetComponent<RectTransform>().rect.width * 0.25f;
			}
			else
			{
				b.x = -button.GetComponent<RectTransform>().rect.width * 0.25f;
			}
			component.transform.SetPosition(button.transform.GetPosition() + b);
		};
		binfo.onClick = unityAction;
		buttonClickedEvent.AddListener(unityAction);
		return buttonClickedEvent;
	}

	// Token: 0x06009B95 RID: 39829 RVA: 0x003C0688 File Offset: 0x003BE888
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.buttons == null)
		{
			return;
		}
		for (int i = 0; i < this.buttons.Count; i++)
		{
			KButtonMenu.ButtonInfo buttonInfo = this.buttons[i];
			if (e.TryConsume(buttonInfo.shortcutKey))
			{
				this.buttonObjects[i].GetComponent<KButton>().PlayPointerDownSound();
				this.buttonObjects[i].GetComponent<KButton>().SignalClick(KKeyCode.Mouse0);
				break;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009B96 RID: 39830 RVA: 0x00105434 File Offset: 0x00103634
	protected override void OnPrefabInit()
	{
		base.Subscribe<KButtonMenu>(315865555, KButtonMenu.OnSetActivatorDelegate);
	}

	// Token: 0x06009B97 RID: 39831 RVA: 0x00105447 File Offset: 0x00103647
	private void OnSetActivator(object data)
	{
		this.go = (GameObject)data;
		this.Update();
	}

	// Token: 0x06009B98 RID: 39832 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected override void OnDeactivate()
	{
	}

	// Token: 0x06009B99 RID: 39833 RVA: 0x003C0704 File Offset: 0x003BE904
	private void Update()
	{
		if (!this.followGameObject || this.go == null || base.canvas == null)
		{
			return;
		}
		Vector3 vector = Camera.main.WorldToViewportPoint(this.go.transform.GetPosition());
		RectTransform component = base.GetComponent<RectTransform>();
		RectTransform component2 = base.canvas.GetComponent<RectTransform>();
		if (component != null)
		{
			component.anchoredPosition = new Vector2(vector.x * component2.sizeDelta.x - component2.sizeDelta.x * 0.5f, vector.y * component2.sizeDelta.y - component2.sizeDelta.y * 0.5f);
		}
	}

	// Token: 0x040079D8 RID: 31192
	[SerializeField]
	protected bool followGameObject;

	// Token: 0x040079D9 RID: 31193
	[SerializeField]
	protected bool keepMenuOpen;

	// Token: 0x040079DA RID: 31194
	[SerializeField]
	protected Transform buttonParent;

	// Token: 0x040079DB RID: 31195
	public GameObject buttonPrefab;

	// Token: 0x040079DC RID: 31196
	public bool ShouldConsumeMouseScroll;

	// Token: 0x040079DD RID: 31197
	[NonSerialized]
	public GameObject[] buttonObjects;

	// Token: 0x040079DE RID: 31198
	protected GameObject go;

	// Token: 0x040079DF RID: 31199
	protected IList<KButtonMenu.ButtonInfo> buttons;

	// Token: 0x040079E0 RID: 31200
	private static readonly EventSystem.IntraObjectHandler<KButtonMenu> OnSetActivatorDelegate = new EventSystem.IntraObjectHandler<KButtonMenu>(delegate(KButtonMenu component, object data)
	{
		component.OnSetActivator(data);
	});

	// Token: 0x02001D1C RID: 7452
	public class ButtonInfo
	{
		// Token: 0x06009B9C RID: 39836 RVA: 0x003C07C0 File Offset: 0x003BE9C0
		public ButtonInfo(string text = null, UnityAction on_click = null, global::Action shortcut_key = global::Action.NumActions, KButtonMenu.ButtonInfo.HoverCallback on_hover = null, string tool_tip = null, GameObject visualizer = null, bool is_enabled = true, string[] popup_options = null, Action<string> on_popup_click = null, Func<string[]> on_populate_popup = null)
		{
			this.text = text;
			this.shortcutKey = shortcut_key;
			this.onClick = on_click;
			this.onHover = on_hover;
			this.visualizer = visualizer;
			this.toolTip = tool_tip;
			this.isEnabled = is_enabled;
			this.uibutton = null;
			this.popupOptions = popup_options;
			this.onPopupClick = on_popup_click;
			this.onPopulatePopup = on_populate_popup;
		}

		// Token: 0x06009B9D RID: 39837 RVA: 0x003C0830 File Offset: 0x003BEA30
		public ButtonInfo(string text, global::Action shortcutKey, UnityAction onClick, KButtonMenu.ButtonInfo.HoverCallback onHover = null, object userData = null)
		{
			this.text = text;
			this.shortcutKey = shortcutKey;
			this.onClick = onClick;
			this.onHover = onHover;
			this.userData = userData;
			this.visualizer = null;
			this.uibutton = null;
		}

		// Token: 0x06009B9E RID: 39838 RVA: 0x003C0880 File Offset: 0x003BEA80
		public ButtonInfo(string text, GameObject visualizer, global::Action shortcutKey, UnityAction onClick, KButtonMenu.ButtonInfo.HoverCallback onHover = null, object userData = null)
		{
			this.text = text;
			this.shortcutKey = shortcutKey;
			this.onClick = onClick;
			this.onHover = onHover;
			this.visualizer = visualizer;
			this.userData = userData;
			this.uibutton = null;
		}

		// Token: 0x040079E1 RID: 31201
		public string text;

		// Token: 0x040079E2 RID: 31202
		public global::Action shortcutKey;

		// Token: 0x040079E3 RID: 31203
		public GameObject visualizer;

		// Token: 0x040079E4 RID: 31204
		public UnityAction onClick;

		// Token: 0x040079E5 RID: 31205
		public KButtonMenu.ButtonInfo.HoverCallback onHover;

		// Token: 0x040079E6 RID: 31206
		public FMODAsset clickSound;

		// Token: 0x040079E7 RID: 31207
		public KButton uibutton;

		// Token: 0x040079E8 RID: 31208
		public string toolTip;

		// Token: 0x040079E9 RID: 31209
		public bool isEnabled = true;

		// Token: 0x040079EA RID: 31210
		public string[] popupOptions;

		// Token: 0x040079EB RID: 31211
		public Action<string> onPopupClick;

		// Token: 0x040079EC RID: 31212
		public Func<string[]> onPopulatePopup;

		// Token: 0x040079ED RID: 31213
		public object userData;

		// Token: 0x02001D1D RID: 7453
		// (Invoke) Token: 0x06009BA0 RID: 39840
		public delegate void HoverCallback(GameObject hoverTarget);

		// Token: 0x02001D1E RID: 7454
		// (Invoke) Token: 0x06009BA4 RID: 39844
		public delegate void Callback();
	}
}
