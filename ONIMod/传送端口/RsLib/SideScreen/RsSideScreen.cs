using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RsLib;

public class RsSideScreen : RsModule<RsSideScreen> {
    protected List<ItemInfo> itemInfos = new();

    public RsSideScreen Create<TScreen>(DetailsScreen.SidescreenTabTypes tab) where TScreen : SideScreenContent {
        var itemInfo = new ItemInfo();
        itemInfo.newScreen = typeof(TScreen);
        itemInfo.tab       = tab;
        itemInfos.Add(itemInfo);
        return this;
    }

    public RsSideScreen Create<TScreen>() where TScreen : SideScreenContent {
        var info = new ItemInfo();
        info.newScreen = typeof(TScreen);
        itemInfos.Add(info);
        return this;
    }

    public RsSideScreen Add(Func<SideScreenContent> add, bool isPrefab) {
        var info = new ItemInfo();
        info.add      = add;
        info.isPrefab = isPrefab;
        itemInfos.Add(info);
        return this;
    }

    public RsSideScreen Add(SideScreenContent prefab) {
        var info = new ItemInfo();
        info.prefab   = prefab;
        info.isPrefab = true;
        itemInfos.Add(info);
        return this;
    }

    protected override void Initialized() {
        Harmony.Patch(typeof(DetailsScreen),
                      "OnPrefabInit",
                      null,
                      new HarmonyMethod(typeof(RsSideScreen),
                                        nameof(DetailsScreen_OnPrefabInit_Patch),
                                        new[] { typeof(List<DetailsScreen.SideScreenRef>) }));
    }

    public static void DetailsScreen_OnPrefabInit_Patch(List<DetailsScreen.SideScreenRef> ___sideScreens) {
        foreach (var itemInfo in Instance.itemInfos)
            if (itemInfo.sourceScreen != null && itemInfo.newScreen != null)
                CreateSideScreen(___sideScreens, itemInfo.sourceScreen, itemInfo.newScreen);
            else {
                if (itemInfo.newScreen != null)
                    CreateSideScreen(___sideScreens, itemInfo.newScreen, itemInfo.tab);
                else {
                    if (itemInfo.add != null)
                        AddSideScreen(___sideScreens, itemInfo.add(), itemInfo.isPrefab);
                    else {
                        if (itemInfo.prefab != null) AddSideScreen(___sideScreens, itemInfo.prefab, true);
                    }
                }
            }
    }

    /// <summary>
    ///     创建一个侧边栏面板
    /// </summary>
    /// <param name="existing">已经存在的面板</param>
    /// <param name="parent"></param>
    /// <typeparam name="TSourceScreen"></typeparam>
    /// <typeparam name="TNewScreen"></typeparam>
    /// <returns></returns>
    public static TNewScreen CreateSideScreen<TSourceScreen, TNewScreen>(
        IList<DetailsScreen.SideScreenRef> existing,
        GameObject parent) where TSourceScreen : SideScreenContent where TNewScreen : RsSideScreenContent {
        return (TNewScreen)CreateSideScreen(existing, parent, typeof(TSourceScreen), typeof(TNewScreen));
    }

    public static RsSideScreenContent CreateSideScreen(IList<DetailsScreen.SideScreenRef> existing,
                                                       GameObject                         parent,
                                                       Type                               sourceScreen,
                                                       Type                               newScreen) {
        if (sourceScreen.IsAssignableFrom(typeof(SideScreenContent)))
            throw new TypeLoadException("参数sourceScreen不可用的，该类型必须继承" + typeof(SideScreenContent).FullName);

        if (newScreen.IsAssignableFrom(typeof(RsSideScreenContent)))
            throw new TypeLoadException("参数newScreen不可用的，该类型必须继承" + typeof(RsSideScreenContent).FullName);

        foreach (var sideScreenRef in existing) {
            if (sideScreenRef.screenPrefab.GetType() != sourceScreen) continue;

            var logicBroadcastChannelSideScreen = sideScreenRef.screenPrefab;
            if (logicBroadcastChannelSideScreen != null) {
                var sideScreenRef2    = new DetailsScreen.SideScreenRef();
                var newScreenInstance = CopySideScreen(logicBroadcastChannelSideScreen, newScreen);
                sideScreenRef2.name           = newScreenInstance.name;
                sideScreenRef2.screenPrefab   = newScreenInstance;
                sideScreenRef2.screenInstance = newScreenInstance;
                var transform = newScreenInstance.gameObject.transform;
                transform.SetParent(parent.transform);
                transform.localScale = Vector3.one;
                existing.Insert(0, sideScreenRef2);
                return newScreenInstance;
            }
        }

        return null;
    }

    public static TNewScreen CreateSideScreen<TNewScreen>(IList<DetailsScreen.SideScreenRef> existing,
                                                          DetailsScreen.SidescreenTabTypes   tab)
        where TNewScreen : SideScreenContent {
        return (TNewScreen)CreateSideScreen(existing, typeof(TNewScreen), tab);
    }

    public RsSideScreen CopyAndCreate<TSourceScreen, TNewScreen>()
        where TSourceScreen : SideScreenContent where TNewScreen : RsSideScreenContent {
        ItemInfo itemInfo = new ItemInfo();
        itemInfo.sourceScreen = typeof(TSourceScreen);
        itemInfo.newScreen    = typeof(TNewScreen);
        itemInfos.Add(itemInfo);
        return this;
    }

    public static RsSideScreenContent CreateSideScreen(IList<DetailsScreen.SideScreenRef> existing,
                                                       Type                               sourceScreen,
                                                       Type                               newScreen) {
        var flag = sourceScreen.IsAssignableFrom(typeof(SideScreenContent));
        if (flag) throw new TypeLoadException("参数sourceScreen不可用的，该类型必须继承" + typeof(SideScreenContent).FullName);

        var flag2 = newScreen.IsAssignableFrom(typeof(RsSideScreenContent));
        if (flag2) throw new TypeLoadException("参数newScreen不可用的，该类型必须继承" + typeof(RsSideScreenContent).FullName);

        foreach (var sideScreenRef in existing) {
            var flag3 = sideScreenRef.screenPrefab.GetType() != sourceScreen;
            if (!flag3) {
                var screenPrefab = sideScreenRef.screenPrefab;
                var flag4        = screenPrefab != null;
                if (flag4) {
                    var sideScreenRef2      = new DetailsScreen.SideScreenRef();
                    var rsSideScreenContent = CopySideScreen(screenPrefab, newScreen);
                    sideScreenRef2.name           = rsSideScreenContent.name;
                    sideScreenRef2.screenPrefab   = rsSideScreenContent;
                    sideScreenRef2.screenInstance = rsSideScreenContent;
                    var transform = rsSideScreenContent.gameObject.transform;
                    var tabOfType = DetailsScreen.Instance.GetTabOfType(sideScreenRef.tab);
                    transform.SetParent(tabOfType.bodyInstance.transform);
                    transform.localScale = Vector3.one;
                    existing.Insert(0, sideScreenRef2);
                    return rsSideScreenContent;
                }
            }
        }

        return null;
    }

    public static SideScreenContent CreateSideScreen(IList<DetailsScreen.SideScreenRef> existing,
                                                     Type                               newScreen,
                                                     DetailsScreen.SidescreenTabTypes   tab) {
        var flag = newScreen.IsAssignableFrom(typeof(SideScreenContent));
        if (flag) throw new TypeLoadException("参数newScreen不可用的，该类型必须继承" + typeof(SideScreenContent).FullName);

        var gameObject = new GameObject();
        gameObject.SetActive(true);
        gameObject.name = newScreen.Name;
        var tabOfType = DetailsScreen.Instance.GetTabOfType(tab);
        gameObject.transform.SetParent(tabOfType.bodyInstance.transform, false);
        var sideScreenContent = (SideScreenContent)gameObject.AddComponent(newScreen);
        existing.Add(new DetailsScreen.SideScreenRef {
            name = gameObject.name, screenPrefab = sideScreenContent, screenInstance = sideScreenContent
        });

        return sideScreenContent;
    }

    private static RsSideScreenContent CopySideScreen(SideScreenContent sourceScreen, Type newScreen) {
        var gameObject = Object.Instantiate(sourceScreen.gameObject, null, false);
        gameObject.name = newScreen.Name;
        var activeSelf = gameObject.activeSelf;
        gameObject.SetActive(false);
        var sourceScreen2 = (SideScreenContent)gameObject.GetComponent(sourceScreen.GetType());
        var newScreen2    = (RsSideScreenContent)gameObject.AddComponent(newScreen);

        var copyFieldDict = GetCopyFieldDict(newScreen);

        if (copyFieldDict != null && copyFieldDict.Count > 0)
            foreach (var (newName, sourceName) in copyFieldDict) {
                var sourceField = sourceScreen.GetType()
                                              .GetField(sourceName,
                                                        BindingFlags.Instance  |
                                                        BindingFlags.NonPublic |
                                                        BindingFlags.Public);

                var newField = newScreen.GetField(newName,
                                                  BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (sourceField == null) throw new Exception("not found sourceField, name: " + sourceName);

                if (newField == null) throw new Exception("not found newField, name: " + newName);

                newField.SetValue(newScreen2, sourceField.GetValue(sourceScreen2));
            }

        newScreen2.CopyFieldAfter();

        Object.DestroyImmediate(sourceScreen2);
        gameObject.SetActive(activeSelf);
        return newScreen2;
    }

    public static void AddSideScreen(IList<DetailsScreen.SideScreenRef> existing,
                                     SideScreenContent                  gameObject,
                                     bool                               isPrefab) {
        var sideScreenRef = new DetailsScreen.SideScreenRef();
        sideScreenRef.name         = gameObject.name;
        sideScreenRef.screenPrefab = gameObject;
        var flag                               = !isPrefab;
        if (flag) sideScreenRef.screenInstance = gameObject;

        existing.Add(sideScreenRef);
    }

    /// <summary>
    ///     new -> source
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetCopyFieldDict(Type type) {
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        Dictionary<string, string> dic = new();
        foreach (var info in fields) {
            var copyField = (CopyField)Attribute.GetCustomAttribute(info, typeof(CopyField));
            if (copyField != null) {
                if (string.IsNullOrWhiteSpace(copyField.alias))
                    dic.Add(info.Name, info.Name);
                else
                    dic.Add(info.Name, copyField.alias);
            }
        }

        return dic;
    }

    protected class ItemInfo {
        public Func<SideScreenContent>          add;
        public bool                             isPrefab;
        public Type                             newScreen;
        public SideScreenContent                prefab;
        public Type                             sourceScreen;
        public DetailsScreen.SidescreenTabTypes tab;
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class CopyField : Attribute {
        public string alias;
        public CopyField() { }
        public CopyField(string alias) { this.alias = alias; }
    }
}