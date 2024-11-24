using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x0200149F RID: 5279
[Serializable]
public class LocString
{
	// Token: 0x17000705 RID: 1797
	// (get) Token: 0x06006DCC RID: 28108 RVA: 0x000E7F6E File Offset: 0x000E616E
	public string text
	{
		get
		{
			return this._text;
		}
	}

	// Token: 0x17000706 RID: 1798
	// (get) Token: 0x06006DCD RID: 28109 RVA: 0x000E7F76 File Offset: 0x000E6176
	public StringKey key
	{
		get
		{
			return this._key;
		}
	}

	// Token: 0x06006DCE RID: 28110 RVA: 0x000E7F7E File Offset: 0x000E617E
	public LocString(string text)
	{
		this._text = text;
		this._key = default(StringKey);
	}

	// Token: 0x06006DCF RID: 28111 RVA: 0x000E7F99 File Offset: 0x000E6199
	public LocString(string text, string keystring)
	{
		this._text = text;
		this._key = new StringKey(keystring);
	}

	// Token: 0x06006DD0 RID: 28112 RVA: 0x000E7F7E File Offset: 0x000E617E
	public LocString(string text, bool isLocalized)
	{
		this._text = text;
		this._key = default(StringKey);
	}

	// Token: 0x06006DD1 RID: 28113 RVA: 0x000E7FB4 File Offset: 0x000E61B4
	public static implicit operator LocString(string text)
	{
		return new LocString(text);
	}

	// Token: 0x06006DD2 RID: 28114 RVA: 0x000E7FBC File Offset: 0x000E61BC
	public static implicit operator string(LocString loc_string)
	{
		return loc_string.text;
	}

	// Token: 0x06006DD3 RID: 28115 RVA: 0x000E7FC4 File Offset: 0x000E61C4
	public override string ToString()
	{
		return Strings.Get(this.key).String;
	}

	// Token: 0x06006DD4 RID: 28116 RVA: 0x000E7FD6 File Offset: 0x000E61D6
	public void SetKey(string key_name)
	{
		this._key = new StringKey(key_name);
	}

	// Token: 0x06006DD5 RID: 28117 RVA: 0x000E7FE4 File Offset: 0x000E61E4
	public void SetKey(StringKey key)
	{
		this._key = key;
	}

	// Token: 0x06006DD6 RID: 28118 RVA: 0x000E7FED File Offset: 0x000E61ED
	public string Replace(string search, string replacement)
	{
		return this.ToString().Replace(search, replacement);
	}

	// Token: 0x06006DD7 RID: 28119 RVA: 0x002EC878 File Offset: 0x002EAA78
	public static void CreateLocStringKeys(Type type, string parent_path = "STRINGS.")
	{
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		string text = parent_path;
		if (text == null)
		{
			text = "";
		}
		text = text + type.Name + ".";
		foreach (FieldInfo fieldInfo in fields)
		{
			if (!(fieldInfo.FieldType != typeof(LocString)))
			{
				if (!fieldInfo.IsStatic)
				{
					DebugUtil.DevLogError("LocString fields must be static, skipping. " + parent_path);
				}
				else
				{
					string text2 = text + fieldInfo.Name;
					LocString locString = (LocString)fieldInfo.GetValue(null);
					locString.SetKey(text2);
					string text3 = locString.text;
					Strings.Add(new string[]
					{
						text2,
						text3
					});
					fieldInfo.SetValue(null, locString);
				}
			}
		}
		Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		for (int i = 0; i < nestedTypes.Length; i++)
		{
			LocString.CreateLocStringKeys(nestedTypes[i], text);
		}
	}

	// Token: 0x06006DD8 RID: 28120 RVA: 0x002EC964 File Offset: 0x002EAB64
	public static string[] GetStrings(Type type)
	{
		List<string> list = new List<string>();
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		for (int i = 0; i < fields.Length; i++)
		{
			LocString locString = (LocString)fields[i].GetValue(null);
			list.Add(locString.text);
		}
		return list.ToArray();
	}

	// Token: 0x04005237 RID: 21047
	[SerializeField]
	private string _text;

	// Token: 0x04005238 RID: 21048
	[SerializeField]
	private StringKey _key;

	// Token: 0x04005239 RID: 21049
	public const BindingFlags data_member_fields = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
}
