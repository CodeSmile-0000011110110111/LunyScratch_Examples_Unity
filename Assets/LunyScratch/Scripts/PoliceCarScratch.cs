using LunyScratch;
using System;
using Unity.Cinemachine;
using UnityEngine;
using static LunyScratch.Blocks;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public sealed class PoliceCarScratch : ScratchBehaviour
{
	[Tooltip("In degrees per second")]
	[SerializeField] private Single _turnSpeed = 30f;
	[SerializeField] private Single _moveSpeed = 30f;
	[SerializeField] private Single _deceleration = 0.85f;
	[SerializeField] private Int32 _startTimeInSeconds = 30;

	protected override void OnBehaviourAwake()
	{
		Run(HideMenu(), ShowHUD());

		// don't play minicube sound too often
		var globalVariables = ScratchRuntime.Singleton.Variables;
		var globalTimeout = globalVariables["MiniCubeSoundTimeout"];
		RepeatForever(new ExecuteBlock(() => globalTimeout.Subtract(1)),
			If(IsKeyPressed(Key.Escape), ShowMenu()));

		var progressVar = globalVariables["Progress"];
		progressVar.Set(0);
		RepeatForever(new ExecuteBlock(() => progressVar.Add(1)), Wait(20));

		// Use RepeatForeverPhysics for physics-based movement
		RepeatForeverPhysics(

			// Forward/Backward movement
			If(IsKeyPressed(Key.W),
					MoveForward(_moveSpeed), Disable("BrakeLight1"), Disable("BrakeLight2"))
				.Else(If(IsKeyPressed(Key.S),
						MoveBackward(_moveSpeed),
						Enable("BrakeLight1"), Enable("BrakeLight2"))
					.Else(SlowDownMoving(_deceleration), Disable("BrakeLight1"), Disable("BrakeLight2"))
				),

			// Steering
			If(IsKeyPressed(Key.A), TurnLeft(_turnSpeed)),
			If(IsKeyPressed(Key.D), TurnRight(_turnSpeed))
		);

		// RepeatForever(
		// 	Enable("BlueLight"),
		// 	Disable("RedLight"),
		// 	Wait(0.15),
		// 	Disable("BlueLight"),
		// 	Enable("RedLight"),
		// 	Wait(0.12)
		// );
		RepeatForever(
			Enable("RedLight"),
			Wait(0.16),
			Disable("RedLight"),
			Wait(0.12)
		);
		RepeatForever(
			Disable("BlueLight"),
			Wait(0.13),
			Enable("BlueLight"),
			Wait(0.17)
		);

		var scoreVariable = Variables.Set("Score", 0);
		var timeVariable = Variables.Set("Time", _startTimeInSeconds);
		var hud = ScratchRuntime.Singleton.HUD;
		hud.BindVariable("Score", scoreVariable);
		hud.BindVariable("Time", timeVariable);

		When(CollisionEnter(tag: "CompanionCube"),
			new ExecuteBlock(() => scoreVariable.Add(progressVar * progressVar)),
			IncrementVariable("Time"));

		RepeatForever(Wait(1), DecrementVariable("Time"),
			If(() => timeVariable.AsNumber() <= 0, ShowMenu(),
				new ExecuteBlock(() => GameObject.Find("CinemachineCamera").GetComponent<CinemachineCamera>().Target.TrackingTarget = null),
				Wait(0.5),
				new ExecuteBlock(() => enabled = false)));

		// must run globally because we Disable() the car and thus all object sequences will stop updating
		Scratch.When(ButtonClicked("TryAgain"), ReloadCurrentScene());
		Scratch.When(ButtonClicked("Quit"), QuitApplication());

		// TODO:
		// IsVariable(name, operator) or IsVariableEqual/Greater(name)
		// GlobalVariable variants
		// variable add/sub/mul/div etc
		// add name to each variable for debugging and binding
		// Binding: update to use variable's name
		// CollisionEnter: allow specifying multiple tags or names
		// add IsVelocity tests
		// add PlaySound with timeout?
		// If/When: allow multiple conditions, events (AND or OR)
	}
}
