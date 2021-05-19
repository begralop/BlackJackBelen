using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
    public Text probMessage1;
    public Text probMessage2;

    public int[] values = new int[52];
    int cardIndex = 0;

    int valuesPlayer = 0;
    int valuesDealer = 0;
    int[] cardsPlayer = new int[50];
    int[] cardsDealer = new int[50];
    int round = 0;

    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        int aux = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                values[aux] = j + 1;
                aux++;
            }
        }

        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */
    }

    private void ShuffleCards()
    {
        int rndNum;
        Sprite tempFaces;
        int tempValues;
        for (int i = 0; i < 52; i++)
        {
            rndNum = Random.Range(0, 52);
            tempFaces = faces[0];
            faces[0] = faces[rndNum];
            faces[rndNum] = tempFaces;
            tempValues = values[0];
            values[0] = values[rndNum];
            values[rndNum] = tempValues;
        }

        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */
    }

    void StartGame()
    {

        for (int i = 0; i < 2; i++)
        {
            PushDealer();
            PushPlayer();
            round++;

        }
        /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
        if (valuesPlayer == 21)
        {
            finalMessage.text = "Blacjack! Ganaste";
            stickButton.interactable = false;
            hitButton.interactable = false;
        }
        else if (valuesDealer == 21)
        {
            finalMessage.text = "Blacjack! Perdiste";
            stickButton.interactable = false;
            hitButton.interactable = false;
        }
        if (valuesPlayer > 21)
        {
            finalMessage.text = "Te pasaste, perdiste";
            stickButton.interactable = false;
            hitButton.interactable = false;
        }
        else if (valuesDealer > 21)
        {
            finalMessage.text = "El dealer se pasó, ganaste";
            stickButton.interactable = false;
            hitButton.interactable = false;
        }
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */

        float probabilidad;
        int casosPosibles;
        //- Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
        if (round != 0)
        {
            int valoresVisiblesDealer = valuesDealer - cardsDealer[0];
            casosPosibles = 13 - valuesPlayer + valoresVisiblesDealer;
            probabilidad = casosPosibles / 13f;
            if (probabilidad > 1)
            {
                probabilidad = 1;
            }
            else if (probabilidad < 0)
            {
                probabilidad = 0;
            }

            probMessage.text = (probabilidad * 100).ToString() + " %";

        }

        //Probabilidad de que el jugador obtenga más de 21 si pide una carta

        float probabilidad2;
        int casosPosibles2;
        casosPosibles2 = 13 - (21 - valuesPlayer);
        probabilidad2 = casosPosibles2 / 13f;
        if (probabilidad2 > 1)
        {
            probabilidad2 = 1;
        }
        else if (probabilidad2 < 0)
        {
            probabilidad2 = 0;
        }
        probMessage1.text = (probabilidad2 * 100).ToString() + " %";

        // Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta

        float probabilidadLlegarA17;
        int casosPosiblesHasta17;
        casosPosiblesHasta17 = 13 - (16 - valuesPlayer);
        probabilidadLlegarA17 = casosPosiblesHasta17 / 13f;
        if (probabilidadLlegarA17 > 1)
        {
            probabilidadLlegarA17 = 1;
        }
        else if (probabilidadLlegarA17 < 0)
        {
            probabilidadLlegarA17 = 0;
        }

        float probabilidadEntre17y21 = probabilidadLlegarA17 - probabilidad2;
        if (probabilidadEntre17y21 > 1)
        {
            probabilidadEntre17y21 = 1;
        }
        else if (probabilidadEntre17y21 < 0)
        {
            probabilidadEntre17y21 = 0;
        }

        probMessage2.text = (probabilidadEntre17y21 * 100).ToString() + " %";

    }

    void PushDealer()
    {
        dealer.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]);
        valuesDealer += values[cardIndex];
        cardsDealer[round] = values[cardIndex];
        cardIndex++;
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        valuesPlayer += values[cardIndex];
        cardsPlayer[round] = values[cardIndex];
        cardIndex++;

        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();

        if (valuesPlayer > 21)
        {
            finalMessage.text = "Te pasaste, perdiste";
            stickButton.interactable = false;
            hitButton.interactable = false;
       
        }
        else if (valuesPlayer == 21)
        {
            finalMessage.text = "Blacjack! Ganaste";
            stickButton.interactable = false;
            hitButton.interactable = false;
        }
        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        hitButton.interactable = false;
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);

        while (valuesDealer <= 16)
        {
            PushDealer();
        }

        if (valuesDealer == 21)
        {
            finalMessage.text = "Blacjack! Perdiste";
        }
        else if (valuesDealer > 21)
        {
            finalMessage.text = "El dealer se pasó, ganaste";
        }
        else if (valuesDealer < valuesPlayer)
        {
            finalMessage.text = "Ganaste";
        }
        else if (valuesDealer == valuesPlayer)
        {
            finalMessage.text = "Empataste";
        }
        else
        {
            finalMessage.text = "Perdiste";
        }

        stickButton.interactable = false;

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */

    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();
        cardIndex = 0;
        valuesPlayer = 0;
        round = 0;
        valuesDealer = 0;
        ShuffleCards();
        StartGame();
    }
    
}
