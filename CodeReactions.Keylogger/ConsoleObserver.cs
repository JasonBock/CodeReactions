﻿using System;
namespace CodeReactions.Keylogger
{
	public sealed class ConsoleObserver
		: IObserver<char>
	{
		public void OnCompleted() 
		{
			Console.Out.WriteLine("Completed.");
		}

		public void OnError(Exception error)
		{
			Console.Out.WriteLine(error.Message);
		}

		public void OnNext(char value)
		{
			Console.Out.Write(value);
		}
	}
}
