using UnityEngine;

namespace GameNet.KriptoFX.Realistic_Effects_Pack_3.Scripts.Share
{
  public class Billboard : MonoBehaviour
  {
    public UnityEngine.Camera Camera;
    public bool Active = true;
    public bool AutoInitCamera = true;

    private GameObject myContainer;
    private Transform t, camT, contT;

    private void Awake()
    {
      if (AutoInitCamera) {
        Camera = UnityEngine.Camera.main;
        Active = true;
      }

      t = transform;
      camT = Camera.transform;
      var parent = t.parent;
      myContainer = new GameObject { name = "Billboard_" + t.gameObject.name };
      contT = myContainer.transform;
      contT.position = t.position;
      t.parent = myContainer.transform;
      contT.parent = parent;
    }

    private void Update()
    {
      if (Active)
        contT.LookAt(contT.position + camT.rotation * Vector3.back, camT.rotation * Vector3.up);
    }
  }
}