using System;
using UnityEngine;

// Token: 0x02001E92 RID: 7826
[AddComponentMenu("KMonoBehaviour/scripts/PageView")]
public class PageView : KMonoBehaviour
{
	// Token: 0x17000A8D RID: 2701
	// (get) Token: 0x0600A41A RID: 42010 RVA: 0x0010A758 File Offset: 0x00108958
	public int ChildrenPerPage
	{
		get
		{
			return this.childrenPerPage;
		}
	}

	// Token: 0x0600A41B RID: 42011 RVA: 0x0010A760 File Offset: 0x00108960
	private void Update()
	{
		if (this.oldChildCount != base.transform.childCount)
		{
			this.oldChildCount = base.transform.childCount;
			this.RefreshPage();
		}
	}

	// Token: 0x0600A41C RID: 42012 RVA: 0x003E4EA0 File Offset: 0x003E30A0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.nextButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.currentPage = (this.currentPage + 1) % this.pageCount;
			if (this.OnChangePage != null)
			{
				this.OnChangePage(this.currentPage);
			}
			this.RefreshPage();
		}));
		MultiToggle multiToggle2 = this.prevButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			this.currentPage--;
			if (this.currentPage < 0)
			{
				this.currentPage += this.pageCount;
			}
			if (this.OnChangePage != null)
			{
				this.OnChangePage(this.currentPage);
			}
			this.RefreshPage();
		}));
	}

	// Token: 0x17000A8E RID: 2702
	// (get) Token: 0x0600A41D RID: 42013 RVA: 0x003E4F04 File Offset: 0x003E3104
	private int pageCount
	{
		get
		{
			int num = base.transform.childCount / this.childrenPerPage;
			if (base.transform.childCount % this.childrenPerPage != 0)
			{
				num++;
			}
			return num;
		}
	}

	// Token: 0x0600A41E RID: 42014 RVA: 0x003E4F40 File Offset: 0x003E3140
	private void RefreshPage()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (i < this.currentPage * this.childrenPerPage)
			{
				base.transform.GetChild(i).gameObject.SetActive(false);
			}
			else if (i >= this.currentPage * this.childrenPerPage + this.childrenPerPage)
			{
				base.transform.GetChild(i).gameObject.SetActive(false);
			}
			else
			{
				base.transform.GetChild(i).gameObject.SetActive(true);
			}
		}
		this.pageLabel.SetText((this.currentPage % this.pageCount + 1).ToString() + "/" + this.pageCount.ToString());
	}

	// Token: 0x04008043 RID: 32835
	[SerializeField]
	private MultiToggle nextButton;

	// Token: 0x04008044 RID: 32836
	[SerializeField]
	private MultiToggle prevButton;

	// Token: 0x04008045 RID: 32837
	[SerializeField]
	private LocText pageLabel;

	// Token: 0x04008046 RID: 32838
	[SerializeField]
	private int childrenPerPage = 8;

	// Token: 0x04008047 RID: 32839
	private int currentPage;

	// Token: 0x04008048 RID: 32840
	private int oldChildCount;

	// Token: 0x04008049 RID: 32841
	public Action<int> OnChangePage;
}
