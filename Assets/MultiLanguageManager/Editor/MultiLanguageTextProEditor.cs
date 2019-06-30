using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace MultiLanguage
{
	[CustomEditor(typeof(MultiLanguageTextPro))]
	[CanEditMultipleObjects]
	public class MultiLanguageTextProEditor : TMP_UiEditorPanel  {
		private SerializedProperty _translateId;
		private SerializedProperty _fontStylePreset;
		private SerializedProperty _text;
		private SerializedProperty _beforeSymbols;
		private SerializedProperty _afterSymbols;

		protected override void OnEnable()
		{
			base.OnEnable();
			_text = serializedObject.FindProperty("m_text");
			_translateId = serializedObject.FindProperty("_translateId");
			_fontStylePreset = serializedObject.FindProperty("_fontStylePreset");
			_beforeSymbols = serializedObject.FindProperty("_beforeSymbols");
			_afterSymbols = serializedObject.FindProperty("_afterSymbols");
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			serializedObject.Update();
			EditorGUILayout.PropertyField(_translateId);
			if (GUILayout.Button("Get Translate String", GUILayout.MinHeight(20)))
			{
				string value;
				if (MultiLanguageManager.GetTranslateString(_translateId.stringValue, out value))
				{
					_text.stringValue = value;
				}
				else
				{
				}
			}
			EditorGUILayout.PropertyField(_fontStylePreset);
			EditorGUILayout.PropertyField(_beforeSymbols);
			EditorGUILayout.PropertyField(_afterSymbols);
			serializedObject.ApplyModifiedProperties();
		}
	}

}