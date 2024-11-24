using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000209 RID: 521
public class SegmentedCreature : GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>
{
	// Token: 0x06000709 RID: 1801 RVA: 0x0015DCCC File Offset: 0x0015BECC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.freeMovement.idle;
		this.root.Enter(new StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State.Callback(this.SetRetractedPath));
		this.retracted.DefaultState(this.retracted.pre).Enter(delegate(SegmentedCreature.Instance smi)
		{
			this.PlayBodySegmentsAnim(smi, "idle_loop", KAnim.PlayMode.Loop, false, 0);
		}).Exit(new StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State.Callback(this.SetRetractedPath));
		this.retracted.pre.Update(new Action<SegmentedCreature.Instance, float>(this.UpdateRetractedPre), UpdateRate.SIM_EVERY_TICK, false);
		this.retracted.loop.ParamTransition<bool>(this.isRetracted, this.freeMovement, (SegmentedCreature.Instance smi, bool p) => !this.isRetracted.Get(smi)).Update(new Action<SegmentedCreature.Instance, float>(this.UpdateRetractedLoop), UpdateRate.SIM_EVERY_TICK, false);
		this.freeMovement.DefaultState(this.freeMovement.idle).ParamTransition<bool>(this.isRetracted, this.retracted, (SegmentedCreature.Instance smi, bool p) => this.isRetracted.Get(smi)).Update(new Action<SegmentedCreature.Instance, float>(this.UpdateFreeMovement), UpdateRate.SIM_EVERY_TICK, false);
		this.freeMovement.idle.Transition(this.freeMovement.moving, (SegmentedCreature.Instance smi) => smi.GetComponent<Navigator>().IsMoving(), UpdateRate.SIM_200ms).Enter(delegate(SegmentedCreature.Instance smi)
		{
			this.PlayBodySegmentsAnim(smi, "idle_loop", KAnim.PlayMode.Loop, true, 0);
		});
		this.freeMovement.moving.Transition(this.freeMovement.idle, (SegmentedCreature.Instance smi) => !smi.GetComponent<Navigator>().IsMoving(), UpdateRate.SIM_200ms).Enter(delegate(SegmentedCreature.Instance smi)
		{
			this.PlayBodySegmentsAnim(smi, "walking_pre", KAnim.PlayMode.Once, false, 0);
			this.PlayBodySegmentsAnim(smi, "walking_loop", KAnim.PlayMode.Loop, false, smi.def.animFrameOffset);
		}).Exit(delegate(SegmentedCreature.Instance smi)
		{
			this.PlayBodySegmentsAnim(smi, "walking_pst", KAnim.PlayMode.Once, true, 0);
		});
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x0015DE84 File Offset: 0x0015C084
	private void PlayBodySegmentsAnim(SegmentedCreature.Instance smi, string animName, KAnim.PlayMode playMode, bool queue = false, int frameOffset = 0)
	{
		LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode();
		int num = 0;
		while (linkedListNode != null)
		{
			if (queue)
			{
				linkedListNode.Value.animController.Queue(animName, playMode, 1f, 0f);
			}
			else
			{
				linkedListNode.Value.animController.Play(animName, playMode, 1f, 0f);
			}
			if (frameOffset > 0)
			{
				float num2 = (float)linkedListNode.Value.animController.GetCurrentNumFrames();
				float elapsedTime = (float)num * ((float)frameOffset / num2);
				linkedListNode.Value.animController.SetElapsedTime(elapsedTime);
			}
			num++;
			linkedListNode = linkedListNode.Next;
		}
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x0015DF2C File Offset: 0x0015C12C
	private void UpdateRetractedPre(SegmentedCreature.Instance smi, float dt)
	{
		if (this.UpdateHeadPosition(smi) == 0f)
		{
			return;
		}
		bool flag = true;
		for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			linkedListNode.Value.distanceToPreviousSegment = Mathf.Max(smi.def.minSegmentSpacing, linkedListNode.Value.distanceToPreviousSegment - dt * smi.def.retractionSegmentSpeed);
			if (linkedListNode.Value.distanceToPreviousSegment > smi.def.minSegmentSpacing)
			{
				flag = false;
			}
		}
		SegmentedCreature.CreatureSegment value = smi.GetHeadSegmentNode().Value;
		LinkedListNode<SegmentedCreature.PathNode> linkedListNode2 = smi.path.First;
		Vector3 forward = value.Forward;
		Quaternion rotation = value.Rotation;
		int num = 0;
		while (linkedListNode2 != null)
		{
			Vector3 b = value.Position - smi.def.pathSpacing * (float)num * forward;
			linkedListNode2.Value.position = Vector3.Lerp(linkedListNode2.Value.position, b, dt * smi.def.retractionPathSpeed);
			linkedListNode2.Value.rotation = Quaternion.Slerp(linkedListNode2.Value.rotation, rotation, dt * smi.def.retractionPathSpeed);
			num++;
			linkedListNode2 = linkedListNode2.Next;
		}
		this.UpdateBodyPosition(smi);
		if (flag)
		{
			smi.GoTo(this.retracted.loop);
		}
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x000A956F File Offset: 0x000A776F
	private void UpdateRetractedLoop(SegmentedCreature.Instance smi, float dt)
	{
		if (this.UpdateHeadPosition(smi) != 0f)
		{
			this.SetRetractedPath(smi);
			this.UpdateBodyPosition(smi);
		}
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x0015E080 File Offset: 0x0015C280
	private void SetRetractedPath(SegmentedCreature.Instance smi)
	{
		SegmentedCreature.CreatureSegment value = smi.GetHeadSegmentNode().Value;
		LinkedListNode<SegmentedCreature.PathNode> linkedListNode = smi.path.First;
		Vector3 position = value.Position;
		Quaternion rotation = value.Rotation;
		Vector3 forward = value.Forward;
		int num = 0;
		while (linkedListNode != null)
		{
			linkedListNode.Value.position = position - smi.def.pathSpacing * (float)num * forward;
			linkedListNode.Value.rotation = rotation;
			num++;
			linkedListNode = linkedListNode.Next;
		}
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x0015E100 File Offset: 0x0015C300
	private void UpdateFreeMovement(SegmentedCreature.Instance smi, float dt)
	{
		float num = this.UpdateHeadPosition(smi);
		if (num != 0f)
		{
			this.AdjustBodySegmentsSpacing(smi, num);
			this.UpdateBodyPosition(smi);
		}
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x0015E12C File Offset: 0x0015C32C
	private float UpdateHeadPosition(SegmentedCreature.Instance smi)
	{
		SegmentedCreature.CreatureSegment value = smi.GetHeadSegmentNode().Value;
		if (value.Position == smi.previousHeadPosition)
		{
			return 0f;
		}
		SegmentedCreature.PathNode value2 = smi.path.First.Value;
		SegmentedCreature.PathNode value3 = smi.path.First.Next.Value;
		float magnitude = (value2.position - value3.position).magnitude;
		float magnitude2 = (value.Position - value3.position).magnitude;
		float result = magnitude2 - magnitude;
		value2.position = value.Position;
		value2.rotation = value.Rotation;
		smi.previousHeadPosition = value2.position;
		Vector3 normalized = (value2.position - value3.position).normalized;
		int num = Mathf.FloorToInt(magnitude2 / smi.def.pathSpacing);
		for (int i = 0; i < num; i++)
		{
			Vector3 position = value3.position + normalized * smi.def.pathSpacing;
			LinkedListNode<SegmentedCreature.PathNode> last = smi.path.Last;
			last.Value.position = position;
			last.Value.rotation = value2.rotation;
			float num2 = magnitude2 - (float)i * smi.def.pathSpacing;
			float t = num2 - smi.def.pathSpacing / num2;
			last.Value.rotation = Quaternion.Lerp(value2.rotation, value3.rotation, t);
			smi.path.RemoveLast();
			smi.path.AddAfter(smi.path.First, last);
			value3 = last.Value;
		}
		return result;
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x0015E2F0 File Offset: 0x0015C4F0
	private void AdjustBodySegmentsSpacing(SegmentedCreature.Instance smi, float spacing)
	{
		for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			linkedListNode.Value.distanceToPreviousSegment += spacing;
			if (linkedListNode.Value.distanceToPreviousSegment < smi.def.minSegmentSpacing)
			{
				spacing = linkedListNode.Value.distanceToPreviousSegment - smi.def.minSegmentSpacing;
				linkedListNode.Value.distanceToPreviousSegment = smi.def.minSegmentSpacing;
			}
			else
			{
				if (linkedListNode.Value.distanceToPreviousSegment <= smi.def.maxSegmentSpacing)
				{
					break;
				}
				spacing = linkedListNode.Value.distanceToPreviousSegment - smi.def.maxSegmentSpacing;
				linkedListNode.Value.distanceToPreviousSegment = smi.def.maxSegmentSpacing;
			}
		}
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x0015E3BC File Offset: 0x0015C5BC
	private void UpdateBodyPosition(SegmentedCreature.Instance smi)
	{
		LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode();
		LinkedListNode<SegmentedCreature.PathNode> linkedListNode2 = smi.path.First;
		float num = 0f;
		float num2 = smi.LengthPercentage();
		int num3 = 0;
		while (linkedListNode != null)
		{
			float num4 = linkedListNode.Value.distanceToPreviousSegment;
			float num5 = 0f;
			while (linkedListNode2.Next != null)
			{
				num5 = (linkedListNode2.Value.position - linkedListNode2.Next.Value.position).magnitude - num;
				if (num4 < num5)
				{
					break;
				}
				num4 -= num5;
				num = 0f;
				linkedListNode2 = linkedListNode2.Next;
			}
			if (linkedListNode2.Next == null)
			{
				linkedListNode.Value.SetPosition(linkedListNode2.Value.position);
				linkedListNode.Value.SetRotation(smi.path.Last.Value.rotation);
			}
			else
			{
				SegmentedCreature.PathNode value = linkedListNode2.Value;
				SegmentedCreature.PathNode value2 = linkedListNode2.Next.Value;
				linkedListNode.Value.SetPosition(linkedListNode2.Value.position + (linkedListNode2.Next.Value.position - linkedListNode2.Value.position).normalized * num4);
				linkedListNode.Value.SetRotation(Quaternion.Slerp(value.rotation, value2.rotation, num4 / num5));
				num = num4;
			}
			linkedListNode.Value.animController.FlipX = (linkedListNode.Previous.Value.Position.x < linkedListNode.Value.Position.x);
			linkedListNode.Value.animController.animScale = smi.baseAnimScale + smi.baseAnimScale * smi.def.compressedMaxScale * ((float)(smi.def.numBodySegments - num3) / (float)smi.def.numBodySegments) * (1f - num2);
			linkedListNode = linkedListNode.Next;
			num3++;
		}
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x0015E5B8 File Offset: 0x0015C7B8
	private void DrawDebug(SegmentedCreature.Instance smi, float dt)
	{
		SegmentedCreature.CreatureSegment value = smi.GetHeadSegmentNode().Value;
		DrawUtil.Arrow(value.Position, value.Position + value.Up, 0.05f, Color.red, 0f);
		DrawUtil.Arrow(value.Position, value.Position + value.Forward * 0.06f, 0.05f, Color.cyan, 0f);
		int num = 0;
		foreach (SegmentedCreature.PathNode pathNode in smi.path)
		{
			Color color = Color.HSVToRGB((float)num / (float)smi.def.numPathNodes, 1f, 1f);
			DrawUtil.Gnomon(pathNode.position, 0.05f, Color.cyan, 0f);
			DrawUtil.Arrow(pathNode.position, pathNode.position + pathNode.rotation * Vector3.up * 0.5f, 0.025f, color, 0f);
			num++;
		}
		for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.segments.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			DrawUtil.Circle(linkedListNode.Value.Position, 0.05f, Color.white, new Vector3?(Vector3.forward), 0f);
			DrawUtil.Gnomon(linkedListNode.Value.Position, 0.05f, Color.white, 0f);
		}
	}

	// Token: 0x0400052B RID: 1323
	public SegmentedCreature.RectractStates retracted;

	// Token: 0x0400052C RID: 1324
	public SegmentedCreature.FreeMovementStates freeMovement;

	// Token: 0x0400052D RID: 1325
	private StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.BoolParameter isRetracted;

	// Token: 0x0200020A RID: 522
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400052E RID: 1326
		public HashedString segmentTrackerSymbol;

		// Token: 0x0400052F RID: 1327
		public Vector3 headOffset = Vector3.zero;

		// Token: 0x04000530 RID: 1328
		public Vector3 bodyPivot = Vector3.zero;

		// Token: 0x04000531 RID: 1329
		public Vector3 tailPivot = Vector3.zero;

		// Token: 0x04000532 RID: 1330
		public int numBodySegments;

		// Token: 0x04000533 RID: 1331
		public float minSegmentSpacing;

		// Token: 0x04000534 RID: 1332
		public float maxSegmentSpacing;

		// Token: 0x04000535 RID: 1333
		public int numPathNodes;

		// Token: 0x04000536 RID: 1334
		public float pathSpacing;

		// Token: 0x04000537 RID: 1335
		public KAnimFile midAnim;

		// Token: 0x04000538 RID: 1336
		public KAnimFile tailAnim;

		// Token: 0x04000539 RID: 1337
		public string movingAnimName;

		// Token: 0x0400053A RID: 1338
		public string idleAnimName;

		// Token: 0x0400053B RID: 1339
		public float retractionSegmentSpeed = 1f;

		// Token: 0x0400053C RID: 1340
		public float retractionPathSpeed = 1f;

		// Token: 0x0400053D RID: 1341
		public float compressedMaxScale = 1.2f;

		// Token: 0x0400053E RID: 1342
		public int animFrameOffset;

		// Token: 0x0400053F RID: 1343
		public HashSet<HashedString> hideBoddyWhenStartingAnimNames = new HashSet<HashedString>
		{
			"rocket_biological"
		};

		// Token: 0x04000540 RID: 1344
		public HashSet<HashedString> retractWhenStartingAnimNames = new HashSet<HashedString>
		{
			"trapped",
			"trussed",
			"escape",
			"drown_pre",
			"drown_loop",
			"drown_pst",
			"rocket_biological"
		};

		// Token: 0x04000541 RID: 1345
		public HashSet<HashedString> retractWhenEndingAnimNames = new HashSet<HashedString>
		{
			"floor_floor_2_0",
			"grooming_pst",
			"fall"
		};
	}

	// Token: 0x0200020B RID: 523
	public class RectractStates : GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State
	{
		// Token: 0x04000542 RID: 1346
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State pre;

		// Token: 0x04000543 RID: 1347
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State loop;
	}

	// Token: 0x0200020C RID: 524
	public class FreeMovementStates : GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State
	{
		// Token: 0x04000544 RID: 1348
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State idle;

		// Token: 0x04000545 RID: 1349
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State moving;

		// Token: 0x04000546 RID: 1350
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State layEgg;

		// Token: 0x04000547 RID: 1351
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State poop;

		// Token: 0x04000548 RID: 1352
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State dead;
	}

	// Token: 0x0200020D RID: 525
	public new class Instance : GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.GameInstance
	{
		// Token: 0x0600071D RID: 1821 RVA: 0x0015E88C File Offset: 0x0015CA8C
		public Instance(IStateMachineTarget master, SegmentedCreature.Def def) : base(master, def)
		{
			global::Debug.Assert((float)def.numBodySegments * def.maxSegmentSpacing < (float)def.numPathNodes * def.pathSpacing);
			this.CreateSegments();
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0015E8E0 File Offset: 0x0015CAE0
		private void CreateSegments()
		{
			float num = (float)SegmentedCreature.Instance.creatureBatchSlot * 0.01f;
			SegmentedCreature.Instance.creatureBatchSlot = (SegmentedCreature.Instance.creatureBatchSlot + 1) % 10;
			SegmentedCreature.CreatureSegment value = this.segments.AddFirst(new SegmentedCreature.CreatureSegment(base.GetComponent<KBatchedAnimController>(), base.gameObject, num, base.smi.def.headOffset, Vector3.zero)).Value;
			base.gameObject.SetActive(false);
			value.animController = base.GetComponent<KBatchedAnimController>();
			value.animController.SetSymbolVisiblity(base.smi.def.segmentTrackerSymbol, false);
			value.symbol = base.smi.def.segmentTrackerSymbol;
			value.SetPosition(base.transform.position);
			base.gameObject.SetActive(true);
			this.baseAnimScale = value.animController.animScale;
			value.animController.onAnimEnter += this.AnimEntered;
			value.animController.onAnimComplete += this.AnimComplete;
			for (int i = 0; i < base.def.numBodySegments; i++)
			{
				GameObject gameObject = new GameObject(base.gameObject.GetProperName() + string.Format(" Segment {0}", i));
				gameObject.SetActive(false);
				gameObject.transform.parent = base.transform;
				gameObject.transform.position = value.Position;
				KAnimFile kanimFile = base.def.midAnim;
				Vector3 pivot = base.def.bodyPivot;
				if (i == base.def.numBodySegments - 1)
				{
					kanimFile = base.def.tailAnim;
					pivot = base.def.tailPivot;
				}
				KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
				kbatchedAnimController.AnimFiles = new KAnimFile[]
				{
					kanimFile
				};
				kbatchedAnimController.isMovable = true;
				kbatchedAnimController.SetSymbolVisiblity(base.smi.def.segmentTrackerSymbol, false);
				kbatchedAnimController.sceneLayer = value.animController.sceneLayer;
				SegmentedCreature.CreatureSegment creatureSegment = new SegmentedCreature.CreatureSegment(value.animController, gameObject, num + (float)(i + 1) * 0.0001f, Vector3.zero, pivot);
				creatureSegment.animController = kbatchedAnimController;
				creatureSegment.symbol = base.smi.def.segmentTrackerSymbol;
				creatureSegment.distanceToPreviousSegment = base.smi.def.minSegmentSpacing;
				creatureSegment.animLink = new KAnimLink(value.animController, kbatchedAnimController);
				this.segments.AddLast(creatureSegment);
				gameObject.SetActive(true);
			}
			for (int j = 0; j < base.def.numPathNodes; j++)
			{
				this.path.AddLast(new SegmentedCreature.PathNode(value.Position));
			}
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0015EBA0 File Offset: 0x0015CDA0
		public void AnimEntered(HashedString name)
		{
			if (base.smi.def.retractWhenStartingAnimNames.Contains(name))
			{
				base.smi.sm.isRetracted.Set(true, base.smi, false);
			}
			else
			{
				base.smi.sm.isRetracted.Set(false, base.smi, false);
			}
			if (base.smi.def.hideBoddyWhenStartingAnimNames.Contains(name))
			{
				this.SetBodySegmentsVisibility(false);
				return;
			}
			this.SetBodySegmentsVisibility(true);
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0015EC2C File Offset: 0x0015CE2C
		public void SetBodySegmentsVisibility(bool visible)
		{
			for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = base.smi.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value.animController.SetVisiblity(visible);
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x000A9619 File Offset: 0x000A7819
		public void AnimComplete(HashedString name)
		{
			if (base.smi.def.retractWhenEndingAnimNames.Contains(name))
			{
				base.smi.sm.isRetracted.Set(true, base.smi, false);
			}
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x000A9651 File Offset: 0x000A7851
		public LinkedListNode<SegmentedCreature.CreatureSegment> GetHeadSegmentNode()
		{
			return base.smi.segments.First;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x000A9663 File Offset: 0x000A7863
		public LinkedListNode<SegmentedCreature.CreatureSegment> GetFirstBodySegmentNode()
		{
			return base.smi.segments.First.Next;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0015EC64 File Offset: 0x0015CE64
		public float LengthPercentage()
		{
			float num = 0f;
			for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = this.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				num += linkedListNode.Value.distanceToPreviousSegment;
			}
			float num2 = this.MinLength();
			float num3 = this.MaxLength();
			return Mathf.Clamp(num - num2, 0f, num3) / (num3 - num2);
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x000A967A File Offset: 0x000A787A
		public float MinLength()
		{
			return base.smi.def.minSegmentSpacing * (float)base.smi.def.numBodySegments;
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x000A969E File Offset: 0x000A789E
		public float MaxLength()
		{
			return base.smi.def.maxSegmentSpacing * (float)base.smi.def.numBodySegments;
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0015ECB8 File Offset: 0x0015CEB8
		protected override void OnCleanUp()
		{
			this.GetHeadSegmentNode().Value.animController.onAnimEnter -= this.AnimEntered;
			this.GetHeadSegmentNode().Value.animController.onAnimComplete -= this.AnimComplete;
			for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = this.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value.CleanUp();
			}
		}

		// Token: 0x04000549 RID: 1353
		private const int NUM_CREATURE_SLOTS = 10;

		// Token: 0x0400054A RID: 1354
		private static int creatureBatchSlot;

		// Token: 0x0400054B RID: 1355
		public float baseAnimScale;

		// Token: 0x0400054C RID: 1356
		public Vector3 previousHeadPosition;

		// Token: 0x0400054D RID: 1357
		public float previousDist;

		// Token: 0x0400054E RID: 1358
		public LinkedList<SegmentedCreature.PathNode> path = new LinkedList<SegmentedCreature.PathNode>();

		// Token: 0x0400054F RID: 1359
		public LinkedList<SegmentedCreature.CreatureSegment> segments = new LinkedList<SegmentedCreature.CreatureSegment>();
	}

	// Token: 0x0200020E RID: 526
	public class PathNode
	{
		// Token: 0x06000728 RID: 1832 RVA: 0x000A96C2 File Offset: 0x000A78C2
		public PathNode(Vector3 position)
		{
			this.position = position;
			this.rotation = Quaternion.identity;
		}

		// Token: 0x04000550 RID: 1360
		public Vector3 position;

		// Token: 0x04000551 RID: 1361
		public Quaternion rotation;
	}

	// Token: 0x0200020F RID: 527
	public class CreatureSegment
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x000A96DC File Offset: 0x000A78DC
		public float ZOffset
		{
			get
			{
				return Grid.GetLayerZ(this.head.sceneLayer) + this.zRelativeOffset;
			}
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0015ED28 File Offset: 0x0015CF28
		public CreatureSegment(KBatchedAnimController head, GameObject go, float zRelativeOffset, Vector3 offset, Vector3 pivot)
		{
			this.head = head;
			this.m_transform = go.transform;
			this.zRelativeOffset = zRelativeOffset;
			this.offset = offset;
			this.pivot = pivot;
			this.SetPosition(go.transform.position);
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x0015ED78 File Offset: 0x0015CF78
		public Vector3 Position
		{
			get
			{
				Vector3 vector = this.offset;
				vector.x *= (float)(this.animController.FlipX ? -1 : 1);
				if (vector != Vector3.zero)
				{
					vector = this.Rotation * vector;
				}
				if (this.symbol.IsValid)
				{
					bool flag;
					Vector3 a = this.animController.GetSymbolTransform(this.symbol, out flag).GetColumn(3);
					a.z = this.ZOffset;
					return a + vector;
				}
				return this.m_transform.position + vector;
			}
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0015EE1C File Offset: 0x0015D01C
		public void SetPosition(Vector3 value)
		{
			bool flag = false;
			if (this.animController != null && this.animController.sceneLayer != this.head.sceneLayer)
			{
				this.animController.SetSceneLayer(this.head.sceneLayer);
				flag = true;
			}
			value.z = this.ZOffset;
			this.m_transform.position = value;
			if (flag)
			{
				this.animController.enabled = false;
				this.animController.enabled = true;
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x000A96F5 File Offset: 0x000A78F5
		public void SetRotation(Quaternion rotation)
		{
			this.m_transform.rotation = rotation;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x0015EEA0 File Offset: 0x0015D0A0
		public Quaternion Rotation
		{
			get
			{
				if (this.symbol.IsValid)
				{
					bool flag;
					Vector3 toDirection = this.animController.GetSymbolLocalTransform(this.symbol, out flag).MultiplyVector(Vector3.right);
					if (!this.animController.FlipX)
					{
						toDirection.y *= -1f;
					}
					return Quaternion.FromToRotation(Vector3.right, toDirection);
				}
				return this.m_transform.rotation;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x000A9703 File Offset: 0x000A7903
		public Vector3 Forward
		{
			get
			{
				return this.Rotation * (this.animController.FlipX ? Vector3.left : Vector3.right);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x000A9729 File Offset: 0x000A7929
		public Vector3 Up
		{
			get
			{
				return this.Rotation * Vector3.up;
			}
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x000A973B File Offset: 0x000A793B
		public void CleanUp()
		{
			UnityEngine.Object.Destroy(this.m_transform.gameObject);
		}

		// Token: 0x04000552 RID: 1362
		public KBatchedAnimController animController;

		// Token: 0x04000553 RID: 1363
		public KAnimLink animLink;

		// Token: 0x04000554 RID: 1364
		public float distanceToPreviousSegment;

		// Token: 0x04000555 RID: 1365
		public HashedString symbol;

		// Token: 0x04000556 RID: 1366
		public Vector3 offset;

		// Token: 0x04000557 RID: 1367
		public Vector3 pivot;

		// Token: 0x04000558 RID: 1368
		public KBatchedAnimController head;

		// Token: 0x04000559 RID: 1369
		private float zRelativeOffset;

		// Token: 0x0400055A RID: 1370
		private Transform m_transform;
	}
}
