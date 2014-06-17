using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libcrypto.a", LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.ArmV7s, ForceLoad = true, IsCxx = true)]
