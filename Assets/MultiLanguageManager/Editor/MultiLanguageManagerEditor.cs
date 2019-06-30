using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MultiLanguage
{
	[CustomEditor(typeof(MultiLanguageManager))]
	public class MultiLanguageManagerEditor : Editor
	{
		private SerializedProperty _currentLanguage;
		private MultiLanguageManager _languageManager;

		protected void OnEnable()
		{
			_languageManager = (MultiLanguageManager) target;
			_currentLanguage = serializedObject.FindProperty("EditorLanguage");
			_currentLanguage.stringValue = MultiLanguageManager.CurrentLanguage;
		}
		
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			serializedObject.Update();
			EditorGUILayout.PropertyField(_currentLanguage);
			if (GUILayout.Button("Change Language", GUILayout.MinHeight(20)))
			{
				MultiLanguageManager.CurrentLanguage = _currentLanguage.stringValue;
			}
			serializedObject.ApplyModifiedProperties();
		}
	}
}