using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class AnimEventHandlerManager : KMonoBehaviour {
    private const float                   HIDE_DISTANCE = 40f;
    private       List<AnimEventHandler>  handlers;
    public static AnimEventHandlerManager Instance          { get; private set; }
    public static void                    DestroyInstance() { Instance = null; }

    protected override void OnPrefabInit() {
        Instance = this;
        handlers = new List<AnimEventHandler>();
    }

    public  void Add(AnimEventHandler    handler) { handlers.Add(handler); }
    public  void Remove(AnimEventHandler handler) { handlers.Remove(handler); }
    private bool IsVisibleToZoom() { return !(Game.MainCamera == null) && Game.MainCamera.orthographicSize < 40f; }

    public void LateUpdate() {
        if (!IsVisibleToZoom()) return;

        AnimEventHandlerManager.<>c__DisplayClass11_0 CS$<>8__locals1;
        Grid.GetVisibleCellRangeInActiveWorld(out CS$ <>8__locals1.min, out CS$<>8__locals1.max, 4, 1.5f);
        foreach (var animEventHandler in handlers) {
            if (AnimEventHandlerManager.<(LateUpdate > g__IsVisible) | 11_0(animEventHandler, ref CS$ <>8__locals1))
            {
                animEventHandler.UpdateOffset();
            }
        }
    }

    [CompilerGenerated]
    internal static bool<LateUpdate>g__IsVisible|11_0<>
    private c__DisplayClass11_0 A_1)
    {
        int num;
        int num2;
        Grid.CellToXY(handler.GetCachedCell(), out num, out num2);
        return num >= A_1.min.x && num2 >= A_1.min.y && num < A_1.max.x && num2 < A_1.max.y;
    }
}