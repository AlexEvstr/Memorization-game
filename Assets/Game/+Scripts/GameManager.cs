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

    [SerializeField] private Text _levelText;

    private List<Sprite> usedSprites = new List<Sprite>();

    private bool isGameOver;

    private CoinManager _coinManager;

    private GameAudioController _gameAudioController;

    void Start()
    {
        isGameOver = false;
        _popupEffect = GetComponent<PopupEffect>();
        _coinManager = GetComponent<CoinManager>();
        _gameAudioController = GetComponent<GameAudioController>();
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

        switch (difficultyLevel)
        {
            case 0: _levelText.text = "Easy"; break; // Легкий уровень
            case 1: _levelText.text = "Normal"; break; // Средний уровень
            case 2: _levelText.text = "Hard"; break; // Сложный уровень
        }

        correctAnswers = 0;
        livesRemaining = 3;

        // Устанавливаем видимость жизней
        for (int i = 0; i < lives.Length; i++)
        {
            lives[i].SetActive(i < livesRemaining);
        }

        AssignSpritesToCards();

        DisableCards();
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
        float previousTime = 0f;

        while (timer > 0)
        {
            if (!_pausePopup.activeInHierarchy)
            {
                timer -= Time.deltaTime;

                // Проверяем, прошла ли хотя бы одна секунда
                if (Mathf.FloorToInt(timer) != Mathf.FloorToInt(previousTime))
                {
                    // Проигрываем звук каждую секунду
                    _gameAudioController.PlayTimeSound();

                    // Обновляем предыдущую секунду
                    previousTime = timer;
                }

                // Обновляем текст таймера
                timerText.text = $"0:0{Mathf.CeilToInt(timer)}";

            }

            yield return null;
        }

        _gameAudioController.PlayTimesUpSound();
        timerText.text = "";

        ShowQuestionMarksOnCards();

        EnableCards();

        target.SetActive(true);
    }

    private void EnableCards()
    {
        foreach (var card in currentCards)
        {
            card.EnableCardButton();
        }
    }

    private void DisableCards()
    {
        foreach (var card in currentCards)
        {
            card.DisableCardButton();
        }
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
            DisableCards();
            selectedCard.ShowGreenFrame(); // Показываем зеленую рамку при правильном ответе
            correctAnswers++;
            _coinManager.AddRandomCoins();
            StartCoroutine(ShowNextTarget());
            _gameAudioController.PlayTrueSound();
        }
        else
        {
            selectedCard.ShowRedFrame();
            selectedCard.ShowMinucHeart();
            DecreaseLife(); // Уменьшаем количество жизней
            usedSprites.Remove(selectedCard.GetCardSprite());
            _gameAudioController.PlayFailSound();
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
        EnableCards();
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
        DisableCards();
        isGameOver = true;
        target.SetActive(false);
        _coinManager.IncreaseAndSaveTotalCoins();
        yield return new WaitForSeconds(0.5f);
        _cardsInLose.text = $"{correctAnswers}/{currentCards.Length}";
        _popupEffect.OpenWindow(_losePopup);
        _gameAudioController.DisableMusic();
        _gameAudioController.PlayLoseSound();
    }

    private IEnumerator Win()
    {
        DisableCards();
        isGameOver = true;
        target.SetActive(false);
        _coinManager.IncreaseAndSaveTotalCoins();
        yield return new WaitForSeconds(0.5f);
        _cardsInWin.text = $"{correctAnswers}/{currentCards.Length}";
        _popupEffect.OpenWindow(_winPopup);
        _gameAudioController.DisableMusic();
        _gameAudioController.PlayWinSound();
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
