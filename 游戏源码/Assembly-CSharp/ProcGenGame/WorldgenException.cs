using System;

namespace ProcGenGame
{
	// Token: 0x020020A8 RID: 8360
	public class WorldgenException : Exception
	{
		// Token: 0x0600B1B1 RID: 45489 RVA: 0x0011373E File Offset: 0x0011193E
		public WorldgenException(string message, string userMessage) : base(message)
		{
			this.userMessage = userMessage;
		}

		// Token: 0x04008C6A RID: 35946
		public readonly string userMessage;
	}
}
