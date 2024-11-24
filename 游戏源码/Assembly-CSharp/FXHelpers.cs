using System.Linq;
using UnityEngine;

public static class FXHelpers
{
	public static KBatchedAnimController CreateEffect(string anim_file_name, Vector3 position, Transform parent = null, bool update_looping_sounds_position = false, Grid.SceneLayer layer = Grid.SceneLayer.Front, bool set_inactive = false)
	{
		KBatchedAnimController component = GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.EffectTemplateId), position, layer).GetComponent<KBatchedAnimController>();
		component.GetComponent<KPrefabID>().PrefabTag = TagManager.Create(anim_file_name);
		component.name = anim_file_name;
		if (parent != null)
		{
			component.transform.SetParent(parent, worldPositionStays: false);
		}
		component.transform.SetPosition(position);
		if (update_looping_sounds_position)
		{
			component.FindOrAddComponent<LoopingSounds>().updatePosition = true;
		}
		KAnimFile anim = Assets.GetAnim(anim_file_name);
		if (anim == null)
		{
			Debug.LogWarning("Missing effect anim: " + anim_file_name);
		}
		else
		{
			component.AnimFiles = new KAnimFile[1] { anim };
		}
		if (!set_inactive)
		{
			component.gameObject.SetActive(value: true);
		}
		return component;
	}

	public static KBatchedAnimController CreateEffect(string[] anim_file_names, Vector3 position, Transform parent = null, bool update_looping_sounds_position = false, Grid.SceneLayer layer = Grid.SceneLayer.Front, bool set_inactive = false)
	{
		KBatchedAnimController component = GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.EffectTemplateId), position, layer).GetComponent<KBatchedAnimController>();
		component.GetComponent<KPrefabID>().PrefabTag = TagManager.Create(anim_file_names[0]);
		component.name = anim_file_names[0];
		if (parent != null)
		{
			component.transform.SetParent(parent, worldPositionStays: false);
		}
		component.transform.SetPosition(position);
		if (update_looping_sounds_position)
		{
			component.FindOrAddComponent<LoopingSounds>().updatePosition = true;
		}
		component.AnimFiles = (from e in anim_file_names.Select((string name) => (name: name, anim: Assets.GetAnim(name))).Where(delegate((string name, KAnimFile anim) e)
			{
				if (e.anim == null)
				{
					Debug.LogWarning("Missing effect anim: " + e.name);
					return false;
				}
				return true;
			})
			select e.anim).ToArray();
		if (!set_inactive)
		{
			component.gameObject.SetActive(value: true);
		}
		return component;
	}

	public static KBatchedAnimController CreateEffectOverride(string[] anim_file_names, Vector3 position, Transform parent = null, bool update_looping_sounds_position = false, Grid.SceneLayer layer = Grid.SceneLayer.Front, bool set_inactive = false)
	{
		KBatchedAnimController component = GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.EffectTemplateOverrideId), position, layer).GetComponent<KBatchedAnimController>();
		component.GetComponent<KPrefabID>().PrefabTag = TagManager.Create(anim_file_names[0]);
		component.name = anim_file_names[0];
		if (parent != null)
		{
			component.transform.SetParent(parent, worldPositionStays: false);
		}
		component.transform.SetPosition(position);
		if (update_looping_sounds_position)
		{
			component.FindOrAddComponent<LoopingSounds>().updatePosition = true;
		}
		component.AnimFiles = (from e in anim_file_names.Select((string name) => (name: name, anim: Assets.GetAnim(name))).Where(delegate((string name, KAnimFile anim) e)
			{
				if (e.anim == null)
				{
					Debug.LogWarning("Missing effect anim: " + e.name);
					return false;
				}
				return true;
			})
			select e.anim).ToArray();
		if (!set_inactive)
		{
			component.gameObject.SetActive(value: true);
		}
		return component;
	}
}
