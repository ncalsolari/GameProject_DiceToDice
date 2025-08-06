using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGamePlayerUI : MonoBehaviour {

    [SerializeField] private Button rollDiceButton;
    [SerializeField] private Button showDiceButton;


    private void Awake() {
        rollDiceButton.onClick.AddListener(() => {
            GameInput.Instance.RollDiceInput();

        });

        showDiceButton.onClick.AddListener(() => {
            GameInput.Instance.ShowDiceInput();

        });


    }

}
