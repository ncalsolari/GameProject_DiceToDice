using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InRemoveDicesUI : MonoBehaviour {


    [SerializeField] private Button removeDiceButton;
    [SerializeField] private Button returnMainGameButton;


    private void Awake() {

        returnMainGameButton.onClick.AddListener(() => {

            Hide();
            DealerRemoveDice.Instance.EraseDices(); //aqui talvez eu deva refatorar pra essa classe se comunicar com o GameInput e nao direto cm o dealer, 
                                                    // tipo a func GameInput_OnRemoveDiceActionTransitionScene ali embaixo
                                                    //aqui ele soh apaga os prefabs da Scene, talvez eu tenha que estruturar melhor essa logica
        });
    }

    private void Start() {

        GameInput.Instance.OnRemoveDiceActionTransitionScene += GameInput_OnRemoveDiceActionTransitionScene;

        Hide();
    }


    private void GameInput_OnRemoveDiceActionTransitionScene(object sender, EventArgs e) {
        Show();
        GameInput.Instance.RemoveDiceInput();
    }


    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}
