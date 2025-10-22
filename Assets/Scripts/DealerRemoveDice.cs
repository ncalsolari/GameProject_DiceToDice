using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class DealerRemoveDice : MonoBehaviour {

    const int DICE_QNT = 5;
    public static DealerRemoveDice Instance { get; private set; }

    public event EventHandler OnDiceRemovedUI;
    public event EventHandler OnDiceRemoved;

    [SerializeField] private List<Transform> dicePositionList = new List<Transform>();
    private int selectedDice;

    private bool[] diceIsBoostin = new bool[DICE_QNT];


    private float rotationSpeed;
    private float floatOffset;
    public float minRotationSpeed = 7f;
    public float maxRotationSpeed = 13f;
    public float floatSpeed = 2.5f;
    public float floatAmplitude = 0.007f;
    GameObject localDice;
    Vector3 startPosition;
    Quaternion baseRotation;
    Quaternion yRotation;

    private List<GameObject> diceList = new List<GameObject>();
    private List<Vector2> offsetList = new List<Vector2>();


    private List<float> baseRotSpeed = new();
    private List<float> currentRotSpeed = new();
    private List<float> baseFloatPos = new();

    private void Awake() {
        Instance = this;

    }

    private void Start() {
        GameInput.Instance.OnSelectedDiceToRemove += GameInput_OnSelectedDiceToRemove;
        GameInput.Instance.OnRemoveSelectedDiceAction += GameInput_OnRemoveSelectedDiceAction;

    }

    private void Update() {
        FloatingDices(diceList);
    }



    //gira os dados ao seleciona-los e poe efeito visual de selecionado
    private void GameInput_OnSelectedDiceToRemove(object sender, GameInput.OnSelectedDiceToRemoveEventArgs e) {

        if (selectedDice == -1) {
            return;
        }

        if (e.indexSelectedDice <= diceList.Count) { //se eu apertar no botao aonde nao tem mais dado ele nao faz nada
            for (int i = 0; i < diceList.Count; i++) {//varre meus dados
                if (e.indexSelectedDice == i + 1) {//se for o dado do indice do botao selecionado
                    diceList[i].GetComponentInChildren<SelectedDiceVisual>().SetVisualGameObject(false);//seleciona o componente com o script do visual selecionado
                    BoostDiceRotation(i, 50f, 2f);//boost de rotacao

                    selectedDice = e.indexSelectedDice;


                }
                else {
                    diceList[i].GetComponentInChildren<SelectedDiceVisual>().SetVisualGameObject(true);//desseleciona visualmente os outros
                }
            }
        }

    }

    private void GameInput_OnRemoveSelectedDiceAction(object sender, EventArgs e) {
        if (selectedDice > 0) {
            Rigidbody rb = diceList[selectedDice - 1].GetComponent<Rigidbody>();
            rb.isKinematic = false;//poe a fisica

            // diceList[selectedDice - 1].gameObject.SetActive(false);

            Player.Instance.RemoveSelectedDice(selectedDice - 1);
            selectedDice = -1;
            OnDiceRemovedUI.Invoke(this, EventArgs.Empty);
        }

    }


    public void BoostDiceRotation(int diceIndex, float multiplier, float duration) {

        if (!diceIsBoostin[diceIndex]) {
            diceIsBoostin[diceIndex] = true;
            StartCoroutine(BoostRoutine(diceIndex, multiplier, duration));
        }

    }

    private System.Collections.IEnumerator BoostRoutine(int diceIndex, float multiplier, float duration) {
        float original = baseRotSpeed[diceIndex];//guardo rot inicial
        float boosted = original * multiplier;//calculo o boost
        float elapsed = 0f;

        while (elapsed < duration) { //durante o tempo selecionado adiciono a rotacao com aceleracao e desaceleracao
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration); //efeito desacelera mais rapido no comeco doq no fim
                                                                    //mudar essa func muda como o dado acelera/desacelera, procurar outros modelos talvez

            //interpola a velocidade
            baseRotSpeed[diceIndex] = Mathf.Lerp(boosted, original, t);

            yield return null; // espera 1 frame e continua

        }

        //transicao estava dura
        // baseRotSpeed[diceIndex] = original * multiplier;
        //yield return new WaitForSeconds(duration);

        baseRotSpeed[diceIndex] = original; // volta ao normal
        diceIsBoostin[diceIndex] = false;
    }






    //Player chama essa func ao abrir a tela de remove dice
    public void ShowRemoveDiceList(List<DiceSO> playerDiceList, int diceQnt, int[] playerDiceValue) {

        selectedDice = 0;

        for (int i = 0; i < diceQnt; i++) {


            Vector3 spawnPos = dicePositionList[i].position;//pega posicao do unity
            GameObject dice = Instantiate(playerDiceList[i].prefab, spawnPos, UnityEngine.Random.rotation, transform);

            Dice diceScript = dice.GetComponent<Dice>();
            Rigidbody rb = dice.GetComponent<Rigidbody>();
            rb.isKinematic = true;//tira fisica
            if (i == 0) {//abro a tela de remocao com o primeiro dado já selecionado
                dice.GetComponentInChildren<SelectedDiceVisual>().SetVisualGameObject(false);
                selectedDice = 1;
            }
            else {
                dice.GetComponentInChildren<SelectedDiceVisual>().SetVisualGameObject(true);
            }

            diceList.Add(dice);

            rotationSpeed = UnityEngine.Random.Range(minRotationSpeed, maxRotationSpeed);//rotacao aleatoria pro dado em especifico
            if (UnityEngine.Random.value > 0.5f) rotationSpeed *= -1;//pode ser que ele gire para o outro lado
            floatOffset = UnityEngine.Random.Range(0f, Mathf.PI * 2f);//offset no efeito de flutuacao 

            baseRotSpeed.Add(rotationSpeed);
            baseFloatPos.Add(floatOffset);
            offsetList.Add(new Vector2(rotationSpeed, floatOffset));//guarda valroes para movimento de flutuacao

            diceIsBoostin[i] = false;

        }

    }

    private void FloatingDices(List<GameObject> diceList) {

        for (int i = 0; i < diceList.Count; i++) {//pra cada dado
            localDice = diceList[i];//o dado em si
            float rotSpeed = baseRotSpeed[i];//recupero dados gerados no momento de instanciamento do dado
            float floatPos = baseFloatPos[i];
            startPosition = localDice.transform.position;//guardo posicao inicial
            baseRotation = localDice.transform.rotation;//guardo rotacao inicial
            yRotation = Quaternion.Euler(0, Time.deltaTime * rotSpeed, 0);//rotacao dependendo do tempo
            localDice.transform.rotation = yRotation * baseRotation;//aplico rotacao

            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed + floatPos) * floatAmplitude;//posicao em Y, o seno da o efeito flutuante, vai e volta, sobe-desce
            localDice.transform.position = new Vector3(startPosition.x, newY, startPosition.z);//aplico nova posicao

        }


    }

    public void EraseDices() {
        // percorre todos os dados instanciados e destrói
        foreach (var diceEntry in diceList) {
            if (diceEntry.gameObject != null) {
                Destroy(diceEntry.gameObject);
            }
        }


        diceList.Clear(); // limpa a lista
        baseRotSpeed.Clear();
        baseFloatPos.Clear();
        OnDiceRemoved.Invoke(this, EventArgs.Empty);

    }
}
