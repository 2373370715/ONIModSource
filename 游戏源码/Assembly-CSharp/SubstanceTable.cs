using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020019CB RID: 6603
public class SubstanceTable : ScriptableObject, ISerializationCallbackReceiver
{
	// Token: 0x06008988 RID: 35208 RVA: 0x000FA143 File Offset: 0x000F8343
	public List<Substance> GetList()
	{
		return this.list;
	}

	// Token: 0x06008989 RID: 35209 RVA: 0x00357FC8 File Offset: 0x003561C8
	public Substance GetSubstance(SimHashes substance)
	{
		int count = this.list.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.list[i].elementID == substance)
			{
				return this.list[i];
			}
		}
		return null;
	}

	// Token: 0x0600898A RID: 35210 RVA: 0x000FA14B File Offset: 0x000F834B
	public void OnBeforeSerialize()
	{
		this.BindAnimList();
	}

	// Token: 0x0600898B RID: 35211 RVA: 0x000FA14B File Offset: 0x000F834B
	public void OnAfterDeserialize()
	{
		this.BindAnimList();
	}

	// Token: 0x0600898C RID: 35212 RVA: 0x00358010 File Offset: 0x00356210
	private void BindAnimList()
	{
		foreach (Substance substance in this.list)
		{
			if (substance.anim != null && (substance.anims == null || substance.anims.Length == 0))
			{
				substance.anims = new KAnimFile[1];
				substance.anims[0] = substance.anim;
			}
		}
	}

	// Token: 0x0600898D RID: 35213 RVA: 0x000FA153 File Offset: 0x000F8353
	public void RemoveDuplicates()
	{
		this.list = this.list.Distinct(new SubstanceTable.SubstanceEqualityComparer()).ToList<Substance>();
	}

	// Token: 0x04006791 RID: 26513
	[SerializeField]
	private List<Substance> list;

	// Token: 0x04006792 RID: 26514
	public Material solidMaterial;

	// Token: 0x04006793 RID: 26515
	public Material liquidMaterial;

	// Token: 0x020019CC RID: 6604
	private class SubstanceEqualityComparer : IEqualityComparer<Substance>
	{
		// Token: 0x0600898F RID: 35215 RVA: 0x000FA170 File Offset: 0x000F8370
		public bool Equals(Substance x, Substance y)
		{
			return x.elementID.Equals(y.elementID);
		}

		// Token: 0x06008990 RID: 35216 RVA: 0x000FA18E File Offset: 0x000F838E
		public int GetHashCode(Substance obj)
		{
			return obj.elementID.GetHashCode();
		}
	}
}
