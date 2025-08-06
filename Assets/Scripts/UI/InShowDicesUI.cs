using System;
using System.Collections.Generic;
using UnityEngine;

public class InShowDicesUI : MonoBehaviour {

    [SerializeField]


    private void Start() {

        GameInput.Instance.OnShowDiceAction += GameInput_OnShowDiceAction;


        Hide();
    }


    private void GameInput_OnShowDiceAction(object sender, EventArgs e) {
        Show();


    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
