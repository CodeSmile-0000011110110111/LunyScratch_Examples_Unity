using Luny;
using System;

/// <summary>
/// Example LunyScript demonstrating the block system.
/// This script will automatically bind to any GameObject named "ExampleLunyScript" in the scene.
/// </summary>
internal sealed class ExampleLunyScript : LunyScript.LunyScript
{
	public override void Build()
	{
		// Set up variables
		Variables["Health"] = 100;
		Variables["Name"] = "Player1";
		GlobalVariables["GameScore"] = 0;

		// Log messages on Update
		OnUpdate(Log("ExampleLunyScript Update tick"));

		// Multi-block sequence on Update
		OnUpdate(
			Log("Multi-block sequence start"),
			Do(() =>
			{
				var health = Variables.Get<Int32>("Health");
				Variables["Health"] = health - 1;
				LunyLogger.LogInfo($"Health decreased to {health - 1}", this);

				// I might consider putting the context back, but then we'd need two kinds of contexts ...
				//Log("TEST TEST TEST").Execute(context);
			}),
			Log("Multi-block sequence end")
		);

		// Fixed step logging
		OnFixedStep(Log("ExampleLunyScript FixedStep tick"));

		// Late update logging
		OnLateUpdate(Log("ExampleLunyScript LateUpdate tick"));

		// Demonstrate global variables
		OnUpdate(Do(() =>
		{
			var score = GlobalVariables.Get<Int32>("GameScore");
			GlobalVariables["GameScore"] = score + 1;
		}));
	}
}
