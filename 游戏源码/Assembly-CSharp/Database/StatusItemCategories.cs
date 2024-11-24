using System;

namespace Database
{
	// Token: 0x0200216B RID: 8555
	public class StatusItemCategories : ResourceSet<StatusItemCategory>
	{
		// Token: 0x0600B604 RID: 46596 RVA: 0x00456694 File Offset: 0x00454894
		public StatusItemCategories(ResourceSet parent) : base("StatusItemCategories", parent)
		{
			this.Main = new StatusItemCategory("Main", this, "Main");
			this.Role = new StatusItemCategory("Role", this, "Role");
			this.Power = new StatusItemCategory("Power", this, "Power");
			this.Toilet = new StatusItemCategory("Toilet", this, "Toilet");
			this.Research = new StatusItemCategory("Research", this, "Research");
			this.Hitpoints = new StatusItemCategory("Hitpoints", this, "Hitpoints");
			this.Suffocation = new StatusItemCategory("Suffocation", this, "Suffocation");
			this.WoundEffects = new StatusItemCategory("WoundEffects", this, "WoundEffects");
			this.EntityReceptacle = new StatusItemCategory("EntityReceptacle", this, "EntityReceptacle");
			this.PreservationState = new StatusItemCategory("PreservationState", this, "PreservationState");
			this.PreservationTemperature = new StatusItemCategory("PreservationTemperature", this, "PreservationTemperature");
			this.PreservationAtmosphere = new StatusItemCategory("PreservationAtmosphere", this, "PreservationAtmosphere");
			this.ExhaustTemperature = new StatusItemCategory("ExhaustTemperature", this, "ExhaustTemperature");
			this.OperatingEnergy = new StatusItemCategory("OperatingEnergy", this, "OperatingEnergy");
			this.AccessControl = new StatusItemCategory("AccessControl", this, "AccessControl");
			this.RequiredRoom = new StatusItemCategory("RequiredRoom", this, "RequiredRoom");
			this.Yield = new StatusItemCategory("Yield", this, "Yield");
			this.Heat = new StatusItemCategory("Heat", this, "Heat");
			this.Stored = new StatusItemCategory("Stored", this, "Stored");
			this.Ownable = new StatusItemCategory("Ownable", this, "Ownable");
		}

		// Token: 0x04009460 RID: 37984
		public StatusItemCategory Main;

		// Token: 0x04009461 RID: 37985
		public StatusItemCategory Role;

		// Token: 0x04009462 RID: 37986
		public StatusItemCategory Power;

		// Token: 0x04009463 RID: 37987
		public StatusItemCategory Toilet;

		// Token: 0x04009464 RID: 37988
		public StatusItemCategory Research;

		// Token: 0x04009465 RID: 37989
		public StatusItemCategory Hitpoints;

		// Token: 0x04009466 RID: 37990
		public StatusItemCategory Suffocation;

		// Token: 0x04009467 RID: 37991
		public StatusItemCategory WoundEffects;

		// Token: 0x04009468 RID: 37992
		public StatusItemCategory EntityReceptacle;

		// Token: 0x04009469 RID: 37993
		public StatusItemCategory PreservationState;

		// Token: 0x0400946A RID: 37994
		public StatusItemCategory PreservationAtmosphere;

		// Token: 0x0400946B RID: 37995
		public StatusItemCategory PreservationTemperature;

		// Token: 0x0400946C RID: 37996
		public StatusItemCategory ExhaustTemperature;

		// Token: 0x0400946D RID: 37997
		public StatusItemCategory OperatingEnergy;

		// Token: 0x0400946E RID: 37998
		public StatusItemCategory AccessControl;

		// Token: 0x0400946F RID: 37999
		public StatusItemCategory RequiredRoom;

		// Token: 0x04009470 RID: 38000
		public StatusItemCategory Yield;

		// Token: 0x04009471 RID: 38001
		public StatusItemCategory Heat;

		// Token: 0x04009472 RID: 38002
		public StatusItemCategory Stored;

		// Token: 0x04009473 RID: 38003
		public StatusItemCategory Ownable;
	}
}
