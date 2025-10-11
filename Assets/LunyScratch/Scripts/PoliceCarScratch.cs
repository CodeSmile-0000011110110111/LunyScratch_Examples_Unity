using LunyScratch;
using System;
using UnityEngine;
using UnityEngine.UI;
using static LunyScratch.Blocks;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public sealed class PoliceCarScratch : ScratchBehaviour
{
	[Tooltip("In degrees per second")]
	[SerializeField] private Single _turnSpeed = 30f;
	[SerializeField] private Single _moveSpeed = 30f;
	[SerializeField] private Single _deceleration = 0.85f;
	[SerializeField] private int _startTimeInSeconds = 30;

	protected override void OnBehaviourAwake()
	{
		Run(HideMenu(), ShowHUD());

		// Use RepeatForeverPhysics for physics-based movement
		RepeatForeverPhysics(
			// Forward/Backward movement
			If(IsKeyPressed(Key.W), MoveForward(_moveSpeed))
				.Else(If(IsKeyPressed(Key.S), MoveBackward(_moveSpeed))
					.Else(SlowDownMoving(_deceleration))
				),

			// Steering
			If(IsKeyPressed(Key.A), TurnLeft(_turnSpeed)),
			If(IsKeyPressed(Key.D), TurnRight(_turnSpeed))
		);

		// Context-aware! No need to get lights manually
		RepeatForever(
			Enable("BlueLight"),
			Disable("RedLight"),
			Wait(0.15),
			Disable("BlueLight"),
			Enable("RedLight"),
			Wait(0.12)
		);

		var scoreVariable = Variables.Set("Score", 0);
		var timeVariable = Variables.Set("Time", _startTimeInSeconds);
		var hud = ScratchRuntime.Singleton.HUD;
		hud.BindVariable("Score", scoreVariable);
		hud.BindVariable("Time", timeVariable);

		When(CollisionEnter(tag: "CompanionCube"),
			IncrementVariable("Score"),
			IncrementVariable("Time"));

		RepeatForever(Wait(1), DecrementVariable("Time"),
			If(() => timeVariable.AsNumber() <= 0, ShowMenu(), Disable()));

		// must run globally because we Disable() the car and thus all object sequences will stop updating
		Scratch.When(ButtonClicked("TryAgain"), ReloadCurrentScene());
		Scratch.When(ButtonClicked("Quit"), QuitApplication());

	}
}
