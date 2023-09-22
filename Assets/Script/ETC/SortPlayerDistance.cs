using System.Collections.Generic;
public class SortPlayerDistance
{
    public static List<T> SortListByReference<T>(List<T> inputList, int referenceValue)
    {
        List<T> sortedList = new List<T>();
        for (int i = referenceValue; i < inputList.Count; i++) { sortedList.Add(inputList[i]); }
        for (int i = 0; i < referenceValue; i++) { sortedList.Add(inputList[i]); }
        return sortedList;
    }
}
