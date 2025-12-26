using LunyScript;
using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Object = System.Object;

public sealed partial class VariablesDebugOverlay : MonoBehaviour
{
	[SerializeField] private TMP_Text m_GlobalVarsText;
	[SerializeField] private TMP_Text m_LocalVarsText;
	[SerializeField] private TMP_Text m_InspectorVarsText;

	private StringBuilder m_StringBuilder = new();
	private ScriptContext m_ScriptContext;

	private void Awake()
	{
		if (m_GlobalVarsText == null)
			throw new ArgumentNullException(nameof(m_GlobalVarsText));
		if (m_LocalVarsText == null)
			throw new ArgumentNullException(nameof(m_LocalVarsText));
		if (m_InspectorVarsText == null)
			throw new ArgumentNullException(nameof(m_InspectorVarsText));
	}

	private void OnEnable()
	{
		var globalVariables = LunyScript.LunyScript.GlobalVariables;
		globalVariables.OnVariableChanged += OnGlobalVariableChanged;
		Editor_RegisterSelectionChangedEvent();

		UpdateAllVariables();
	}

	private void OnDisable()
	{
		var globalVariables = LunyScript.LunyScript.GlobalVariables;
		globalVariables.OnVariableChanged -= OnGlobalVariableChanged;
		Editor_UnregisterSelectionChangedEvent();
	}

	private void OnGlobalVariableChanged(Object sender, VariableChangedEventArgs e) => UpdateGlobalVariables(e.Name);

	private void UpdateAllVariables()
	{
		UpdateGlobalVariables();
		UpdateLocalVariables();
		UpdateInspectorVariables();
	}

	private void UpdateGlobalVariables(String variableName = null)
	{
		var globalVariables = ScriptContext.GlobalVariables;
		UpdateLabel(nameof(ScriptContext.GlobalVariables), globalVariables, variableName, m_GlobalVarsText);
	}

	private void UpdateLocalVariables(String variableName = null)
	{
		if (m_ScriptContext != null)
		{
			var name = m_ScriptContext.EngineObject.Name;
			var localVariables = m_ScriptContext.LocalVariables;
			var title = $"{nameof(ScriptContext.LocalVariables)} ({name})";
			UpdateLabel(title, localVariables, variableName, m_LocalVarsText);
		}
		else
			UpdateLabel(nameof(ScriptContext.LocalVariables), null, null, m_LocalVarsText);
	}

	private void UpdateInspectorVariables(String variableName = null)
	{
		if (m_ScriptContext != null)
		{
			var name = m_ScriptContext.EngineObject.Name;
			var inspectorVariables = m_ScriptContext.InspectorVariables;
			var title = $"{nameof(ScriptContext.InspectorVariables)} ({name})";
			UpdateLabel(title, inspectorVariables, variableName, m_InspectorVarsText);
		}
		else
			UpdateLabel(nameof(ScriptContext.InspectorVariables), null, null, m_InspectorVarsText);
	}

	private void UpdateLabel(String title, Variables variables, String variableName, TMP_Text label)
	{
		var currentFrame = Time.frameCount;
		m_StringBuilder.AppendLine(title);
		if (variables != null)
		{
			foreach (var kvp in variables)
			{
				if (kvp.Key == variableName)
				{
					m_StringBuilder.Append("<color=\"green\">");
					m_StringBuilder.AppendLine($"  {kvp.Key} = {kvp.Value} [{currentFrame}]");
					m_StringBuilder.Append("</color>");
				}
				else
					m_StringBuilder.AppendLine($"  {kvp.Key} = {kvp.Value}");
			}
		}

		label.text = m_StringBuilder.ToString();
		m_StringBuilder.Clear();
	}
}
