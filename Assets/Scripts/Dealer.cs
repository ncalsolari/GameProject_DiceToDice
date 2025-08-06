using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour //Essa classe lida com a logica do jogo, eh pra ela funcionar independente da unity
{

    public static Dealer Instance { get; private set; }

    [SerializeField] private List<DiceSO> diceSOList = new List<DiceSO>();
    [SerializeField] private Transform dicePosition;

    private void Awake() {
        Instance = this;
    }

    public void Show() {
        //Transform dicetransform = Instantiate(diceSO.prefab, dicePosition);
        //dicetransform.localPosition = Vector3.zero;
    }

    public void RollDices() {

    }
}
