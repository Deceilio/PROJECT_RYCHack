using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Deceilio.Psychain
{
    public class WRLD_TIMELINE_MANAGER : MonoBehaviour
    {
        private PlayableDirector playableDirector;

        private void Start()
        {
            playableDirector = GetComponent<PlayableDirector>();
            playableDirector.stopped += OnTimelineStopped;
        }

        private void OnTimelineStopped(PlayableDirector director)
        {
            // BELOW CODE: Timeline has finished playing
            playableDirector.enabled = false;
        }
    }
}