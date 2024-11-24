using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000D6D RID: 3437
public class FishFeeder : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>
{
	// Token: 0x0600435E RID: 17246 RVA: 0x00244890 File Offset: 0x00242A90
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.notoperational;
		this.root.Enter(new StateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State.Callback(FishFeeder.SetupFishFeederTopAndBot)).Exit(new StateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State.Callback(FishFeeder.CleanupFishFeederTopAndBot)).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameEvent.Callback(FishFeeder.OnStorageChange)).EventHandler(GameHashes.RefreshUserMenu, new GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameEvent.Callback(FishFeeder.OnRefreshUserMenu));
		this.notoperational.TagTransition(GameTags.Operational, this.operational, false);
		this.operational.DefaultState(this.operational.on).TagTransition(GameTags.Operational, this.notoperational, true);
		this.operational.on.DoNothing();
		int num = 19;
		FishFeeder.ballSymbols = new HashedString[num];
		for (int i = 0; i < num; i++)
		{
			FishFeeder.ballSymbols[i] = "ball" + i.ToString();
		}
	}

	// Token: 0x0600435F RID: 17247 RVA: 0x00244988 File Offset: 0x00242B88
	private static void SetupFishFeederTopAndBot(FishFeeder.Instance smi)
	{
		Storage storage = smi.Get<Storage>();
		smi.fishFeederTop = new FishFeeder.FishFeederTop(smi, FishFeeder.ballSymbols, storage.Capacity());
		smi.fishFeederTop.RefreshStorage();
		smi.fishFeederBot = new FishFeeder.FishFeederBot(smi, 10f, FishFeeder.ballSymbols);
		smi.fishFeederBot.RefreshStorage();
		smi.fishFeederTop.ToggleMutantSeedFetches(smi.ForbidMutantSeeds);
		smi.UpdateMutantSeedStatusItem();
	}

	// Token: 0x06004360 RID: 17248 RVA: 0x000CB875 File Offset: 0x000C9A75
	private static void CleanupFishFeederTopAndBot(FishFeeder.Instance smi)
	{
		smi.fishFeederTop.Cleanup();
	}

	// Token: 0x06004361 RID: 17249 RVA: 0x002449F8 File Offset: 0x00242BF8
	private static void MoveStoredContentsToConsumeOffset(FishFeeder.Instance smi)
	{
		foreach (GameObject gameObject in smi.GetComponent<Storage>().items)
		{
			if (!(gameObject == null))
			{
				FishFeeder.OnStorageChange(smi, gameObject);
			}
		}
	}

	// Token: 0x06004362 RID: 17250 RVA: 0x000CB882 File Offset: 0x000C9A82
	private static void OnStorageChange(FishFeeder.Instance smi, object data)
	{
		if ((GameObject)data == null)
		{
			return;
		}
		smi.fishFeederTop.RefreshStorage();
		smi.fishFeederBot.RefreshStorage();
	}

	// Token: 0x06004363 RID: 17251 RVA: 0x00244A5C File Offset: 0x00242C5C
	private static void OnRefreshUserMenu(FishFeeder.Instance smi, object data)
	{
		if (DlcManager.FeatureRadiationEnabled())
		{
			Game.Instance.userMenu.AddButton(smi.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", smi.ForbidMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT, delegate()
			{
				smi.ForbidMutantSeeds = !smi.ForbidMutantSeeds;
				FishFeeder.OnRefreshUserMenu(smi, null);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.FISH_FEEDER_TOOLTIP, true), 1f);
		}
	}

	// Token: 0x04002E1F RID: 11807
	public GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State notoperational;

	// Token: 0x04002E20 RID: 11808
	public FishFeeder.OperationalState operational;

	// Token: 0x04002E21 RID: 11809
	public static HashedString[] ballSymbols;

	// Token: 0x02000D6E RID: 3438
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000D6F RID: 3439
	public class OperationalState : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State
	{
		// Token: 0x04002E22 RID: 11810
		public GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State on;
	}

	// Token: 0x02000D70 RID: 3440
	public new class Instance : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameInstance
	{
		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06004367 RID: 17255 RVA: 0x000CB8B9 File Offset: 0x000C9AB9
		// (set) Token: 0x06004368 RID: 17256 RVA: 0x000CB8C1 File Offset: 0x000C9AC1
		public bool ForbidMutantSeeds
		{
			get
			{
				return this.forbidMutantSeeds;
			}
			set
			{
				this.forbidMutantSeeds = value;
				this.fishFeederTop.ToggleMutantSeedFetches(this.forbidMutantSeeds);
				this.UpdateMutantSeedStatusItem();
			}
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x00244AE4 File Offset: 0x00242CE4
		public Instance(IStateMachineTarget master, FishFeeder.Def def) : base(master, def)
		{
			this.mutantSeedStatusItem = new StatusItem("FISHFEEDERACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettingsDelegate));
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x00244B3C File Offset: 0x00242D3C
		private void OnCopySettingsDelegate(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject == null)
			{
				return;
			}
			FishFeeder.Instance smi = gameObject.GetSMI<FishFeeder.Instance>();
			if (smi == null)
			{
				return;
			}
			this.ForbidMutantSeeds = smi.ForbidMutantSeeds;
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x000CB8E1 File Offset: 0x000C9AE1
		public void UpdateMutantSeedStatusItem()
		{
			base.gameObject.GetComponent<KSelectable>().ToggleStatusItem(this.mutantSeedStatusItem, SaveLoader.Instance.IsDLCActiveForCurrentSave("EXPANSION1_ID") && !this.forbidMutantSeeds, null);
		}

		// Token: 0x04002E23 RID: 11811
		private StatusItem mutantSeedStatusItem;

		// Token: 0x04002E24 RID: 11812
		public FishFeeder.FishFeederTop fishFeederTop;

		// Token: 0x04002E25 RID: 11813
		public FishFeeder.FishFeederBot fishFeederBot;

		// Token: 0x04002E26 RID: 11814
		[Serialize]
		private bool forbidMutantSeeds;
	}

	// Token: 0x02000D71 RID: 3441
	public class FishFeederTop : IRenderEveryTick
	{
		// Token: 0x0600436C RID: 17260 RVA: 0x000CB918 File Offset: 0x000C9B18
		public FishFeederTop(FishFeeder.Instance smi, HashedString[] ball_symbols, float capacity)
		{
			this.smi = smi;
			this.ballSymbols = ball_symbols;
			this.massPerBall = capacity / (float)ball_symbols.Length;
			this.FillFeeder(this.mass);
			SimAndRenderScheduler.instance.Add(this, false);
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x00244B74 File Offset: 0x00242D74
		private void FillFeeder(float mass)
		{
			KBatchedAnimController component = this.smi.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < this.ballSymbols.Length; i++)
			{
				bool is_visible = mass > (float)(i + 1) * this.massPerBall;
				component.SetSymbolVisiblity(this.ballSymbols[i], is_visible);
			}
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x00244BC8 File Offset: 0x00242DC8
		public void RefreshStorage()
		{
			float num = 0f;
			foreach (GameObject gameObject in this.smi.GetComponent<Storage>().items)
			{
				if (!(gameObject == null))
				{
					num += gameObject.GetComponent<PrimaryElement>().Mass;
				}
			}
			this.targetMass = num;
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x00244C44 File Offset: 0x00242E44
		public void RenderEveryTick(float dt)
		{
			this.timeSinceLastBallAppeared += dt;
			if (this.targetMass > this.mass && this.timeSinceLastBallAppeared > 0.025f)
			{
				float num = Mathf.Min(this.massPerBall, this.targetMass - this.mass);
				this.mass += num;
				this.FillFeeder(this.mass);
				this.timeSinceLastBallAppeared = 0f;
			}
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x000C09DD File Offset: 0x000BEBDD
		public void Cleanup()
		{
			SimAndRenderScheduler.instance.Remove(this);
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x00244CB8 File Offset: 0x00242EB8
		public void ToggleMutantSeedFetches(bool allow)
		{
			StorageLocker component = this.smi.GetComponent<StorageLocker>();
			if (component != null)
			{
				component.UpdateForbiddenTag(GameTags.MutatedSeed, !allow);
			}
		}

		// Token: 0x04002E27 RID: 11815
		private FishFeeder.Instance smi;

		// Token: 0x04002E28 RID: 11816
		private float mass;

		// Token: 0x04002E29 RID: 11817
		private float targetMass;

		// Token: 0x04002E2A RID: 11818
		private HashedString[] ballSymbols;

		// Token: 0x04002E2B RID: 11819
		private float massPerBall;

		// Token: 0x04002E2C RID: 11820
		private float timeSinceLastBallAppeared;
	}

	// Token: 0x02000D72 RID: 3442
	public class FishFeederBot
	{
		// Token: 0x06004372 RID: 17266 RVA: 0x00244CEC File Offset: 0x00242EEC
		public FishFeederBot(FishFeeder.Instance smi, float mass_per_ball, HashedString[] ball_symbols)
		{
			this.smi = smi;
			this.massPerBall = mass_per_ball;
			this.anim = GameUtil.KInstantiate(Assets.GetPrefab("FishFeederBot"), smi.transform.GetPosition(), Grid.SceneLayer.Front, null, 0).GetComponent<KBatchedAnimController>();
			this.anim.transform.SetParent(smi.transform);
			this.anim.gameObject.SetActive(true);
			this.anim.SetSceneLayer(Grid.SceneLayer.Building);
			this.anim.Play("ball", KAnim.PlayMode.Once, 1f, 0f);
			this.anim.Stop();
			foreach (HashedString hash in ball_symbols)
			{
				this.anim.SetSymbolVisiblity(hash, false);
			}
			Storage[] components = smi.gameObject.GetComponents<Storage>();
			this.topStorage = components[0];
			this.botStorage = components[1];
			if (!this.botStorage.IsEmpty())
			{
				this.SetBallSymbol(this.botStorage.items[0].gameObject);
				this.anim.Play("ball", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x00244E2C File Offset: 0x0024302C
		public void RefreshStorage()
		{
			if (this.refreshingStorage)
			{
				return;
			}
			this.refreshingStorage = true;
			foreach (GameObject gameObject in this.botStorage.items)
			{
				if (!(gameObject == null))
				{
					int cell = Grid.CellBelow(Grid.CellBelow(Grid.PosToCell(this.smi.transform.GetPosition())));
					gameObject.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore));
				}
			}
			if (this.botStorage.IsEmpty())
			{
				float num = 0f;
				foreach (GameObject gameObject2 in this.topStorage.items)
				{
					if (!(gameObject2 == null))
					{
						num += gameObject2.GetComponent<PrimaryElement>().Mass;
					}
				}
				if (num > 0f)
				{
					Pickupable pickupable = this.topStorage.items[0].GetComponent<Pickupable>().Take(this.massPerBall);
					this.SetBallSymbol(pickupable.gameObject);
					this.anim.Play("ball", KAnim.PlayMode.Once, 1f, 0f);
					this.botStorage.Store(pickupable.gameObject, false, false, true, false);
				}
				else
				{
					this.anim.SetSymbolVisiblity(FishFeeder.FishFeederBot.HASH_FEEDBALL, false);
				}
			}
			this.refreshingStorage = false;
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x00244FC8 File Offset: 0x002431C8
		private void SetBallSymbol(GameObject stored_go)
		{
			if (stored_go == null)
			{
				return;
			}
			this.anim.SetSymbolVisiblity(FishFeeder.FishFeederBot.HASH_FEEDBALL, true);
			KAnim.Build build = stored_go.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build;
			KAnim.Build.Symbol symbol = stored_go.HasTag(GameTags.Seed) ? build.GetSymbol("object") : build.GetSymbol("algae");
			if (symbol != null)
			{
				this.anim.GetComponent<SymbolOverrideController>().AddSymbolOverride(FishFeeder.FishFeederBot.HASH_FEEDBALL, symbol, 0);
			}
			HashedString batchGroupOverride = new HashedString("FishFeeder" + stored_go.GetComponent<KPrefabID>().PrefabTag.Name);
			this.anim.SetBatchGroupOverride(batchGroupOverride);
			int cell = Grid.CellBelow(Grid.CellBelow(Grid.PosToCell(this.smi.transform.GetPosition())));
			stored_go.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingUse));
		}

		// Token: 0x04002E2D RID: 11821
		private KBatchedAnimController anim;

		// Token: 0x04002E2E RID: 11822
		private Storage topStorage;

		// Token: 0x04002E2F RID: 11823
		private Storage botStorage;

		// Token: 0x04002E30 RID: 11824
		private bool refreshingStorage;

		// Token: 0x04002E31 RID: 11825
		private FishFeeder.Instance smi;

		// Token: 0x04002E32 RID: 11826
		private float massPerBall;

		// Token: 0x04002E33 RID: 11827
		private static readonly HashedString HASH_FEEDBALL = "feedball";
	}
}
