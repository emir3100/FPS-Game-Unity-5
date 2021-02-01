using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public Transform Player;
    public Transform Legs;
    public Transform LegOrientation;
    public Transform PlayerOrientation;
    public float LegDistance = 0.5f;
    
    void Update() {
        this.transform.position = Player.transform.position;

        Vector3 CameraAngle = new Vector3(0, this.transform.eulerAngles.y, 0);
        Legs.eulerAngles = CameraAngle;

        Vector3 legOrientation = new Vector3(LegOrientation.position.x, Player.position.y-1.5f, PlayerOrientation.position.z);
        Legs.position = legOrientation;
    }
}
