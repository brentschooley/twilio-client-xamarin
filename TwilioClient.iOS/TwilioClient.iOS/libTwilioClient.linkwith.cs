using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libTwilioClient.a", LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator, ForceLoad = true, IsCxx = true, Frameworks = "AudioToolbox AVFoundation SystemConfiguration")]
