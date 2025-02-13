using UnityEngine;
using UnityEngine.UI;

public class SkinShopManager : MonoBehaviour
{
    public int[] skinPrices = {}; // Цены скинов
    public Button[] skinButtons; // Кнопки покупки/выбора скинов
    public Image[] buttonImages; // Изображения кнопок (для смены спрайта при выборе)
    public Sprite normalSprite; // Обычный спрайт кнопки
    public Sprite selectedSprite; // Зелёный спрайт кнопки (если скин выбран)

    public GameObject[] coinIcons; // Иконки монет у скинов
    public GameObject[] priceTexts; // Тексты с ценой у скинов
    public GameObject[] useTexts; // Тексты "Use" у купленных скинов

    public Text balanceText1; // Отображение баланса (дубликат)
    public Text balanceText2; // Отображение баланса (дубликат)

    private int totalCoins;
    private int selectedSkinIndex;

    private SettingsController _SettingsController;

    void Start()
    {
        totalCoins = PlayerPrefs.GetInt("TotalGameCoins", 0); // Загружаем баланс
        selectedSkinIndex = PlayerPrefs.GetInt("SelectedSkin", 6); // Загружаем выбранный скин (-1 если нет)

        UpdateUI(); // Обновляем интерфейс магазина
        _SettingsController = GetComponent<SettingsController>();
    }

    void UpdateUI()
    {
        // Обновляем отображение баланса
        balanceText1.text = totalCoins.ToString();
        balanceText2.text = totalCoins.ToString();

        for (int i = 0; i < skinButtons.Length; i++)
        {
            bool isPurchased = PlayerPrefs.GetInt("SkinPurchased_" + i, 0) == 1;

            if (isPurchased)
            {
                // Если скин куплен, скрываем монету и цену, показываем "Use"
                coinIcons[i].SetActive(false);
                priceTexts[i].SetActive(false);
                useTexts[i].SetActive(true);
            }
            else
            {
                // Если не хватает монет, делаем кнопку неактивной и затемняем элементы
                if (totalCoins < skinPrices[i])
                {
                    skinButtons[i].interactable = false; // Выключаем кнопку

                    // Делаем серой монетку (меняем цвет Image)
                    coinIcons[i].GetComponent<Image>().color = Color.gray;

                    // Делаем серым текст с ценой
                    priceTexts[i].GetComponent<Text>().color = Color.gray;
                }
            }

            // Если это выбранный скин, меняем изображение кнопки на зелёное
            buttonImages[i].sprite = (i == selectedSkinIndex) ? selectedSprite : normalSprite;
        }
    }


    public void BuyOrSelectSkin(int index)
    {
        bool isPurchased = PlayerPrefs.GetInt("SkinPurchased_" + index, 0) == 1;

        if (isPurchased)
        {
            // Если скин уже куплен, просто выбираем его
            SelectSkin(index);
            _SettingsController.PlayClickSound();
        }
        else if (totalCoins >= skinPrices[index])
        {
            // Если скин не куплен, но хватает денег - покупаем и выбираем его
            totalCoins -= skinPrices[index]; // Вычитаем цену из баланса
            PlayerPrefs.SetInt("TotalGameCoins", totalCoins); // Сохраняем новый баланс
            PlayerPrefs.SetInt("SkinPurchased_" + index, 1); // Помечаем скин как купленный
            SelectSkin(index); // Автоматически выбираем купленный скин
            _SettingsController.PlayBuySound();
        }
        else
        {
            Debug.Log("Недостаточно монет!"); // Вывод в консоль, можно заменить на UI-сообщение
        }

        UpdateUI(); // Обновляем интерфейс
    }

    private void SelectSkin(int index)
    {
        PlayerPrefs.SetInt("SelectedSkin", index); // Сохраняем выбранный скин
        selectedSkinIndex = index;

        // Обновляем UI, чтобы показать, что скин выбран
        for (int i = 0; i < buttonImages.Length; i++)
        {
            buttonImages[i].sprite = (i == selectedSkinIndex) ? selectedSprite : normalSprite;
        }
    }
}
