using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001F1B RID: 7963
public class SideDetailsScreen : KScreen
{
	// Token: 0x0600A7E0 RID: 42976 RVA: 0x0010CEE0 File Offset: 0x0010B0E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SideDetailsScreen.Instance = this;
		this.Initialize();
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600A7E1 RID: 42977 RVA: 0x0010CF00 File Offset: 0x0010B100
	protected override void OnForcedCleanUp()
	{
		SideDetailsScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600A7E2 RID: 42978 RVA: 0x003FACF8 File Offset: 0x003F8EF8
	private void Initialize()
	{
		if (this.screens == null)
		{
			return;
		}
		this.rectTransform = base.GetComponent<RectTransform>();
		this.screenMap = new Dictionary<string, SideTargetScreen>();
		List<SideTargetScreen> list = new List<SideTargetScreen>();
		foreach (SideTargetScreen sideTargetScreen in this.screens)
		{
			SideTargetScreen sideTargetScreen2 = Util.KInstantiateUI<SideTargetScreen>(sideTargetScreen.gameObject, this.body.gameObject, false);
			sideTargetScreen2.gameObject.SetActive(false);
			list.Add(sideTargetScreen2);
		}
		list.ForEach(delegate(SideTargetScreen s)
		{
			this.screenMap.Add(s.name, s);
		});
		this.backButton.onClick += delegate()
		{
			this.Show(false);
		};
	}

	// Token: 0x0600A7E3 RID: 42979 RVA: 0x0010CF0E File Offset: 0x0010B10E
	public void SetTitle(string newTitle)
	{
		this.title.text = newTitle;
	}

	// Token: 0x0600A7E4 RID: 42980 RVA: 0x003FADBC File Offset: 0x003F8FBC
	public void SetScreen(string screenName, object content, float x)
	{
		if (!this.screenMap.ContainsKey(screenName))
		{
			global::Debug.LogError("Tried to open a screen that does exist on the manager!");
			return;
		}
		if (content == null)
		{
			global::Debug.LogError("Tried to set " + screenName + " with null content!");
			return;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(true);
		}
		Rect rect = this.rectTransform.rect;
		this.rectTransform.offsetMin = new Vector2(x, this.rectTransform.offsetMin.y);
		this.rectTransform.offsetMax = new Vector2(x + rect.width, this.rectTransform.offsetMax.y);
		if (this.activeScreen != null)
		{
			this.activeScreen.gameObject.SetActive(false);
		}
		this.activeScreen = this.screenMap[screenName];
		this.activeScreen.gameObject.SetActive(true);
		this.SetTitle(this.activeScreen.displayName);
		this.activeScreen.SetTarget(content);
	}

	// Token: 0x04008402 RID: 33794
	[SerializeField]
	private List<SideTargetScreen> screens;

	// Token: 0x04008403 RID: 33795
	[SerializeField]
	private LocText title;

	// Token: 0x04008404 RID: 33796
	[SerializeField]
	private KButton backButton;

	// Token: 0x04008405 RID: 33797
	[SerializeField]
	private RectTransform body;

	// Token: 0x04008406 RID: 33798
	private RectTransform rectTransform;

	// Token: 0x04008407 RID: 33799
	private Dictionary<string, SideTargetScreen> screenMap;

	// Token: 0x04008408 RID: 33800
	private SideTargetScreen activeScreen;

	// Token: 0x04008409 RID: 33801
	public static SideDetailsScreen Instance;
}
