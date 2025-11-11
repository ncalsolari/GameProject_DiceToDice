using UnityEngine;

public class DealerBet : MonoBehaviour {


    private int diceQntBet = 0;
    private int diceQntAllHands = 10;
    private int diceFaceSelected = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        GameInput.Instance.OnChangeQntDiceBet += GameInput_OnChangeQntDiceBet;
        GameInput.Instance.OnSelectDiceFaceBet += GameInput_OnSelectDiceFaceBet;
        GameInput.Instance.OnConfirmBet += GameInput_OnConfirmBet;

    }

    // Update is called once per frame
    void Update() {

    }


    private void GameInput_OnChangeQntDiceBet(object sender, GameInput.SingleIntEventArgs e) {

        int aux = diceQntBet;
        aux += e.value;

        if (aux < 0 || aux > diceQntAllHands) {
            Debug.Log("impossible do add/remove qntd dice bet");
            return;
        }

        diceQntBet = aux;
        Debug.Log(diceQntBet);
    }


    private void GameInput_OnSelectDiceFaceBet(object sender, GameInput.SingleIntEventArgs e) {

        diceFaceSelected = e.value;

        Debug.Log(diceFaceSelected);
    }

    private void GameInput_OnConfirmBet(object sender, GameInput.SingleIntEventArgs e) {

        int confirmResult = e.value;

        Debug.Log(confirmResult);
    }
}
