using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiLanguage
{
	[Serializable]
	public struct Language
	{
		public string LanguageCode;
		public string LanguageString;
	}
	[CreateAssetMenu(fileName = "Languages", menuName = "MultiLanguage/Create Languages", order = 1)]
	public class Languages : ScriptableObject
	{
		public Language[] LanguageList;
	}
}