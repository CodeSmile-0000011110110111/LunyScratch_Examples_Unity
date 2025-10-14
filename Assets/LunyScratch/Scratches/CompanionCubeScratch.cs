using LunyScratch;
using System;
using UnityEngine;
using static LunyScratch.Blocks;

[DisallowMultipleComponent]
public sealed class CompanionCubeScratch : ScratchBehaviour
{
	[SerializeField] private Single _minVelocityForSound = 25f;

	private void Start()
	{
		var globalVariables = ScratchRuntime.Singleton.Variables;
		var progressVar = globalVariables["Progress"];
		var counterVar = Variables["Counter"];

		// increment counter to be able to hit the ball again
		RepeatForever(AddVariable(counterVar, 5), Wait(1));

		When(CollisionEnter("police"),
			// play bump sound unconditionally and make cube glow
			PlaySound(), Enable("Lights"),
			// count down from current progress value to spawn more cube instances the longer the game progresses
			RepeatWhileTrue(() =>
			{
				if (counterVar.Number > progressVar.Number)
					counterVar.Set(Math.Clamp(progressVar.Number, 1, 33));
				counterVar.Subtract(1);
				Debug.Log($"[{Time.frameCount}] Spawn gold cube");
				return counterVar.Number >= 0;
			}, InstantiatePrefab("Prefabs/HitEffect")),
			Wait(1), Disable("Lights"));

		// play sound when ball bumps into anything
		When(CollisionEnter(),
			If(IsLinearVelocityGreaterThan(_minVelocityForSound),
				PlaySound()));
	}
}
