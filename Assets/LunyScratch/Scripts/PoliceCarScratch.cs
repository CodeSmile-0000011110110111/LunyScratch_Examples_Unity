using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static LunyScratch.Blocks;
using static LunyScratch.UnityBlocks;

namespace LunyScratch
{
	[RequireComponent(typeof(Rigidbody))]
	[DisallowMultipleComponent]
	public sealed class PoliceCarScratch : ScratchBehaviour
	{
		[SerializeField] private Single _accelerateDelay = 1.2f;
		[SerializeField] [Range(0.001f, 1f)] private Single _acceleration = 0.012f;
		[SerializeField] private Single _maxSpeed = 2f;
		[SerializeField] private Single _stopX = 9f;
		[SerializeField] private Single _stopVelocitySlowdown = 0.95f;
		[SerializeField] private Single _stopVelocityY = -1f;

		private Single _speed;
		private Vector3 _heading;
		private Rigidbody _rigidbody;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_heading = transform.forward;
		}

		private void Start()
		{
			/*
			Scratch.Run(
				Wait(_accelerateDelay),
				RepeatForever(MoveCar)
			);
		*/

			Scratch.RepeatForever(
				If(() => IsKeyJustPressed(Key.Any),
					Log("hello")
				)
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


		void Update()
		{
			var keyboard = Keyboard.current;
			if (keyboard.wKey.wasPressedThisFrame)
				Debug.Log($"Key: {keyboard.wKey}");
		}

		private void MoveCar()
		{
			if (transform.position.x < _stopX)
			{
				_speed = 0f;
				var velocity = _rigidbody.linearVelocity;
				velocity *= _stopVelocitySlowdown;
				velocity.y = -_stopVelocityY;
				_rigidbody.linearVelocity = velocity;
			}
			else if (_speed < _maxSpeed)
			{
				_speed += _acceleration;
				_rigidbody.linearVelocity += _heading * _speed;
			}
		}
	}
}
