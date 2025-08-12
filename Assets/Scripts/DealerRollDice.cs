using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;

public class DealerRollDice : MonoBehaviour {

    public static DealerRollDice Instance { get; private set; }

    [SerializeField] Transform bucketCenter;
    [SerializeField] Transform spawnCenter;

    public float spawnHeight = 5f;  //altura do spawn dos dados
    private float spawnAreaRadiusX = 5.5f; // area do spawn dos dados
    private float spawnAreaRadiusZ = 0.5f; // area do spawn dos dados

    //forca do lancamento e giro dos dados
    public float minLaunchForce = 25f;
    public float maxLaunchForce = 240f;
    private float minSpinForce = 2f;
    private float maxSpinForce = 5f;


    private void Awake() {


        Instance = this;
    }

    private void Start() {

        GameInput.Instance.OnRollDiceAction += GameInput_OnRollDiceAction;



    }

    private void GameInput_OnRollDiceAction(object sender, EventArgs e) {

    }

    public void ShowDiceHand(List<DiceSO> diceList, int diceQnt) {

        for (int i = 0; i < diceQnt; i++) {

            // Posição aleatória em cima do balde
            Vector3 randomOffset = new Vector3(
                UnityEngine.Random.Range(-spawnAreaRadiusX, spawnAreaRadiusX),
                0,
                UnityEngine.Random.Range(-spawnAreaRadiusZ, spawnAreaRadiusZ)
            );

            Vector3 spawnPos = spawnCenter.position + randomOffset + Vector3.up * spawnHeight;
            GameObject dice = Instantiate(diceList[i].prefab, spawnPos, UnityEngine.Random.rotation);
            Rigidbody rb = dice.GetComponent<Rigidbody>();

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
}
