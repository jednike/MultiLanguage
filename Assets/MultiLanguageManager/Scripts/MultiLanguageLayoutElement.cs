using MultiLanguage;
using UnityEngine;
using UnityEngine.UI;

namespace MultiLanguage
{
	[RequireComponent(typeof(MultiLanguageText))]
	public class MultiLanguageLayoutElement : LayoutElement
	{
		private MultiLanguageText _text;

		protected override void Awake()
		{
			base.Awake();
			_text = GetComponent<MultiLanguageText>();
		}

		protected override void OnEnable()
		{
			base.OnDisable();
			_text.OnTextChanged += OnTextChanged;
		}

		private void OnTextChanged()
		{
			SetDirty();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_text.OnTextChanged -= OnTextChanged;
		}
	}

}