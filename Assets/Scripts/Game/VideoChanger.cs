using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Connections.Features;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;

public class VideoChanger : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip[] videoClips;
    
    
    private int currentClipIndex = 0;
    
    private void Start()
    {
        videoPlayer.clip = videoClips[currentClipIndex];
        videoPlayer.loopPointReached += OnVideoEnded;
        
    }
    
    [Button]
    public void NextVideo()
    {
        videoPlayer.Prepare();
        videoPlayer.Play();
    }
    
    private void OnVideoEnded(VideoPlayer vp)
    {
        currentClipIndex++;
        videoPlayer.clip = videoClips[currentClipIndex % videoClips.Length];
    }
}
