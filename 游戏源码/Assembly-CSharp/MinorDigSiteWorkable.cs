public class MinorDigSiteWorkable : FossilExcavationWorkable
{
	private MinorFossilDigSite.Instance digsite;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SetWorkTime(90f);
	}

	protected override void OnSpawn()
	{
		digsite = base.gameObject.GetSMI<MinorFossilDigSite.Instance>();
		base.OnSpawn();
	}

	protected override bool IsMarkedForExcavation()
	{
		if (digsite != null)
		{
			if (!digsite.sm.IsRevealed.Get(digsite))
			{
				return digsite.sm.MarkedForDig.Get(digsite);
			}
			return false;
		}
		return false;
	}
}
