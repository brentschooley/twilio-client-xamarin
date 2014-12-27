using System;
using Foundation;

namespace TwilioClient.iOS
{
	public enum TCConnectionState {
		Pending = 0,
		Connecting,
		Connected,
		Disconnected
	}

	public enum TCDeviceState {
		Offline = 0,
		Ready,
		Busy
	}
}