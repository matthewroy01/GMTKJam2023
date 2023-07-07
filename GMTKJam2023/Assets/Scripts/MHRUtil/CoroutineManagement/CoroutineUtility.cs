using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MHR.CoroutineManagement
{
    public static class CoroutineUtility
    {
        private static float _customTimeScale = 1.0f;
        public static float CustomTimeScale = 1.0f;

        public static void SetCustomTimeScale(float customTimeScale)
        {
            _customTimeScale = customTimeScale;
        }

        public static IEnumerator WaitForSeconds(float _time)
        {
            for(float i = 0.0f; i < _time; i += Time.deltaTime * _customTimeScale)
            {
                yield return null;
            }
        }
    }
}