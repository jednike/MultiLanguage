using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiLanguage
{
    [CreateAssetMenu(fileName = "FontStylesPro", menuName = "MultiLanguage/Create Font Style Pro", order = 2)]
    public class FontStylePresetPro : ScriptableObject {
        public string FontStyleName;
        public LanguageFontPresetPro Default;
        public AlternativeLanguageFontPresetPro[] Alternatives;
    }

}