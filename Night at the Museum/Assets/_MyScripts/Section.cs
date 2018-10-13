using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour {

    [SerializeField] private VideoController vc;
    [SerializeField] private Sphere360 sphere360;
    [SerializeField] private Waypoint waypoint;

    // TODO make the videos and subtitles here, just to make it easy assigning them anytime


    private void Start() {
        vc.SetUp(waypoint.GetComponent<AudioSource>());
        sphere360.SetUp(waypoint.transform);
        Sphere360.PauseAll += VideoController.PauseAllOtherVideoPlayers;
    }
}
