using System;

/// <summary>
/// Example LunyScript demonstrating the block system and Step 2 debug/profiling features.
/// This script will automatically bind to any GameObject named "ExampleLunyScript" in the scene.
/// </summary>
public sealed class EditorLunyScriptTest : LunyScript.LunyScript
{
	public override void Build()
	{
		// Set up variables
		LocalVariables["Health"] = 100;
		LocalVariables["Name"] = "Player1";
		GlobalVariables["GameScore"] = 0;

		// Demonstrate Log vs DebugLog
		// DebugLog() is completely stripped in release builds
		Every.Frame(Debug.LogInfo("Debug-only log - stripped in release"));
		// Log() appears in both debug and release builds
		Every.Frame(Log("ExampleLunyScript Update tick - always logs"));
		Every.FixedStep(Log("ExampleLunyScript FixedStep tick"));
		Every.FrameEnds(Log("ExampleLunyScript LateUpdate tick"));

		// Multi-block sequence demonstrating debug breakpoint
		Every.Frame(
			Log("Multi-block sequence start"),
			Run(() =>
			{
				var health = LocalVariables.Get<Int32>("Health");
				LocalVariables["Health"] = health - 1;
			}),
			Debug.Break("sequence breakpoint"),
			Log("Multi-block sequence end")
		);

		// Demonstrate global variables with variable change tracking
		// In debug builds, Variables.OnVariableChanged events will fire
		Every.Frame(Run(() =>
		{
			var score = GlobalVariables.Get<Int32>("GameScore");
			GlobalVariables["GameScore"] = score + 1;
		}));

		// Note: To enable debug features, build with DEBUG or LUNYSCRIPT_DEBUG defined
		// In release builds, all DebugLog() and DebugBreak() calls have zero overhead

		// To enable execution tracing: context.DebugHooks.EnableTracing = true
		// To enable internal logging: LunyLogger.EnableInternalLogging = true
		// To access profiler: context.BlockProfiler.TakeSnapshot()
	}
}
