using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Signals;
using UnityEngine;

public class CameraController : CinemachineExtension
{
    [Tooltip("Lock the camera's Z position to this value")]
    public float m_YPosition = 20;
 
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            pos.y = m_YPosition;
            state.RawPosition = pos;
        }
    }
}