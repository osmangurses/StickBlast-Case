using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "listContainer", menuName = "new shape list container", order = 1)]
public class ShapeListContainer : ScriptableObject
{
    public List<ShapeList> shapeLists;
}
