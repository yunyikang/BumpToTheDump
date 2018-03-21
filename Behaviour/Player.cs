using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // method for movement with arrow keys
    void Update()
    {
        // constant variables for movement
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        // if (Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        //     Vector3 position = this.transform.position;
        //     position.x--;
        //     this.transform.position = position;
        // }
        // if (Input.GetKeyDown(KeyCode.RightArrow))
        // {
        //     Vector3 position = this.transform.position;
        //     position.x++;
        //     this.transform.position = position;
        // }
        // if (Input.GetKeyDown(KeyCode.UpArrow))
        // {
        //     Vector3 position = this.transform.position;
        //     position.y++;
        //     this.transform.position = position;
        // }
        // if (Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     Vector3 position = this.transform.position;
        //     position.y--;
        //     this.transform.position = position;
        // }


        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }
}