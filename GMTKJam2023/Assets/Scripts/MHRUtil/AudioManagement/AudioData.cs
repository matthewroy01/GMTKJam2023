using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioManagement
{
    [CreateAssetMenu(fileName = "new Audio Data", menuName = "AudioManagement/AudioData")]
    public class AudioData : ScriptableObject
    {
        [SerializeField] private List<ManagedAudio> _managedAudio = new List<ManagedAudio>();

        public ManagedAudio GetAudio(string name)
        {
            foreach(ManagedAudio managedAudio in _managedAudio)
            {
                if (managedAudio.Name == name)
                {
                    return managedAudio;
                }
            }

            Debug.LogWarning("Could not find audio with provided name " + name + ".");

            return null;
        }
    }
}