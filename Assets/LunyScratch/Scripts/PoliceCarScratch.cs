using System;
using UnityEngine;
using static LunyScratch.Blocks;
using static LunyScratch.UnityBlocks;

namespace LunyScratch
{
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
				// Forward/Backward movement - no lambdas needed!
				If(IsKeyPressed(Key.W), MoveForward(_moveSpeed))
					.Else(If(IsKeyPressed(Key.S), MoveBackward(_moveSpeed))
						.Else(SlowDownMoving(_deceleration))
					),

				// Steering - clean and simple!
				If(IsKeyPressed(Key.A), TurnLeft(_turnSpeed)),
				If(IsKeyPressed(Key.D), TurnRight(_turnSpeed))
			);

			var lights = GetComponentsInChildren<Light>();
			RepeatForever(
				Enable(lights[0]),
				Disable(lights[1]),
				Wait(0.15),
				Disable(lights[0]),
				Enable(lights[1]),
				Wait(0.12)
			);
		}
	}
}
