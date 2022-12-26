
using UnityEngine;

public class CheckPointsPhysicsController : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Playable")&& gameObject.CompareTag("Untagged"))
    {
      gameObject.tag = "Playable";
      GameManager.Instance.checkPoints.Remove(gameObject.transform);
      GameManager.Instance.lastTempZ = 50;
      Debug.Log("checkpoint silindi");
      
     gameObject.SetActive(false);
      GameManager.Instance.nextCheckPoint++;

    }
  }
}
