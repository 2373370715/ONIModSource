using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020004E9 RID: 1257
public class CommonPlacerConfig
{
	// Token: 0x0600162C RID: 5676 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600162D RID: 5677 RVA: 0x00196D3C File Offset: 0x00194F3C
	public GameObject CreatePrefab(string id, string name, Material default_material)
	{
		GameObject gameObject = EntityTemplates.CreateEntity(id, name, true);
		gameObject.layer = LayerMask.NameToLayer("PlaceWithDepth");
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<StateMachineController>();
		gameObject.AddOrGet<Prioritizable>().iconOffset = new Vector2(0.3f, 0.32f);
		KBoxCollider2D kboxCollider2D = gameObject.AddOrGet<KBoxCollider2D>();
		kboxCollider2D.offset = new Vector2(0f, 0.5f);
		kboxCollider2D.size = new Vector2(1f, 1f);
		GameObject gameObject2 = new GameObject("Mask");
		gameObject2.layer = LayerMask.NameToLayer("PlaceWithDepth");
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.SetLocalPosition(new Vector3(0f, 0.5f, -3.537f));
		gameObject2.transform.eulerAngles = new Vector3(0f, 180f, 0f);
		gameObject2.AddComponent<MeshFilter>().sharedMesh = Assets.instance.commonPlacerAssets.mesh;
		MeshRenderer meshRenderer = gameObject2.AddComponent<MeshRenderer>();
		meshRenderer.lightProbeUsage = LightProbeUsage.Off;
		meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		meshRenderer.receiveShadows = false;
		meshRenderer.sharedMaterial = default_material;
		EasingAnimations easingAnimations = gameObject2.AddComponent<EasingAnimations>();
		EasingAnimations.AnimationScales animationScales = default(EasingAnimations.AnimationScales);
		animationScales.name = "ScaleUp";
		animationScales.startScale = 0f;
		animationScales.endScale = 1f;
		animationScales.type = EasingAnimations.AnimationScales.AnimationType.EaseInOutBack;
		animationScales.easingMultiplier = 5f;
		EasingAnimations.AnimationScales animationScales2 = new EasingAnimations.AnimationScales
		{
			name = "ScaleDown",
			startScale = 1f,
			endScale = 0f,
			type = EasingAnimations.AnimationScales.AnimationType.EaseOutBack,
			easingMultiplier = 1f
		};
		easingAnimations.scales = new EasingAnimations.AnimationScales[]
		{
			animationScales,
			animationScales2
		};
		return gameObject;
	}

	// Token: 0x020004EA RID: 1258
	[Serializable]
	public class CommonPlacerAssets
	{
		// Token: 0x04000EF9 RID: 3833
		public Mesh mesh;
	}
}
