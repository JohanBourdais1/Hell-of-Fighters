using IA;
using UnityEngine;

namespace ScriptPLayer
{
    public class DeadZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("DeadZone"))
            {
                col.GetComponent<Player>().Die();
                int rand = Random.Range(0, Respawn.All.Count);
                col.transform.position = Respawn.All[rand].transform.position;
            }
            else if (col.gameObject.CompareTag("IA"))
            {
                int rand = Random.Range(0, Respawn.All.Count);
                col.gameObject.transform.position = Respawn.All[rand].transform.position;
            }
        }
    }
}
