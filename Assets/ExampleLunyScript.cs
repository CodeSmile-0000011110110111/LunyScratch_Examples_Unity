using Luny;
using System;

/// <summary>
/// Example LunyScript demonstrating the block system and Step 2 debug/profiling features.
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

		// Demonstrate Log vs DebugLog
		// Log() appears in both debug and release builds
		OnUpdate(Log("ExampleLunyScript Update tick - always logs"));

		// DebugLog() is completely stripped in release builds
		OnUpdate(DebugLog("Debug-only log - stripped in release"));

		// Multi-block sequence demonstrating debug breakpoint
		OnUpdate(
			Log("Multi-block sequence start"),
			Do(() =>
			{
				var health = Variables.Get<Int32>("Health");
				Variables["Health"] = health - 1;

				// Use LunyLogger directly for contextual logging
				LunyLogger.LogInfo($"Health decreased to {health - 1}", this);

				// Example: Trigger debugger breakpoint when health gets low
				// This is completely stripped in release builds
				if (health - 1 < 20)
				{
					// Note: DebugBreak().Execute() would need the context, so we don't use it inline
					// It's meant to be used as a block in the chain
				}
			}),
			Log("Multi-block sequence end")
		);

		// Demonstrate DebugBreak - triggers when health is low
		// This is completely stripped in release builds
		OnUpdate(
			Do(() =>
			{
				var health = Variables.Get<Int32>("Health");
				if (health < 20)
				{
					LunyLogger.LogWarning("Health critically low!", this);
				}
			})
			// DebugBreak("Health critically low!") could be added here if needed
		);

		// Fixed step logging
		OnFixedStep(Log("ExampleLunyScript FixedStep tick"));

		// Late update with debug logging
		OnLateUpdate(
			DebugLog("Debug: LateUpdate tick"),
			Log("ExampleLunyScript LateUpdate tick")
		);

		// Demonstrate global variables with variable change tracking
		// In debug builds, Variables.OnVariableChanged events will fire
		OnUpdate(Do(() =>
		{
			var score = GlobalVariables.Get<Int32>("GameScore");
			GlobalVariables["GameScore"] = score + 1;
		}));

		// Note: To enable debug features, build with DEBUG, LUNY_DEBUG, or LUNYSCRIPT_DEBUG defined
		// In release builds, all DebugLog() and DebugBreak() calls have zero overhead

		// To enable execution tracing: context.DebugHooks.EnableTracing = true
		// To enable internal logging: LunyLogger.EnableInternalLogging = true
		// To access profiler: context.BlockProfiler.TakeSnapshot()
	}
}
