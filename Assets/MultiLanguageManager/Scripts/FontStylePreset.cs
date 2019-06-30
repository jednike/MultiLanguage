using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiLanguage
{
    [CreateAssetMenu(fileName = "FontStyles", menuName = "MultiLanguage/Create Font Style", order = 1)]
    public class FontStylePreset : ScriptableObject {
        public string FontStyleName;
        public LanguageFontPreset Default;
        public AlternativeLanguageFontPreset[] Alternatives;
    }

}