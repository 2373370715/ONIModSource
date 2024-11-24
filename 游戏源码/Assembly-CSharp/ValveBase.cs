using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200102A RID: 4138
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ValveBase")]
public class ValveBase : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x06005479 RID: 21625 RVA: 0x000D7087 File Offset: 0x000D5287
	// (set) Token: 0x06005478 RID: 21624 RVA: 0x000D707E File Offset: 0x000D527E
	public float CurrentFlow
	{
		get
		{
			return this.currentFlow;
		}
		set
		{
			this.currentFlow = value;
		}
	}

	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x0600547A RID: 21626 RVA: 0x000D708F File Offset: 0x000D528F
	public HandleVector<int>.Handle AccumulatorHandle
	{
		get
		{
			return this.flowAccumulator;
		}
	}

	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x0600547B RID: 21627 RVA: 0x000D7097 File Offset: 0x000D5297
	public float MaxFlow
	{
		get
		{
			return this.maxFlow;
		}
	}

	// Token: 0x0600547C RID: 21628 RVA: 0x000D709F File Offset: 0x000D529F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.flowAccumulator = Game.Instance.accumulators.Add("Flow", this);
	}

	// Token: 0x0600547D RID: 21629 RVA: 0x0027B264 File Offset: 0x00279464
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		Conduit.GetFlowManager(this.conduitType).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.UpdateAnim();
		this.OnCmpEnable();
	}

	// Token: 0x0600547E RID: 21630 RVA: 0x000D70C2 File Offset: 0x000D52C2
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.flowAccumulator);
		Conduit.GetFlowManager(this.conduitType).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x0600547F RID: 21631 RVA: 0x0027B2C0 File Offset: 0x002794C0
	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.conduitType);
		ConduitFlow.Conduit conduit = flowManager.GetConduit(this.inputCell);
		if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
		{
			this.OnMassTransfer(0f);
			this.UpdateAnim();
			return;
		}
		ConduitFlow.ConduitContents contents = conduit.GetContents(flowManager);
		float num = Mathf.Min(contents.mass, this.currentFlow * dt);
		float num2 = 0f;
		if (num > 0f)
		{
			int disease_count = (int)(num / contents.mass * (float)contents.diseaseCount);
			num2 = flowManager.AddElement(this.outputCell, contents.element, num, contents.temperature, contents.diseaseIdx, disease_count);
			Game.Instance.accumulators.Accumulate(this.flowAccumulator, num2);
			if (num2 > 0f)
			{
				flowManager.RemoveElement(this.inputCell, num2);
			}
		}
		this.OnMassTransfer(num2);
		this.UpdateAnim();
	}

	// Token: 0x06005480 RID: 21632 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnMassTransfer(float amount)
	{
	}

	// Token: 0x06005481 RID: 21633 RVA: 0x0027B3B8 File Offset: 0x002795B8
	public virtual void UpdateAnim()
	{
		float averageRate = Game.Instance.accumulators.GetAverageRate(this.flowAccumulator);
		if (averageRate > 0f)
		{
			int i = 0;
			while (i < this.animFlowRanges.Length)
			{
				if (averageRate <= this.animFlowRanges[i].minFlow)
				{
					if (this.curFlowIdx != i)
					{
						this.curFlowIdx = i;
						this.controller.Play(this.animFlowRanges[i].animName, (averageRate <= 0f) ? KAnim.PlayMode.Once : KAnim.PlayMode.Loop, 1f, 0f);
						return;
					}
					return;
				}
				else
				{
					i++;
				}
			}
			return;
		}
		this.controller.Play("off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04003B23 RID: 15139
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x04003B24 RID: 15140
	[SerializeField]
	public float maxFlow = 0.5f;

	// Token: 0x04003B25 RID: 15141
	[Serialize]
	private float currentFlow;

	// Token: 0x04003B26 RID: 15142
	[MyCmpGet]
	protected KBatchedAnimController controller;

	// Token: 0x04003B27 RID: 15143
	protected HandleVector<int>.Handle flowAccumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04003B28 RID: 15144
	private int curFlowIdx = -1;

	// Token: 0x04003B29 RID: 15145
	private int inputCell;

	// Token: 0x04003B2A RID: 15146
	private int outputCell;

	// Token: 0x04003B2B RID: 15147
	[SerializeField]
	public ValveBase.AnimRangeInfo[] animFlowRanges;

	// Token: 0x0200102B RID: 4139
	[Serializable]
	public struct AnimRangeInfo
	{
		// Token: 0x06005483 RID: 21635 RVA: 0x000D7121 File Offset: 0x000D5321
		public AnimRangeInfo(float min_flow, string anim_name)
		{
			this.minFlow = min_flow;
			this.animName = anim_name;
		}

		// Token: 0x04003B2C RID: 15148
		public float minFlow;

		// Token: 0x04003B2D RID: 15149
		public string animName;
	}
}
