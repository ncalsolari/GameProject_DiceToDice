using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    [SerializeField] private GameInput gameInput;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        gameInput.OnShowDiceAction += GameInput_OnShowDiceAction;
    }



    private void GameInput_OnShowDiceAction(object sender, EventArgs e) {



    }

}
