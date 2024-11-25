using System.Collections.Generic;
using STRINGS;
using UnityEngine;

[SkipSaveFileSerialization, AddComponentMenu("KMonoBehaviour/scripts/MinimumOperatingTemperature")]
public class MinimumOperatingTemperature : KMonoBehaviour, ISim200ms, IGameObjectEffectDescriptor {
    private const float TURN_ON_DELAY = 5f;

    public static readonly Operational.Flag warmEnoughFlag
        = new Operational.Flag("warm_enough", Operational.Flag.Type.Functional);

    [MyCmpReq]
    private Building building;

    private bool  isWarm;
    private float lastOffTime;
    public  float minimumTemperature = 275.15f;

    [MyCmpReq]
    private Operational operational;

    private HandleVector<int>.Handle partitionerEntry;

    [MyCmpReq]
    private PrimaryElement primaryElement;

    public List<Descriptor> GetDescriptors(GameObject go) {
        var list = new List<Descriptor>();
        var item
            = new Descriptor(string.Format(UI.BUILDINGEFFECTS.MINIMUM_TEMP,
                                           GameUtil.GetFormattedTemperature(minimumTemperature)),
                             string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.MINIMUM_TEMP,
                                           GameUtil.GetFormattedTemperature(minimumTemperature)));

        list.Add(item);
        return list;
    }

    public void Sim200ms(float dt) { TestTemperature(false); }

    protected override void OnSpawn() {
        base.OnSpawn();
        TestTemperature(true);
    }

    private void TestTemperature(bool force) {
        bool flag;
        if (primaryElement.Temperature < minimumTemperature)
            flag = false;
        else {
            flag = true;
            for (var i = 0; i < building.PlacementCells.Length; i++) {
                var i2   = building.PlacementCells[i];
                var num  = Grid.Temperature[i2];
                var num2 = Grid.Mass[i2];
                if ((num != 0f || num2 != 0f) && num < minimumTemperature) {
                    flag = false;
                    break;
                }
            }
        }

        if (!flag) lastOffTime = Time.time;
        if ((flag != isWarm && !flag) || (flag != isWarm && flag && Time.time > lastOffTime + 5f) || force) {
            isWarm = flag;
            operational.SetFlag(warmEnoughFlag, isWarm);
            GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.TooCold, !isWarm, this);
        }
    }

    protected override void OnCleanUp() {
        base.OnCleanUp();
        GameScenePartitioner.Instance.Free(ref partitionerEntry);
    }
}