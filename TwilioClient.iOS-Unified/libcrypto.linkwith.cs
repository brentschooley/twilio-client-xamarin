using System;
using ObjCRuntime;

[assembly: LinkWith ("libcrypto.a", LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.ArmV7s, ForceLoad = false, SmartLink = true, IsCxx = true)] 
