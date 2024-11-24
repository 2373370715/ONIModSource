using System;

namespace Klei.AI
{
	// Token: 0x02003B96 RID: 15254
	public class TraitGroup : ModifierGroup<Trait>
	{
		// Token: 0x0600EAF4 RID: 60148 RVA: 0x0013D130 File Offset: 0x0013B330
		public TraitGroup(string id, string name, bool is_spawn_trait) : base(id, name)
		{
			this.IsSpawnTrait = is_spawn_trait;
		}

		// Token: 0x0400E62B RID: 58923
		public bool IsSpawnTrait;
	}
}
