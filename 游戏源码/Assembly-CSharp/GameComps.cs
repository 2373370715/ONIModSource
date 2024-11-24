using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x020010D0 RID: 4304
public class GameComps : KComponents
{
	// Token: 0x06005862 RID: 22626 RVA: 0x0028BBA0 File Offset: 0x00289DA0
	public GameComps()
	{
		foreach (FieldInfo fieldInfo in typeof(GameComps).GetFields())
		{
			object obj = Activator.CreateInstance(fieldInfo.FieldType);
			fieldInfo.SetValue(null, obj);
			base.Add<IComponentManager>(obj as IComponentManager);
			if (obj is IKComponentManager)
			{
				IKComponentManager inst = obj as IKComponentManager;
				GameComps.AddKComponentManager(fieldInfo.FieldType, inst);
			}
		}
	}

	// Token: 0x06005863 RID: 22627 RVA: 0x0028BC14 File Offset: 0x00289E14
	public new void Clear()
	{
		FieldInfo[] fields = typeof(GameComps).GetFields();
		for (int i = 0; i < fields.Length; i++)
		{
			IComponentManager componentManager = fields[i].GetValue(null) as IComponentManager;
			if (componentManager != null)
			{
				componentManager.Clear();
			}
		}
	}

	// Token: 0x06005864 RID: 22628 RVA: 0x000D9A26 File Offset: 0x000D7C26
	public static void AddKComponentManager(Type kcomponent, IKComponentManager inst)
	{
		GameComps.kcomponentManagers[kcomponent] = inst;
	}

	// Token: 0x06005865 RID: 22629 RVA: 0x000D9A34 File Offset: 0x000D7C34
	public static IKComponentManager GetKComponentManager(Type kcomponent_type)
	{
		return GameComps.kcomponentManagers[kcomponent_type];
	}

	// Token: 0x04003E3A RID: 15930
	public static GravityComponents Gravities;

	// Token: 0x04003E3B RID: 15931
	public static FallerComponents Fallers;

	// Token: 0x04003E3C RID: 15932
	public static InfraredVisualizerComponents InfraredVisualizers;

	// Token: 0x04003E3D RID: 15933
	public static ElementSplitterComponents ElementSplitters;

	// Token: 0x04003E3E RID: 15934
	public static OreSizeVisualizerComponents OreSizeVisualizers;

	// Token: 0x04003E3F RID: 15935
	public static StructureTemperatureComponents StructureTemperatures;

	// Token: 0x04003E40 RID: 15936
	public static DiseaseContainers DiseaseContainers;

	// Token: 0x04003E41 RID: 15937
	public static RequiresFoundation RequiresFoundations;

	// Token: 0x04003E42 RID: 15938
	public static WhiteBoard WhiteBoards;

	// Token: 0x04003E43 RID: 15939
	private static Dictionary<Type, IKComponentManager> kcomponentManagers = new Dictionary<Type, IKComponentManager>();
}
