using LunyScratch;
using UnityEngine;
using static LunyScratch.Blocks;

public class GoldCubeScratch : ScratchBehaviour
{
	private Rigidbody _rigidbody;

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();

		var globalTimeout = ScratchRuntime.Singleton.Variables["MiniCubeSoundTimeout"];
		When(CollisionEnter(),
			If(() => globalTimeout.Number < 0 && _rigidbody.linearVelocity.sqrMagnitude > 10,
				PlaySound(), new ExecuteBlock(() => globalTimeout.Set(0))));
	}
}
