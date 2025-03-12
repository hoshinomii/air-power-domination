using UnityEngine;

namespace GameNet.KriptoFX.Realistic_Effects_Pack_3.Scripts.Share
{
  public class DeadTime: MonoBehaviour
  {
    public float deadTime = 1.5f;
    public bool destroyRoot;
    void Awake()
    {
      Destroy(!destroyRoot ? gameObject : transform.root.gameObject, deadTime);
    }
  }
}
