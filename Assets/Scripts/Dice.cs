using UnityEngine;

public class Dice : MonoBehaviour {
    [SerializeField] private DiceSO diceSO;
    private int faceUp = -1;
    private bool IsSettling = false;


    public DiceSO GetDiceSO() {
        return diceSO;
    }
    public void SetDiceFace(int value) {
        faceUp = value;
    }

    public int GetDiceFace() {
        return faceUp;
    }

    public void SetDiceSettling(bool value) {
        IsSettling = value;
    }

    public bool IsDiceSettling() {
        return IsSettling;
    }
}
