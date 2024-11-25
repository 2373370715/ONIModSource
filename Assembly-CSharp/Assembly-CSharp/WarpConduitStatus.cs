using UnityEngine;

public static class WarpConduitStatus {
    public static readonly Operational.Flag warpConnectedFlag
        = new Operational.Flag("warp_conduit_connected", Operational.Flag.Type.Requirement);

    public static void UpdateWarpConduitsOperational(GameObject sender, GameObject receiver) {
        object obj   = sender   != null && sender.GetComponent<Activatable>().IsActivated;
        var    flag  = receiver != null && receiver.GetComponent<Activatable>().IsActivated;
        var    obj2  = obj;
        bool   value = (obj2 & flag) != null;
        var    num   = 0;
        if (obj2 != null) num++;
        if (flag) num++;
        if (sender != null) {
            sender.GetComponent<Operational>().SetFlag(warpConnectedFlag, value);
            if (num != 2) {
                sender.GetComponent<KSelectable>()
                      .RemoveStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled);

                sender.GetComponent<KSelectable>()
                      .AddStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled, num);
            } else
                sender.GetComponent<KSelectable>()
                      .RemoveStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled);
        }

        if (receiver != null) {
            receiver.GetComponent<Operational>().SetFlag(warpConnectedFlag, value);
            if (num != 2) {
                receiver.GetComponent<KSelectable>()
                        .RemoveStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled);

                receiver.GetComponent<KSelectable>()
                        .AddStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled, num);

                return;
            }

            receiver.GetComponent<KSelectable>()
                    .RemoveStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled);
        }
    }
}