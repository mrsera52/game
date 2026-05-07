using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoordinateInput : MonoBehaviour
{
    [Header("Поля ввода")]
    public TMP_InputField inputX;
    public TMP_InputField inputY;
    public TMP_InputField inputZ;

    [Header("Кнопка стрельбы")]
    public Button fireButton;

    [Header("Статус")]
    public TextMeshProUGUI statusText;

    [Header("Пушка")]
    public CannonFireByCoords cannon;

    void Start()
    {
        if (fireButton != null)
            fireButton.onClick.AddListener(OnFirePressed);
    }

    void OnFirePressed()
    {
        if (!float.TryParse(inputX.text.Replace(',', '.'),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float x) ||
            !float.TryParse(inputY.text.Replace(',', '.'),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float y) ||
            !float.TryParse(inputZ.text.Replace(',', '.'),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float z))
        {
            SetStatus("Ошибка: введите числа во все три поля", Color.red);
            return;
        }

        Vector3 target = new Vector3(x, y, z);
        cannon.AimAndFire(target, OnResult);
    }

    void OnResult(string message, bool success)
    {
        SetStatus(message, success ? Color.green : Color.red);
    }

    void SetStatus(string text, Color color)
    {
        if (statusText != null)
        {
            statusText.text = text;
            statusText.color = color;
        }
    }
}