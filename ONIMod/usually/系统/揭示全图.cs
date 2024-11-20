using HarmonyLib;

#if 揭示全图
// 点复制种子后等一会儿
[HarmonyPatch]
public class 揭示全图 {
    [HarmonyPatch(typeof(PauseScreen), "GetClipboardText"), HarmonyPostfix]
    public static void Postfix() {
        for (var i = 0; i < ClusterManager.Instance.activeWorld.WorldSize.X; i++)
            for (var j = 0; j < ClusterManager.Instance.activeWorld.WorldSize.Y; j++)
                Grid.Reveal(Grid.XYToCell(i + ClusterManager.Instance.activeWorld.WorldOffset.X,
                                          j + ClusterManager.Instance.activeWorld.WorldOffset.Y),
                            byte.MaxValue,
                            true);
    }
}
#endif