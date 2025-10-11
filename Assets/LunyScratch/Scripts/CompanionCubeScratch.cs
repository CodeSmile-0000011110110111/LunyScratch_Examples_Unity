using LunyScratch;
using UnityEngine;
using static LunyScratch.Blocks;

[DisallowMultipleComponent]
public sealed class CompanionCubeScratch : ScratchBehaviour
{
	private void Start()
	{
		// Using short asset address; see AssetRegistryBuilder normalization
		When(CollisionEnter(name:"police"), PlaySound(), InstantiatePrefab("Prefabs/HitEffect"));
		When(CollisionEnter(tag:"CompanionCube"), PlaySound());
	}
}
