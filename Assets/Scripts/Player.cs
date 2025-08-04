using System;
using UnityEngine;



public class Player : MonoBehaviour // Essa classe guarda as infos do player e lida com acoes do mesmo
{

    const int DICE_QNT = 5;
    public static Player Instance { get; private set; }


    [SerializeField] private GameInput gameInput;

    private int[] diceQnt = new int[6]; //o valor de diceQnt[0] significa quantos dados cm face 1 eu tenho, diceQnt[1] face 2 e ...
    private int[] diceValue = new int[DICE_QNT]; //o valor de diceValue[0] significa a face do meu dado 1 (de um dos meus dados)


    private void Awake() {

        if (Instance != null) {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }
    private void Start() {

        gameInput.OnRollDiceAction += GameInput_OnRollDiceAction;

        ResetHand();
    }


    private void GameInput_OnRollDiceAction(object sender, EventArgs e) {
        ResetHand();
        RollDiceHand();
        for (int i = 0; i < DICE_QNT; i++) {
            Debug.Log("face do dado " + (i + 1) + ": " + diceValue[i] + "\n");
        }
        Debug.Log("\n\n\n");
        for (int i = 0; i < 6; i++) {
            Debug.Log("qnt de faces " + (i + 1) + ": " + diceQnt[i] + "\n");
        }



    }


    private void RollDiceHand() {

        for (int i = 0; i < DICE_QNT; i++) {

            if (diceValue[i] >= 0) { //se o valor for -1 eu perdi este dado, nao atribuo valor a ele
                int rand = UnityEngine.Random.Range(1, 7); //jogo dado
                diceValue[i] = rand; //face do dado
                diceQnt[rand - 1] += 1; // qntdade de dados com essa face
            }

        }

    }

    private void ResetHand() {

        for (int i = 0; i < DICE_QNT; i++) {
            diceValue[i] = 0; // atribuir valor -1 para quando vc eliminar o dado da mao
        }

        for (int i = 0; i < 6; i++) {
            diceQnt[i] = 0;
        }
    }


}
