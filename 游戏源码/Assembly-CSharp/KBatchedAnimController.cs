using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000921 RID: 2337
[DebuggerDisplay("{name} visible={isVisible} suspendUpdates={suspendUpdates} moving={moving}")]
public class KBatchedAnimController : KAnimControllerBase, KAnimConverter.IAnimConverter
{
	// Token: 0x060029F3 RID: 10739 RVA: 0x000BB500 File Offset: 0x000B9700
	public int GetCurrentFrameIndex()
	{
		return this.curAnimFrameIdx;
	}

	// Token: 0x060029F4 RID: 10740 RVA: 0x000BB508 File Offset: 0x000B9708
	public KBatchedAnimInstanceData GetBatchInstanceData()
	{
		return this.batchInstanceData;
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x060029F5 RID: 10741 RVA: 0x000BB510 File Offset: 0x000B9710
	// (set) Token: 0x060029F6 RID: 10742 RVA: 0x000BB518 File Offset: 0x000B9718
	protected bool forceRebuild
	{
		get
		{
			return this._forceRebuild;
		}
		set
		{
			this._forceRebuild = value;
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x060029F7 RID: 10743 RVA: 0x000BB521 File Offset: 0x000B9721
	public bool IsMoving
	{
		get
		{
			return this.moving;
		}
	}

	// Token: 0x060029F8 RID: 10744 RVA: 0x001D7928 File Offset: 0x001D5B28
	public KBatchedAnimController()
	{
		this.batchInstanceData = new KBatchedAnimInstanceData(this);
	}

	// Token: 0x060029F9 RID: 10745 RVA: 0x000BB529 File Offset: 0x000B9729
	public bool IsActive()
	{
		return base.isActiveAndEnabled && this._enabled;
	}

	// Token: 0x060029FA RID: 10746 RVA: 0x000BB53B File Offset: 0x000B973B
	public bool IsVisible()
	{
		return this.isVisible;
	}

	// Token: 0x060029FB RID: 10747 RVA: 0x001D79A8 File Offset: 0x001D5BA8
	public Vector4 GetPositionData()
	{
		if (this.getPositionDataFunctionInUse != null)
		{
			return this.getPositionDataFunctionInUse();
		}
		Vector3 position = base.transform.GetPosition();
		Vector3 positionIncludingOffset = base.PositionIncludingOffset;
		return new Vector4(position.x, position.y, positionIncludingOffset.x, positionIncludingOffset.y);
	}

	// Token: 0x060029FC RID: 10748 RVA: 0x001D79FC File Offset: 0x001D5BFC
	public void SetSymbolScale(KAnimHashedString symbol_name, float scale)
	{
		KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(this.GetBatchGroupID(false)).GetSymbol(symbol_name);
		if (symbol == null)
		{
			return;
		}
		base.symbolInstanceGpuData.SetSymbolScale(symbol.symbolIndexInSourceBuild, scale);
		this.SuspendUpdates(false);
		this.SetDirty();
	}

	// Token: 0x060029FD RID: 10749 RVA: 0x001D7A44 File Offset: 0x001D5C44
	public void SetSymbolTint(KAnimHashedString symbol_name, Color color)
	{
		KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(this.GetBatchGroupID(false)).GetSymbol(symbol_name);
		if (symbol == null)
		{
			return;
		}
		base.symbolInstanceGpuData.SetSymbolTint(symbol.symbolIndexInSourceBuild, color);
		this.SuspendUpdates(false);
		this.SetDirty();
	}

	// Token: 0x060029FE RID: 10750 RVA: 0x001D7A8C File Offset: 0x001D5C8C
	public Vector2I GetCellXY()
	{
		Vector3 positionIncludingOffset = base.PositionIncludingOffset;
		if (Grid.CellSizeInMeters == 0f)
		{
			return new Vector2I((int)positionIncludingOffset.x, (int)positionIncludingOffset.y);
		}
		return Grid.PosToXY(positionIncludingOffset);
	}

	// Token: 0x060029FF RID: 10751 RVA: 0x000BB543 File Offset: 0x000B9743
	public float GetZ()
	{
		return base.transform.GetPosition().z;
	}

	// Token: 0x06002A00 RID: 10752 RVA: 0x000BB555 File Offset: 0x000B9755
	public string GetName()
	{
		return base.name;
	}

	// Token: 0x06002A01 RID: 10753 RVA: 0x001D7AC8 File Offset: 0x001D5CC8
	public override KAnim.Anim GetAnim(int index)
	{
		if (!this.batchGroupID.IsValid || !(this.batchGroupID != KAnimBatchManager.NO_BATCH))
		{
			global::Debug.LogError(base.name + " batch not ready");
		}
		KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.batchGroupID);
		global::Debug.Assert(batchGroupData != null);
		return batchGroupData.GetAnim(index);
	}

	// Token: 0x06002A02 RID: 10754 RVA: 0x001D7B2C File Offset: 0x001D5D2C
	private void Initialize()
	{
		if (this.batchGroupID.IsValid && this.batchGroupID != KAnimBatchManager.NO_BATCH)
		{
			this.DeRegister();
			this.Register();
		}
	}

	// Token: 0x06002A03 RID: 10755 RVA: 0x000BB55D File Offset: 0x000B975D
	private void OnMovementStateChanged(bool is_moving)
	{
		if (is_moving == this.moving)
		{
			return;
		}
		this.moving = is_moving;
		this.SetDirty();
		this.ConfigureUpdateListener();
	}

	// Token: 0x06002A04 RID: 10756 RVA: 0x000BB57C File Offset: 0x000B977C
	private static void OnMovementStateChanged(Transform transform, bool is_moving)
	{
		transform.GetComponent<KBatchedAnimController>().OnMovementStateChanged(is_moving);
	}

	// Token: 0x06002A05 RID: 10757 RVA: 0x001D7B68 File Offset: 0x001D5D68
	private void SetBatchGroup(KAnimFileData kafd)
	{
		if (this.batchGroupID.IsValid && kafd != null && this.batchGroupID == kafd.batchTag)
		{
			return;
		}
		DebugUtil.Assert(!this.batchGroupID.IsValid, "Should only be setting the batch group once.");
		DebugUtil.Assert(kafd != null, "Null anim data!! For", base.name);
		base.curBuild = kafd.build;
		DebugUtil.Assert(base.curBuild != null, "Null build for anim!! ", base.name, kafd.name);
		KAnimGroupFile.Group group = KAnimGroupFile.GetGroup(base.curBuild.batchTag);
		HashedString batchGroupID = kafd.build.batchTag;
		if (group.renderType == KAnimBatchGroup.RendererType.DontRender || group.renderType == KAnimBatchGroup.RendererType.AnimOnly)
		{
			bool isValid = group.swapTarget.IsValid;
			string str = "Invalid swap target fro group [";
			HashedString id = group.id;
			global::Debug.Assert(isValid, str + id.ToString() + "]");
			batchGroupID = group.swapTarget;
		}
		this.batchGroupID = batchGroupID;
		base.symbolInstanceGpuData = new SymbolInstanceGpuData(KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID).maxSymbolsPerBuild);
		base.symbolOverrideInfoGpuData = new SymbolOverrideInfoGpuData(KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID).symbolFrameInstances.Count);
		if (!this.batchGroupID.IsValid || this.batchGroupID == KAnimBatchManager.NO_BATCH)
		{
			global::Debug.LogError("Batch is not ready: " + base.name);
		}
		if (this.materialType == KAnimBatchGroup.MaterialType.Default && this.batchGroupID == KAnimBatchManager.BATCH_HUMAN)
		{
			this.materialType = KAnimBatchGroup.MaterialType.Human;
		}
	}

	// Token: 0x06002A06 RID: 10758 RVA: 0x001D7D04 File Offset: 0x001D5F04
	public void LoadAnims()
	{
		if (!KAnimBatchManager.Instance().isReady)
		{
			global::Debug.LogError("KAnimBatchManager is not ready when loading anim:" + base.name);
		}
		if (this.animFiles.Length == 0)
		{
			DebugUtil.Assert(false, "KBatchedAnimController has no anim files:" + base.name);
		}
		if (!this.animFiles[0].IsBuildLoaded)
		{
			DebugUtil.LogErrorArgs(base.gameObject, new object[]
			{
				string.Format("First anim file needs to be the build file but {0} doesn't have an associated build", this.animFiles[0].GetData().name)
			});
		}
		this.overrideAnims.Clear();
		this.anims.Clear();
		this.SetBatchGroup(this.animFiles[0].GetData());
		for (int i = 0; i < this.animFiles.Length; i++)
		{
			base.AddAnims(this.animFiles[i]);
		}
		this.forceRebuild = true;
		if (this.layering != null)
		{
			this.layering.HideSymbols();
		}
		if (this.usingNewSymbolOverrideSystem)
		{
			DebugUtil.Assert(base.GetComponent<SymbolOverrideController>() != null);
		}
	}

	// Token: 0x06002A07 RID: 10759 RVA: 0x001D7E10 File Offset: 0x001D6010
	public void SwapAnims(KAnimFile[] anims)
	{
		if (this.batchGroupID.IsValid)
		{
			this.DeRegister();
			this.batchGroupID = HashedString.Invalid;
		}
		base.AnimFiles = anims;
		this.LoadAnims();
		if (base.curBuild != null)
		{
			this.UpdateHiddenSymbolSet(this.hiddenSymbolsSet);
		}
		this.Register();
	}

	// Token: 0x06002A08 RID: 10760 RVA: 0x001D7E68 File Offset: 0x001D6068
	public void UpdateAnim(float dt)
	{
		if (this.batch != null && base.transform.hasChanged)
		{
			base.transform.hasChanged = false;
			if (this.batch != null && this.batch.group.maxGroupSize == 1 && this.lastPos.z != base.transform.GetPosition().z)
			{
				this.batch.OverrideZ(base.transform.GetPosition().z);
			}
			Vector3 positionIncludingOffset = base.PositionIncludingOffset;
			this.lastPos = positionIncludingOffset;
			if (this.visibilityType != KAnimControllerBase.VisibilityType.Always && KAnimBatchManager.ControllerToChunkXY(this) != this.lastChunkXY && this.lastChunkXY != KBatchedAnimUpdater.INVALID_CHUNK_ID)
			{
				this.DeRegister();
				this.Register();
			}
			this.SetDirty();
		}
		if (this.batchGroupID == KAnimBatchManager.NO_BATCH || !this.IsActive())
		{
			return;
		}
		if (!this.forceRebuild && (this.mode == KAnim.PlayMode.Paused || this.stopped || this.curAnim == null || (this.mode == KAnim.PlayMode.Once && this.curAnim != null && (this.elapsedTime > this.curAnim.totalTime || this.curAnim.totalTime <= 0f) && this.animQueue.Count == 0)))
		{
			this.SuspendUpdates(true);
		}
		if (!this.isVisible && !this.forceRebuild)
		{
			if (this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate && !this.stopped && this.mode != KAnim.PlayMode.Paused)
			{
				base.SetElapsedTime(this.elapsedTime + dt * this.playSpeed);
			}
			return;
		}
		this.curAnimFrameIdx = base.GetFrameIdx(this.elapsedTime, true);
		if (this.eventManagerHandle.IsValid() && this.aem != null)
		{
			float elapsedTime = this.aem.GetElapsedTime(this.eventManagerHandle);
			if ((int)((this.elapsedTime - elapsedTime) * 100f) != 0)
			{
				base.UpdateAnimEventSequenceTime();
			}
		}
		this.UpdateFrame(this.elapsedTime);
		if (!this.stopped && this.mode != KAnim.PlayMode.Paused)
		{
			base.SetElapsedTime(this.elapsedTime + dt * this.playSpeed);
		}
		this.forceRebuild = false;
	}

	// Token: 0x06002A09 RID: 10761 RVA: 0x001D8090 File Offset: 0x001D6290
	protected override void UpdateFrame(float t)
	{
		base.previousFrame = base.currentFrame;
		if (!this.stopped || this.forceRebuild)
		{
			if (this.curAnim != null && (this.mode == KAnim.PlayMode.Loop || this.elapsedTime <= base.GetDuration() || this.forceRebuild))
			{
				base.currentFrame = this.curAnim.GetFrameIdx(this.mode, this.elapsedTime);
				if (base.currentFrame != base.previousFrame || this.forceRebuild)
				{
					this.SetDirty();
				}
			}
			else
			{
				this.TriggerStop();
			}
			if (!this.stopped && this.mode == KAnim.PlayMode.Loop && base.currentFrame == 0)
			{
				base.AnimEnter(this.curAnim.hash);
			}
		}
		if (this.synchronizer != null)
		{
			this.synchronizer.SyncTime();
		}
	}

	// Token: 0x06002A0A RID: 10762 RVA: 0x001D8160 File Offset: 0x001D6360
	public override void TriggerStop()
	{
		if (this.animQueue.Count > 0)
		{
			base.StartQueuedAnim();
			return;
		}
		if (this.curAnim != null && this.mode == KAnim.PlayMode.Once)
		{
			base.currentFrame = this.curAnim.numFrames - 1;
			base.Stop();
			base.gameObject.Trigger(-1061186183, null);
			if (this.destroyOnAnimComplete)
			{
				base.DestroySelf();
			}
		}
	}

	// Token: 0x06002A0B RID: 10763 RVA: 0x001D81CC File Offset: 0x001D63CC
	public override void UpdateHiddenSymbol(KAnimHashedString symbolToUpdate)
	{
		KBatchGroupData batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID);
		for (int i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
		{
			if (!(symbolToUpdate != batchGroupData.frameElementSymbols[i].hash))
			{
				KAnim.Build.Symbol symbol = batchGroupData.frameElementSymbols[i];
				bool is_visible = !this.hiddenSymbolsSet.Contains(symbol.hash);
				base.symbolInstanceGpuData.SetVisible(i, is_visible);
			}
		}
		this.SetDirty();
	}

	// Token: 0x06002A0C RID: 10764 RVA: 0x001D8250 File Offset: 0x001D6450
	public override void UpdateHiddenSymbolSet(HashSet<KAnimHashedString> symbolsToUpdate)
	{
		KBatchGroupData batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID);
		for (int i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
		{
			if (symbolsToUpdate.Contains(batchGroupData.frameElementSymbols[i].hash))
			{
				KAnim.Build.Symbol symbol = batchGroupData.frameElementSymbols[i];
				bool is_visible = !this.hiddenSymbolsSet.Contains(symbol.hash);
				base.symbolInstanceGpuData.SetVisible(i, is_visible);
			}
		}
		this.SetDirty();
	}

	// Token: 0x06002A0D RID: 10765 RVA: 0x001D82D4 File Offset: 0x001D64D4
	public override void UpdateAllHiddenSymbols()
	{
		KBatchGroupData batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID);
		for (int i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
		{
			KAnim.Build.Symbol symbol = batchGroupData.frameElementSymbols[i];
			bool is_visible = !this.hiddenSymbolsSet.Contains(symbol.hash);
			base.symbolInstanceGpuData.SetVisible(i, is_visible);
		}
		this.SetDirty();
	}

	// Token: 0x06002A0E RID: 10766 RVA: 0x000BB58A File Offset: 0x000B978A
	public int GetMaxVisible()
	{
		return this.maxSymbols;
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06002A0F RID: 10767 RVA: 0x000BB592 File Offset: 0x000B9792
	// (set) Token: 0x06002A10 RID: 10768 RVA: 0x000BB59A File Offset: 0x000B979A
	public HashedString batchGroupID { get; private set; }

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06002A11 RID: 10769 RVA: 0x000BB5A3 File Offset: 0x000B97A3
	// (set) Token: 0x06002A12 RID: 10770 RVA: 0x000BB5AB File Offset: 0x000B97AB
	public HashedString batchGroupIDOverride { get; private set; }

	// Token: 0x06002A13 RID: 10771 RVA: 0x001D8340 File Offset: 0x001D6540
	public HashedString GetBatchGroupID(bool isEditorWindow = false)
	{
		global::Debug.Assert(isEditorWindow || this.animFiles == null || this.animFiles.Length == 0 || (this.batchGroupID.IsValid && this.batchGroupID != KAnimBatchManager.NO_BATCH));
		return this.batchGroupID;
	}

	// Token: 0x06002A14 RID: 10772 RVA: 0x000BB5B4 File Offset: 0x000B97B4
	public HashedString GetBatchGroupIDOverride()
	{
		return this.batchGroupIDOverride;
	}

	// Token: 0x06002A15 RID: 10773 RVA: 0x000BB5BC File Offset: 0x000B97BC
	public void SetBatchGroupOverride(HashedString id)
	{
		this.batchGroupIDOverride = id;
		this.DeRegister();
		this.Register();
	}

	// Token: 0x06002A16 RID: 10774 RVA: 0x000BB5D1 File Offset: 0x000B97D1
	public int GetLayer()
	{
		return base.gameObject.layer;
	}

	// Token: 0x06002A17 RID: 10775 RVA: 0x000BB5DE File Offset: 0x000B97DE
	public KAnimBatch GetBatch()
	{
		return this.batch;
	}

	// Token: 0x06002A18 RID: 10776 RVA: 0x001D8398 File Offset: 0x001D6598
	public void SetBatch(KAnimBatch new_batch)
	{
		this.batch = new_batch;
		if (this.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			KBatchedAnimCanvasRenderer kbatchedAnimCanvasRenderer = base.GetComponent<KBatchedAnimCanvasRenderer>();
			if (kbatchedAnimCanvasRenderer == null && new_batch != null)
			{
				kbatchedAnimCanvasRenderer = base.gameObject.AddComponent<KBatchedAnimCanvasRenderer>();
			}
			if (kbatchedAnimCanvasRenderer != null)
			{
				kbatchedAnimCanvasRenderer.SetBatch(this);
			}
		}
	}

	// Token: 0x06002A19 RID: 10777 RVA: 0x000BB5E6 File Offset: 0x000B97E6
	public int GetCurrentNumFrames()
	{
		if (this.curAnim == null)
		{
			return 0;
		}
		return this.curAnim.numFrames;
	}

	// Token: 0x06002A1A RID: 10778 RVA: 0x000BB5FD File Offset: 0x000B97FD
	public int GetFirstFrameIndex()
	{
		if (this.curAnim == null)
		{
			return -1;
		}
		return this.curAnim.firstFrameIdx;
	}

	// Token: 0x06002A1B RID: 10779 RVA: 0x001D83E4 File Offset: 0x001D65E4
	private Canvas GetRootCanvas()
	{
		if (this.rt == null)
		{
			return null;
		}
		RectTransform component = this.rt.parent.GetComponent<RectTransform>();
		while (component != null)
		{
			Canvas component2 = component.GetComponent<Canvas>();
			if (component2 != null && component2.isRootCanvas)
			{
				return component2;
			}
			component = component.parent.GetComponent<RectTransform>();
		}
		return null;
	}

	// Token: 0x06002A1C RID: 10780 RVA: 0x001D8448 File Offset: 0x001D6648
	public override Matrix2x3 GetTransformMatrix()
	{
		Vector3 v = base.PositionIncludingOffset;
		v.z = 0f;
		Vector2 scale = new Vector2(this.animScale * this.animWidth, -this.animScale * this.animHeight);
		if (this.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			this.rt = base.GetComponent<RectTransform>();
			if (this.rootCanvas == null)
			{
				this.rootCanvas = this.GetRootCanvas();
			}
			if (this.scaler == null && this.rootCanvas != null)
			{
				this.scaler = this.rootCanvas.GetComponent<CanvasScaler>();
			}
			if (this.rootCanvas == null)
			{
				this.screenOffset.x = (float)(Screen.width / 2);
				this.screenOffset.y = (float)(Screen.height / 2);
			}
			else
			{
				this.screenOffset.x = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.width / 2f));
				this.screenOffset.y = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.height / 2f));
			}
			float num = 1f;
			if (this.scaler != null)
			{
				num = 1f / this.scaler.scaleFactor;
			}
			v = (this.rt.localToWorldMatrix.MultiplyPoint(this.rt.pivot) + this.offset) * num - this.screenOffset;
			float num2 = this.animWidth * this.animScale;
			float num3 = this.animHeight * this.animScale;
			if (this.setScaleFromAnim && this.curAnim != null)
			{
				num2 *= this.rt.rect.size.x / this.curAnim.unScaledSize.x;
				num3 *= this.rt.rect.size.y / this.curAnim.unScaledSize.y;
			}
			else
			{
				num2 *= this.rt.rect.size.x / this.animOverrideSize.x;
				num3 *= this.rt.rect.size.y / this.animOverrideSize.y;
			}
			scale = new Vector3(this.rt.lossyScale.x * num2 * num, -this.rt.lossyScale.y * num3 * num, this.rt.lossyScale.z * num);
			this.pivot = this.rt.pivot;
		}
		Matrix2x3 n = Matrix2x3.Scale(scale);
		Matrix2x3 n2 = Matrix2x3.Scale(new Vector2(this.flipX ? -1f : 1f, this.flipY ? -1f : 1f));
		Matrix2x3 result;
		if (this.rotation != 0f)
		{
			Matrix2x3 n3 = Matrix2x3.Translate(-this.pivot);
			Matrix2x3 n4 = Matrix2x3.Rotate(this.rotation * 0.017453292f);
			Matrix2x3 n5 = Matrix2x3.Translate(this.pivot) * n4 * n3;
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n5 * n * this.navMatrix * n2;
		}
		else
		{
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n * this.navMatrix * n2;
		}
		return result;
	}

	// Token: 0x06002A1D RID: 10781 RVA: 0x001D8868 File Offset: 0x001D6A68
	public Matrix2x3 GetTransformMatrix(Vector2 customScale)
	{
		Vector3 v = base.PositionIncludingOffset;
		v.z = 0f;
		Vector2 scale = customScale;
		if (this.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			this.rt = base.GetComponent<RectTransform>();
			if (this.rootCanvas == null)
			{
				this.rootCanvas = this.GetRootCanvas();
			}
			if (this.scaler == null && this.rootCanvas != null)
			{
				this.scaler = this.rootCanvas.GetComponent<CanvasScaler>();
			}
			if (this.rootCanvas == null)
			{
				this.screenOffset.x = (float)(Screen.width / 2);
				this.screenOffset.y = (float)(Screen.height / 2);
			}
			else
			{
				this.screenOffset.x = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.width / 2f));
				this.screenOffset.y = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.height / 2f));
			}
			float num = 1f;
			if (this.scaler != null)
			{
				num = 1f / this.scaler.scaleFactor;
			}
			v = (this.rt.localToWorldMatrix.MultiplyPoint(this.rt.pivot) + this.offset) * num - this.screenOffset;
			float num2 = this.animWidth * this.animScale;
			float num3 = this.animHeight * this.animScale;
			if (this.setScaleFromAnim && this.curAnim != null)
			{
				num2 *= this.rt.rect.size.x / this.curAnim.unScaledSize.x;
				num3 *= this.rt.rect.size.y / this.curAnim.unScaledSize.y;
			}
			else
			{
				num2 *= this.rt.rect.size.x / this.animOverrideSize.x;
				num3 *= this.rt.rect.size.y / this.animOverrideSize.y;
			}
			scale = new Vector3(this.rt.lossyScale.x * num2 * num, -this.rt.lossyScale.y * num3 * num, this.rt.lossyScale.z * num);
			this.pivot = this.rt.pivot;
		}
		Matrix2x3 n = Matrix2x3.Scale(scale);
		Matrix2x3 n2 = Matrix2x3.Scale(new Vector2(this.flipX ? -1f : 1f, this.flipY ? -1f : 1f));
		Matrix2x3 result;
		if (this.rotation != 0f)
		{
			Matrix2x3 n3 = Matrix2x3.Translate(-this.pivot);
			Matrix2x3 n4 = Matrix2x3.Rotate(this.rotation * 0.017453292f);
			Matrix2x3 n5 = Matrix2x3.Translate(this.pivot) * n4 * n3;
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n5 * n * this.navMatrix * n2;
		}
		else
		{
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n * this.navMatrix * n2;
		}
		return result;
	}

	// Token: 0x06002A1E RID: 10782 RVA: 0x001D8C68 File Offset: 0x001D6E68
	public override Matrix4x4 GetSymbolTransform(HashedString symbol, out bool symbolVisible)
	{
		if (this.curAnimFrameIdx != -1 && this.batch != null)
		{
			Matrix2x3 symbolLocalTransform = this.GetSymbolLocalTransform(symbol, out symbolVisible);
			if (symbolVisible)
			{
				return this.GetTransformMatrix() * symbolLocalTransform;
			}
		}
		symbolVisible = false;
		return default(Matrix4x4);
	}

	// Token: 0x06002A1F RID: 10783 RVA: 0x001D8CB8 File Offset: 0x001D6EB8
	public override Matrix2x3 GetSymbolLocalTransform(HashedString symbol, out bool symbolVisible)
	{
		KAnim.Anim.Frame frame;
		if (this.curAnimFrameIdx != -1 && this.batch != null && this.batch.group.data.TryGetFrame(this.curAnimFrameIdx, out frame))
		{
			for (int i = 0; i < frame.numElements; i++)
			{
				int num = frame.firstElementIdx + i;
				if (num < this.batch.group.data.frameElements.Count)
				{
					KAnim.Anim.FrameElement frameElement = this.batch.group.data.frameElements[num];
					if (frameElement.symbol == symbol)
					{
						symbolVisible = true;
						return frameElement.transform;
					}
				}
			}
		}
		symbolVisible = false;
		return Matrix2x3.identity;
	}

	// Token: 0x06002A20 RID: 10784 RVA: 0x000BB614 File Offset: 0x000B9814
	public override void SetLayer(int layer)
	{
		if (layer == base.gameObject.layer)
		{
			return;
		}
		base.SetLayer(layer);
		this.DeRegister();
		base.gameObject.layer = layer;
		this.Register();
	}

	// Token: 0x06002A21 RID: 10785 RVA: 0x000BB644 File Offset: 0x000B9844
	public override void SetDirty()
	{
		if (this.batch != null)
		{
			this.batch.SetDirty(this);
		}
	}

	// Token: 0x06002A22 RID: 10786 RVA: 0x000BB65A File Offset: 0x000B985A
	protected override void OnStartQueuedAnim()
	{
		this.SuspendUpdates(false);
	}

	// Token: 0x06002A23 RID: 10787 RVA: 0x001D8D70 File Offset: 0x001D6F70
	protected override void OnAwake()
	{
		this.LoadAnims();
		if (this.visibilityType == KAnimControllerBase.VisibilityType.Default)
		{
			this.visibilityType = ((this.materialType == KAnimBatchGroup.MaterialType.UI) ? KAnimControllerBase.VisibilityType.Always : this.visibilityType);
		}
		if (this.materialType == KAnimBatchGroup.MaterialType.Default && this.batchGroupID == KAnimBatchManager.BATCH_HUMAN)
		{
			this.materialType = KAnimBatchGroup.MaterialType.Human;
		}
		this.symbolOverrideController = base.GetComponent<SymbolOverrideController>();
		this.UpdateAllHiddenSymbols();
		this.hasEnableRun = false;
	}

	// Token: 0x06002A24 RID: 10788 RVA: 0x001D8DE0 File Offset: 0x001D6FE0
	protected override void OnStart()
	{
		if (this.batch == null)
		{
			this.Initialize();
		}
		if (this.visibilityType == KAnimControllerBase.VisibilityType.Always || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate)
		{
			this.ConfigureUpdateListener();
		}
		CellChangeMonitor instance = Singleton<CellChangeMonitor>.Instance;
		if (instance != null)
		{
			instance.RegisterMovementStateChanged(base.transform, new Action<Transform, bool>(KBatchedAnimController.OnMovementStateChanged));
			this.moving = instance.IsMoving(base.transform);
		}
		this.symbolOverrideController = base.GetComponent<SymbolOverrideController>();
		this.SetDirty();
	}

	// Token: 0x06002A25 RID: 10789 RVA: 0x000BB663 File Offset: 0x000B9863
	protected override void OnStop()
	{
		this.SetDirty();
	}

	// Token: 0x06002A26 RID: 10790 RVA: 0x000BB66B File Offset: 0x000B986B
	private void OnEnable()
	{
		if (this._enabled)
		{
			this.Enable();
		}
	}

	// Token: 0x06002A27 RID: 10791 RVA: 0x001D8E58 File Offset: 0x001D7058
	protected override void Enable()
	{
		if (this.hasEnableRun)
		{
			return;
		}
		this.hasEnableRun = true;
		if (this.batch == null)
		{
			this.Initialize();
		}
		this.SetDirty();
		this.SuspendUpdates(false);
		this.ConfigureVisibilityListener(true);
		if (!this.stopped && this.curAnim != null && this.mode != KAnim.PlayMode.Paused && !this.eventManagerHandle.IsValid())
		{
			base.StartAnimEventSequence();
		}
	}

	// Token: 0x06002A28 RID: 10792 RVA: 0x000BB67B File Offset: 0x000B987B
	private void OnDisable()
	{
		this.Disable();
	}

	// Token: 0x06002A29 RID: 10793 RVA: 0x001D8EC4 File Offset: 0x001D70C4
	protected override void Disable()
	{
		if (App.IsExiting || KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		if (!this.hasEnableRun)
		{
			return;
		}
		this.hasEnableRun = false;
		this.SuspendUpdates(true);
		if (this.batch != null)
		{
			this.DeRegister();
		}
		this.ConfigureVisibilityListener(false);
		base.StopAnimEventSequence();
	}

	// Token: 0x06002A2A RID: 10794 RVA: 0x001D8F14 File Offset: 0x001D7114
	protected override void OnDestroy()
	{
		if (App.IsExiting)
		{
			return;
		}
		CellChangeMonitor instance = Singleton<CellChangeMonitor>.Instance;
		if (instance != null)
		{
			instance.UnregisterMovementStateChanged(base.transform, new Action<Transform, bool>(KBatchedAnimController.OnMovementStateChanged));
		}
		KBatchedAnimUpdater instance2 = Singleton<KBatchedAnimUpdater>.Instance;
		if (instance2 != null)
		{
			instance2.UpdateUnregister(this);
		}
		this.isVisible = false;
		this.DeRegister();
		this.stopped = true;
		base.StopAnimEventSequence();
		this.batchInstanceData = null;
		this.batch = null;
		base.OnDestroy();
	}

	// Token: 0x06002A2B RID: 10795 RVA: 0x000BB683 File Offset: 0x000B9883
	public void SetBlendValue(float value)
	{
		this.batchInstanceData.SetBlend(value);
		this.SetDirty();
	}

	// Token: 0x06002A2C RID: 10796 RVA: 0x000BB697 File Offset: 0x000B9897
	public SymbolOverrideController SetupSymbolOverriding()
	{
		if (!this.symbolOverrideController.IsNullOrDestroyed())
		{
			return this.symbolOverrideController;
		}
		this.usingNewSymbolOverrideSystem = true;
		this.symbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(base.gameObject);
		return this.symbolOverrideController;
	}

	// Token: 0x06002A2D RID: 10797 RVA: 0x001D8F88 File Offset: 0x001D7188
	public bool ApplySymbolOverrides()
	{
		this.batch.atlases.Apply(this.batch.matProperties);
		if (this.symbolOverrideController != null)
		{
			if (this.symbolOverrideControllerVersion != this.symbolOverrideController.version || this.symbolOverrideController.applySymbolOverridesEveryFrame)
			{
				this.symbolOverrideControllerVersion = this.symbolOverrideController.version;
				this.symbolOverrideController.ApplyOverrides();
			}
			this.symbolOverrideController.ApplyAtlases();
			return true;
		}
		return false;
	}

	// Token: 0x06002A2E RID: 10798 RVA: 0x000BB6CB File Offset: 0x000B98CB
	public void SetSymbolOverrides(int symbol_start_idx, int symbol_num_frames, int atlas_idx, KBatchGroupData source_data, int source_start_idx, int source_num_frames)
	{
		base.symbolOverrideInfoGpuData.SetSymbolOverrideInfo(symbol_start_idx, symbol_num_frames, atlas_idx, source_data, source_start_idx, source_num_frames);
	}

	// Token: 0x06002A2F RID: 10799 RVA: 0x000BB6E1 File Offset: 0x000B98E1
	public void SetSymbolOverride(int symbol_idx, ref KAnim.Build.SymbolFrameInstance symbol_frame_instance)
	{
		base.symbolOverrideInfoGpuData.SetSymbolOverrideInfo(symbol_idx, ref symbol_frame_instance);
	}

	// Token: 0x06002A30 RID: 10800 RVA: 0x001D9008 File Offset: 0x001D7208
	protected override void Register()
	{
		if (!this.IsActive())
		{
			return;
		}
		if (this.batch != null)
		{
			return;
		}
		if (this.batchGroupID.IsValid && this.batchGroupID != KAnimBatchManager.NO_BATCH)
		{
			this.lastChunkXY = KAnimBatchManager.ControllerToChunkXY(this);
			KAnimBatchManager.Instance().Register(this);
			this.forceRebuild = true;
			this.SetDirty();
		}
	}

	// Token: 0x06002A31 RID: 10801 RVA: 0x000BB6F0 File Offset: 0x000B98F0
	protected override void DeRegister()
	{
		if (this.batch != null)
		{
			this.batch.Deregister(this);
		}
	}

	// Token: 0x06002A32 RID: 10802 RVA: 0x001D9070 File Offset: 0x001D7270
	private void ConfigureUpdateListener()
	{
		if ((this.IsActive() && !this.suspendUpdates && this.isVisible) || this.moving || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate || this.visibilityType == KAnimControllerBase.VisibilityType.Always)
		{
			Singleton<KBatchedAnimUpdater>.Instance.UpdateRegister(this);
			return;
		}
		Singleton<KBatchedAnimUpdater>.Instance.UpdateUnregister(this);
	}

	// Token: 0x06002A33 RID: 10803 RVA: 0x000BB706 File Offset: 0x000B9906
	protected override void SuspendUpdates(bool suspend)
	{
		this.suspendUpdates = suspend;
		this.ConfigureUpdateListener();
	}

	// Token: 0x06002A34 RID: 10804 RVA: 0x000BB715 File Offset: 0x000B9915
	public void SetVisiblity(bool is_visible)
	{
		if (is_visible != this.isVisible)
		{
			this.isVisible = is_visible;
			if (is_visible)
			{
				this.SuspendUpdates(false);
				this.SetDirty();
				base.UpdateAnimEventSequenceTime();
				return;
			}
			this.SuspendUpdates(true);
			this.SetDirty();
		}
	}

	// Token: 0x06002A35 RID: 10805 RVA: 0x000BB74B File Offset: 0x000B994B
	private void ConfigureVisibilityListener(bool enabled)
	{
		if (this.visibilityType == KAnimControllerBase.VisibilityType.Always || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate)
		{
			return;
		}
		if (enabled)
		{
			this.RegisterVisibilityListener();
			return;
		}
		this.UnregisterVisibilityListener();
	}

	// Token: 0x06002A36 RID: 10806 RVA: 0x000BB770 File Offset: 0x000B9970
	public virtual KAnimConverter.PostProcessingEffects GetPostProcessingEffectsCompatibility()
	{
		return this.postProcessingEffectsAllowed;
	}

	// Token: 0x06002A37 RID: 10807 RVA: 0x000BB778 File Offset: 0x000B9978
	public float GetPostProcessingParams()
	{
		return this.postProcessingParameters;
	}

	// Token: 0x06002A38 RID: 10808 RVA: 0x000BB780 File Offset: 0x000B9980
	protected override void RefreshVisibilityListener()
	{
		if (!this.visibilityListenerRegistered)
		{
			return;
		}
		this.ConfigureVisibilityListener(false);
		this.ConfigureVisibilityListener(true);
	}

	// Token: 0x06002A39 RID: 10809 RVA: 0x000BB799 File Offset: 0x000B9999
	private void RegisterVisibilityListener()
	{
		DebugUtil.Assert(!this.visibilityListenerRegistered);
		Singleton<KBatchedAnimUpdater>.Instance.VisibilityRegister(this);
		this.visibilityListenerRegistered = true;
	}

	// Token: 0x06002A3A RID: 10810 RVA: 0x000BB7BB File Offset: 0x000B99BB
	private void UnregisterVisibilityListener()
	{
		DebugUtil.Assert(this.visibilityListenerRegistered);
		Singleton<KBatchedAnimUpdater>.Instance.VisibilityUnregister(this);
		this.visibilityListenerRegistered = false;
	}

	// Token: 0x06002A3B RID: 10811 RVA: 0x001D90CC File Offset: 0x001D72CC
	public void SetSceneLayer(Grid.SceneLayer layer)
	{
		float layerZ = Grid.GetLayerZ(layer);
		this.sceneLayer = layer;
		Vector3 position = base.transform.GetPosition();
		position.z = layerZ;
		base.transform.SetPosition(position);
		this.DeRegister();
		this.Register();
	}

	// Token: 0x04001BEC RID: 7148
	[NonSerialized]
	protected bool _forceRebuild;

	// Token: 0x04001BED RID: 7149
	private Vector3 lastPos = Vector3.zero;

	// Token: 0x04001BEE RID: 7150
	private Vector2I lastChunkXY = KBatchedAnimUpdater.INVALID_CHUNK_ID;

	// Token: 0x04001BEF RID: 7151
	private KAnimBatch batch;

	// Token: 0x04001BF0 RID: 7152
	public float animScale = 0.005f;

	// Token: 0x04001BF1 RID: 7153
	private bool suspendUpdates;

	// Token: 0x04001BF2 RID: 7154
	private bool visibilityListenerRegistered;

	// Token: 0x04001BF3 RID: 7155
	private bool moving;

	// Token: 0x04001BF4 RID: 7156
	private SymbolOverrideController symbolOverrideController;

	// Token: 0x04001BF5 RID: 7157
	private int symbolOverrideControllerVersion;

	// Token: 0x04001BF6 RID: 7158
	[NonSerialized]
	public KBatchedAnimUpdater.RegistrationState updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Unregistered;

	// Token: 0x04001BF7 RID: 7159
	public Grid.SceneLayer sceneLayer;

	// Token: 0x04001BF8 RID: 7160
	private RectTransform rt;

	// Token: 0x04001BF9 RID: 7161
	private Vector3 screenOffset = new Vector3(0f, 0f, 0f);

	// Token: 0x04001BFA RID: 7162
	public Matrix2x3 navMatrix = Matrix2x3.identity;

	// Token: 0x04001BFB RID: 7163
	private CanvasScaler scaler;

	// Token: 0x04001BFC RID: 7164
	public bool setScaleFromAnim = true;

	// Token: 0x04001BFD RID: 7165
	public Vector2 animOverrideSize = Vector2.one;

	// Token: 0x04001BFE RID: 7166
	private Canvas rootCanvas;

	// Token: 0x04001BFF RID: 7167
	public bool isMovable;

	// Token: 0x04001C00 RID: 7168
	public Func<Vector4> getPositionDataFunctionInUse;

	// Token: 0x04001C01 RID: 7169
	public KAnimConverter.PostProcessingEffects postProcessingEffectsAllowed;

	// Token: 0x04001C02 RID: 7170
	public float postProcessingParameters;
}
