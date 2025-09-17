using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    private int roundNumber = 0;
    private bool isPlayerTurn = false;
    private enum GameState {
        StartGame,
        RollDice,
        StartRound,
        PlayerTurn,
        CpuTurn,
        EndTurn,
        Liar,
        EndRound,
        RemoveDice,
        EndGame,
        Exit, //sair do game e voltar pro menu inicial

    }

    private GameState gameState;










    [SerializeField] private GameInput gameInput;

    private void Awake() {
        Instance = this;

        gameState = GameState.StartGame;
    }

    private void Start() {
        GameInput.Instance.OnShowDiceAction += GameInput_OnShowDiceAction;
        ChangeGameState(gameState);
    }

    private void Update() {
        switch (gameState) {
            case GameState.PlayerTurn:
                //espera input do player
                //EndTurn();
                break;
        }
    }

    private void ChangeGameState(GameState newState) {

        gameState = newState;

        switch (newState) {

            case GameState.StartGame:
                roundNumber = 1;
                ChangeGameState(GameState.StartRound);
                break;
            case GameState.StartRound:
                SetupRound();
                ChangeGameState(GameState.RollDice);
                break;
            case GameState.RollDice:
                // aqui soh ativa UI da mao
                //o resto daqui pra baixo eh tudo fora deste switch case
                //espero rolar os dados
                //disparar evento quando dados pararem
                //muda estado para playerturn ou cpu turn
                break;

            case GameState.CpuTurn:
                StartCoroutine(CpuTurnRoutine());
                break;

            case GameState.EndTurn:
                //checo se a ultima jogada foi mentira ou aposta
                // se mentira chamo o state LiarCall 
                //se aposta chamo player turn ou cpu turn, depende d quem eh a vez
                break;
            case GameState.Liar:

                //tratativas de perda de dado

                //se player perdeu dado vou pro ui de remover dado
                //chamo endround
                break;
            case GameState.RemoveDice:
                //ativa ui de remocao dedados
                //o resto daqui pra baixo eh tudo fora deste switch case
                //espero remover o dado
                //disparar evento quando dados for removido
                //muda estado para endround
                break;

            case GameState.EndRound:
                roundNumber++;
                if (CheckGameOver())
                    ChangeGameState(GameState.EndGame);
                else
                    ChangeGameState(GameState.StartRound);
                break;


            case GameState.EndGame:
                Debug.Log("Fim de Jogo");
                break;
        }


    }

    private void SetupRound() {
        Debug.Log("Rodada " + roundNumber + " começou!");

        //tratativas de inicio de rodada
        //mao dos cpus
        //de quem eh a vez
        //reseto dados da mesa

    }

    private IEnumerator CpuTurnRoutine() {

        Debug.Log("CPU pensando...");
        yield return new WaitForSeconds(2f);

        //IA

        EndTurn();

    }

    private void EndTurn() {
        if (isPlayerTurn) {
            isPlayerTurn = false;
            ChangeGameState(GameState.EndTurn);
        }
        else {
            isPlayerTurn = true;
            ChangeGameState(GameState.EndTurn);
        }
    }


    private bool CheckGameOver() {
        return false;
    }


    private void GameInput_OnShowDiceAction(object sender, EventArgs e) {



    }

}
