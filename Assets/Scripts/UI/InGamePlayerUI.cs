using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGamePlayerUI : MonoBehaviour {

    [SerializeField] private Button rollDiceButton;
    [SerializeField] private Button showDiceButton;

    [SerializeField] private Button removeDiceButton;


    private void Awake() {

        rollDiceButton.onClick.AddListener(() => {
            Debug.Log("Rolando os Dados!");
            GameInput.Instance.RollDiceInputTransitionScene();

        });

        showDiceButton.onClick.AddListener(() => {
            GameInput.Instance.ShowDiceInput();

        });

        removeDiceButton.onClick.AddListener(() => {
            GameInput.Instance.RemoveDiceInputTransitionScene();
        });

    }

}
