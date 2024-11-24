using System;
using System.Collections.Generic;
using ProcGen.Map;
using ProcGenGame;
using VoronoiTree;

namespace Klei
{
	// Token: 0x02003AED RID: 15085
	public class TerrainCellLogged : TerrainCell
	{
		// Token: 0x0600E7D6 RID: 59350 RVA: 0x0013B18C File Offset: 0x0013938C
		public TerrainCellLogged()
		{
		}

		// Token: 0x0600E7D7 RID: 59351 RVA: 0x0013B194 File Offset: 0x00139394
		public TerrainCellLogged(Cell node, Diagram.Site site, Dictionary<Tag, int> distancesToTags) : base(node, site, distancesToTags)
		{
		}

		// Token: 0x0600E7D8 RID: 59352 RVA: 0x000A5E40 File Offset: 0x000A4040
		public override void LogInfo(string evt, string param, float value)
		{
		}
	}
}
