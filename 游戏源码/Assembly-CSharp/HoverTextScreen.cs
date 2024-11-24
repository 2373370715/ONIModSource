using System;
using UnityEngine;

// Token: 0x02001AD2 RID: 6866
public class HoverTextScreen : KScreen
{
	// Token: 0x06008FC8 RID: 36808 RVA: 0x000FDFBE File Offset: 0x000FC1BE
	public static void DestroyInstance()
	{
		HoverTextScreen.Instance = null;
	}

	// Token: 0x06008FC9 RID: 36809 RVA: 0x000FDFC6 File Offset: 0x000FC1C6
	protected override void OnActivate()
	{
		base.OnActivate();
		HoverTextScreen.Instance = this;
		this.drawer = new HoverTextDrawer(this.skin.skin, base.GetComponent<RectTransform>());
	}

	// Token: 0x06008FCA RID: 36810 RVA: 0x00378424 File Offset: 0x00376624
	public HoverTextDrawer BeginDrawing()
	{
		Vector2 zero = Vector2.zero;
		Vector2 screenPoint = KInputManager.GetMousePos();
		RectTransform rectTransform = base.transform.parent as RectTransform;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, base.transform.parent.GetComponent<Canvas>().worldCamera, out zero);
		zero.x += rectTransform.sizeDelta.x / 2f;
		zero.y -= rectTransform.sizeDelta.y / 2f;
		this.drawer.BeginDrawing(zero);
		return this.drawer;
	}

	// Token: 0x06008FCB RID: 36811 RVA: 0x003784BC File Offset: 0x003766BC
	private void Update()
	{
		bool enabled = PlayerController.Instance.ActiveTool.ShowHoverUI();
		this.drawer.SetEnabled(enabled);
	}

	// Token: 0x06008FCC RID: 36812 RVA: 0x003784E8 File Offset: 0x003766E8
	public Sprite GetSprite(string byName)
	{
		foreach (Sprite sprite in this.HoverIcons)
		{
			if (sprite != null && sprite.name == byName)
			{
				return sprite;
			}
		}
		global::Debug.LogWarning("No icon named " + byName + " was found on HoverTextScreen.prefab");
		return null;
	}

	// Token: 0x06008FCD RID: 36813 RVA: 0x000FDFF0 File Offset: 0x000FC1F0
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.drawer.Cleanup();
	}

	// Token: 0x04006C99 RID: 27801
	[SerializeField]
	private HoverTextSkin skin;

	// Token: 0x04006C9A RID: 27802
	public Sprite[] HoverIcons;

	// Token: 0x04006C9B RID: 27803
	public HoverTextDrawer drawer;

	// Token: 0x04006C9C RID: 27804
	public static HoverTextScreen Instance;
}
