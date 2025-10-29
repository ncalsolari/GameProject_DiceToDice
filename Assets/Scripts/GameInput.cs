using System;
using UnityEngine;

public class GameInput : MonoBehaviour {//Essa classe dispara os eventos dos botoes clicados no jogo estado: InGame (jogo rolando)
    public static GameInput Instance { get; private set; }


    public event EventHandler OnRollDiceActionTransitionScene;
    public event EventHandler OnRemoveDiceActionTransitionScene;
    public event EventHandler OnRollDiceAction;
    public event EventHandler OnShowDiceAction;
    public event EventHandler OnEraseDiceAction;
    public event EventHandler OnRemoveDiceAction;
    public event EventHandler OnRemoveSelectedDiceAction;

    //  public event EventHandler OnRemoveSelectedDiceAction;

    public event EventHandler<OnSelectedDiceToRemoveEventArgs> OnSelectedDiceToRemove;
    public class OnSelectedDiceToRemoveEventArgs : EventArgs {
        public int indexSelectedDice;
    }
    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one GameInput instance");
        }
        Instance = this;
    }

    public void RollDiceInputTransitionScene() {
        OnRollDiceActionTransitionScene?.Invoke(this, EventArgs.Empty);
    }

    public void RollDiceInput() {
        Debug.Log("Rolando os Dados pelo World!");
        OnRollDiceAction?.Invoke(this, EventArgs.Empty);

    }

    public void ShowDiceInput() {
        Debug.Log("Mostrando os Dados pelo World!");
        OnShowDiceAction?.Invoke(this, EventArgs.Empty);

    }

    public void EraseDiceInput() {
        Debug.Log("Apagandos Dados");
        OnEraseDiceAction?.Invoke(this, EventArgs.Empty);

    }

    public void RemoveDiceInputTransitionScene() {
        Debug.Log("Mudando para UI de remover dados");
        OnRemoveDiceActionTransitionScene?.Invoke(this, EventArgs.Empty);

    }

    public void RemoveDiceInput() { //essa func eh soh pra exibicao da pagina de remocao, acho q ta cm redundancia, rever
        Debug.Log("Removendo dados");
        OnRemoveDiceAction?.Invoke(this, EventArgs.Empty);

    }

    public void RemoveSelectedDiceInput() {
        Debug.Log("Removendo dado selecionado");
        OnRemoveSelectedDiceAction?.Invoke(this, EventArgs.Empty);

    }
    public void SelectDiceToRemove(int index) {
        Debug.Log("Selecionando os dados para remover");
        OnSelectedDiceToRemove?.Invoke(this, new OnSelectedDiceToRemoveEventArgs {
            indexSelectedDice = index
        });

    }

    #region BetPanel
    public void BetMinusButton() {

    }

    public void BetPlusButton() {

    }

    public void SelectFaceBet(int index) {

    }


    public void BetLiarButton() {

    }

    public void BetButton() {

    }
    #endregion

}
