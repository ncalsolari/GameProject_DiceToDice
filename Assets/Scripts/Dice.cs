using UnityEngine;

public class Dice : MonoBehaviour {
    [SerializeField] private DiceSO diceSO;
    private int face;
    private bool IsSettling = false;


    public DiceSO GetDiceSO() {
        return diceSO;
    }
    public void SetDiceFace(int value) {
        face = value;
    }

    public int GetDiceFace() {
        return face;
    }

    public void SetDiceSettling(bool value) {
        IsSettling = value;
    }

    public bool IsDiceSettling() {
        return IsSettling;
    }
}
