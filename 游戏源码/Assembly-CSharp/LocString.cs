using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class LocString
{
	[SerializeField]
	private string _text;

	[SerializeField]
	private StringKey _key;

	public const BindingFlags data_member_fields = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

	public string text => _text;

	public StringKey key => _key;

	public LocString(string text)
	{
		_text = text;
		_key = default(StringKey);
	}

	public LocString(string text, string keystring)
	{
		_text = text;
		_key = new StringKey(keystring);
	}

	public LocString(string text, bool isLocalized)
	{
		_text = text;
		_key = default(StringKey);
	}

	public static implicit operator LocString(string text)
	{
		return new LocString(text);
	}

	public static implicit operator string(LocString loc_string)
	{
		return loc_string.text;
	}

	public override string ToString()
	{
		return Strings.Get(key).String;
	}

	public void SetKey(string key_name)
	{
		_key = new StringKey(key_name);
	}

	public void SetKey(StringKey key)
	{
		_key = key;
	}

	public string Replace(string search, string replacement)
	{
		return ToString().Replace(search, replacement);
	}

	public static void CreateLocStringKeys(Type type, string parent_path = "STRINGS.")
	{
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		string text = parent_path;
		if (text == null)
		{
			text = "";
		}
		text = text + type.Name + ".";
		FieldInfo[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			if (!(fieldInfo.FieldType != typeof(LocString)))
			{
				if (!fieldInfo.IsStatic)
				{
					DebugUtil.DevLogError("LocString fields must be static, skipping. " + parent_path);
					continue;
				}
				string text2 = text + fieldInfo.Name;
				LocString locString = (LocString)fieldInfo.GetValue(null);
				locString.SetKey(text2);
				string text3 = locString.text;
				Strings.Add(text2, text3);
				fieldInfo.SetValue(null, locString);
			}
		}
		Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		for (int i = 0; i < nestedTypes.Length; i++)
		{
			CreateLocStringKeys(nestedTypes[i], text);
		}
	}

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
}
