using System;
using System.Collections;
using FMOD.Studio;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02001AED RID: 6893
public class MinionSelectScreen : CharacterSelectionController
{
	// Token: 0x0600909B RID: 37019 RVA: 0x0037CDE0 File Offset: 0x0037AFE0
	protected override void OnPrefabInit()
	{
		base.IsStarterMinion = true;
		base.OnPrefabInit();
		if (MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
		{
			MusicManager.instance.SetSongParameter("Music_FrontEnd", "songSection", 2f, true);
		}
		GameObject parent = GameObject.Find("ScreenSpaceOverlayCanvas");
		GameObject gameObject = global::Util.KInstantiateUI(this.wattsonMessagePrefab.gameObject, parent, false);
		gameObject.name = "WattsonMessage";
		gameObject.SetActive(false);
		Game.Instance.Subscribe(-1992507039, new Action<object>(this.OnBaseAlreadyCreated));
		this.backButton.onClick += delegate()
		{
			LoadScreen.ForceStopGame();
			App.LoadScene("frontend");
		};
		this.InitializeContainers();
		base.StartCoroutine(this.SetDefaultMinionsRoutine());
	}

	// Token: 0x0600909C RID: 37020 RVA: 0x000FEA1E File Offset: 0x000FCC1E
	private IEnumerator SetDefaultMinionsRoutine()
	{
		yield return SequenceUtil.WaitForNextFrame;
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		if (SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id).clusterTags.Contains("CeresCluster"))
		{
			((CharacterContainer)this.containers[2]).SetMinion(new MinionStartingStats(Db.Get().Personalities.Get("FREYJA"), null, null, false));
			((CharacterContainer)this.containers[1]).GenerateCharacter(true, null);
			((CharacterContainer)this.containers[0]).GenerateCharacter(true, null);
		}
		yield break;
	}

	// Token: 0x0600909D RID: 37021 RVA: 0x0037CEAC File Offset: 0x0037B0AC
	public void SetProceedButtonActive(bool state, string tooltip = null)
	{
		if (state)
		{
			base.EnableProceedButton();
		}
		else
		{
			base.DisableProceedButton();
		}
		ToolTip component = this.proceedButton.GetComponent<ToolTip>();
		if (component != null)
		{
			if (tooltip != null)
			{
				component.toolTip = tooltip;
				return;
			}
			component.ClearMultiStringTooltip();
		}
	}

	// Token: 0x0600909E RID: 37022 RVA: 0x0037CEF0 File Offset: 0x0037B0F0
	protected override void OnSpawn()
	{
		this.OnDeliverableAdded();
		base.EnableProceedButton();
		this.proceedButton.GetComponentInChildren<LocText>().text = UI.IMMIGRANTSCREEN.EMBARK;
		this.containers.ForEach(delegate(ITelepadDeliverableContainer container)
		{
			CharacterContainer characterContainer = container as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.DisableSelectButton();
			}
		});
	}

	// Token: 0x0600909F RID: 37023 RVA: 0x0037CF50 File Offset: 0x0037B150
	protected override void OnProceed()
	{
		global::Util.KInstantiateUI(this.newBasePrefab.gameObject, GameScreenManager.Instance.ssOverlayCanvas, false);
		MusicManager.instance.StopSong("Music_FrontEnd", true, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().NewBaseSetupSnapshot);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWorldGenerationSnapshot, STOP_MODE.ALLOWFADEOUT);
		this.selectedDeliverables.Clear();
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = (CharacterContainer)telepadDeliverableContainer;
			this.selectedDeliverables.Add(characterContainer.Stats);
		}
		NewBaseScreen.Instance.Init(SaveLoader.Instance.Cluster, this.selectedDeliverables.ToArray());
		if (this.OnProceedEvent != null)
		{
			this.OnProceedEvent();
		}
		Game.Instance.Trigger(-838649377, null);
		BuildWatermark.Instance.gameObject.SetActive(false);
		this.Deactivate();
	}

	// Token: 0x060090A0 RID: 37024 RVA: 0x000FEA2D File Offset: 0x000FCC2D
	private void OnBaseAlreadyCreated(object data)
	{
		Game.Instance.StopFE();
		Game.Instance.StartBE();
		Game.Instance.SetGameStarted();
		this.Deactivate();
	}

	// Token: 0x060090A1 RID: 37025 RVA: 0x000FEA53 File Offset: 0x000FCC53
	private void ReshuffleAll()
	{
		if (this.OnReshuffleEvent != null)
		{
			this.OnReshuffleEvent(base.IsStarterMinion);
		}
	}

	// Token: 0x060090A2 RID: 37026 RVA: 0x0037D070 File Offset: 0x0037B270
	public override void OnPressBack()
	{
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = telepadDeliverableContainer as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.ForceStopEditingTitle();
			}
		}
	}

	// Token: 0x04006D4B RID: 27979
	[SerializeField]
	private NewBaseScreen newBasePrefab;

	// Token: 0x04006D4C RID: 27980
	[SerializeField]
	private WattsonMessage wattsonMessagePrefab;

	// Token: 0x04006D4D RID: 27981
	public const string WattsonGameObjName = "WattsonMessage";

	// Token: 0x04006D4E RID: 27982
	public KButton backButton;
}
