using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Game.Scripts
{
    public class ColourPickerControl : MonoBehaviour
    {
        public float CurrentHue, CurrentSaturation, CurrentValue;

        [SerializeField, StatusIcon] private RawImage hueImage, saturationImage, valueImage, outputImage;
        [SerializeField, StatusIcon] private Slider hueSlider, saturationSlider, valueSlider;
        [SerializeField, StatusIcon] private TMP_InputField hexInputField;

        private Texture2D hueTexture, saturationTexture, valueTexture;

        [SerializeField] private UI.MenuCar car;

        private void Start()
        {
            CreateHueImage();
            CreateSaturationImage();
            CreateValueImage();
            Color.RGBToHSV(car.GetCurrentColor(), out CurrentHue, out CurrentSaturation, out CurrentValue);
            hueSlider.value = CurrentHue;
            saturationSlider.value = CurrentSaturation;
            valueSlider.value = CurrentValue;
            UpdateOutputImage();
            ApplyButton(false);
        }

        void OnEnable()
        {
            ApplyButton(false);
        }

        public void ApplyButton(bool value)
        {
            UnityEngine.UI.Image applyButton = transform.Find("ButtonApply").GetComponent<UnityEngine.UI.Image>();
            TMPro.TextMeshProUGUI applyButtonText = applyButton.transform.Find("Lable").GetComponent<TMPro.TextMeshProUGUI>();

            TMPro.TextMeshProUGUI costText = transform.Find("CostText").GetComponent<TMPro.TextMeshProUGUI>();

            applyButton.enabled = value;
            applyButtonText.enabled = value;
            costText.enabled = value;
        }

        private void CreateHueImage()
        {
            hueTexture = new Texture2D(1, 16);
            hueTexture.wrapMode = TextureWrapMode.Clamp;
            hueTexture.name = "HueTexture";

            for (int i = 0; i < hueTexture.height; i++)
                hueTexture.SetPixel(0, i, Color.HSVToRGB((float)i / hueTexture.height, 1, 1f));

            hueTexture.Apply();

            hueImage.texture = hueTexture;
        }

        private void CreateSaturationImage()
        {
            saturationTexture = new Texture2D(16, 16);
            saturationTexture.wrapMode = TextureWrapMode.Clamp;
            saturationTexture.name = "SaturationTexture";

            for (int y = 0; y < saturationTexture.height; y++)
            {
                for (int x = 0; x < saturationTexture.width; x++)
                {
                    saturationTexture.SetPixel(x, y, Color.HSVToRGB(CurrentHue, (float)x / saturationTexture.width, (float)16 / saturationTexture.height));
                }
            }

            saturationTexture.Apply();

            saturationImage.texture = saturationTexture;
        }

        private void CreateValueImage()
        {
            valueTexture = new Texture2D(1, 16);
            valueTexture.wrapMode = TextureWrapMode.Clamp;
            valueTexture.name = "valueTexture";

            for (int y = 0; y < valueTexture.height; y++)
            {
                for (int x = 0; x < valueTexture.width; x++)
                {
                    valueTexture.SetPixel(x, y, Color.HSVToRGB(CurrentHue, (float)x / valueTexture.width, (float)y / valueTexture.height));
                }
            }

            valueTexture.Apply();

            valueImage.texture = valueTexture;
        }

        private void UpdateOutputImage()
        {
            Color currentColor = Color.HSVToRGB(CurrentHue, CurrentSaturation, CurrentValue);

            ApplyButton(true);
            car.ChangeCarColor(currentColor);
        }

        public void SetSaturation()
        {
            CurrentSaturation = saturationSlider.value;

            UpdateOutputImage();
        }

        public void SetValue()
        {
            CurrentValue = valueSlider.value;

            UpdateOutputImage();
        }

        public void UpdateSVPanel()
        {
            CurrentHue = hueSlider.value;

            for (int y = 0; y < saturationTexture.height; y++)
            {
                for (int x = 0; x < saturationTexture.width; x++)
                {
                    saturationTexture.SetPixel(x, y, Color.HSVToRGB(CurrentHue, (float)x / saturationTexture.width, (float)16 / saturationTexture.height));
                }
            }
            for (int y = 0; y < valueTexture.height; y++)
            {
                for (int x = 0; x < valueTexture.width; x++)
                {
                    valueTexture.SetPixel(x, y, Color.HSVToRGB(CurrentHue, (float)x / valueTexture.width, (float)y / valueTexture.height));
                }
            }

            valueTexture.Apply();
            saturationTexture.Apply();
            UpdateOutputImage();
        }
    }
}