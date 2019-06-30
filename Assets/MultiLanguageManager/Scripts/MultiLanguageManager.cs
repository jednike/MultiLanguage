using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace MultiLanguage
{
	[ExecuteAlways]
	public class MultiLanguageManager : MonoBehaviour {
		private static Dictionary<string, string> Dictionary = new Dictionary<string, string>();
		private static MultiLanguageManager _instance;
		
		public static string CurrentLanguage
		{
			get { return _currentLanguage; }
			set
			{
				if(_currentLanguage.Equals(value))
					return;
				
				_currentLanguage = value;
				PlayerPrefs.SetString("Language", _currentLanguage);
				PlayerPrefs.Save();
				LoadNewLanguage();
			}
		}
		private static string _currentLanguage = "en";

		public static event Action OnLanguageChange = delegate { };
		
		[HideInInspector] public string EditorLanguage;
		[SerializeField] private Languages _languages;

		private void Awake()
		{
			_instance = this;
			DetectLanguage();
		}

		private static void DetectLanguage()
		{
			var language = PlayerPrefs.GetString("Language", "");
			if (string.IsNullOrEmpty(language))
			{
				switch (Application.systemLanguage)
				{
					case SystemLanguage.Chinese:
					case SystemLanguage.ChineseTraditional:
						language = "zh-tw";
						break;
					case SystemLanguage.ChineseSimplified:
						language = "zh-cn";
						break;
					case SystemLanguage.French:
						language = "fr";
						break;
					case SystemLanguage.German:
						language = "de";
						break;
					case SystemLanguage.Indonesian:
						language = "id";
						break;
					case SystemLanguage.Japanese:
						language = "ja";
						break;
					case SystemLanguage.Korean:
						language = "ko";
						break;
					case SystemLanguage.Portuguese:
						language = "pt";
						break;
					case SystemLanguage.Spanish:
						language = "es";
						break;
					case SystemLanguage.Russian:
					case SystemLanguage.Ukrainian:
					case SystemLanguage.Belarusian:
						language = "ru";
						break;
					case SystemLanguage.Vietnamese:
						language = "vi";
						break;
					default:
						language = "en";
						break;
				}
			}
			_currentLanguage = language;
			
			LoadNewLanguage();
		}

		public static Languages GetLanguages()
		{
			return _instance._languages;
		}

		public static bool GetTranslateString(string stringCode, out string translate)
		{
			translate = "";
			return !string.IsNullOrEmpty(stringCode) && Dictionary.TryGetValue(stringCode, out translate);
		}
		
		public static void LoadNewLanguage()
		{
			var textAsset = (TextAsset) Resources.Load("Languages/" + CurrentLanguage, typeof(TextAsset));

			if (!textAsset)
			{
				PlayerPrefs.SetString("Language", "en");
				DetectLanguage();
				return;
			}
				
			var newDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(textAsset.text);
			if (newDict == null || newDict.Count == 0)
			{
				Debug.Log("Language not found");
				return;
			}

			Dictionary.Clear();
			Dictionary = newDict;

			OnLanguageChange();
		}
	}
}
