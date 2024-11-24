using System;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02001C12 RID: 7186
public class BuildingGroupScreen : KScreen
{
	// Token: 0x170009B7 RID: 2487
	// (get) Token: 0x06009560 RID: 38240 RVA: 0x0010143A File Offset: 0x000FF63A
	public static bool SearchIsEmpty
	{
		get
		{
			return BuildingGroupScreen.Instance == null || BuildingGroupScreen.Instance.inputField.text.IsNullOrWhiteSpace();
		}
	}

	// Token: 0x170009B8 RID: 2488
	// (get) Token: 0x06009561 RID: 38241 RVA: 0x0010145F File Offset: 0x000FF65F
	public static bool IsEditing
	{
		get
		{
			return !(BuildingGroupScreen.Instance == null) && BuildingGroupScreen.Instance.isEditing;
		}
	}

	// Token: 0x06009562 RID: 38242 RVA: 0x0010147A File Offset: 0x000FF67A
	protected override void OnPrefabInit()
	{
		BuildingGroupScreen.Instance = this;
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06009563 RID: 38243 RVA: 0x0039C538 File Offset: 0x0039A738
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KInputTextField kinputTextField = this.inputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
			UISounds.PlaySound(UISounds.Sound.ClickHUD);
			this.ConfigurePlanScreenForSearch();
		}));
		this.inputField.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
		});
		this.inputField.onValueChanged.AddListener(delegate(string value)
		{
			PlanScreen.Instance.RefreshCategoryPanelTitle();
		});
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.BUILDMENU.SEARCH_TEXT_PLACEHOLDER;
		this.clearButton.onClick += this.ClearSearch;
	}

	// Token: 0x06009564 RID: 38244 RVA: 0x0010148F File Offset: 0x000FF68F
	protected override void OnActivate()
	{
		base.OnActivate();
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06009565 RID: 38245 RVA: 0x0010149E File Offset: 0x000FF69E
	public void ClearSearch()
	{
		this.inputField.text = "";
	}

	// Token: 0x06009566 RID: 38246 RVA: 0x001014B0 File Offset: 0x000FF6B0
	private void ConfigurePlanScreenForSearch()
	{
		PlanScreen.Instance.SoftCloseRecipe();
		PlanScreen.Instance.ClearSelection();
		PlanScreen.Instance.ForceRefreshAllBuildingToggles();
		PlanScreen.Instance.ConfigurePanelSize(null);
	}

	// Token: 0x040073FE RID: 29694
	public static BuildingGroupScreen Instance;

	// Token: 0x040073FF RID: 29695
	public KInputTextField inputField;

	// Token: 0x04007400 RID: 29696
	[SerializeField]
	public KButton clearButton;
}
