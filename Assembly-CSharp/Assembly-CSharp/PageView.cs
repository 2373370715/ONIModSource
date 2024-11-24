﻿using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/PageView")]
public class PageView : KMonoBehaviour
{
		public int ChildrenPerPage
	{
		get
		{
			return this.childrenPerPage;
		}
	}

	private void Update()
	{
		if (this.oldChildCount != base.transform.childCount)
		{
			this.oldChildCount = base.transform.childCount;
			this.RefreshPage();
		}
	}

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

	[SerializeField]
	private MultiToggle nextButton;

	[SerializeField]
	private MultiToggle prevButton;

	[SerializeField]
	private LocText pageLabel;

	[SerializeField]
	private int childrenPerPage = 8;

	private int currentPage;

	private int oldChildCount;

	public Action<int> OnChangePage;
}
