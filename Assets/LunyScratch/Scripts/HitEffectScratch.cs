using LunyScratch;
using System;
using UnityEngine;
using static LunyScratch.Blocks;

public class HitEffectScratch : ScratchBehaviour
{
	[SerializeField] private Double _timeToLiveInSeconds = 3;

	private void Start() => Run(Wait(_timeToLiveInSeconds), DestroySelf());
}
