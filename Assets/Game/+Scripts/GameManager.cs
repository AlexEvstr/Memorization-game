using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public CardController[] easyCards; // Массив карточек для легкого уровня
    public CardController[] mediumCards; // Массив карточек для среднего уровня
    public CardController[] hardCards; // Массив карточек для сложного уровня
    public GameObject target; // Объект, с которым нужно сравнивать карточки
    public Sprite[] targetImages; // Массив спрайтов для target
    public Text timerText; // Текст для отображения таймера
    public GameObject[] lives; // Массив объектов для жизней

    private CardController[] currentCards; // Текущий массив карточек, который будет использоваться
    private List<Sprite> availableSprites;
    private int correctAnswers = 0; // Количество правильных ответов
    private int livesRemaining = 3; // Количество оставшихся жизней
    private float timer; // Таймер для отображения времени
    private int difficultyLevel; // Уровень сложности
    private Sprite targetSprite; // Спрайт, который нужно угадать
    public GameObject[] _levels;
    [SerializeField] private GameObject _readyBtn;
    [SerializeField] private GameObject _pausePopup;
    private PopupEffect _popupEffect;

    [SerializeField] private RectTransform _winPopup;
    [SerializeField] private RectTransform _losePopup;

    [SerializeField] private Text _cardsInLose;
    [SerializeField] private Text _cardsInWin;

    private List<Sprite> usedSprites = new List<Sprite>();

    private bool isGameOver;

    void Start()
    {
        isGameOver = false;
        _popupEffect = GetComponent<PopupEffect>();
        difficultyLevel = PlayerPrefs.GetInt("DifficultyLevel", 0); // Получаем уровень сложности из PlayerPrefs
        SetUpGame();
    }

    void SetUpGame()
    {
        _levels[difficultyLevel].SetActive(true);
        // Устанавливаем таймер в зависимости от уровня сложности
        switch (difficultyLevel)
        {
            case 0: timer = 3f; break; // Легкий уровень
            case 1: timer = 6f; break; // Средний уровень
            case 2: timer = 9f; break; // Сложный уровень
            default: timer = 3f; break;
        }

        // В зависимости от уровня сложности выбираем нужный массив карточек
        switch (difficultyLevel)
        {
            case 0: currentCards = easyCards; break; // Легкий уровень
            case 1: currentCards = mediumCards; break; // Средний уровень
            case 2: currentCards = hardCards; break; // Сложный уровень
        }

        correctAnswers = 0;
        livesRemaining = 3;

        // Устанавливаем видимость жизней
        for (int i = 0; i < lives.Length; i++)
        {
            lives[i].SetActive(i < livesRemaining);
        }

        // Раздаем уникальные изображения на карточках
        AssignSpritesToCards();

        // Запускаем таймер
    }

    void AssignSpritesToCards()
    {
        // Список для доступных спрайтов
        List<Sprite> availableSprites = new List<Sprite>(targetImages);

        // Перемешиваем доступные спрайты
        for (int i = 0; i < availableSprites.Count; i++)
        {
            Sprite temp = availableSprites[i];
            int randomIndex = Random.Range(i, availableSprites.Count);
            availableSprites[i] = availableSprites[randomIndex];
            availableSprites[randomIndex] = temp;
        }

        // Присваиваем изображения карточкам и заполняем список использованных спрайтов
        for (int i = 0; i < currentCards.Length; i++)
        {
            Sprite selectedSprite = availableSprites[i];
            currentCards[i].SetCard(selectedSprite); // Устанавливаем уникальное изображение на карточку
            usedSprites.Add(selectedSprite); // Добавляем это изображение в список использованных
        }

        targetSprite = GetNextTargetSprite();
        target.GetComponent<Image>().sprite = targetSprite;
    }


    IEnumerator TimerCountdown()
    {
        while (timer > 0)
        {
            if (!_pausePopup.activeInHierarchy)
            {
                timer -= Time.deltaTime;
                timerText.text = $"0:0{Mathf.CeilToInt(timer)}";
            }
            yield return null;
        }
        timerText.text = "";

        ShowQuestionMarksOnCards();

        target.SetActive(true);
    }

    void ShowQuestionMarksOnCards()
    {
        foreach (var card in currentCards)
        {
            card.HideAll();
        }

        foreach (var card in currentCards)
        {
            card.ShowQuestionMark(); // Показываем знак вопроса на карточках
        }
    }

    public void CheckAnswer(CardController selectedCard)
    {
        selectedCard.HideAll();
        selectedCard.ShowImage();

        if (selectedCard.GetCardSprite() == targetSprite)
        {
            selectedCard.ShowGreenFrame(); // Показываем зеленую рамку при правильном ответе
            correctAnswers++;
            StartCoroutine(ShowNextTarget());
        }
        else
        {
            selectedCard.ShowRedFrame(); // Показываем красную рамку при неправильном ответе
            DecreaseLife(); // Уменьшаем количество жизней
            usedSprites.Remove(selectedCard.GetCardSprite());
        }
    }

    private IEnumerator ShowNextTarget()
    {
        yield return new WaitForSeconds(0.5f);
        target.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        targetSprite = GetNextTargetSprite();
        target.GetComponent<Image>().sprite = targetSprite;
        if (!isGameOver) target.SetActive(true);
    }

    void DecreaseLife()
    {
        livesRemaining--;
        lives[livesRemaining].SetActive(false); // Отключаем одну жизнь
        if (livesRemaining <= 0)
        {
            StartCoroutine(GameOver()); // Если жизни закончились — игра завершена
        }
    }

    private IEnumerator GameOver()
    {
        isGameOver = true;
        target.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _cardsInLose.text = $"{correctAnswers}/{currentCards.Length}";
        _popupEffect.OpenWindow(_losePopup);
    }

    private IEnumerator Win()
    {
        isGameOver = true;
        target.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _cardsInWin.text = $"{correctAnswers}/{currentCards.Length}";
        _popupEffect.OpenWindow(_winPopup);
    }

    Sprite GetNextTargetSprite()
    {
        if (usedSprites.Count == 0)
        {
            StartCoroutine(Win());
            return null; // Все изображения использованы
        }

        int randomIndex = Random.Range(0, usedSprites.Count); // Выбираем случайный индекс из usedSprites
        Sprite selectedSprite = usedSprites[randomIndex]; // Получаем случайный спрайт для Target
        usedSprites.RemoveAt(randomIndex); // Удаляем этот спрайт из списка, чтобы не использовать его снова
        

        return selectedSprite; // Возвращаем выбранный спрайт
    }




    public void OnReadyButtonPressed()
    {
        StartCoroutine(TimerCountdown());
        target.GetComponent<Image>().sprite = targetSprite; // Устанавливаем спрайт для объекта target

        foreach (var card in currentCards)
        {
            card.ShowImage(); // Показываем уникальное изображение на карточке
        }
        _readyBtn.SetActive(false);
    }
}
