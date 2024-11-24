using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KleiInventoryUISubcategory : KMonoBehaviour
{
	[SerializeField]
	private GameObject dummyPrefab;

	public string subcategoryID;

	public GridLayoutGroup gridLayout;

	public List<GameObject> dummyItems;

	[SerializeField]
	private LayoutElement headerLayout;

	[SerializeField]
	private Image icon;

	[SerializeField]
	private LocText label;

	[SerializeField]
	private MultiToggle expandButton;

	private bool stateExpanded = true;

	public bool IsOpen => stateExpanded;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		expandButton.onClick = delegate
		{
			ToggleOpen(!stateExpanded);
		};
	}

	public void SetIdentity(string label, Sprite icon)
	{
		this.label.SetText(label);
		this.icon.sprite = icon;
	}

	public void RefreshDisplay()
	{
		foreach (GameObject dummyItem in dummyItems)
		{
			dummyItem.SetActive(value: false);
		}
		int num = 0;
		for (int i = 0; i < gridLayout.transform.childCount; i++)
		{
			if (gridLayout.transform.GetChild(i).gameObject.activeSelf)
			{
				num++;
			}
		}
		base.gameObject.SetActive(num != 0);
		int num2 = 0;
		int num3 = num % gridLayout.constraintCount;
		if (num3 > 0)
		{
			num2 = gridLayout.constraintCount - num3;
		}
		while (num2 > dummyItems.Count)
		{
			dummyItems.Add(Util.KInstantiateUI(dummyPrefab, gridLayout.gameObject));
		}
		for (int j = 0; j < num2; j++)
		{
			dummyItems[j].SetActive(value: true);
			dummyItems[j].transform.SetAsLastSibling();
		}
		headerLayout.minWidth = base.transform.parent.rectTransform().rect.width - 8f;
	}

	public void ToggleOpen(bool open)
	{
		gridLayout.gameObject.SetActive(open);
		stateExpanded = open;
		expandButton.ChangeState(stateExpanded ? 1 : 0);
	}
}
