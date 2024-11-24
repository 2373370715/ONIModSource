using System;
using System.Collections.Generic;
using UnityEngine;

namespace KMod
{
	// Token: 0x020021EE RID: 8686
	public class ModErrorsScreen : KScreen
	{
		// Token: 0x0600B843 RID: 47171 RVA: 0x0046528C File Offset: 0x0046348C
		public static bool ShowErrors(List<Event> events)
		{
			if (Global.Instance.modManager.events.Count == 0)
			{
				return false;
			}
			GameObject parent = GameObject.Find("Canvas");
			ModErrorsScreen modErrorsScreen = Util.KInstantiateUI<ModErrorsScreen>(Global.Instance.modErrorsPrefab, parent, false);
			modErrorsScreen.Initialize(events);
			modErrorsScreen.gameObject.SetActive(true);
			return true;
		}

		// Token: 0x0600B844 RID: 47172 RVA: 0x004652E0 File Offset: 0x004634E0
		private void Initialize(List<Event> events)
		{
			foreach (Event @event in events)
			{
				HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.entryPrefab, this.entryParent.gameObject, true);
				LocText reference = hierarchyReferences.GetReference<LocText>("Title");
				LocText reference2 = hierarchyReferences.GetReference<LocText>("Description");
				KButton reference3 = hierarchyReferences.GetReference<KButton>("Details");
				string text;
				string toolTip;
				Event.GetUIStrings(@event.event_type, out text, out toolTip);
				reference.text = text;
				reference.GetComponent<ToolTip>().toolTip = toolTip;
				reference2.text = @event.mod.title;
				ToolTip component = reference2.GetComponent<ToolTip>();
				if (component != null)
				{
					ToolTip toolTip2 = component;
					Label mod = @event.mod;
					toolTip2.toolTip = mod.ToString();
				}
				reference3.isInteractable = false;
				Mod mod2 = Global.Instance.modManager.FindMod(@event.mod);
				if (mod2 != null)
				{
					if (component != null && !string.IsNullOrEmpty(mod2.description))
					{
						component.toolTip = mod2.description;
					}
					if (mod2.on_managed != null)
					{
						reference3.onClick += mod2.on_managed;
						reference3.isInteractable = true;
					}
				}
			}
		}

		// Token: 0x0600B845 RID: 47173 RVA: 0x001169E5 File Offset: 0x00114BE5
		protected override void OnActivate()
		{
			base.OnActivate();
			this.closeButtonTitle.onClick += this.Deactivate;
			this.closeButton.onClick += this.Deactivate;
		}

		// Token: 0x04009693 RID: 38547
		[SerializeField]
		private KButton closeButtonTitle;

		// Token: 0x04009694 RID: 38548
		[SerializeField]
		private KButton closeButton;

		// Token: 0x04009695 RID: 38549
		[SerializeField]
		private GameObject entryPrefab;

		// Token: 0x04009696 RID: 38550
		[SerializeField]
		private Transform entryParent;
	}
}
