using KSerialization;
using STRINGS;
using UnityEngine;

public class FishFeeder : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>
{
	public class Def : BaseDef
	{
	}

	public class OperationalState : State
	{
		public State on;
	}

	public new class Instance : GameInstance
	{
		private StatusItem mutantSeedStatusItem;

		public FishFeederTop fishFeederTop;

		public FishFeederBot fishFeederBot;

		[Serialize]
		private bool forbidMutantSeeds;

		public bool ForbidMutantSeeds
		{
			get
			{
				return forbidMutantSeeds;
			}
			set
			{
				forbidMutantSeeds = value;
				fishFeederTop.ToggleMutantSeedFetches(forbidMutantSeeds);
				UpdateMutantSeedStatusItem();
			}
		}

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			mutantSeedStatusItem = new StatusItem("FISHFEEDERACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);
			Subscribe(-905833192, OnCopySettingsDelegate);
		}

		private void OnCopySettingsDelegate(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (!(gameObject == null))
			{
				Instance sMI = gameObject.GetSMI<Instance>();
				if (sMI != null)
				{
					ForbidMutantSeeds = sMI.ForbidMutantSeeds;
				}
			}
		}

		public void UpdateMutantSeedStatusItem()
		{
			base.gameObject.GetComponent<KSelectable>().ToggleStatusItem(mutantSeedStatusItem, SaveLoader.Instance.IsDLCActiveForCurrentSave("EXPANSION1_ID") && !forbidMutantSeeds);
		}
	}

	public class FishFeederTop : IRenderEveryTick
	{
		private Instance smi;

		private float mass;

		private float targetMass;

		private HashedString[] ballSymbols;

		private float massPerBall;

		private float timeSinceLastBallAppeared;

		public FishFeederTop(Instance smi, HashedString[] ball_symbols, float capacity)
		{
			this.smi = smi;
			ballSymbols = ball_symbols;
			massPerBall = capacity / (float)ball_symbols.Length;
			FillFeeder(mass);
			SimAndRenderScheduler.instance.Add(this);
		}

		private void FillFeeder(float mass)
		{
			KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < ballSymbols.Length; i++)
			{
				bool is_visible = mass > (float)(i + 1) * massPerBall;
				component.SetSymbolVisiblity(ballSymbols[i], is_visible);
			}
		}

		public void RefreshStorage()
		{
			float num = 0f;
			foreach (GameObject item in smi.GetComponent<Storage>().items)
			{
				if (!(item == null))
				{
					num += item.GetComponent<PrimaryElement>().Mass;
				}
			}
			targetMass = num;
		}

		public void RenderEveryTick(float dt)
		{
			timeSinceLastBallAppeared += dt;
			if (targetMass > mass && timeSinceLastBallAppeared > 0.025f)
			{
				float num = Mathf.Min(massPerBall, targetMass - mass);
				mass += num;
				FillFeeder(mass);
				timeSinceLastBallAppeared = 0f;
			}
		}

		public void Cleanup()
		{
			SimAndRenderScheduler.instance.Remove(this);
		}

		public void ToggleMutantSeedFetches(bool allow)
		{
			StorageLocker component = smi.GetComponent<StorageLocker>();
			if (component != null)
			{
				component.UpdateForbiddenTag(GameTags.MutatedSeed, !allow);
			}
		}
	}

	public class FishFeederBot
	{
		private KBatchedAnimController anim;

		private Storage topStorage;

		private Storage botStorage;

		private bool refreshingStorage;

		private Instance smi;

		private float massPerBall;

		private static readonly HashedString HASH_FEEDBALL = "feedball";

		public FishFeederBot(Instance smi, float mass_per_ball, HashedString[] ball_symbols)
		{
			this.smi = smi;
			massPerBall = mass_per_ball;
			anim = GameUtil.KInstantiate(Assets.GetPrefab("FishFeederBot"), smi.transform.GetPosition(), Grid.SceneLayer.Front).GetComponent<KBatchedAnimController>();
			anim.transform.SetParent(smi.transform);
			anim.gameObject.SetActive(value: true);
			anim.SetSceneLayer(Grid.SceneLayer.Building);
			anim.Play("ball");
			anim.Stop();
			foreach (HashedString hashedString in ball_symbols)
			{
				anim.SetSymbolVisiblity(hashedString, is_visible: false);
			}
			Storage[] components = smi.gameObject.GetComponents<Storage>();
			topStorage = components[0];
			botStorage = components[1];
			if (!botStorage.IsEmpty())
			{
				SetBallSymbol(botStorage.items[0].gameObject);
				anim.Play("ball");
			}
		}

		public void RefreshStorage()
		{
			if (refreshingStorage)
			{
				return;
			}
			refreshingStorage = true;
			foreach (GameObject item in botStorage.items)
			{
				if (!(item == null))
				{
					int cell = Grid.CellBelow(Grid.CellBelow(Grid.PosToCell(smi.transform.GetPosition())));
					item.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore));
				}
			}
			if (botStorage.IsEmpty())
			{
				float num = 0f;
				foreach (GameObject item2 in topStorage.items)
				{
					if (!(item2 == null))
					{
						num += item2.GetComponent<PrimaryElement>().Mass;
					}
				}
				if (num > 0f)
				{
					Pickupable pickupable = topStorage.items[0].GetComponent<Pickupable>().Take(massPerBall);
					SetBallSymbol(pickupable.gameObject);
					anim.Play("ball");
					botStorage.Store(pickupable.gameObject);
				}
				else
				{
					anim.SetSymbolVisiblity(HASH_FEEDBALL, is_visible: false);
				}
			}
			refreshingStorage = false;
		}

		private void SetBallSymbol(GameObject stored_go)
		{
			if (!(stored_go == null))
			{
				anim.SetSymbolVisiblity(HASH_FEEDBALL, is_visible: true);
				KAnim.Build build = stored_go.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build;
				KAnim.Build.Symbol symbol = (stored_go.HasTag(GameTags.Seed) ? build.GetSymbol("object") : build.GetSymbol("algae"));
				if (symbol != null)
				{
					anim.GetComponent<SymbolOverrideController>().AddSymbolOverride(HASH_FEEDBALL, symbol);
				}
				HashedString batchGroupOverride = new HashedString("FishFeeder" + stored_go.GetComponent<KPrefabID>().PrefabTag.Name);
				anim.SetBatchGroupOverride(batchGroupOverride);
				int cell = Grid.CellBelow(Grid.CellBelow(Grid.PosToCell(smi.transform.GetPosition())));
				stored_go.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingUse));
			}
		}
	}

	public State notoperational;

	public OperationalState operational;

	public static HashedString[] ballSymbols;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = notoperational;
		root.Enter(SetupFishFeederTopAndBot).Exit(CleanupFishFeederTopAndBot).EventHandler(GameHashes.OnStorageChange, OnStorageChange)
			.EventHandler(GameHashes.RefreshUserMenu, OnRefreshUserMenu);
		notoperational.TagTransition(GameTags.Operational, operational);
		operational.DefaultState(operational.on).TagTransition(GameTags.Operational, notoperational, on_remove: true);
		operational.on.DoNothing();
		int num = 19;
		ballSymbols = new HashedString[num];
		for (int i = 0; i < num; i++)
		{
			ballSymbols[i] = "ball" + i;
		}
	}

	private static void SetupFishFeederTopAndBot(Instance smi)
	{
		Storage storage = smi.Get<Storage>();
		smi.fishFeederTop = new FishFeederTop(smi, ballSymbols, storage.Capacity());
		smi.fishFeederTop.RefreshStorage();
		smi.fishFeederBot = new FishFeederBot(smi, 10f, ballSymbols);
		smi.fishFeederBot.RefreshStorage();
		smi.fishFeederTop.ToggleMutantSeedFetches(smi.ForbidMutantSeeds);
		smi.UpdateMutantSeedStatusItem();
	}

	private static void CleanupFishFeederTopAndBot(Instance smi)
	{
		smi.fishFeederTop.Cleanup();
	}

	private static void MoveStoredContentsToConsumeOffset(Instance smi)
	{
		foreach (GameObject item in smi.GetComponent<Storage>().items)
		{
			if (!(item == null))
			{
				OnStorageChange(smi, item);
			}
		}
	}

	private static void OnStorageChange(Instance smi, object data)
	{
		if (!((GameObject)data == null))
		{
			smi.fishFeederTop.RefreshStorage();
			smi.fishFeederBot.RefreshStorage();
		}
	}

	private static void OnRefreshUserMenu(Instance smi, object data)
	{
		if (DlcManager.FeatureRadiationEnabled())
		{
			Game.Instance.userMenu.AddButton(smi.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", smi.ForbidMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT, delegate
			{
				smi.ForbidMutantSeeds = !smi.ForbidMutantSeeds;
				OnRefreshUserMenu(smi, null);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.FISH_FEEDER_TOOLTIP));
		}
	}
}
