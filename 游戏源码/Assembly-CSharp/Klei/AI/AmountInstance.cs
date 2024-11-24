using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003AFC RID: 15100
	[SerializationConfig(MemberSerialization.OptIn)]
	[DebuggerDisplay("{amount.Name} {value} ({deltaAttribute.value}/{minAttribute.value}/{maxAttribute.value})")]
	public class AmountInstance : ModifierInstance<Amount>, ISaveLoadable, ISim200ms
	{
		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x0600E839 RID: 59449 RVA: 0x0013B580 File Offset: 0x00139780
		public Amount amount
		{
			get
			{
				return this.modifier;
			}
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x0600E83A RID: 59450 RVA: 0x0013B588 File Offset: 0x00139788
		// (set) Token: 0x0600E83B RID: 59451 RVA: 0x0013B590 File Offset: 0x00139790
		public bool paused
		{
			get
			{
				return this._paused;
			}
			set
			{
				this._paused = this.paused;
				if (this._paused)
				{
					this.Deactivate();
					return;
				}
				this.Activate();
			}
		}

		// Token: 0x0600E83C RID: 59452 RVA: 0x0013B5B3 File Offset: 0x001397B3
		public float GetMin()
		{
			return this.minAttribute.GetTotalValue();
		}

		// Token: 0x0600E83D RID: 59453 RVA: 0x0013B5C0 File Offset: 0x001397C0
		public float GetMax()
		{
			return this.maxAttribute.GetTotalValue();
		}

		// Token: 0x0600E83E RID: 59454 RVA: 0x0013B5CD File Offset: 0x001397CD
		public float GetDelta()
		{
			return this.deltaAttribute.GetTotalValue();
		}

		// Token: 0x0600E83F RID: 59455 RVA: 0x004C0828 File Offset: 0x004BEA28
		public AmountInstance(Amount amount, GameObject game_object) : base(game_object, amount)
		{
			Attributes attributes = game_object.GetAttributes();
			this.minAttribute = attributes.Add(amount.minAttribute);
			this.maxAttribute = attributes.Add(amount.maxAttribute);
			this.deltaAttribute = attributes.Add(amount.deltaAttribute);
		}

		// Token: 0x0600E840 RID: 59456 RVA: 0x0013B5DA File Offset: 0x001397DA
		public float SetValue(float value)
		{
			this.value = Mathf.Min(Mathf.Max(value, this.GetMin()), this.GetMax());
			return this.value;
		}

		// Token: 0x0600E841 RID: 59457 RVA: 0x004C087C File Offset: 0x004BEA7C
		public void Publish(float delta, float previous_value)
		{
			if (this.OnDelta != null)
			{
				this.OnDelta(delta);
			}
			if (this.OnValueChanged != null && previous_value != this.value)
			{
				float obj = this.value - previous_value;
				this.OnValueChanged(obj);
			}
			if (this.OnMaxValueReached != null && previous_value < this.GetMax() && this.value >= this.GetMax())
			{
				this.OnMaxValueReached();
			}
			if (this.OnMinValueReached != null && previous_value > this.GetMin() && this.value <= this.GetMin())
			{
				this.OnMinValueReached();
			}
		}

		// Token: 0x0600E842 RID: 59458 RVA: 0x004C0918 File Offset: 0x004BEB18
		public float ApplyDelta(float delta)
		{
			float previous_value = this.value;
			this.SetValue(this.value + delta);
			this.Publish(delta, previous_value);
			return this.value;
		}

		// Token: 0x0600E843 RID: 59459 RVA: 0x0013B5FF File Offset: 0x001397FF
		public string GetValueString()
		{
			return this.amount.GetValueString(this);
		}

		// Token: 0x0600E844 RID: 59460 RVA: 0x0013B60D File Offset: 0x0013980D
		public string GetDescription()
		{
			return this.amount.GetDescription(this);
		}

		// Token: 0x0600E845 RID: 59461 RVA: 0x0013B61B File Offset: 0x0013981B
		public string GetTooltip()
		{
			return this.amount.GetTooltip(this);
		}

		// Token: 0x0600E846 RID: 59462 RVA: 0x0013B629 File Offset: 0x00139829
		public void Activate()
		{
			SimAndRenderScheduler.instance.Add(this, false);
		}

		// Token: 0x0600E847 RID: 59463 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void Sim200ms(float dt)
		{
		}

		// Token: 0x0600E848 RID: 59464 RVA: 0x004C094C File Offset: 0x004BEB4C
		public static void BatchUpdate(List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances, float time_delta)
		{
			if (time_delta == 0f)
			{
				return;
			}
			AmountInstance.BatchUpdateContext batchUpdateContext = new AmountInstance.BatchUpdateContext(amount_instances, time_delta);
			AmountInstance.batch_update_job.Reset(batchUpdateContext);
			int num = 512;
			for (int i = 0; i < amount_instances.Count; i += num)
			{
				int num2 = i + num;
				if (amount_instances.Count < num2)
				{
					num2 = amount_instances.Count;
				}
				AmountInstance.batch_update_job.Add(new AmountInstance.BatchUpdateTask(i, num2));
			}
			GlobalJobManager.Run(AmountInstance.batch_update_job);
			batchUpdateContext.Finish();
			AmountInstance.batch_update_job.Reset(null);
		}

		// Token: 0x0600E849 RID: 59465 RVA: 0x000C09DD File Offset: 0x000BEBDD
		public void Deactivate()
		{
			SimAndRenderScheduler.instance.Remove(this);
		}

		// Token: 0x0400E40C RID: 58380
		[Serialize]
		public float value;

		// Token: 0x0400E40D RID: 58381
		public AttributeInstance minAttribute;

		// Token: 0x0400E40E RID: 58382
		public AttributeInstance maxAttribute;

		// Token: 0x0400E40F RID: 58383
		public AttributeInstance deltaAttribute;

		// Token: 0x0400E410 RID: 58384
		public Action<float> OnDelta;

		// Token: 0x0400E411 RID: 58385
		public Action<float> OnValueChanged;

		// Token: 0x0400E412 RID: 58386
		public System.Action OnMaxValueReached;

		// Token: 0x0400E413 RID: 58387
		public System.Action OnMinValueReached;

		// Token: 0x0400E414 RID: 58388
		public bool hide;

		// Token: 0x0400E415 RID: 58389
		private bool _paused;

		// Token: 0x0400E416 RID: 58390
		private static WorkItemCollection<AmountInstance.BatchUpdateTask, AmountInstance.BatchUpdateContext> batch_update_job = new WorkItemCollection<AmountInstance.BatchUpdateTask, AmountInstance.BatchUpdateContext>();

		// Token: 0x02003AFD RID: 15101
		private class BatchUpdateContext
		{
			// Token: 0x0600E84B RID: 59467 RVA: 0x004C09CC File Offset: 0x004BEBCC
			public BatchUpdateContext(List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances, float time_delta)
			{
				for (int num = 0; num != amount_instances.Count; num++)
				{
					UpdateBucketWithUpdater<ISim200ms>.Entry value = amount_instances[num];
					value.lastUpdateTime = 0f;
					amount_instances[num] = value;
				}
				this.amount_instances = amount_instances;
				this.time_delta = time_delta;
				this.results = ListPool<AmountInstance.BatchUpdateContext.Result, AmountInstance>.Allocate();
				this.results.Capacity = this.amount_instances.Count;
			}

			// Token: 0x0600E84C RID: 59468 RVA: 0x004C0A3C File Offset: 0x004BEC3C
			public void Finish()
			{
				foreach (AmountInstance.BatchUpdateContext.Result result in this.results)
				{
					result.amount_instance.Publish(result.delta, result.previous);
				}
				this.results.Recycle();
			}

			// Token: 0x0400E417 RID: 58391
			public List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances;

			// Token: 0x0400E418 RID: 58392
			public float time_delta;

			// Token: 0x0400E419 RID: 58393
			public ListPool<AmountInstance.BatchUpdateContext.Result, AmountInstance>.PooledList results;

			// Token: 0x02003AFE RID: 15102
			public struct Result
			{
				// Token: 0x0400E41A RID: 58394
				public AmountInstance amount_instance;

				// Token: 0x0400E41B RID: 58395
				public float previous;

				// Token: 0x0400E41C RID: 58396
				public float delta;
			}
		}

		// Token: 0x02003AFF RID: 15103
		private struct BatchUpdateTask : IWorkItem<AmountInstance.BatchUpdateContext>
		{
			// Token: 0x0600E84D RID: 59469 RVA: 0x0013B643 File Offset: 0x00139843
			public BatchUpdateTask(int start, int end)
			{
				this.start = start;
				this.end = end;
			}

			// Token: 0x0600E84E RID: 59470 RVA: 0x004C0AAC File Offset: 0x004BECAC
			public void Run(AmountInstance.BatchUpdateContext context)
			{
				for (int num = this.start; num != this.end; num++)
				{
					AmountInstance amountInstance = (AmountInstance)context.amount_instances[num].data;
					float num2 = amountInstance.GetDelta() * context.time_delta;
					if (num2 != 0f)
					{
						context.results.Add(new AmountInstance.BatchUpdateContext.Result
						{
							amount_instance = amountInstance,
							previous = amountInstance.value,
							delta = num2
						});
						amountInstance.SetValue(amountInstance.value + num2);
					}
				}
			}

			// Token: 0x0400E41D RID: 58397
			private int start;

			// Token: 0x0400E41E RID: 58398
			private int end;
		}
	}
}
