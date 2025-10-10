using LunyScratch;
using UnityEngine;
using static LunyScratch.Blocks;

[DisallowMultipleComponent]
public sealed class CompanionCubeScratch : ScratchBehaviour
{
	private void Start()
	{
		When(CollisionEnter(name:"police"), PlaySound(), InstantiatePrefab("Assets/LunyScratch/Prefabs/HitEffect.prefab"));
		When(CollisionEnter(tag:"CompanionCube"), PlaySound());
	}
}
