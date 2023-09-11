using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CallPannel : Popup
{
    [SerializeField] SubmitManager submitManager;
    [SerializeField] TmpAlpha warningMessage;
    [SerializeField] TextMeshProUGUI callText;
    [SerializeField] int callRank;//
    [SerializeField] bool isSetted = false;//
    private void Start()
    {
        Close(); 
        ResetCall();
    }
    public void SetMessage(WanringType wanringType)
    {
        warningMessage.FadeOut(0);
    }
    public void ResetCall()
    {
        callRank = -99;
        isSetted = false;
        callText.gameObject.SetActive(false);
    }
    public void SetCall(int rank)
    {
        isSetted = true;
        callRank = rank;
        callText.gameObject.SetActive(true);
        if (callRank != -99) callText.text = "Call : " + NumberConverter.ConvertNum(rank);
        else
        {
            callText.gameObject.SetActive(false);
        }
        Close();
    }
    public int GetCall() { return callRank; }
    public bool IsSetted() { return isSetted; }
    public bool CheckCall(List<CardInfo> cardInfos, Genealogy genealogy)
    {
        if (callRank == -99) return false;
        if (GenealogyChekcer.CanMake4k(callRank, cardInfos)) return true;
        if (GenealogyChekcer.CanMakeSF(callRank, cardInfos)) return true;
        if (GenealogyChekcer.CheckMakeGenealogy(callRank, cardInfos, genealogy)) //낼 수 있다면
        {
            if(genealogy.genealogyType == GenealogyType.straight) return submitManager.CheckCanSubmit(GenealogyChekcer.CheckGenealogy(GenealogyChekcer.MakedStraight(callRank, genealogy.length, cardInfos)));
            return submitManager.CheckCanSubmit(GenealogyChekcer.CheckGenealogy(GenealogyChekcer.MakedSingle(callRank, cardInfos)));
        }
        return false;
    }
}
