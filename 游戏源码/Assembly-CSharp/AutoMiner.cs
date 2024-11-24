using System;
using FMODUnity;
using KSerialization;
using UnityEngine;

// Token: 0x02000C9B RID: 3227
[SerializationConfig(MemberSerialization.OptIn)]
public class AutoMiner : StateMachineComponent<AutoMiner.Instance>, ISim1000ms
{
	// Token: 0x170002DA RID: 730
	// (get) Token: 0x06003E21 RID: 15905 RVA: 0x000C85F2 File Offset: 0x000C67F2
	private bool HasDigCell
	{
		get
		{
			return this.dig_cell != Grid.InvalidCell;
		}
	}

	// Token: 0x170002DB RID: 731
	// (get) Token: 0x06003E22 RID: 15906 RVA: 0x000C8604 File Offset: 0x000C6804
	private bool RotationComplete
	{
		get
		{
			return this.HasDigCell && this.rotation_complete;
		}
	}

	// Token: 0x06003E23 RID: 15907 RVA: 0x000C8616 File Offset: 0x000C6816
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.simRenderLoadBalance = true;
	}

	// Token: 0x06003E24 RID: 15908 RVA: 0x002336E8 File Offset: 0x002318E8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.hitEffectPrefab = Assets.GetPrefab("fx_dig_splash");
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		string name = component.name + ".gun";
		this.arm_go = new GameObject(name);
		this.arm_go.SetActive(false);
		this.arm_go.transform.parent = component.transform;
		this.looping_sounds = this.arm_go.AddComponent<LoopingSounds>();
		string sound = GlobalAssets.GetSound(this.rotateSoundName, false);
		this.rotateSound = RuntimeManager.PathToEventReference(sound);
		this.arm_go.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
		this.arm_anim_ctrl = this.arm_go.AddComponent<KBatchedAnimController>();
		this.arm_anim_ctrl.AnimFiles = new KAnimFile[]
		{
			component.AnimFiles[0]
		};
		this.arm_anim_ctrl.initialAnim = "gun";
		this.arm_anim_ctrl.isMovable = true;
		this.arm_anim_ctrl.sceneLayer = Grid.SceneLayer.TransferArm;
		component.SetSymbolVisiblity("gun_target", false);
		bool flag;
		Vector3 position = component.GetSymbolTransform(new HashedString("gun_target"), out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
		this.arm_go.transform.SetPosition(position);
		this.arm_go.SetActive(true);
		this.link = new KAnimLink(component, this.arm_anim_ctrl);
		base.Subscribe<AutoMiner>(-592767678, AutoMiner.OnOperationalChangedDelegate);
		this.RotateArm(this.rotatable.GetRotatedOffset(Quaternion.Euler(0f, 0f, -45f) * Vector3.up), true, 0f);
		this.StopDig();
		base.smi.StartSM();
	}

	// Token: 0x06003E25 RID: 15909 RVA: 0x000C8625 File Offset: 0x000C6825
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003E26 RID: 15910 RVA: 0x000C862D File Offset: 0x000C682D
	public void Sim1000ms(float dt)
	{
		if (!this.operational.IsOperational)
		{
			return;
		}
		this.RefreshDiggableCell();
		this.operational.SetActive(this.HasDigCell, false);
	}

	// Token: 0x06003E27 RID: 15911 RVA: 0x000C8655 File Offset: 0x000C6855
	private void OnOperationalChanged(object data)
	{
		if (!(bool)data)
		{
			this.dig_cell = Grid.InvalidCell;
			this.rotation_complete = false;
		}
	}

	// Token: 0x06003E28 RID: 15912 RVA: 0x002338B8 File Offset: 0x00231AB8
	public void UpdateRotation(float dt)
	{
		if (this.HasDigCell)
		{
			Vector3 a = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.TileMain);
			a.z = 0f;
			Vector3 position = this.arm_go.transform.GetPosition();
			position.z = 0f;
			Vector3 target_dir = Vector3.Normalize(a - position);
			this.RotateArm(target_dir, false, dt);
		}
	}

	// Token: 0x06003E29 RID: 15913 RVA: 0x000C8671 File Offset: 0x000C6871
	private Element GetTargetElement()
	{
		if (this.HasDigCell)
		{
			return Grid.Element[this.dig_cell];
		}
		return null;
	}

	// Token: 0x06003E2A RID: 15914 RVA: 0x0023391C File Offset: 0x00231B1C
	public void StartDig()
	{
		Element targetElement = this.GetTargetElement();
		base.Trigger(-1762453998, targetElement);
		this.CreateHitEffect();
		this.arm_anim_ctrl.Play("gun_digging", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06003E2B RID: 15915 RVA: 0x000C8689 File Offset: 0x000C6889
	public void StopDig()
	{
		base.Trigger(939543986, null);
		this.DestroyHitEffect();
		this.arm_anim_ctrl.Play("gun", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06003E2C RID: 15916 RVA: 0x00233964 File Offset: 0x00231B64
	public void UpdateDig(float dt)
	{
		if (!this.HasDigCell)
		{
			return;
		}
		if (!this.rotation_complete)
		{
			return;
		}
		Diggable.DoDigTick(this.dig_cell, dt, WorldDamage.DamageType.NoBuildingDamage);
		float percentComplete = Grid.Damage[this.dig_cell];
		this.mining_sounds.SetPercentComplete(percentComplete);
		Vector3 a = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.FXFront2);
		a.z = 0f;
		Vector3 position = this.arm_go.transform.GetPosition();
		position.z = 0f;
		float sqrMagnitude = (a - position).sqrMagnitude;
		this.arm_anim_ctrl.GetBatchInstanceData().SetClipRadius(position.x, position.y, sqrMagnitude, true);
		if (!AutoMiner.ValidDigCell(this.dig_cell))
		{
			this.dig_cell = Grid.InvalidCell;
			this.rotation_complete = false;
		}
	}

	// Token: 0x06003E2D RID: 15917 RVA: 0x00233A30 File Offset: 0x00231C30
	private void CreateHitEffect()
	{
		if (this.hitEffectPrefab == null)
		{
			return;
		}
		if (this.hitEffect != null)
		{
			this.DestroyHitEffect();
		}
		Vector3 position = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.FXFront2);
		this.hitEffect = GameUtil.KInstantiate(this.hitEffectPrefab, position, Grid.SceneLayer.FXFront2, null, 0);
		this.hitEffect.SetActive(true);
		KBatchedAnimController component = this.hitEffect.GetComponent<KBatchedAnimController>();
		component.sceneLayer = Grid.SceneLayer.FXFront2;
		component.initialMode = KAnim.PlayMode.Loop;
		component.enabled = false;
		component.enabled = true;
	}

	// Token: 0x06003E2E RID: 15918 RVA: 0x000C86BD File Offset: 0x000C68BD
	private void DestroyHitEffect()
	{
		if (this.hitEffectPrefab == null)
		{
			return;
		}
		if (this.hitEffect != null)
		{
			this.hitEffect.DeleteObject();
			this.hitEffect = null;
		}
	}

	// Token: 0x06003E2F RID: 15919 RVA: 0x00233AB8 File Offset: 0x00231CB8
	private void RefreshDiggableCell()
	{
		CellOffset rotatedCellOffset = this.vision_offset;
		if (this.rotatable)
		{
			rotatedCellOffset = this.rotatable.GetRotatedCellOffset(this.vision_offset);
		}
		int cell = Grid.PosToCell(base.transform.gameObject);
		int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
		int num;
		int num2;
		Grid.CellToXY(cell2, out num, out num2);
		float num3 = float.MaxValue;
		int num4 = Grid.InvalidCell;
		Vector3 a = Grid.CellToPos(cell2);
		bool flag = false;
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				CellOffset rotatedCellOffset2 = new CellOffset(this.x + j, this.y + i);
				if (this.rotatable)
				{
					rotatedCellOffset2 = this.rotatable.GetRotatedCellOffset(rotatedCellOffset2);
				}
				int num5 = Grid.OffsetCell(cell, rotatedCellOffset2);
				if (Grid.IsValidCell(num5))
				{
					int x;
					int y;
					Grid.CellToXY(num5, out x, out y);
					if (Grid.IsValidCell(num5) && AutoMiner.ValidDigCell(num5) && Grid.TestLineOfSight(num, num2, x, y, new Func<int, bool>(AutoMiner.DigBlockingCB), false, false))
					{
						if (num5 == this.dig_cell)
						{
							flag = true;
						}
						Vector3 b = Grid.CellToPos(num5);
						float num6 = Vector3.Distance(a, b);
						if (num6 < num3)
						{
							num3 = num6;
							num4 = num5;
						}
					}
				}
			}
		}
		if (!flag && this.dig_cell != num4)
		{
			this.dig_cell = num4;
			this.rotation_complete = false;
		}
	}

	// Token: 0x06003E30 RID: 15920 RVA: 0x00233C28 File Offset: 0x00231E28
	private static bool ValidDigCell(int cell)
	{
		bool flag = Grid.HasDoor[cell] && Grid.Foundation[cell] && Grid.ObjectLayers[9].ContainsKey(cell);
		if (flag)
		{
			Door component = Grid.ObjectLayers[9][cell].GetComponent<Door>();
			flag = (component != null && component.IsOpen() && !component.IsPendingClose());
		}
		return Grid.Solid[cell] && (!Grid.Foundation[cell] || flag) && Grid.Element[cell].hardness < 150;
	}

	// Token: 0x06003E31 RID: 15921 RVA: 0x00233CD0 File Offset: 0x00231ED0
	public static bool DigBlockingCB(int cell)
	{
		bool flag = Grid.HasDoor[cell] && Grid.Foundation[cell] && Grid.ObjectLayers[9].ContainsKey(cell);
		if (flag)
		{
			Door component = Grid.ObjectLayers[9][cell].GetComponent<Door>();
			flag = (component != null && component.IsOpen() && !component.IsPendingClose());
		}
		return (Grid.Foundation[cell] && Grid.Solid[cell] && !flag) || Grid.Element[cell].hardness >= 150;
	}

	// Token: 0x06003E32 RID: 15922 RVA: 0x00233D74 File Offset: 0x00231F74
	private void RotateArm(Vector3 target_dir, bool warp, float dt)
	{
		if (this.rotation_complete)
		{
			return;
		}
		float num = MathUtil.AngleSigned(Vector3.up, target_dir, Vector3.forward) - this.arm_rot;
		num = MathUtil.Wrap(-180f, 180f, num);
		this.rotation_complete = Mathf.Approximately(num, 0f);
		float num2 = num;
		if (warp)
		{
			this.rotation_complete = true;
		}
		else
		{
			num2 = Mathf.Clamp(num2, -this.turn_rate * dt, this.turn_rate * dt);
		}
		this.arm_rot += num2;
		this.arm_rot = MathUtil.Wrap(-180f, 180f, this.arm_rot);
		this.arm_go.transform.rotation = Quaternion.Euler(0f, 0f, this.arm_rot);
		if (!this.rotation_complete)
		{
			this.StartRotateSound();
			this.looping_sounds.SetParameter(this.rotateSound, AutoMiner.HASH_ROTATION, this.arm_rot);
			return;
		}
		this.StopRotateSound();
	}

	// Token: 0x06003E33 RID: 15923 RVA: 0x000C86EE File Offset: 0x000C68EE
	private void StartRotateSound()
	{
		if (!this.rotate_sound_playing)
		{
			this.looping_sounds.StartSound(this.rotateSound);
			this.rotate_sound_playing = true;
		}
	}

	// Token: 0x06003E34 RID: 15924 RVA: 0x000C8711 File Offset: 0x000C6911
	private void StopRotateSound()
	{
		if (this.rotate_sound_playing)
		{
			this.looping_sounds.StopSound(this.rotateSound);
			this.rotate_sound_playing = false;
		}
	}

	// Token: 0x04002A69 RID: 10857
	private static HashedString HASH_ROTATION = "rotation";

	// Token: 0x04002A6A RID: 10858
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002A6B RID: 10859
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04002A6C RID: 10860
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04002A6D RID: 10861
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04002A6E RID: 10862
	[MyCmpReq]
	private MiningSounds mining_sounds;

	// Token: 0x04002A6F RID: 10863
	public int x;

	// Token: 0x04002A70 RID: 10864
	public int y;

	// Token: 0x04002A71 RID: 10865
	public int width;

	// Token: 0x04002A72 RID: 10866
	public int height;

	// Token: 0x04002A73 RID: 10867
	public CellOffset vision_offset;

	// Token: 0x04002A74 RID: 10868
	private KBatchedAnimController arm_anim_ctrl;

	// Token: 0x04002A75 RID: 10869
	private GameObject arm_go;

	// Token: 0x04002A76 RID: 10870
	private LoopingSounds looping_sounds;

	// Token: 0x04002A77 RID: 10871
	private string rotateSoundName = "AutoMiner_rotate";

	// Token: 0x04002A78 RID: 10872
	private EventReference rotateSound;

	// Token: 0x04002A79 RID: 10873
	private KAnimLink link;

	// Token: 0x04002A7A RID: 10874
	private float arm_rot = 45f;

	// Token: 0x04002A7B RID: 10875
	private float turn_rate = 180f;

	// Token: 0x04002A7C RID: 10876
	private bool rotation_complete;

	// Token: 0x04002A7D RID: 10877
	private bool rotate_sound_playing;

	// Token: 0x04002A7E RID: 10878
	private GameObject hitEffectPrefab;

	// Token: 0x04002A7F RID: 10879
	private GameObject hitEffect;

	// Token: 0x04002A80 RID: 10880
	private int dig_cell = Grid.InvalidCell;

	// Token: 0x04002A81 RID: 10881
	private static readonly EventSystem.IntraObjectHandler<AutoMiner> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<AutoMiner>(delegate(AutoMiner component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x02000C9C RID: 3228
	public class Instance : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.GameInstance
	{
		// Token: 0x06003E37 RID: 15927 RVA: 0x000C8792 File Offset: 0x000C6992
		public Instance(AutoMiner master) : base(master)
		{
		}
	}

	// Token: 0x02000C9D RID: 3229
	public class States : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner>
	{
		// Token: 0x06003E38 RID: 15928 RVA: 0x00233E6C File Offset: 0x0023206C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (AutoMiner.Instance smi) => smi.GetComponent<Operational>().IsOperational);
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (AutoMiner.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.moving, (AutoMiner.Instance smi) => smi.GetComponent<Operational>().IsActive);
			this.on.moving.Exit(delegate(AutoMiner.Instance smi)
			{
				smi.master.StopRotateSound();
			}).PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (AutoMiner.Instance smi) => !smi.GetComponent<Operational>().IsActive).Update(delegate(AutoMiner.Instance smi, float dt)
			{
				smi.master.UpdateRotation(dt);
			}, UpdateRate.SIM_33ms, false).Transition(this.on.digging, new StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback(AutoMiner.States.RotationComplete), UpdateRate.SIM_200ms);
			this.on.digging.Enter(delegate(AutoMiner.Instance smi)
			{
				smi.master.StartDig();
			}).Exit(delegate(AutoMiner.Instance smi)
			{
				smi.master.StopDig();
			}).PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (AutoMiner.Instance smi) => !smi.GetComponent<Operational>().IsActive).Update(delegate(AutoMiner.Instance smi, float dt)
			{
				smi.master.UpdateDig(dt);
			}, UpdateRate.SIM_200ms, false).Transition(this.on.moving, GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Not(new StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback(AutoMiner.States.RotationComplete)), UpdateRate.SIM_200ms);
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x000C879B File Offset: 0x000C699B
		public static bool RotationComplete(AutoMiner.Instance smi)
		{
			return smi.master.RotationComplete;
		}

		// Token: 0x04002A82 RID: 10882
		public StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.BoolParameter transferring;

		// Token: 0x04002A83 RID: 10883
		public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State off;

		// Token: 0x04002A84 RID: 10884
		public AutoMiner.States.ReadyStates on;

		// Token: 0x02000C9E RID: 3230
		public class ReadyStates : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State
		{
			// Token: 0x04002A85 RID: 10885
			public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State idle;

			// Token: 0x04002A86 RID: 10886
			public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State moving;

			// Token: 0x04002A87 RID: 10887
			public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State digging;
		}
	}
}
