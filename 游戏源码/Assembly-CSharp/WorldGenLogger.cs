using System;

// Token: 0x02002079 RID: 8313
public static class WorldGenLogger
{
	// Token: 0x0600B0DB RID: 45275 RVA: 0x00112F16 File Offset: 0x00111116
	public static void LogException(string message, string stack)
	{
		Debug.LogError(message + "\n" + stack);
	}
}
