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
		LocalVars["Health"] = 100;
		LocalVars["Name"] = "Player1";
		GlobalVars["GameScore"] = 0;

		// Demonstrate Log vs DebugLog
		// DebugLog() is completely stripped in release builds
		When.Self.Updates(Debug.LogInfo("Debug-only log - stripped in release"));
		// Log() appears in both debug and release builds
		When.Self.Updates(Engine.Log("ExampleLunyScript Update tick - always logs"));
		When.Self.Steps(Engine.Log("ExampleLunyScript FixedStep tick"));
		When.Self.LateUpdates(Engine.Log("ExampleLunyScript LateUpdate tick"));

		// Multi-block sequence demonstrating debug breakpoint
		When.Self.Updates(
			Engine.Log("Multi-block sequence start"),
			Method.Run(() =>
			{
				var health = LocalVars.Get<Int32>("Health");
				LocalVars["Health"] = health - 1;
			}),
			Debug.Break("sequence breakpoint"),
			Engine.Log("Multi-block sequence end")
		);

		// Demonstrate global variables with variable change tracking
		// In debug builds, Variables.OnVariableChanged events will fire
		When.Self.Updates(Method.Run(() =>
		{
			var score = GlobalVars.Get<Int32>("GameScore");
			GlobalVars["GameScore"] = score + 1;
		}));

		// Note: To enable debug features, build with DEBUG or LUNYSCRIPT_DEBUG defined
		// In release builds, all DebugLog() and DebugBreak() calls have zero overhead

		// To enable execution tracing: context.DebugHooks.EnableTracing = true
		// To enable internal logging: LunyLogger.EnableInternalLogging = true
		// To access profiler: context.BlockProfiler.TakeSnapshot()
	}
}
