using UnityEngine;

public class TransferScene : MonoBehaviour {
    public int ID = 0;

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            GameManager.Instance.LoadScene(ID);
        }
    }
}
