using LunyScript;
using System;
using System.Text;
using TMPro;
using UnityEngine;
using Object = System.Object;

public sealed partial class VariablesDebugOverlay : MonoBehaviour
{
	[SerializeField] private TMP_Text m_GlobalVarsText;
	[SerializeField] private TMP_Text m_LocalVarsText;
	[SerializeField] private TMP_Text m_InspectorVarsText;

	private WorldSpaceDebugAnchor m_WorldSpaceAnchor;
	private StringBuilder m_StringBuilder = new();
	private ScriptContext m_SelectedScriptContext;

	private void Awake()
	{
		if (m_GlobalVarsText == null)
			throw new ArgumentNullException(nameof(m_GlobalVarsText));
		if (m_LocalVarsText == null)
			throw new ArgumentNullException(nameof(m_LocalVarsText));
		if (m_InspectorVarsText == null)
			throw new ArgumentNullException(nameof(m_InspectorVarsText));

		m_WorldSpaceAnchor = FindFirstObjectByType<WorldSpaceDebugAnchor>();
		if (m_WorldSpaceAnchor == null)
			throw new NullReferenceException("WorldSpaceDebugAnchor not found");
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
		var text = UpdateLabel(nameof(ScriptContext.GlobalVariables), globalVariables, variableName);
		m_GlobalVarsText.text = text;
	}

	private void UpdateLocalVariables(String variableName = null)
	{
		var text = "";
		if (m_SelectedScriptContext != null)
		{
			var name = m_SelectedScriptContext.EngineObject.Name;
			var localVariables = m_SelectedScriptContext.LocalVariables;
			var title = $"{nameof(ScriptContext.LocalVariables)} ({name})";
			text = UpdateLabel(title, localVariables, variableName);

			if (m_WorldSpaceAnchor != null)
			{
				var go = m_SelectedScriptContext.EngineObject.GetNativeObject() as GameObject;
				if (go != null)
				{
					m_WorldSpaceAnchor.Target = go.transform;
					m_WorldSpaceAnchor.Label.text = text;
				}
			}
		}
		else
			text = UpdateLabel(nameof(ScriptContext.LocalVariables), null, null);

		m_LocalVarsText.text = text;
	}

	private void UpdateInspectorVariables(String variableName = null)
	{
		if (m_SelectedScriptContext != null)
		{
			var name = m_SelectedScriptContext.EngineObject.Name;
			var inspectorVariables = m_SelectedScriptContext.InspectorVariables;
			var title = $"{nameof(ScriptContext.InspectorVariables)} ({name})";
			var text = UpdateLabel(title, inspectorVariables, variableName);
			m_InspectorVarsText.text = text;
		}
		else
		{
			var text = UpdateLabel(nameof(ScriptContext.InspectorVariables), null, null);
			m_InspectorVarsText.text = text;
		}
	}

	private String UpdateLabel(String title, Variables variables, String variableName)
	{
		m_StringBuilder.Clear();
		m_StringBuilder.AppendLine(title);

		if (variables != null)
		{
			foreach (var kvp in variables)
			{
				if (kvp.Key == variableName)
				{
					m_StringBuilder.Append("<color=\"green\">");
					m_StringBuilder.AppendLine($"  {kvp.Key} = {kvp.Value} [{Time.frameCount}]");
					m_StringBuilder.Append("</color>");
				}
				else
					m_StringBuilder.AppendLine($"  {kvp.Key} = {kvp.Value}");
			}
		}

		return m_StringBuilder.ToString();
	}
}
