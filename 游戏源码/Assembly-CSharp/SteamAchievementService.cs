using System;
using System.Diagnostics;
using Steamworks;
using UnityEngine;

// Token: 0x02001A6B RID: 6763
public class SteamAchievementService : MonoBehaviour
{
	// Token: 0x1700095D RID: 2397
	// (get) Token: 0x06008D6E RID: 36206 RVA: 0x000FC59A File Offset: 0x000FA79A
	public static SteamAchievementService Instance
	{
		get
		{
			return SteamAchievementService.instance;
		}
	}

	// Token: 0x06008D6F RID: 36207 RVA: 0x0036B4F8 File Offset: 0x003696F8
	public static void Initialize()
	{
		if (SteamAchievementService.instance == null)
		{
			GameObject gameObject = GameObject.Find("/SteamManager");
			SteamAchievementService.instance = gameObject.GetComponent<SteamAchievementService>();
			if (SteamAchievementService.instance == null)
			{
				SteamAchievementService.instance = gameObject.AddComponent<SteamAchievementService>();
			}
		}
	}

	// Token: 0x06008D70 RID: 36208 RVA: 0x000FC5A1 File Offset: 0x000FA7A1
	public void Awake()
	{
		this.setupComplete = false;
		global::Debug.Assert(SteamAchievementService.instance == null);
		SteamAchievementService.instance = this;
	}

	// Token: 0x06008D71 RID: 36209 RVA: 0x000FC5C0 File Offset: 0x000FA7C0
	private void OnDestroy()
	{
		global::Debug.Assert(SteamAchievementService.instance == this);
		SteamAchievementService.instance = null;
	}

	// Token: 0x06008D72 RID: 36210 RVA: 0x000FC5D8 File Offset: 0x000FA7D8
	private void Update()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (Game.Instance != null)
		{
			return;
		}
		if (!this.setupComplete && DistributionPlatform.Initialized)
		{
			this.Setup();
		}
	}

	// Token: 0x06008D73 RID: 36211 RVA: 0x0036B540 File Offset: 0x00369740
	private void Setup()
	{
		this.cbUserStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnUserStatsReceived));
		this.cbUserStatsStored = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.OnUserStatsStored));
		this.cbUserAchievementStored = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.OnUserAchievementStored));
		this.setupComplete = true;
		this.RefreshStats();
	}

	// Token: 0x06008D74 RID: 36212 RVA: 0x000FC605 File Offset: 0x000FA805
	private void RefreshStats()
	{
		SteamUserStats.RequestCurrentStats();
	}

	// Token: 0x06008D75 RID: 36213 RVA: 0x000FC60D File Offset: 0x000FA80D
	private void OnUserStatsReceived(UserStatsReceived_t data)
	{
		if (data.m_eResult != EResult.k_EResultOK)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"OnUserStatsReceived",
				data.m_eResult,
				data.m_steamIDUser
			});
			return;
		}
	}

	// Token: 0x06008D76 RID: 36214 RVA: 0x000FC648 File Offset: 0x000FA848
	private void OnUserStatsStored(UserStatsStored_t data)
	{
		if (data.m_eResult != EResult.k_EResultOK)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"OnUserStatsStored",
				data.m_eResult
			});
			return;
		}
	}

	// Token: 0x06008D77 RID: 36215 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void OnUserAchievementStored(UserAchievementStored_t data)
	{
	}

	// Token: 0x06008D78 RID: 36216 RVA: 0x0036B5A0 File Offset: 0x003697A0
	public void Unlock(string achievement_id)
	{
		bool flag = SteamUserStats.SetAchievement(achievement_id);
		global::Debug.LogFormat("SetAchievement {0} {1}", new object[]
		{
			achievement_id,
			flag
		});
		bool flag2 = SteamUserStats.StoreStats();
		global::Debug.LogFormat("StoreStats {0}", new object[]
		{
			flag2
		});
	}

	// Token: 0x06008D79 RID: 36217 RVA: 0x0036B5F0 File Offset: 0x003697F0
	[Conditional("UNITY_EDITOR")]
	[ContextMenu("Reset All Achievements")]
	private void ResetAllAchievements()
	{
		bool flag = SteamUserStats.ResetAllStats(true);
		global::Debug.LogFormat("ResetAllStats {0}", new object[]
		{
			flag
		});
		if (flag)
		{
			this.RefreshStats();
		}
	}

	// Token: 0x04006A45 RID: 27205
	private Callback<UserStatsReceived_t> cbUserStatsReceived;

	// Token: 0x04006A46 RID: 27206
	private Callback<UserStatsStored_t> cbUserStatsStored;

	// Token: 0x04006A47 RID: 27207
	private Callback<UserAchievementStored_t> cbUserAchievementStored;

	// Token: 0x04006A48 RID: 27208
	private bool setupComplete;

	// Token: 0x04006A49 RID: 27209
	private static SteamAchievementService instance;
}
