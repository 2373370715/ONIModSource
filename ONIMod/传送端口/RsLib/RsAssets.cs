using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace RsLib;

public class RsAssets : RsModule<RsAssets> {
    private readonly Dictionary<string, Sprite> sprites       = new();
    private readonly Dictionary<string, Sprite> tintedSprites = new();

    public RsAssets AddSprite(string key, Sprite sprite) {
        sprites.Add(key, sprite);
        return this;
    }

    public RsAssets AddSprite(Sprite sprite) {
        sprites.Add(sprite.name, sprite);
        return this;
    }

    public RsAssets AddTintedSprite(Sprite sprite) {
        tintedSprites.Add(sprite.name, sprite);
        return this;
    }

    public RsAssets AddTintedSprite(string name, Sprite sprite) {
        tintedSprites.Add(name, sprite);
        return this;
    }

    public RsAssets AddStatusItemIcon(string name, Sprite sprite) {
        tintedSprites.Add(name, sprite);
        return this;
    }

    protected override void Initialized() {
        Harmony.Patch(typeof(Assets),
                      "OnPrefabInit",
                      postfix: new HarmonyMethod(typeof(RsAssets), nameof(Assets_OnPrefabInit_Postfix)));
    }

    public static void Assets_OnPrefabInit_Postfix() {
        if (Instance == null) return;

        foreach (var kv in Instance.sprites) Assets.Sprites.Add(new HashedString(kv.Key), kv.Value);
        foreach (var kv in Instance.tintedSprites) {
            var tintedSprite1 = new TintedSprite();
            tintedSprite1.name   = kv.Key;
            tintedSprite1.sprite = kv.Value;
            Assets.TintedSprites.Add(tintedSprite1);
        }
    }
}