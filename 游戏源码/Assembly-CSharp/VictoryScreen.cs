using System;
using UnityEngine;

// Token: 0x02002046 RID: 8262
public class VictoryScreen : KModalScreen
{
	// Token: 0x0600AFDF RID: 45023 RVA: 0x001124B6 File Offset: 0x001106B6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Init();
	}

	// Token: 0x0600AFE0 RID: 45024 RVA: 0x001124C4 File Offset: 0x001106C4
	private void Init()
	{
		if (this.DismissButton)
		{
			this.DismissButton.onClick += delegate()
			{
				this.Dismiss();
			};
		}
	}

	// Token: 0x0600AFE1 RID: 45025 RVA: 0x001124EA File Offset: 0x001106EA
	private void Retire()
	{
		if (RetireColonyUtility.SaveColonySummaryData())
		{
			this.Show(false);
		}
	}

	// Token: 0x0600AFE2 RID: 45026 RVA: 0x000FE04E File Offset: 0x000FC24E
	private void Dismiss()
	{
		this.Show(false);
	}

	// Token: 0x0600AFE3 RID: 45027 RVA: 0x00422550 File Offset: 0x00420750
	public void SetAchievements(string[] achievementIDs)
	{
		string text = "";
		for (int i = 0; i < achievementIDs.Length; i++)
		{
			if (i > 0)
			{
				text += "\n";
			}
			text += GameUtil.ApplyBoldString(Db.Get().ColonyAchievements.Get(achievementIDs[i]).Name);
			text = text + "\n" + Db.Get().ColonyAchievements.Get(achievementIDs[i]).description;
		}
		this.descriptionText.text = text;
	}

	// Token: 0x04008A9C RID: 35484
	[SerializeField]
	private KButton DismissButton;

	// Token: 0x04008A9D RID: 35485
	[SerializeField]
	private LocText descriptionText;
}
