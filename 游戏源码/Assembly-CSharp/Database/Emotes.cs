using Klei.AI;

namespace Database;

public class Emotes : ResourceSet<Resource>
{
	public class MinionEmotes : ResourceSet<Emote>
	{
		private static EmoteStep[] DEFAULT_STEPS = new EmoteStep[1]
		{
			new EmoteStep
			{
				anim = "react"
			}
		};

		private static EmoteStep[] DEFAULT_IDLE_STEPS = new EmoteStep[3]
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

		public Emote ClapCheer;

		public Emote Cheer;

		public Emote ProductiveCheer;

		public Emote ResearchComplete;

		public Emote ThumbsUp;

		public Emote CloseCall_Fall;

		public Emote Cold;

		public Emote Cough;

		public Emote Cough_Small;

		public Emote FoodPoisoning;

		public Emote Hot;

		public Emote IritatedEyes;

		public Emote MorningStretch;

		public Emote Radiation_Glare;

		public Emote Radiation_Itch;

		public Emote Sick;

		public Emote Sneeze;

		public Emote Sneeze_Short;

		public Emote Concern;

		public Emote Cringe;

		public Emote Disappointed;

		public Emote Shock;

		public Emote Sing;

		public Emote FingerGuns;

		public Emote Wave;

		public Emote Wave_Shy;

		public MinionEmotes(ResourceSet parent)
			: base("Minion", parent)
		{
			InitializeCelebrations();
			InitializePhysicalStatus();
			InitializeEmotionalStatus();
			InitializeGreetings();
		}

		public void InitializeCelebrations()
		{
			ClapCheer = new Emote(this, "ClapCheer", new EmoteStep[3]
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
			Cheer = new Emote(this, "Cheer", new EmoteStep[3]
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
			ProductiveCheer = new Emote(this, "Productive Cheer", new EmoteStep[1]
			{
				new EmoteStep
				{
					anim = "productive"
				}
			}, "anim_productive_kanim");
			ResearchComplete = new Emote(this, "ResearchComplete", DEFAULT_STEPS, "anim_react_research_complete_kanim");
			ThumbsUp = new Emote(this, "ThumbsUp", DEFAULT_STEPS, "anim_react_thumbsup_kanim");
		}

		private void InitializePhysicalStatus()
		{
			CloseCall_Fall = new Emote(this, "Near Fall", DEFAULT_STEPS, "anim_react_floor_missing_kanim");
			Cold = new Emote(this, "Cold", DEFAULT_IDLE_STEPS, "anim_idle_cold_kanim");
			Cough = new Emote(this, "Cough", DEFAULT_STEPS, "anim_slimelungcough_kanim");
			Cough_Small = new Emote(this, "Small Cough", new EmoteStep[1]
			{
				new EmoteStep
				{
					anim = "react_small"
				}
			}, "anim_slimelungcough_kanim");
			FoodPoisoning = new Emote(this, "Food Poisoning", DEFAULT_STEPS, "anim_react_contaminated_food_kanim");
			Hot = new Emote(this, "Hot", DEFAULT_IDLE_STEPS, "anim_idle_hot_kanim");
			IritatedEyes = new Emote(this, "Irritated Eyes", new EmoteStep[1]
			{
				new EmoteStep
				{
					anim = "irritated_eyes"
				}
			}, "anim_irritated_eyes_kanim");
			MorningStretch = new Emote(this, "Morning Stretch", DEFAULT_STEPS, "anim_react_morning_stretch_kanim");
			Radiation_Glare = new Emote(this, "Radiation Glare", new EmoteStep[1]
			{
				new EmoteStep
				{
					anim = "react_radiation_glare"
				}
			}, "anim_react_radiation_kanim");
			Radiation_Itch = new Emote(this, "Radiation Itch", new EmoteStep[1]
			{
				new EmoteStep
				{
					anim = "react_radiation_itch"
				}
			}, "anim_react_radiation_kanim");
			Sick = new Emote(this, "Sick", DEFAULT_IDLE_STEPS, "anim_idle_sick_kanim");
			Sneeze = new Emote(this, "Sneeze", new EmoteStep[2]
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
			Sneeze_Short = new Emote(this, "Short Sneeze", new EmoteStep[2]
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

		private void InitializeEmotionalStatus()
		{
			Concern = new Emote(this, "Concern", DEFAULT_STEPS, "anim_react_concern_kanim");
			Cringe = new Emote(this, "Cringe", new EmoteStep[3]
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
			Disappointed = new Emote(this, "Disappointed", DEFAULT_STEPS, "anim_disappointed_kanim");
			Shock = new Emote(this, "Shock", DEFAULT_STEPS, "anim_react_shock_kanim");
			Sing = new Emote(this, "Sing", DEFAULT_STEPS, "anim_react_singer_kanim");
		}

		private void InitializeGreetings()
		{
			FingerGuns = new Emote(this, "Finger Guns", DEFAULT_STEPS, "anim_react_fingerguns_kanim");
			Wave = new Emote(this, "Wave", DEFAULT_STEPS, "anim_react_wave_kanim");
			Wave_Shy = new Emote(this, "Shy Wave", DEFAULT_STEPS, "anim_react_wave_shy_kanim");
		}
	}

	public class CritterEmotes : ResourceSet<Emote>
	{
		public Emote Hungry;

		public Emote Angry;

		public Emote Happy;

		public Emote Idle;

		public Emote Sad;

		public CritterEmotes(ResourceSet parent)
			: base("Critter", parent)
		{
			InitializePhysicalState();
			InitializeEmotionalState();
		}

		private void InitializePhysicalState()
		{
			Hungry = new Emote(this, "Hungry", new EmoteStep[1]
			{
				new EmoteStep
				{
					anim = "react_hungry"
				}
			});
		}

		private void InitializeEmotionalState()
		{
			Angry = new Emote(this, "Angry", new EmoteStep[1]
			{
				new EmoteStep
				{
					anim = "react_angry"
				}
			});
			Happy = new Emote(this, "Happy", new EmoteStep[1]
			{
				new EmoteStep
				{
					anim = "react_happy"
				}
			});
			Idle = new Emote(this, "Idle", new EmoteStep[1]
			{
				new EmoteStep
				{
					anim = "react_idle"
				}
			});
			Sad = new Emote(this, "Sad", new EmoteStep[1]
			{
				new EmoteStep
				{
					anim = "react_sad"
				}
			});
		}
	}

	public MinionEmotes Minion;

	public CritterEmotes Critter;

	public Emotes(ResourceSet parent)
		: base("Emotes", parent)
	{
		Minion = new MinionEmotes(this);
		Critter = new CritterEmotes(this);
	}

	public void ResetProblematicReferences()
	{
		for (int i = 0; i < Minion.resources.Count; i++)
		{
			Emote emote = Minion.resources[i];
			for (int j = 0; j < emote.StepCount; j++)
			{
				emote[j].UnregisterAllCallbacks();
			}
		}
		for (int k = 0; k < Critter.resources.Count; k++)
		{
			Emote emote2 = Critter.resources[k];
			for (int l = 0; l < emote2.StepCount; l++)
			{
				emote2[l].UnregisterAllCallbacks();
			}
		}
	}
}
