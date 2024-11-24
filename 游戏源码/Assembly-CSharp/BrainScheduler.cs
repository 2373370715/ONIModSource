using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000634 RID: 1588
[AddComponentMenu("KMonoBehaviour/scripts/BrainScheduler")]
public class BrainScheduler : KMonoBehaviour, IRenderEveryTick, ICPULoad
{
	// Token: 0x170000AE RID: 174
	// (get) Token: 0x06001CD0 RID: 7376 RVA: 0x000B3039 File Offset: 0x000B1239
	private bool isAsyncPathProbeEnabled
	{
		get
		{
			return !TuningData<BrainScheduler.Tuning>.Get().disableAsyncPathProbes;
		}
	}

	// Token: 0x06001CD1 RID: 7377 RVA: 0x000B3048 File Offset: 0x000B1248
	public List<BrainScheduler.BrainGroup> debugGetBrainGroups()
	{
		return this.brainGroups;
	}

	// Token: 0x06001CD2 RID: 7378 RVA: 0x001AD71C File Offset: 0x001AB91C
	protected override void OnPrefabInit()
	{
		this.brainGroups.Add(new BrainScheduler.DupeBrainGroup());
		this.brainGroups.Add(new BrainScheduler.CreatureBrainGroup());
		Components.Brains.Register(new Action<Brain>(this.OnAddBrain), new Action<Brain>(this.OnRemoveBrain));
		CPUBudget.AddRoot(this);
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			CPUBudget.AddChild(this, brainGroup, brainGroup.LoadBalanceThreshold());
		}
		CPUBudget.FinalizeChildren(this);
	}

	// Token: 0x06001CD3 RID: 7379 RVA: 0x001AD7C4 File Offset: 0x001AB9C4
	private void OnAddBrain(Brain brain)
	{
		bool test = false;
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			if (brain.HasTag(brainGroup.tag))
			{
				brainGroup.AddBrain(brain);
				test = true;
			}
			Navigator component = brain.GetComponent<Navigator>();
			if (component != null)
			{
				component.executePathProbeTaskAsync = this.isAsyncPathProbeEnabled;
			}
		}
		DebugUtil.Assert(test);
	}

	// Token: 0x06001CD4 RID: 7380 RVA: 0x001AD84C File Offset: 0x001ABA4C
	private void OnRemoveBrain(Brain brain)
	{
		bool test = false;
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			if (brain.HasTag(brainGroup.tag))
			{
				test = true;
				brainGroup.RemoveBrain(brain);
			}
			Navigator component = brain.GetComponent<Navigator>();
			if (component != null)
			{
				component.executePathProbeTaskAsync = false;
			}
		}
		DebugUtil.Assert(test);
	}

	// Token: 0x06001CD5 RID: 7381 RVA: 0x001AD8D0 File Offset: 0x001ABAD0
	public void PrioritizeBrain(Brain brain)
	{
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			if (brain.HasTag(brainGroup.tag))
			{
				brainGroup.PrioritizeBrain(brain);
			}
		}
	}

	// Token: 0x06001CD6 RID: 7382 RVA: 0x000B3050 File Offset: 0x000B1250
	public float GetEstimatedFrameTime()
	{
		return TuningData<BrainScheduler.Tuning>.Get().frameTime;
	}

	// Token: 0x06001CD7 RID: 7383 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool AdjustLoad(float currentFrameTime, float frameTimeDelta)
	{
		return false;
	}

	// Token: 0x06001CD8 RID: 7384 RVA: 0x001AD934 File Offset: 0x001ABB34
	public void RenderEveryTick(float dt)
	{
		if (Game.IsQuitting() || KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			brainGroup.RenderEveryTick(dt);
		}
	}

	// Token: 0x06001CD9 RID: 7385 RVA: 0x000B305C File Offset: 0x000B125C
	protected override void OnForcedCleanUp()
	{
		CPUBudget.Remove(this);
		base.OnForcedCleanUp();
	}

	// Token: 0x040011F6 RID: 4598
	public const float millisecondsPerFrame = 33.33333f;

	// Token: 0x040011F7 RID: 4599
	public const float secondsPerFrame = 0.033333328f;

	// Token: 0x040011F8 RID: 4600
	public const float framesPerSecond = 30.000006f;

	// Token: 0x040011F9 RID: 4601
	private List<BrainScheduler.BrainGroup> brainGroups = new List<BrainScheduler.BrainGroup>();

	// Token: 0x02000635 RID: 1589
	private class Tuning : TuningData<BrainScheduler.Tuning>
	{
		// Token: 0x040011FA RID: 4602
		public bool disableAsyncPathProbes;

		// Token: 0x040011FB RID: 4603
		public float frameTime = 5f;
	}

	// Token: 0x02000636 RID: 1590
	public abstract class BrainGroup : ICPULoad
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06001CDC RID: 7388 RVA: 0x000B3090 File Offset: 0x000B1290
		// (set) Token: 0x06001CDD RID: 7389 RVA: 0x000B3098 File Offset: 0x000B1298
		public Tag tag { get; private set; }

		// Token: 0x06001CDE RID: 7390 RVA: 0x001AD994 File Offset: 0x001ABB94
		protected BrainGroup(Tag tag)
		{
			this.tag = tag;
			this.probeSize = this.InitialProbeSize();
			this.probeCount = this.InitialProbeCount();
			string str = tag.ToString();
			this.increaseLoadLabel = "IncLoad" + str;
			this.decreaseLoadLabel = "DecLoad" + str;
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x000B30A1 File Offset: 0x000B12A1
		public void AddBrain(Brain brain)
		{
			this.brains.Add(brain);
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x001ADA18 File Offset: 0x001ABC18
		public void RemoveBrain(Brain brain)
		{
			int num = this.brains.IndexOf(brain);
			if (num != -1)
			{
				this.brains.RemoveAt(num);
				this.OnRemoveBrain(num, ref this.nextUpdateBrain);
				this.OnRemoveBrain(num, ref this.nextPathProbeBrain);
			}
			if (this.priorityBrains.Contains(brain))
			{
				List<Brain> list = new List<Brain>(this.priorityBrains);
				list.Remove(brain);
				this.priorityBrains = new Queue<Brain>(list);
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06001CE1 RID: 7393 RVA: 0x000B30AF File Offset: 0x000B12AF
		public int BrainCount
		{
			get
			{
				return this.brains.Count;
			}
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x000B30BC File Offset: 0x000B12BC
		public void PrioritizeBrain(Brain brain)
		{
			if (!this.priorityBrains.Contains(brain))
			{
				this.priorityBrains.Enqueue(brain);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06001CE3 RID: 7395 RVA: 0x000B30D8 File Offset: 0x000B12D8
		// (set) Token: 0x06001CE4 RID: 7396 RVA: 0x000B30E0 File Offset: 0x000B12E0
		public int probeSize { get; private set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x000B30E9 File Offset: 0x000B12E9
		// (set) Token: 0x06001CE6 RID: 7398 RVA: 0x000B30F1 File Offset: 0x000B12F1
		public int probeCount { get; private set; }

		// Token: 0x06001CE7 RID: 7399 RVA: 0x001ADA8C File Offset: 0x001ABC8C
		public bool AdjustLoad(float currentFrameTime, float frameTimeDelta)
		{
			if (this.debugFreezeLoadAdustment)
			{
				return false;
			}
			bool flag = frameTimeDelta > 0f;
			int num = 0;
			int num2 = Math.Max(this.probeCount, Math.Min(this.brains.Count, CPUBudget.coreCount));
			num += num2 - this.probeCount;
			this.probeCount = num2;
			float num3 = Math.Min(1f, (float)this.probeCount / (float)CPUBudget.coreCount);
			float num4 = num3 * (float)this.probeSize;
			float num5 = num3 * (float)this.probeSize;
			float num6 = currentFrameTime / num5;
			float num7 = frameTimeDelta / num6;
			if (num == 0)
			{
				float num8 = num4 + num7 / (float)CPUBudget.coreCount;
				int num9 = MathUtil.Clamp(this.MinProbeSize(), this.IdealProbeSize(), (int)(num8 / num3));
				num += num9 - this.probeSize;
				this.probeSize = num9;
			}
			if (num == 0)
			{
				int num10 = Math.Max(1, (int)num3 + (flag ? 1 : -1));
				int probeSize = MathUtil.Clamp(this.MinProbeSize(), this.IdealProbeSize(), (int)((num5 + num7) / (float)num10));
				int num11 = Math.Min(this.brains.Count, num10 * CPUBudget.coreCount);
				num += num11 - this.probeCount;
				this.probeCount = num11;
				this.probeSize = probeSize;
			}
			if (num == 0 && flag)
			{
				int num12 = this.probeSize + this.ProbeSizeStep();
				num += num12 - this.probeSize;
				this.probeSize = num12;
			}
			if (num >= 0 && num <= 0 && this.brains.Count > 0)
			{
				global::Debug.LogWarning("AdjustLoad() failed");
			}
			return num != 0;
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x000B30FA File Offset: 0x000B12FA
		public void ResetLoad()
		{
			this.probeSize = this.InitialProbeSize();
			this.probeCount = this.InitialProbeCount();
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x000B3114 File Offset: 0x000B1314
		private void IncrementBrainIndex(ref int brainIndex)
		{
			brainIndex++;
			if (brainIndex == this.brains.Count)
			{
				brainIndex = 0;
			}
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x000B312E File Offset: 0x000B132E
		private void ClampBrainIndex(ref int brainIndex)
		{
			brainIndex = MathUtil.Clamp(0, this.brains.Count - 1, brainIndex);
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x000B3147 File Offset: 0x000B1347
		private void OnRemoveBrain(int removedIndex, ref int brainIndex)
		{
			if (removedIndex < brainIndex)
			{
				brainIndex--;
				return;
			}
			if (brainIndex == this.brains.Count)
			{
				brainIndex = 0;
			}
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x001ADC10 File Offset: 0x001ABE10
		private void AsyncPathProbe()
		{
			this.pathProbeJob.Reset(null);
			for (int num = 0; num != this.brains.Count; num++)
			{
				this.ClampBrainIndex(ref this.nextPathProbeBrain);
				Brain brain = this.brains[this.nextPathProbeBrain];
				if (brain.IsRunning())
				{
					Navigator component = brain.GetComponent<Navigator>();
					if (component != null)
					{
						component.executePathProbeTaskAsync = true;
						component.PathProber.potentialCellsPerUpdate = this.probeSize;
						component.pathProbeTask.Update();
						this.pathProbeJob.Add(component.pathProbeTask);
						if (this.pathProbeJob.Count == this.probeCount)
						{
							break;
						}
					}
				}
				this.IncrementBrainIndex(ref this.nextPathProbeBrain);
			}
			CPUBudget.Start(this);
			GlobalJobManager.Run(this.pathProbeJob);
			CPUBudget.End(this);
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x001ADCE8 File Offset: 0x001ABEE8
		public void RenderEveryTick(float dt)
		{
			this.BeginBrainGroupUpdate();
			int num = this.InitialProbeCount();
			int num2 = 0;
			while (num2 != this.brains.Count && num != 0)
			{
				this.ClampBrainIndex(ref this.nextUpdateBrain);
				this.debugMaxPriorityBrainCountSeen = Mathf.Max(this.debugMaxPriorityBrainCountSeen, this.priorityBrains.Count);
				Brain brain;
				if (this.AllowPriorityBrains() && this.priorityBrains.Count > 0)
				{
					brain = this.priorityBrains.Dequeue();
				}
				else
				{
					brain = this.brains[this.nextUpdateBrain];
					this.IncrementBrainIndex(ref this.nextUpdateBrain);
				}
				if (brain.IsRunning())
				{
					brain.UpdateBrain();
					num--;
				}
				num2++;
			}
			this.EndBrainGroupUpdate();
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x001ADDA8 File Offset: 0x001ABFA8
		public void AccumulatePathProbeIterations(Dictionary<string, int> pathProbeIterations)
		{
			foreach (Brain brain in this.brains)
			{
				Navigator component = brain.GetComponent<Navigator>();
				if (!(component == null) && !pathProbeIterations.ContainsKey(brain.name))
				{
					pathProbeIterations.Add(brain.name, component.PathProber.updateCount);
				}
			}
		}

		// Token: 0x06001CEF RID: 7407
		protected abstract int InitialProbeCount();

		// Token: 0x06001CF0 RID: 7408
		protected abstract int InitialProbeSize();

		// Token: 0x06001CF1 RID: 7409
		protected abstract int MinProbeSize();

		// Token: 0x06001CF2 RID: 7410
		protected abstract int IdealProbeSize();

		// Token: 0x06001CF3 RID: 7411
		protected abstract int ProbeSizeStep();

		// Token: 0x06001CF4 RID: 7412
		public abstract float GetEstimatedFrameTime();

		// Token: 0x06001CF5 RID: 7413
		public abstract float LoadBalanceThreshold();

		// Token: 0x06001CF6 RID: 7414
		public abstract bool AllowPriorityBrains();

		// Token: 0x06001CF7 RID: 7415 RVA: 0x000B3167 File Offset: 0x000B1367
		public virtual void BeginBrainGroupUpdate()
		{
			if (Game.BrainScheduler.isAsyncPathProbeEnabled)
			{
				this.AsyncPathProbe();
			}
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x000A5E40 File Offset: 0x000A4040
		public virtual void EndBrainGroupUpdate()
		{
		}

		// Token: 0x040011FD RID: 4605
		protected List<Brain> brains = new List<Brain>();

		// Token: 0x040011FE RID: 4606
		protected Queue<Brain> priorityBrains = new Queue<Brain>();

		// Token: 0x040011FF RID: 4607
		private string increaseLoadLabel;

		// Token: 0x04001200 RID: 4608
		private string decreaseLoadLabel;

		// Token: 0x04001201 RID: 4609
		public bool debugFreezeLoadAdustment;

		// Token: 0x04001202 RID: 4610
		public int debugMaxPriorityBrainCountSeen;

		// Token: 0x04001203 RID: 4611
		private WorkItemCollection<Navigator.PathProbeTask, object> pathProbeJob = new WorkItemCollection<Navigator.PathProbeTask, object>();

		// Token: 0x04001204 RID: 4612
		private int nextUpdateBrain;

		// Token: 0x04001205 RID: 4613
		private int nextPathProbeBrain;
	}

	// Token: 0x02000637 RID: 1591
	private class DupeBrainGroup : BrainScheduler.BrainGroup
	{
		// Token: 0x06001CF9 RID: 7417 RVA: 0x000B317B File Offset: 0x000B137B
		public DupeBrainGroup() : base(GameTags.DupeBrain)
		{
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x000B318F File Offset: 0x000B138F
		protected override int InitialProbeCount()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().initialProbeCount;
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x000B319B File Offset: 0x000B139B
		protected override int InitialProbeSize()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().initialProbeSize;
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x000B31A7 File Offset: 0x000B13A7
		protected override int MinProbeSize()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().minProbeSize;
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x000B31B3 File Offset: 0x000B13B3
		protected override int IdealProbeSize()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().idealProbeSize;
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x000B31BF File Offset: 0x000B13BF
		protected override int ProbeSizeStep()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().probeSizeStep;
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x000B31CB File Offset: 0x000B13CB
		public override float GetEstimatedFrameTime()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().estimatedFrameTime;
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x000B31D7 File Offset: 0x000B13D7
		public override float LoadBalanceThreshold()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().loadBalanceThreshold;
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x000B31E3 File Offset: 0x000B13E3
		public override bool AllowPriorityBrains()
		{
			return this.usePriorityBrain;
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x000B31EB File Offset: 0x000B13EB
		public override void BeginBrainGroupUpdate()
		{
			base.BeginBrainGroupUpdate();
			this.usePriorityBrain = !this.usePriorityBrain;
		}

		// Token: 0x04001208 RID: 4616
		private bool usePriorityBrain = true;

		// Token: 0x02000638 RID: 1592
		public class Tuning : TuningData<BrainScheduler.DupeBrainGroup.Tuning>
		{
			// Token: 0x04001209 RID: 4617
			public int initialProbeCount = 1;

			// Token: 0x0400120A RID: 4618
			public int initialProbeSize = 1000;

			// Token: 0x0400120B RID: 4619
			public int minProbeSize = 100;

			// Token: 0x0400120C RID: 4620
			public int idealProbeSize = 1000;

			// Token: 0x0400120D RID: 4621
			public int probeSizeStep = 100;

			// Token: 0x0400120E RID: 4622
			public float estimatedFrameTime = 2f;

			// Token: 0x0400120F RID: 4623
			public float loadBalanceThreshold = 0.1f;
		}
	}

	// Token: 0x02000639 RID: 1593
	private class CreatureBrainGroup : BrainScheduler.BrainGroup
	{
		// Token: 0x06001D04 RID: 7428 RVA: 0x000B3202 File Offset: 0x000B1402
		public CreatureBrainGroup() : base(GameTags.CreatureBrain)
		{
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x000B320F File Offset: 0x000B140F
		protected override int InitialProbeCount()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().initialProbeCount;
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x000B321B File Offset: 0x000B141B
		protected override int InitialProbeSize()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().initialProbeSize;
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x000B3227 File Offset: 0x000B1427
		protected override int MinProbeSize()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().minProbeSize;
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x000B3233 File Offset: 0x000B1433
		protected override int IdealProbeSize()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().idealProbeSize;
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x000B323F File Offset: 0x000B143F
		protected override int ProbeSizeStep()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().probeSizeStep;
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x000B324B File Offset: 0x000B144B
		public override float GetEstimatedFrameTime()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().estimatedFrameTime;
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x000B3257 File Offset: 0x000B1457
		public override float LoadBalanceThreshold()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().loadBalanceThreshold;
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x000A65EC File Offset: 0x000A47EC
		public override bool AllowPriorityBrains()
		{
			return true;
		}

		// Token: 0x0200063A RID: 1594
		public class Tuning : TuningData<BrainScheduler.CreatureBrainGroup.Tuning>
		{
			// Token: 0x04001210 RID: 4624
			public int initialProbeCount = 5;

			// Token: 0x04001211 RID: 4625
			public int initialProbeSize = 1000;

			// Token: 0x04001212 RID: 4626
			public int minProbeSize = 100;

			// Token: 0x04001213 RID: 4627
			public int idealProbeSize = 300;

			// Token: 0x04001214 RID: 4628
			public int probeSizeStep = 100;

			// Token: 0x04001215 RID: 4629
			public float estimatedFrameTime = 1f;

			// Token: 0x04001216 RID: 4630
			public float loadBalanceThreshold = 0.1f;
		}
	}
}
