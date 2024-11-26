using System.Collections.Generic;
using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/OrbitalObject"), SerializationConfig(MemberSerialization.OptIn)]
public class OrbitalObject : KMonoBehaviour, IRenderEveryTick {
    [Serialize]
    private float angle;

    private KBatchedAnimController animController;

    [Serialize]
    private string animFilename;

    [Serialize]
    private string initialAnim;

    [Serialize]
    public string orbitalDBId;

    private OrbitalData orbitData;

    [Serialize]
    private int orbitingWorldId;

    [Serialize]
    public int timeoffset;

    private WorldContainer world;

    [Serialize]
    private Vector3 worldOrbitingOrigin;

    public void RenderEveryTick(float dt) {
        var  time = GameClock.Instance.GetTime();
        bool flag;
        var  vector  = CalculateWorldPos(time, out flag);
        var  vector2 = vector;
        if (orbitData.periodInCycles > 0f) {
            vector2.x = vector.x / Grid.WidthInCells;
            vector2.y = vector.y / Grid.HeightInCells;
            vector2.x = Camera.main.ViewportToWorldPoint(vector2).x;
            vector2.y = Camera.main.ViewportToWorldPoint(vector2).y;
        }

        var flag2 = (!orbitData.rotatesBehind || !flag) &&
                    (world == null            || ClusterManager.Instance.activeWorldId == world.id);

        var offset = vector2 - gameObject.transform.position;
        offset.z              = 0f;
        animController.Offset = offset;
        var position = vector2;
        position.x = worldOrbitingOrigin.x;
        position.y = worldOrbitingOrigin.y;
        gameObject.transform.SetPosition(position);
        if (orbitData.periodInCycles > 0f)
            gameObject.transform.localScale
                = Vector3.one * (CameraController.Instance.baseCamera.orthographicSize / orbitData.distance);
        else
            gameObject.transform.localScale = Vector3.one * orbitData.distance;

        if (gameObject.activeSelf != flag2) gameObject.SetActive(flag2);
    }

    public void Init(string orbit_data_name, WorldContainer orbiting_world, List<Ref<OrbitalObject>> orbiting_obj) {
        var orbitalData = Db.Get().OrbitalTypeCategories.Get(orbit_data_name);
        if (orbiting_world != null) {
            orbitingWorldId     = orbiting_world.id;
            world               = orbiting_world;
            worldOrbitingOrigin = GetWorldOrigin(world, orbitalData);
        } else
            worldOrbitingOrigin
                = new Vector3(Grid.WidthInCells * 0.5f, Grid.HeightInCells * orbitalData.yGridPercent, 0f);

        animFilename = orbitalData.animFile;
        initialAnim  = GetInitialAnim(orbitalData);
        angle        = GetAngle(orbitalData);
        timeoffset   = GetTimeOffset(orbiting_obj);
        orbitalDBId  = orbitalData.Id;
    }

    protected override void OnSpawn() {
        world     = ClusterManager.Instance.GetWorld(orbitingWorldId);
        orbitData = Db.Get().OrbitalTypeCategories.Get(orbitalDBId);
        gameObject.SetActive(false);
        var kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
        kbatchedAnimController.isMovable      = true;
        kbatchedAnimController.initialAnim    = initialAnim;
        kbatchedAnimController.AnimFiles      = new[] { Assets.GetAnim(animFilename) };
        kbatchedAnimController.initialMode    = KAnim.PlayMode.Loop;
        kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.Always;
        animController                        = kbatchedAnimController;
    }

    private Vector3 CalculateWorldPos(float time, out bool behind) {
        Vector3 result;
        if (orbitData.periodInCycles > 0f) {
            var num    = orbitData.periodInCycles * 600f;
            var f      = ((time + timeoffset) / num - (int)((time + timeoffset) / num)) * 2f * 3.1415927f;
            var d      = 0.5f * orbitData.radiusScale * world.WorldSize.x;
            var vector = new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f));
            behind = vector.z > orbitData.behindZ;
            var b = Quaternion.Euler(angle, 0f, 0f) * (vector * d);
            result   = worldOrbitingOrigin + b;
            result.z = orbitData.GetRenderZ == null ? orbitData.renderZ : orbitData.GetRenderZ();
        } else {
            behind   = false;
            result   = worldOrbitingOrigin;
            result.z = orbitData.GetRenderZ == null ? orbitData.renderZ : orbitData.GetRenderZ();
        }

        return result;
    }

    private string GetInitialAnim(OrbitalData data) {
        if (data.initialAnim.IsNullOrWhiteSpace()) {
            var data2 = Assets.GetAnim(data.animFile).GetData();
            var index = new KRandom().Next(0, data2.animCount - 1);
            return data2.GetAnim(index).name;
        }

        return data.initialAnim;
    }

    private Vector3 GetWorldOrigin(WorldContainer wc, OrbitalData data) {
        if (wc != null) {
            var x = wc.WorldOffset.x + wc.WorldSize.x * data.xGridPercent;
            var y = wc.WorldOffset.y + wc.WorldSize.y * data.yGridPercent;
            return new Vector3(x, y, 0f);
        }

        return new Vector3(Grid.WidthInCells * data.xGridPercent, Grid.HeightInCells * data.yGridPercent, 0f);
    }

    private float GetAngle(OrbitalData data) { return Random.Range(data.minAngle, data.maxAngle); }

    private int GetTimeOffset(List<Ref<OrbitalObject>> orbiting_obj) {
        var list = new List<int>();
        foreach (var @ref in orbiting_obj)
            if (@ref.Get().world == world)
                list.Add(@ref.Get().timeoffset);

        var num                        = Random.Range(0, 600);
        while (list.Contains(num)) num = Random.Range(0, 600);
        return num;
    }
}