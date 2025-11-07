using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create GestureContainer", fileName = "GestureContainer", order = 0)]
public class GestureContainer : SingletonScriptableObject<GestureContainer>
{
    public List<Gesture> gestures;
}