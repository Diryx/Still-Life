using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class ThemeToggleButton : ButtonParent
    {
        [Header("Lamp Settings")]
        [SerializeField] private Image _lampImage;
        [SerializeField] private Sprite _lightLampSprite;
        [SerializeField] private Sprite _darkLampSprite;

        [Header("Theme Panels")]
        [SerializeField] private GameObject _lightVersion;
        [SerializeField] private GameObject _darkVersion;

        private bool _isLight = true;

        protected override void Start()
        {
            base.Start();
            ApplyTheme(true);
        }

        protected override void ButtonAction()
        {
            base.ButtonAction();
            _isLight = !_isLight;
            ApplyTheme(_isLight);
        }

        private void ApplyTheme(bool isLight)
        {
            if (_lightVersion != null) _lightVersion.SetActive(isLight);
            if (_darkVersion != null) _darkVersion.SetActive(!isLight);

            if (_lampImage != null)
                _lampImage.sprite = isLight ? _lightLampSprite : _darkLampSprite;
        }

        private void OnDisable()
        {
            _isLight = true;
            ApplyTheme(true);
        }
    }
}