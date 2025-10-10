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

	protected override void OnBehaviourAwake()
	{
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

		When(CollisionEnter(tag:"CompanionCube"),
			Say("Police collided with cube!"),
			IncrementVariable("Score"));
	}
}
