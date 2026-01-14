using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetPanelUI : MonoBehaviour {

    [SerializeField] private Button minusQntButton;
    [SerializeField] private Button plusQntButton;
    [SerializeField] private Button selectFace1Button;
    [SerializeField] private Button selectFace2Button;
    [SerializeField] private Button selectFace3Button;
    [SerializeField] private Button selectFace4Button;
    [SerializeField] private Button selectFace5Button;
    [SerializeField] private Button selectFace6Button;

    [SerializeField] private Button betButton;
    [SerializeField] private Button liarButton;


    private void Awake() {

        minusQntButton.onClick.AddListener(() => {
            Debug.Log("menos 1 na aposta!");
            GameInput.Instance.BetButton(-1);

        });

        plusQntButton.onClick.AddListener(() => {
            Debug.Log("mais 1 na aposta");
            GameInput.Instance.BetButton(1);

        });

        selectFace1Button.onClick.AddListener(() => {
            Debug.Log("Selecionando face 1 na aposta!");
            GameInput.Instance.SelectFaceBet(1);

        });

        selectFace2Button.onClick.AddListener(() => {
            Debug.Log("Selecionando face 2 na aposta!");
            GameInput.Instance.SelectFaceBet(2);

        });

        selectFace3Button.onClick.AddListener(() => {
            Debug.Log("Selecionando face 3 na aposta!");
            GameInput.Instance.SelectFaceBet(3);

        });

        selectFace4Button.onClick.AddListener(() => {
            Debug.Log("Selecionando face 4 na aposta!");
            GameInput.Instance.SelectFaceBet(4);

        });

        selectFace5Button.onClick.AddListener(() => {
            Debug.Log("Selecionando face 5 na aposta!");
            GameInput.Instance.SelectFaceBet(5);

        });

        selectFace6Button.onClick.AddListener(() => {
            Debug.Log("Selecionando face 6 na aposta!");
            GameInput.Instance.SelectFaceBet(6);

        });

        betButton.onClick.AddListener(() => {
            Debug.Log("Confirmando aposta!");
            GameInput.Instance.ConfirmBetButton(0);

        });

        liarButton.onClick.AddListener(() => {
            Debug.Log("MENTIRA!");
            GameInput.Instance.ConfirmBetButton(1);

        });

    }
    private void Start() {
        GameManager.Instance.OnPlayerTurn += GameManager_OnPlayerTurn;
        GameInput.Instance.OnConfirmBet += GameInput_OnConfirmBet;

        Hide();
    }

    // Update is called once per frame
    void Update() {

    }


    private void GameManager_OnPlayerTurn(object sender, EventArgs e) {
        Show();
    }

    private void GameInput_OnConfirmBet(object sender, EventArgs e) {
        Hide();
    }






    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
