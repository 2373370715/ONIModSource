using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

// Token: 0x02001932 RID: 6450
[AddComponentMenu("KMonoBehaviour/scripts/RocketModule")]
public class RocketModule : KMonoBehaviour
{
	// Token: 0x0600864C RID: 34380 RVA: 0x0034C380 File Offset: 0x0034A580
	public ProcessCondition AddModuleCondition(ProcessCondition.ProcessConditionType conditionType, ProcessCondition condition)
	{
		if (!this.moduleConditions.ContainsKey(conditionType))
		{
			this.moduleConditions.Add(conditionType, new List<ProcessCondition>());
		}
		if (!this.moduleConditions[conditionType].Contains(condition))
		{
			this.moduleConditions[conditionType].Add(condition);
		}
		return condition;
	}

	// Token: 0x0600864D RID: 34381 RVA: 0x0034C3D4 File Offset: 0x0034A5D4
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		List<ProcessCondition> list = new List<ProcessCondition>();
		if (conditionType == ProcessCondition.ProcessConditionType.All)
		{
			using (Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>>.Enumerator enumerator = this.moduleConditions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ProcessCondition.ProcessConditionType, List<ProcessCondition>> keyValuePair = enumerator.Current;
					list.AddRange(keyValuePair.Value);
				}
				return list;
			}
		}
		if (this.moduleConditions.ContainsKey(conditionType))
		{
			list = this.moduleConditions[conditionType];
		}
		return list;
	}

	// Token: 0x0600864E RID: 34382 RVA: 0x000F7F6D File Offset: 0x000F616D
	public void SetBGKAnim(KAnimFile anim_file)
	{
		this.bgAnimFile = anim_file;
	}

	// Token: 0x0600864F RID: 34383 RVA: 0x000F7F76 File Offset: 0x000F6176
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GameUtil.SubscribeToTags<RocketModule>(this, RocketModule.OnRocketOnGroundTagDelegate, false);
		GameUtil.SubscribeToTags<RocketModule>(this, RocketModule.OnRocketNotOnGroundTagDelegate, false);
	}

	// Token: 0x06008650 RID: 34384 RVA: 0x0034C454 File Offset: 0x0034A654
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			this.conditionManager = this.FindLaunchConditionManager();
			Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.conditionManager);
			if (spacecraftFromLaunchConditionManager != null)
			{
				this.SetParentRocketName(spacecraftFromLaunchConditionManager.GetRocketName());
			}
			this.RegisterWithConditionManager();
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.AddStatusItem(Db.Get().BuildingStatusItems.RocketName, this);
		}
		base.Subscribe<RocketModule>(1502190696, RocketModule.DEBUG_OnDestroyDelegate);
		this.FixSorting();
		AttachableBuilding component2 = base.GetComponent<AttachableBuilding>();
		component2.onAttachmentNetworkChanged = (Action<object>)Delegate.Combine(component2.onAttachmentNetworkChanged, new Action<object>(this.OnAttachmentNetworkChanged));
		if (this.bgAnimFile != null)
		{
			this.AddBGGantry();
		}
	}

	// Token: 0x06008651 RID: 34385 RVA: 0x0034C51C File Offset: 0x0034A71C
	public void FixSorting()
	{
		int num = 0;
		AttachableBuilding component = base.GetComponent<AttachableBuilding>();
		while (component != null)
		{
			BuildingAttachPoint attachedTo = component.GetAttachedTo();
			if (!(attachedTo != null))
			{
				break;
			}
			component = attachedTo.GetComponent<AttachableBuilding>();
			num++;
		}
		Vector3 localPosition = base.transform.GetLocalPosition();
		localPosition.z = Grid.GetLayerZ(Grid.SceneLayer.Building) - (float)num * 0.01f;
		base.transform.SetLocalPosition(localPosition);
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		if (component2.enabled)
		{
			component2.enabled = false;
			component2.enabled = true;
		}
	}

	// Token: 0x06008652 RID: 34386 RVA: 0x000F7F96 File Offset: 0x000F6196
	private void OnAttachmentNetworkChanged(object ab)
	{
		this.FixSorting();
	}

	// Token: 0x06008653 RID: 34387 RVA: 0x0034C5A8 File Offset: 0x0034A7A8
	private void AddBGGantry()
	{
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		GameObject gameObject = new GameObject();
		gameObject.name = string.Format(this.rocket_module_bg_base_string, base.name, this.rocket_module_bg_affix);
		gameObject.SetActive(false);
		Vector3 position = component.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.InteriorWall);
		gameObject.transform.SetPosition(position);
		gameObject.transform.parent = base.transform;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			this.bgAnimFile
		};
		kbatchedAnimController.initialAnim = this.rocket_module_bg_anim;
		kbatchedAnimController.fgLayer = Grid.SceneLayer.NoLayer;
		kbatchedAnimController.initialMode = KAnim.PlayMode.Paused;
		kbatchedAnimController.FlipX = component.FlipX;
		kbatchedAnimController.FlipY = component.FlipY;
		gameObject.SetActive(true);
	}

	// Token: 0x06008654 RID: 34388 RVA: 0x0034C674 File Offset: 0x0034A874
	private void DEBUG_OnDestroy(object data)
	{
		if (this.conditionManager != null && !App.IsExiting && !KMonoBehaviour.isLoadingScene)
		{
			Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.conditionManager);
			this.conditionManager.DEBUG_TraceModuleDestruction(base.name, (spacecraftFromLaunchConditionManager == null) ? "null spacecraft" : spacecraftFromLaunchConditionManager.state.ToString(), new StackTrace(true).ToString());
		}
	}

	// Token: 0x06008655 RID: 34389 RVA: 0x0034C6E8 File Offset: 0x0034A8E8
	private void OnRocketOnGroundTag(object data)
	{
		this.RegisterComponents();
		Operational component = base.GetComponent<Operational>();
		if (this.operationalLandedRequired && component != null)
		{
			component.SetFlag(RocketModule.landedFlag, true);
		}
	}

	// Token: 0x06008656 RID: 34390 RVA: 0x0034C720 File Offset: 0x0034A920
	private void OnRocketNotOnGroundTag(object data)
	{
		this.DeregisterComponents();
		Operational component = base.GetComponent<Operational>();
		if (this.operationalLandedRequired && component != null)
		{
			component.SetFlag(RocketModule.landedFlag, false);
		}
	}

	// Token: 0x06008657 RID: 34391 RVA: 0x0034C758 File Offset: 0x0034A958
	public void DeregisterComponents()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		component.IsSelectable = false;
		BuildingComplete component2 = base.GetComponent<BuildingComplete>();
		if (component2 != null)
		{
			component2.UpdatePosition();
		}
		if (SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(null, false);
		}
		Deconstructable component3 = base.GetComponent<Deconstructable>();
		if (component3 != null)
		{
			component3.SetAllowDeconstruction(false);
		}
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		if (handle.IsValid())
		{
			GameComps.StructureTemperatures.Disable(handle);
		}
		FakeFloorAdder component4 = base.GetComponent<FakeFloorAdder>();
		if (component4 != null)
		{
			component4.SetFloor(false);
		}
		AccessControl component5 = base.GetComponent<AccessControl>();
		if (component5 != null)
		{
			component5.SetRegistered(false);
		}
		foreach (ManualDeliveryKG manualDeliveryKG in base.GetComponents<ManualDeliveryKG>())
		{
			DebugUtil.DevAssert(!manualDeliveryKG.IsPaused, "RocketModule ManualDeliver chore was already paused, when this rocket lands it will re-enable it.", null);
			manualDeliveryKG.Pause(true, "Rocket heading to space");
		}
		BuildingConduitEndpoints[] components2 = base.GetComponents<BuildingConduitEndpoints>();
		for (int i = 0; i < components2.Length; i++)
		{
			components2[i].RemoveEndPoint();
		}
		ReorderableBuilding component6 = base.GetComponent<ReorderableBuilding>();
		if (component6 != null)
		{
			component6.ShowReorderArm(false);
		}
		Workable component7 = base.GetComponent<Workable>();
		if (component7 != null)
		{
			component7.RefreshReachability();
		}
		Structure component8 = base.GetComponent<Structure>();
		if (component8 != null)
		{
			component8.UpdatePosition();
		}
		WireUtilitySemiVirtualNetworkLink component9 = base.GetComponent<WireUtilitySemiVirtualNetworkLink>();
		if (component9 != null)
		{
			component9.SetLinkConnected(false);
		}
		PartialLightBlocking component10 = base.GetComponent<PartialLightBlocking>();
		if (component10 != null)
		{
			component10.ClearLightBlocking();
		}
	}

	// Token: 0x06008658 RID: 34392 RVA: 0x0034C8FC File Offset: 0x0034AAFC
	public void RegisterComponents()
	{
		base.GetComponent<KSelectable>().IsSelectable = true;
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component != null)
		{
			component.UpdatePosition();
		}
		Deconstructable component2 = base.GetComponent<Deconstructable>();
		if (component2 != null)
		{
			component2.SetAllowDeconstruction(true);
		}
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		if (handle.IsValid())
		{
			GameComps.StructureTemperatures.Enable(handle);
		}
		Storage[] components = base.GetComponents<Storage>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].UpdateStoredItemCachedCells();
		}
		FakeFloorAdder component3 = base.GetComponent<FakeFloorAdder>();
		if (component3 != null)
		{
			component3.SetFloor(true);
		}
		AccessControl component4 = base.GetComponent<AccessControl>();
		if (component4 != null)
		{
			component4.SetRegistered(true);
		}
		ManualDeliveryKG[] components2 = base.GetComponents<ManualDeliveryKG>();
		for (int i = 0; i < components2.Length; i++)
		{
			components2[i].Pause(false, "Landing on world");
		}
		BuildingConduitEndpoints[] components3 = base.GetComponents<BuildingConduitEndpoints>();
		for (int i = 0; i < components3.Length; i++)
		{
			components3[i].AddEndpoint();
		}
		ReorderableBuilding component5 = base.GetComponent<ReorderableBuilding>();
		if (component5 != null)
		{
			component5.ShowReorderArm(true);
		}
		Workable component6 = base.GetComponent<Workable>();
		if (component6 != null)
		{
			component6.RefreshReachability();
		}
		Structure component7 = base.GetComponent<Structure>();
		if (component7 != null)
		{
			component7.UpdatePosition();
		}
		WireUtilitySemiVirtualNetworkLink component8 = base.GetComponent<WireUtilitySemiVirtualNetworkLink>();
		if (component8 != null)
		{
			component8.SetLinkConnected(true);
		}
		PartialLightBlocking component9 = base.GetComponent<PartialLightBlocking>();
		if (component9 != null)
		{
			component9.SetLightBlocking();
		}
	}

	// Token: 0x06008659 RID: 34393 RVA: 0x0034CA8C File Offset: 0x0034AC8C
	private void ToggleComponent(Type cmpType, bool enabled)
	{
		MonoBehaviour monoBehaviour = (MonoBehaviour)base.GetComponent(cmpType);
		if (monoBehaviour != null)
		{
			monoBehaviour.enabled = enabled;
		}
	}

	// Token: 0x0600865A RID: 34394 RVA: 0x000F7F9E File Offset: 0x000F619E
	public void RegisterWithConditionManager()
	{
		global::Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
		if (this.conditionManager != null)
		{
			this.conditionManager.RegisterRocketModule(this);
		}
	}

	// Token: 0x0600865B RID: 34395 RVA: 0x000F7FC7 File Offset: 0x000F61C7
	protected override void OnCleanUp()
	{
		if (this.conditionManager != null)
		{
			this.conditionManager.UnregisterRocketModule(this);
		}
		base.OnCleanUp();
	}

	// Token: 0x0600865C RID: 34396 RVA: 0x0034CAB8 File Offset: 0x0034ACB8
	public virtual LaunchConditionManager FindLaunchConditionManager()
	{
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				LaunchConditionManager component = gameObject.GetComponent<LaunchConditionManager>();
				if (component != null)
				{
					return component;
				}
			}
		}
		return null;
	}

	// Token: 0x0600865D RID: 34397 RVA: 0x000F7FE9 File Offset: 0x000F61E9
	public void SetParentRocketName(string newName)
	{
		this.parentRocketName = newName;
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
	}

	// Token: 0x0600865E RID: 34398 RVA: 0x000F8002 File Offset: 0x000F6202
	public virtual string GetParentRocketName()
	{
		return this.parentRocketName;
	}

	// Token: 0x0600865F RID: 34399 RVA: 0x0034CB28 File Offset: 0x0034AD28
	public void MoveToSpace()
	{
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (component != null && component.GetMyWorld() != null)
		{
			component.GetMyWorld().RemoveTopPriorityPrioritizable(component);
		}
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Building component2 = base.GetComponent<Building>();
		component2.Def.UnmarkArea(cell, component2.Orientation, component2.Def.ObjectLayer, base.gameObject);
		Vector3 position = new Vector3(-1f, -1f, 0f);
		base.gameObject.transform.SetPosition(position);
		LogicPorts component3 = base.GetComponent<LogicPorts>();
		if (component3 != null)
		{
			component3.OnMove();
		}
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Entombed, false, this);
	}

	// Token: 0x06008660 RID: 34400 RVA: 0x0034CBF8 File Offset: 0x0034ADF8
	public void MoveToPad(int newCell)
	{
		base.gameObject.transform.SetPosition(Grid.CellToPos(newCell, CellAlignment.Bottom, Grid.SceneLayer.Building));
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Building component = base.GetComponent<Building>();
		component.RefreshCells();
		component.Def.MarkArea(cell, component.Orientation, component.Def.ObjectLayer, base.gameObject);
		LogicPorts component2 = base.GetComponent<LogicPorts>();
		if (component2 != null)
		{
			component2.OnMove();
		}
		Prioritizable component3 = base.GetComponent<Prioritizable>();
		if (component3 != null && component3.IsTopPriority())
		{
			component3.GetMyWorld().AddTopPriorityPrioritizable(component3);
		}
	}

	// Token: 0x0400657E RID: 25982
	public LaunchConditionManager conditionManager;

	// Token: 0x0400657F RID: 25983
	public Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>> moduleConditions = new Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>>();

	// Token: 0x04006580 RID: 25984
	public static readonly Operational.Flag landedFlag = new Operational.Flag("landed", Operational.Flag.Type.Requirement);

	// Token: 0x04006581 RID: 25985
	public bool operationalLandedRequired = true;

	// Token: 0x04006582 RID: 25986
	private string rocket_module_bg_base_string = "{0}{1}";

	// Token: 0x04006583 RID: 25987
	private string rocket_module_bg_affix = "BG";

	// Token: 0x04006584 RID: 25988
	private string rocket_module_bg_anim = "on";

	// Token: 0x04006585 RID: 25989
	[SerializeField]
	private KAnimFile bgAnimFile;

	// Token: 0x04006586 RID: 25990
	protected string parentRocketName = UI.STARMAP.DEFAULT_NAME;

	// Token: 0x04006587 RID: 25991
	private static readonly EventSystem.IntraObjectHandler<RocketModule> DEBUG_OnDestroyDelegate = new EventSystem.IntraObjectHandler<RocketModule>(delegate(RocketModule component, object data)
	{
		component.DEBUG_OnDestroy(data);
	});

	// Token: 0x04006588 RID: 25992
	private static readonly EventSystem.IntraObjectHandler<RocketModule> OnRocketOnGroundTagDelegate = GameUtil.CreateHasTagHandler<RocketModule>(GameTags.RocketOnGround, delegate(RocketModule component, object data)
	{
		component.OnRocketOnGroundTag(data);
	});

	// Token: 0x04006589 RID: 25993
	private static readonly EventSystem.IntraObjectHandler<RocketModule> OnRocketNotOnGroundTagDelegate = GameUtil.CreateHasTagHandler<RocketModule>(GameTags.RocketNotOnGround, delegate(RocketModule component, object data)
	{
		component.OnRocketNotOnGroundTag(data);
	});
}
