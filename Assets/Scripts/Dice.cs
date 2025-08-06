using UnityEngine;

public class Dice : MonoBehaviour {
    [SerializeField] private DiceSO diceSO;

    public DiceSO GetDiceSO() {
        return diceSO;
    }


}
