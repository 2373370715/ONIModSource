using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E3B RID: 7739
public class MotdBox : KMonoBehaviour
{
	// Token: 0x0600A22A RID: 41514 RVA: 0x003DC370 File Offset: 0x003DA570
	public void Config(MotdBox.PageData[] data)
	{
		this.pageDatas = data;
		if (this.pageButtons != null)
		{
			for (int i = this.pageButtons.Length - 1; i >= 0; i--)
			{
				UnityEngine.Object.Destroy(this.pageButtons[i]);
			}
			this.pageButtons = null;
		}
		this.pageButtons = new GameObject[data.Length];
		for (int j = 0; j < this.pageButtons.Length; j++)
		{
			int idx = j;
			GameObject gameObject = Util.KInstantiateUI(this.pageCarouselButtonPrefab, this.pageCarouselContainer, false);
			gameObject.SetActive(true);
			this.pageButtons[j] = gameObject;
			MultiToggle component = gameObject.GetComponent<MultiToggle>();
			component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
			{
				this.SwitchPage(idx);
			}));
		}
		this.SwitchPage(0);
	}

	// Token: 0x0600A22B RID: 41515 RVA: 0x003DC43C File Offset: 0x003DA63C
	private void SwitchPage(int newPage)
	{
		this.selectedPage = newPage;
		for (int i = 0; i < this.pageButtons.Length; i++)
		{
			this.pageButtons[i].GetComponent<MultiToggle>().ChangeState((i == this.selectedPage) ? 1 : 0);
		}
		this.image.texture = this.pageDatas[newPage].Texture;
		this.headerLabel.SetText(this.pageDatas[newPage].HeaderText);
		this.urlOpener.SetURL(this.pageDatas[newPage].URL);
		if (string.IsNullOrEmpty(this.pageDatas[newPage].ImageText))
		{
			this.imageLabel.gameObject.SetActive(false);
			this.imageLabel.SetText("");
			return;
		}
		this.imageLabel.gameObject.SetActive(true);
		this.imageLabel.SetText(this.pageDatas[newPage].ImageText);
	}

	// Token: 0x04007E7F RID: 32383
	[SerializeField]
	private GameObject pageCarouselContainer;

	// Token: 0x04007E80 RID: 32384
	[SerializeField]
	private GameObject pageCarouselButtonPrefab;

	// Token: 0x04007E81 RID: 32385
	[SerializeField]
	private RawImage image;

	// Token: 0x04007E82 RID: 32386
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x04007E83 RID: 32387
	[SerializeField]
	private LocText imageLabel;

	// Token: 0x04007E84 RID: 32388
	[SerializeField]
	private URLOpenFunction urlOpener;

	// Token: 0x04007E85 RID: 32389
	private int selectedPage;

	// Token: 0x04007E86 RID: 32390
	private GameObject[] pageButtons;

	// Token: 0x04007E87 RID: 32391
	private MotdBox.PageData[] pageDatas;

	// Token: 0x02001E3C RID: 7740
	public class PageData
	{
		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x0600A22D RID: 41517 RVA: 0x001093E8 File Offset: 0x001075E8
		// (set) Token: 0x0600A22E RID: 41518 RVA: 0x001093F0 File Offset: 0x001075F0
		public Texture2D Texture { get; set; }

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x0600A22F RID: 41519 RVA: 0x001093F9 File Offset: 0x001075F9
		// (set) Token: 0x0600A230 RID: 41520 RVA: 0x00109401 File Offset: 0x00107601
		public string HeaderText { get; set; }

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x0600A231 RID: 41521 RVA: 0x0010940A File Offset: 0x0010760A
		// (set) Token: 0x0600A232 RID: 41522 RVA: 0x00109412 File Offset: 0x00107612
		public string ImageText { get; set; }

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x0600A233 RID: 41523 RVA: 0x0010941B File Offset: 0x0010761B
		// (set) Token: 0x0600A234 RID: 41524 RVA: 0x00109423 File Offset: 0x00107623
		public string URL { get; set; }
	}
}
