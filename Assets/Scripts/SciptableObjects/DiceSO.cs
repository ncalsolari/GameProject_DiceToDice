using UnityEngine;


[CreateAssetMenu(fileName = "NewDice", menuName = "Dices/StandardDice")]
public class DiceSO : ScriptableObject {
    public Transform prefab;
    public Sprite[] faces;
    public string diceName;
}
