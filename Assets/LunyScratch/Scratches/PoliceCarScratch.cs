using LunyScratch;
using System;
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
		var globalVariables = ScratchRuntime.Singleton.Variables;
		var progressVar = globalVariables["Progress"];
		var scoreVariable = Variables.Set("Score", 0);
		var timeVariable = Variables.Set("Time", _startTimeInSeconds);

		// Handle UI State
		var hud = ScratchRuntime.Singleton.HUD;
		hud.BindVariable(scoreVariable);
		hud.BindVariable(timeVariable);
		Run(HideMenu(), ShowHUD());
		RepeatForever(If(IsKeyPressed(Key.Escape), ShowMenu()));

		// don't play minicube sound too often
		RepeatForever(SubtractVariable(globalVariables["MiniCubeSoundTimeout"], 1));

		// increment progress every so often
		RepeatForever(IncrementVariable(progressVar), Wait(15), PlaySound());

		// Use RepeatForeverPhysics for physics-based movement
		var enableBrakeLights = Sequence(Enable("BrakeLight1"), Enable("BrakeLight2"));
		var disableBrakeLights = Sequence(Disable("BrakeLight1"), Disable("BrakeLight2"));
		RepeatForeverPhysics(
			// Forward/Backward movement
			If(IsKeyPressed(Key.W),
					MoveForward(_moveSpeed), disableBrakeLights)
				.Else(If(IsKeyPressed(Key.S),
						MoveBackward(_moveSpeed), enableBrakeLights)
					.Else(SlowDownMoving(_deceleration), disableBrakeLights)
				),

			// Steering
			If(IsKeyPressed(Key.A), TurnLeft(_turnSpeed)),
			If(IsKeyPressed(Key.D), TurnRight(_turnSpeed))
		);

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

		When(CollisionEnter(tag: "CompanionCube"),
			//AddVariable(scoreVariable, progressVar * progressVar * progressVar), // doesn't work that way
			new ExecuteBlock(() => scoreVariable.Add(progressVar * progressVar * progressVar)),
			IncrementVariable("Time"));

		RepeatForever(Wait(1), DecrementVariable("Time"),
			If(IsVariableLessOrEqual(timeVariable, 0),
				ShowMenu(), SetCameraTrackingTarget(null), Wait(0.5), DisableComponent()));

		// must run globally because we Disable() the car and thus all object sequences will stop updating
		Scratch.When(ButtonClicked("TryAgain"), ReloadCurrentScene());
		Scratch.When(ButtonClicked("Quit"), QuitApplication());

		// TODO:
		// IsVariable(name, operator) or IsVariableEqual/Greater(name)
		// GlobalVariable variants
		// variable add/sub/mul/div etc
		// add name to each variable for debugging and binding
		// AsNumber => property 'number'

		// UI Binding: update to use variable's name

		// CollisionEnter: allow specifying multiple tags or names
		// add IsVelocity tests
		// add PlaySound with timeout?
		// If/When: allow multiple conditions, events (AND or OR)
	}
}
