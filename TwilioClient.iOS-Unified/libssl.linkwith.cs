using System;
using ObjCRuntime;

[assembly: LinkWith ("libssl.a", LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.ArmV7s, ForceLoad = false, IsCxx = true)]