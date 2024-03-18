using System;
using System.Collections.Generic;
using System.Windows.Forms;

using RobotManager.Utility.Debug;

using static RobotManager.Utility.Imports;

using Action = System.Action;
using Application = System.Windows.Forms.Application;

namespace RobotManager.Utility;

public static class Input {

	//-------------------
    //      Events
    //-------------------

    //KB
	public static event EventHandler<Keys> OnKeyPressed;

    //Mouse
    public static event EventHandler<EventArgs> OnMouseClickLeft;
    public static event EventHandler<EventArgs> OnMouseClickRight;
    public static event EventHandler<EventArgs> OnMouseClickMiddle;

	private static readonly List<KeyEvent> KeyEvents = [];
	private static readonly FilterInterface Filter = new();

	private class KeyEvent(ref Action keyAction, List<Keys> KeyData) {
		public readonly Action KeyAction = keyAction;
		public readonly List<Keys> KeyData = KeyData;
	}

	//-------------------
	//   Input
	//-------------------

    static Input() {
		Application.AddMessageFilter(Filter);
		Logger.Info("Init MessageFiter OK");
	}

	public static void RegisterKeyboardEvent(Action KeyAction, params Keys[] InputKeys) =>
		KeyEvents.Add(new(ref KeyAction, [..InputKeys]));


	//-------------------
	//   Message Filter
	//-------------------

    private class FilterInterface : IMessageFilter {
		public bool PreFilterMessage(ref Message PreMsg) {
			ProcessKeyMessage(ref PreMsg);
			ProcessMouseMessage(ref PreMsg);
			return false;
		}


		private void ProcessKeyMessage(ref Message WinMsg) {
			byte[] Keystate = new byte[256];
            //If Msg == Keydown & not held down.
			if (WinMsg.Msg == 0x100 && (WinMsg.LParam.ToInt32() & (1 << 30)) == 0) {
				//If WParam Somehow is not within a vKey range do nothing.
				if ((int)WinMsg.WParam > 255) return;
				Keys KeyCode = (Keys)WinMsg.WParam;

				//Fire Generic Event.
				OnKeyPressed?.Invoke(null, KeyCode);

				//Loop Through Each Registered Event and Process it...
				//This is O(n) with each registered Key event :/ a parallel foreach causes events to be fired too late...
				KeyEvents.ForEach(Event => {
					//Get State of all keys, if fail return
					if (!GetKeyboardState(Keystate)) return;
					//For Each Registered Key (Can be any vkey) Check if its Held down (0x80) bitmask the toggled state away (& 0xFE)
					//If true That Key Combo Has just been fully pressed, Async Call the registered ACtion
					if (Event.KeyData.TrueForAll(X => (Keystate[(int)X] & 0xFE) == 0x80)) {
						Event.KeyAction.BeginInvoke(Event.KeyAction.EndInvoke, null);
						Logger.Info($"Key Event: {KeyCode}");
					}
				});
			}
		}

		private void ProcessMouseMessage(ref Message WinMsg) {

			switch (WinMsg.Msg) {
				case 0x201: {   //Left Click Press
					OnMouseClickLeft?.Invoke(this, EventArgs.Empty);
					return;
				}
				case 0x204: {   //Right Click Press
					OnMouseClickRight?.Invoke(this, EventArgs.Empty);
					return;
				}
				case 0x207: {   //Middle Click Press
					OnMouseClickMiddle?.Invoke(this, EventArgs.Empty);
					return;
				}
			}
		}
	}
}