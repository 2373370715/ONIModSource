using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x0200170D RID: 5901
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/PrimaryElement")]
public class PrimaryElement : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06007983 RID: 31107 RVA: 0x000EFF78 File Offset: 0x000EE178
	public void SetUseSimDiseaseInfo(bool use)
	{
		this.useSimDiseaseInfo = use;
	}

	// Token: 0x17000798 RID: 1944
	// (get) Token: 0x06007984 RID: 31108 RVA: 0x000EFF81 File Offset: 0x000EE181
	// (set) Token: 0x06007985 RID: 31109 RVA: 0x003148B4 File Offset: 0x00312AB4
	[Serialize]
	public float Units
	{
		get
		{
			return this._units;
		}
		set
		{
			if (float.IsInfinity(value) || float.IsNaN(value))
			{
				DebugUtil.DevLogError("Invalid units value for element, setting Units to 0");
				this._units = 0f;
			}
			else
			{
				this._units = value;
			}
			if (this.onDataChanged != null)
			{
				this.onDataChanged(this);
			}
		}
	}

	// Token: 0x17000799 RID: 1945
	// (get) Token: 0x06007986 RID: 31110 RVA: 0x000EFF89 File Offset: 0x000EE189
	// (set) Token: 0x06007987 RID: 31111 RVA: 0x000EFF97 File Offset: 0x000EE197
	public float Temperature
	{
		get
		{
			return this.getTemperatureCallback(this);
		}
		set
		{
			this.SetTemperature(value);
		}
	}

	// Token: 0x1700079A RID: 1946
	// (get) Token: 0x06007988 RID: 31112 RVA: 0x000EFFA0 File Offset: 0x000EE1A0
	// (set) Token: 0x06007989 RID: 31113 RVA: 0x000EFFA8 File Offset: 0x000EE1A8
	public float InternalTemperature
	{
		get
		{
			return this._Temperature;
		}
		set
		{
			this._Temperature = value;
		}
	}

	// Token: 0x0600798A RID: 31114 RVA: 0x00314904 File Offset: 0x00312B04
	[OnSerializing]
	private void OnSerializing()
	{
		this._Temperature = this.Temperature;
		this.SanitizeMassAndTemperature();
		this.diseaseID.HashValue = 0;
		this.diseaseCount = 0;
		if (this.useSimDiseaseInfo)
		{
			int i = Grid.PosToCell(base.transform.GetPosition());
			if (Grid.DiseaseIdx[i] != 255)
			{
				this.diseaseID = Db.Get().Diseases[(int)Grid.DiseaseIdx[i]].id;
				this.diseaseCount = Grid.DiseaseCount[i];
				return;
			}
		}
		else if (this.diseaseHandle.IsValid())
		{
			DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle);
			if (header.diseaseIdx != 255)
			{
				this.diseaseID = Db.Get().Diseases[(int)header.diseaseIdx].id;
				this.diseaseCount = header.diseaseCount;
			}
		}
	}

	// Token: 0x0600798B RID: 31115 RVA: 0x003149F4 File Offset: 0x00312BF4
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.ElementID == (SimHashes)351109216)
		{
			this.ElementID = SimHashes.Creature;
		}
		this.SanitizeMassAndTemperature();
		float temperature = this._Temperature;
		if (float.IsNaN(temperature) || float.IsInfinity(temperature) || temperature < 0f || 10000f < temperature)
		{
			DeserializeWarnings.Instance.PrimaryElementTemperatureIsNan.Warn(string.Format("{0} has invalid temperature of {1}. Resetting temperature.", base.name, this.Temperature), null);
			temperature = this.Element.defaultValues.temperature;
		}
		this._Temperature = temperature;
		this.Temperature = temperature;
		if (this.Element == null)
		{
			DeserializeWarnings.Instance.PrimaryElementHasNoElement.Warn(base.name + "Primary element has no element.", null);
		}
		if (this.Mass < 0f)
		{
			DebugUtil.DevLogError(base.gameObject, "deserialized ore with less than 0 mass. Error! Destroying");
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		if (this.Mass == 0f && !this.KeepZeroMassObject)
		{
			DebugUtil.DevLogError(base.gameObject, "deserialized element with 0 mass. Destroying");
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		if (this.onDataChanged != null)
		{
			this.onDataChanged(this);
		}
		byte index = Db.Get().Diseases.GetIndex(this.diseaseID);
		if (index == 255 || this.diseaseCount <= 0)
		{
			if (this.diseaseHandle.IsValid())
			{
				GameComps.DiseaseContainers.Remove(base.gameObject);
				this.diseaseHandle.Clear();
				return;
			}
		}
		else
		{
			if (this.diseaseHandle.IsValid())
			{
				DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle);
				header.diseaseIdx = index;
				header.diseaseCount = this.diseaseCount;
				GameComps.DiseaseContainers.SetHeader(this.diseaseHandle, header);
				return;
			}
			this.diseaseHandle = GameComps.DiseaseContainers.Add(base.gameObject, index, this.diseaseCount);
		}
	}

	// Token: 0x0600798C RID: 31116 RVA: 0x000EFFB1 File Offset: 0x000EE1B1
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
	}

	// Token: 0x0600798D RID: 31117 RVA: 0x00314BD8 File Offset: 0x00312DD8
	private void SanitizeMassAndTemperature()
	{
		if (this._Temperature <= 0f)
		{
			DebugUtil.DevLogError(base.gameObject.name + " is attempting to serialize a temperature of <= 0K. Resetting to default. world=" + base.gameObject.DebugGetMyWorldName());
			this._Temperature = this.Element.defaultValues.temperature;
		}
		if (this.Mass > PrimaryElement.MAX_MASS)
		{
			DebugUtil.DevLogError(string.Format("{0} is attempting to serialize very large mass {1}. Resetting to default. world={2}", base.gameObject.name, this.Mass, base.gameObject.DebugGetMyWorldName()));
			this.Mass = this.Element.defaultValues.mass;
		}
	}

	// Token: 0x1700079B RID: 1947
	// (get) Token: 0x0600798E RID: 31118 RVA: 0x000EFFB9 File Offset: 0x000EE1B9
	// (set) Token: 0x0600798F RID: 31119 RVA: 0x000EFFC8 File Offset: 0x000EE1C8
	public float Mass
	{
		get
		{
			return this.Units * this.MassPerUnit;
		}
		set
		{
			this.SetMass(value);
			if (this.onDataChanged != null)
			{
				this.onDataChanged(this);
			}
		}
	}

	// Token: 0x06007990 RID: 31120 RVA: 0x00314C80 File Offset: 0x00312E80
	private void SetMass(float mass)
	{
		if ((mass > PrimaryElement.MAX_MASS || mass < 0f) && this.ElementID != SimHashes.Regolith)
		{
			DebugUtil.DevLogErrorFormat(base.gameObject, "{0} is getting an abnormal mass set {1}.", new object[]
			{
				base.gameObject.name,
				mass
			});
		}
		mass = Mathf.Clamp(mass, 0f, PrimaryElement.MAX_MASS);
		this.Units = mass / this.MassPerUnit;
		if (this.Units <= 0f && !this.KeepZeroMassObject)
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x06007991 RID: 31121 RVA: 0x00314D18 File Offset: 0x00312F18
	private void SetTemperature(float temperature)
	{
		if (float.IsNaN(temperature) || float.IsInfinity(temperature))
		{
			DebugUtil.LogErrorArgs(base.gameObject, new object[]
			{
				"Invalid temperature [" + temperature.ToString() + "]"
			});
			return;
		}
		if (temperature <= 0f)
		{
			KCrashReporter.Assert(false, "Tried to set PrimaryElement.Temperature to a value <= 0", null);
		}
		this.setTemperatureCallback(this, temperature);
	}

	// Token: 0x06007992 RID: 31122 RVA: 0x000EFFE5 File Offset: 0x000EE1E5
	public void SetMassTemperature(float mass, float temperature)
	{
		this.SetMass(mass);
		this.SetTemperature(temperature);
	}

	// Token: 0x1700079C RID: 1948
	// (get) Token: 0x06007993 RID: 31123 RVA: 0x000EFFF5 File Offset: 0x000EE1F5
	public Element Element
	{
		get
		{
			if (this._Element == null)
			{
				this._Element = ElementLoader.FindElementByHash(this.ElementID);
			}
			return this._Element;
		}
	}

	// Token: 0x1700079D RID: 1949
	// (get) Token: 0x06007994 RID: 31124 RVA: 0x00314D84 File Offset: 0x00312F84
	public byte DiseaseIdx
	{
		get
		{
			if (this.diseaseRedirectTarget)
			{
				return this.diseaseRedirectTarget.DiseaseIdx;
			}
			byte result = byte.MaxValue;
			if (this.useSimDiseaseInfo)
			{
				int i = Grid.PosToCell(base.transform.GetPosition());
				result = Grid.DiseaseIdx[i];
			}
			else if (this.diseaseHandle.IsValid())
			{
				result = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle).diseaseIdx;
			}
			return result;
		}
	}

	// Token: 0x1700079E RID: 1950
	// (get) Token: 0x06007995 RID: 31125 RVA: 0x00314DFC File Offset: 0x00312FFC
	public int DiseaseCount
	{
		get
		{
			if (this.diseaseRedirectTarget)
			{
				return this.diseaseRedirectTarget.DiseaseCount;
			}
			int result = 0;
			if (this.useSimDiseaseInfo)
			{
				int i = Grid.PosToCell(base.transform.GetPosition());
				result = Grid.DiseaseCount[i];
			}
			else if (this.diseaseHandle.IsValid())
			{
				result = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle).diseaseCount;
			}
			return result;
		}
	}

	// Token: 0x06007996 RID: 31126 RVA: 0x000F0016 File Offset: 0x000EE216
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GameComps.InfraredVisualizers.Add(base.gameObject);
		base.Subscribe<PrimaryElement>(1335436905, PrimaryElement.OnSplitFromChunkDelegate);
		base.Subscribe<PrimaryElement>(-2064133523, PrimaryElement.OnAbsorbDelegate);
	}

	// Token: 0x06007997 RID: 31127 RVA: 0x00314E70 File Offset: 0x00313070
	protected override void OnSpawn()
	{
		Attributes attributes = this.GetAttributes();
		if (attributes != null)
		{
			foreach (AttributeModifier modifier in this.Element.attributeModifiers)
			{
				attributes.Add(modifier);
			}
		}
	}

	// Token: 0x06007998 RID: 31128 RVA: 0x00314ED4 File Offset: 0x003130D4
	public void ForcePermanentDiseaseContainer(bool force_on)
	{
		if (force_on)
		{
			if (!this.diseaseHandle.IsValid())
			{
				this.diseaseHandle = GameComps.DiseaseContainers.Add(base.gameObject, byte.MaxValue, 0);
			}
		}
		else if (this.diseaseHandle.IsValid() && this.DiseaseIdx == 255)
		{
			GameComps.DiseaseContainers.Remove(base.gameObject);
			this.diseaseHandle.Clear();
		}
		this.forcePermanentDiseaseContainer = force_on;
	}

	// Token: 0x06007999 RID: 31129 RVA: 0x000F0051 File Offset: 0x000EE251
	protected override void OnCleanUp()
	{
		GameComps.InfraredVisualizers.Remove(base.gameObject);
		if (this.diseaseHandle.IsValid())
		{
			GameComps.DiseaseContainers.Remove(base.gameObject);
			this.diseaseHandle.Clear();
		}
		base.OnCleanUp();
	}

	// Token: 0x0600799A RID: 31130 RVA: 0x000F0091 File Offset: 0x000EE291
	public void SetElement(SimHashes element_id, bool addTags = true)
	{
		this.ElementID = element_id;
		if (addTags)
		{
			this.UpdateTags();
		}
	}

	// Token: 0x0600799B RID: 31131 RVA: 0x00314F4C File Offset: 0x0031314C
	public void UpdateTags()
	{
		if (this.ElementID == (SimHashes)0)
		{
			global::Debug.Log("UpdateTags() Primary element 0", base.gameObject);
			return;
		}
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (component != null)
		{
			List<Tag> list = new List<Tag>();
			foreach (Tag item in this.Element.oreTags)
			{
				list.Add(item);
			}
			if (component.HasAnyTags(PrimaryElement.metalTags))
			{
				list.Add(GameTags.StoredMetal);
			}
			foreach (Tag tag in list)
			{
				component.AddTag(tag, false);
			}
		}
	}

	// Token: 0x0600799C RID: 31132 RVA: 0x00315010 File Offset: 0x00313210
	public void ModifyDiseaseCount(int delta, string reason)
	{
		if (this.diseaseRedirectTarget)
		{
			this.diseaseRedirectTarget.ModifyDiseaseCount(delta, reason);
			return;
		}
		if (this.useSimDiseaseInfo)
		{
			SimMessages.ModifyDiseaseOnCell(Grid.PosToCell(this), byte.MaxValue, delta);
			return;
		}
		if (delta != 0 && this.diseaseHandle.IsValid() && GameComps.DiseaseContainers.ModifyDiseaseCount(this.diseaseHandle, delta) <= 0 && !this.forcePermanentDiseaseContainer)
		{
			base.Trigger(-1689370368, false);
			GameComps.DiseaseContainers.Remove(base.gameObject);
			this.diseaseHandle.Clear();
		}
	}

	// Token: 0x0600799D RID: 31133 RVA: 0x003150AC File Offset: 0x003132AC
	public void AddDisease(byte disease_idx, int delta, string reason)
	{
		if (delta == 0)
		{
			return;
		}
		if (this.diseaseRedirectTarget)
		{
			this.diseaseRedirectTarget.AddDisease(disease_idx, delta, reason);
			return;
		}
		if (this.useSimDiseaseInfo)
		{
			SimMessages.ModifyDiseaseOnCell(Grid.PosToCell(this), disease_idx, delta);
			return;
		}
		if (this.diseaseHandle.IsValid())
		{
			if (GameComps.DiseaseContainers.AddDisease(this.diseaseHandle, disease_idx, delta) <= 0)
			{
				GameComps.DiseaseContainers.Remove(base.gameObject);
				this.diseaseHandle.Clear();
				return;
			}
		}
		else if (delta > 0)
		{
			this.diseaseHandle = GameComps.DiseaseContainers.Add(base.gameObject, disease_idx, delta);
			base.Trigger(-1689370368, true);
			base.Trigger(-283306403, null);
		}
	}

	// Token: 0x0600799E RID: 31134 RVA: 0x000EFFA0 File Offset: 0x000EE1A0
	private static float OnGetTemperature(PrimaryElement primary_element)
	{
		return primary_element._Temperature;
	}

	// Token: 0x0600799F RID: 31135 RVA: 0x00315168 File Offset: 0x00313368
	private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		global::Debug.Assert(!float.IsNaN(temperature));
		if (temperature <= 0f)
		{
			DebugUtil.LogErrorArgs(primary_element.gameObject, new object[]
			{
				primary_element.gameObject.name + " has a temperature of zero which has always been an error in my experience."
			});
		}
		primary_element._Temperature = temperature;
	}

	// Token: 0x060079A0 RID: 31136 RVA: 0x003151BC File Offset: 0x003133BC
	private void OnSplitFromChunk(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable == null)
		{
			return;
		}
		float percent = this.Units / (this.Units + pickupable.PrimaryElement.Units);
		SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(pickupable.PrimaryElement, percent);
		this.AddDisease(percentOfDisease.idx, percentOfDisease.count, "PrimaryElement.SplitFromChunk");
		pickupable.PrimaryElement.ModifyDiseaseCount(-percentOfDisease.count, "PrimaryElement.SplitFromChunk");
	}

	// Token: 0x060079A1 RID: 31137 RVA: 0x00315230 File Offset: 0x00313430
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable == null)
		{
			return;
		}
		this.AddDisease(pickupable.PrimaryElement.DiseaseIdx, pickupable.PrimaryElement.DiseaseCount, "PrimaryElement.OnAbsorb");
	}

	// Token: 0x060079A2 RID: 31138 RVA: 0x00315270 File Offset: 0x00313470
	private void SetDiseaseVisualProvider(GameObject visualizer)
	{
		HandleVector<int>.Handle handle = GameComps.DiseaseContainers.GetHandle(base.gameObject);
		if (handle != HandleVector<int>.InvalidHandle)
		{
			DiseaseContainer payload = GameComps.DiseaseContainers.GetPayload(handle);
			payload.visualDiseaseProvider = visualizer;
			GameComps.DiseaseContainers.SetPayload(handle, ref payload);
		}
	}

	// Token: 0x060079A3 RID: 31139 RVA: 0x000F00A3 File Offset: 0x000EE2A3
	public void RedirectDisease(GameObject target)
	{
		this.SetDiseaseVisualProvider(target);
		this.diseaseRedirectTarget = (target ? target.GetComponent<PrimaryElement>() : null);
		global::Debug.Assert(this.diseaseRedirectTarget != this, "Disease redirect target set to myself");
	}

	// Token: 0x04005B16 RID: 23318
	public static float MAX_MASS = 100000f;

	// Token: 0x04005B17 RID: 23319
	public SimTemperatureTransfer sttOptimizationHook;

	// Token: 0x04005B18 RID: 23320
	public PrimaryElement.GetTemperatureCallback getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(PrimaryElement.OnGetTemperature);

	// Token: 0x04005B19 RID: 23321
	public PrimaryElement.SetTemperatureCallback setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(PrimaryElement.OnSetTemperature);

	// Token: 0x04005B1A RID: 23322
	private PrimaryElement diseaseRedirectTarget;

	// Token: 0x04005B1B RID: 23323
	private bool useSimDiseaseInfo;

	// Token: 0x04005B1C RID: 23324
	public const float DefaultChunkMass = 400f;

	// Token: 0x04005B1D RID: 23325
	private static readonly Tag[] metalTags = new Tag[]
	{
		GameTags.Metal,
		GameTags.RefinedMetal
	};

	// Token: 0x04005B1E RID: 23326
	[Serialize]
	[HashedEnum]
	public SimHashes ElementID;

	// Token: 0x04005B1F RID: 23327
	private float _units = 1f;

	// Token: 0x04005B20 RID: 23328
	[Serialize]
	[SerializeField]
	private float _Temperature;

	// Token: 0x04005B21 RID: 23329
	[Serialize]
	[NonSerialized]
	public bool KeepZeroMassObject;

	// Token: 0x04005B22 RID: 23330
	[Serialize]
	private HashedString diseaseID;

	// Token: 0x04005B23 RID: 23331
	[Serialize]
	private int diseaseCount;

	// Token: 0x04005B24 RID: 23332
	private HandleVector<int>.Handle diseaseHandle = HandleVector<int>.InvalidHandle;

	// Token: 0x04005B25 RID: 23333
	public float MassPerUnit = 1f;

	// Token: 0x04005B26 RID: 23334
	[NonSerialized]
	private Element _Element;

	// Token: 0x04005B27 RID: 23335
	[NonSerialized]
	public Action<PrimaryElement> onDataChanged;

	// Token: 0x04005B28 RID: 23336
	[NonSerialized]
	private bool forcePermanentDiseaseContainer;

	// Token: 0x04005B29 RID: 23337
	private static readonly EventSystem.IntraObjectHandler<PrimaryElement> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<PrimaryElement>(delegate(PrimaryElement component, object data)
	{
		component.OnSplitFromChunk(data);
	});

	// Token: 0x04005B2A RID: 23338
	private static readonly EventSystem.IntraObjectHandler<PrimaryElement> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<PrimaryElement>(delegate(PrimaryElement component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x0200170E RID: 5902
	// (Invoke) Token: 0x060079A7 RID: 31143
	public delegate float GetTemperatureCallback(PrimaryElement primary_element);

	// Token: 0x0200170F RID: 5903
	// (Invoke) Token: 0x060079AB RID: 31147
	public delegate void SetTemperatureCallback(PrimaryElement primary_element, float temperature);
}
