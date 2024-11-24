using System;
using UnityEngine;

// Token: 0x02000D50 RID: 3408
public class EightDirectionController
{
	// Token: 0x1700034C RID: 844
	// (get) Token: 0x060042C0 RID: 17088 RVA: 0x000CB17D File Offset: 0x000C937D
	// (set) Token: 0x060042C1 RID: 17089 RVA: 0x000CB185 File Offset: 0x000C9385
	public KBatchedAnimController controller { get; private set; }

	// Token: 0x060042C2 RID: 17090 RVA: 0x000CB18E File Offset: 0x000C938E
	public EightDirectionController(KAnimControllerBase buildingController, string targetSymbol, string defaultAnim, EightDirectionController.Offset frontBank)
	{
		this.Initialize(buildingController, targetSymbol, defaultAnim, frontBank, Grid.SceneLayer.NoLayer);
	}

	// Token: 0x060042C3 RID: 17091 RVA: 0x002427CC File Offset: 0x002409CC
	private void Initialize(KAnimControllerBase buildingController, string targetSymbol, string defaultAnim, EightDirectionController.Offset frontBack, Grid.SceneLayer userSpecifiedRenderLayer)
	{
		string name = buildingController.name + ".eight_direction";
		this.gameObject = new GameObject(name);
		this.gameObject.SetActive(false);
		this.gameObject.transform.parent = buildingController.transform;
		this.gameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
		this.defaultAnim = defaultAnim;
		this.controller = this.gameObject.AddOrGet<KBatchedAnimController>();
		this.controller.AnimFiles = new KAnimFile[]
		{
			buildingController.AnimFiles[0]
		};
		this.controller.initialAnim = defaultAnim;
		this.controller.isMovable = true;
		this.controller.sceneLayer = Grid.SceneLayer.NoLayer;
		if (EightDirectionController.Offset.UserSpecified == frontBack)
		{
			this.controller.sceneLayer = userSpecifiedRenderLayer;
		}
		buildingController.SetSymbolVisiblity(targetSymbol, false);
		bool flag;
		Vector3 position = buildingController.GetSymbolTransform(new HashedString(targetSymbol), out flag).GetColumn(3);
		switch (frontBack)
		{
		case EightDirectionController.Offset.Infront:
			position.z = buildingController.transform.GetPosition().z - 0.1f;
			break;
		case EightDirectionController.Offset.Behind:
			position.z = buildingController.transform.GetPosition().z + 0.1f;
			break;
		case EightDirectionController.Offset.UserSpecified:
			position.z = Grid.GetLayerZ(userSpecifiedRenderLayer);
			break;
		}
		this.gameObject.transform.SetPosition(position);
		this.gameObject.SetActive(true);
		this.link = new KAnimLink(buildingController, this.controller);
	}

	// Token: 0x060042C4 RID: 17092 RVA: 0x000CB1A3 File Offset: 0x000C93A3
	public void SetPositionPercent(float percent_full)
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.SetPositionPercent(percent_full);
	}

	// Token: 0x060042C5 RID: 17093 RVA: 0x000CB1C0 File Offset: 0x000C93C0
	public void SetSymbolTint(KAnimHashedString symbol, Color32 colour)
	{
		if (this.controller != null)
		{
			this.controller.SetSymbolTint(symbol, colour);
		}
	}

	// Token: 0x060042C6 RID: 17094 RVA: 0x000CB1E2 File Offset: 0x000C93E2
	public void SetRotation(float rot)
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.Rotation = rot;
	}

	// Token: 0x060042C7 RID: 17095 RVA: 0x000CB1FF File Offset: 0x000C93FF
	public void PlayAnim(string anim, KAnim.PlayMode mode = KAnim.PlayMode.Once)
	{
		this.controller.Play(anim, mode, 1f, 0f);
	}

	// Token: 0x04002D9C RID: 11676
	public GameObject gameObject;

	// Token: 0x04002D9D RID: 11677
	private string defaultAnim;

	// Token: 0x04002D9E RID: 11678
	private KAnimLink link;

	// Token: 0x02000D51 RID: 3409
	public enum Offset
	{
		// Token: 0x04002DA0 RID: 11680
		Infront,
		// Token: 0x04002DA1 RID: 11681
		Behind,
		// Token: 0x04002DA2 RID: 11682
		UserSpecified
	}
}
