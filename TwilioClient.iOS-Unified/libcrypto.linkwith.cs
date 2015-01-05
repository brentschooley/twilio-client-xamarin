using System;
using ObjCRuntime;

[assembly: LinkWith ("libcrypto.a", LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Arm64 | LinkTarget.Simulator64, ForceLoad = false, SmartLink = true, IsCxx = true)] 
