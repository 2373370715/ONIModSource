using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02001869 RID: 6249
public abstract class SimComponent : KMonoBehaviour, ISim200ms
{
	// Token: 0x1700083C RID: 2108
	// (get) Token: 0x0600811D RID: 33053 RVA: 0x000F4EA3 File Offset: 0x000F30A3
	public bool IsSimActive
	{
		get
		{
			return this.simActive;
		}
	}

	// Token: 0x0600811E RID: 33054 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
	}

	// Token: 0x0600811F RID: 33055 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnSimRegistered()
	{
	}

	// Token: 0x06008120 RID: 33056 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnSimActivate()
	{
	}

	// Token: 0x06008121 RID: 33057 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnSimDeactivate()
	{
	}

	// Token: 0x06008122 RID: 33058 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnSimUnregister()
	{
	}

	// Token: 0x06008123 RID: 33059
	protected abstract Action<int> GetStaticUnregister();

	// Token: 0x06008124 RID: 33060 RVA: 0x000B2F5A File Offset: 0x000B115A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06008125 RID: 33061 RVA: 0x000F4EAB File Offset: 0x000F30AB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SimRegister();
	}

	// Token: 0x06008126 RID: 33062 RVA: 0x000F4EB9 File Offset: 0x000F30B9
	protected override void OnCleanUp()
	{
		this.SimUnregister();
		base.OnCleanUp();
	}

	// Token: 0x06008127 RID: 33063 RVA: 0x000F4EC7 File Offset: 0x000F30C7
	public void SetSimActive(bool active)
	{
		this.simActive = active;
		this.dirty = true;
	}

	// Token: 0x06008128 RID: 33064 RVA: 0x000F4ED7 File Offset: 0x000F30D7
	public void Sim200ms(float dt)
	{
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		this.UpdateSimState();
	}

	// Token: 0x06008129 RID: 33065 RVA: 0x000F4EED File Offset: 0x000F30ED
	private void UpdateSimState()
	{
		if (!this.dirty)
		{
			return;
		}
		this.dirty = false;
		if (this.simActive)
		{
			this.OnSimActivate();
			return;
		}
		this.OnSimDeactivate();
	}

	// Token: 0x0600812A RID: 33066 RVA: 0x00337160 File Offset: 0x00335360
	private void SimRegister()
	{
		if (base.isSpawned && this.simHandle == -1)
		{
			this.simHandle = -2;
			Action<int> static_unregister = this.GetStaticUnregister();
			HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle = Game.Instance.simComponentCallbackManager.Add(delegate(int handle, object data)
			{
				SimComponent.OnSimRegistered(this, handle, static_unregister);
			}, this, "SimComponent.SimRegister");
			this.OnSimRegister(cb_handle);
		}
	}

	// Token: 0x0600812B RID: 33067 RVA: 0x000F4F14 File Offset: 0x000F3114
	private void SimUnregister()
	{
		if (Sim.IsValidHandle(this.simHandle))
		{
			this.OnSimUnregister();
		}
		this.simHandle = -1;
	}

	// Token: 0x0600812C RID: 33068 RVA: 0x000F4F30 File Offset: 0x000F3130
	private static void OnSimRegistered(SimComponent instance, int handle, Action<int> static_unregister)
	{
		if (instance != null)
		{
			instance.simHandle = handle;
			instance.OnSimRegistered();
			return;
		}
		static_unregister(handle);
	}

	// Token: 0x0600812D RID: 33069 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("ENABLE_LOGGER")]
	protected void Log(string msg)
	{
	}

	// Token: 0x040061E1 RID: 25057
	[SerializeField]
	protected int simHandle = -1;

	// Token: 0x040061E2 RID: 25058
	private bool simActive = true;

	// Token: 0x040061E3 RID: 25059
	private bool dirty = true;
}
