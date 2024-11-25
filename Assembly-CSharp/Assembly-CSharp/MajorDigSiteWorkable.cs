﻿using System;

public class MajorDigSiteWorkable : FossilExcavationWorkable
{
		protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(90f);
	}

		protected override void OnSpawn()
	{
		this.digsite = base.gameObject.GetSMI<MajorFossilDigSite.Instance>();
		base.OnSpawn();
	}

		protected override bool IsMarkedForExcavation()
	{
		return this.digsite != null && !this.digsite.sm.IsRevealed.Get(this.digsite) && this.digsite.sm.MarkedForDig.Get(this.digsite);
	}

		private MajorFossilDigSite.Instance digsite;
}
