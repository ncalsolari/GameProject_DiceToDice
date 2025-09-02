using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealerRemoveDice : MonoBehaviour {
    public static DealerRemoveDice Instance { get; private set; }

    [SerializeField] private List<Transform> dicePositionList = new List<Transform>();
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

    private void Awake() {
        Instance = this;
    }

    private void Start() {

    }

    private void Update() {
        FloatingDices(diceList);
    }

    public void ShowRemoveDiceList(List<DiceSO> playerDiceList, int diceQnt, int[] playerDiceValue) {

        for (int i = 0; i < diceQnt; i++) {


            Vector3 spawnPos = dicePositionList[i].position;//pega posicao do unity
            GameObject dice = Instantiate(playerDiceList[i].prefab, spawnPos, UnityEngine.Random.rotation, transform);
            Dice diceScript = dice.GetComponent<Dice>();
            Rigidbody rb = dice.GetComponent<Rigidbody>();
            rb.isKinematic = true;//tira fisica

            diceList.Add(dice);

            rotationSpeed = UnityEngine.Random.Range(minRotationSpeed, maxRotationSpeed);//rotacao aleatoria pro dado em especifico
            if (UnityEngine.Random.value > 0.5f) rotationSpeed *= -1;//pode ser que ele gire para o outro lado
            floatOffset = UnityEngine.Random.Range(0f, Mathf.PI * 2f);//offset no efeito de flutuacao 

            offsetList.Add(new Vector2(rotationSpeed, floatOffset));//guarda valroes para movimento de flutuacao

        }

    }

    private void FloatingDices(List<GameObject> diceList) {

        for (int i = 0; i < diceList.Count; i++) {//pra cada dado
            localDice = diceList[i];//o dado em si
            float rotSpeed = offsetList[i].x;//recupero dados gerados no momento de instanciamento do dado
            float floatPos = offsetList[i].y;
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
    }
}
