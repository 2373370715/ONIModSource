using System;
using Klei.AI;
using UnityEngine;

public class EntityLuminescence : GameStateMachine<EntityLuminescence, EntityLuminescence.Instance, IStateMachineTarget, EntityLuminescence.Def>
{
	public class Def : BaseDef
	{
		public Color lightColor;

		public float lightRange;

		public float lightAngle;

		public Vector2 lightOffset;

		public Vector2 lightDirection;

		public LightShape lightShape;
	}

	public new class Instance : GameInstance
	{
		[MyCmpAdd]
		private Light2D light;

		private AttributeInstance luminescence;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			light.Color = def.lightColor;
			light.Range = def.lightRange;
			light.Angle = def.lightAngle;
			light.Direction = def.lightDirection;
			light.Offset = def.lightOffset;
			light.shape = def.lightShape;
		}

		public override void StartSM()
		{
			base.StartSM();
			luminescence = Db.Get().Attributes.Luminescence.Lookup(base.gameObject);
			AttributeInstance attributeInstance = luminescence;
			attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, new System.Action(OnLuminescenceChanged));
			RefreshLight();
		}

		private void OnLuminescenceChanged()
		{
			RefreshLight();
		}

		public void RefreshLight()
		{
			if (luminescence != null)
			{
				int num = (int)luminescence.GetTotalValue();
				light.Lux = num;
				bool flag = num > 0;
				if (light.enabled != flag)
				{
					light.enabled = flag;
				}
			}
		}

		protected override void OnCleanUp()
		{
			if (luminescence != null)
			{
				AttributeInstance attributeInstance = luminescence;
				attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, new System.Action(OnLuminescenceChanged));
			}
			base.OnCleanUp();
		}
	}

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = root;
	}
}
