// Copyright (c) 2016 Charlie Gerhardus
//
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// 1. Redistributions of source code must retain the above copyright 
//    notice, this list of conditions and the following disclaimer.
// 
// 2. Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution.
// 
// 3. Neither the name of the copyright holder nor the names of its 
//    contributors may be used to endorse or promote products derived 
//    from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT 
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS 
// FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE 
// COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
// BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS 
// OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND 
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
// TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
// USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace Chromium.WebBrowser
{
	internal static class NativeWindow
	{
		private static NativeWindowPlatform platform = NativeWindowPlatform.Create();

		public static void SetPosition(IntPtr wnd, int x, int y, int w, int h)
		{
			platform.SetPosition(wnd, x, y, w, h);
		}

		public static void SetStyle(IntPtr wnd, int style)
		{
			platform.SetStyle(wnd, style);
		}

		public static void SetWatermark(IntPtr wnd, string str)
		{
			platform.SetWatermark(wnd, str);
		}
		private abstract class NativeWindowPlatform
		{
			public static NativeWindowPlatform Create()
			{
				switch (CfxRuntime.PlatformOS)
				{
				//TODO mac?
				case CfxPlatformOS.Windows:
					return new NativeWindowWindows();
				case CfxPlatformOS.Linux:
					return new NativeWindowLinux();
				default:
					throw new Exception("Unsupported platform!");
				}
			}

			public abstract void SetStyle(IntPtr wnd, int style);
			public abstract void SetPosition(IntPtr wnd, int x, int y, int w, int h);
			public abstract void SetWatermark(IntPtr wnd, string str);
		}

		private class NativeWindowWindows : NativeWindowPlatform
		{

			public override void SetPosition(IntPtr wnd, int x, int y, int w, int h)
			{
				SetWindowPos(wnd, IntPtr.Zero, x, y, w, h, SWP_NOMOVE | SWP_NOZORDER);
			}

			public override void SetStyle(IntPtr wnd, int style)
			{
				SetWindowLong(wnd, -16, (int)style);
			}

			public override void SetWatermark(IntPtr wnd, string str)
			{
				SendMessage(wnd, EM_SETCUEBANNER, 1, str);
			}

			public const uint SWP_NOMOVE = 0x2;
			public const uint SWP_NOZORDER = 0x4;

			public const int EM_SETCUEBANNER = 0x1501;

			[DllImport("user32.dll")]
			private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

			[DllImport("user32", SetLastError = false)]
			private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

			[DllImport("user32", SetLastError = false)]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
		}

		private class NativeWindowLinux : NativeWindowPlatform, IMessageFilter
		{

			public NativeWindowLinux()
			{
				Application.AddMessageFilter(this);
			}

			public bool PreFilterMessage(ref Message msg)
			{
				//FIXED Forcing focus to any control receiving a mouse click solves the focus stealing of a cef browser
				//TODO expand filter with more relevant msg id's (RBUTTONDOWN...)
				if (msg.Msg == WM_LBUTTONDOWN)
				{
					XSetInputFocus(CfxRuntime.Linux.GetXDisplay(), msg.HWnd, XInputFocus.RevertToNone, 0);
				}

				return false;
			}

			public override void SetPosition(IntPtr wnd, int x, int y, int w, int h)
			{
			    var ws = System.Windows.Forms.NativeWindow.FromHandle(wnd);
               
               

				XMoveResizeWindow(CfxRuntime.Linux.GetXDisplay(), wnd, x, y, w, h);
			}

			public override void SetStyle(IntPtr wnd, int style)
			{
				//TODO locate X11 SetWindowLong alternative!
			}

			public override void SetWatermark(IntPtr wnd, string str)
			{
				//TODO locate X11 EM_SETCUEBANNER alternative!
			}

			public const int WM_LBUTTONDOWN = 0x0201;

			public enum XInputFocus
			{
				RevertToNone = 0,
				RevertToPointerRoot = 1,
				RevertToParent = 2
			}

			[DllImport("libX11")]
			private static extern int XSetInputFocus(IntPtr display, IntPtr focus, XInputFocus revert_to, int time);

			[DllImport("libX11")]
			private static extern int XMoveResizeWindow(IntPtr display, IntPtr w, int x, int y, int width, int height);

		}
	}
}