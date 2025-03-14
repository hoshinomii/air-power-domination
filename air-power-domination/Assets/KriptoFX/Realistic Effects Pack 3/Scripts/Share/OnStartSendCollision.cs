﻿using UnityEngine;

namespace GameNet.KriptoFX.Realistic_Effects_Pack_3.Scripts.Share
{
  public class OnStartSendCollision : MonoBehaviour
  {

    private EffectSettings effectSettings;
    private bool isInitialized;

    private void GetEffectSettingsComponent(Transform tr)
    {
      var parent = tr.parent;
      if (parent != null)
      {
        effectSettings = parent.GetComponentInChildren<EffectSettings>();
        if (effectSettings == null)
          GetEffectSettingsComponent(parent.transform);
      }
    }
    void Start () {
      GetEffectSettingsComponent(transform);
      effectSettings.OnCollisionHandler(new CollisionInfo());
      isInitialized = true;
    }

    void OnEnable()
    {
      if (isInitialized) effectSettings.OnCollisionHandler(new CollisionInfo());
    }
  }
}
