using System.Collections.Generic;
using Database;
using UnityEngine;

public class MonumentSideScreen : SideScreenContent
{
	private MonumentPart target;

	public KButton debugVictoryButton;

	public KButton flipButton;

	public GameObject stateButtonPrefab;

	private List<GameObject> buttons = new List<GameObject>();

	[SerializeField]
	private RectTransform buttonContainer;

	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<MonumentPart>() != null;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		debugVictoryButton.onClick += delegate
		{
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Thriving.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Clothe8Dupes.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Build4NatureReserves.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.ReachedSpace.Id);
			GameScheduler.Instance.Schedule("ForceCheckAchievements", 0.1f, delegate
			{
				Game.Instance.Trigger(395452326);
			});
		};
		debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && target.part == MonumentPartResource.Part.Top);
		flipButton.onClick += delegate
		{
			target.GetComponent<Rotatable>().Rotate();
		};
	}

	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.target = target.GetComponent<MonumentPart>();
		debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.target.part == MonumentPartResource.Part.Top);
		GenerateStateButtons();
	}

	public void GenerateStateButtons()
	{
		for (int num = buttons.Count - 1; num >= 0; num--)
		{
			Util.KDestroyGameObject(buttons[num]);
		}
		buttons.Clear();
		foreach (MonumentPartResource state in Db.GetMonumentParts().GetParts(target.part))
		{
			GameObject gameObject = Util.KInstantiateUI(stateButtonPrefab, buttonContainer.gameObject, force_active: true);
			string state2 = state.State;
			string symbolName = state.SymbolName;
			gameObject.GetComponent<KButton>().onClick += delegate
			{
				target.SetState(state.Id);
			};
			buttons.Add(gameObject);
			gameObject.GetComponent<KButton>().fgImage.sprite = Def.GetUISpriteFromMultiObjectAnim(state.AnimFile, state2, centered: false, symbolName);
		}
	}
}
