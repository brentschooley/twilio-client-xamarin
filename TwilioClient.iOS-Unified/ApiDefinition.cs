using System;
using ObjCRuntime;
using Foundation;

namespace TwilioClient.iOS
{
	[BaseType (typeof (NSObject), Name = "TCConnectionInternal",
		Delegates= new string [] {"WeakDelegate"},
		Events=new Type [] { typeof (TCConnectionDelegate) })]
	interface TCConnection {

		[Field ("TCConnectionIncomingParameterFromKey", "__Internal")]
		NSString FromKey { get; }

		[Field ("TCConnectionIncomingParameterToKey", "__Internal")]
		NSString ToKey { get; }

		[Field ("TCConnectionIncomingParameterAccountSIDKey", "__Internal")]
		NSString AccountSIDKey { get; }

		[Field ("TCConnectionIncomingParameterAPIVersionKey", "__Internal")]
		NSString APIVersionKey { get; }

		[Field ("TCConnectionIncomingParameterCallSIDKey", "__Internal")]
		NSString CallSIDKey { get; }

		[Export ("state")]
		TCConnectionState State { get; }

		[Export ("incoming")]
		bool Incoming { [Bind ("isIncoming")] get; }

		[Export ("parameters")]
		NSDictionary Parameters { get; }

		[Wrap ("WeakDelegate")][NullAllowed]
		TCConnectionDelegate Delegate { get; set; }

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Export ("muted")]
		bool Muted { [Bind ("isMuted")] get; set; }

		[Export ("accept")]
		void Accept ();

		[Export ("ignore")]
		void Ignore ();

		[Export ("reject")]
		void Reject ();

		[Export ("disconnect")]
		void Disconnect ();

		[Export ("sendDigits:")]
		void SendDigits (string digits);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface TCConnectionDelegate {

		[Abstract]
		[Export ("connection:didFailWithError:"), EventArgs ("TCConnectionError"), EventName("Failed")]
		void DidFail (TCConnection connection, NSError error);

		[Export ("connectionDidStartConnecting:"), EventArgs ("TCConnection"), EventName("StartedConnecting")]
		void DidStartConnecting (TCConnection connection);

		[Export ("connectionDidConnect:"), EventArgs ("TCConnection"), EventName("Connected")]
		void DidConnect (TCConnection connection);

		[Export ("connectionDidDisconnect:"), EventArgs ("TCConnection"), EventName("Disconnected")]
		void DidDisconnect (TCConnection connection);
	}

	[BaseType (typeof (NSObject),
		Delegates= new string [] {"WeakDelegate"},
		Events=new Type [] { typeof (TCDeviceDelegate) })]
	interface TCDevice {

		[Field ("TCDeviceCapabilityIncomingKey", "__Internal")]
		NSString IncomingKey { get; }

		[Field ("TCDeviceCapabilityOutgoingKey", "__Internal")]
		NSString OutgoingKey { get; }

		[Field ("TCDeviceCapabilityExpirationKey", "__Internal")]
		NSString ExpirationKey { get; }

		[Field ("TCDeviceCapabilityAccountSIDKey", "__Internal")]
		NSString AccountSIDKey { get; }

		[Field ("TCDeviceCapabilityApplicationSIDKey", "__Internal")]
		NSString ApplicationSIDKey { get; }

		[Field ("TCDeviceCapabilityApplicationParametersKey", "__Internal")]
		NSString ApplicationParametersKey { get; }

		[Field ("TCDeviceCapabilityClientNameKey", "__Internal")]
		NSString ClientNameKey { get; }

		[Export ("state")]
		TCDeviceState State { get; }

		[Export ("capabilities")]
		NSDictionary Capabilities { get; }

		[Wrap ("WeakDelegate")][NullAllowed]
		TCDeviceDelegate Delegate { get; set; }

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Export ("incomingSoundEnabled")]
		bool IncomingSoundEnabled { get; set; }

		[Export ("outgoingSoundEnabled")]
		bool OutgoingSoundEnabled { get; set; }

		[Export ("disconnectSoundEnabled")]
		bool DisconnectSoundEnabled { get; set; }

		[Export ("initWithCapabilityToken:delegate:")]
		IntPtr Constructor (string capabilityToken, [NullAllowed] TCDeviceDelegate aDelegate);

		[Export ("listen")]
		void Listen ();

		[Export ("unlisten")]
		void Unlisten ();

		[Export ("updateCapabilityToken:")]
		void UpdateCapabilityToken (string capabilityToken);

		[Export ("connect:delegate:")]
		TCConnection Connect ([NullAllowed] NSDictionary parameters, [NullAllowed] TCConnectionDelegate aDelegate);

		[Wrap ("Connect (parameters == null ? null : parameters.Dictionary, aDelegate)")]
		TCConnection Connect (TCConnectionParameters parameters, [NullAllowed] TCConnectionDelegate aDelegate);

		[Export ("disconnectAll")]
		void DisconnectAll ();
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface TCDeviceDelegate {

		[Abstract]
		[Export ("device:didStopListeningForIncomingConnections:"), EventArgs ("TCDeviceDelegateError"), EventName("StoppedListeningForIncomingConnections")]
		void DidStopListeningForIncomingConnections (TCDevice device, NSError error);

		[Export ("deviceDidStartListeningForIncomingConnections:"), EventArgs ("TCDeviceDelegate"), EventName("StartedListeningForIncomingConnections")]
		void DidStartListeningForIncomingConnections (TCDevice device);

		[Export ("device:didReceiveIncomingConnection:"), EventArgs ("TCDeviceDelegateConnection"), EventName("ReceivedIncomingConnection")]
		void DidReceiveIncomingConnection (TCDevice device, TCConnection connection);

		[Export ("device:didReceivePresenceUpdate:"), EventArgs ("TCDeviceDelegatePresence"), EventName("ReceivedPresenceUpdate")]
		void DidReceivePresenceUpdate (TCDevice device, TCPresenceEvent presenceEvent);
	}

	[BaseType (typeof (NSObject))]
	interface TCPresenceEvent {

		[Export ("name")]
		string Name { get; }

		[Export ("available")]
		bool Available { [Bind ("isAvailable")] get; }
	}
}

