using UnityEngine;


[CreateAssetMenu(fileName = "NewDice", menuName = "Dices/StandardDice")]
public class DiceSO : ScriptableObject {
    public GameObject prefab;
    public Sprite[] faces;
    public string diceName;
}
