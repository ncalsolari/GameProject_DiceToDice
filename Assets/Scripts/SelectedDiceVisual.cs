using System;
using UnityEngine;

public class SelectedDiceVisual : MonoBehaviour {

    [SerializeField] private GameObject visualGameObject;
    private void Start() {

    }



    public void SetVisualGameObject(bool status) {
        visualGameObject.SetActive(status);
    }
}
