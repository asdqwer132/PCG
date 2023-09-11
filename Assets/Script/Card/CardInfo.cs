using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
[System.Serializable]
[DataContract]
public class CardInfo
{
    [DataMember]
    public string cardType;
    [DataMember]
    public int number;
    public CardInfo(string cardType, int number)
    {
        this.cardType = cardType;
        this.number = number;
    }    // Á÷·ÄÈ­
   
}
