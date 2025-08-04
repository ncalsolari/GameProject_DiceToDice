using System;
using UnityEngine;

public class GameInput : MonoBehaviour {//Essa classe dispara os eventos dos botoes clicados no jogo estado: InGame (jogo rolando)
    public static GameInput Instance { get; private set; }

    public event EventHandler OnRollDiceAction;

    private void Awake() {
        Instance = this;
    }

    public void RollDiceInput() {
        Debug.Log("Rolando os Dados pelo World!");
        OnRollDiceAction?.Invoke(this, EventArgs.Empty);

    }
}
