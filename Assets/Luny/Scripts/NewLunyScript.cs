using Luny;
using Luny.Diagnostics;
using System;

public sealed class PresentDeliveryProcessor : LunyScript.LunyScript
{
	public override void Build()
	{
		LocalVariables["PlayerName"] = "Santa";
		LocalVariables["PresentsDelivered"] = 0;
		GlobalVariables["PresentsToDeliver"] = Int32.MaxValue;
		GlobalVariables["PresentsPerSecond"] = new Number(12427);

		var pps = (Int32)GlobalVariables.Get<Number>("PresentsPerSecond");
		//OnUpdate(Debug.Log($"Delivered {pps / 60} presents"));

		OnUpdate(
			Run(() =>
			{
				var delivered = LocalVariables.Get<Int32>("PresentsDelivered");
				LocalVariables["PresentsDelivered"] = ++delivered;
			})
		);
		OnFixedStep(
			Run(() =>
			{
				var wrapped = LocalVariables.Get<Int32>("PresentsWrapped");
				LocalVariables["PresentsWrapped"] = ++wrapped;
			})
		);

		// OnUpdate(Run(() => LunyLogger.LogInfo("custom lambda runs")));
		// OnUpdate(Run(MyCustomCode));
	}

	private void MyCustomCode() => LunyLogger.LogInfo("custom method runs");
}
