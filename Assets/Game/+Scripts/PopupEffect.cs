using UnityEngine;

public class PopupEffect : MonoBehaviour
{
    private float duration = 0.25f; // Время анимации
    private Vector3 targetScale = new Vector3(1, 1, 1); // Окончательный размер окна
    private float initialScaleMultiplier = 0.1f; // Начальный масштаб
    private bool startOnAwake = true; // Флаг для старта анимации при старте сцены

    void Awake()
    {
        if (startOnAwake)
        {
            // Вызываем метод с передачей нужного RectTransform
        }
    }

    // Метод для запуска анимации, теперь принимаем объект окна как параметр
    public void OpenWindow(RectTransform windowToOpen)
    {
        // Включаем окно перед анимацией
        windowToOpen.gameObject.SetActive(true);

        // Стартуем анимацию
        StartCoroutine(ScaleAndFadeIn(windowToOpen));
    }

    private System.Collections.IEnumerator ScaleAndFadeIn(RectTransform windowToOpen)
    {
        float timeElapsed = 0f;

        // Плавное увеличение масштаба
        while (timeElapsed < duration)
        {
            float scale = Mathf.Lerp(initialScaleMultiplier, targetScale.x, timeElapsed / duration); // Линейное увеличение масштаба
            windowToOpen.localScale = new Vector3(scale, scale, scale);

            timeElapsed += Time.deltaTime; // Обновляем время
            yield return null;
        }

        // Убедитесь, что после завершения анимации масштаб установлен точно
        windowToOpen.localScale = targetScale;
    }

    // Метод для закрытия окна с эффектом (если нужно)
    public void CloseWindow(RectTransform windowToClose)
    {
        StartCoroutine(ScaleAndFadeOut(windowToClose));
    }

    private System.Collections.IEnumerator ScaleAndFadeOut(RectTransform windowToClose)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float scale = Mathf.Lerp(targetScale.x, initialScaleMultiplier, timeElapsed / duration); // Линейное уменьшение масштаба
            windowToClose.localScale = new Vector3(scale, scale, scale);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // После завершения анимации выключаем объект
        windowToClose.localScale = new Vector3(initialScaleMultiplier, initialScaleMultiplier, initialScaleMultiplier);
        windowToClose.gameObject.SetActive(false);
    }
}
