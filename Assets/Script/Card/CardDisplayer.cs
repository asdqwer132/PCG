using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class CardDisplayer : MonoBehaviour
{
    [SerializeField] List<GameObject> cards;
    [SerializeField] TextMeshProUGUI remainNumber;
    [SerializeField] TextMeshProUGUI nicknameText;
    [SerializeField] TextMeshProUGUI sendCard;
    [SerializeField] TextMeshProUGUI receiveCard;
    [SerializeField] GameObject largeTichu;
    [SerializeField] GameObject smallTichu;
    [SerializeField] TichuPlayer player;
    public void Setup(string nickname, TichuPlayer player)
    {
        this.player = player;
        nicknameText.text = nickname;
        nicknameText.color = player.Team == Team.Red ? Color.red : Color.blue;
    }
    public void SetTichu()
    {
        largeTichu.gameObject.SetActive(player.Tichu == Tichu.largeTichu);
        smallTichu.gameObject.SetActive(player.Tichu == Tichu.smallTichu);
    }
    public void SetCard()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].SetActive(i < player.HandCards.Count);
        }
        remainNumber.text = player.HandCards.Count.ToString();
    }
    public void SetSendRecieve(bool isOn)
    {
        sendCard.transform.parent.gameObject.SetActive(isOn);
        receiveCard.transform.parent.gameObject.SetActive(isOn);
        sendCard.text = player.SendCard.ToString();
        receiveCard.text = player.RecieveCard.Count.ToString();
    }
}
