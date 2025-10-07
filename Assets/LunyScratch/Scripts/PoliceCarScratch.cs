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
		// FIXME: due to sequences set up in start, changes of values at runtime do not take effect
		[Tooltip("In degrees per second")]
		[SerializeField] private Single _turnSpeed = 30f;
		[SerializeField] private Single _moveSpeed = 30f;
		[SerializeField] private Single _deceleration = 0.85f;

		private Rigidbody _rigidbody;

		private void Awake() => _rigidbody = GetComponent<Rigidbody>();

		private void Start()
		{
			// Use RepeatForeverPhysics for physics-based movement
			Scratch.RepeatForeverPhysics(
				// Forward/Backward movement
				If(() => IsKeyPressed(Key.W), MoveForward(_rigidbody, _moveSpeed))
					.Else(If(() => IsKeyPressed(Key.S), MoveBackward(_rigidbody, _moveSpeed))
							.Else(SlowDownMoving(_rigidbody, _deceleration))
					),

				// Steering - now uses rigidbody instead of transform
				If(() => IsKeyPressed(Key.A), TurnLeft(_rigidbody, _turnSpeed)),
				If(() => IsKeyPressed(Key.D), TurnRight(_rigidbody, _turnSpeed))
			);

			var lights = GetComponentsInChildren<Light>();
			Scratch.RepeatForever(
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
