using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B07 RID: 15111
	public class AttributeLevel
	{
		// Token: 0x0600E879 RID: 59513 RVA: 0x004C118C File Offset: 0x004BF38C
		public AttributeLevel(AttributeInstance attribute)
		{
			this.notification = new Notification(MISC.NOTIFICATIONS.LEVELUP.NAME, NotificationType.Good, new Func<List<Notification>, object, string>(AttributeLevel.OnLevelUpTooltip), null, true, 0f, null, null, null, true, false, false);
			this.attribute = attribute;
		}

		// Token: 0x0600E87A RID: 59514 RVA: 0x0013B860 File Offset: 0x00139A60
		public int GetLevel()
		{
			return this.level;
		}

		// Token: 0x0600E87B RID: 59515 RVA: 0x004C11D8 File Offset: 0x004BF3D8
		public void Apply(AttributeLevels levels)
		{
			Attributes attributes = levels.GetAttributes();
			if (this.modifier != null)
			{
				attributes.Remove(this.modifier);
				this.modifier = null;
			}
			this.modifier = new AttributeModifier(this.attribute.Id, (float)this.GetLevel(), DUPLICANTS.MODIFIERS.SKILLLEVEL.NAME, false, false, true);
			attributes.Add(this.modifier);
		}

		// Token: 0x0600E87C RID: 59516 RVA: 0x0013B868 File Offset: 0x00139A68
		public void SetExperience(float experience)
		{
			this.experience = experience;
		}

		// Token: 0x0600E87D RID: 59517 RVA: 0x0013B871 File Offset: 0x00139A71
		public void SetLevel(int level)
		{
			this.level = level;
		}

		// Token: 0x0600E87E RID: 59518 RVA: 0x004C1240 File Offset: 0x004BF440
		public float GetExperienceForNextLevel()
		{
			float num = Mathf.Pow((float)this.level / (float)DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL, DUPLICANTSTATS.ATTRIBUTE_LEVELING.EXPERIENCE_LEVEL_POWER) * (float)DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE * 600f;
			return Mathf.Pow(((float)this.level + 1f) / (float)DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL, DUPLICANTSTATS.ATTRIBUTE_LEVELING.EXPERIENCE_LEVEL_POWER) * (float)DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE * 600f - num;
		}

		// Token: 0x0600E87F RID: 59519 RVA: 0x0013B87A File Offset: 0x00139A7A
		public float GetPercentComplete()
		{
			return this.experience / this.GetExperienceForNextLevel();
		}

		// Token: 0x0600E880 RID: 59520 RVA: 0x004C12A0 File Offset: 0x004BF4A0
		public void LevelUp(AttributeLevels levels)
		{
			this.level++;
			this.experience = 0f;
			this.Apply(levels);
			this.experience = 0f;
			if (PopFXManager.Instance != null)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, this.attribute.modifier.Name, levels.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
			}
			levels.GetComponent<Notifier>().Add(this.notification, string.Format(MISC.NOTIFICATIONS.LEVELUP.SUFFIX, this.attribute.modifier.Name, this.level));
			StateMachine.Instance instance = new UpgradeFX.Instance(levels.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, -0.1f));
			ReportManager.Instance.ReportValue(ReportManager.ReportType.LevelUp, 1f, levels.GetProperName(), null);
			instance.StartSM();
			levels.Trigger(-110704193, this.attribute.Id);
		}

		// Token: 0x0600E881 RID: 59521 RVA: 0x004C13B8 File Offset: 0x004BF5B8
		public bool AddExperience(AttributeLevels levels, float experience)
		{
			if (this.level >= DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL)
			{
				return false;
			}
			this.experience += experience;
			this.experience = Mathf.Max(0f, this.experience);
			if (this.experience >= this.GetExperienceForNextLevel())
			{
				this.LevelUp(levels);
				return true;
			}
			return false;
		}

		// Token: 0x0600E882 RID: 59522 RVA: 0x0013B889 File Offset: 0x00139A89
		private static string OnLevelUpTooltip(List<Notification> notifications, object data)
		{
			return MISC.NOTIFICATIONS.LEVELUP.TOOLTIP + notifications.ReduceMessages(false);
		}

		// Token: 0x0400E43F RID: 58431
		public float experience;

		// Token: 0x0400E440 RID: 58432
		public int level;

		// Token: 0x0400E441 RID: 58433
		public AttributeInstance attribute;

		// Token: 0x0400E442 RID: 58434
		public AttributeModifier modifier;

		// Token: 0x0400E443 RID: 58435
		public Notification notification;
	}
}
