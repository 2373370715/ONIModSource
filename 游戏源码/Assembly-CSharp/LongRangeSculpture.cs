public class LongRangeSculpture : Sculpture
{
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		overrideAnims = null;
		SetOffsetTable(OffsetGroups.InvertedStandardTable);
		multitoolContext = "paint";
		multitoolHitEffectTag = "fx_paint_splash";
	}
}
