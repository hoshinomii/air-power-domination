using GameNet.KriptoFX.Realistic_Effects_Pack_3.Scripts.Share;
using UnityEngine;

namespace GameNet.KriptoFX.Realistic_Effects_Pack_3.Scripts.Prefabs.Objects
{
  public class ResetPositionOnDiactivated : MonoBehaviour
  {

    public EffectSettings EffectSettings;

    void Start()
    {
      EffectSettings.EffectDeactivated += EffectSettings_EffectDeactivated;
    }

    void EffectSettings_EffectDeactivated(object sender, System.EventArgs e)
    {
      transform.localPosition = Vector3.zero;
    }
  }
}
