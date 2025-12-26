using Luny;
using LunyScript;
using System;
using System.Diagnostics;
#if UNITY_EDITOR
using UnityEditor;
#endif

public sealed partial class VariablesDebugOverlay
{
	[Conditional("UNITY_EDITOR")]
	private void Editor_RegisterSelectionChangedEvent()
	{
#if UNITY_EDITOR
		Selection.selectionChanged += OnEditorSelectionChanged;
		OnEditorSelectionChanged();
#endif
	}

	[Conditional("UNITY_EDITOR")]
	private void Editor_UnregisterSelectionChangedEvent()
	{
#if UNITY_EDITOR
		Selection.selectionChanged -= OnEditorSelectionChanged;
		UnregisterScriptContextVariableChangedEvents();
#endif
	}

#if UNITY_EDITOR
	private void Start() => OnEditorSelectionChanged(); // update once on launch

	private void OnEditorSelectionChanged()
	{
		UnregisterScriptContextVariableChangedEvents();
		m_ScriptContext = null;

		var selectedGameObject = Selection.activeGameObject;
		if (selectedGameObject != null)
		{
			var nativeID = (NativeID)(Int32)selectedGameObject.GetEntityId();
			var context = LunyScriptEngine.Instance.GetScriptContext(nativeID);
			if (context != null)
			{
				m_ScriptContext = context;
				RegisterScriptContextVariableChangeEvents();
			}
		}

		UpdateLocalVariables();
		UpdateInspectorVariables();
	}

	private void RegisterScriptContextVariableChangeEvents()
	{
		if (m_ScriptContext != null)
		{
			m_ScriptContext.LocalVariables.OnVariableChanged += OnLocalVariableChanged;
			m_ScriptContext.InspectorVariables.OnVariableChanged += OnInspectorVariableChanged;
		}
	}

	private void UnregisterScriptContextVariableChangedEvents()
	{
		if (m_ScriptContext != null)
		{
			m_ScriptContext.LocalVariables.OnVariableChanged -= OnLocalVariableChanged;
			m_ScriptContext.InspectorVariables.OnVariableChanged -= OnInspectorVariableChanged;
		}
	}

	private void OnLocalVariableChanged(Object sender, VariableChangedEventArgs e) => UpdateLocalVariables(e.Name);
	private void OnInspectorVariableChanged(Object sender, VariableChangedEventArgs e) => UpdateInspectorVariables(e.Name);
#endif
}
