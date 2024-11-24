public class FossilMineWorkable : ComplexFabricatorWorkable
{
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		shouldShowSkillPerkStatusItem = false;
	}
}
