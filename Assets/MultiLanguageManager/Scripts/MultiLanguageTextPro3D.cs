using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiLanguage
{
	[AddComponentMenu("MultiLanguage/TextMesh Pro 3D", 10)]
	public class MultiLanguageTextPro3D : TextMeshPro
	{
		[Header("MultiLanguage")]
		[SerializeField] private string _translateId;
		public string TranslateId
		{
			get { return _translateId; }
			set
			{
				if(_translateId.Equals(value))
					return;
				_translateId = value;
				SetTranslateValue();
			}
		}
		
		[SerializeField] private FontStylePresetPro _fontStylePreset;
		public FontStylePresetPro FontStylePreset
		{
			get { return _fontStylePreset; }
			set
			{
				_fontStylePreset = value;
				ApplyFontStylePreset(false);
			}
		}
		private FontStylePresetPro _lastFontStylePreset;

		public string BeforeSymbols
		{
			get => _beforeSymbols;
			set
			{
				_beforeSymbols = value;
				AdditionalSymbols();
			}
		}
		[SerializeField] private string _beforeSymbols = "";
		public string AfterSymbols
		{
			get => _afterSymbols;
			set
			{
				_afterSymbols = value;
				AdditionalSymbols();
			}
		}
		[SerializeField] private string _afterSymbols = "";
		private string _cachedString = "";

		public bool NeedToChangeFont = true;
		public event Action OnTextChanged = delegate { };
		
		public override void SetVerticesDirty()
		{
			ApplyFontStyleToText(m_text);
			base.SetVerticesDirty();
		}

		private CaseFormat _caseFormat;

		protected override void Awake()
		{
			base.Awake();
			
			SetTranslateValue();
			if(_fontStylePreset)
				SetFontCharacteristics(MultiLanguageManager.CurrentLanguage, true);
		}

		private void OnLanguageChange()
		{
			ApplyFontStylePreset(false);
			SetTranslateValue();
		}

		private void ApplyFontStylePreset(bool needChangeText)
		{
			if (!_fontStylePreset || !NeedToChangeFont) return;
			_lastFontStylePreset = _fontStylePreset;
			
			SetFontCharacteristics(MultiLanguageManager.CurrentLanguage, needChangeText);
		}
		public void SetFontCharacteristics(string language, bool needChangeText)
		{
			var stylePreset = _fontStylePreset.Alternatives.FirstOrDefault(alternative => alternative.Language.Equals(language)) ??
			                  _fontStylePreset.Default;
			font = stylePreset.Font;
			fontStyle = stylePreset.FontStyle;
			fontSize = stylePreset.Size;
			lineSpacing = stylePreset.LineSpacing;
			_caseFormat = stylePreset.CaseFormat;
			m_characterSpacing = stylePreset.LetterSpacing;
			
			if(needChangeText)
				ApplyFontStyleToText(text);
		}
		private void SetTranslateValue()
		{
			if(string.IsNullOrEmpty(_translateId))
				return;
			
			string str;
			if (MultiLanguageManager.GetTranslateString(_translateId, out str))
			{
				_cachedString = str;
				text = str;
			}
		}

		private void AdditionalSymbols()
		{
			_cachedString = _beforeSymbols + _cachedString;
			_cachedString += _afterSymbols;
			m_text = _cachedString;
		}
		private void ApplyFontStyleToText(string value)
		{
			_cachedString = value;
			AdditionalSymbols();
			switch (_caseFormat)
			{
				case CaseFormat.UpperCase:
					m_text = value.ToUpper();
					break;
				case CaseFormat.LowerCare:
					m_text = value.ToLower();
					break;
				default:
					m_text = value;
					break;
			}

			OnTextChanged();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			MultiLanguageManager.OnLanguageChange += OnLanguageChange;
			OnLanguageChange();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			MultiLanguageManager.OnLanguageChange -= OnLanguageChange;
		}
		#if UNITY_EDITOR
		protected override void OnValidate()
		{
			if (!IsActive())
			{
				base.OnValidate();
			}
			else
			{
				if (_lastFontStylePreset != _fontStylePreset)
				{
					ApplyFontStylePreset(true);
				}
				base.OnValidate();
			}
		}
		#endif
	}
}