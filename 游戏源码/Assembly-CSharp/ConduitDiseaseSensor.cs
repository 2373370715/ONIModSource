using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000D05 RID: 3333
[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitDiseaseSensor : ConduitThresholdSensor, IThresholdSwitch
{
	// Token: 0x06004111 RID: 16657 RVA: 0x0023CC78 File Offset: 0x0023AE78
	protected override void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			if (this.switchedOn)
			{
				this.animController.Play(ConduitSensor.ON_ANIMS, KAnim.PlayMode.Loop);
				int num;
				int num2;
				bool flag;
				this.GetContentsDisease(out num, out num2, out flag);
				Color32 c = Color.white;
				if (num != 255)
				{
					Disease disease = Db.Get().Diseases[num];
					c = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
				}
				this.animController.SetSymbolTint(ConduitDiseaseSensor.TINT_SYMBOL, c);
				return;
			}
			this.animController.Play(ConduitSensor.OFF_ANIMS, KAnim.PlayMode.Once);
		}
	}

	// Token: 0x06004112 RID: 16658 RVA: 0x0023CD38 File Offset: 0x0023AF38
	private void GetContentsDisease(out int diseaseIdx, out int diseaseCount, out bool hasMass)
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
			diseaseIdx = (int)contents.diseaseIdx;
			diseaseCount = contents.diseaseCount;
			hasMass = (contents.mass > 0f);
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents2.pickupableHandle);
		if (pickupable != null && pickupable.PrimaryElement.Mass > 0f)
		{
			diseaseIdx = (int)pickupable.PrimaryElement.DiseaseIdx;
			diseaseCount = pickupable.PrimaryElement.DiseaseCount;
			hasMass = true;
			return;
		}
		diseaseIdx = 0;
		diseaseCount = 0;
		hasMass = false;
	}

	// Token: 0x17000323 RID: 803
	// (get) Token: 0x06004113 RID: 16659 RVA: 0x0023CDEC File Offset: 0x0023AFEC
	public override float CurrentValue
	{
		get
		{
			int num;
			int num2;
			bool flag;
			this.GetContentsDisease(out num, out num2, out flag);
			if (flag)
			{
				this.lastValue = (float)num2;
			}
			return this.lastValue;
		}
	}

	// Token: 0x17000324 RID: 804
	// (get) Token: 0x06004114 RID: 16660 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float RangeMin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x06004115 RID: 16661 RVA: 0x000CA1CC File Offset: 0x000C83CC
	public float RangeMax
	{
		get
		{
			return 100000f;
		}
	}

	// Token: 0x06004116 RID: 16662 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float GetRangeMinInputField()
	{
		return 0f;
	}

	// Token: 0x06004117 RID: 16663 RVA: 0x000CA1CC File Offset: 0x000C83CC
	public float GetRangeMaxInputField()
	{
		return 100000f;
	}

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x06004118 RID: 16664 RVA: 0x000CA1D3 File Offset: 0x000C83D3
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TITLE;
		}
	}

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x06004119 RID: 16665 RVA: 0x000CA1DA File Offset: 0x000C83DA
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_DISEASE;
		}
	}

	// Token: 0x17000328 RID: 808
	// (get) Token: 0x0600411A RID: 16666 RVA: 0x000CA1E1 File Offset: 0x000C83E1
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000329 RID: 809
	// (get) Token: 0x0600411B RID: 16667 RVA: 0x000CA1ED File Offset: 0x000C83ED
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x0600411C RID: 16668 RVA: 0x000CA1F9 File Offset: 0x000C83F9
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedInt((float)((int)value), GameUtil.TimeSlice.None);
	}

	// Token: 0x0600411D RID: 16669 RVA: 0x000B1FA8 File Offset: 0x000B01A8
	public float ProcessedSliderValue(float input)
	{
		return input;
	}

	// Token: 0x0600411E RID: 16670 RVA: 0x000B1FA8 File Offset: 0x000B01A8
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x0600411F RID: 16671 RVA: 0x000CA204 File Offset: 0x000C8404
	public LocString ThresholdValueUnits()
	{
		return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_UNITS;
	}

	// Token: 0x1700032A RID: 810
	// (get) Token: 0x06004120 RID: 16672 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x1700032B RID: 811
	// (get) Token: 0x06004121 RID: 16673 RVA: 0x000A65EC File Offset: 0x000A47EC
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700032C RID: 812
	// (get) Token: 0x06004122 RID: 16674 RVA: 0x000CA20B File Offset: 0x000C840B
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x04002C6A RID: 11370
	private const float rangeMin = 0f;

	// Token: 0x04002C6B RID: 11371
	private const float rangeMax = 100000f;

	// Token: 0x04002C6C RID: 11372
	[Serialize]
	private float lastValue;

	// Token: 0x04002C6D RID: 11373
	private static readonly HashedString TINT_SYMBOL = "germs";
}
