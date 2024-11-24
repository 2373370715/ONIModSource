using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020012A1 RID: 4769
public class EntityLuminescence : GameStateMachine<EntityLuminescence, EntityLuminescence.Instance, IStateMachineTarget, EntityLuminescence.Def>
{
	// Token: 0x0600621C RID: 25116 RVA: 0x000E001A File Offset: 0x000DE21A
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x020012A2 RID: 4770
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040045D0 RID: 17872
		public Color lightColor;

		// Token: 0x040045D1 RID: 17873
		public float lightRange;

		// Token: 0x040045D2 RID: 17874
		public float lightAngle;

		// Token: 0x040045D3 RID: 17875
		public Vector2 lightOffset;

		// Token: 0x040045D4 RID: 17876
		public Vector2 lightDirection;

		// Token: 0x040045D5 RID: 17877
		public global::LightShape lightShape;
	}

	// Token: 0x020012A3 RID: 4771
	public new class Instance : GameStateMachine<EntityLuminescence, EntityLuminescence.Instance, IStateMachineTarget, EntityLuminescence.Def>.GameInstance
	{
		// Token: 0x0600621F RID: 25119 RVA: 0x002B542C File Offset: 0x002B362C
		public Instance(IStateMachineTarget master, EntityLuminescence.Def def) : base(master, def)
		{
			this.light.Color = def.lightColor;
			this.light.Range = def.lightRange;
			this.light.Angle = def.lightAngle;
			this.light.Direction = def.lightDirection;
			this.light.Offset = def.lightOffset;
			this.light.shape = def.lightShape;
		}

		// Token: 0x06006220 RID: 25120 RVA: 0x002B54A8 File Offset: 0x002B36A8
		public override void StartSM()
		{
			base.StartSM();
			this.luminescence = Db.Get().Attributes.Luminescence.Lookup(base.gameObject);
			AttributeInstance attributeInstance = this.luminescence;
			attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, new System.Action(this.OnLuminescenceChanged));
			this.RefreshLight();
		}

		// Token: 0x06006221 RID: 25121 RVA: 0x000E0033 File Offset: 0x000DE233
		private void OnLuminescenceChanged()
		{
			this.RefreshLight();
		}

		// Token: 0x06006222 RID: 25122 RVA: 0x002B5508 File Offset: 0x002B3708
		public void RefreshLight()
		{
			if (this.luminescence != null)
			{
				int num = (int)this.luminescence.GetTotalValue();
				this.light.Lux = num;
				bool flag = num > 0;
				if (this.light.enabled != flag)
				{
					this.light.enabled = flag;
				}
			}
		}

		// Token: 0x06006223 RID: 25123 RVA: 0x000E003B File Offset: 0x000DE23B
		protected override void OnCleanUp()
		{
			if (this.luminescence != null)
			{
				AttributeInstance attributeInstance = this.luminescence;
				attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, new System.Action(this.OnLuminescenceChanged));
			}
			base.OnCleanUp();
		}

		// Token: 0x040045D6 RID: 17878
		[MyCmpAdd]
		private Light2D light;

		// Token: 0x040045D7 RID: 17879
		private AttributeInstance luminescence;
	}
}
