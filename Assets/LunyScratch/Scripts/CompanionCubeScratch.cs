using LunyScratch;
using UnityEngine;
using static LunyScratch.Blocks;

[DisallowMultipleComponent]
public sealed class CompanionCubeScratch : ScratchBehaviour
{
	private void Start() => When(CollisionEnter(name: "police"),
		PlaySound(),
		Wait(1),
		DestroySelf()
	);
}
