using System;

// Token: 0x0200063B RID: 1595
public class CreatureBrain : Brain
{
	// Token: 0x06001D0E RID: 7438 RVA: 0x001ADEDC File Offset: 0x001AC0DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Navigator component = base.GetComponent<Navigator>();
		if (component != null)
		{
			component.SetAbilities(new CreaturePathFinderAbilities(component));
		}
	}

	// Token: 0x04001217 RID: 4631
	public string symbolPrefix;

	// Token: 0x04001218 RID: 4632
	public Tag species;
}
