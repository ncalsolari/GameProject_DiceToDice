using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;

public class DealerRollDice : MonoBehaviour {

    public static DealerRollDice Instance { get; private set; }
    [SerializeField] private AnimationCurve snapCurve;

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
    private List<DiceList> diceList = new List<DiceList>();
    public float settleThresholdSpeed = 1f;
    public float settleThresholdSpin = 0.4f;
    public float snapDuration = 1f;

    #endregion
    private void Awake() {


        Instance = this;
    }

    private void Start() {

        GameInput.Instance.OnRollDiceAction += GameInput_OnRollDiceAction;
        GameInput.Instance.OnEraseDiceAction += GameInput_OnRollEraseAction;

    }

    private void Update() {

        // CheckSettling();

    }

    private void GameInput_OnRollDiceAction(object sender, EventArgs e) {

    }
    private void GameInput_OnRollEraseAction(object sender, EventArgs e) {
        // percorre todos os dados instanciados e destrói
        foreach (var diceEntry in diceList) {
            if (diceEntry.GameObject != null) {
                Destroy(diceEntry.GameObject);
            }
        }

        // limpa a lista
        diceList.Clear();
        Player.Instance.SetDiceHandActive(false);
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
            Rigidbody rb = dice.GetComponent<Rigidbody>();

            diceList.Add(new DiceList(dice, playerDiceValue[i])); // guardo o dado instanciado e sua face para a rolagem fake


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

    }


    private void CheckSettling() {
        //para cada dado checo se algum deles esta parando
        for (int i = 0; i < diceList.Count; i++) {

            if (!diceList[i].IsSettling) {

                Rigidbody rb = diceList[i].GameObject.GetComponent<Rigidbody>();

                if (rb.linearVelocity.magnitude < settleThresholdSpeed && rb.angularVelocity.magnitude < settleThresholdSpin) { //dado esta parando

                    diceList[i].IsSettling = true;
                    // rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    // rb.isKinematic = true;
                    StartCoroutine(SnapToFace(diceList[i], rb));//comeco a coroutine de direcionar o dado para a face desejada

                }

            }


        }

    }

    private IEnumerator SnapToFace(DiceList dice, Rigidbody rb) {

        int tiltAmount = UnityEngine.Random.Range(0, 360);
        Quaternion startRot = dice.GameObject.transform.rotation; //coordenadas atuais do dado
        Quaternion endRot = GetRotationForFace(dice.Face, tiltAmount); // calcula coordenadas finais do dado para ter a face desejada para cima

        float elapsed = 0f;

        while (elapsed < snapDuration) {
            elapsed += Time.deltaTime;
            //float t = t = 1 - Mathf.Pow(1 - elapsed / snapDuration, 2); // sera a fracao dessa interpolacao
            float t = Mathf.Clamp01(elapsed / snapDuration);
            float curveT = snapCurve.Evaluate(t);
            dice.GameObject.transform.rotation = Quaternion.Slerp(startRot, endRot, curveT);
            yield return null; //ele para aqui a cada frame, assim o dado segue girando no sentido que queremos ao decorrer do passar dos frmaes
        }

        dice.GameObject.transform.rotation = endRot;


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

}
