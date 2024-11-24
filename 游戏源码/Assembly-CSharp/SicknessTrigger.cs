using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using UnityEngine;

// Token: 0x02001861 RID: 6241
[AddComponentMenu("KMonoBehaviour/scripts/SicknessTrigger")]
public class SicknessTrigger : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x060080FC RID: 33020 RVA: 0x003367F8 File Offset: 0x003349F8
	public void AddTrigger(GameHashes src_event, string[] sickness_ids, SicknessTrigger.SourceCallback source_callback)
	{
		this.triggers.Add(new SicknessTrigger.TriggerInfo
		{
			srcEvent = src_event,
			sickness_ids = sickness_ids,
			sourceCallback = source_callback
		});
	}

	// Token: 0x060080FD RID: 33021 RVA: 0x00336834 File Offset: 0x00334A34
	protected override void OnSpawn()
	{
		for (int i = 0; i < this.triggers.Count; i++)
		{
			SicknessTrigger.TriggerInfo trigger = this.triggers[i];
			base.Subscribe((int)trigger.srcEvent, delegate(object data)
			{
				this.OnSicknessTrigger((GameObject)data, trigger);
			});
		}
	}

	// Token: 0x060080FE RID: 33022 RVA: 0x00336894 File Offset: 0x00334A94
	private void OnSicknessTrigger(GameObject target, SicknessTrigger.TriggerInfo trigger)
	{
		int num = UnityEngine.Random.Range(0, trigger.sickness_ids.Length);
		string text = trigger.sickness_ids[num];
		Sickness sickness = null;
		Database.Sicknesses sicknesses = Db.Get().Sicknesses;
		for (int i = 0; i < sicknesses.Count; i++)
		{
			if (sicknesses[i].Id == text)
			{
				sickness = sicknesses[i];
				break;
			}
		}
		if (sickness != null)
		{
			string infection_source_info = trigger.sourceCallback(base.gameObject, target);
			SicknessExposureInfo exposure_info = new SicknessExposureInfo(sickness.Id, infection_source_info);
			target.GetComponent<MinionModifiers>().sicknesses.Infect(exposure_info);
			return;
		}
		DebugUtil.DevLogErrorFormat(base.gameObject, "Couldn't find sickness with id [{0}]", new object[]
		{
			text
		});
	}

	// Token: 0x060080FF RID: 33023 RVA: 0x00336950 File Offset: 0x00334B50
	public List<Descriptor> EffectDescriptors(GameObject go)
	{
		Dictionary<GameHashes, HashSet<string>> dictionary = new Dictionary<GameHashes, HashSet<string>>();
		foreach (SicknessTrigger.TriggerInfo triggerInfo in this.triggers)
		{
			HashSet<string> hashSet = null;
			if (!dictionary.TryGetValue(triggerInfo.srcEvent, out hashSet))
			{
				hashSet = new HashSet<string>();
				dictionary[triggerInfo.srcEvent] = hashSet;
			}
			foreach (string item in triggerInfo.sickness_ids)
			{
				hashSet.Add(item);
			}
		}
		List<Descriptor> list = new List<Descriptor>();
		List<string> list2 = new List<string>();
		string properName = base.GetComponent<KSelectable>().GetProperName();
		foreach (KeyValuePair<GameHashes, HashSet<string>> keyValuePair in dictionary)
		{
			HashSet<string> value = keyValuePair.Value;
			list2.Clear();
			foreach (string id in value)
			{
				Sickness sickness = Db.Get().Sicknesses.TryGet(id);
				list2.Add(sickness.Name);
			}
			string newValue = string.Join(", ", list2.ToArray());
			string text = Strings.Get("STRINGS.DUPLICANTS.DISEASES.TRIGGERS." + Enum.GetName(typeof(GameHashes), keyValuePair.Key).ToUpper()).String;
			string text2 = Strings.Get("STRINGS.DUPLICANTS.DISEASES.TRIGGERS.TOOLTIPS." + Enum.GetName(typeof(GameHashes), keyValuePair.Key).ToUpper()).String;
			text = text.Replace("{ItemName}", properName).Replace("{Diseases}", newValue);
			text2 = text2.Replace("{ItemName}", properName).Replace("{Diseases}", newValue);
			list.Add(new Descriptor(text, text2, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x06008100 RID: 33024 RVA: 0x000F4DD0 File Offset: 0x000F2FD0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.EffectDescriptors(go);
	}

	// Token: 0x040061C8 RID: 25032
	public List<SicknessTrigger.TriggerInfo> triggers = new List<SicknessTrigger.TriggerInfo>();

	// Token: 0x02001862 RID: 6242
	// (Invoke) Token: 0x06008103 RID: 33027
	public delegate string SourceCallback(GameObject source, GameObject target);

	// Token: 0x02001863 RID: 6243
	[Serializable]
	public struct TriggerInfo
	{
		// Token: 0x040061C9 RID: 25033
		[HashedEnum]
		public GameHashes srcEvent;

		// Token: 0x040061CA RID: 25034
		public string[] sickness_ids;

		// Token: 0x040061CB RID: 25035
		public SicknessTrigger.SourceCallback sourceCallback;
	}
}
