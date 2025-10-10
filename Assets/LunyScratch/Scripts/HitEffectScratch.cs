using LunyScratch;
using UnityEngine;
using static LunyScratch.Blocks;

public class HitEffectScratch : ScratchBehaviour
{
	[SerializeField] private double _timeToLiveInSeconds = 3;
	private void Start() => Run(Wait(_timeToLiveInSeconds), DestroySelf());
}
