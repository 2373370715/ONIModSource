using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02002177 RID: 8567
	public class TechTreeTitles : ResourceSet<TechTreeTitle>
	{
		// Token: 0x0600B628 RID: 46632 RVA: 0x00115643 File Offset: 0x00113843
		public TechTreeTitles(ResourceSet parent) : base("TreeTitles", parent)
		{
		}

		// Token: 0x0600B629 RID: 46633 RVA: 0x00456FAC File Offset: 0x004551AC
		public void Load(TextAsset tree_file)
		{
			foreach (ResourceTreeNode resourceTreeNode in new ResourceTreeLoader<ResourceTreeNode>(tree_file))
			{
				if (string.Equals(resourceTreeNode.Id.Substring(0, 1), "_"))
				{
					new TechTreeTitle(resourceTreeNode.Id, this, Strings.Get("STRINGS.RESEARCH.TREES.TITLE" + resourceTreeNode.Id.ToUpper()), resourceTreeNode);
				}
			}
		}
	}
}
