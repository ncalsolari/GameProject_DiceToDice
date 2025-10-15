using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using UnityEditor.AssetImporters;

public class DealerRollDice : MonoBehaviour {

    public static DealerRollDice Instance { get; private set; }

    public event EventHandler OnAllDiceStoped;
    [SerializeField] private AnimationCurve snapCurve;
    [SerializeField] private GameObject wallLeft;
    [SerializeField] private GameObject wallRight;
    [SerializeField] private GameObject wallBottom;
    [SerializeField] private GameObject wallTop;
    [SerializeField] private GameObject wallRoof;

    private bool allDiceStoped = true;
    private bool diceBoxDiseabled = false;


    #region VariaveisDeLancamento
    [SerializeField] Transform bucketCenter;
    [SerializeField] Transform spawnCenter;

    public float spawnHeight = 5f;  //altura do spawn dos dados
    private float spawnAreaRadiusX = 5.5f; // area do spawn dos dados
    private float spawnAreaRadiusZ = 0.5f; // area do spawn dos dados

    //forca do lancamento e giro dos dados
    public float minLaunchForce = 25f;
    public float maxLaunchForce = 300f;
    private float minSpinForce = 3f;
    private float maxSpinForce = 6f;
    #endregion



    private List<GameObject> diceList = new List<GameObject>();
    public float settleThresholdSpeed = 1f;
    public float settleThresholdSpin = 0.4f;

    private void Awake() {


        Instance = this;
    }

    private void Start() {

        GameInput.Instance.OnRollDiceAction += GameInput_OnRollDiceAction;
        GameManager.Instance.OnStartRound += GameManager_OnStartRound;

    }

    private void Update() {

        //a logica precisa ser repensada para a 2 parada, talvez por um delay de 1s
        ReshuffleDiceStop(); // maneja as 2 instancias de parada dos dados (1 antes das paredes serem desabilidatas, 2 dps das paredes serem desabilitadas)


    }

    private void GameInput_OnRollDiceAction(object sender, EventArgs e) {

    }

    private void GameManager_OnStartRound(object sender, EventArgs e) { //reseto as variaveis para inicio de rodada
        Player.Instance.SetDiceHandActive(false); //desativo mao do player
        EraseDices();
        ShowDiceBoxWalls(); // reativa as paredes da DiceBox 


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


    public void RollDiceHand(List<DiceSO> playerDiceList, int diceQnt, int[] playerDiceValue) {

        for (int i = 0; i < diceQnt; i++) {

            // Posição aleatória em cima do balde
            Vector3 randomOffset = new Vector3(
                UnityEngine.Random.Range(-spawnAreaRadiusX, spawnAreaRadiusX),
                0,
                UnityEngine.Random.Range(-spawnAreaRadiusZ, spawnAreaRadiusZ)
            );

            Vector3 spawnPos = spawnCenter.position + randomOffset + Vector3.up * spawnHeight;
            GameObject dice = Instantiate(playerDiceList[i].prefab, spawnPos, UnityEngine.Random.rotation, transform);
            Dice diceScript = dice.GetComponent<Dice>();

            Rigidbody rb = dice.GetComponent<Rigidbody>();

            diceList.Add(dice); // guardo o dado instanciado e sua face para a rolagem fake


            // Velocidade inicial aleatória (empurrando para baixo e para dentro do balde)
            Vector3 launchDir = (bucketCenter.position - spawnPos).normalized + Vector3.forward;
            float launchForce = UnityEngine.Random.Range(minLaunchForce, maxLaunchForce);
            rb.AddForce(launchDir * launchForce, ForceMode.Impulse);

            // Rotação inicial aleatória
            Vector3 spin = new Vector3(
                UnityEngine.Random.Range(minSpinForce, maxSpinForce),
                UnityEngine.Random.Range(minSpinForce, maxSpinForce),
                UnityEngine.Random.Range(minSpinForce, maxSpinForce)
            );
            rb.angularVelocity = spin;
            //  dice.localPosition = Vector3.zero;
            // UnityEngine.UI.Image img = dice.GetComponentInChildren<UnityEngine.UI.Image>();
            // img.sprite = diceSOList[d].faces[diceValue[d] - 1];
        }

        diceBoxDiseabled = false;
        allDiceStoped = false; //ReshuffleDiceStop comeca a fazer a checagem

    }



    //CONTROLE DOS 2 MOMENTOS DE PARADA DOS DADOS E DESABILITAR DICEBOX WALLS
    private void ReshuffleDiceStop() {


        if (!allDiceStoped) {//checo se os dados estao parados
            Debug.Log("Dados em momivento");
            if (!diceBoxDiseabled) { //primeiro os dados param, desabilito as paredes da caixa, os dados voltam a se mover (pode haver troca de face ainda) entao mantenho a flag de dados em movimento
                Debug.Log("DiceBox Habilitada");
                if (CheckAllDiceStop()) {
                    Debug.Log("Primeira parada dos dados");
                    HideDiceBoxWalls();//desabilito walls
                    diceBoxDiseabled = true;//muda a flag, agra muda para a checagem para a segunda e definitiva para dos dados
                }


            }
            else { //paredes ja foram desabilitadas, movimento final do dado
                Debug.Log("DiceBox Desabilitada");
                if (CheckAllDiceStop()) {
                    Debug.Log("Segunda parada dos dados");
                    allDiceStoped = true;

                    GetDiceFaces(); //define as faces voltadas para cima
                    Player.Instance.SetDiceValues(this.diceList);// atualiza mao do player

                    OnAllDiceStoped?.Invoke(this, EventArgs.Empty);

                }
            }

        }

    }

    //RETORNA AS FACES DE TODOS OS DADOS DA MAO
    //incompleto
    private void GetDiceFaces() {

        for (int i = 0; i < diceList.Count; i++) {

            int faceNumber = GetFaceUp(diceList[i]);
            Debug.Log("Face: " + faceNumber);

            Dice diceScript = diceList[i].GetComponent<Dice>();
            diceScript.SetDiceFace(faceNumber);


        }

    }

    public int GetFaceUp(GameObject localDice) {

        float crossProductX;
        float crossProductY;
        float crossProductZ;
        float minCrossProduct = -1;
        int resultCrossProduct;// 1=eixo X, 2=eixo Y; 3=eixo Z

        float dotProduct;

        //calculo o cross product, area do paralelogramo q os dois vetores formam. Se igual a zero, eles sao paralelos
        crossProductX = Vector3.Cross(Vector3.up, localDice.transform.right).magnitude;
        crossProductY = Vector3.Cross(Vector3.up, localDice.transform.up).magnitude;
        crossProductZ = Vector3.Cross(Vector3.up, localDice.transform.forward).magnitude;

        //o menor seria o mais proximo de 0, o mais paralelo possivel com o eixo Y
        minCrossProduct = crossProductX;
        resultCrossProduct = 1;
        if (crossProductY < minCrossProduct) {
            minCrossProduct = crossProductY;
            resultCrossProduct = 2;
        }
        if (crossProductZ < minCrossProduct) {
            minCrossProduct = crossProductZ;
            resultCrossProduct = 3;
        }


        //depois de definido qual direcao esta para cima (x, y ou z) acho qual o sentido que esta para cima
        switch (resultCrossProduct) {
            case 1:
                dotProduct = Vector3.Dot(Vector3.up, localDice.transform.right);//se +1 o vector3.up tem mesmo sentido q transform.right, se -1 entao vec3.up tem sentido oposto do transform.right
                if (dotProduct > 0) {//quais direcoes e sentidos representam quais faces? esta definido no prefab no unity, lembrar de manter o padrao de direcao para diferentes dados
                    return 3;
                }
                else {
                    return 4;
                }
            case 2:
                dotProduct = Vector3.Dot(Vector3.up, localDice.transform.up);
                if (dotProduct > 0) {
                    return 2;
                }
                else {
                    return 5;
                }
            case 3:
                dotProduct = Vector3.Dot(Vector3.up, localDice.transform.forward);
                if (dotProduct > 0) {
                    return 1;
                }
                else {
                    return 6;
                }
            default: return -1;

        }
    }


    //CHECAGEM DE DADOS PARADOS OU NAO
    //obs: se todos os dados estiverem parando dentro do mesmo frame, soh ira considerar os dados parados no frame seguinte
    //n acho q eh algo digno de refatoracao nao, eu penso q esse bug n traz nada d prejudicial 
    private bool CheckAllDiceStop() { //checa quando todos os dados estao parados, usado para desativar as paredes da caixa

        bool allStoped = false;
        //para cada dado checo se algum deles esta parando
        for (int i = 0; i < diceList.Count; i++) {
            Dice diceScript = diceList[i].GetComponent<Dice>();

            if (!diceScript.IsDiceSettling()) { //se o dado ja estava parando seto a flag para true e vou para o proximo dado, se nao eu entro no if e checo se agra o dado esta parando
                Rigidbody rb = diceList[i].gameObject.GetComponent<Rigidbody>();

                //checo os limites para considerar dado em movimento ou parado
                if (rb.linearVelocity.magnitude < settleThresholdSpeed && rb.angularVelocity.magnitude < settleThresholdSpin) { //dado esta parando
                    diceScript.SetDiceSettling(true);

                }
                else { //se pelo menos 1 dado nao esta parando eu ja retorno false
                    allStoped = false;
                    return allStoped;
                }
            }
            else {
                allStoped = true;
            }

        }

        return allStoped;

    }


    //DESABILITAR DICEBOX WALLS
    private void HideDiceBoxWalls() {

        wallLeft.gameObject.SetActive(false);
        wallRight.gameObject.SetActive(false);
        wallBottom.gameObject.SetActive(false);
        wallTop.gameObject.SetActive(false);
        wallRoof.gameObject.SetActive(false);

    }

    //HABILITAR DICEBOX WALLS
    private void ShowDiceBoxWalls() {

        wallLeft.gameObject.SetActive(true);
        wallRight.gameObject.SetActive(true);
        wallBottom.gameObject.SetActive(true);
        wallTop.gameObject.SetActive(true);
        wallRoof.gameObject.SetActive(true);

    }





}
