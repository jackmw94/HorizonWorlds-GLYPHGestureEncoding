using UnityEngine;

[CreateAssetMenu(menuName = "Create GestureConfiguration", fileName = "GestureConfiguration", order = 0)]
public class GestureConfiguration : SingletonScriptableObject<GestureConfiguration>
{
    [field: SerializeField] public int CellsPerDimension { get; private set; } = 128;
    [field: SerializeField] public float FalloffDistance { get; private set; } = 20;
}