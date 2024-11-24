using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02002136 RID: 8502
	public class Emotes : ResourceSet<Resource>
	{
		// Token: 0x0600B53A RID: 46394 RVA: 0x00114F93 File Offset: 0x00113193
		public Emotes(ResourceSet parent) : base("Emotes", parent)
		{
			this.Minion = new Emotes.MinionEmotes(this);
			this.Critter = new Emotes.CritterEmotes(this);
		}

		// Token: 0x0600B53B RID: 46395 RVA: 0x0044DFDC File Offset: 0x0044C1DC
		public void ResetProblematicReferences()
		{
			for (int i = 0; i < this.Minion.resources.Count; i++)
			{
				Emote emote = this.Minion.resources[i];
				for (int j = 0; j < emote.StepCount; j++)
				{
					emote[j].UnregisterAllCallbacks();
				}
			}
			for (int k = 0; k < this.Critter.resources.Count; k++)
			{
				Emote emote2 = this.Critter.resources[k];
				for (int l = 0; l < emote2.StepCount; l++)
				{
					emote2[l].UnregisterAllCallbacks();
				}
			}
		}

		// Token: 0x04009255 RID: 37461
		public Emotes.MinionEmotes Minion;

		// Token: 0x04009256 RID: 37462
		public Emotes.CritterEmotes Critter;

		// Token: 0x02002137 RID: 8503
		public class MinionEmotes : ResourceSet<Emote>
		{
			// Token: 0x0600B53C RID: 46396 RVA: 0x00114FB9 File Offset: 0x001131B9
			public MinionEmotes(ResourceSet parent) : base("Minion", parent)
			{
				this.InitializeCelebrations();
				this.InitializePhysicalStatus();
				this.InitializeEmotionalStatus();
				this.InitializeGreetings();
			}

			// Token: 0x0600B53D RID: 46397 RVA: 0x0044E088 File Offset: 0x0044C288
			public void InitializeCelebrations()
			{
				this.ClapCheer = new Emote(this, "ClapCheer", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "clapcheer_pre"
					},
					new EmoteStep
					{
						anim = "clapcheer_loop"
					},
					new EmoteStep
					{
						anim = "clapcheer_pst"
					}
				}, "anim_clapcheer_kanim");
				this.Cheer = new Emote(this, "Cheer", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "cheer_pre"
					},
					new EmoteStep
					{
						anim = "cheer_loop"
					},
					new EmoteStep
					{
						anim = "cheer_pst"
					}
				}, "anim_cheer_kanim");
				this.ProductiveCheer = new Emote(this, "Productive Cheer", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "productive"
					}
				}, "anim_productive_kanim");
				this.ResearchComplete = new Emote(this, "ResearchComplete", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_research_complete_kanim");
				this.ThumbsUp = new Emote(this, "ThumbsUp", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_thumbsup_kanim");
			}

			// Token: 0x0600B53E RID: 46398 RVA: 0x0044E1C8 File Offset: 0x0044C3C8
			private void InitializePhysicalStatus()
			{
				this.CloseCall_Fall = new Emote(this, "Near Fall", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_floor_missing_kanim");
				this.Cold = new Emote(this, "Cold", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_cold_kanim");
				this.Cough = new Emote(this, "Cough", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_slimelungcough_kanim");
				this.Cough_Small = new Emote(this, "Small Cough", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_small"
					}
				}, "anim_slimelungcough_kanim");
				this.FoodPoisoning = new Emote(this, "Food Poisoning", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_contaminated_food_kanim");
				this.Hot = new Emote(this, "Hot", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_hot_kanim");
				this.IritatedEyes = new Emote(this, "Irritated Eyes", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "irritated_eyes"
					}
				}, "anim_irritated_eyes_kanim");
				this.MorningStretch = new Emote(this, "Morning Stretch", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_morning_stretch_kanim");
				this.Radiation_Glare = new Emote(this, "Radiation Glare", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_radiation_glare"
					}
				}, "anim_react_radiation_kanim");
				this.Radiation_Itch = new Emote(this, "Radiation Itch", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_radiation_itch"
					}
				}, "anim_react_radiation_kanim");
				this.Sick = new Emote(this, "Sick", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_sick_kanim");
				this.Sneeze = new Emote(this, "Sneeze", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "sneeze"
					},
					new EmoteStep
					{
						anim = "sneeze_pst"
					}
				}, "anim_sneeze_kanim");
				this.WaterDamage = new Emote(this, "WaterDamage", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "zapped"
					}
				}, "anim_bionic_kanim");
				this.Sneeze_Short = new Emote(this, "Short Sneeze", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "sneeze_short"
					},
					new EmoteStep
					{
						anim = "sneeze_short_pst"
					}
				}, "anim_sneeze_kanim");
			}

			// Token: 0x0600B53F RID: 46399 RVA: 0x0044E430 File Offset: 0x0044C630
			private void InitializeEmotionalStatus()
			{
				this.Concern = new Emote(this, "Concern", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_concern_kanim");
				this.Cringe = new Emote(this, "Cringe", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "cringe_pre"
					},
					new EmoteStep
					{
						anim = "cringe_loop"
					},
					new EmoteStep
					{
						anim = "cringe_pst"
					}
				}, "anim_cringe_kanim");
				this.Disappointed = new Emote(this, "Disappointed", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_disappointed_kanim");
				this.Shock = new Emote(this, "Shock", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_shock_kanim");
				this.Sing = new Emote(this, "Sing", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_singer_kanim");
			}

			// Token: 0x0600B540 RID: 46400 RVA: 0x0044E510 File Offset: 0x0044C710
			private void InitializeGreetings()
			{
				this.FingerGuns = new Emote(this, "Finger Guns", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_fingerguns_kanim");
				this.Wave = new Emote(this, "Wave", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_wave_kanim");
				this.Wave_Shy = new Emote(this, "Shy Wave", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_wave_shy_kanim");
			}

			// Token: 0x04009257 RID: 37463
			private static EmoteStep[] DEFAULT_STEPS = new EmoteStep[]
			{
				new EmoteStep
				{
					anim = "react"
				}
			};

			// Token: 0x04009258 RID: 37464
			private static EmoteStep[] DEFAULT_IDLE_STEPS = new EmoteStep[]
			{
				new EmoteStep
				{
					anim = "idle_pre"
				},
				new EmoteStep
				{
					anim = "idle_default"
				},
				new EmoteStep
				{
					anim = "idle_pst"
				}
			};

			// Token: 0x04009259 RID: 37465
			public Emote ClapCheer;

			// Token: 0x0400925A RID: 37466
			public Emote Cheer;

			// Token: 0x0400925B RID: 37467
			public Emote ProductiveCheer;

			// Token: 0x0400925C RID: 37468
			public Emote ResearchComplete;

			// Token: 0x0400925D RID: 37469
			public Emote ThumbsUp;

			// Token: 0x0400925E RID: 37470
			public Emote CloseCall_Fall;

			// Token: 0x0400925F RID: 37471
			public Emote Cold;

			// Token: 0x04009260 RID: 37472
			public Emote Cough;

			// Token: 0x04009261 RID: 37473
			public Emote Cough_Small;

			// Token: 0x04009262 RID: 37474
			public Emote FoodPoisoning;

			// Token: 0x04009263 RID: 37475
			public Emote Hot;

			// Token: 0x04009264 RID: 37476
			public Emote IritatedEyes;

			// Token: 0x04009265 RID: 37477
			public Emote MorningStretch;

			// Token: 0x04009266 RID: 37478
			public Emote Radiation_Glare;

			// Token: 0x04009267 RID: 37479
			public Emote Radiation_Itch;

			// Token: 0x04009268 RID: 37480
			public Emote Sick;

			// Token: 0x04009269 RID: 37481
			public Emote Sneeze;

			// Token: 0x0400926A RID: 37482
			public Emote WaterDamage;

			// Token: 0x0400926B RID: 37483
			public Emote Sneeze_Short;

			// Token: 0x0400926C RID: 37484
			public Emote Concern;

			// Token: 0x0400926D RID: 37485
			public Emote Cringe;

			// Token: 0x0400926E RID: 37486
			public Emote Disappointed;

			// Token: 0x0400926F RID: 37487
			public Emote Shock;

			// Token: 0x04009270 RID: 37488
			public Emote Sing;

			// Token: 0x04009271 RID: 37489
			public Emote FingerGuns;

			// Token: 0x04009272 RID: 37490
			public Emote Wave;

			// Token: 0x04009273 RID: 37491
			public Emote Wave_Shy;
		}

		// Token: 0x02002138 RID: 8504
		public class CritterEmotes : ResourceSet<Emote>
		{
			// Token: 0x0600B542 RID: 46402 RVA: 0x00114FDF File Offset: 0x001131DF
			public CritterEmotes(ResourceSet parent) : base("Critter", parent)
			{
				this.InitializePhysicalState();
				this.InitializeEmotionalState();
			}

			// Token: 0x0600B543 RID: 46403 RVA: 0x0044E5F4 File Offset: 0x0044C7F4
			private void InitializePhysicalState()
			{
				this.Hungry = new Emote(this, "Hungry", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_hungry"
					}
				}, null);
			}

			// Token: 0x0600B544 RID: 46404 RVA: 0x0044E634 File Offset: 0x0044C834
			private void InitializeEmotionalState()
			{
				this.Angry = new Emote(this, "Angry", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_angry"
					}
				}, null);
				this.Happy = new Emote(this, "Happy", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_happy"
					}
				}, null);
				this.Idle = new Emote(this, "Idle", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_idle"
					}
				}, null);
				this.Sad = new Emote(this, "Sad", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_sad"
					}
				}, null);
			}

			// Token: 0x04009274 RID: 37492
			public Emote Hungry;

			// Token: 0x04009275 RID: 37493
			public Emote Angry;

			// Token: 0x04009276 RID: 37494
			public Emote Happy;

			// Token: 0x04009277 RID: 37495
			public Emote Idle;

			// Token: 0x04009278 RID: 37496
			public Emote Sad;
		}
	}
}
