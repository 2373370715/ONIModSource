using System.Diagnostics;

[DebuggerDisplay("{Name}")]
public class FloorSoundEvent : SoundEvent {
    public static float IDLE_WALKING_VOLUME_REDUCTION = 0.55f;

    public FloorSoundEvent(string file_name, string sound_name, int frame) :
        base(file_name, sound_name, frame, false, false, IGNORE_INTERVAL, true) {
        noiseValues = SoundEventVolumeCache.instance.GetVolume("FloorSoundEvent", sound_name);
    }

    public override void PlaySound(AnimEventManager.EventPlayerData behaviour) {
        var vector                     = behaviour.position;
        var controller                 = behaviour.controller;
        if (controller != null) vector = controller.GetPivotSymbolPosition();
        var num                        = Grid.PosToCell(vector);
        var cell                       = Grid.CellBelow(num);
        if (!Grid.IsValidCell(cell)) return;

        var sound = GlobalAssets.GetSound(StringFormatter.Combine(GetAudioCategory(cell), "_", name), true);
        if (sound == null) {
            sound = GlobalAssets.GetSound(StringFormatter.Combine("Rock_", name), true);
            if (sound == null) sound = GlobalAssets.GetSound(name, true);
        }

        var gameObject = behaviour.controller.gameObject;
        var component  = gameObject.GetComponent<MinionIdentity>();
        objectIsSelectedAndVisible = ObjectIsSelectedAndVisible(gameObject);
        if (IsLowPrioritySound(sound) && !objectIsSelectedAndVisible) return;

        vector   = GetCameraScaledPosition(vector);
        vector.z = 0f;
        if (objectIsSelectedAndVisible) vector = AudioHighlightListenerPosition(vector);
        if (Grid.Element == null) return;

        var isLiquid = Grid.Element[num].IsLiquid;
        var num2     = 0f;
        if (isLiquid) {
            num2 = SoundUtil.GetLiquidDepth(num);
            var sound2 = GlobalAssets.GetSound("Liquid_footstep", true);
            if (sound2 != null &&
                (objectIsSelectedAndVisible || ShouldPlaySound(behaviour.controller, sound2, looping, isDynamic))) {
                var instance = BeginOneShot(sound2, vector, GetVolume(objectIsSelectedAndVisible));
                if (num2 > 0f) instance.setParameterByName("liquidDepth", num2);
                EndOneShot(instance);
            }
        }

        if (component != null && component.model == BionicMinionConfig.MODEL) {
            var sound3 = GlobalAssets.GetSound("Bionic_move", true);
            if (sound3 != null &&
                (objectIsSelectedAndVisible || ShouldPlaySound(behaviour.controller, sound3, looping, isDynamic)))
                EndOneShot(BeginOneShot(sound3, vector, GetVolume(objectIsSelectedAndVisible)));
        }

        if (sound != null &&
            (objectIsSelectedAndVisible || ShouldPlaySound(behaviour.controller, sound, looping, isDynamic))) {
            var instance2 = BeginOneShot(sound, vector);
            if (instance2.isValid()) {
                if (num2 > 0f) instance2.setParameterByName("liquidDepth", num2);
                if (behaviour.controller.HasAnimationFile("anim_loco_walk_kanim"))
                    instance2.setVolume(IDLE_WALKING_VOLUME_REDUCTION);

                EndOneShot(instance2);
            }
        }
    }

    private static string GetAudioCategory(int cell) {
        if (!Grid.IsValidCell(cell)) return "Rock";

        var element = Grid.Element[cell];
        if (Grid.Foundation[cell]) {
            BuildingDef buildingDef = null;
            var         gameObject  = Grid.Objects[cell, 1];
            if (gameObject != null) {
                Building component                 = gameObject.GetComponent<BuildingComplete>();
                if (component != null) buildingDef = component.Def;
            }

            var result = "";
            if (buildingDef != null) {
                var prefabID = buildingDef.PrefabID;
                if (prefabID == "PlasticTile")
                    result = "TilePlastic";
                else if (prefabID == "GlassTile")
                    result = "TileGlass";
                else if (prefabID == "BunkerTile")
                    result = "TileBunker";
                else if (prefabID == "MetalTile")
                    result = "TileMetal";
                else if (prefabID == "CarpetTile")
                    result = "Carpet";
                else if (prefabID == "SnowTile")
                    result = "TileSnow";
                else if (prefabID == "WoodTile")
                    result = "TileWood";
                else
                    result = "Tile";
            }

            return result;
        }

        var floorEventAudioCategory = element.substance.GetFloorEventAudioCategory();
        if (floorEventAudioCategory != null) return floorEventAudioCategory;

        if (element.HasTag(GameTags.RefinedMetal)) return "RefinedMetal";

        if (element.HasTag(GameTags.Metal)) return "RawMetal";

        return "Rock";
    }
}