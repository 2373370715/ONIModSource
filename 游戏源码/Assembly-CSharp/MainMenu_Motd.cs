using System;
using System.Linq;
using UnityEngine;

// Token: 0x02001D8B RID: 7563
[Serializable]
public class MainMenu_Motd
{
	// Token: 0x06009E19 RID: 40473 RVA: 0x003C9DC8 File Offset: 0x003C7FC8
	public void Setup()
	{
		this.CleanUp();
		this.boxA.gameObject.SetActive(false);
		this.boxB.gameObject.SetActive(false);
		this.boxC.gameObject.SetActive(false);
		this.motdDataFetchRequest = new MotdDataFetchRequest();
		this.motdDataFetchRequest.Fetch(MotdDataFetchRequest.BuildUrl());
		this.motdDataFetchRequest.OnComplete(delegate(MotdData motdData)
		{
			this.RecieveMotdData(motdData);
		});
	}

	// Token: 0x06009E1A RID: 40474 RVA: 0x00106F72 File Offset: 0x00105172
	public void CleanUp()
	{
		if (this.motdDataFetchRequest != null)
		{
			this.motdDataFetchRequest.Dispose();
			this.motdDataFetchRequest = null;
		}
	}

	// Token: 0x06009E1B RID: 40475 RVA: 0x003C9E40 File Offset: 0x003C8040
	private void RecieveMotdData(MotdData motdData)
	{
		MainMenu_Motd.<>c__DisplayClass6_0 CS$<>8__locals1 = new MainMenu_Motd.<>c__DisplayClass6_0();
		CS$<>8__locals1.<>4__this = this;
		if (motdData == null || motdData.boxesLive == null || motdData.boxesLive.Count == 0)
		{
			global::Debug.LogWarning("MOTD Error: failed to get valid motd data, hiding ui.");
			this.boxA.gameObject.SetActive(false);
			this.boxB.gameObject.SetActive(false);
			this.boxC.gameObject.SetActive(false);
			return;
		}
		CS$<>8__locals1.boxes = motdData.boxesLive.StableSort((MotdData_Box a, MotdData_Box b) => CS$<>8__locals1.<>4__this.CalcScore(a).CompareTo(CS$<>8__locals1.<>4__this.CalcScore(b))).ToList<MotdData_Box>();
		MotdData_Box motdData_Box = CS$<>8__locals1.<RecieveMotdData>g__ConsumeBox|1("PatchNotes");
		MotdData_Box motdData_Box2 = CS$<>8__locals1.<RecieveMotdData>g__ConsumeBox|1("News");
		MotdData_Box motdData_Box3 = CS$<>8__locals1.<RecieveMotdData>g__ConsumeBox|1("Skins");
		if (motdData_Box != null)
		{
			this.boxA.Config(new MotdBox.PageData[]
			{
				this.ConvertToPageData(motdData_Box)
			});
			this.boxA.gameObject.SetActive(true);
		}
		if (motdData_Box2 != null)
		{
			this.boxB.Config(new MotdBox.PageData[]
			{
				this.ConvertToPageData(motdData_Box2)
			});
			this.boxB.gameObject.SetActive(true);
		}
		if (motdData_Box3 != null)
		{
			this.boxC.Config(new MotdBox.PageData[]
			{
				this.ConvertToPageData(motdData_Box3)
			});
			this.boxC.gameObject.SetActive(true);
		}
	}

	// Token: 0x06009E1C RID: 40476 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	private int CalcScore(MotdData_Box box)
	{
		return 0;
	}

	// Token: 0x06009E1D RID: 40477 RVA: 0x00106F8E File Offset: 0x0010518E
	private MotdBox.PageData ConvertToPageData(MotdData_Box box)
	{
		return new MotdBox.PageData
		{
			Texture = box.resolvedImage,
			HeaderText = box.title,
			ImageText = box.text,
			URL = box.href
		};
	}

	// Token: 0x04007BE7 RID: 31719
	[SerializeField]
	private MotdBox boxA;

	// Token: 0x04007BE8 RID: 31720
	[SerializeField]
	private MotdBox boxB;

	// Token: 0x04007BE9 RID: 31721
	[SerializeField]
	private MotdBox boxC;

	// Token: 0x04007BEA RID: 31722
	private MotdDataFetchRequest motdDataFetchRequest;
}
