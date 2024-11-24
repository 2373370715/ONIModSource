using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B7F RID: 15231
	public class ModifierInstance<ModifierType> : IStateMachineTarget
	{
		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x0600EA82 RID: 60034 RVA: 0x0013CC30 File Offset: 0x0013AE30
		// (set) Token: 0x0600EA83 RID: 60035 RVA: 0x0013CC38 File Offset: 0x0013AE38
		public GameObject gameObject { get; private set; }

		// Token: 0x0600EA84 RID: 60036 RVA: 0x0013CC41 File Offset: 0x0013AE41
		public ModifierInstance(GameObject game_object, ModifierType modifier)
		{
			this.gameObject = game_object;
			this.modifier = modifier;
		}

		// Token: 0x0600EA85 RID: 60037 RVA: 0x0013CC57 File Offset: 0x0013AE57
		public ComponentType GetComponent<ComponentType>()
		{
			return this.gameObject.GetComponent<ComponentType>();
		}

		// Token: 0x0600EA86 RID: 60038 RVA: 0x0013CC64 File Offset: 0x0013AE64
		public int Subscribe(int hash, Action<object> handler)
		{
			return this.gameObject.GetComponent<KMonoBehaviour>().Subscribe(hash, handler);
		}

		// Token: 0x0600EA87 RID: 60039 RVA: 0x0013CC78 File Offset: 0x0013AE78
		public void Unsubscribe(int hash, Action<object> handler)
		{
			this.gameObject.GetComponent<KMonoBehaviour>().Unsubscribe(hash, handler);
		}

		// Token: 0x0600EA88 RID: 60040 RVA: 0x0013CC8C File Offset: 0x0013AE8C
		public void Unsubscribe(int id)
		{
			this.gameObject.GetComponent<KMonoBehaviour>().Unsubscribe(id);
		}

		// Token: 0x0600EA89 RID: 60041 RVA: 0x0013CC9F File Offset: 0x0013AE9F
		public void Trigger(int hash, object data = null)
		{
			this.gameObject.GetComponent<KPrefabID>().Trigger(hash, data);
		}

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x0600EA8A RID: 60042 RVA: 0x0013CCB3 File Offset: 0x0013AEB3
		public Transform transform
		{
			get
			{
				return this.gameObject.transform;
			}
		}

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x0600EA8B RID: 60043 RVA: 0x0013CCC0 File Offset: 0x0013AEC0
		public bool isNull
		{
			get
			{
				return this.gameObject == null;
			}
		}

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x0600EA8C RID: 60044 RVA: 0x0013CCCE File Offset: 0x0013AECE
		public string name
		{
			get
			{
				return this.gameObject.name;
			}
		}

		// Token: 0x0600EA8D RID: 60045 RVA: 0x000A5E40 File Offset: 0x000A4040
		public virtual void OnCleanUp()
		{
		}

		// Token: 0x0400E5CA RID: 58826
		public ModifierType modifier;
	}
}
