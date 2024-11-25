using System;
using System.Collections.Generic;

namespace RsTransferPort;

public class SingleChannelController : IComparable<SingleChannelController> {
    public List<TransferPortChannel> all       = new();
    public List<TransferPortChannel> receivers = new();
    public List<TransferPortChannel> senders   = new();

    public SingleChannelController(BuildingType buildingType, string channelName, int worldIdAG) {
        BuildingType = buildingType;
        ChannelName  = channelName;
        WorldIdAG    = worldIdAG;
        OnInit();
    }

    public BuildingType BuildingType { get; }
    public string       ChannelName  { get; }
    public int          Total        => all.Count;
    public int          WorldIdAG    { get; } // -1表示全球 
    public bool         IsGlobal     => WorldIdAG == -1;

    public string DisplayChannelName {
        get {
            if (IsInvalid()) return MYSTRINGS.UI.SIDESCREEN.RS_PORT_CHANNEL.CHANNEL_NULL;

            return ChannelName;
        }
    }

    public int CompareTo(SingleChannelController other) {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;

        return string.Compare(ChannelName, other.ChannelName, StringComparison.Ordinal);
    }

    public void Add(TransferPortChannel item) {
        if (item == null) {
            Debug.LogErrorFormat("SingleChannelController buildingType:{0} Add null", BuildingType);
            return;
        }

        if (item.InOutType == InOutType.Receiver)
            receivers.Add(item);
        else
            senders.Add(item);

        all.Add(item);
        OnAdd(item);
    }

    public void Remove(TransferPortChannel item) {
        if (item == null) return;

        if (item.InOutType == InOutType.Receiver)
            receivers.Remove(item);
        else
            senders.Remove(item);

        all.Remove(item);
        OnRemove(item);
    }

    protected virtual void OnAdd(TransferPortChannel    item) { }
    protected virtual void OnRemove(TransferPortChannel item) { }
    public virtual    bool Contains(TransferPortChannel item) { return all.Contains(item); }
    protected virtual void OnInit()                           { }
    public virtual    void OnSpawn()                          { }
    public virtual    void OnConUpdate()                      { }

    public virtual void OnCleanUp() {
        senders   = null;
        receivers = null;
        all       = null;
    }

    public bool IsInvalid() { return string.IsNullOrEmpty(ChannelName); }

    public ICollection<WorldContainer> GetIncludeWorldContainer() {
        var worldContainers = new HashSet<WorldContainer>();
        foreach (var channel in all) {
            var myWorld = channel.GetMyWorld();
            if (myWorld != null) worldContainers.Add(myWorld);
        }

        return worldContainers;
    }
}