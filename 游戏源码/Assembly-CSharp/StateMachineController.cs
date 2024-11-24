using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;
using UnityEngine;

// Token: 0x020008F6 RID: 2294
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/StateMachineController")]
public class StateMachineController : KMonoBehaviour, ISaveLoadableDetails, IStateMachineControllerHack
{
	// Token: 0x1700012A RID: 298
	// (get) Token: 0x060028AE RID: 10414 RVA: 0x000BA6BE File Offset: 0x000B88BE
	public StateMachineController.CmpDef cmpdef
	{
		get
		{
			return this.defHandle.Get<StateMachineController.CmpDef>();
		}
	}

	// Token: 0x060028AF RID: 10415 RVA: 0x000BA6CB File Offset: 0x000B88CB
	public IEnumerator<StateMachine.Instance> GetEnumerator()
	{
		return this.stateMachines.GetEnumerator();
	}

	// Token: 0x060028B0 RID: 10416 RVA: 0x000BA6DD File Offset: 0x000B88DD
	public void AddStateMachineInstance(StateMachine.Instance state_machine)
	{
		if (!this.stateMachines.Contains(state_machine))
		{
			this.stateMachines.Add(state_machine);
			MyAttributes.OnAwake(state_machine, this);
		}
	}

	// Token: 0x060028B1 RID: 10417 RVA: 0x000BA700 File Offset: 0x000B8900
	public void RemoveStateMachineInstance(StateMachine.Instance state_machine)
	{
		if (!state_machine.GetStateMachine().saveHistory && !state_machine.GetStateMachine().debugSettings.saveHistory)
		{
			this.stateMachines.Remove(state_machine);
		}
	}

	// Token: 0x060028B2 RID: 10418 RVA: 0x000BA72E File Offset: 0x000B892E
	public bool HasStateMachineInstance(StateMachine.Instance state_machine)
	{
		return this.stateMachines.Contains(state_machine);
	}

	// Token: 0x060028B3 RID: 10419 RVA: 0x000BA73C File Offset: 0x000B893C
	public void AddDef(StateMachine.BaseDef def)
	{
		this.cmpdef.defs.Add(def);
	}

	// Token: 0x060028B4 RID: 10420 RVA: 0x000BA74F File Offset: 0x000B894F
	public LoggerFSSSS GetLog()
	{
		return this.log;
	}

	// Token: 0x060028B5 RID: 10421 RVA: 0x000BA757 File Offset: 0x000B8957
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.log.SetName(base.name);
		base.Subscribe<StateMachineController>(1969584890, StateMachineController.OnTargetDestroyedDelegate);
		base.Subscribe<StateMachineController>(1502190696, StateMachineController.OnTargetDestroyedDelegate);
	}

	// Token: 0x060028B6 RID: 10422 RVA: 0x001D3610 File Offset: 0x001D1810
	private void OnTargetDestroyed(object data)
	{
		while (this.stateMachines.Count > 0)
		{
			StateMachine.Instance instance = this.stateMachines[0];
			instance.StopSM("StateMachineController.OnCleanUp");
			this.stateMachines.Remove(instance);
		}
	}

	// Token: 0x060028B7 RID: 10423 RVA: 0x001D3654 File Offset: 0x001D1854
	protected override void OnLoadLevel()
	{
		while (this.stateMachines.Count > 0)
		{
			StateMachine.Instance instance = this.stateMachines[0];
			instance.FreeResources();
			this.stateMachines.Remove(instance);
		}
	}

	// Token: 0x060028B8 RID: 10424 RVA: 0x001D3694 File Offset: 0x001D1894
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		while (this.stateMachines.Count > 0)
		{
			StateMachine.Instance instance = this.stateMachines[0];
			instance.StopSM("StateMachineController.OnCleanUp");
			this.stateMachines.Remove(instance);
		}
	}

	// Token: 0x060028B9 RID: 10425 RVA: 0x001D36DC File Offset: 0x001D18DC
	public void CreateSMIS()
	{
		if (!this.defHandle.IsValid())
		{
			return;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			baseDef.CreateSMI(this);
		}
	}

	// Token: 0x060028BA RID: 10426 RVA: 0x001D3744 File Offset: 0x001D1944
	public void StartSMIS()
	{
		if (!this.defHandle.IsValid())
		{
			return;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			if (!baseDef.preventStartSMIOnSpawn)
			{
				StateMachine.Instance smi = this.GetSMI(Singleton<StateMachineManager>.Instance.CreateStateMachine(baseDef.GetStateMachineType()).GetStateMachineInstanceType());
				if (smi != null && !smi.IsRunning())
				{
					smi.StartSM();
				}
			}
		}
	}

	// Token: 0x060028BB RID: 10427 RVA: 0x000BA792 File Offset: 0x000B8992
	public void Serialize(BinaryWriter writer)
	{
		this.serializer.Serialize(this.stateMachines, writer);
	}

	// Token: 0x060028BC RID: 10428 RVA: 0x000BA7A6 File Offset: 0x000B89A6
	public void Deserialize(IReader reader)
	{
		this.serializer.Deserialize(reader);
	}

	// Token: 0x060028BD RID: 10429 RVA: 0x000BA7B4 File Offset: 0x000B89B4
	public bool Restore(StateMachine.Instance smi)
	{
		return this.serializer.Restore(smi);
	}

	// Token: 0x060028BE RID: 10430 RVA: 0x001D37D8 File Offset: 0x001D19D8
	public DefType GetDef<DefType>() where DefType : StateMachine.BaseDef
	{
		if (!this.defHandle.IsValid())
		{
			return default(DefType);
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			DefType defType = baseDef as DefType;
			if (defType != null)
			{
				return defType;
			}
		}
		return default(DefType);
	}

	// Token: 0x060028BF RID: 10431 RVA: 0x001D3864 File Offset: 0x001D1A64
	public List<DefType> GetDefs<DefType>() where DefType : StateMachine.BaseDef
	{
		List<DefType> list = new List<DefType>();
		if (!this.defHandle.IsValid())
		{
			return list;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			DefType defType = baseDef as DefType;
			if (defType != null)
			{
				list.Add(defType);
			}
		}
		return list;
	}

	// Token: 0x060028C0 RID: 10432 RVA: 0x001D38E4 File Offset: 0x001D1AE4
	public StateMachine.Instance GetSMI(Type type)
	{
		for (int i = 0; i < this.stateMachines.Count; i++)
		{
			StateMachine.Instance instance = this.stateMachines[i];
			if (type.IsAssignableFrom(instance.GetType()))
			{
				return instance;
			}
		}
		return null;
	}

	// Token: 0x060028C1 RID: 10433 RVA: 0x000BA7C2 File Offset: 0x000B89C2
	public StateMachineInstanceType GetSMI<StateMachineInstanceType>() where StateMachineInstanceType : class
	{
		return this.GetSMI(typeof(StateMachineInstanceType)) as StateMachineInstanceType;
	}

	// Token: 0x060028C2 RID: 10434 RVA: 0x001D3928 File Offset: 0x001D1B28
	public List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>() where StateMachineInstanceType : class
	{
		List<StateMachineInstanceType> list = new List<StateMachineInstanceType>();
		foreach (StateMachine.Instance instance in this.stateMachines)
		{
			StateMachineInstanceType stateMachineInstanceType = instance as StateMachineInstanceType;
			if (stateMachineInstanceType != null)
			{
				list.Add(stateMachineInstanceType);
			}
		}
		return list;
	}

	// Token: 0x060028C3 RID: 10435 RVA: 0x001D3994 File Offset: 0x001D1B94
	public List<IGameObjectEffectDescriptor> GetDescriptors()
	{
		List<IGameObjectEffectDescriptor> list = new List<IGameObjectEffectDescriptor>();
		if (!this.defHandle.IsValid())
		{
			return list;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			if (baseDef is IGameObjectEffectDescriptor)
			{
				list.Add(baseDef as IGameObjectEffectDescriptor);
			}
		}
		return list;
	}

	// Token: 0x04001B2E RID: 6958
	public DefHandle defHandle;

	// Token: 0x04001B2F RID: 6959
	private List<StateMachine.Instance> stateMachines = new List<StateMachine.Instance>();

	// Token: 0x04001B30 RID: 6960
	private LoggerFSSSS log = new LoggerFSSSS("StateMachineController", 35);

	// Token: 0x04001B31 RID: 6961
	private StateMachineSerializer serializer = new StateMachineSerializer();

	// Token: 0x04001B32 RID: 6962
	private static readonly EventSystem.IntraObjectHandler<StateMachineController> OnTargetDestroyedDelegate = new EventSystem.IntraObjectHandler<StateMachineController>(delegate(StateMachineController component, object data)
	{
		component.OnTargetDestroyed(data);
	});

	// Token: 0x020008F7 RID: 2295
	public class CmpDef
	{
		// Token: 0x04001B33 RID: 6963
		public List<StateMachine.BaseDef> defs = new List<StateMachine.BaseDef>();
	}
}
