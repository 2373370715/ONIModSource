﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class CodexText : CodexWidget<CodexText>
{
			public string text { get; set; }

			public string messageID { get; set; }

			public CodexTextStyle style { get; set; }

			public string stringKey
	{
		get
		{
			return "--> " + (this.text ?? "NULL");
		}
		set
		{
			this.text = Strings.Get(value);
		}
	}

	public CodexText()
	{
		this.style = CodexTextStyle.Body;
	}

	public CodexText(string text, CodexTextStyle style = CodexTextStyle.Body, string id = null)
	{
		this.text = text;
		this.style = style;
		if (id != null)
		{
			this.messageID = id;
		}
	}

	public void ConfigureLabel(LocText label, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		label.gameObject.SetActive(true);
		label.AllowLinks = (this.style == CodexTextStyle.Body);
		label.textStyleSetting = textStyles[this.style];
		label.text = this.text;
		label.ApplySettings();
	}

	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureLabel(contentGameObject.GetComponent<LocText>(), textStyles);
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
