using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/BreakdownList")]
public class BreakdownList : KMonoBehaviour {
    private readonly List<GameObject>       customRows = new List<GameObject>();
    public           Image                  headerBar;
    public           Image                  headerIcon;
    public           Sprite                 headerIconSprite;
    public           LocText                headerTitle;
    public           LocText                headerValue;
    public           LocText                infoTextLabel;
    private readonly List<BreakdownListRow> listRows = new List<BreakdownListRow>();
    public           BreakdownListRow       listRowTemplate;
    private readonly List<BreakdownListRow> unusedListRows = new List<BreakdownListRow>();

    public BreakdownListRow AddRow() {
        BreakdownListRow breakdownListRow;
        if (unusedListRows.Count > 0) {
            breakdownListRow = unusedListRows[0];
            unusedListRows.RemoveAt(0);
        } else
            breakdownListRow = Instantiate(listRowTemplate);

        breakdownListRow.gameObject.transform.SetParent(transform);
        breakdownListRow.gameObject.transform.SetAsLastSibling();
        listRows.Add(breakdownListRow);
        breakdownListRow.gameObject.SetActive(true);
        return breakdownListRow;
    }

    public GameObject AddCustomRow(GameObject newRow) {
        newRow.transform.SetParent(transform);
        newRow.gameObject.transform.SetAsLastSibling();
        customRows.Add(newRow);
        newRow.SetActive(true);
        return newRow;
    }

    public void ClearRows() {
        foreach (var breakdownListRow in listRows) {
            unusedListRows.Add(breakdownListRow);
            breakdownListRow.gameObject.SetActive(false);
            breakdownListRow.ClearTooltip();
        }

        listRows.Clear();
        foreach (var gameObject in customRows) gameObject.SetActive(false);
    }

    public void SetTitle(string title) { headerTitle.text = title; }

    public void SetDescription(string description) {
        if (description != null && description.Length >= 0) {
            infoTextLabel.gameObject.SetActive(true);
            infoTextLabel.text = description;
            return;
        }

        infoTextLabel.gameObject.SetActive(false);
    }

    public void SetIcon(Sprite icon) { headerIcon.sprite = icon; }
}