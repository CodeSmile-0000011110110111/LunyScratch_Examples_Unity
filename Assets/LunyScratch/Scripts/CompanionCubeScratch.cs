using LunyScratch;
using System;
using UnityEngine;
using static LunyScratch.Blocks;

[DisallowMultipleComponent]
public sealed class CompanionCubeScratch : ScratchBehaviour
{
	[SerializeField] private Single _minVelocityForSound = 25f;

	private Rigidbody _rigidbody;

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();

		var globalVariables = ScratchRuntime.Singleton.Variables;
		var progressVar = globalVariables["Progress"];
		var counterVar = Variables["Counter"];

		RepeatForever(new ExecuteBlock(() => counterVar.Add(5)), Wait(1));
		When(CollisionEnter("police"),
			RepeatWhileTrue(() =>
			{
				if (counterVar.AsNumber() > progressVar.AsNumber())
					counterVar.Set(Math.Clamp(progressVar.AsNumber(), 1, 33));
				counterVar.Subtract(1);
				return counterVar.AsNumber() >= 0;
			}, InstantiatePrefab("Prefabs/HitEffect")),
			PlaySound(), Enable("Lights"), Wait(1), Disable("Lights"));
		When(CollisionEnter(),
			If(() => _rigidbody.linearVelocity.sqrMagnitude > _minVelocityForSound * _minVelocityForSound,
				PlaySound()));
	}
}
