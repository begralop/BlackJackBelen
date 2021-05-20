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
    public Button apuesta10Button;
    public Button apuesta100Button;
    public Text BancaMessage;
    public Text apuestaMessage;
    public Text finalMessage;
    public Text probMessage;
    public Text probMessage1;
    public Text probMessage2;


    public int[] values = new int[52]; // Inicializamos en 22 
    int cardIndex = 0;

    int valuesPlayer = 0;
    int valuesDealer = 0;
    int[] cardsPlayer = new int[50];
    int[] cardsDealer = new int[50];
    int round = 0;

    int banca = 1000; // Inicializamos la banca en 100
    int apuesta = 0; // Inicializamos la apuesta en 0

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
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */

        int aux = 0; 

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                values[aux] = j + 1;
                aux++;
            }
        }

        
    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */

        int rndNum; // variable para el num random
        Sprite tempFaces;
        int tempValues;

        for (int i = 0; i < 52; i++) // bucle que recorre las 52 cartas
        {
            rndNum = Random.Range(0, 52); // num random
            tempFaces = faces[0]; // tempFaces valdrá lo q valga el primer valor del array faces
            faces[0] = faces[rndNum]; // el valor valdrá un rndnum
            faces[rndNum] = tempFaces; // ese eleemento valdrá tempFaces
            tempValues = values[0]; // tempvalues valdrá el valor de values primera cas
            values[0] = values[rndNum]; // que será un numRandom
            values[rndNum] = tempValues; // y ese valdrá tempValues
        }

        
    }

    void StartGame()
    {

        apuesta = 0; // volvemos a inicializar la apuesta en 0 cada vez que se vuelva a jugar
        actualizarBanca(); // actualizamos la banca

        for (int i = 0; i < 2; i++)
        {
            PushDealer();
            PushPlayer();
            round++;

        }

        /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */

        if (valuesPlayer == 21) // si el valor de las cartas del jugador vale 21
        {
            finalMessage.text = "Blacjack! Ganaste"; // gana el jugador
            stickButton.interactable = false; // inhabilitamos los dos botones
            hitButton.interactable = false;
            banca += apuesta * 2; // la banca será la apuesta por 2
            apuesta = 0; // apuesta vale 0
            actualizarBanca(); // y actualizamos banca
        }
        else if (valuesDealer == 21) // si el valor de las cartas del dealer vale 21
        {
            finalMessage.text = "Blacjack! Perdiste"; // pierdes
            stickButton.interactable = false; // inhabilitamos los dos botones
            hitButton.interactable = false;
            banca += 0; // a la banca se le suma 0
            apuesta = 0; // apuesta vale 0
            actualizarBanca(); // y actualizamos banca
        }
        if (valuesPlayer > 21) // si el valor de las cartas del jugador vale más de 21
        {
            finalMessage.text = "Te pasaste, perdiste"; // pierdes
            stickButton.interactable = false; // inhabilitamos los dos botones
            hitButton.interactable = false;
            banca += 0; // a la banca se le suma 0
            apuesta = 0; // apuesta vale 0
            actualizarBanca(); // y actualizamos banca
        }
        else if (valuesDealer > 21) // si el valor de las cartas del dealer vale más de 21
        {
            finalMessage.text = "El dealer se pasó, ganaste"; // ganas
            stickButton.interactable = false; // inhabilitamos los dos botones
            hitButton.interactable = false;
            banca += apuesta * 2; // la banca será la apuesta por 2
            apuesta = 0; // apuesta vale 0
            actualizarBanca(); // y actualizamos banca
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

        float probabilidad; // variable float de la probabilidad
        int casosPosibles; // variable de casos posibles

        // Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador

        if (round != 0) // si la ronda es diferente de 0
        {
            int valoresVisiblesDealer = valuesDealer - cardsDealer[0]; // variable valoresVisDea valdrá valuesDealer menos la posición 1 de sus cartas
            casosPosibles = 13 - valuesPlayer + valoresVisiblesDealer; // casos posibles será 13 menos valuesPlayes más la anterior variables
            probabilidad = casosPosibles / 13f; // la probabilidad son los caso posibles dividido 13f

            if (probabilidad > 1) // si la probabilidad es mayor que 1
            {
                probabilidad = 1; // valdrá 1 
            }
            else if (probabilidad < 0) // también si es menos que 0
            {
                probabilidad = 0; // valdrá 0
            }

            probMessage.text = (probabilidad * 100).ToString() + " %"; // el texto de probabilidad será la probabilidad por 100 a string

        }

        //Probabilidad de que el jugador obtenga más de 21 si pide una carta

        float probabilidad2; // variable prob2
        int casosPosibles2; // variable casos posibles 2
        casosPosibles2 = 13 - (21 - valuesPlayer); // esta valdrá 13 menos (21 menos el valor del jugador)
        probabilidad2 = casosPosibles2 / 13f; // la prob valdrá casos posibles dividido 13f

        if (probabilidad2 > 1) // si la probabilidad es mayor que 1
        {
            probabilidad2 = 1; // valdrá 1 
        }
        else if (probabilidad2 < 0) // también si la probabilidad es menor que 0
        {
            probabilidad2 = 0; // valdrá 0
        }

        probMessage1.text = (probabilidad2 * 100).ToString() + " %"; // mostramos la probabilidad


        // Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta

        float probabilidadLlegarA17; // variable para la probabilidad de llegar a 17
        int casosPosiblesHasta17; // variable para casps posible
        casosPosiblesHasta17 = 13 - (16 - valuesPlayer); // casos posibles valdrá 13 menos (16 menos el valuesPlayer)
        probabilidadLlegarA17 = casosPosiblesHasta17 / 13f; // probabilidad de llegar a 17 valdrá los casos partido 13f

        if (probabilidadLlegarA17 > 1) // si la probabilidad es mayor que 1
        {
            probabilidadLlegarA17 = 1; // valdrá 1 
        }
        else if (probabilidadLlegarA17 < 0) // también si la probabilidad es menor que 0
        {
            probabilidadLlegarA17 = 0; // valdrá 0
        }

        float probabilidadEntre17y21 = probabilidadLlegarA17 - probabilidad2; // probabilidad entre 17 y 21, prob de llegar a 17 menos la de 21

        if (probabilidadEntre17y21 > 1) // si la probabilidad es mayor que 1
        {
            probabilidadEntre17y21 = 1; // valdrá 1 
        }
        else if (probabilidadEntre17y21 < 0) // también si la probabilidad es menor que 0
        {
            probabilidadEntre17y21 = 0; // valdrá 0
        }

        probMessage2.text = (probabilidadEntre17y21 * 100).ToString() + " %"; // mostramos la probabilidad

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

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */

        if (valuesPlayer > 21) // si el valor del jugador es mayor que 21
        {
            finalMessage.text = "Te pasaste, perdiste"; // pierdes
            stickButton.interactable = false; // inhabilitamos los botones
            hitButton.interactable = false;
            banca += 0; // la banca vale 0
            apuesta = 0; // la apuesta vale 0
            actualizarBanca(); // actualizamos la banca

        }

        /*TODO:
       * Comprobamos si el jugador ya ha perdido y mostramos mensaje
       */

        else if (valuesPlayer == 21) // también si el valor del jugador vale 21
        {
            finalMessage.text = "Blacjack! Ganaste"; // ganas
            stickButton.interactable = false; // inhabilitamos los botones
            hitButton.interactable = false;
            apuesta100Button.interactable = false;
            apuesta10Button.interactable = false;
            banca += apuesta * 2; // la banca vale la apuesta por 2
            apuesta = 0; // la apuesta vale 0
            actualizarBanca(); // actualizamos la banca
        }
      

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        hitButton.interactable = false; // inhabilitamos los botones
        apuesta100Button.interactable = false;
        apuesta10Button.interactable = false;
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);

        /*TODO:
       * Repartimos cartas al dealer si tiene 16 puntos o menos
       * El dealer se planta al obtener 17 puntos o más
       * Mostramos el mensaje del que ha ganado
       */

        while (valuesDealer <= 16) // mientras el valor del dealer valga 16 o menos
        {
            PushDealer(); // llamamos a Pushdealer
        }

        if (valuesDealer == 21) // si el valor del dealer es 21
        {
            finalMessage.text = "Blacjack! Perdiste"; // pierdes
            banca += 0; // la banca vale 0
            apuesta = 0; // la apuesta vale 0
            actualizarBanca(); // actualizamos la banca
        }
        else if (valuesDealer > 21) // si el valor del dealer es mayor que 21
        {
            finalMessage.text = "El dealer se pasó, ganaste";  // ganas
            banca += apuesta * 2; // la banca vale la apuesta por 2
            apuesta = 0; // la apuesta vale 0
            actualizarBanca(); // actualizamos la banca
        }
        else if (valuesDealer < valuesPlayer) // si el valor del dealer es menor que el del jugador
        {
            finalMessage.text = "Ganaste"; // ganas
            banca += apuesta * 2; // la banca vale la apuesta por 2
            apuesta = 0; // la apuesta vale 0
            actualizarBanca(); // actualizamos la banca
        } 
        else if (valuesDealer == valuesPlayer) // si el valor del dealer es igual que el del jugador
        {
            finalMessage.text = "Empataste"; // empatas
            banca += apuesta; // la banca vale 0
            apuesta = 0; // la apuesta vale 0
            actualizarBanca(); // actualizamos la banca
        }
        else // sino
        {
            finalMessage.text = "Perdiste"; // pierdes
            banca += apuesta; // la banca vale 0
            apuesta = 0; // la apuesta vale 0
            actualizarBanca(); // actualizamos la banca
        }

        stickButton.interactable = false; // inhabilitamos botón
     
    }

    public void PlayAgain()
    {
        hitButton.interactable = true; // habilitamos los botones
        stickButton.interactable = true;
        apuesta100Button.interactable = true;
        apuesta10Button.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();
        cardIndex = 0;
        valuesPlayer = 0;
        round = 0;
        valuesDealer = 0;
        ShuffleCards(); // mezclamos
        StartGame(); // empezamos juego
    }

    public void apostar10()
    {
        if (banca > 10) // si la banca es mayor que 10
        {
            apuesta += 10; // se le suma 10 a la apuesta
            banca -= 10; // se le resta 10 a la banca
            actualizarBanca(); // actualizamos la banca
        }
    }
    public void Apostar100()
    {
        if (banca > 100) // si la banca es mayor que 100
        {
            apuesta += 100; // se le suma 100 a la apuesta
            banca -= 100; // se le resta 100 a la banca
            actualizarBanca(); // actualizamos la banca
        }
    }

    private void actualizarBanca()
    {
        apuestaMessage.text = apuesta.ToString(); // mostramos el valor de la apuesta
        BancaMessage.text = banca.ToString(); // mostramos el valor de la banca
    }

}
