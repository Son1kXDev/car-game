using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Game.Scripts.UI;
using System.IO;

public class StickerUploader : MonoBehaviour
{
    [SerializeField] RawImage _preview;

    private Texture2D _texture;
    private MenuCar _car;

    private void Awake() => _car = FindFirstObjectByType<MenuCar>();
    private void OnEnable() => ApplyButton(false);
    public void LoadTexture() => FileManager.Instance.LoadFile(LoadTexture, ApplyButton);
    private void LoadTexture(string path)
    {
        _preview.texture = GetSprite(path).texture;
        _car.ChangeCarSticker(GetSprite(path));
    }

    public static Sprite GetSprite(string path)
    {
        Texture2D texture = new Texture2D(2, 2);

        if (path == null || path == string.Empty)
            texture = Resources.Load<Texture2D>("Empty");
        else
        {
            var bytes = File.ReadAllBytes(path);
            texture.LoadImage(bytes);
        }

        Sprite fromTexture = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1500f);

        return fromTexture;
    }

    public void ApplyButton(bool value)
    {
        Image applyButton = transform.Find("ButtonApply").GetComponent<Image>();
        TMPro.TextMeshProUGUI applyButtonText = applyButton.transform.Find("Lable").GetComponent<TMPro.TextMeshProUGUI>();

        TMPro.TextMeshProUGUI costText = transform.Find("CostText").GetComponent<TMPro.TextMeshProUGUI>();

        applyButton.enabled = value;
        applyButtonText.enabled = value;
        costText.enabled = value;
    }
}