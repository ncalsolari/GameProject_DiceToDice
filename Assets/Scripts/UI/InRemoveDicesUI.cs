using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InRemoveDicesUI : MonoBehaviour {


    [SerializeField] private Button removeDiceButton;
    [SerializeField] private Button returnMainGameButton;
    [SerializeField] private Button selectDice1;
    [SerializeField] private Button selectDice2;
    [SerializeField] private Button selectDice3;
    [SerializeField] private Button selectDice4;
    [SerializeField] private Button selectDice5;


    private void Awake() {

        returnMainGameButton.onClick.AddListener(() => {

            Hide();
            DealerRemoveDice.Instance.EraseDices(); //aqui talvez eu deva refatorar pra essa classe se comunicar com o GameInput e nao direto cm o dealer, 
                                                    // tipo a func GameInput_OnRemoveDiceActionTransitionScene ali embaixo
                                                    //aqui ele soh apaga os prefabs da Scene, talvez eu tenha que estruturar melhor essa logica
        });

        removeDiceButton.onClick.AddListener(() => {
            Debug.Log("Apagando dado selecionado!");
            GameInput.Instance.RemoveSelectedDiceInput();


        });


        selectDice1.onClick.AddListener(() => {
            Debug.Log("Selecionando dado 1!");
            GameInput.Instance.SelectDiceToRemove(1);

        });

        selectDice2.onClick.AddListener(() => {
            Debug.Log("Selecionando dado 2!");
            GameInput.Instance.SelectDiceToRemove(2);

        });

        selectDice3.onClick.AddListener(() => {
            Debug.Log("Selecionando dado 3!");
            GameInput.Instance.SelectDiceToRemove(3);

        });

        selectDice4.onClick.AddListener(() => {
            Debug.Log("Selecionando dado 4!");
            GameInput.Instance.SelectDiceToRemove(4);

        });

        selectDice5.onClick.AddListener(() => {
            Debug.Log("Selecionando dado 5!");
            GameInput.Instance.SelectDiceToRemove(5);

        });
    }

    private void Start() {

        GameInput.Instance.OnRemoveDiceActionTransitionScene += GameInput_OnRemoveDiceActionTransitionScene;
        DealerRemoveDice.Instance.OnDiceRemovedUI += DealerRemoveDice_OnDiceRemovedUI;

        Hide();
    }


    private void GameInput_OnRemoveDiceActionTransitionScene(object sender, EventArgs e) {
        Show();
        GameInput.Instance.RemoveDiceInput();
    }

    private void DealerRemoveDice_OnDiceRemovedUI(object sender, EventArgs e) {
        StartCoroutine(WaitingToReturnToTable());
    }

    private System.Collections.IEnumerator WaitingToReturnToTable() {
        yield return new WaitForSeconds(2);

        Hide();
        DealerRemoveDice.Instance.EraseDices();
    }


    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}
