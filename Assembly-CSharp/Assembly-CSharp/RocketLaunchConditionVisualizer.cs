using System.Collections.Generic;
using UnityEngine;

public class RocketLaunchConditionVisualizer : KMonoBehaviour {
    private RocketModuleCluster         clusterModule;
    private LaunchConditionManager      launchConditionManager;
    public  RocketModuleVisualizeData[] moduleVisualizeData;

    protected override void OnSpawn() {
        base.OnSpawn();
        if (DlcManager.FeatureClusterSpaceEnabled())
            clusterModule = GetComponent<RocketModuleCluster>();
        else
            launchConditionManager = GetComponent<LaunchConditionManager>();

        UpdateAllModuleData();
        Subscribe(1512695988, OnAnyRocketModuleChanged);
    }

    protected override void OnCleanUp()                          { Unsubscribe(1512695988, OnAnyRocketModuleChanged); }
    private            void OnAnyRocketModuleChanged(object obj) { UpdateAllModuleData(); }

    private void UpdateAllModuleData() {
        if (moduleVisualizeData != null) moduleVisualizeData = null;
        var                            flag                  = clusterModule != null;
        List<Ref<RocketModuleCluster>> list                  = null;
        List<RocketModule>             list2                 = null;
        if (flag) {
            list                = new List<Ref<RocketModuleCluster>>(clusterModule.CraftInterface.ClusterModules);
            moduleVisualizeData = new RocketModuleVisualizeData[list.Count];
            list.Sort(delegate(Ref<RocketModuleCluster> a, Ref<RocketModuleCluster> b) {
                          var y  = Grid.PosToXY(a.Get().transform.GetPosition()).y;
                          var y2 = Grid.PosToXY(b.Get().transform.GetPosition()).y;
                          return y.CompareTo(y2);
                      });
        } else {
            list2 = new List<RocketModule>(launchConditionManager.rocketModules);
            list2.Sort(delegate(RocketModule a, RocketModule b) {
                           var y  = Grid.PosToXY(a.transform.GetPosition()).y;
                           var y2 = Grid.PosToXY(b.transform.GetPosition()).y;
                           return y.CompareTo(y2);
                       });

            moduleVisualizeData = new RocketModuleVisualizeData[list2.Count];
        }

        for (var i = 0; i < moduleVisualizeData.Length; i++) {
            var rocketModule = flag ? list[i].Get() : list2[i];
            var component    = rocketModule.GetComponent<Building>();
            moduleVisualizeData[i] = new RocketModuleVisualizeData {
                Module   = rocketModule,
                RangeMax = Mathf.FloorToInt(component.Def.WidthInCells / 2f),
                RangeMin = -Mathf.FloorToInt((component.Def.WidthInCells - 1) / 2f)
            };
        }
    }

    public struct RocketModuleVisualizeData {
        public RocketModule Module;
        public Vector2I     OriginOffset;
        public int          RangeMin;
        public int          RangeMax;
    }
}