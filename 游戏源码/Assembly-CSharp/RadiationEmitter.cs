using System;
using UnityEngine;

// Token: 0x0200172D RID: 5933
public class RadiationEmitter : SimComponent
{
	// Token: 0x06007A2A RID: 31274 RVA: 0x000F03D8 File Offset: 0x000EE5D8
	protected override void OnSpawn()
	{
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "RadiationEmitter.OnSpawn");
		base.OnSpawn();
	}

	// Token: 0x06007A2B RID: 31275 RVA: 0x000F0402 File Offset: 0x000EE602
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		base.OnCleanUp();
	}

	// Token: 0x06007A2C RID: 31276 RVA: 0x000DF8EB File Offset: 0x000DDAEB
	public void SetEmitting(bool emitting)
	{
		base.SetSimActive(emitting);
	}

	// Token: 0x06007A2D RID: 31277 RVA: 0x000F0426 File Offset: 0x000EE626
	public int GetEmissionCell()
	{
		return Grid.PosToCell(base.transform.GetPosition() + this.emissionOffset);
	}

	// Token: 0x06007A2E RID: 31278 RVA: 0x00317C40 File Offset: 0x00315E40
	public void Refresh()
	{
		int emissionCell = this.GetEmissionCell();
		if (this.radiusProportionalToRads)
		{
			this.SetRadiusProportionalToRads();
		}
		SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, this.emitRadiusX, this.emitRadiusY, this.emitRads, this.emitRate, this.emitSpeed, this.emitDirection, this.emitAngle, this.emitType);
	}

	// Token: 0x06007A2F RID: 31279 RVA: 0x000F0443 File Offset: 0x000EE643
	private void OnCellChange()
	{
		this.Refresh();
	}

	// Token: 0x06007A30 RID: 31280 RVA: 0x00317CA0 File Offset: 0x00315EA0
	private void SetRadiusProportionalToRads()
	{
		this.emitRadiusX = (short)Mathf.Clamp(Mathf.RoundToInt(this.emitRads * 1f), 1, 128);
		this.emitRadiusY = (short)Mathf.Clamp(Mathf.RoundToInt(this.emitRads * 1f), 1, 128);
	}

	// Token: 0x06007A31 RID: 31281 RVA: 0x00317C40 File Offset: 0x00315E40
	protected override void OnSimActivate()
	{
		int emissionCell = this.GetEmissionCell();
		if (this.radiusProportionalToRads)
		{
			this.SetRadiusProportionalToRads();
		}
		SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, this.emitRadiusX, this.emitRadiusY, this.emitRads, this.emitRate, this.emitSpeed, this.emitDirection, this.emitAngle, this.emitType);
	}

	// Token: 0x06007A32 RID: 31282 RVA: 0x00317CF4 File Offset: 0x00315EF4
	protected override void OnSimDeactivate()
	{
		int emissionCell = this.GetEmissionCell();
		SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, 0, 0, 0f, 0f, 0f, 0f, 0f, this.emitType);
	}

	// Token: 0x06007A33 RID: 31283 RVA: 0x00317D38 File Offset: 0x00315F38
	protected override void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
		Game.Instance.simComponentCallbackManager.GetItem(cb_handle);
		int emissionCell = this.GetEmissionCell();
		SimMessages.AddRadiationEmitter(cb_handle.index, emissionCell, 0, 0, 0f, 0f, 0f, 0f, 0f, this.emitType);
	}

	// Token: 0x06007A34 RID: 31284 RVA: 0x000F044B File Offset: 0x000EE64B
	protected override void OnSimUnregister()
	{
		RadiationEmitter.StaticUnregister(this.simHandle);
	}

	// Token: 0x06007A35 RID: 31285 RVA: 0x000F0458 File Offset: 0x000EE658
	private static void StaticUnregister(int sim_handle)
	{
		global::Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveRadiationEmitter(-1, sim_handle);
	}

	// Token: 0x06007A36 RID: 31286 RVA: 0x00317D8C File Offset: 0x00315F8C
	private void OnDrawGizmosSelected()
	{
		int emissionCell = this.GetEmissionCell();
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(Grid.CellToPos(emissionCell) + Vector3.right / 2f + Vector3.up / 2f, 0.2f);
	}

	// Token: 0x06007A37 RID: 31287 RVA: 0x000F046C File Offset: 0x000EE66C
	protected override Action<int> GetStaticUnregister()
	{
		return new Action<int>(RadiationEmitter.StaticUnregister);
	}

	// Token: 0x04005BA9 RID: 23465
	public bool radiusProportionalToRads;

	// Token: 0x04005BAA RID: 23466
	[SerializeField]
	public short emitRadiusX = 4;

	// Token: 0x04005BAB RID: 23467
	[SerializeField]
	public short emitRadiusY = 4;

	// Token: 0x04005BAC RID: 23468
	[SerializeField]
	public float emitRads = 10f;

	// Token: 0x04005BAD RID: 23469
	[SerializeField]
	public float emitRate = 1f;

	// Token: 0x04005BAE RID: 23470
	[SerializeField]
	public float emitSpeed = 1f;

	// Token: 0x04005BAF RID: 23471
	[SerializeField]
	public float emitDirection;

	// Token: 0x04005BB0 RID: 23472
	[SerializeField]
	public float emitAngle = 360f;

	// Token: 0x04005BB1 RID: 23473
	[SerializeField]
	public RadiationEmitter.RadiationEmitterType emitType;

	// Token: 0x04005BB2 RID: 23474
	[SerializeField]
	public Vector3 emissionOffset = Vector3.zero;

	// Token: 0x0200172E RID: 5934
	public enum RadiationEmitterType
	{
		// Token: 0x04005BB4 RID: 23476
		Constant,
		// Token: 0x04005BB5 RID: 23477
		Pulsing,
		// Token: 0x04005BB6 RID: 23478
		PulsingAveraged,
		// Token: 0x04005BB7 RID: 23479
		SimplePulse,
		// Token: 0x04005BB8 RID: 23480
		RadialBeams,
		// Token: 0x04005BB9 RID: 23481
		Attractor
	}
}
