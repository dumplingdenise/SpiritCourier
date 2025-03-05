using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryPuzzle : MonoBehaviour
{
    [System.Serializable]
    public class Card // Store the card and the front image
    {
        public Button cardButton; // The button (card base)
        public Image frontImage;  // The image component inside the button (front face)
    }

    public Card[] cards; // Array of all cards
    private Dictionary<Button, Image> cardPairs = new Dictionary<Button, Image>(); // Store button-image pairs
    public GameObject puzzleCompletePanel; // UI pop-up when puzzle is done

    private Button firstCard, secondCard; //store two clicked cards
    private int matchedPairs = 0; //tracks how many pairs are found
    private bool isFlipping = false;  //prevent clicking multiple cards too quickly

    // public Sprite[] possibleImages; // Drag your possible images here in the Inspector

    void Start()
    {
        GameController gameController = FindAnyObjectByType<GameController>(); // Find the GameController
        if (gameController != null)
        {
            gameController.SetGameState(GameState.Puzzle); // Set the state to FreeRoam when exiting the puzzle
        }
        InitializeCards();
        puzzleCompletePanel.SetActive(false); // Show pop-up when puzzle is done
    }

    void InitializeCards()
    {
         List<Card> shuffledCards = new List<Card>(cards); //convert array to list
          shuffledCards = ShuffleList(shuffledCards); //shuffle cards

           foreach (Card card in shuffledCards)
           {
               cardPairs[card.cardButton] = card.frontImage; // Store the button-image pair
               card.frontImage.gameObject.SetActive(false); // Hide front initially
               card.cardButton.onClick.AddListener(() => FlipCard(card.cardButton)); // Click event
           } 

        /* List<Card> shuffledCards = new List<Card>(cards);
         shuffledCards = ShuffleList(shuffledCards);

         List<Sprite> selectedImages = GetRandomPairs(shuffledCards.Count / 2); // Get shuffled image pairs

         int index = 0;
         foreach (Card card in shuffledCards)
         {
             cardPairs[card.cardButton] = card.frontImage;
             card.frontImage.sprite = selectedImages[index]; // Assign random image
             card.frontImage.gameObject.SetActive(false);
             card.cardButton.onClick.AddListener(() => FlipCard(card.cardButton));
             index++;
         }
     }

     List<Sprite> GetRandomPairs(int pairCount)
     {
         List<Sprite> selected = new List<Sprite>();
         List<Sprite> availableImages = new List<Sprite>(possibleImages);

         for (int i = 0; i < pairCount; i++)
         {
             int randomIndex = Random.Range(0, availableImages.Count);
             Sprite selectedSprite = availableImages[randomIndex];
             availableImages.RemoveAt(randomIndex);

             selected.Add(selectedSprite); // First copy
             selected.Add(selectedSprite); // Second copy (for matching pair)
         }

         return ShuffleList(selected); */
    }



    void FlipCard(Button clickedButton)
    {
        if (isFlipping || cardPairs[clickedButton].gameObject.activeSelf) return;

        cardPairs[clickedButton].gameObject.SetActive(true); // Show front image

        if (firstCard == null)
        {
            firstCard = clickedButton;
        }
        else
        {
            secondCard = clickedButton;
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        isFlipping = true;
        yield return new WaitForSeconds(0.5f);

        if (cardPairs[firstCard].sprite == cardPairs[secondCard].sprite) // Match check
        {
            matchedPairs++;

            //disable the matched card button, so they cant be clicked
            firstCard.interactable = false;
            secondCard.interactable = false;

            if (matchedPairs == cards.Length / 2)
            {
                Debug.Log("Puzzle Completed!");
                puzzleCompletePanel.SetActive(true); // Show pop-up when puzzle is done
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f); // Wait before flipping back
            cardPairs[firstCard].gameObject.SetActive(false);
            cardPairs[secondCard].gameObject.SetActive(false);
        }

        firstCard = null;
        secondCard = null;
        isFlipping = false;
    }

    List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]); // Swap elements
        }
        return list;
    }


}