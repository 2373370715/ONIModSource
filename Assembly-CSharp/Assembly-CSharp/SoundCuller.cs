using UnityEngine;

public struct SoundCuller {
    public static bool IsAudibleWorld(Vector2 pos) {
        var result = false;
        var num = Grid.PosToCell(pos);
        if (Grid.IsValidCell(num) && Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId) result = true;
        return result;
    }

    public bool IsAudible(Vector2 pos) { return IsAudibleWorld(pos) && min.LessEqual(pos) && pos.LessEqual(max); }

    public bool IsAudibleNoCameraScaling(Vector2 pos, float falloff_distance_sq) {
        return (pos.x - cameraPos.x) * (pos.x - cameraPos.x) + (pos.y - cameraPos.y) * (pos.y - cameraPos.y) <
               falloff_distance_sq;
    }

    public bool IsAudible(Vector2 pos, float falloff_distance_sq) {
        if (!IsAudibleWorld(pos)) return false;

        pos = GetVerticallyScaledPosition(pos);
        return IsAudibleNoCameraScaling(pos, falloff_distance_sq);
    }

    public bool IsAudible(Vector2 pos, HashedString sound_path) {
        return sound_path.IsValid && IsAudible(pos, KFMOD.GetSoundEventDescription(sound_path).falloffDistanceSq);
    }

    public Vector3 GetVerticallyScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false) {
        var   num = 1f;
        float num2;
        if (pos.y > max.y)
            num2 = Mathf.Abs(pos.y - max.y);
        else if (pos.y < min.y) {
            num2 = Mathf.Abs(pos.y - min.y);
            num  = -1f;
        } else
            num2 = 0f;

        var extraYRange = TuningData<Tuning>.Get().extraYRange;
        num2 = num2 < extraYRange ? num2 : extraYRange;
        var num3 = num2 * num2 / (4f * zoomScaler);
        num3 *= num;
        var result                               = new Vector3(pos.x, pos.y + num3, 0f);
        if (objectIsSelectedAndVisible) result.z = pos.z;
        return result;
    }

    public static SoundCuller CreateCuller() {
        var result  = default(SoundCuller);
        var main    = Camera.main;
        var vector  = main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
        var vector2 = main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
        result.min       = new Vector3(vector2.x, vector2.y, 0f);
        result.max       = new Vector3(vector.x,  vector.y,  0f);
        result.cameraPos = main.transform.GetPosition();
        var audio = Audio.Get();
        var num   = CameraController.Instance.OrthographicSize / (audio.listenerReferenceZ - audio.listenerMinZ);
        if (num <= 0f)
            num = 2f;
        else
            num = 1f;

        result.zoomScaler = num;
        return result;
    }

    private Vector2 min;
    private Vector2 max;
    private Vector2 cameraPos;
    private float   zoomScaler;

    public class Tuning : TuningData<Tuning> {
        public float extraYRange;
    }
}