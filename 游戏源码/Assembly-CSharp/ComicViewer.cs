using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C74 RID: 7284
public class ComicViewer : KScreen
{
	// Token: 0x060097F1 RID: 38897 RVA: 0x003AE430 File Offset: 0x003AC630
	public void ShowComic(ComicData comic, bool isVictoryComic)
	{
		for (int i = 0; i < Mathf.Max(comic.images.Length, comic.stringKeys.Length); i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.panelPrefab, this.contentContainer, true);
			this.activePanels.Add(gameObject);
			gameObject.GetComponentInChildren<Image>().sprite = comic.images[i];
			gameObject.GetComponentInChildren<LocText>().SetText(comic.stringKeys[i]);
		}
		this.closeButton.ClearOnClick();
		if (isVictoryComic)
		{
			this.closeButton.onClick += delegate()
			{
				this.Stop();
				this.Show(false);
			};
			return;
		}
		this.closeButton.onClick += delegate()
		{
			this.Stop();
		};
	}

	// Token: 0x060097F2 RID: 38898 RVA: 0x00102C0F File Offset: 0x00100E0F
	public void Stop()
	{
		this.OnStop();
		this.Show(false);
		base.gameObject.SetActive(false);
	}

	// Token: 0x04007640 RID: 30272
	public GameObject panelPrefab;

	// Token: 0x04007641 RID: 30273
	public GameObject contentContainer;

	// Token: 0x04007642 RID: 30274
	public List<GameObject> activePanels = new List<GameObject>();

	// Token: 0x04007643 RID: 30275
	public KButton closeButton;

	// Token: 0x04007644 RID: 30276
	public System.Action OnStop;
}
