using System;
using ObjCRuntime;

[assembly: LinkWith ("libTwilioClient.a", 
	LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.ArmV7s, 
	Frameworks="SystemConfiguration AVFoundation AudioToolbox CoreAudio",
	ForceLoad = false, 
	IsCxx = true,
	LinkerFlags = "-ObjC")]