using System;
using KSerialization;

namespace ProcGenGame
{
	// Token: 0x02002099 RID: 8345
	[SerializationConfig(MemberSerialization.OptOut)]
	public struct Neighbors
	{
		// Token: 0x0600B152 RID: 45394 RVA: 0x001133B2 File Offset: 0x001115B2
		public Neighbors(TerrainCell a, TerrainCell b)
		{
			Debug.Assert(a != null && b != null, "NULL Neighbor");
			this.n0 = a;
			this.n1 = b;
		}

		// Token: 0x04008C2A RID: 35882
		public TerrainCell n0;

		// Token: 0x04008C2B RID: 35883
		public TerrainCell n1;
	}
}
