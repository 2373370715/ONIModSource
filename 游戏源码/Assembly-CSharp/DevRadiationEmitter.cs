using System;
using STRINGS;

// Token: 0x02000D37 RID: 3383
public class DevRadiationEmitter : KMonoBehaviour, ISingleSliderControl, ISliderControl
{
	// Token: 0x06004226 RID: 16934 RVA: 0x000CAC30 File Offset: 0x000C8E30
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.radiationEmitter != null)
		{
			this.radiationEmitter.SetEmitting(true);
		}
	}

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x06004227 RID: 16935 RVA: 0x000CAC52 File Offset: 0x000C8E52
	public string SliderTitleKey
	{
		get
		{
			return BUILDINGS.PREFABS.DEVRADIATIONGENERATOR.NAME;
		}
	}

	// Token: 0x17000346 RID: 838
	// (get) Token: 0x06004228 RID: 16936 RVA: 0x000CAC5E File Offset: 0x000C8E5E
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.RADIATION.RADS;
		}
	}

	// Token: 0x06004229 RID: 16937 RVA: 0x000CAC6A File Offset: 0x000C8E6A
	public float GetSliderMax(int index)
	{
		return 5000f;
	}

	// Token: 0x0600422A RID: 16938 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x0600422B RID: 16939 RVA: 0x000CA99D File Offset: 0x000C8B9D
	public string GetSliderTooltip(int index)
	{
		return "";
	}

	// Token: 0x0600422C RID: 16940 RVA: 0x000CA99D File Offset: 0x000C8B9D
	public string GetSliderTooltipKey(int index)
	{
		return "";
	}

	// Token: 0x0600422D RID: 16941 RVA: 0x000CAC71 File Offset: 0x000C8E71
	public float GetSliderValue(int index)
	{
		return this.radiationEmitter.emitRads;
	}

	// Token: 0x0600422E RID: 16942 RVA: 0x000CAC7E File Offset: 0x000C8E7E
	public void SetSliderValue(float value, int index)
	{
		this.radiationEmitter.emitRads = value;
		this.radiationEmitter.Refresh();
	}

	// Token: 0x0600422F RID: 16943 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x04002D0D RID: 11533
	[MyCmpReq]
	private RadiationEmitter radiationEmitter;
}
