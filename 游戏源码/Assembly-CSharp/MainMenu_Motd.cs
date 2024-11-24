using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class MainMenu_Motd
{
	[SerializeField]
	private MotdBox boxA;

	[SerializeField]
	private MotdBox boxB;

	[SerializeField]
	private MotdBox boxC;

	private MotdDataFetchRequest motdDataFetchRequest;

	public void Setup()
	{
		CleanUp();
		boxA.gameObject.SetActive(value: false);
		boxB.gameObject.SetActive(value: false);
		boxC.gameObject.SetActive(value: false);
		motdDataFetchRequest = new MotdDataFetchRequest();
		motdDataFetchRequest.Fetch(MotdDataFetchRequest.BuildUrl());
		motdDataFetchRequest.OnComplete(delegate(MotdData motdData)
		{
			RecieveMotdData(motdData);
		});
	}

	public void CleanUp()
	{
		if (motdDataFetchRequest != null)
		{
			motdDataFetchRequest.Dispose();
			motdDataFetchRequest = null;
		}
	}

	private void RecieveMotdData(MotdData motdData)
	{
		if (motdData == null || motdData.boxesLive == null || motdData.boxesLive.Count == 0)
		{
			Debug.LogWarning("MOTD Error: failed to get valid motd data, hiding ui.");
			boxA.gameObject.SetActive(value: false);
			boxB.gameObject.SetActive(value: false);
			boxC.gameObject.SetActive(value: false);
			return;
		}
		List<MotdData_Box> boxes = motdData.boxesLive.StableSort((MotdData_Box a, MotdData_Box b) => CalcScore(a).CompareTo(CalcScore(b))).ToList();
		MotdData_Box motdData_Box = ConsumeBox("PatchNotes");
		MotdData_Box motdData_Box2 = ConsumeBox("News");
		MotdData_Box motdData_Box3 = ConsumeBox("Skins");
		if (motdData_Box != null)
		{
			boxA.Config(new MotdBox.PageData[1] { ConvertToPageData(motdData_Box) });
			boxA.gameObject.SetActive(value: true);
		}
		if (motdData_Box2 != null)
		{
			boxB.Config(new MotdBox.PageData[1] { ConvertToPageData(motdData_Box2) });
			boxB.gameObject.SetActive(value: true);
		}
		if (motdData_Box3 != null)
		{
			boxC.Config(new MotdBox.PageData[1] { ConvertToPageData(motdData_Box3) });
			boxC.gameObject.SetActive(value: true);
		}
		MotdData_Box ConsumeBox(string idealTag)
		{
			if (boxes.Count != 0)
			{
				int num = -1;
				for (int i = 0; i < boxes.Count; i++)
				{
					if (string.Compare(boxes[i].category, idealTag, StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						num = i;
						break;
					}
				}
				if (num < 0)
				{
					num = boxes.Count - 1;
				}
				MotdData_Box result = boxes[num];
				boxes.RemoveAt(num);
				return result;
			}
			return null;
		}
	}

	private int CalcScore(MotdData_Box box)
	{
		return 0;
	}

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
}
