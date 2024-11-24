using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

// Token: 0x02001AC1 RID: 6849
public class CreditsScreen : KModalScreen
{
	// Token: 0x06008F7D RID: 36733 RVA: 0x00376E34 File Offset: 0x00375034
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (TextAsset csv in this.creditsFiles)
		{
			this.AddCredits(csv);
		}
		this.CloseButton.onClick += this.Close;
	}

	// Token: 0x06008F7E RID: 36734 RVA: 0x000FDC79 File Offset: 0x000FBE79
	public void Close()
	{
		this.Deactivate();
	}

	// Token: 0x06008F7F RID: 36735 RVA: 0x00376E80 File Offset: 0x00375080
	private void AddCredits(TextAsset csv)
	{
		string[,] array = CSVReader.SplitCsvGrid(csv.text, csv.name);
		List<string> list = new List<string>();
		for (int i = 1; i < array.GetLength(1); i++)
		{
			string text = string.Format("{0} {1}", array[0, i], array[1, i]);
			if (!(text == " "))
			{
				list.Add(text);
			}
		}
		list.Shuffle<string>();
		string text2 = array[0, 0];
		GameObject gameObject = Util.KInstantiateUI(this.teamHeaderPrefab, this.entryContainer.gameObject, true);
		gameObject.GetComponent<LocText>().text = text2;
		this.teamContainers.Add(text2, gameObject);
		foreach (string text3 in list)
		{
			Util.KInstantiateUI(this.entryPrefab, this.teamContainers[text2], true).GetComponent<LocText>().text = text3;
		}
	}

	// Token: 0x04006C43 RID: 27715
	public GameObject entryPrefab;

	// Token: 0x04006C44 RID: 27716
	public GameObject teamHeaderPrefab;

	// Token: 0x04006C45 RID: 27717
	private Dictionary<string, GameObject> teamContainers = new Dictionary<string, GameObject>();

	// Token: 0x04006C46 RID: 27718
	public Transform entryContainer;

	// Token: 0x04006C47 RID: 27719
	public KButton CloseButton;

	// Token: 0x04006C48 RID: 27720
	public TextAsset[] creditsFiles;
}
