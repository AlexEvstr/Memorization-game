using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    private GameObject greenFrame;
    private GameObject redFrame;
    private GameObject questionMark;
    private GameObject image;
    private GameObject _minusHeart;

    private Sprite cardSprite;

    private Button _cardButton;

    void Awake()
    {
        _cardButton = GetComponent<Button>();
        // Получаем дочерние объекты по индексам
        greenFrame = transform.GetChild(0).gameObject;
        redFrame = transform.GetChild(1).gameObject;
        questionMark = transform.GetChild(2).gameObject;
        image = transform.GetChild(3).gameObject;
        _minusHeart = redFrame.transform.GetChild(0).gameObject;

        // Прячем все дочерние элементы по умолчанию
        HideAll();
    }

    public void HideAll()
    {
        greenFrame.SetActive(false);
        redFrame.SetActive(false);
        questionMark.SetActive(false);
        image.SetActive(false);
    }

    public void SetCard(Sprite sprite)
    {
        cardSprite = sprite;
        image.GetComponent<Image>().sprite = sprite; // Устанавливаем уникальное изображение
    }

    public void ShowGreenFrame()
    {
        greenFrame.SetActive(true);
    }

    public void ShowRedFrame()
    {
        redFrame.SetActive(true);
    }

    public void ShowQuestionMark()
    {
        questionMark.SetActive(true);
    }

    public void ShowImage()
    {
        image.SetActive(true);
    }

    public Sprite GetCardSprite()
    {
        return cardSprite;
    }

    public void DisableCardButton()
    {
        _cardButton.interactable = false;
    }

    public void EnableCardButton()
    {
        _cardButton.interactable = true;
    }

    public void ShowMinucHeart()
    {
        StartCoroutine(ShowAndCloseHeart());
    }

    private IEnumerator ShowAndCloseHeart()
    {
        _minusHeart.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        _minusHeart.SetActive(false);
    }
}
