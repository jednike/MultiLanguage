using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiLanguage
{
	public enum CaseFormat
	{
		Normal,
		UpperCase,
		LowerCare
	}
	[Serializable]
	public class LanguageFontPreset
	{		
		public Font Font;
		public FontStyle FontStyle = FontStyle.Normal;
		public int Size = 60;
		public CaseFormat CaseFormat = CaseFormat.Normal; 
		public float LetterSpacing = 0f;
		public float LineSpacing = 1f;
	}
	[Serializable]
	public class LanguageFontPresetPro
	{		
		public TMP_FontAsset Font;
		public FontStyles FontStyle = FontStyles.Normal;
		public float Size = 60;
		public CaseFormat CaseFormat = CaseFormat.Normal; 
		public float LetterSpacing = 0f;
		public float LineSpacing = 1f;
	}
	[Serializable]
	public class AlternativeLanguageFontPreset: LanguageFontPreset
	{
		public string Language = "Russian";
	}
	[Serializable]
	public class AlternativeLanguageFontPresetPro: LanguageFontPresetPro
	{
		public string Language = "Russian";
	}
	
	[RequireComponent(typeof(LetterSpacing))]
	[AddComponentMenu("MultiLanguage/Text", 10)]
	public class MultiLanguageText : Text
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
		
		[SerializeField] private FontStylePreset _fontStylePreset;
		public FontStylePreset FontStylePreset
		{
			get { return _fontStylePreset; }
			set
			{
				_fontStylePreset = value;
				ApplyFontStylePreset(false);
			}
		}
		private FontStylePreset _lastFontStylePreset;

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
		private string _newString = "";

		public bool NeedToChangeFont = true;
		public event Action OnTextChanged = delegate { };
		
		public override string text
		{
			get
			{
				return m_Text;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					if (string.IsNullOrEmpty(m_Text))
						return;
					m_Text = "";
					SetVerticesDirty();
				}
				else
				{
					if (m_Text == value)
						return;
					_newString = value;
					ApplyFontStyleToText(value);
					SetVerticesDirty();
					SetLayoutDirty();
				}
			}
		}

		private LetterSpacing _letterSpacing;
		private CaseFormat _caseFormat;

		protected override void Awake()
		{
			base.Awake();
			
			_letterSpacing = GetComponent<LetterSpacing>();
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
			if(_letterSpacing)
				_letterSpacing.spacing = stylePreset.LetterSpacing;
			
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
			if (!string.IsNullOrEmpty(_cachedString))
			{
				m_Text = _beforeSymbols + _cachedString;
				m_Text += _afterSymbols;
				return;
			}
			m_Text = _newString;
		}
		private void ApplyFontStyleToText(string value)
		{
			AdditionalSymbols();
			switch (_caseFormat)
			{
				case CaseFormat.UpperCase:
					m_Text = m_Text.ToUpper();
					break;
				case CaseFormat.LowerCare:
					m_Text = m_Text.ToLower();
					break;
				default:
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