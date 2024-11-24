using System;
using KSerialization;

// Token: 0x02001C7D RID: 7293
[SerializationConfig(MemberSerialization.OptIn)]
public class CreatureBait : StateMachineComponent<CreatureBait.StatesInstance>
{
	// Token: 0x06009813 RID: 38931 RVA: 0x000B2F5A File Offset: 0x000B115A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06009814 RID: 38932 RVA: 0x003AEE80 File Offset: 0x003AD080
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Tag[] constructionElements = base.GetComponent<Deconstructable>().constructionElements;
		this.baitElement = ((constructionElements.Length > 1) ? constructionElements[1] : constructionElements[0]);
		base.gameObject.GetSMI<Lure.Instance>().SetActiveLures(new Tag[]
		{
			this.baitElement
		});
		base.smi.StartSM();
	}

	// Token: 0x04007669 RID: 30313
	[Serialize]
	public Tag baitElement;

	// Token: 0x02001C7E RID: 7294
	public class StatesInstance : GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.GameInstance
	{
		// Token: 0x06009816 RID: 38934 RVA: 0x00102DB9 File Offset: 0x00100FB9
		public StatesInstance(CreatureBait master) : base(master)
		{
		}
	}

	// Token: 0x02001C7F RID: 7295
	public class States : GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait>
	{
		// Token: 0x06009817 RID: 38935 RVA: 0x003AEEEC File Offset: 0x003AD0EC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Baited, null).Enter(delegate(CreatureBait.StatesInstance smi)
			{
				KAnim.Build build = ElementLoader.FindElementByName(smi.master.baitElement.ToString()).substance.anim.GetData().build;
				KAnim.Build.Symbol symbol = build.GetSymbol(new KAnimHashedString(build.name));
				HashedString target_symbol = "snapTo_bait";
				smi.GetComponent<SymbolOverrideController>().AddSymbolOverride(target_symbol, symbol, 0);
			}).TagTransition(GameTags.LureUsed, this.destroy, false);
			this.destroy.PlayAnim("use").EventHandler(GameHashes.AnimQueueComplete, delegate(CreatureBait.StatesInstance smi)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			});
		}

		// Token: 0x0400766A RID: 30314
		public GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.State idle;

		// Token: 0x0400766B RID: 30315
		public GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.State destroy;
	}
}
