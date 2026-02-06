using Luny;
using System;

public sealed class PresentDeliveryProcessor : LunyScript.LunyScript
{
	public override void Build()
	{
		Var("PlayerName").Set("Santa");
		Var("PresentsDelivered").Set(0);
		GVar("PresentsToDeliver").Set(Int32.MaxValue);
		GVar("PresentsPerSecond").Set(new Number(12427));

		On.FrameUpdate(Var("PresentsDelivered").Inc());
		On.Heartbeat(Var("PresentsWrapped").Inc());
	}

	private void MyCustomCode() => LunyLogger.LogInfo("custom method runs");
}
