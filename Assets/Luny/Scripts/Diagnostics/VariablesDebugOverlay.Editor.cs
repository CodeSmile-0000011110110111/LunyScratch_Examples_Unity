using Luny;
using Luny.Engine.Bridge;
using Luny.Engine.Identity;
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
		m_SelectedScriptContext = null;

		var selectedGameObject = Selection.activeGameObject;
		if (selectedGameObject != null)
		{
			var nativeID = (LunyNativeObjectID)(Int32)selectedGameObject.GetEntityId();
			var context = LunyScriptEngine.Instance.GetScriptContext(nativeID);
			if (context != null)
			{
				m_SelectedScriptContext = context;
				RegisterScriptContextVariableChangeEvents();
			}
		}

		UpdateLocalVariables();
		UpdateInspectorVariables();
	}

	private void RegisterScriptContextVariableChangeEvents()
	{
		if (m_SelectedScriptContext != null)
		{
			m_SelectedScriptContext.LocalVariables.OnVariableChanged += OnLocalVariableChanged;
			//m_SelectedScriptContext.InspectorVariables.OnVariableChanged += OnInspectorVariableChanged;
		}
	}

	private void UnregisterScriptContextVariableChangedEvents()
	{
		if (m_SelectedScriptContext != null)
		{
			m_SelectedScriptContext.LocalVariables.OnVariableChanged -= OnLocalVariableChanged;
			//m_SelectedScriptContext.InspectorVariables.OnVariableChanged -= OnInspectorVariableChanged;
		}
	}

	private void OnLocalVariableChanged(Object sender, LunyScriptVariableChangedArgs e) => UpdateLocalVariables(e.Name);
	//private void OnInspectorVariableChanged(Object sender, VariableChangedEventArgs e) => UpdateInspectorVariables(e.Name);
#endif
}
