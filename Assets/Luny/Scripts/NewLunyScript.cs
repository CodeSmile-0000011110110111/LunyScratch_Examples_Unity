using Luny;
using System;

public sealed class PresentDeliveryProcessor : LunyScript.LunyScript
{
	public override void Build()
	{
		LocalVars["PlayerName"] = "Santa";
		LocalVars["PresentsDelivered"] = 0;
		GlobalVars["PresentsToDeliver"] = Int32.MaxValue;
		GlobalVars["PresentsPerSecond"] = new Number(12427);

		var pps = (Int32)GlobalVars.Get<Number>("PresentsPerSecond");
		//OnUpdate(Debug.Log($"Delivered {pps / 60} presents"));

		When.Self.Updates(
			Method.Run(() =>
			{
				var delivered = LocalVars.Get<Int32>("PresentsDelivered");
				LocalVars["PresentsDelivered"] = ++delivered;
			})
		);
		When.Self.Steps(
			Method.Run(() =>
			{
				var wrapped = LocalVars.Get<Int32>("PresentsWrapped");
				LocalVars["PresentsWrapped"] = ++wrapped;
			})
		);

		// OnUpdate(Run(() => LunyLogger.LogInfo("custom lambda runs")));
		// OnUpdate(Run(MyCustomCode));
	}

	private void MyCustomCode() => LunyLogger.LogInfo("custom method runs");
}
