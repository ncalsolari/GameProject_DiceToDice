using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;



public class Player : MonoBehaviour // Essa classe guarda as infos do player e lida com acoes do mesmo
{

    const int DICE_QNT = 5;
    public static Player Instance { get; private set; }



    [SerializeField] private GameInput gameInput;
    [SerializeField] private List<DiceSO> diceSOList = new List<DiceSO>();
    private List<DiceSO> diceSOListCopy = new List<DiceSO>();
    [SerializeField] private List<Transform> dicePositionList = new List<Transform>();


    private int[] diceQnt = new int[6]; //o valor de diceQnt[0] significa quantos dados cm face 1 eu tenho, diceQnt[1] face 2 e ...
    private int[] diceValue = new int[DICE_QNT]; //o valor de diceValue[0] significa a face do meu dado 1 (de um dos meus dados)
    private int actualDiceQnt = DICE_QNT;
    private bool diceHandActive = false;

    private void Awake() {

        if (Instance != null) {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;

        diceSOListCopy = diceSOList;
    }
    private void Start() {

        GameInput.Instance.OnRollDiceAction += GameInput_OnRollDiceAction;
        GameInput.Instance.OnShowDiceAction += GameInput_OnShowDiceAction;
        GameInput.Instance.OnRemoveDiceAction += GameInput_OnRemoveDiceAction;

        ResetHand();
    }


    private void GameInput_OnRollDiceAction(object sender, EventArgs e) {

        if (!diceHandActive) {
            DealerRollDice.Instance.RollDiceHand(diceSOListCopy, actualDiceQnt, diceValue);
            diceHandActive = true;
        }

    }


    private void GameInput_OnShowDiceAction(object sender, EventArgs e) {
        ShowDiceHand();

    }

    private void GameInput_OnRemoveDiceAction(object sender, EventArgs e) {
        DealerRemoveDice.Instance.ShowRemoveDiceList(diceSOListCopy, actualDiceQnt, diceValue);
    }

    private void RollDiceHand() {

        for (int i = 0; i < actualDiceQnt; i++) {

            if (diceValue[i] >= 0) { //se o valor for -1 eu perdi este dado, nao atribuo valor a ele
                int rand = UnityEngine.Random.Range(1, 7); //jogo dado
                diceValue[i] = rand; //face do dado
                diceQnt[rand - 1] += 1; // qntdade de dados com essa face
            }

        }

    }

    private void ResetHand() {

        for (int i = 0; i < actualDiceQnt; i++) {
            diceValue[i] = 0; // atribuir valor -1 para quando vc eliminar o dado da mao
        }

        for (int i = 0; i < 6; i++) {
            diceQnt[i] = 0;
        }
    }

    private void ShowDiceHand() {

        /*
        for (int d = 0; d < actualDiceQnt; d++) {
            Transform dice = Instantiate(diceSOList[d].prefab, dicePositionList[d]);
            dice.localPosition = Vector3.zero;
            UnityEngine.UI.Image img = dice.GetComponentInChildren<UnityEngine.UI.Image>();
            img.sprite = diceSOList[d].faces[diceValue[d] - 1];
        }*/
    }
    public int GetDiceQnt() {
        return actualDiceQnt;
    }

    public bool HasDiceHandActive() {
        return diceHandActive;
    }

    public void SetDiceHandActive(bool status) {
        diceHandActive = status;
    }

    public void SetDiceValues(List<GameObject> diceList) {
        ResetHand();

        for (int i = 0; i < diceList.Count; i++) {



            if (diceValue[i] >= 0) { //se o valor for -1 eu perdi este dado, nao atribuo valor a ele
                int diceFace = diceList[i].GetComponent<Dice>().GetDiceFace();

                diceValue[i] = diceFace; //face do dado
                diceQnt[diceFace - 1] += 1; // qntdade de dados com essa face

                Debug.Log("Face dado" + i + ": " + diceFace);
            }

        }

    }

    public void RemoveSelectedDice(int index) {

        diceSOListCopy.RemoveAt(index);
        actualDiceQnt -= 1;
        diceValue[actualDiceQnt] = -1;


    }
}
