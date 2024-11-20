using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AnimEventManager : Singleton<AnimEventManager> {
    private const           int                               INITIAL_VECTOR_SIZE = 256;
    private static readonly List<AnimEvent>                   emptyEventList = new List<AnimEvent>();
    private readonly        KCompactedVector<AnimData>        animData = new KCompactedVector<AnimData>(256);
    private readonly        KCompactedVector<EventPlayerData> eventData = new KCompactedVector<EventPlayerData>(256);
    private readonly        List<KBatchedAnimController>      finishedCalls = new List<KBatchedAnimController>();
    private readonly        KCompactedVector<IndirectionData> indirectionData = new KCompactedVector<IndirectionData>();
    private readonly        KCompactedVector<AnimData>        uiAnimData = new KCompactedVector<AnimData>(256);
    private readonly        KCompactedVector<EventPlayerData> uiEventData = new KCompactedVector<EventPlayerData>(256);
    public                  void                              FreeResources() { }

    public HandleVector<int>.Handle PlayAnim(KAnimControllerBase controller,
                                             KAnim.Anim          anim,
                                             KAnim.PlayMode      mode,
                                             float               time,
                                             bool                use_unscaled_time) {
        var animData = default(AnimData);
        animData.frameRate       = anim.frameRate;
        animData.totalTime       = anim.totalTime;
        animData.numFrames       = anim.numFrames;
        animData.useUnscaledTime = use_unscaled_time;
        var eventPlayerData = default(EventPlayerData);
        eventPlayerData.elapsedTime    = time;
        eventPlayerData.mode           = mode;
        eventPlayerData.controller     = controller as KBatchedAnimController;
        eventPlayerData.currentFrame   = eventPlayerData.controller.GetFrameIdx(eventPlayerData.elapsedTime, false);
        eventPlayerData.previousFrame  = -1;
        eventPlayerData.events         = null;
        eventPlayerData.updatingEvents = null;
        eventPlayerData.events         = GameAudioSheets.Get().GetEvents(anim.id);
        if (eventPlayerData.events == null) eventPlayerData.events = emptyEventList;
        HandleVector<int>.Handle result;
        if (animData.useUnscaledTime) {
            var anim_data_handle  = uiAnimData.Allocate(animData);
            var event_data_handle = uiEventData.Allocate(eventPlayerData);
            result = indirectionData.Allocate(new IndirectionData(anim_data_handle, event_data_handle, true));
        } else {
            var anim_data_handle2  = this.animData.Allocate(animData);
            var event_data_handle2 = eventData.Allocate(eventPlayerData);
            result = indirectionData.Allocate(new IndirectionData(anim_data_handle2, event_data_handle2, false));
        }

        return result;
    }

    public void SetMode(HandleVector<int>.Handle handle, KAnim.PlayMode mode) {
        if (!handle.IsValid()) return;

        var data             = indirectionData.GetData(handle);
        var kcompactedVector = data.isUIData ? uiEventData : eventData;
        var data2            = kcompactedVector.GetData(data.eventDataHandle);
        data2.mode = mode;
        kcompactedVector.SetData(data.eventDataHandle, data2);
    }

    public void StopAnim(HandleVector<int>.Handle handle) {
        if (!handle.IsValid()) return;

        var data              = indirectionData.GetData(handle);
        var kcompactedVector  = data.isUIData ? uiAnimData : animData;
        var kcompactedVector2 = data.isUIData ? uiEventData : eventData;
        var data2             = kcompactedVector2.GetData(data.eventDataHandle);
        StopEvents(data2);
        kcompactedVector.Free(data.animDataHandle);
        kcompactedVector2.Free(data.eventDataHandle);
        indirectionData.Free(handle);
    }

    public float GetElapsedTime(HandleVector<int>.Handle handle) {
        var data = indirectionData.GetData(handle);
        return (data.isUIData ? uiEventData : eventData).GetData(data.eventDataHandle).elapsedTime;
    }

    public void SetElapsedTime(HandleVector<int>.Handle handle, float elapsed_time) {
        var data             = indirectionData.GetData(handle);
        var kcompactedVector = data.isUIData ? uiEventData : eventData;
        var data2            = kcompactedVector.GetData(data.eventDataHandle);
        data2.elapsedTime = elapsed_time;
        kcompactedVector.SetData(data.eventDataHandle, data2);
    }

    public void Update() {
        var deltaTime         = Time.deltaTime;
        var unscaledDeltaTime = Time.unscaledDeltaTime;
        Update(deltaTime,         animData.GetDataList(),   eventData.GetDataList());
        Update(unscaledDeltaTime, uiAnimData.GetDataList(), uiEventData.GetDataList());
        for (var i = 0; i < finishedCalls.Count; i++) finishedCalls[i].TriggerStop();
        finishedCalls.Clear();
    }

    private void Update(float dt, List<AnimData> anim_data, List<EventPlayerData> event_data) {
        if (dt <= 0f) return;

        for (var i = 0; i < event_data.Count; i++) {
            var eventPlayerData = event_data[i];
            if (!(eventPlayerData.controller == null) && eventPlayerData.mode != KAnim.PlayMode.Paused) {
                eventPlayerData.currentFrame
                    = eventPlayerData.controller.GetFrameIdx(eventPlayerData.elapsedTime, false);

                event_data[i] = eventPlayerData;
                PlayEvents(eventPlayerData);
                eventPlayerData.previousFrame =  eventPlayerData.currentFrame;
                eventPlayerData.elapsedTime   += dt * eventPlayerData.controller.GetPlaySpeed();
                event_data[i]                 =  eventPlayerData;
                if (eventPlayerData.updatingEvents != null)
                    for (var j = 0; j < eventPlayerData.updatingEvents.Count; j++)
                        eventPlayerData.updatingEvents[j].OnUpdate(eventPlayerData);

                event_data[i] = eventPlayerData;
                if (eventPlayerData.mode         != KAnim.PlayMode.Loop &&
                    eventPlayerData.currentFrame >= anim_data[i].numFrames - 1) {
                    StopEvents(eventPlayerData);
                    finishedCalls.Add(eventPlayerData.controller);
                }
            }
        }
    }

    private void PlayEvents(EventPlayerData data) {
        for (var i = 0; i < data.events.Count; i++) data.events[i].Play(data);
    }

    private void StopEvents(EventPlayerData data) {
        for (var i = 0; i       < data.events.Count; i++) data.events[i].Stop(data);
        if (data.updatingEvents != null) data.updatingEvents.Clear();
    }

    public DevTools_DebugInfo DevTools_GetDebugInfo() {
        return new DevTools_DebugInfo(this, animData, eventData, uiAnimData, uiEventData);
    }

    public struct AnimData {
        public float frameRate;
        public float totalTime;
        public int   numFrames;
        public bool  useUnscaledTime;
    }

    [DebuggerDisplay("{controller.name}, Anim={currentAnim}, Frame={currentFrame}, Mode={mode}")]
    public struct EventPlayerData {
        public int           currentFrame                  { get; set; }
        public int           previousFrame                 { get; set; }
        public ComponentType GetComponent<ComponentType>() { return controller.GetComponent<ComponentType>(); }
        public string        name                          => controller.name;
        public float         normalizedTime                => elapsedTime / controller.CurrentAnim.totalTime;
        public Vector3       position                      => controller.transform.GetPosition();

        public void AddUpdatingEvent(AnimEvent ev) {
            if (updatingEvents == null) updatingEvents = new List<AnimEvent>();
            updatingEvents.Add(ev);
        }

        public void SetElapsedTime(float elapsedTime) { this.elapsedTime = elapsedTime; }

        public void FreeResources() {
            elapsedTime    = 0f;
            mode           = KAnim.PlayMode.Once;
            currentFrame   = 0;
            previousFrame  = 0;
            events         = null;
            updatingEvents = null;
            controller     = null;
        }

        public float                  elapsedTime;
        public KAnim.PlayMode         mode;
        public List<AnimEvent>        events;
        public List<AnimEvent>        updatingEvents;
        public KBatchedAnimController controller;
    }

    private struct IndirectionData {
        public IndirectionData(HandleVector<int>.Handle anim_data_handle,
                               HandleVector<int>.Handle event_data_handle,
                               bool                     is_ui_data) {
            isUIData        = is_ui_data;
            animDataHandle  = anim_data_handle;
            eventDataHandle = event_data_handle;
        }

        public readonly bool                     isUIData;
        public readonly HandleVector<int>.Handle animDataHandle;
        public readonly HandleVector<int>.Handle eventDataHandle;
    }

    public readonly struct DevTools_DebugInfo {
        public DevTools_DebugInfo(AnimEventManager                  eventManager,
                                  KCompactedVector<AnimData>        animData,
                                  KCompactedVector<EventPlayerData> eventData,
                                  KCompactedVector<AnimData>        uiAnimData,
                                  KCompactedVector<EventPlayerData> uiEventData) {
            this.eventManager = eventManager;
            this.animData     = animData;
            this.eventData    = eventData;
            this.uiAnimData   = uiAnimData;
            this.uiEventData  = uiEventData;
        }

        public readonly AnimEventManager                  eventManager;
        public readonly KCompactedVector<AnimData>        animData;
        public readonly KCompactedVector<EventPlayerData> eventData;
        public readonly KCompactedVector<AnimData>        uiAnimData;
        public readonly KCompactedVector<EventPlayerData> uiEventData;
    }
}