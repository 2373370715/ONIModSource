using System;
using System.Diagnostics;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003AFB RID: 15099
	[DebuggerDisplay("{Id}")]
	public class Amount : Resource
	{
		// Token: 0x0600E830 RID: 59440 RVA: 0x004C0770 File Offset: 0x004BE970
		public Amount(string id, string name, string description, Attribute min_attribute, Attribute max_attribute, Attribute delta_attribute, bool show_max, Units units, float visual_delta_threshold, bool show_in_ui, string uiSprite = null, string thoughtSprite = null)
		{
			this.Id = id;
			this.Name = name;
			this.description = description;
			this.minAttribute = min_attribute;
			this.maxAttribute = max_attribute;
			this.deltaAttribute = delta_attribute;
			this.showMax = show_max;
			this.units = units;
			this.visualDeltaThreshold = visual_delta_threshold;
			this.showInUI = show_in_ui;
			this.uiSprite = uiSprite;
			this.thoughtSprite = thoughtSprite;
		}

		// Token: 0x0600E831 RID: 59441 RVA: 0x0013B4E9 File Offset: 0x001396E9
		public void SetDisplayer(IAmountDisplayer displayer)
		{
			this.displayer = displayer;
			this.minAttribute.SetFormatter(displayer.Formatter);
			this.maxAttribute.SetFormatter(displayer.Formatter);
			this.deltaAttribute.SetFormatter(displayer.Formatter);
		}

		// Token: 0x0600E832 RID: 59442 RVA: 0x0013B525 File Offset: 0x00139725
		public AmountInstance Lookup(Component cmp)
		{
			return this.Lookup(cmp.gameObject);
		}

		// Token: 0x0600E833 RID: 59443 RVA: 0x004C07E0 File Offset: 0x004BE9E0
		public AmountInstance Lookup(GameObject go)
		{
			Amounts amounts = go.GetAmounts();
			if (amounts != null)
			{
				return amounts.Get(this);
			}
			return null;
		}

		// Token: 0x0600E834 RID: 59444 RVA: 0x004C0800 File Offset: 0x004BEA00
		public void Copy(GameObject to, GameObject from)
		{
			AmountInstance amountInstance = this.Lookup(to);
			AmountInstance amountInstance2 = this.Lookup(from);
			amountInstance.value = amountInstance2.value;
		}

		// Token: 0x0600E835 RID: 59445 RVA: 0x0013B533 File Offset: 0x00139733
		public string GetValueString(AmountInstance instance)
		{
			return this.displayer.GetValueString(this, instance);
		}

		// Token: 0x0600E836 RID: 59446 RVA: 0x0013B542 File Offset: 0x00139742
		public string GetDescription(AmountInstance instance)
		{
			return this.displayer.GetDescription(this, instance);
		}

		// Token: 0x0600E837 RID: 59447 RVA: 0x0013B551 File Offset: 0x00139751
		public string GetTooltip(AmountInstance instance)
		{
			return this.displayer.GetTooltip(this, instance);
		}

		// Token: 0x0600E838 RID: 59448 RVA: 0x0013B560 File Offset: 0x00139760
		public void DebugSetValue(AmountInstance instance, float value)
		{
			if (this.debugSetValue != null)
			{
				this.debugSetValue(instance, value);
				return;
			}
			instance.SetValue(value);
		}

		// Token: 0x0400E400 RID: 58368
		public string description;

		// Token: 0x0400E401 RID: 58369
		public bool showMax;

		// Token: 0x0400E402 RID: 58370
		public Units units;

		// Token: 0x0400E403 RID: 58371
		public float visualDeltaThreshold;

		// Token: 0x0400E404 RID: 58372
		public Attribute minAttribute;

		// Token: 0x0400E405 RID: 58373
		public Attribute maxAttribute;

		// Token: 0x0400E406 RID: 58374
		public Attribute deltaAttribute;

		// Token: 0x0400E407 RID: 58375
		public Action<AmountInstance, float> debugSetValue;

		// Token: 0x0400E408 RID: 58376
		public bool showInUI;

		// Token: 0x0400E409 RID: 58377
		public string uiSprite;

		// Token: 0x0400E40A RID: 58378
		public string thoughtSprite;

		// Token: 0x0400E40B RID: 58379
		public IAmountDisplayer displayer;
	}
}
