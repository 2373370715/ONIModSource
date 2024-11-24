using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x020005D8 RID: 1496
public class CodeWriter
{
	// Token: 0x06001ADA RID: 6874 RVA: 0x000B1733 File Offset: 0x000AF933
	public CodeWriter(string path)
	{
		this.Path = path;
	}

	// Token: 0x06001ADB RID: 6875 RVA: 0x000B174D File Offset: 0x000AF94D
	public void Comment(string text)
	{
		this.Lines.Add("// " + text);
	}

	// Token: 0x06001ADC RID: 6876 RVA: 0x001A97EC File Offset: 0x001A79EC
	public void BeginPartialClass(string class_name, string parent_name = null)
	{
		string text = "public partial class " + class_name;
		if (parent_name != null)
		{
			text = text + " : " + parent_name;
		}
		this.Line(text);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001ADD RID: 6877 RVA: 0x001A9838 File Offset: 0x001A7A38
	public void BeginClass(string class_name, string parent_name = null)
	{
		string text = "public class " + class_name;
		if (parent_name != null)
		{
			text = text + " : " + parent_name;
		}
		this.Line(text);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001ADE RID: 6878 RVA: 0x000B1765 File Offset: 0x000AF965
	public void EndClass()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001ADF RID: 6879 RVA: 0x000B1780 File Offset: 0x000AF980
	public void BeginNameSpace(string name)
	{
		this.Line("namespace " + name);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AE0 RID: 6880 RVA: 0x000B1765 File Offset: 0x000AF965
	public void EndNameSpace()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001AE1 RID: 6881 RVA: 0x000B17AC File Offset: 0x000AF9AC
	public void BeginArrayStructureInitialization(string name)
	{
		this.Line("new " + name);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x000B17D8 File Offset: 0x000AF9D8
	public void EndArrayStructureInitialization(bool last_item)
	{
		this.Indent--;
		if (!last_item)
		{
			this.Line("},");
			return;
		}
		this.Line("}");
	}

	// Token: 0x06001AE3 RID: 6883 RVA: 0x000B1802 File Offset: 0x000AFA02
	public void BeginArraArrayInitialization(string array_type, string array_name)
	{
		this.Line(array_name + " = new " + array_type + "[]");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AE4 RID: 6884 RVA: 0x000B1834 File Offset: 0x000AFA34
	public void EndArrayArrayInitialization(bool last_item)
	{
		this.Indent--;
		if (last_item)
		{
			this.Line("}");
			return;
		}
		this.Line("},");
	}

	// Token: 0x06001AE5 RID: 6885 RVA: 0x000B185E File Offset: 0x000AFA5E
	public void BeginConstructor(string name)
	{
		this.Line("public " + name + "()");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AE6 RID: 6886 RVA: 0x000B1765 File Offset: 0x000AF965
	public void EndConstructor()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001AE7 RID: 6887 RVA: 0x000B1802 File Offset: 0x000AFA02
	public void BeginArrayAssignment(string array_type, string array_name)
	{
		this.Line(array_name + " = new " + array_type + "[]");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AE8 RID: 6888 RVA: 0x000B188F File Offset: 0x000AFA8F
	public void EndArrayAssignment()
	{
		this.Indent--;
		this.Line("};");
	}

	// Token: 0x06001AE9 RID: 6889 RVA: 0x000B18AA File Offset: 0x000AFAAA
	public void FieldAssignment(string field_name, string value)
	{
		this.Line(field_name + " = " + value + ";");
	}

	// Token: 0x06001AEA RID: 6890 RVA: 0x000B18C3 File Offset: 0x000AFAC3
	public void BeginStructureDelegateFieldInitializer(string name)
	{
		this.Line(name + "=delegate()");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AEB RID: 6891 RVA: 0x000B18EF File Offset: 0x000AFAEF
	public void EndStructureDelegateFieldInitializer()
	{
		this.Indent--;
		this.Line("},");
	}

	// Token: 0x06001AEC RID: 6892 RVA: 0x000B190A File Offset: 0x000AFB0A
	public void BeginIf(string condition)
	{
		this.Line("if(" + condition + ")");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AED RID: 6893 RVA: 0x001A9884 File Offset: 0x001A7A84
	public void BeginElseIf(string condition)
	{
		this.Indent--;
		this.Line("}");
		this.Line("else if(" + condition + ")");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AEE RID: 6894 RVA: 0x000B1765 File Offset: 0x000AF965
	public void EndIf()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001AEF RID: 6895 RVA: 0x001A98DC File Offset: 0x001A7ADC
	public void BeginFunctionDeclaration(string name, string parameter, string return_type)
	{
		this.Line(string.Concat(new string[]
		{
			"public ",
			return_type,
			" ",
			name,
			"(",
			parameter,
			")"
		}));
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AF0 RID: 6896 RVA: 0x001A9940 File Offset: 0x001A7B40
	public void BeginFunctionDeclaration(string name, string return_type)
	{
		this.Line(string.Concat(new string[]
		{
			"public ",
			return_type,
			" ",
			name,
			"()"
		}));
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AF1 RID: 6897 RVA: 0x000B1765 File Offset: 0x000AF965
	public void EndFunctionDeclaration()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001AF2 RID: 6898 RVA: 0x001A9998 File Offset: 0x001A7B98
	private void InternalNamedParameter(string name, string value, bool last_parameter)
	{
		string str = "";
		if (!last_parameter)
		{
			str = ",";
		}
		this.Line(name + ":" + value + str);
	}

	// Token: 0x06001AF3 RID: 6899 RVA: 0x000B193B File Offset: 0x000AFB3B
	public void NamedParameterBool(string name, bool value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value.ToString().ToLower(), last_parameter);
	}

	// Token: 0x06001AF4 RID: 6900 RVA: 0x000B1951 File Offset: 0x000AFB51
	public void NamedParameterInt(string name, int value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value.ToString(), last_parameter);
	}

	// Token: 0x06001AF5 RID: 6901 RVA: 0x000B1962 File Offset: 0x000AFB62
	public void NamedParameterFloat(string name, float value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value.ToString() + "f", last_parameter);
	}

	// Token: 0x06001AF6 RID: 6902 RVA: 0x000B197D File Offset: 0x000AFB7D
	public void NamedParameterString(string name, string value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value, last_parameter);
	}

	// Token: 0x06001AF7 RID: 6903 RVA: 0x000B1988 File Offset: 0x000AFB88
	public void BeginFunctionCall(string name)
	{
		this.Line(name);
		this.Line("(");
		this.Indent++;
	}

	// Token: 0x06001AF8 RID: 6904 RVA: 0x000B19AA File Offset: 0x000AFBAA
	public void EndFunctionCall()
	{
		this.Indent--;
		this.Line(");");
	}

	// Token: 0x06001AF9 RID: 6905 RVA: 0x001A99C8 File Offset: 0x001A7BC8
	public void FunctionCall(string function_name, params string[] parameters)
	{
		string str = function_name + "(";
		for (int i = 0; i < parameters.Length; i++)
		{
			str += parameters[i];
			if (i != parameters.Length - 1)
			{
				str += ", ";
			}
		}
		this.Line(str + ");");
	}

	// Token: 0x06001AFA RID: 6906 RVA: 0x000B19C5 File Offset: 0x000AFBC5
	public void StructureFieldInitializer(string field, string value)
	{
		this.Line(field + " = " + value + ",");
	}

	// Token: 0x06001AFB RID: 6907 RVA: 0x001A9A20 File Offset: 0x001A7C20
	public void StructureArrayFieldInitializer(string field, string field_type, params string[] values)
	{
		string text = field + " = new " + field_type + "[]{ ";
		for (int i = 0; i < values.Length; i++)
		{
			text += values[i];
			if (i < values.Length - 1)
			{
				text += ", ";
			}
		}
		text += " },";
		this.Line(text);
	}

	// Token: 0x06001AFC RID: 6908 RVA: 0x001A9A80 File Offset: 0x001A7C80
	public void Line(string text = "")
	{
		for (int i = 0; i < this.Indent; i++)
		{
			text = "\t" + text;
		}
		this.Lines.Add(text);
	}

	// Token: 0x06001AFD RID: 6909 RVA: 0x000B19DE File Offset: 0x000AFBDE
	public void Flush()
	{
		File.WriteAllLines(this.Path, this.Lines.ToArray());
	}

	// Token: 0x04001104 RID: 4356
	private List<string> Lines = new List<string>();

	// Token: 0x04001105 RID: 4357
	private string Path;

	// Token: 0x04001106 RID: 4358
	private int Indent;
}
