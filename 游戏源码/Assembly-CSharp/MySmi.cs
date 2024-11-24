using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x020008BD RID: 2237
public class MySmi : MyAttributeManager<StateMachine.Instance>
{
	// Token: 0x060027A6 RID: 10150 RVA: 0x001D1B88 File Offset: 0x001CFD88
	public static void Init()
	{
		MyAttributes.Register(new MySmi(new Dictionary<Type, MethodInfo>
		{
			{
				typeof(MySmiGet),
				typeof(MySmi).GetMethod("FindSmi")
			},
			{
				typeof(MySmiReq),
				typeof(MySmi).GetMethod("RequireSmi")
			}
		}));
	}

	// Token: 0x060027A7 RID: 10151 RVA: 0x000B9C4F File Offset: 0x000B7E4F
	public MySmi(Dictionary<Type, MethodInfo> attributeMap) : base(attributeMap, null)
	{
	}

	// Token: 0x060027A8 RID: 10152 RVA: 0x001D1BEC File Offset: 0x001CFDEC
	public static StateMachine.Instance FindSmi<T>(KMonoBehaviour c, bool isStart) where T : StateMachine.Instance
	{
		StateMachineController component = c.GetComponent<StateMachineController>();
		if (component != null)
		{
			return component.GetSMI<T>();
		}
		return null;
	}

	// Token: 0x060027A9 RID: 10153 RVA: 0x001D1C18 File Offset: 0x001CFE18
	public static StateMachine.Instance RequireSmi<T>(KMonoBehaviour c, bool isStart) where T : StateMachine.Instance
	{
		if (isStart)
		{
			StateMachine.Instance instance = MySmi.FindSmi<T>(c, isStart);
			Debug.Assert(instance != null, string.Format("{0} '{1}' requires a StateMachineInstance of type {2}!", c.GetType().ToString(), c.name, typeof(T)));
			return instance;
		}
		return MySmi.FindSmi<T>(c, isStart);
	}
}
