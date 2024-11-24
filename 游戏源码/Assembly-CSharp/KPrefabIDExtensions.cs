using System;
using UnityEngine;

// Token: 0x02001486 RID: 5254
public static class KPrefabIDExtensions
{
	// Token: 0x06006CEB RID: 27883 RVA: 0x000E7770 File Offset: 0x000E5970
	public static Tag PrefabID(this Component cmp)
	{
		return cmp.GetComponent<KPrefabID>().PrefabID();
	}

	// Token: 0x06006CEC RID: 27884 RVA: 0x000E777D File Offset: 0x000E597D
	public static Tag PrefabID(this GameObject go)
	{
		return go.GetComponent<KPrefabID>().PrefabID();
	}

	// Token: 0x06006CED RID: 27885 RVA: 0x000E778A File Offset: 0x000E598A
	public static Tag PrefabID(this StateMachine.Instance smi)
	{
		return smi.GetComponent<KPrefabID>().PrefabID();
	}

	// Token: 0x06006CEE RID: 27886 RVA: 0x000E7797 File Offset: 0x000E5997
	public static bool IsPrefabID(this Component cmp, Tag id)
	{
		return cmp.GetComponent<KPrefabID>().IsPrefabID(id);
	}

	// Token: 0x06006CEF RID: 27887 RVA: 0x000E77A5 File Offset: 0x000E59A5
	public static bool IsPrefabID(this GameObject go, Tag id)
	{
		return go.GetComponent<KPrefabID>().IsPrefabID(id);
	}

	// Token: 0x06006CF0 RID: 27888 RVA: 0x000E77B3 File Offset: 0x000E59B3
	public static bool HasTag(this Component cmp, Tag tag)
	{
		return cmp.GetComponent<KPrefabID>().HasTag(tag);
	}

	// Token: 0x06006CF1 RID: 27889 RVA: 0x000E77C1 File Offset: 0x000E59C1
	public static bool HasTag(this GameObject go, Tag tag)
	{
		return go.GetComponent<KPrefabID>().HasTag(tag);
	}

	// Token: 0x06006CF2 RID: 27890 RVA: 0x000E77CF File Offset: 0x000E59CF
	public static bool HasAnyTags(this Component cmp, Tag[] tags)
	{
		return cmp.GetComponent<KPrefabID>().HasAnyTags(tags);
	}

	// Token: 0x06006CF3 RID: 27891 RVA: 0x000E77DD File Offset: 0x000E59DD
	public static bool HasAnyTags(this GameObject go, Tag[] tags)
	{
		return go.GetComponent<KPrefabID>().HasAnyTags(tags);
	}

	// Token: 0x06006CF4 RID: 27892 RVA: 0x000E77EB File Offset: 0x000E59EB
	public static bool HasAllTags(this Component cmp, Tag[] tags)
	{
		return cmp.GetComponent<KPrefabID>().HasAllTags(tags);
	}

	// Token: 0x06006CF5 RID: 27893 RVA: 0x000E77F9 File Offset: 0x000E59F9
	public static bool HasAllTags(this GameObject go, Tag[] tags)
	{
		return go.GetComponent<KPrefabID>().HasAllTags(tags);
	}

	// Token: 0x06006CF6 RID: 27894 RVA: 0x000E7807 File Offset: 0x000E5A07
	public static void AddTag(this GameObject go, Tag tag)
	{
		go.GetComponent<KPrefabID>().AddTag(tag, false);
	}

	// Token: 0x06006CF7 RID: 27895 RVA: 0x000E7816 File Offset: 0x000E5A16
	public static void AddTag(this Component cmp, Tag tag)
	{
		cmp.GetComponent<KPrefabID>().AddTag(tag, false);
	}

	// Token: 0x06006CF8 RID: 27896 RVA: 0x000E7825 File Offset: 0x000E5A25
	public static void RemoveTag(this GameObject go, Tag tag)
	{
		go.GetComponent<KPrefabID>().RemoveTag(tag);
	}

	// Token: 0x06006CF9 RID: 27897 RVA: 0x000E7833 File Offset: 0x000E5A33
	public static void RemoveTag(this Component cmp, Tag tag)
	{
		cmp.GetComponent<KPrefabID>().RemoveTag(tag);
	}
}
