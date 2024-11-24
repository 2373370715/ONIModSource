using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000907 RID: 2311
public class AnimEventManager : Singleton<AnimEventManager>
{
	// Token: 0x0600290F RID: 10511 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void FreeResources()
	{
	}

	// Token: 0x06002910 RID: 10512 RVA: 0x001D4620 File Offset: 0x001D2820
	public HandleVector<int>.Handle PlayAnim(KAnimControllerBase controller, KAnim.Anim anim, KAnim.PlayMode mode, float time, bool use_unscaled_time)
	{
		AnimEventManager.AnimData animData = default(AnimEventManager.AnimData);
		animData.frameRate = anim.frameRate;
		animData.totalTime = anim.totalTime;
		animData.numFrames = anim.numFrames;
		animData.useUnscaledTime = use_unscaled_time;
		AnimEventManager.EventPlayerData eventPlayerData = default(AnimEventManager.EventPlayerData);
		eventPlayerData.elapsedTime = time;
		eventPlayerData.mode = mode;
		eventPlayerData.controller = (controller as KBatchedAnimController);
		eventPlayerData.currentFrame = eventPlayerData.controller.GetFrameIdx(eventPlayerData.elapsedTime, false);
		eventPlayerData.previousFrame = -1;
		eventPlayerData.events = null;
		eventPlayerData.updatingEvents = null;
		eventPlayerData.events = GameAudioSheets.Get().GetEvents(anim.id);
		if (eventPlayerData.events == null)
		{
			eventPlayerData.events = AnimEventManager.emptyEventList;
		}
		HandleVector<int>.Handle result;
		if (animData.useUnscaledTime)
		{
			HandleVector<int>.Handle anim_data_handle = this.uiAnimData.Allocate(animData);
			HandleVector<int>.Handle event_data_handle = this.uiEventData.Allocate(eventPlayerData);
			result = this.indirectionData.Allocate(new AnimEventManager.IndirectionData(anim_data_handle, event_data_handle, true));
		}
		else
		{
			HandleVector<int>.Handle anim_data_handle2 = this.animData.Allocate(animData);
			HandleVector<int>.Handle event_data_handle2 = this.eventData.Allocate(eventPlayerData);
			result = this.indirectionData.Allocate(new AnimEventManager.IndirectionData(anim_data_handle2, event_data_handle2, false));
		}
		return result;
	}

	// Token: 0x06002911 RID: 10513 RVA: 0x001D4754 File Offset: 0x001D2954
	public void SetMode(HandleVector<int>.Handle handle, KAnim.PlayMode mode)
	{
		if (!handle.IsValid())
		{
			return;
		}
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector = data.isUIData ? this.uiEventData : this.eventData;
		AnimEventManager.EventPlayerData data2 = kcompactedVector.GetData(data.eventDataHandle);
		data2.mode = mode;
		kcompactedVector.SetData(data.eventDataHandle, data2);
	}

	// Token: 0x06002912 RID: 10514 RVA: 0x001D47B0 File Offset: 0x001D29B0
	public void StopAnim(HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return;
		}
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		KCompactedVector<AnimEventManager.AnimData> kcompactedVector = data.isUIData ? this.uiAnimData : this.animData;
		KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector2 = data.isUIData ? this.uiEventData : this.eventData;
		AnimEventManager.EventPlayerData data2 = kcompactedVector2.GetData(data.eventDataHandle);
		this.StopEvents(data2);
		kcompactedVector.Free(data.animDataHandle);
		kcompactedVector2.Free(data.eventDataHandle);
		this.indirectionData.Free(handle);
	}

	// Token: 0x06002913 RID: 10515 RVA: 0x001D483C File Offset: 0x001D2A3C
	public float GetElapsedTime(HandleVector<int>.Handle handle)
	{
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		return (data.isUIData ? this.uiEventData : this.eventData).GetData(data.eventDataHandle).elapsedTime;
	}

	// Token: 0x06002914 RID: 10516 RVA: 0x001D487C File Offset: 0x001D2A7C
	public void SetElapsedTime(HandleVector<int>.Handle handle, float elapsed_time)
	{
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector = data.isUIData ? this.uiEventData : this.eventData;
		AnimEventManager.EventPlayerData data2 = kcompactedVector.GetData(data.eventDataHandle);
		data2.elapsedTime = elapsed_time;
		kcompactedVector.SetData(data.eventDataHandle, data2);
	}

	// Token: 0x06002915 RID: 10517 RVA: 0x001D48D0 File Offset: 0x001D2AD0
	public void Update()
	{
		float deltaTime = Time.deltaTime;
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		this.Update(deltaTime, this.animData.GetDataList(), this.eventData.GetDataList());
		this.Update(unscaledDeltaTime, this.uiAnimData.GetDataList(), this.uiEventData.GetDataList());
		for (int i = 0; i < this.finishedCalls.Count; i++)
		{
			this.finishedCalls[i].TriggerStop();
		}
		this.finishedCalls.Clear();
	}

	// Token: 0x06002916 RID: 10518 RVA: 0x001D4958 File Offset: 0x001D2B58
	private void Update(float dt, List<AnimEventManager.AnimData> anim_data, List<AnimEventManager.EventPlayerData> event_data)
	{
		if (dt <= 0f)
		{
			return;
		}
		for (int i = 0; i < event_data.Count; i++)
		{
			AnimEventManager.EventPlayerData eventPlayerData = event_data[i];
			if (!(eventPlayerData.controller == null) && eventPlayerData.mode != KAnim.PlayMode.Paused)
			{
				eventPlayerData.currentFrame = eventPlayerData.controller.GetFrameIdx(eventPlayerData.elapsedTime, false);
				event_data[i] = eventPlayerData;
				this.PlayEvents(eventPlayerData);
				eventPlayerData.previousFrame = eventPlayerData.currentFrame;
				eventPlayerData.elapsedTime += dt * eventPlayerData.controller.GetPlaySpeed();
				event_data[i] = eventPlayerData;
				if (eventPlayerData.updatingEvents != null)
				{
					for (int j = 0; j < eventPlayerData.updatingEvents.Count; j++)
					{
						eventPlayerData.updatingEvents[j].OnUpdate(eventPlayerData);
					}
				}
				event_data[i] = eventPlayerData;
				if (eventPlayerData.mode != KAnim.PlayMode.Loop && eventPlayerData.currentFrame >= anim_data[i].numFrames - 1)
				{
					this.StopEvents(eventPlayerData);
					this.finishedCalls.Add(eventPlayerData.controller);
				}
			}
		}
	}

	// Token: 0x06002917 RID: 10519 RVA: 0x001D4A70 File Offset: 0x001D2C70
	private void PlayEvents(AnimEventManager.EventPlayerData data)
	{
		for (int i = 0; i < data.events.Count; i++)
		{
			data.events[i].Play(data);
		}
	}

	// Token: 0x06002918 RID: 10520 RVA: 0x001D4AA8 File Offset: 0x001D2CA8
	private void StopEvents(AnimEventManager.EventPlayerData data)
	{
		for (int i = 0; i < data.events.Count; i++)
		{
			data.events[i].Stop(data);
		}
		if (data.updatingEvents != null)
		{
			data.updatingEvents.Clear();
		}
	}

	// Token: 0x06002919 RID: 10521 RVA: 0x000BAADB File Offset: 0x000B8CDB
	public AnimEventManager.DevTools_DebugInfo DevTools_GetDebugInfo()
	{
		return new AnimEventManager.DevTools_DebugInfo(this, this.animData, this.eventData, this.uiAnimData, this.uiEventData);
	}

	// Token: 0x04001B5D RID: 7005
	private static readonly List<AnimEvent> emptyEventList = new List<AnimEvent>();

	// Token: 0x04001B5E RID: 7006
	private const int INITIAL_VECTOR_SIZE = 256;

	// Token: 0x04001B5F RID: 7007
	private KCompactedVector<AnimEventManager.AnimData> animData = new KCompactedVector<AnimEventManager.AnimData>(256);

	// Token: 0x04001B60 RID: 7008
	private KCompactedVector<AnimEventManager.EventPlayerData> eventData = new KCompactedVector<AnimEventManager.EventPlayerData>(256);

	// Token: 0x04001B61 RID: 7009
	private KCompactedVector<AnimEventManager.AnimData> uiAnimData = new KCompactedVector<AnimEventManager.AnimData>(256);

	// Token: 0x04001B62 RID: 7010
	private KCompactedVector<AnimEventManager.EventPlayerData> uiEventData = new KCompactedVector<AnimEventManager.EventPlayerData>(256);

	// Token: 0x04001B63 RID: 7011
	private KCompactedVector<AnimEventManager.IndirectionData> indirectionData = new KCompactedVector<AnimEventManager.IndirectionData>(0);

	// Token: 0x04001B64 RID: 7012
	private List<KBatchedAnimController> finishedCalls = new List<KBatchedAnimController>();

	// Token: 0x02000908 RID: 2312
	public struct AnimData
	{
		// Token: 0x04001B65 RID: 7013
		public float frameRate;

		// Token: 0x04001B66 RID: 7014
		public float totalTime;

		// Token: 0x04001B67 RID: 7015
		public int numFrames;

		// Token: 0x04001B68 RID: 7016
		public bool useUnscaledTime;
	}

	// Token: 0x02000909 RID: 2313
	[DebuggerDisplay("{controller.name}, Anim={currentAnim}, Frame={currentFrame}, Mode={mode}")]
	public struct EventPlayerData
	{
		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600291C RID: 10524 RVA: 0x000BAB07 File Offset: 0x000B8D07
		// (set) Token: 0x0600291D RID: 10525 RVA: 0x000BAB0F File Offset: 0x000B8D0F
		public int currentFrame { readonly get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600291E RID: 10526 RVA: 0x000BAB18 File Offset: 0x000B8D18
		// (set) Token: 0x0600291F RID: 10527 RVA: 0x000BAB20 File Offset: 0x000B8D20
		public int previousFrame { readonly get; set; }

		// Token: 0x06002920 RID: 10528 RVA: 0x000BAB29 File Offset: 0x000B8D29
		public ComponentType GetComponent<ComponentType>()
		{
			return this.controller.GetComponent<ComponentType>();
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06002921 RID: 10529 RVA: 0x000BAB36 File Offset: 0x000B8D36
		public string name
		{
			get
			{
				return this.controller.name;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06002922 RID: 10530 RVA: 0x000BAB43 File Offset: 0x000B8D43
		public float normalizedTime
		{
			get
			{
				return this.elapsedTime / this.controller.CurrentAnim.totalTime;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06002923 RID: 10531 RVA: 0x000BAB5C File Offset: 0x000B8D5C
		public Vector3 position
		{
			get
			{
				return this.controller.transform.GetPosition();
			}
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x000BAB6E File Offset: 0x000B8D6E
		public void AddUpdatingEvent(AnimEvent ev)
		{
			if (this.updatingEvents == null)
			{
				this.updatingEvents = new List<AnimEvent>();
			}
			this.updatingEvents.Add(ev);
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x000BAB8F File Offset: 0x000B8D8F
		public void SetElapsedTime(float elapsedTime)
		{
			this.elapsedTime = elapsedTime;
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x000BAB98 File Offset: 0x000B8D98
		public void FreeResources()
		{
			this.elapsedTime = 0f;
			this.mode = KAnim.PlayMode.Once;
			this.currentFrame = 0;
			this.previousFrame = 0;
			this.events = null;
			this.updatingEvents = null;
			this.controller = null;
		}

		// Token: 0x04001B69 RID: 7017
		public float elapsedTime;

		// Token: 0x04001B6A RID: 7018
		public KAnim.PlayMode mode;

		// Token: 0x04001B6D RID: 7021
		public List<AnimEvent> events;

		// Token: 0x04001B6E RID: 7022
		public List<AnimEvent> updatingEvents;

		// Token: 0x04001B6F RID: 7023
		public KBatchedAnimController controller;
	}

	// Token: 0x0200090A RID: 2314
	private struct IndirectionData
	{
		// Token: 0x06002927 RID: 10535 RVA: 0x000BABCF File Offset: 0x000B8DCF
		public IndirectionData(HandleVector<int>.Handle anim_data_handle, HandleVector<int>.Handle event_data_handle, bool is_ui_data)
		{
			this.isUIData = is_ui_data;
			this.animDataHandle = anim_data_handle;
			this.eventDataHandle = event_data_handle;
		}

		// Token: 0x04001B70 RID: 7024
		public bool isUIData;

		// Token: 0x04001B71 RID: 7025
		public HandleVector<int>.Handle animDataHandle;

		// Token: 0x04001B72 RID: 7026
		public HandleVector<int>.Handle eventDataHandle;
	}

	// Token: 0x0200090B RID: 2315
	public readonly struct DevTools_DebugInfo
	{
		// Token: 0x06002928 RID: 10536 RVA: 0x000BABE6 File Offset: 0x000B8DE6
		public DevTools_DebugInfo(AnimEventManager eventManager, KCompactedVector<AnimEventManager.AnimData> animData, KCompactedVector<AnimEventManager.EventPlayerData> eventData, KCompactedVector<AnimEventManager.AnimData> uiAnimData, KCompactedVector<AnimEventManager.EventPlayerData> uiEventData)
		{
			this.eventManager = eventManager;
			this.animData = animData;
			this.eventData = eventData;
			this.uiAnimData = uiAnimData;
			this.uiEventData = uiEventData;
		}

		// Token: 0x04001B73 RID: 7027
		public readonly AnimEventManager eventManager;

		// Token: 0x04001B74 RID: 7028
		public readonly KCompactedVector<AnimEventManager.AnimData> animData;

		// Token: 0x04001B75 RID: 7029
		public readonly KCompactedVector<AnimEventManager.EventPlayerData> eventData;

		// Token: 0x04001B76 RID: 7030
		public readonly KCompactedVector<AnimEventManager.AnimData> uiAnimData;

		// Token: 0x04001B77 RID: 7031
		public readonly KCompactedVector<AnimEventManager.EventPlayerData> uiEventData;
	}
}
