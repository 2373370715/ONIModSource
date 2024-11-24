using System;
using UnityEngine;
using UnityEngine.UI;

public class MotdBox : KMonoBehaviour
{
	public class PageData
	{
		public Texture2D Texture { get; set; }

		public string HeaderText { get; set; }

		public string ImageText { get; set; }

		public string URL { get; set; }
	}

	[SerializeField]
	private GameObject pageCarouselContainer;

	[SerializeField]
	private GameObject pageCarouselButtonPrefab;

	[SerializeField]
	private RawImage image;

	[SerializeField]
	private LocText headerLabel;

	[SerializeField]
	private LocText imageLabel;

	[SerializeField]
	private URLOpenFunction urlOpener;

	private int selectedPage;

	private GameObject[] pageButtons;

	private PageData[] pageDatas;

	public void Config(PageData[] data)
	{
		pageDatas = data;
		if (pageButtons != null)
		{
			for (int num = pageButtons.Length - 1; num >= 0; num--)
			{
				UnityEngine.Object.Destroy(pageButtons[num]);
			}
			pageButtons = null;
		}
		pageButtons = new GameObject[data.Length];
		for (int i = 0; i < pageButtons.Length; i++)
		{
			int idx = i;
			GameObject gameObject = Util.KInstantiateUI(pageCarouselButtonPrefab, pageCarouselContainer);
			gameObject.SetActive(value: true);
			pageButtons[i] = gameObject;
			MultiToggle component = gameObject.GetComponent<MultiToggle>();
			component.onClick = (System.Action)Delegate.Combine(component.onClick, (System.Action)delegate
			{
				SwitchPage(idx);
			});
		}
		SwitchPage(0);
	}

	private void SwitchPage(int newPage)
	{
		selectedPage = newPage;
		for (int i = 0; i < pageButtons.Length; i++)
		{
			pageButtons[i].GetComponent<MultiToggle>().ChangeState((i == selectedPage) ? 1 : 0);
		}
		image.texture = pageDatas[newPage].Texture;
		headerLabel.SetText(pageDatas[newPage].HeaderText);
		urlOpener.SetURL(pageDatas[newPage].URL);
		if (string.IsNullOrEmpty(pageDatas[newPage].ImageText))
		{
			imageLabel.gameObject.SetActive(value: false);
			imageLabel.SetText("");
		}
		else
		{
			imageLabel.gameObject.SetActive(value: true);
			imageLabel.SetText(pageDatas[newPage].ImageText);
		}
	}
}
