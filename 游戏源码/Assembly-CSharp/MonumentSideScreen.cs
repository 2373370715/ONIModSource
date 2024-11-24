using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02001F8D RID: 8077
public class MonumentSideScreen : SideScreenContent
{
	// Token: 0x0600AA88 RID: 43656 RVA: 0x0010EC41 File Offset: 0x0010CE41
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<MonumentPart>() != null;
	}

	// Token: 0x0600AA89 RID: 43657 RVA: 0x00406528 File Offset: 0x00404728
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.debugVictoryButton.onClick += delegate()
		{
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Thriving.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Clothe8Dupes.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Build4NatureReserves.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.ReachedSpace.Id);
			GameScheduler.Instance.Schedule("ForceCheckAchievements", 0.1f, delegate(object data)
			{
				Game.Instance.Trigger(395452326, null);
			}, null, null);
		};
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.target.part == MonumentPartResource.Part.Top);
		this.flipButton.onClick += delegate()
		{
			this.target.GetComponent<Rotatable>().Rotate();
		};
	}

	// Token: 0x0600AA8A RID: 43658 RVA: 0x004065A4 File Offset: 0x004047A4
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.target = target.GetComponent<MonumentPart>();
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.target.part == MonumentPartResource.Part.Top);
		this.GenerateStateButtons();
	}

	// Token: 0x0600AA8B RID: 43659 RVA: 0x004065F4 File Offset: 0x004047F4
	public void GenerateStateButtons()
	{
		for (int i = this.buttons.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.buttons[i]);
		}
		this.buttons.Clear();
		using (List<MonumentPartResource>.Enumerator enumerator = Db.GetMonumentParts().GetParts(this.target.part).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MonumentPartResource state = enumerator.Current;
				GameObject gameObject = Util.KInstantiateUI(this.stateButtonPrefab, this.buttonContainer.gameObject, true);
				string state2 = state.State;
				string symbolName = state.SymbolName;
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.target.SetState(state.Id);
				};
				this.buttons.Add(gameObject);
				gameObject.GetComponent<KButton>().fgImage.sprite = Def.GetUISpriteFromMultiObjectAnim(state.AnimFile, state2, false, symbolName);
			}
		}
	}

	// Token: 0x0400860F RID: 34319
	private MonumentPart target;

	// Token: 0x04008610 RID: 34320
	public KButton debugVictoryButton;

	// Token: 0x04008611 RID: 34321
	public KButton flipButton;

	// Token: 0x04008612 RID: 34322
	public GameObject stateButtonPrefab;

	// Token: 0x04008613 RID: 34323
	private List<GameObject> buttons = new List<GameObject>();

	// Token: 0x04008614 RID: 34324
	[SerializeField]
	private RectTransform buttonContainer;
}
