using LunyScratch;
using System;
using UnityEngine;

public class DayNightCycleScratch : ScratchBehaviour
{
	public Single _timeIncrement = 0.02f;

	private void Start()
	{
		var rotIncrement = Quaternion.Euler(_timeIncrement, 0, 0);
		RepeatForeverPhysics(new ExecuteBlock(() =>
		{
			var rot = transform.localRotation;
			rot *= rotIncrement;
			transform.localRotation = rot;
		}));
	}
}
