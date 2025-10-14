using LunyScratch;
using UnityEngine;
using static LunyScratch.Blocks;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public sealed class GoldCubeScratch : ScratchBehaviour
{
	private Rigidbody _rigidbody;

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();

		var globalTimeout = ScratchRuntime.Singleton.Variables["MiniCubeSoundTimeout"];
		When(CollisionEnter(),
			If(AND(IsVariableLessThan(globalTimeout, 0), IsVelocityGreater(10)),
				PlaySound(), SetVariable(globalTimeout, 0)));
	}
}
