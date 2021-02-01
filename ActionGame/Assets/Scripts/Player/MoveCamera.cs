using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public Transform Player;
    
    void Update() {
        this.transform.position = Player.transform.position;
    }
}
