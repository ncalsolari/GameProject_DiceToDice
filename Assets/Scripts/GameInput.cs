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

    private void Awake() {
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

    public void RemoveDiceInput() {
        Debug.Log("Removendo dados");
        OnRemoveDiceAction?.Invoke(this, EventArgs.Empty);

    }
}
