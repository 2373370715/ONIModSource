using System;
using System.Diagnostics;
using FMOD.Studio;
using UnityEngine;

[DebuggerDisplay("{Name}")]
public class FloorSoundEvent : SoundEvent
{
	public FloorSoundEvent(string file_name, string sound_name, int frame) : base(file_name, sound_name, frame, false, false, (float)SoundEvent.IGNORE_INTERVAL, true)
	{
		base.noiseValues = SoundEventVolumeCache.instance.GetVolume("FloorSoundEvent", sound_name);
	}

	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		Vector3 vector = behaviour.GetComponent<Transform>().GetPosition();
		KBatchedAnimController component = behaviour.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			vector = component.GetPivotSymbolPosition();
		}
		int num = Grid.PosToCell(vector);
		int cell = Grid.CellBelow(num);
		if (!Grid.IsValidCell(cell))
		{
			return;
		}
		string sound = GlobalAssets.GetSound(StringFormatter.Combine(FloorSoundEvent.GetAudioCategory(cell), "_", base.name), true);
		if (sound == null)
		{
			sound = GlobalAssets.GetSound(StringFormatter.Combine("Rock_", base.name), true);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound(base.name, true);
			}
		}
		GameObject gameObject = behaviour.controller.gameObject;
		base.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
		if (SoundEvent.IsLowPrioritySound(sound) && !base.objectIsSelectedAndVisible)
		{
			return;
		}
		vector = SoundEvent.GetCameraScaledPosition(vector, false);
		vector.z = 0f;
		if (base.objectIsSelectedAndVisible)
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
		}
		if (Grid.Element == null)
		{
			return;
		}
		bool isLiquid = Grid.Element[num].IsLiquid;
		float num2 = 0f;
		if (isLiquid)
		{
			num2 = SoundUtil.GetLiquidDepth(num);
			string sound2 = GlobalAssets.GetSound("Liquid_footstep", true);
			if (sound2 != null && (base.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, sound2, base.looping, this.isDynamic)))
			{
				FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(sound2, vector, SoundEvent.GetVolume(base.objectIsSelectedAndVisible), false);
				if (num2 > 0f)
				{
					instance.setParameterByName("liquidDepth", num2, false);
				}
				SoundEvent.EndOneShot(instance);
			}
		}
		if (sound != null && (base.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, sound, base.looping, this.isDynamic)))
		{
			FMOD.Studio.EventInstance instance2 = SoundEvent.BeginOneShot(sound, vector, 1f, false);
			if (instance2.isValid())
			{
				if (num2 > 0f)
				{
					instance2.setParameterByName("liquidDepth", num2, false);
				}
				if (behaviour.controller.HasAnimationFile("anim_loco_walk_kanim"))
				{
					instance2.setVolume(FloorSoundEvent.IDLE_WALKING_VOLUME_REDUCTION);
				}
				SoundEvent.EndOneShot(instance2);
			}
		}
	}

	private static string GetAudioCategory(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return "Rock";
		}
		Element element = Grid.Element[cell];
		if (Grid.Foundation[cell])
		{
			BuildingDef buildingDef = null;
			GameObject gameObject = Grid.Objects[cell, 1];
			if (gameObject != null)
			{
				Building component = gameObject.GetComponent<BuildingComplete>();
				if (component != null)
				{
					buildingDef = component.Def;
				}
			}
			string result = "";
			if (buildingDef != null)
			{
				string prefabID = buildingDef.PrefabID;
				if (prefabID == "PlasticTile")
				{
					result = "TilePlastic";
				}
				else if (prefabID == "GlassTile")
				{
					result = "TileGlass";
				}
				else if (prefabID == "BunkerTile")
				{
					result = "TileBunker";
				}
				else if (prefabID == "MetalTile")
				{
					result = "TileMetal";
				}
				else if (prefabID == "CarpetTile")
				{
					result = "Carpet";
				}
				else if (prefabID == "SnowTile")
				{
					result = "TileSnow";
				}
				else if (prefabID == "WoodTile")
				{
					result = "TileWood";
				}
				else
				{
					result = "Tile";
				}
			}
			return result;
		}
		string floorEventAudioCategory = element.substance.GetFloorEventAudioCategory();
		if (floorEventAudioCategory != null)
		{
			return floorEventAudioCategory;
		}
		if (element.HasTag(GameTags.RefinedMetal))
		{
			return "RefinedMetal";
		}
		if (element.HasTag(GameTags.Metal))
		{
			return "RawMetal";
		}
		return "Rock";
	}

	public static float IDLE_WALKING_VOLUME_REDUCTION = 0.55f;
}
