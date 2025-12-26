using Luny;
using System;

public sealed class PresentDeliveryProcessor : LunyScript.LunyScript
{
	public override void Build()
	{
		LocalVariables["PlayerName"] = "Santa";
		GlobalVariables["PresentsToDeliver"] = Int32.MaxValue;
		GlobalVariables["PresentsPerSecond"] = new Number(12427);

		var pps = (Int32)GlobalVariables.Get<Number>("PresentsPerSecond");
		//OnUpdate(Log($"Delivered {pps/60} presents"));
	}
}
