using System.Collections.Generic;



public class MyOverlayScreen {
    public static HashSet<Tag> LiquidPortIDs = new();
    public static HashSet<Tag> SolidPortDs   = new();
    public static HashSet<Tag> GasPortIDs    = new();
    public static HashSet<Tag> PowerPortIDs  = new();
    public static HashSet<Tag> LogicPortIDs  = new();

    private static readonly SampleLazy<HashSet<Tag>> _AllPortIDs = new(() => {
                                                                           var tags = new HashSet<Tag>();
                                                                           tags.UnionWith(LiquidPortIDs);
                                                                           tags.UnionWith(SolidPortDs);
                                                                           tags.UnionWith(GasPortIDs);
                                                                           tags.UnionWith(PowerPortIDs);
                                                                           tags.UnionWith(LogicPortIDs);
                                                                           return tags;
                                                                       });

    public static HashSet<Tag> AllPortIDs => _AllPortIDs.Value;
}