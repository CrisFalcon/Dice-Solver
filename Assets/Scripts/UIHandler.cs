using System.Collections;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public Dice dice;
    public GameObject diceUI;

    Coroutine activateRouine = null;

    void Update() => HandleDices();
    
    void HandleDices()
    {
        if (diceUI.activeSelf == false && !dice.IsMoving && activateRouine == null)
            activateRouine = StartCoroutine(ActivateUI());

        if (dice.IsMoving == true) diceUI.SetActive(false);
    }

    IEnumerator ActivateUI()
    {
        yield return new WaitForSeconds(0.3f);
        diceUI.SetActive(true);
        activateRouine = null;
    }

}
