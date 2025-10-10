using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
	[field: SerializeField] [CreateProperty] public Double Score { get; set; }

	private void OnEnable()
	{
		var doc = GetComponent<UIDocument>();

		var scoreLabel = doc.rootVisualElement.Q<Label>("ScoreLabel");
		scoreLabel.SetBinding("text", new DataBinding
		{
			dataSource = this,
			dataSourcePath = PropertyPath.FromName(nameof(Score)),
			bindingMode = BindingMode.ToTarget,
		});
	}
}
