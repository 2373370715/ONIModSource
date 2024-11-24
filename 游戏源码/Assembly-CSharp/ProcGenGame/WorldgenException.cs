using System;

namespace ProcGenGame;

public class WorldgenException : Exception
{
	public readonly string userMessage;

	public WorldgenException(string message, string userMessage)
		: base(message)
	{
		this.userMessage = userMessage;
	}
}
