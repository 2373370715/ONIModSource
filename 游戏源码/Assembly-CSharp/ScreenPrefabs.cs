using System;
using UnityEngine;

// Token: 0x02001849 RID: 6217
[AddComponentMenu("KMonoBehaviour/scripts/ScreenPrefabs")]
public class ScreenPrefabs : KMonoBehaviour
{
	// Token: 0x17000838 RID: 2104
	// (get) Token: 0x060080A1 RID: 32929 RVA: 0x000F4ABB File Offset: 0x000F2CBB
	// (set) Token: 0x060080A2 RID: 32930 RVA: 0x000F4AC2 File Offset: 0x000F2CC2
	public static ScreenPrefabs Instance { get; private set; }

	// Token: 0x060080A3 RID: 32931 RVA: 0x000F4ACA File Offset: 0x000F2CCA
	protected override void OnPrefabInit()
	{
		ScreenPrefabs.Instance = this;
	}

	// Token: 0x060080A4 RID: 32932 RVA: 0x0033563C File Offset: 0x0033383C
	public void ConfirmDoAction(string message, System.Action action, Transform parent)
	{
		((ConfirmDialogScreen)KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent.gameObject)).PopupConfirmDialog(message, action, delegate
		{
		}, null, null, null, null, null, null);
	}

	// Token: 0x04006171 RID: 24945
	public ControlsScreen ControlsScreen;

	// Token: 0x04006172 RID: 24946
	public Hud HudScreen;

	// Token: 0x04006173 RID: 24947
	public HoverTextScreen HoverTextScreen;

	// Token: 0x04006174 RID: 24948
	public OverlayScreen OverlayScreen;

	// Token: 0x04006175 RID: 24949
	public TileScreen TileScreen;

	// Token: 0x04006176 RID: 24950
	public SpeedControlScreen SpeedControlScreen;

	// Token: 0x04006177 RID: 24951
	public ManagementMenu ManagementMenu;

	// Token: 0x04006178 RID: 24952
	public ToolTipScreen ToolTipScreen;

	// Token: 0x04006179 RID: 24953
	public DebugPaintElementScreen DebugPaintElementScreen;

	// Token: 0x0400617A RID: 24954
	public UserMenuScreen UserMenuScreen;

	// Token: 0x0400617B RID: 24955
	public KButtonMenu OwnerScreen;

	// Token: 0x0400617C RID: 24956
	public KButtonMenu ButtonGrid;

	// Token: 0x0400617D RID: 24957
	public NameDisplayScreen NameDisplayScreen;

	// Token: 0x0400617E RID: 24958
	public ConfirmDialogScreen ConfirmDialogScreen;

	// Token: 0x0400617F RID: 24959
	public CustomizableDialogScreen CustomizableDialogScreen;

	// Token: 0x04006180 RID: 24960
	public SpriteListDialogScreen SpriteListDialogScreen;

	// Token: 0x04006181 RID: 24961
	public InfoDialogScreen InfoDialogScreen;

	// Token: 0x04006182 RID: 24962
	public StoryMessageScreen StoryMessageScreen;

	// Token: 0x04006183 RID: 24963
	public SubSpeciesInfoScreen SubSpeciesInfoScreen;

	// Token: 0x04006184 RID: 24964
	public EventInfoScreen eventInfoScreen;

	// Token: 0x04006185 RID: 24965
	public FileNameDialog FileNameDialog;

	// Token: 0x04006186 RID: 24966
	public TagFilterScreen TagFilterScreen;

	// Token: 0x04006187 RID: 24967
	public ResearchScreen ResearchScreen;

	// Token: 0x04006188 RID: 24968
	public MessageDialogFrame MessageDialogFrame;

	// Token: 0x04006189 RID: 24969
	public ResourceCategoryScreen ResourceCategoryScreen;

	// Token: 0x0400618A RID: 24970
	public ColonyDiagnosticScreen ColonyDiagnosticScreen;

	// Token: 0x0400618B RID: 24971
	public LanguageOptionsScreen languageOptionsScreen;

	// Token: 0x0400618C RID: 24972
	public ModsScreen modsMenu;

	// Token: 0x0400618D RID: 24973
	public RailModUploadScreen RailModUploadMenu;

	// Token: 0x0400618E RID: 24974
	public GameObject GameOverScreen;

	// Token: 0x0400618F RID: 24975
	public GameObject VictoryScreen;

	// Token: 0x04006190 RID: 24976
	public GameObject StatusItemIndicatorScreen;

	// Token: 0x04006191 RID: 24977
	public GameObject CollapsableContentPanel;

	// Token: 0x04006192 RID: 24978
	public GameObject DescriptionLabel;

	// Token: 0x04006193 RID: 24979
	public LoadingOverlay loadingOverlay;

	// Token: 0x04006194 RID: 24980
	public LoadScreen LoadScreen;

	// Token: 0x04006195 RID: 24981
	public InspectSaveScreen InspectSaveScreen;

	// Token: 0x04006196 RID: 24982
	public OptionsMenuScreen OptionsScreen;

	// Token: 0x04006197 RID: 24983
	public WorldGenScreen WorldGenScreen;

	// Token: 0x04006198 RID: 24984
	public ModeSelectScreen ModeSelectScreen;

	// Token: 0x04006199 RID: 24985
	public ColonyDestinationSelectScreen ColonyDestinationSelectScreen;

	// Token: 0x0400619A RID: 24986
	public RetiredColonyInfoScreen RetiredColonyInfoScreen;

	// Token: 0x0400619B RID: 24987
	public VideoScreen VideoScreen;

	// Token: 0x0400619C RID: 24988
	public ComicViewer ComicViewer;

	// Token: 0x0400619D RID: 24989
	public GameObject OldVersionWarningScreen;

	// Token: 0x0400619E RID: 24990
	public GameObject DLCBetaWarningScreen;

	// Token: 0x0400619F RID: 24991
	[Header("Klei Items")]
	public GameObject KleiItemDropScreen;

	// Token: 0x040061A0 RID: 24992
	public GameObject LockerMenuScreen;

	// Token: 0x040061A1 RID: 24993
	public GameObject LockerNavigator;

	// Token: 0x040061A2 RID: 24994
	[Header("Main Menu")]
	public GameObject MainMenuForVanilla;

	// Token: 0x040061A3 RID: 24995
	public GameObject MainMenuForSpacedOut;

	// Token: 0x040061A4 RID: 24996
	public GameObject MainMenuIntroShort;

	// Token: 0x040061A5 RID: 24997
	public GameObject MainMenuHealthyGameMessage;
}
