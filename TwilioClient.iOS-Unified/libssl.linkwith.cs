using System;
using ObjCRuntime;

[assembly: LinkWith ("libssl.a", LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Arm64 | LinkTarget.Simulator64, ForceLoad = false, IsCxx = true)]