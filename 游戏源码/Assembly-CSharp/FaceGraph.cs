using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020014E6 RID: 5350
[AddComponentMenu("KMonoBehaviour/scripts/FaceGraph")]
public class FaceGraph : KMonoBehaviour
{
	// Token: 0x06006F63 RID: 28515 RVA: 0x000E8E6A File Offset: 0x000E706A
	public IEnumerator<Expression> GetEnumerator()
	{
		return this.expressions.GetEnumerator();
	}

	// Token: 0x17000725 RID: 1829
	// (get) Token: 0x06006F64 RID: 28516 RVA: 0x000E8E7C File Offset: 0x000E707C
	// (set) Token: 0x06006F65 RID: 28517 RVA: 0x000E8E84 File Offset: 0x000E7084
	public Expression overrideExpression { get; private set; }

	// Token: 0x17000726 RID: 1830
	// (get) Token: 0x06006F66 RID: 28518 RVA: 0x000E8E8D File Offset: 0x000E708D
	// (set) Token: 0x06006F67 RID: 28519 RVA: 0x000E8E95 File Offset: 0x000E7095
	public Expression currentExpression { get; private set; }

	// Token: 0x06006F68 RID: 28520 RVA: 0x000E8E9E File Offset: 0x000E709E
	public void AddExpression(Expression expression)
	{
		if (this.expressions.Contains(expression))
		{
			return;
		}
		this.expressions.Add(expression);
		this.UpdateFace();
	}

	// Token: 0x06006F69 RID: 28521 RVA: 0x000E8EC1 File Offset: 0x000E70C1
	public void RemoveExpression(Expression expression)
	{
		if (this.expressions.Remove(expression))
		{
			this.UpdateFace();
		}
	}

	// Token: 0x06006F6A RID: 28522 RVA: 0x000E8ED7 File Offset: 0x000E70D7
	public void SetOverrideExpression(Expression expression)
	{
		if (expression != this.overrideExpression)
		{
			this.overrideExpression = expression;
			this.UpdateFace();
		}
	}

	// Token: 0x06006F6B RID: 28523 RVA: 0x002F36B4 File Offset: 0x002F18B4
	public void ApplyShape()
	{
		KAnimFile anim = Assets.GetAnim(FaceGraph.HASH_HEAD_MASTER_SWAP_KANIM);
		bool should_use_sideways_symbol = this.ShouldUseSidewaysSymbol(this.m_controller);
		if (this.m_blinkMonitor == null)
		{
			Accessory accessory = this.m_accessorizer.GetAccessory(Db.Get().AccessorySlots.Eyes);
			this.m_blinkMonitor = this.m_accessorizer.GetSMI<BlinkMonitor.Instance>();
			if (this.m_blinkMonitor != null)
			{
				this.m_blinkMonitor.eye_anim = accessory.Name;
			}
		}
		if (this.m_speechMonitor == null)
		{
			this.m_speechMonitor = this.m_accessorizer.GetSMI<SpeechMonitor.Instance>();
		}
		if (this.m_blinkMonitor.IsNullOrStopped() || !this.m_blinkMonitor.IsBlinking())
		{
			KAnim.Build.Symbol symbol = this.m_accessorizer.GetAccessory(Db.Get().AccessorySlots.Eyes).symbol;
			this.ApplyShape(symbol, this.m_controller, anim, FaceGraph.ANIM_HASH_SNAPTO_EYES, should_use_sideways_symbol);
		}
		if (this.m_speechMonitor.IsNullOrStopped() || !this.m_speechMonitor.IsPlayingSpeech())
		{
			KAnim.Build.Symbol symbol2 = this.m_accessorizer.GetAccessory(Db.Get().AccessorySlots.Mouth).symbol;
			this.ApplyShape(symbol2, this.m_controller, anim, FaceGraph.ANIM_HASH_SNAPTO_MOUTH, should_use_sideways_symbol);
			return;
		}
		this.m_speechMonitor.DrawMouth();
	}

	// Token: 0x06006F6C RID: 28524 RVA: 0x002F37EC File Offset: 0x002F19EC
	private bool ShouldUseSidewaysSymbol(KBatchedAnimController controller)
	{
		KAnim.Anim currentAnim = controller.GetCurrentAnim();
		if (currentAnim == null)
		{
			return false;
		}
		int currentFrameIndex = controller.GetCurrentFrameIndex();
		if (currentFrameIndex <= 0)
		{
			return false;
		}
		KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(currentAnim.animFile.animBatchTag);
		KAnim.Anim.Frame frame;
		batchGroupData.TryGetFrame(currentFrameIndex, out frame);
		for (int i = 0; i < frame.numElements; i++)
		{
			KAnim.Anim.FrameElement frameElement = batchGroupData.GetFrameElement(frame.firstElementIdx + i);
			if (frameElement.symbol == FaceGraph.ANIM_HASH_SNAPTO_EYES && frameElement.frame >= FaceGraph.FIRST_SIDEWAYS_FRAME)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006F6D RID: 28525 RVA: 0x002F387C File Offset: 0x002F1A7C
	private void ApplyShape(KAnim.Build.Symbol variation_symbol, KBatchedAnimController controller, KAnimFile shapes_file, KAnimHashedString symbol_name_in_shape_file, bool should_use_sideways_symbol)
	{
		HashedString hashedString = FaceGraph.ANIM_HASH_NEUTRAL;
		if (this.currentExpression != null)
		{
			hashedString = this.currentExpression.face.hash;
		}
		KAnim.Anim anim = null;
		KAnim.Anim.FrameElement frameElement = default(KAnim.Anim.FrameElement);
		bool flag = false;
		bool flag2 = false;
		int num = 0;
		while (num < shapes_file.GetData().animCount && !flag)
		{
			KAnim.Anim anim2 = shapes_file.GetData().GetAnim(num);
			if (anim2.hash == hashedString)
			{
				anim = anim2;
				KAnim.Anim.Frame frame;
				if (anim.TryGetFrame(shapes_file.GetData().build.batchTag, 0, out frame))
				{
					for (int i = 0; i < frame.numElements; i++)
					{
						frameElement = KAnimBatchManager.Instance().GetBatchGroupData(shapes_file.GetData().animBatchTag).GetFrameElement(frame.firstElementIdx + i);
						if (!(frameElement.symbol != symbol_name_in_shape_file))
						{
							if (flag2 || !should_use_sideways_symbol)
							{
								flag = true;
							}
							flag2 = true;
							break;
						}
					}
				}
			}
			num++;
		}
		if (anim == null)
		{
			DebugUtil.Assert(false, "Could not find shape for expression: " + HashCache.Get().Get(hashedString));
		}
		if (!flag2)
		{
			DebugUtil.Assert(false, "Could not find shape element for shape:" + HashCache.Get().Get(variation_symbol.hash));
		}
		KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(controller.batchGroupID).GetSymbol(symbol_name_in_shape_file);
		KAnim.Build.SymbolFrameInstance symbolFrameInstance = KAnimBatchManager.Instance().GetBatchGroupData(variation_symbol.build.batchTag).symbolFrameInstances[variation_symbol.firstFrameIdx + frameElement.frame];
		symbolFrameInstance.buildImageIdx = this.m_symbolOverrideController.GetAtlasIdx(variation_symbol.build.GetTexture(0));
		controller.SetSymbolOverride(symbol.firstFrameIdx, ref symbolFrameInstance);
	}

	// Token: 0x06006F6E RID: 28526 RVA: 0x002F3A2C File Offset: 0x002F1C2C
	private void UpdateFace()
	{
		Expression expression = null;
		if (this.overrideExpression != null)
		{
			expression = this.overrideExpression;
		}
		else if (this.expressions.Count > 0)
		{
			this.expressions.Sort((Expression a, Expression b) => b.priority.CompareTo(a.priority));
			expression = this.expressions[0];
		}
		if (expression != this.currentExpression || expression == null)
		{
			this.currentExpression = expression;
			this.m_symbolOverrideController.MarkDirty();
		}
		AccessorySlot headEffects = Db.Get().AccessorySlots.HeadEffects;
		if (this.currentExpression != null)
		{
			Accessory accessory = this.m_accessorizer.GetAccessory(Db.Get().AccessorySlots.HeadEffects);
			HashedString hashedString = HashedString.Invalid;
			foreach (Expression expression2 in this.expressions)
			{
				if (expression2.face.headFXHash.IsValid)
				{
					hashedString = expression2.face.headFXHash;
					break;
				}
			}
			Accessory accessory2 = (hashedString != HashedString.Invalid) ? headEffects.Lookup(hashedString) : null;
			if (accessory != accessory2)
			{
				if (accessory != null)
				{
					this.m_accessorizer.RemoveAccessory(accessory);
				}
				if (accessory2 != null)
				{
					this.m_accessorizer.AddAccessory(accessory2);
				}
			}
			this.m_controller.SetSymbolVisiblity(headEffects.targetSymbolId, accessory2 != null);
			return;
		}
		this.m_controller.SetSymbolVisiblity(headEffects.targetSymbolId, false);
	}

	// Token: 0x0400534B RID: 21323
	private List<Expression> expressions = new List<Expression>();

	// Token: 0x0400534E RID: 21326
	[MyCmpGet]
	private KBatchedAnimController m_controller;

	// Token: 0x0400534F RID: 21327
	[MyCmpGet]
	private Accessorizer m_accessorizer;

	// Token: 0x04005350 RID: 21328
	[MyCmpGet]
	private SymbolOverrideController m_symbolOverrideController;

	// Token: 0x04005351 RID: 21329
	private BlinkMonitor.Instance m_blinkMonitor;

	// Token: 0x04005352 RID: 21330
	private SpeechMonitor.Instance m_speechMonitor;

	// Token: 0x04005353 RID: 21331
	private static HashedString HASH_HEAD_MASTER_SWAP_KANIM = "head_master_swap_kanim";

	// Token: 0x04005354 RID: 21332
	private static KAnimHashedString ANIM_HASH_SNAPTO_EYES = "snapto_eyes";

	// Token: 0x04005355 RID: 21333
	private static KAnimHashedString ANIM_HASH_SNAPTO_MOUTH = "snapto_mouth";

	// Token: 0x04005356 RID: 21334
	private static KAnimHashedString ANIM_HASH_NEUTRAL = "neutral";

	// Token: 0x04005357 RID: 21335
	private static int FIRST_SIDEWAYS_FRAME = 29;
}
