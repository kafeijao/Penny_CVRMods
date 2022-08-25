﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ABI_RC.Core.Player;
using MelonLoader;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CameraIndicator;

public static class BuildShit
{
    public const string Name = "CameraIndicator";
    public const string Author = "Penny";
    public const string Version = "1.0.1";
    public const string DownloadLink = "https://github.com/PennyBunny/CVRMods/";
    public const string Description = "A mod that gives you a indicator of where other players cameras are postioned";
}
    
public class CameraIndicator : MelonMod
{
    public static readonly MelonLogger.Instance Log = new(BuildShit.Name, ConsoleColor.DarkYellow);
    public static List<CVRPlayerEntity> CvrPlayerEntities = new();
    public static List<CameraObject> CameraObjects = new();
    public static GameObject Base;
    private static bool _ready;

    public override void OnApplicationStart()
    {
        Bundle.Init();
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (sceneName != "Headquarters" && !_ready) return;
        CvrPlayerEntities.Clear();
        CameraObjects.Clear();
        Base = new GameObject("CameraIndicator");
        _ready = true;
    }

    public override void OnLateUpdate()
    {
        foreach (var cameraObject in CameraObjects)
        {
            var player = CvrPlayerEntities.FirstOrDefault(player => player.Username == cameraObject.CamTran.name);
            if (player == null) return;
            if (player.PuppetMaster.PlayerAvatarMovementDataInput.CameraEnabled)
            {
                cameraObject.CamTran.SetActive(true);
            }
            else
            {
                cameraObject.CamTran.SetActive(false);
                continue;
            }
            cameraObject.CamTran.transform.position = Vector3.Lerp(cameraObject.CamTran.transform.position, player.PuppetMaster.PlayerAvatarMovementDataInput.CameraPosition, 20f * Time.deltaTime);
            cameraObject.CamRot.transform.rotation = Quaternion.Lerp(cameraObject.CamRot.transform.rotation, Quaternion.Euler(player.PuppetMaster.PlayerAvatarMovementDataInput.CameraRotation + new Vector3(90f, 0, 0)), 20f * Time.deltaTime);
            cameraObject.NameTag.transform.LookAt(Camera.main.transform);
        }
    }
}