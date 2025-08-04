using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGamePlayerUI : MonoBehaviour {

    [SerializeField] private Button rollDiceButton;


    private void Awake() {
        rollDiceButton.onClick.AddListener(() => {
            GameInput.Instance.RollDiceInput();

        });
    }

}
