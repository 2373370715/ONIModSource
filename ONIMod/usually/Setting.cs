using System;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

[RestartRequired]
[JsonObject(MemberSerialization.OptIn)]
public class Setting {
    private static Setting _INSTANCE = new Setting();

    public static void Init(Setting settings) {
        if (settings != null) { _INSTANCE = settings; }
    }

    public static Setting Get() { return _INSTANCE; }
#if 孵化器mod
    [Option("产热", "heat production", Format = "F2"), JsonProperty]
    public float selfheart { get; set; } = 4f;

    [Option("功率", "power", Format = "F0"), JsonProperty]
    public float power { get; set; } = 240f;

    [Option("摇篮曲加速倍率", "acceleration ratio of egg song", Format = "F0"), JsonProperty]
    public float songSpeed { get; set; } = 4f;

    [Option("需要抱抱", "egg need hug"), JsonProperty]
    public bool need_hug { get; set; } = true;
#endif
}