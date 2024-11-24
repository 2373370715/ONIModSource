using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020014B0 RID: 5296
internal class LogicEventHandler : ILogicEventReceiver, ILogicNetworkConnection, ILogicUIElement, IUniformGridObject
{
	// Token: 0x06006E40 RID: 28224 RVA: 0x000E840A File Offset: 0x000E660A
	public LogicEventHandler(int cell, Action<int, int> on_value_changed, Action<int, bool> on_connection_changed, LogicPortSpriteType sprite_type)
	{
		this.cell = cell;
		this.onValueChanged = on_value_changed;
		this.onConnectionChanged = on_connection_changed;
		this.spriteType = sprite_type;
	}

	// Token: 0x06006E41 RID: 28225 RVA: 0x002EE3EC File Offset: 0x002EC5EC
	public void ReceiveLogicEvent(int value)
	{
		this.TriggerAudio(value);
		int arg = this.value;
		this.value = value;
		this.onValueChanged(value, arg);
	}

	// Token: 0x1700070F RID: 1807
	// (get) Token: 0x06006E42 RID: 28226 RVA: 0x000E842F File Offset: 0x000E662F
	public int Value
	{
		get
		{
			return this.value;
		}
	}

	// Token: 0x06006E43 RID: 28227 RVA: 0x000E8437 File Offset: 0x000E6637
	public int GetLogicUICell()
	{
		return this.cell;
	}

	// Token: 0x06006E44 RID: 28228 RVA: 0x000E843F File Offset: 0x000E663F
	public LogicPortSpriteType GetLogicPortSpriteType()
	{
		return this.spriteType;
	}

	// Token: 0x06006E45 RID: 28229 RVA: 0x000E8447 File Offset: 0x000E6647
	public Vector2 PosMin()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06006E46 RID: 28230 RVA: 0x000E8447 File Offset: 0x000E6647
	public Vector2 PosMax()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06006E47 RID: 28231 RVA: 0x000E8437 File Offset: 0x000E6637
	public int GetLogicCell()
	{
		return this.cell;
	}

	// Token: 0x06006E48 RID: 28232 RVA: 0x002EE41C File Offset: 0x002EC61C
	private void TriggerAudio(int new_value)
	{
		LogicCircuitNetwork networkForCell = Game.Instance.logicCircuitManager.GetNetworkForCell(this.cell);
		SpeedControlScreen instance = SpeedControlScreen.Instance;
		if (networkForCell != null && new_value != this.value && instance != null && !instance.IsPaused)
		{
			if (KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayAutomation) && KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) != 1 && OverlayScreen.Instance.GetMode() != OverlayModes.Logic.ID)
			{
				return;
			}
			string name = "Logic_Building_Toggle";
			if (!CameraController.Instance.IsAudibleSound(Grid.CellToPosCCC(this.cell, Grid.SceneLayer.BuildingFront)))
			{
				return;
			}
			LogicCircuitNetwork.LogicSoundPair logicSoundPair = new LogicCircuitNetwork.LogicSoundPair();
			Dictionary<int, LogicCircuitNetwork.LogicSoundPair> logicSoundRegister = LogicCircuitNetwork.logicSoundRegister;
			int id = networkForCell.id;
			if (!logicSoundRegister.ContainsKey(id))
			{
				logicSoundRegister.Add(id, logicSoundPair);
			}
			else
			{
				logicSoundPair.playedIndex = logicSoundRegister[id].playedIndex;
				logicSoundPair.lastPlayed = logicSoundRegister[id].lastPlayed;
			}
			if (logicSoundPair.playedIndex < 2)
			{
				logicSoundRegister[id].playedIndex = logicSoundPair.playedIndex + 1;
			}
			else
			{
				logicSoundRegister[id].playedIndex = 0;
				logicSoundRegister[id].lastPlayed = Time.time;
			}
			float num = (Time.time - logicSoundPair.lastPlayed) / 3f;
			EventInstance instance2 = KFMOD.BeginOneShot(GlobalAssets.GetSound(name, false), Grid.CellToPos(this.cell), 1f);
			instance2.setParameterByName("logic_volumeModifer", num, false);
			instance2.setParameterByName("wireCount", (float)(networkForCell.WireCount % 24), false);
			instance2.setParameterByName("enabled", (float)new_value, false);
			KFMOD.EndOneShot(instance2);
		}
	}

	// Token: 0x06006E49 RID: 28233 RVA: 0x000E8459 File Offset: 0x000E6659
	public void OnLogicNetworkConnectionChanged(bool connected)
	{
		if (this.onConnectionChanged != null)
		{
			this.onConnectionChanged(this.cell, connected);
		}
	}

	// Token: 0x0400527A RID: 21114
	private int cell;

	// Token: 0x0400527B RID: 21115
	private int value;

	// Token: 0x0400527C RID: 21116
	private Action<int, int> onValueChanged;

	// Token: 0x0400527D RID: 21117
	private Action<int, bool> onConnectionChanged;

	// Token: 0x0400527E RID: 21118
	private LogicPortSpriteType spriteType;
}
