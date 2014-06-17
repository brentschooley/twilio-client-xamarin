using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using TwilioClient.iOS;
using System.Net.Http;
using System.Diagnostics;
using AlertView;

namespace TwilioClientTest.iOS
{
	public class StatusStringElement : StringElement {
		public StatusStringElement(string caption):base(caption){}
		public StatusStringElement(string caption, string value):base(caption, value){}
		public StatusStringElement(string caption, NSAction tapped):base(caption, tapped){}

		public void SetValueAndUpdate (string value) 
		{
			Value = value;
			if (GetContainerTableView () != null) 
			{
				var root = GetImmediateRootElement ();
				root.Reload (this, UITableViewRowAnimation.Fade);
			}
		}
	}
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		TCConnection connection;
		TCDevice device;

		BooleanElement disconnectSoundEnabled, incomingCallSoundEnabled, outgoingCallSoundEnabled, muted, listening;
		StatusStringElement deviceState, connectionState;
		StyledStringElement callButton;
		EntryElement numberOrClient;

		// class-level declarations
		public override UIWindow Window {
			get;
			set;
		}

		public async override void FinishedLaunching (UIApplication application)
		{
			this.Window = new UIWindow (UIScreen.MainScreen.Bounds);

			var client = new HttpClient ();
			var token = await client.GetStringAsync("http://twilioclientazure.azurewebsites.net/Client/Token?clientName=xamarin");

			device = new TCDevice (token, null);
		
			var dialogController = new DialogViewController(new RootElement("Twilio Client Test") {
				new Section ("Call Options") {
					(numberOrClient = new EntryElement("Target", "Phone # or client name", "")),
					(callButton = new StyledStringElement("Call", delegate {
						NSDictionary param = NSDictionary.FromObjectsAndKeys (
							new object[] { "+14848015076", numberOrClient.Value },
							new object[] { "Source", "Target" }
						);

						connection = device.Connect(param, null);

						SetupConnectionEvents ();
					})),
				},
				new Section ("Status") {
					(deviceState = new StatusStringElement("Device", device.State.ToString())),
					(connectionState = new StatusStringElement("Connection", "Uninitialized"))
				},

				new Section("Sounds") {
					(disconnectSoundEnabled = new BooleanElement("Disconnect Sound", device.DisconnectSoundEnabled)),
					(incomingCallSoundEnabled = new BooleanElement("Incoming Sound", device.IncomingSoundEnabled)),
					(outgoingCallSoundEnabled = new BooleanElement("Outgoing Sound", device.OutgoingSoundEnabled))
				},

				new Section("Options") {
					(muted = new BooleanElement("Muted", false)),
					(listening = new BooleanElement("Device Listening", true))
				}
			});

			callButton.Alignment = UITextAlignment.Center;
			callButton.TextColor = UIColor.FromRGB (0x00, 0x7A, 0xFF);

			var navigationController = new UINavigationController (dialogController);
			//navigationController.NavigationBar.BarTintColor = UIColor.Red;
			//navigationController.NavigationBar.TintColor = UIColor.White;

			Window.RootViewController = navigationController;

			Window.MakeKeyAndVisible ();

			SetupSoundOptionEvents ();
			SetupOptions ();
			SetupDeviceEvents ();
			device.Listen ();
		}

		void SetupSoundOptionEvents ()
		{
			disconnectSoundEnabled.ValueChanged += (sender, e) =>  {
				if (device != null) {
					device.DisconnectSoundEnabled = disconnectSoundEnabled.Value;
				}
				Debug.WriteLine ("disconnectSoundEnabled: " + device.DisconnectSoundEnabled);
			};
			incomingCallSoundEnabled.ValueChanged += (sender, e) =>  {
				if (device != null) {
					device.IncomingSoundEnabled = incomingCallSoundEnabled.Value;
				}
				Debug.WriteLine ("incomingSoundEnabled: " + device.IncomingSoundEnabled);
			};
			outgoingCallSoundEnabled.ValueChanged += (sender, e) =>  {
				if (device != null) {
					device.OutgoingSoundEnabled = outgoingCallSoundEnabled.Value;
				}
				Debug.WriteLine ("outgoingSoundEnabled: " + device.OutgoingSoundEnabled);
			};
		}

		void SetupOptions() {
			muted.ValueChanged += (sender, e) => {
				if(connection != null && connection.State == TCConnectionState.Connected)
				{
					connection.Muted = muted.Value;
				}
			};

			listening.ValueChanged += (sender, e) => {
				if(device != null)
				{
					if(listening.Value)
					{
						device.Listen();
					}
					else
					{
						device.Unlisten();
					}
				}
			};
		}

		void SetupDeviceEvents() {
			device.StoppedListeningForIncomingConnections += delegate {
				UpdateStatus ();
			};

			device.StartedListeningForIncomingConnections += delegate {
				UpdateStatus();
			};

			device.ReceivedIncomingConnection += (sender, e) => {
				if(connection != null && connection.State == TCConnectionState.Connected) {
					connection.Disconnect();
				}

				connection = e.Connection;
				SetupConnectionEvents();
				UpdateStatus();

				var alert = MBAlertView.AlertWithBody (
					body: "Incoming call",
					buttonTitle: "Answer",
					handler: () => connection.Accept()
				);

				alert.AddButtonWithText (
					text: "Reject", 
					bType:MBAlertViewItemType.Destructive,
					handler: () => connection.Reject()
				);
				//alert.Size = new System.Drawing.SizeF(200, 300);
				alert.AddToDisplayQueue();
			};

			device.ReceivedPresenceUpdate += delegate {
				Debug.WriteLine("Received Presence Update");
			};
		}

		void SetupConnectionEvents ()
		{
			if (connection != null) {
			connection.Failed += delegate {
				UpdateStatus ();
			};
			connection.StartedConnecting += delegate {
				UpdateStatus ();
			};
			connection.Connected += delegate {
				UpdateStatus ();
			};
			connection.Disconnected += delegate {
				UpdateStatus ();
				connection = null;
			};
			}
		}

		public void UpdateStatus() {
			InvokeOnMainThread (delegate {
				deviceState.SetValueAndUpdate (device.State.ToString ());

				if (connection != null) {
					connectionState.SetValueAndUpdate (connection.State.ToString ());
				}
			});
		}

		// This method is invoked when the application is about to move from active to inactive state.
		// OpenGL applications should use this method to pause.
		public override void OnResignActivation (UIApplication application)
		{
		}
		// This method should be used to release shared resources and it should store the application state.
		// If your application supports background exection this method is called instead of WillTerminate
		// when the user quits.
		public override void DidEnterBackground (UIApplication application)
		{
		}
		// This method is called as part of the transiton from background to active state.
		public override void WillEnterForeground (UIApplication application)
		{
		}
		// This method is called when the application is about to terminate. Save data, if needed.
		public override void WillTerminate (UIApplication application)
		{
		}
	}
}

