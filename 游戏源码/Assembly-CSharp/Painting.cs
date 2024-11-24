using System;

// Token: 0x02000ABF RID: 2751
public class Painting : Artable
{
	// Token: 0x06003338 RID: 13112 RVA: 0x000C172C File Offset: 0x000BF92C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.multitoolContext = "paint";
		this.multitoolHitEffectTag = "fx_paint_splash";
	}

	// Token: 0x06003339 RID: 13113 RVA: 0x000C175F File Offset: 0x000BF95F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Paintings.Add(this);
	}

	// Token: 0x0600333A RID: 13114 RVA: 0x000C1772 File Offset: 0x000BF972
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Paintings.Remove(this);
	}

	// Token: 0x0600333B RID: 13115 RVA: 0x000C1785 File Offset: 0x000BF985
	public override void SetStage(string stage_id, bool skip_effect)
	{
		base.SetStage(stage_id, skip_effect);
		if (Db.GetArtableStages().Get(stage_id) == null)
		{
			Debug.LogError("Missing stage: " + stage_id);
		}
	}
}
