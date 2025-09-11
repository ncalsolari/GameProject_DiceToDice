using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using UnityEditor.AssetImporters;

public class DealerRollDice : MonoBehaviour {

    public static DealerRollDice Instance { get; private set; }
    [SerializeField] private AnimationCurve snapCurve;
    [SerializeField] private GameObject wallLeft;
    [SerializeField] private GameObject wallRight;
    [SerializeField] private GameObject wallBottom;
    [SerializeField] private GameObject wallTop;
    [SerializeField] private GameObject wallRoof;

    private bool allDiceStoped = true;
    private bool diceBoxDiseabled = false;
    private bool getFaceUpTime = false;

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

    #region VariaveisDeFalsoRolamento
    private class DiceList {
        public GameObject GameObject { get; set; }
        public int Face { get; set; }
        public bool IsSettling { get; set; }

        public DiceList(GameObject gameObject, int face) {
            this.GameObject = gameObject;
            this.Face = face;
            this.IsSettling = false;
        }
    }
    private List<DiceList> diceList2 = new List<DiceList>();
    private List<GameObject> diceList = new List<GameObject>();
    public float settleThresholdSpeed = 1f;
    public float settleThresholdSpin = 0.4f;
    public float snapDuration = 1f;

    #endregion
    private void Awake() {


        Instance = this;
    }

    private void Start() {

        GameInput.Instance.OnRollDiceAction += GameInput_OnRollDiceAction;
        GameInput.Instance.OnEraseDiceAction += GameInput_OnEraseDiceAction;

    }

    private void Update() {

        //a logica precisa ser repensada para a 2 parada, talvez por um delay de 1s
        ReshuffleDiceStop(); // maneja as 2 instancias de parada dos dados (1 antes das paredes serem desabilidatas, 2 dps das paredes serem desabilitadas)


    }

    private void GameInput_OnRollDiceAction(object sender, EventArgs e) {

    }
    private void GameInput_OnEraseDiceAction(object sender, EventArgs e) {
        // percorre todos os dados instanciados e destrói
        foreach (var diceEntry in diceList) {
            if (diceEntry.gameObject != null) {
                Destroy(diceEntry.gameObject);
            }
        }


        diceList.Clear(); // limpa a lista
        Player.Instance.SetDiceHandActive(false); //desativo mao do player
        ShowDiceBoxWalls(); // reativa as paredes da DiceBox 
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
                    getFaceUpTime = true;

                    GetDiceFaces(); //define as faces voltadas para cima

                    Player.Instance.SetDiceValues(this.diceList);// atualiza mao do player

                }
            }

        }

    }

    //RETORNA AS FACES DE TODOS OS DADOS DA MAO
    //incompleto
    private void GetDiceFaces() {
        if (getFaceUpTime) { //talvez usar uma coroutine ao inves dessa variavel boolean 
            for (int i = 0; i < diceList.Count; i++) {

                int faceNumber = GetFaceUp(diceList[i]);
                Debug.Log("Face: " + faceNumber);

                Dice diceScript = diceList[i].GetComponent<Dice>();
                diceScript.SetDiceFace(faceNumber);


            }

            getFaceUpTime = false;

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

        bool allDiceStoped = false;
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
                    allDiceStoped = false;
                    return allDiceStoped;
                }
            }
            else {
                allDiceStoped = true;
            }

        }

        return allDiceStoped;

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


    //OPCAO DE FAKEROLL DESCONTINUADA (MOVIMENTO DE LANCAMENTO DOS DADOS NAO ESTAVA FLUIDO)
    #region FakeRoll

    private void CheckSettling() {

        //para cada dado checo se algum deles esta parando
        for (int i = 0; i < diceList.Count; i++) {
            Dice diceScript = diceList[i].GetComponent<Dice>();

            if (!diceScript.IsDiceSettling()) {

                Rigidbody rb = diceScript.gameObject.GetComponent<Rigidbody>();

                if (rb.linearVelocity.magnitude < settleThresholdSpeed && rb.angularVelocity.magnitude < settleThresholdSpin) { //dado esta parando

                    diceScript.SetDiceSettling(true);
                    // rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    // rb.isKinematic = true;
                    StartCoroutine(SnapToFace(diceList[i], rb));//comeco a coroutine de direcionar o dado para a face desejada

                }

            }



        }

    }

    private IEnumerator SnapToFace(GameObject dice, Rigidbody rb) {

        int tiltAmount = UnityEngine.Random.Range(0, 360);
        Quaternion startRot = dice.gameObject.transform.rotation; //coordenadas atuais do dado
        Quaternion endRot = GetRotationForFace(dice.GetComponent<Dice>().GetDiceFace(), tiltAmount); // calcula coordenadas finais do dado para ter a face desejada para cima

        float elapsed = 0f;

        while (elapsed < snapDuration) {
            elapsed += Time.deltaTime;
            //float t = t = 1 - Mathf.Pow(1 - elapsed / snapDuration, 2); // sera a fracao dessa interpolacao
            float t = Mathf.Clamp01(elapsed / snapDuration);
            float curveT = snapCurve.Evaluate(t);
            dice.gameObject.transform.rotation = Quaternion.Slerp(startRot, endRot, curveT);
            yield return null; //ele para aqui a cada frame, assim o dado segue girando no sentido que queremos ao decorrer do passar dos frmaes
        }

        dice.gameObject.transform.rotation = endRot;


    }

    private Quaternion GetRotationForFace(int face, int tiltAmount) {


        switch (face) {
            case 1: return Quaternion.Euler(-90, tiltAmount, 0);
            case 2: return Quaternion.Euler(0, tiltAmount, 0);
            case 3: return Quaternion.Euler(0, tiltAmount, 90);
            case 4: return Quaternion.Euler(0, tiltAmount, -90);
            case 5: return Quaternion.Euler(180, tiltAmount, 0);
            case 6: return Quaternion.Euler(90, tiltAmount, 0);
            default: return Quaternion.identity;
        }

    }
    #endregion



}
