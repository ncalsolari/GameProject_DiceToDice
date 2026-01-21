using UnityEngine;

public class DealerBet : MonoBehaviour {


    private int diceQntBet = 0;
    private int diceQntAllHands = 10; // ESTA HARDCODE - MUDAR, total de dados no jogo todo, somando todos os players
    private int diceFaceSelected = 0;

    private int betStatus = -1;//false=liar / true=bet
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

        betStatus = e.value;

        Debug.Log(betStatus);
    }

    private void CpuTurn(int diceQnt, int diceFace) {
        //CPU pensa na jogada e retorna liar ou nova aposta
        //preciso da mao do cpu
    }
}
