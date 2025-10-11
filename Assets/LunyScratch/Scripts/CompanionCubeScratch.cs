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

		When(CollisionEnter("police"), InstantiatePrefab("Prefabs/HitEffect"));
		When(CollisionEnter(),
			If(() => _rigidbody.linearVelocity.sqrMagnitude > _minVelocityForSound * _minVelocityForSound, PlaySound()));
	}
}
