using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InShowDicesUI : MonoBehaviour {

    [SerializeField] private Button rollDiceButton;
    [SerializeField] private Button returnMainGameButton;


    private void Awake() {

        rollDiceButton.onClick.AddListener(() => {
            Debug.Log("Rolando os Dados!");
            GameInput.Instance.RollDiceInput();

        });

        returnMainGameButton.onClick.AddListener(() => {

            Hide();
            GameInput.Instance.EraseDiceInput();

        });



    }
    private void Start() {

        GameInput.Instance.OnRollDiceActionTransitionScene += GameInput_OnRollDiceActionTransitionScene;
        Hide();
    }

    private void Update() {
        if (Player.Instance.HasDiceHandActive()) { //tirar isso do update, por em uma func e soh executar ela qndo o boolean trocar de valor
            rollDiceButton.interactable = false;
        }
        else {
            rollDiceButton.interactable = true;
        }
    }


    private void GameInput_OnRollDiceActionTransitionScene(object sender, EventArgs e) {
        Show();


    }
    private void SetRollButton(bool value) {

        rollDiceButton.interactable = value;
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
