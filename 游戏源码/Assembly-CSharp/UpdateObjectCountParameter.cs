using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000936 RID: 2358
internal class UpdateObjectCountParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06002AAD RID: 10925 RVA: 0x001DB740 File Offset: 0x001D9940
	public static UpdateObjectCountParameter.Settings GetSettings(HashedString path_hash, SoundDescription description)
	{
		UpdateObjectCountParameter.Settings settings = default(UpdateObjectCountParameter.Settings);
		if (!UpdateObjectCountParameter.settings.TryGetValue(path_hash, out settings))
		{
			settings = default(UpdateObjectCountParameter.Settings);
			EventDescription eventDescription = RuntimeManager.GetEventDescription(description.path);
			USER_PROPERTY user_PROPERTY;
			if (eventDescription.getUserProperty("minObj", out user_PROPERTY) == RESULT.OK)
			{
				settings.minObjects = (float)((short)user_PROPERTY.floatValue());
			}
			else
			{
				settings.minObjects = 1f;
			}
			USER_PROPERTY user_PROPERTY2;
			if (eventDescription.getUserProperty("maxObj", out user_PROPERTY2) == RESULT.OK)
			{
				settings.maxObjects = user_PROPERTY2.floatValue();
			}
			else
			{
				settings.maxObjects = 0f;
			}
			USER_PROPERTY user_PROPERTY3;
			if (eventDescription.getUserProperty("curveType", out user_PROPERTY3) == RESULT.OK && user_PROPERTY3.stringValue() == "exp")
			{
				settings.useExponentialCurve = true;
			}
			settings.parameterId = description.GetParameterId(UpdateObjectCountParameter.parameterHash);
			settings.path = path_hash;
			UpdateObjectCountParameter.settings[path_hash] = settings;
		}
		return settings;
	}

	// Token: 0x06002AAE RID: 10926 RVA: 0x001DB828 File Offset: 0x001D9A28
	public static void ApplySettings(EventInstance ev, int count, UpdateObjectCountParameter.Settings settings)
	{
		float num = 0f;
		if (settings.maxObjects != settings.minObjects)
		{
			num = ((float)count - settings.minObjects) / (settings.maxObjects - settings.minObjects);
			num = Mathf.Clamp01(num);
		}
		if (settings.useExponentialCurve)
		{
			num *= num;
		}
		ev.setParameterByID(settings.parameterId, num, false);
	}

	// Token: 0x06002AAF RID: 10927 RVA: 0x000BBB84 File Offset: 0x000B9D84
	public UpdateObjectCountParameter() : base("objectCount")
	{
	}

	// Token: 0x06002AB0 RID: 10928 RVA: 0x001DB884 File Offset: 0x001D9A84
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateObjectCountParameter.Settings settings = UpdateObjectCountParameter.GetSettings(sound.path, sound.description);
		UpdateObjectCountParameter.Entry item = new UpdateObjectCountParameter.Entry
		{
			ev = sound.ev,
			settings = settings
		};
		this.entries.Add(item);
	}

	// Token: 0x06002AB1 RID: 10929 RVA: 0x001DB8D0 File Offset: 0x001D9AD0
	public override void Update(float dt)
	{
		DictionaryPool<HashedString, int, LoopingSoundManager>.PooledDictionary pooledDictionary = DictionaryPool<HashedString, int, LoopingSoundManager>.Allocate();
		foreach (UpdateObjectCountParameter.Entry entry in this.entries)
		{
			int num = 0;
			pooledDictionary.TryGetValue(entry.settings.path, out num);
			num = (pooledDictionary[entry.settings.path] = num + 1);
		}
		foreach (UpdateObjectCountParameter.Entry entry2 in this.entries)
		{
			int count = pooledDictionary[entry2.settings.path];
			UpdateObjectCountParameter.ApplySettings(entry2.ev, count, entry2.settings);
		}
		pooledDictionary.Recycle();
	}

	// Token: 0x06002AB2 RID: 10930 RVA: 0x001DB9BC File Offset: 0x001D9BBC
	public override void Remove(LoopingSoundParameterUpdater.Sound sound)
	{
		for (int i = 0; i < this.entries.Count; i++)
		{
			if (this.entries[i].ev.handle == sound.ev.handle)
			{
				this.entries.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06002AB3 RID: 10931 RVA: 0x000BBBA1 File Offset: 0x000B9DA1
	public static void Clear()
	{
		UpdateObjectCountParameter.settings.Clear();
	}

	// Token: 0x04001C5D RID: 7261
	private List<UpdateObjectCountParameter.Entry> entries = new List<UpdateObjectCountParameter.Entry>();

	// Token: 0x04001C5E RID: 7262
	private static Dictionary<HashedString, UpdateObjectCountParameter.Settings> settings = new Dictionary<HashedString, UpdateObjectCountParameter.Settings>();

	// Token: 0x04001C5F RID: 7263
	private static readonly HashedString parameterHash = "objectCount";

	// Token: 0x02000937 RID: 2359
	private struct Entry
	{
		// Token: 0x04001C60 RID: 7264
		public EventInstance ev;

		// Token: 0x04001C61 RID: 7265
		public UpdateObjectCountParameter.Settings settings;
	}

	// Token: 0x02000938 RID: 2360
	public struct Settings
	{
		// Token: 0x04001C62 RID: 7266
		public HashedString path;

		// Token: 0x04001C63 RID: 7267
		public PARAMETER_ID parameterId;

		// Token: 0x04001C64 RID: 7268
		public float minObjects;

		// Token: 0x04001C65 RID: 7269
		public float maxObjects;

		// Token: 0x04001C66 RID: 7270
		public bool useExponentialCurve;
	}
}
