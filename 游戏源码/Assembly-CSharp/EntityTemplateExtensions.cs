using System;
using UnityEngine;

// Token: 0x020000B5 RID: 181
public static class EntityTemplateExtensions
{
	// Token: 0x06000304 RID: 772 RVA: 0x0014BA5C File Offset: 0x00149C5C
	public static DefType AddOrGetDef<DefType>(this GameObject go) where DefType : StateMachine.BaseDef
	{
		StateMachineController stateMachineController = go.AddOrGet<StateMachineController>();
		DefType defType = stateMachineController.GetDef<DefType>();
		if (defType == null)
		{
			defType = Activator.CreateInstance<DefType>();
			stateMachineController.AddDef(defType);
			defType.Configure(go);
		}
		return defType;
	}

	// Token: 0x06000305 RID: 773 RVA: 0x0014BAA0 File Offset: 0x00149CA0
	public static ComponentType AddOrGet<ComponentType>(this GameObject go) where ComponentType : Component
	{
		ComponentType componentType = go.GetComponent<ComponentType>();
		if (componentType == null)
		{
			componentType = go.AddComponent<ComponentType>();
		}
		return componentType;
	}
}
