using System;
using System.Collections.Generic;

public static class DevToolCommandPaletteUtil
{
	public static List<DevToolCommandPalette.Command> GenerateDefaultCommandPalette()
	{
		List<DevToolCommandPalette.Command> list = new List<DevToolCommandPalette.Command>();
		foreach (Type devToolType in ReflectionUtil.CollectTypesThatInheritOrImplement<DevTool>())
		{
			if (!devToolType.IsAbstract && ReflectionUtil.HasDefaultConstructor(devToolType))
			{
				list.Add(new DevToolCommandPalette.Command("Open DevTool: \"" + DevToolUtil.GenerateDevToolName(devToolType) + "\"", delegate
				{
					DevToolUtil.Open((DevTool)Activator.CreateInstance(devToolType));
				}));
			}
		}
		return list;
	}
}
