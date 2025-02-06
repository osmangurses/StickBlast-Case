using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "list", menuName = "new shape list", order = 1)]
public class ShapeList : ScriptableObject
{
    public List<GameObject> shapes;
}