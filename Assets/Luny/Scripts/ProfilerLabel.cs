using Luny;
using Luny.Diagnostics;
using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public sealed class ProfilerLabel : MonoBehaviour
{
	[SerializeField] [Range(1, 120)] private Int32 m_UpdateIntervalInFrames = 30;

	private TMP_Text m_Text;
	private IEngineProfiler m_Profiler;
	private StringBuilder m_StringBuilder = new();

	private void OnValidate()
	{
		if (Application.isPlaying)
			StartSnapshotUpdates();
	}

	private void Start()
	{
		m_Text = GetComponent<TMP_Text>();
		m_Profiler = LunyEngine.Instance.Profiler;

		StartSnapshotUpdates();
	}

	private void StartSnapshotUpdates()
	{
		StopAllCoroutines();
		StartCoroutine(UpdateProfilerSnapshot());
	}

	private IEnumerator UpdateProfilerSnapshot()
	{
		Debug.LogWarning("Profiler updates running ...");

		var nextIntervalFrame = Time.frameCount + m_UpdateIntervalInFrames;
		var waitInterval = new WaitUntil(() =>
		{
			if (Time.frameCount < nextIntervalFrame)
				return false;

			nextIntervalFrame = Time.frameCount + m_UpdateIntervalInFrames;
			return true;
		});

		var waitForEndOfFrame = new WaitForEndOfFrame();

		while (true)
		{
			yield return waitInterval;
			yield return waitForEndOfFrame;

			UpdateLabel();
		}
	}

	private void UpdateLabel()
	{
		var snapshot = m_Profiler.TakeSnapshot();
		m_StringBuilder.AppendLine(snapshot.ToString());

		foreach (var metrics in snapshot.ObserverMetrics)
			m_StringBuilder.AppendLine($"  {metrics.ObserverName} Ø {metrics.AverageMs:F2} ms ({metrics.MinMs:F3}—{metrics.MaxMs:F3} ms)");

		m_Text.text = m_StringBuilder.ToString();
		m_StringBuilder.Clear();
	}
}
