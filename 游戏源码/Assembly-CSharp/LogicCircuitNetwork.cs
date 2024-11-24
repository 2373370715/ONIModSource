using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x020014B1 RID: 5297
public class LogicCircuitNetwork : UtilityNetwork
{
	// Token: 0x06006E4A RID: 28234 RVA: 0x002EE5CC File Offset: 0x002EC7CC
	public override void AddItem(object item)
	{
		if (item is LogicWire)
		{
			LogicWire logicWire = (LogicWire)item;
			LogicWire.BitDepth maxBitDepth = logicWire.MaxBitDepth;
			List<LogicWire> list = this.wireGroups[(int)maxBitDepth];
			if (list == null)
			{
				list = new List<LogicWire>();
				this.wireGroups[(int)maxBitDepth] = list;
			}
			list.Add(logicWire);
			return;
		}
		if (item is ILogicEventReceiver)
		{
			ILogicEventReceiver item2 = (ILogicEventReceiver)item;
			this.receivers.Add(item2);
			return;
		}
		if (item is ILogicEventSender)
		{
			ILogicEventSender item3 = (ILogicEventSender)item;
			this.senders.Add(item3);
		}
	}

	// Token: 0x06006E4B RID: 28235 RVA: 0x002EE64C File Offset: 0x002EC84C
	public override void RemoveItem(object item)
	{
		if (item is LogicWire)
		{
			LogicWire logicWire = (LogicWire)item;
			this.wireGroups[(int)logicWire.MaxBitDepth].Remove(logicWire);
			return;
		}
		if (item is ILogicEventReceiver)
		{
			ILogicEventReceiver item2 = item as ILogicEventReceiver;
			this.receivers.Remove(item2);
			return;
		}
		if (item is ILogicEventSender)
		{
			ILogicEventSender item3 = (ILogicEventSender)item;
			this.senders.Remove(item3);
		}
	}

	// Token: 0x06006E4C RID: 28236 RVA: 0x000E8475 File Offset: 0x000E6675
	public override void ConnectItem(object item)
	{
		if (item is ILogicEventReceiver)
		{
			((ILogicEventReceiver)item).OnLogicNetworkConnectionChanged(true);
			return;
		}
		if (item is ILogicEventSender)
		{
			((ILogicEventSender)item).OnLogicNetworkConnectionChanged(true);
		}
	}

	// Token: 0x06006E4D RID: 28237 RVA: 0x000E84A0 File Offset: 0x000E66A0
	public override void DisconnectItem(object item)
	{
		if (item is ILogicEventReceiver)
		{
			ILogicEventReceiver logicEventReceiver = item as ILogicEventReceiver;
			logicEventReceiver.ReceiveLogicEvent(0);
			logicEventReceiver.OnLogicNetworkConnectionChanged(false);
			return;
		}
		if (item is ILogicEventSender)
		{
			(item as ILogicEventSender).OnLogicNetworkConnectionChanged(false);
		}
	}

	// Token: 0x06006E4E RID: 28238 RVA: 0x002EE6B8 File Offset: 0x002EC8B8
	public override void Reset(UtilityNetworkGridNode[] grid)
	{
		this.resetting = true;
		this.previousValue = -1;
		this.outputValue = 0;
		for (int i = 0; i < 2; i++)
		{
			List<LogicWire> list = this.wireGroups[i];
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					LogicWire logicWire = list[j];
					if (logicWire != null)
					{
						int num = Grid.PosToCell(logicWire.transform.GetPosition());
						UtilityNetworkGridNode utilityNetworkGridNode = grid[num];
						utilityNetworkGridNode.networkIdx = -1;
						grid[num] = utilityNetworkGridNode;
					}
				}
				list.Clear();
			}
		}
		this.senders.Clear();
		this.receivers.Clear();
		this.resetting = false;
		this.RemoveOverloadedNotification();
	}

	// Token: 0x06006E4F RID: 28239 RVA: 0x002EE76C File Offset: 0x002EC96C
	public void UpdateLogicValue()
	{
		if (this.resetting)
		{
			return;
		}
		this.previousValue = this.outputValue;
		this.outputValue = 0;
		foreach (ILogicEventSender logicEventSender in this.senders)
		{
			logicEventSender.LogicTick();
		}
		foreach (ILogicEventSender logicEventSender2 in this.senders)
		{
			int logicValue = logicEventSender2.GetLogicValue();
			this.outputValue |= logicValue;
		}
	}

	// Token: 0x06006E50 RID: 28240 RVA: 0x002EE828 File Offset: 0x002ECA28
	public int GetBitsUsed()
	{
		int result;
		if (this.outputValue > 1)
		{
			result = 4;
		}
		else
		{
			result = 1;
		}
		return result;
	}

	// Token: 0x06006E51 RID: 28241 RVA: 0x000E84D2 File Offset: 0x000E66D2
	public bool IsBitActive(int bit)
	{
		return (this.OutputValue & 1 << bit) > 0;
	}

	// Token: 0x06006E52 RID: 28242 RVA: 0x000E84E4 File Offset: 0x000E66E4
	public static bool IsBitActive(int bit, int value)
	{
		return (value & 1 << bit) > 0;
	}

	// Token: 0x06006E53 RID: 28243 RVA: 0x000E84F1 File Offset: 0x000E66F1
	public static int GetBitValue(int bit, int value)
	{
		return value & 1 << bit;
	}

	// Token: 0x06006E54 RID: 28244 RVA: 0x002EE848 File Offset: 0x002ECA48
	public void SendLogicEvents(bool force_send, int id)
	{
		if (this.resetting)
		{
			return;
		}
		if (this.outputValue != this.previousValue || force_send)
		{
			foreach (ILogicEventReceiver logicEventReceiver in this.receivers)
			{
				logicEventReceiver.ReceiveLogicEvent(this.outputValue);
			}
			if (!force_send)
			{
				this.TriggerAudio((this.previousValue >= 0) ? this.previousValue : 0, id);
			}
		}
	}

	// Token: 0x06006E55 RID: 28245 RVA: 0x002EE8D8 File Offset: 0x002ECAD8
	private void TriggerAudio(int old_value, int id)
	{
		SpeedControlScreen instance = SpeedControlScreen.Instance;
		if (old_value != this.outputValue && instance != null && !instance.IsPaused)
		{
			int num = 0;
			GridArea visibleArea = GridVisibleArea.GetVisibleArea();
			List<LogicWire> list = new List<LogicWire>();
			for (int i = 0; i < 2; i++)
			{
				List<LogicWire> list2 = this.wireGroups[i];
				if (list2 != null)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						num++;
						if (visibleArea.Min <= list2[j].transform.GetPosition() && list2[j].transform.GetPosition() <= visibleArea.Max)
						{
							list.Add(list2[j]);
						}
					}
				}
			}
			if (list.Count > 0)
			{
				int index = Mathf.CeilToInt((float)(list.Count / 2));
				if (list[index] != null)
				{
					Vector3 position = list[index].transform.GetPosition();
					position.z = 0f;
					string name = "Logic_Circuit_Toggle";
					LogicCircuitNetwork.LogicSoundPair logicSoundPair = new LogicCircuitNetwork.LogicSoundPair();
					if (!LogicCircuitNetwork.logicSoundRegister.ContainsKey(id))
					{
						LogicCircuitNetwork.logicSoundRegister.Add(id, logicSoundPair);
					}
					else
					{
						logicSoundPair.playedIndex = LogicCircuitNetwork.logicSoundRegister[id].playedIndex;
						logicSoundPair.lastPlayed = LogicCircuitNetwork.logicSoundRegister[id].lastPlayed;
					}
					if (logicSoundPair.playedIndex < 2)
					{
						LogicCircuitNetwork.logicSoundRegister[id].playedIndex = logicSoundPair.playedIndex + 1;
					}
					else
					{
						LogicCircuitNetwork.logicSoundRegister[id].playedIndex = 0;
						LogicCircuitNetwork.logicSoundRegister[id].lastPlayed = Time.time;
					}
					float value = (Time.time - logicSoundPair.lastPlayed) / 3f;
					EventInstance instance2 = KFMOD.BeginOneShot(GlobalAssets.GetSound(name, false), position, 1f);
					instance2.setParameterByName("logic_volumeModifer", value, false);
					instance2.setParameterByName("wireCount", (float)(num % 24), false);
					instance2.setParameterByName("enabled", (float)this.outputValue, false);
					KFMOD.EndOneShot(instance2);
				}
			}
		}
	}

	// Token: 0x06006E56 RID: 28246 RVA: 0x002EEB10 File Offset: 0x002ECD10
	public void UpdateOverloadTime(float dt, int bits_used)
	{
		bool flag = false;
		List<LogicWire> list = null;
		List<LogicUtilityNetworkLink> list2 = null;
		for (int i = 0; i < 2; i++)
		{
			List<LogicWire> list3 = this.wireGroups[i];
			List<LogicUtilityNetworkLink> list4 = this.relevantBridges[i];
			float num = (float)LogicWire.GetBitDepthAsInt((LogicWire.BitDepth)i);
			if ((float)bits_used > num && ((list4 != null && list4.Count > 0) || (list3 != null && list3.Count > 0)))
			{
				flag = true;
				list = list3;
				list2 = list4;
				break;
			}
		}
		if (list != null)
		{
			list.RemoveAll((LogicWire x) => x == null);
		}
		if (list2 != null)
		{
			list2.RemoveAll((LogicUtilityNetworkLink x) => x == null);
		}
		if (flag)
		{
			this.timeOverloaded += dt;
			if (this.timeOverloaded > 6f)
			{
				this.timeOverloaded = 0f;
				if (this.targetOverloadedWire == null)
				{
					if (list2 != null && list2.Count > 0)
					{
						int index = UnityEngine.Random.Range(0, list2.Count);
						this.targetOverloadedWire = list2[index].gameObject;
					}
					else if (list != null && list.Count > 0)
					{
						int index2 = UnityEngine.Random.Range(0, list.Count);
						this.targetOverloadedWire = list[index2].gameObject;
					}
				}
				if (this.targetOverloadedWire != null)
				{
					this.targetOverloadedWire.Trigger(-794517298, new BuildingHP.DamageSourceInfo
					{
						damage = 1,
						source = BUILDINGS.DAMAGESOURCES.LOGIC_CIRCUIT_OVERLOADED,
						popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.LOGIC_CIRCUIT_OVERLOADED,
						takeDamageEffect = SpawnFXHashes.BuildingLogicOverload,
						fullDamageEffectName = "logic_ribbon_damage_kanim",
						statusItemID = Db.Get().BuildingStatusItems.LogicOverloaded.Id
					});
				}
				if (this.overloadedNotification == null)
				{
					this.timeOverloadNotificationDisplayed = 0f;
					this.overloadedNotification = new Notification(MISC.NOTIFICATIONS.LOGIC_CIRCUIT_OVERLOADED.NAME, NotificationType.BadMinor, null, null, true, 0f, null, null, this.targetOverloadedWire.transform, true, false, false);
					Game.Instance.FindOrAdd<Notifier>().Add(this.overloadedNotification, "");
					return;
				}
			}
		}
		else
		{
			this.timeOverloaded = Mathf.Max(0f, this.timeOverloaded - dt * 0.95f);
			this.timeOverloadNotificationDisplayed += dt;
			if (this.timeOverloadNotificationDisplayed > 5f)
			{
				this.RemoveOverloadedNotification();
			}
		}
	}

	// Token: 0x06006E57 RID: 28247 RVA: 0x000E84FB File Offset: 0x000E66FB
	private void RemoveOverloadedNotification()
	{
		if (this.overloadedNotification != null)
		{
			Game.Instance.FindOrAdd<Notifier>().Remove(this.overloadedNotification);
			this.overloadedNotification = null;
		}
	}

	// Token: 0x06006E58 RID: 28248 RVA: 0x002EED8C File Offset: 0x002ECF8C
	public void UpdateRelevantBridges(List<LogicUtilityNetworkLink>[] bridgeGroups)
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		for (int i = 0; i < bridgeGroups.Length; i++)
		{
			if (this.relevantBridges[i] != null)
			{
				this.relevantBridges[i].Clear();
			}
			for (int j = 0; j < bridgeGroups[i].Count; j++)
			{
				if (logicCircuitManager.GetNetworkForCell(bridgeGroups[i][j].cell_one) == this || logicCircuitManager.GetNetworkForCell(bridgeGroups[i][j].cell_two) == this)
				{
					if (this.relevantBridges[i] == null)
					{
						this.relevantBridges[i] = new List<LogicUtilityNetworkLink>();
					}
					this.relevantBridges[i].Add(bridgeGroups[i][j]);
				}
			}
		}
	}

	// Token: 0x17000710 RID: 1808
	// (get) Token: 0x06006E59 RID: 28249 RVA: 0x000E8521 File Offset: 0x000E6721
	public int OutputValue
	{
		get
		{
			return this.outputValue;
		}
	}

	// Token: 0x17000711 RID: 1809
	// (get) Token: 0x06006E5A RID: 28250 RVA: 0x002EEE40 File Offset: 0x002ED040
	public int WireCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < 2; i++)
			{
				if (this.wireGroups[i] != null)
				{
					num += this.wireGroups[i].Count;
				}
			}
			return num;
		}
	}

	// Token: 0x17000712 RID: 1810
	// (get) Token: 0x06006E5B RID: 28251 RVA: 0x000E8529 File Offset: 0x000E6729
	public ReadOnlyCollection<ILogicEventSender> Senders
	{
		get
		{
			return this.senders.AsReadOnly();
		}
	}

	// Token: 0x17000713 RID: 1811
	// (get) Token: 0x06006E5C RID: 28252 RVA: 0x000E8536 File Offset: 0x000E6736
	public ReadOnlyCollection<ILogicEventReceiver> Receivers
	{
		get
		{
			return this.receivers.AsReadOnly();
		}
	}

	// Token: 0x0400527F RID: 21119
	private List<LogicWire>[] wireGroups = new List<LogicWire>[2];

	// Token: 0x04005280 RID: 21120
	private List<LogicUtilityNetworkLink>[] relevantBridges = new List<LogicUtilityNetworkLink>[2];

	// Token: 0x04005281 RID: 21121
	private List<ILogicEventReceiver> receivers = new List<ILogicEventReceiver>();

	// Token: 0x04005282 RID: 21122
	private List<ILogicEventSender> senders = new List<ILogicEventSender>();

	// Token: 0x04005283 RID: 21123
	private int previousValue = -1;

	// Token: 0x04005284 RID: 21124
	private int outputValue;

	// Token: 0x04005285 RID: 21125
	private bool resetting;

	// Token: 0x04005286 RID: 21126
	public static float logicSoundLastPlayedTime = 0f;

	// Token: 0x04005287 RID: 21127
	private const float MIN_OVERLOAD_TIME_FOR_DAMAGE = 6f;

	// Token: 0x04005288 RID: 21128
	private const float MIN_OVERLOAD_NOTIFICATION_DISPLAY_TIME = 5f;

	// Token: 0x04005289 RID: 21129
	public const int VALID_LOGIC_SIGNAL_MASK = 15;

	// Token: 0x0400528A RID: 21130
	public const int UNINITIALIZED_LOGIC_STATE = -16;

	// Token: 0x0400528B RID: 21131
	private GameObject targetOverloadedWire;

	// Token: 0x0400528C RID: 21132
	private float timeOverloaded;

	// Token: 0x0400528D RID: 21133
	private float timeOverloadNotificationDisplayed;

	// Token: 0x0400528E RID: 21134
	private Notification overloadedNotification;

	// Token: 0x0400528F RID: 21135
	public static Dictionary<int, LogicCircuitNetwork.LogicSoundPair> logicSoundRegister = new Dictionary<int, LogicCircuitNetwork.LogicSoundPair>();

	// Token: 0x020014B2 RID: 5298
	public class LogicSoundPair
	{
		// Token: 0x04005290 RID: 21136
		public int playedIndex;

		// Token: 0x04005291 RID: 21137
		public float lastPlayed;
	}
}
