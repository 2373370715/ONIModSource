using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public class HugMinionReactable : Reactable
{
	// Token: 0x06000628 RID: 1576 RVA: 0x0015B12C File Offset: 0x0015932C
	public HugMinionReactable(GameObject gameObject) : base(gameObject, "HugMinionReactable", Db.Get().ChoreTypes.Hug, 1, 1, true, 1f, 0f, float.PositiveInfinity, 0f, ObjectLayer.Minion)
	{
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x0015B174 File Offset: 0x00159374
	public override bool InternalCanBegin(GameObject newReactor, Navigator.ActiveTransition transition)
	{
		if (this.reactor != null)
		{
			return false;
		}
		Navigator component = newReactor.GetComponent<Navigator>();
		return !(component == null) && component.IsMoving();
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x000A8CDC File Offset: 0x000A6EDC
	public override void Update(float dt)
	{
		this.gameObject.GetComponent<Facing>().SetFacing(this.reactor.GetComponent<Facing>().GetFacing());
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x0015B1B0 File Offset: 0x001593B0
	protected override void InternalBegin()
	{
		KAnimControllerBase component = this.reactor.GetComponent<KAnimControllerBase>();
		component.AddAnimOverrides(Assets.GetAnim("anim_react_pip_kanim"), 0f);
		component.Play("hug_dupe_pre", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("hug_dupe_loop", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("hug_dupe_pst", KAnim.PlayMode.Once, 1f, 0f);
		component.onAnimComplete += this.Finish;
		this.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnimSequence(new HashedString[]
		{
			"hug_dupe_pre",
			"hug_dupe_loop",
			"hug_dupe_pst"
		});
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x0015B290 File Offset: 0x00159490
	private void Finish(HashedString anim)
	{
		if (anim == "hug_dupe_pst")
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KAnimControllerBase>().onAnimComplete -= this.Finish;
				this.ApplyEffects();
			}
			else
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"HugMinionReactable finishing without adding a Hugged effect."
				});
			}
			base.End();
		}
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x0015B2FC File Offset: 0x001594FC
	private void ApplyEffects()
	{
		this.reactor.GetComponent<Effects>().Add("Hugged", true);
		HugMonitor.Instance smi = this.gameObject.GetSMI<HugMonitor.Instance>();
		if (smi != null)
		{
			smi.EnterHuggingFrenzy();
		}
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected override void InternalEnd()
	{
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected override void InternalCleanup()
	{
	}
}
