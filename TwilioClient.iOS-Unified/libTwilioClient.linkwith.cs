using System;
using ObjCRuntime;

[assembly: LinkWith ("libTwilioClient.a", 
	LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Arm64 | LinkTarget.Simulator64, 
	Frameworks="SystemConfiguration AVFoundation AudioToolbox CoreAudio",
	ForceLoad = false, 
	IsCxx = true,
	LinkerFlags = "-ObjC")]