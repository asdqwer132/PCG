using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct NDSubmit
{
    public int rank;
    public bool isMine;
    public Team team;
    public NDSubmit(int rank, bool isMine ,Team team)
    {
        this.isMine = isMine;
        this.rank = rank;
        this.team = team;
    }
}
public class NDSubmitManager : MonoBehaviour
{
    [SerializeField] CardDrop card;
    [SerializeField] private List<NDSubmit> currentSubmit = new List<NDSubmit>();
    public bool CanSubmit() { return card.GetCardInfo().number != 0; }
    public int GetSubmitRank() { return card.GetCardInfo().number; }
    public bool IsEnd() { return currentSubmit.Count == 2; }
    public void Submit() { card.ResetCard(); }
    public void ResetSubmit() { currentSubmit = new List<NDSubmit>(); }
    public void AddSubmit(int rank,bool isMine ,Team team) { currentSubmit.Add(new NDSubmit(rank,isMine, team)); }
    public List<NDSubmit> GetSubmits() { return currentSubmit; }
}
