using UnityEngine;

[CreateAssetMenu(fileName = "TileSettings", menuName = "WeMade2048/Tile Settings", order = 0)]
public class TileSettings : ScriptableObject {
    public float AnimationTime = .3f;
    public AnimationCurve AnimationCurve;
    
}