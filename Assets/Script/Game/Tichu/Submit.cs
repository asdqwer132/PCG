using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Submit : MonoBehaviour
{
    [SerializeField] Button submitBtn;
    [SerializeField] TextMeshProUGUI btnText;
    bool isMyTurn = false;
    public void SetTextByGenealogy(Genealogy genealogy, bool canGenealogy)
    {
        string gemealogyText = genealogy.length > 0 ? NumberConverter.ConvertNum(genealogy.rank) + " " + genealogy.genealogyType : "";
        btnText.text = gemealogyText;
        SetBtn(canGenealogy);
    }
    public void SetTurn(bool isMyTurn) { this.isMyTurn = isMyTurn; }
    public void SetBtn(bool interactbale) {  submitBtn.interactable = interactbale && isMyTurn; }
    public void ForceSetBtn() { submitBtn.interactable = true; }
}
