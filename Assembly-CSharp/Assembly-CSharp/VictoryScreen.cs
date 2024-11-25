﻿using System;
using UnityEngine;

public class VictoryScreen : KModalScreen
{
		protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Init();
	}

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

		private void Retire()
	{
		if (RetireColonyUtility.SaveColonySummaryData())
		{
			this.Show(false);
		}
	}

		private void Dismiss()
	{
		this.Show(false);
	}

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

		[SerializeField]
	private KButton DismissButton;

		[SerializeField]
	private LocText descriptionText;
}
