using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using STRINGS;
using UnityEngine;

// Token: 0x02000AA2 RID: 2722
public class Navigator : StateMachineComponent<Navigator.StatesInstance>, ISaveLoadableDetails
{
	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06003285 RID: 12933 RVA: 0x000C0D76 File Offset: 0x000BEF76
	// (set) Token: 0x06003286 RID: 12934 RVA: 0x000C0D7E File Offset: 0x000BEF7E
	public KMonoBehaviour target { get; set; }

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06003287 RID: 12935 RVA: 0x000C0D87 File Offset: 0x000BEF87
	// (set) Token: 0x06003288 RID: 12936 RVA: 0x000C0D8F File Offset: 0x000BEF8F
	public CellOffset[] targetOffsets { get; private set; }

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06003289 RID: 12937 RVA: 0x000C0D98 File Offset: 0x000BEF98
	// (set) Token: 0x0600328A RID: 12938 RVA: 0x000C0DA0 File Offset: 0x000BEFA0
	public NavGrid NavGrid { get; private set; }

	// Token: 0x0600328B RID: 12939 RVA: 0x00203CE0 File Offset: 0x00201EE0
	public void Serialize(BinaryWriter writer)
	{
		byte currentNavType = (byte)this.CurrentNavType;
		writer.Write(currentNavType);
		writer.Write(this.distanceTravelledByNavType.Count);
		foreach (KeyValuePair<NavType, int> keyValuePair in this.distanceTravelledByNavType)
		{
			byte key = (byte)keyValuePair.Key;
			writer.Write(key);
			writer.Write(keyValuePair.Value);
		}
	}

	// Token: 0x0600328C RID: 12940 RVA: 0x00203D68 File Offset: 0x00201F68
	public void Deserialize(IReader reader)
	{
		NavType navType = (NavType)reader.ReadByte();
		if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 11))
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				NavType key = (NavType)reader.ReadByte();
				int value = reader.ReadInt32();
				if (this.distanceTravelledByNavType.ContainsKey(key))
				{
					this.distanceTravelledByNavType[key] = value;
				}
			}
		}
		bool flag = false;
		NavType[] validNavTypes = this.NavGrid.ValidNavTypes;
		for (int j = 0; j < validNavTypes.Length; j++)
		{
			if (validNavTypes[j] == navType)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.CurrentNavType = navType;
		}
	}

	// Token: 0x0600328D RID: 12941 RVA: 0x00203E10 File Offset: 0x00202010
	protected override void OnPrefabInit()
	{
		this.transitionDriver = new TransitionDriver(this);
		this.targetLocator = Util.KInstantiate(Assets.GetPrefab(TargetLocator.ID), null, null).GetComponent<KPrefabID>();
		this.targetLocator.gameObject.SetActive(true);
		this.log = new LoggerFSS("Navigator", 35);
		this.simRenderLoadBalance = true;
		this.autoRegisterSimRender = false;
		this.NavGrid = Pathfinding.Instance.GetNavGrid(this.NavGridName);
		base.GetComponent<PathProber>().SetValidNavTypes(this.NavGrid.ValidNavTypes, this.maxProbingRadius);
		this.distanceTravelledByNavType = new Dictionary<NavType, int>();
		for (int i = 0; i < 11; i++)
		{
			this.distanceTravelledByNavType.Add((NavType)i, 0);
		}
	}

	// Token: 0x0600328E RID: 12942 RVA: 0x00203ED4 File Offset: 0x002020D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Navigator>(1623392196, Navigator.OnDefeatedDelegate);
		base.Subscribe<Navigator>(-1506500077, Navigator.OnDefeatedDelegate);
		base.Subscribe<Navigator>(493375141, Navigator.OnRefreshUserMenuDelegate);
		base.Subscribe<Navigator>(-1503271301, Navigator.OnSelectObjectDelegate);
		base.Subscribe<Navigator>(856640610, Navigator.OnStoreDelegate);
		if (this.updateProber)
		{
			SimAndRenderScheduler.instance.Add(this, false);
		}
		this.pathProbeTask = new Navigator.PathProbeTask(this);
		this.SetCurrentNavType(this.CurrentNavType);
		this.SubscribeUnstuckFunctions();
	}

	// Token: 0x0600328F RID: 12943 RVA: 0x000C0DA9 File Offset: 0x000BEFA9
	private void SubscribeUnstuckFunctions()
	{
		if (this.CurrentNavType == NavType.Tube)
		{
			GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingTileChanged));
		}
	}

	// Token: 0x06003290 RID: 12944 RVA: 0x000C0DD6 File Offset: 0x000BEFD6
	private void UnsubscribeUnstuckFunctions()
	{
		GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingTileChanged));
	}

	// Token: 0x06003291 RID: 12945 RVA: 0x00203F70 File Offset: 0x00202170
	private void OnBuildingTileChanged(int cell, object building)
	{
		if (this.CurrentNavType == NavType.Tube && building == null)
		{
			bool flag = cell == Grid.PosToCell(this);
			if (base.smi != null && flag)
			{
				this.SetCurrentNavType(NavType.Floor);
				this.UnsubscribeUnstuckFunctions();
			}
		}
	}

	// Token: 0x06003292 RID: 12946 RVA: 0x000C0DFA File Offset: 0x000BEFFA
	protected override void OnCleanUp()
	{
		this.UnsubscribeUnstuckFunctions();
		base.OnCleanUp();
	}

	// Token: 0x06003293 RID: 12947 RVA: 0x000C0E08 File Offset: 0x000BF008
	public bool IsMoving()
	{
		return base.smi.IsInsideState(base.smi.sm.normal.moving);
	}

	// Token: 0x06003294 RID: 12948 RVA: 0x000C0E2A File Offset: 0x000BF02A
	public bool GoTo(int cell, CellOffset[] offsets = null)
	{
		if (offsets == null)
		{
			offsets = new CellOffset[1];
		}
		this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
		return this.GoTo(this.targetLocator, offsets, NavigationTactics.ReduceTravelDistance);
	}

	// Token: 0x06003295 RID: 12949 RVA: 0x000C0E62 File Offset: 0x000BF062
	public bool GoTo(int cell, CellOffset[] offsets, NavTactic tactic)
	{
		if (offsets == null)
		{
			offsets = new CellOffset[1];
		}
		this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
		return this.GoTo(this.targetLocator, offsets, tactic);
	}

	// Token: 0x06003296 RID: 12950 RVA: 0x000C0E96 File Offset: 0x000BF096
	public void UpdateTarget(int cell)
	{
		this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
	}

	// Token: 0x06003297 RID: 12951 RVA: 0x00203FB0 File Offset: 0x002021B0
	public bool GoTo(KMonoBehaviour target, CellOffset[] offsets, NavTactic tactic)
	{
		if (tactic == null)
		{
			tactic = NavigationTactics.ReduceTravelDistance;
		}
		base.smi.GoTo(base.smi.sm.normal.moving);
		base.smi.sm.moveTarget.Set(target.gameObject, base.smi, false);
		this.tactic = tactic;
		this.target = target;
		this.targetOffsets = offsets;
		this.ClearReservedCell();
		this.AdvancePath(true);
		return this.IsMoving();
	}

	// Token: 0x06003298 RID: 12952 RVA: 0x000C0EB1 File Offset: 0x000BF0B1
	public void BeginTransition(NavGrid.Transition transition)
	{
		this.transitionDriver.EndTransition();
		base.smi.GoTo(base.smi.sm.normal.moving);
		this.transitionDriver.BeginTransition(this, transition, this.defaultSpeed);
	}

	// Token: 0x06003299 RID: 12953 RVA: 0x00204034 File Offset: 0x00202234
	private bool ValidatePath(ref PathFinder.Path path, out bool atNextNode)
	{
		atNextNode = false;
		bool flag = false;
		if (path.IsValid())
		{
			int target_cell = Grid.PosToCell(this.target);
			flag = (this.reservedCell != NavigationReservations.InvalidReservation && this.CanReach(this.reservedCell));
			flag &= Grid.IsCellOffsetOf(this.reservedCell, target_cell, this.targetOffsets);
		}
		if (flag)
		{
			int num = Grid.PosToCell(this);
			flag = (num == path.nodes[0].cell && this.CurrentNavType == path.nodes[0].navType);
			flag |= (atNextNode = (num == path.nodes[1].cell && this.CurrentNavType == path.nodes[1].navType));
		}
		if (!flag)
		{
			return false;
		}
		PathFinderAbilities currentAbilities = this.GetCurrentAbilities();
		return PathFinder.ValidatePath(this.NavGrid, currentAbilities, ref path);
	}

	// Token: 0x0600329A RID: 12954 RVA: 0x0020411C File Offset: 0x0020231C
	public void AdvancePath(bool trigger_advance = true)
	{
		int num = Grid.PosToCell(this);
		if (this.target == null)
		{
			base.Trigger(-766531887, null);
			this.Stop(false, true);
		}
		else if (num == this.reservedCell && this.CurrentNavType != NavType.Tube)
		{
			this.Stop(true, true);
		}
		else
		{
			bool flag2;
			bool flag = !this.ValidatePath(ref this.path, out flag2);
			if (flag2)
			{
				this.path.nodes.RemoveAt(0);
			}
			if (flag)
			{
				int root = Grid.PosToCell(this.target);
				int cellPreferences = this.tactic.GetCellPreferences(root, this.targetOffsets, this);
				this.SetReservedCell(cellPreferences);
				if (this.reservedCell == NavigationReservations.InvalidReservation)
				{
					this.Stop(false, true);
				}
				else
				{
					PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(num, this.CurrentNavType, this.flags);
					PathFinder.UpdatePath(this.NavGrid, this.GetCurrentAbilities(), potential_path, PathFinderQueries.cellQuery.Reset(this.reservedCell), ref this.path);
				}
			}
			if (this.path.IsValid())
			{
				this.BeginTransition(this.NavGrid.transitions[(int)this.path.nodes[1].transitionId]);
				this.distanceTravelledByNavType[this.CurrentNavType] = Mathf.Max(this.distanceTravelledByNavType[this.CurrentNavType] + 1, this.distanceTravelledByNavType[this.CurrentNavType]);
			}
			else if (this.path.HasArrived())
			{
				this.Stop(true, true);
			}
			else
			{
				this.ClearReservedCell();
				this.Stop(false, true);
			}
		}
		if (trigger_advance)
		{
			base.Trigger(1347184327, null);
		}
	}

	// Token: 0x0600329B RID: 12955 RVA: 0x000C0EF1 File Offset: 0x000BF0F1
	public NavGrid.Transition GetNextTransition()
	{
		return this.NavGrid.transitions[(int)this.path.nodes[1].transitionId];
	}

	// Token: 0x0600329C RID: 12956 RVA: 0x002042C4 File Offset: 0x002024C4
	public void Stop(bool arrived_at_destination = false, bool play_idle = true)
	{
		this.target = null;
		this.targetOffsets = null;
		this.path.Clear();
		base.smi.sm.moveTarget.Set(null, base.smi);
		this.transitionDriver.EndTransition();
		if (play_idle)
		{
			HashedString idleAnim = this.NavGrid.GetIdleAnim(this.CurrentNavType);
			this.animController.Play(idleAnim, KAnim.PlayMode.Loop, 1f, 0f);
		}
		if (arrived_at_destination)
		{
			base.smi.GoTo(base.smi.sm.normal.arrived);
			return;
		}
		if (base.smi.GetCurrentState() == base.smi.sm.normal.moving)
		{
			this.ClearReservedCell();
			base.smi.GoTo(base.smi.sm.normal.failed);
		}
	}

	// Token: 0x0600329D RID: 12957 RVA: 0x000C0F19 File Offset: 0x000BF119
	private void SimEveryTick(float dt)
	{
		if (this.IsMoving())
		{
			this.transitionDriver.UpdateTransition(dt);
		}
	}

	// Token: 0x0600329E RID: 12958 RVA: 0x000C0F2F File Offset: 0x000BF12F
	public void Sim4000ms(float dt)
	{
		this.UpdateProbe(true);
	}

	// Token: 0x0600329F RID: 12959 RVA: 0x000C0F38 File Offset: 0x000BF138
	public void UpdateProbe(bool forceUpdate = false)
	{
		if (forceUpdate || !this.executePathProbeTaskAsync)
		{
			this.pathProbeTask.Update();
			this.pathProbeTask.Run(null);
		}
	}

	// Token: 0x060032A0 RID: 12960 RVA: 0x000C0F5C File Offset: 0x000BF15C
	public void DrawPath()
	{
		if (base.gameObject.activeInHierarchy && this.IsMoving())
		{
			NavPathDrawer.Instance.DrawPath(this.animController.GetPivotSymbolPosition(), this.path);
		}
	}

	// Token: 0x060032A1 RID: 12961 RVA: 0x000C0F8E File Offset: 0x000BF18E
	public void Pause(string reason)
	{
		base.smi.sm.isPaused.Set(true, base.smi, false);
	}

	// Token: 0x060032A2 RID: 12962 RVA: 0x000C0FAE File Offset: 0x000BF1AE
	public void Unpause(string reason)
	{
		base.smi.sm.isPaused.Set(false, base.smi, false);
	}

	// Token: 0x060032A3 RID: 12963 RVA: 0x000C0FCE File Offset: 0x000BF1CE
	private void OnDefeated(object data)
	{
		this.ClearReservedCell();
		this.Stop(false, false);
	}

	// Token: 0x060032A4 RID: 12964 RVA: 0x000C0FDE File Offset: 0x000BF1DE
	private void ClearReservedCell()
	{
		if (this.reservedCell != NavigationReservations.InvalidReservation)
		{
			NavigationReservations.Instance.RemoveOccupancy(this.reservedCell);
			this.reservedCell = NavigationReservations.InvalidReservation;
		}
	}

	// Token: 0x060032A5 RID: 12965 RVA: 0x000C1008 File Offset: 0x000BF208
	private void SetReservedCell(int cell)
	{
		this.ClearReservedCell();
		this.reservedCell = cell;
		NavigationReservations.Instance.AddOccupancy(cell);
	}

	// Token: 0x060032A6 RID: 12966 RVA: 0x000C1022 File Offset: 0x000BF222
	public int GetReservedCell()
	{
		return this.reservedCell;
	}

	// Token: 0x060032A7 RID: 12967 RVA: 0x000C102A File Offset: 0x000BF22A
	public int GetAnchorCell()
	{
		return this.AnchorCell;
	}

	// Token: 0x060032A8 RID: 12968 RVA: 0x000C1032 File Offset: 0x000BF232
	public bool IsValidNavType(NavType nav_type)
	{
		return this.NavGrid.HasNavTypeData(nav_type);
	}

	// Token: 0x060032A9 RID: 12969 RVA: 0x002043AC File Offset: 0x002025AC
	public void SetCurrentNavType(NavType nav_type)
	{
		this.CurrentNavType = nav_type;
		this.AnchorCell = NavTypeHelper.GetAnchorCell(nav_type, Grid.PosToCell(this));
		NavGrid.NavTypeData navTypeData = this.NavGrid.GetNavTypeData(this.CurrentNavType);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		Vector2 one = Vector2.one;
		if (navTypeData.flipX)
		{
			one.x = -1f;
		}
		if (navTypeData.flipY)
		{
			one.y = -1f;
		}
		component.navMatrix = Matrix2x3.Translate(navTypeData.animControllerOffset * 200f) * Matrix2x3.Rotate(navTypeData.rotation) * Matrix2x3.Scale(one);
	}

	// Token: 0x060032AA RID: 12970 RVA: 0x00204450 File Offset: 0x00202650
	private void OnRefreshUserMenu(object data)
	{
		if (base.gameObject.HasTag(GameTags.Dead))
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (NavPathDrawer.Instance.GetNavigator() != this) ? new KIconButtonMenu.ButtonInfo("action_navigable_regions", UI.USERMENUACTIONS.DRAWPATHS.NAME, new System.Action(this.OnDrawPaths), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DRAWPATHS.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_navigable_regions", UI.USERMENUACTIONS.DRAWPATHS.NAME_OFF, new System.Action(this.OnDrawPaths), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DRAWPATHS.TOOLTIP_OFF, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 0.1f);
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_follow_cam", UI.USERMENUACTIONS.FOLLOWCAM.NAME, new System.Action(this.OnFollowCam), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.FOLLOWCAM.TOOLTIP, true), 0.3f);
	}

	// Token: 0x060032AB RID: 12971 RVA: 0x000C1040 File Offset: 0x000BF240
	private void OnFollowCam()
	{
		if (CameraController.Instance.followTarget == base.transform)
		{
			CameraController.Instance.ClearFollowTarget();
			return;
		}
		CameraController.Instance.SetFollowTarget(base.transform);
	}

	// Token: 0x060032AC RID: 12972 RVA: 0x000C1074 File Offset: 0x000BF274
	private void OnDrawPaths()
	{
		if (NavPathDrawer.Instance.GetNavigator() != this)
		{
			NavPathDrawer.Instance.SetNavigator(this);
			return;
		}
		NavPathDrawer.Instance.ClearNavigator();
	}

	// Token: 0x060032AD RID: 12973 RVA: 0x000C109E File Offset: 0x000BF29E
	private void OnSelectObject(object data)
	{
		NavPathDrawer.Instance.ClearNavigator();
	}

	// Token: 0x060032AE RID: 12974 RVA: 0x000C10AA File Offset: 0x000BF2AA
	public void OnStore(object data)
	{
		if (data is Storage || (data != null && (bool)data))
		{
			this.Stop(false, true);
		}
	}

	// Token: 0x060032AF RID: 12975 RVA: 0x000C10CD File Offset: 0x000BF2CD
	public PathFinderAbilities GetCurrentAbilities()
	{
		this.abilities.Refresh();
		return this.abilities;
	}

	// Token: 0x060032B0 RID: 12976 RVA: 0x000C10E0 File Offset: 0x000BF2E0
	public void SetAbilities(PathFinderAbilities abilities)
	{
		this.abilities = abilities;
	}

	// Token: 0x060032B1 RID: 12977 RVA: 0x000C10E9 File Offset: 0x000BF2E9
	public bool CanReach(IApproachable approachable)
	{
		return this.CanReach(approachable.GetCell(), approachable.GetOffsets());
	}

	// Token: 0x060032B2 RID: 12978 RVA: 0x00204554 File Offset: 0x00202754
	public bool CanReach(int cell, CellOffset[] offsets)
	{
		foreach (CellOffset offset in offsets)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			if (this.CanReach(cell2))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060032B3 RID: 12979 RVA: 0x000C10FD File Offset: 0x000BF2FD
	public bool CanReach(int cell)
	{
		return this.GetNavigationCost(cell) != -1;
	}

	// Token: 0x060032B4 RID: 12980 RVA: 0x000C110C File Offset: 0x000BF30C
	public int GetNavigationCost(int cell)
	{
		if (Grid.IsValidCell(cell))
		{
			return this.PathProber.GetCost(cell);
		}
		return -1;
	}

	// Token: 0x060032B5 RID: 12981 RVA: 0x000C1124 File Offset: 0x000BF324
	public int GetNavigationCostIgnoreProberOffset(int cell, CellOffset[] offsets)
	{
		return this.PathProber.GetNavigationCostIgnoreProberOffset(cell, offsets);
	}

	// Token: 0x060032B6 RID: 12982 RVA: 0x00204590 File Offset: 0x00202790
	public int GetNavigationCost(int cell, CellOffset[] offsets)
	{
		int num = -1;
		int num2 = offsets.Length;
		for (int i = 0; i < num2; i++)
		{
			int cell2 = Grid.OffsetCell(cell, offsets[i]);
			int navigationCost = this.GetNavigationCost(cell2);
			if (navigationCost != -1 && (num == -1 || navigationCost < num))
			{
				num = navigationCost;
			}
		}
		return num;
	}

	// Token: 0x060032B7 RID: 12983 RVA: 0x000C1133 File Offset: 0x000BF333
	public int GetNavigationCost(IApproachable approachable)
	{
		return this.GetNavigationCost(approachable.GetCell(), approachable.GetOffsets());
	}

	// Token: 0x060032B8 RID: 12984 RVA: 0x002045D8 File Offset: 0x002027D8
	public void RunQuery(PathFinderQuery query)
	{
		int cell = Grid.PosToCell(this);
		PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(cell, this.CurrentNavType, this.flags);
		PathFinder.Run(this.NavGrid, this.GetCurrentAbilities(), potential_path, query);
	}

	// Token: 0x060032B9 RID: 12985 RVA: 0x000C1147 File Offset: 0x000BF347
	public void SetFlags(PathFinder.PotentialPath.Flags new_flags)
	{
		this.flags |= new_flags;
	}

	// Token: 0x060032BA RID: 12986 RVA: 0x000C1157 File Offset: 0x000BF357
	public void ClearFlags(PathFinder.PotentialPath.Flags new_flags)
	{
		this.flags &= ~new_flags;
	}

	// Token: 0x060032BB RID: 12987 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("ENABLE_DETAILED_NAVIGATOR_PROFILE_INFO")]
	public static void BeginDetailedSample(string region_name)
	{
	}

	// Token: 0x060032BC RID: 12988 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("ENABLE_DETAILED_NAVIGATOR_PROFILE_INFO")]
	public static void EndDetailedSample(string region_name)
	{
	}

	// Token: 0x040021EE RID: 8686
	public bool DebugDrawPath;

	// Token: 0x040021F2 RID: 8690
	[MyCmpAdd]
	public PathProber PathProber;

	// Token: 0x040021F3 RID: 8691
	[MyCmpAdd]
	public Facing facing;

	// Token: 0x040021F4 RID: 8692
	public float defaultSpeed = 1f;

	// Token: 0x040021F5 RID: 8693
	public TransitionDriver transitionDriver;

	// Token: 0x040021F6 RID: 8694
	public string NavGridName;

	// Token: 0x040021F7 RID: 8695
	public bool updateProber;

	// Token: 0x040021F8 RID: 8696
	public int maxProbingRadius;

	// Token: 0x040021F9 RID: 8697
	public PathFinder.PotentialPath.Flags flags;

	// Token: 0x040021FA RID: 8698
	private LoggerFSS log;

	// Token: 0x040021FB RID: 8699
	public Dictionary<NavType, int> distanceTravelledByNavType;

	// Token: 0x040021FC RID: 8700
	public Grid.SceneLayer sceneLayer = Grid.SceneLayer.Move;

	// Token: 0x040021FD RID: 8701
	private PathFinderAbilities abilities;

	// Token: 0x040021FE RID: 8702
	[MyCmpReq]
	public KBatchedAnimController animController;

	// Token: 0x040021FF RID: 8703
	[NonSerialized]
	public PathFinder.Path path;

	// Token: 0x04002200 RID: 8704
	public NavType CurrentNavType;

	// Token: 0x04002201 RID: 8705
	private int AnchorCell;

	// Token: 0x04002202 RID: 8706
	private KPrefabID targetLocator;

	// Token: 0x04002203 RID: 8707
	private int reservedCell = NavigationReservations.InvalidReservation;

	// Token: 0x04002204 RID: 8708
	private NavTactic tactic;

	// Token: 0x04002205 RID: 8709
	public Navigator.PathProbeTask pathProbeTask;

	// Token: 0x04002206 RID: 8710
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnDefeatedDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnDefeated(data);
	});

	// Token: 0x04002207 RID: 8711
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04002208 RID: 8712
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnSelectObjectDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnSelectObject(data);
	});

	// Token: 0x04002209 RID: 8713
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnStoreDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x0400220A RID: 8714
	public bool executePathProbeTaskAsync;

	// Token: 0x02000AA3 RID: 2723
	public class ActiveTransition
	{
		// Token: 0x060032BF RID: 12991 RVA: 0x0020468C File Offset: 0x0020288C
		public void Init(NavGrid.Transition transition, float default_speed)
		{
			this.x = transition.x;
			this.y = transition.y;
			this.isLooping = transition.isLooping;
			this.start = transition.start;
			this.end = transition.end;
			this.preAnim = transition.preAnim;
			this.anim = transition.anim;
			this.speed = default_speed;
			this.animSpeed = transition.animSpeed;
			this.navGridTransition = transition;
		}

		// Token: 0x060032C0 RID: 12992 RVA: 0x00204714 File Offset: 0x00202914
		public void Copy(Navigator.ActiveTransition other)
		{
			this.x = other.x;
			this.y = other.y;
			this.isLooping = other.isLooping;
			this.start = other.start;
			this.end = other.end;
			this.preAnim = other.preAnim;
			this.anim = other.anim;
			this.speed = other.speed;
			this.animSpeed = other.animSpeed;
			this.navGridTransition = other.navGridTransition;
		}

		// Token: 0x0400220B RID: 8715
		public int x;

		// Token: 0x0400220C RID: 8716
		public int y;

		// Token: 0x0400220D RID: 8717
		public bool isLooping;

		// Token: 0x0400220E RID: 8718
		public NavType start;

		// Token: 0x0400220F RID: 8719
		public NavType end;

		// Token: 0x04002210 RID: 8720
		public HashedString preAnim;

		// Token: 0x04002211 RID: 8721
		public HashedString anim;

		// Token: 0x04002212 RID: 8722
		public float speed;

		// Token: 0x04002213 RID: 8723
		public float animSpeed = 1f;

		// Token: 0x04002214 RID: 8724
		public Func<bool> isCompleteCB;

		// Token: 0x04002215 RID: 8725
		public NavGrid.Transition navGridTransition;
	}

	// Token: 0x02000AA4 RID: 2724
	public class StatesInstance : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.GameInstance
	{
		// Token: 0x060032C2 RID: 12994 RVA: 0x000C11A2 File Offset: 0x000BF3A2
		public StatesInstance(Navigator master) : base(master)
		{
		}
	}

	// Token: 0x02000AA5 RID: 2725
	public class States : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator>
	{
		// Token: 0x060032C3 RID: 12995 RVA: 0x0020479C File Offset: 0x0020299C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.normal.stopped;
			this.saveHistory = true;
			this.normal.ParamTransition<bool>(this.isPaused, this.paused, GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.IsTrue).Update("NavigatorProber", delegate(Navigator.StatesInstance smi, float dt)
			{
				smi.master.Sim4000ms(dt);
			}, UpdateRate.SIM_4000ms, false);
			this.normal.moving.Enter(delegate(Navigator.StatesInstance smi)
			{
				smi.Trigger(1027377649, GameHashes.ObjectMovementWakeUp);
			}).Update("UpdateNavigator", delegate(Navigator.StatesInstance smi, float dt)
			{
				smi.master.SimEveryTick(dt);
			}, UpdateRate.SIM_EVERY_TICK, true).Exit(delegate(Navigator.StatesInstance smi)
			{
				smi.Trigger(1027377649, GameHashes.ObjectMovementSleep);
			});
			this.normal.arrived.TriggerOnEnter(GameHashes.DestinationReached, null).GoTo(this.normal.stopped);
			this.normal.failed.TriggerOnEnter(GameHashes.NavigationFailed, null).GoTo(this.normal.stopped);
			this.normal.stopped.Enter(delegate(Navigator.StatesInstance smi)
			{
				smi.master.SubscribeUnstuckFunctions();
			}).DoNothing().Exit(delegate(Navigator.StatesInstance smi)
			{
				smi.master.UnsubscribeUnstuckFunctions();
			});
			this.paused.ParamTransition<bool>(this.isPaused, this.normal, GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.IsFalse);
		}

		// Token: 0x04002216 RID: 8726
		public StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.TargetParameter moveTarget;

		// Token: 0x04002217 RID: 8727
		public StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.BoolParameter isPaused = new StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.BoolParameter(false);

		// Token: 0x04002218 RID: 8728
		public Navigator.States.NormalStates normal;

		// Token: 0x04002219 RID: 8729
		public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State paused;

		// Token: 0x02000AA6 RID: 2726
		public class NormalStates : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State
		{
			// Token: 0x0400221A RID: 8730
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State moving;

			// Token: 0x0400221B RID: 8731
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State arrived;

			// Token: 0x0400221C RID: 8732
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State failed;

			// Token: 0x0400221D RID: 8733
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State stopped;
		}
	}

	// Token: 0x02000AA8 RID: 2728
	public struct PathProbeTask : IWorkItem<object>
	{
		// Token: 0x060032CE RID: 13006 RVA: 0x000C1237 File Offset: 0x000BF437
		public PathProbeTask(Navigator navigator)
		{
			this.navigator = navigator;
			this.cell = -1;
		}

		// Token: 0x060032CF RID: 13007 RVA: 0x000C1247 File Offset: 0x000BF447
		public void Update()
		{
			this.cell = Grid.PosToCell(this.navigator);
			this.navigator.abilities.Refresh();
		}

		// Token: 0x060032D0 RID: 13008 RVA: 0x00204948 File Offset: 0x00202B48
		public void Run(object sharedData)
		{
			this.navigator.PathProber.UpdateProbe(this.navigator.NavGrid, this.cell, this.navigator.CurrentNavType, this.navigator.abilities, this.navigator.flags);
		}

		// Token: 0x04002225 RID: 8741
		private int cell;

		// Token: 0x04002226 RID: 8742
		private Navigator navigator;
	}
}
