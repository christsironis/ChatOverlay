using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;


namespace ChatOverlay
{
    public class CustomWindow : System.Windows.Window
    {
        private Point cursorOffset;
        private double restoreTop;

        private FrameworkElement borderLeft;
        private FrameworkElement borderTopLeft;
        private FrameworkElement borderRight;
        private FrameworkElement borderTopRight;
        private FrameworkElement borderTop;
        private FrameworkElement borderBottomLeft;
        private FrameworkElement borderBottomRight;
        private FrameworkElement borderBottom;
        private FrameworkElement caption;
        private FrameworkElement frame;
        private Button minimizeButton;
        private Button maximizeButton;
        private Button closeButton;
        private IntPtr handle;

        public CustomWindow()
        {
            SourceInitialized += (sender, e) =>
            {
                handle = new WindowInteropHelper(this).Handle;
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WndProc));
            };
            Style = (Style)TryFindResource("CustomWindowStyle");
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            RegisterFrame();
            RegisterBorder();
            RegisterCaption();
            RegisterMinimizeButton();
            RegisterMaximizeButton();
            RegisterCloseButton();
        }

        private void RegisterCloseButton()
        {
            closeButton = (Button)GetTemplateChild("PART_WindowCaptionCloseButton");
            if (closeButton != null)
            {
                closeButton.Click += (sender, e) => Application.Current.Shutdown();
            }
        }

        private void RegisterMaximizeButton()
        {
            maximizeButton = (Button)GetTemplateChild("PART_WindowCaptionMaximizeButton");
            if (maximizeButton != null)
            {
                maximizeButton.Click += (sender, e) =>
                {
                    if (WindowState == System.Windows.WindowState.Normal)
                    {
                        WindowState = System.Windows.WindowState.Maximized;
                    }
                    else
                    {
                        WindowState = System.Windows.WindowState.Normal;
                    }
                };
            }
        }

        private void RegisterMinimizeButton()
        {
            minimizeButton = (Button)GetTemplateChild("PART_WindowCaptionMinimizeButton");
            if (minimizeButton != null)
            {
                minimizeButton.Click += (sender, e) => WindowState = System.Windows.WindowState.Minimized;
            }
        }

        private void RegisterBoderEvents(WindowBorderEdge borderEdge, FrameworkElement border)
        {
            border.MouseEnter += (sender, e) =>
            {
                if (WindowState != WindowState.Maximized && ResizeMode == ResizeMode.CanResize)
                {
                    switch (borderEdge)
                    {
                        case WindowBorderEdge.Left:
                        case WindowBorderEdge.Right:
                            border.Cursor = Cursors.SizeWE;
                            break;
                        case WindowBorderEdge.Top:
                        case WindowBorderEdge.Bottom:
                            border.Cursor = Cursors.SizeNS;
                            break;
                        case WindowBorderEdge.TopLeft:
                        case WindowBorderEdge.BottomRight:
                            border.Cursor = Cursors.SizeNWSE;
                            break;
                        case WindowBorderEdge.TopRight:
                        case WindowBorderEdge.BottomLeft:
                            border.Cursor = Cursors.SizeNESW;
                            break;
                    }
                }
                else
                {
                    border.Cursor = Cursors.Arrow;
                }
            };

            border.MouseLeftButtonDown += (sender, e) =>
            {
                if (WindowState != WindowState.Maximized && ResizeMode == ResizeMode.CanResize)
                {
                    Point curseorLocation = e.GetPosition(this);
                    Point cursorOffset = new Point();

                    switch (borderEdge)
                    {
                        case WindowBorderEdge.Left:
                            cursorOffset.X = curseorLocation.X;
                            break;
                        case WindowBorderEdge.Right:
                            cursorOffset.X = (Width - cursorOffset.X);
                            break;
                        case WindowBorderEdge.Top:
                            cursorOffset.Y = curseorLocation.Y;
                            break;
                        case WindowBorderEdge.Bottom:
                            cursorOffset.Y = (Height - curseorLocation.Y);
                            break;
                        case WindowBorderEdge.TopLeft:
                            cursorOffset.Y = curseorLocation.Y;
                            cursorOffset.X = curseorLocation.X;
                            break;
                        case WindowBorderEdge.BottomRight:
                            cursorOffset.Y = (Height - curseorLocation.Y);
                            cursorOffset.X = (Width - curseorLocation.X);
                            break;
                        case WindowBorderEdge.TopRight:
                            cursorOffset.Y = curseorLocation.Y;
                            cursorOffset.X = (Width - cursorOffset.X);
                            break;
                        case WindowBorderEdge.BottomLeft:
                            cursorOffset.X = curseorLocation.X;
                            cursorOffset.Y = (Height - curseorLocation.Y);
                            break;

                    }

                    this.cursorOffset = cursorOffset;

                    border.CaptureMouse();
                }
            };

            border.MouseMove += (sender, e) =>
            {
                if (WindowState != WindowState.Minimized && border.IsMouseCaptured && ResizeMode == ResizeMode.CanResize)
                {
                    Point cursorLocation = e.GetPosition(this);

                    double nHorizontalChange = (cursorLocation.X - cursorOffset.X);
                    double pHorizontalChange = (cursorLocation.X + cursorOffset.X);
                    double nVerticalChange = (cursorLocation.Y - cursorOffset.Y);
                    double pVerticalChange = (cursorLocation.Y + cursorOffset.Y);

                    switch (borderEdge)
                    {
                        case WindowBorderEdge.Left:
                            if (Width - nHorizontalChange <= MinWidth)
                                break;
                            Left += nHorizontalChange;
                            Width -= nHorizontalChange;
                            break;
                        case WindowBorderEdge.TopLeft:
                            if (Width - nHorizontalChange <= MinWidth)
                                break;
                            Left += nHorizontalChange;
                            Width -= nHorizontalChange;
                            if (Height - nVerticalChange <= MinHeight)
                                break;
                            Top += nVerticalChange;
                            Height -= nVerticalChange;
                            break;
                        case WindowBorderEdge.Top:
                            if (Height - nVerticalChange <= MinHeight)
                                break;
                            Top += nVerticalChange;
                            Height -= nVerticalChange;
                            break;
                        case WindowBorderEdge.TopRight:
                            if (pHorizontalChange <= MinWidth)
                                break;
                            Width = pHorizontalChange;
                            if (Height - nVerticalChange <= MinHeight)
                                break;
                            Top += nVerticalChange;
                            Height -= nVerticalChange;
                            break;
                        case WindowBorderEdge.Right:
                            if (pHorizontalChange <= MinWidth)
                                break;
                            Width = pHorizontalChange;
                            break;
                        case WindowBorderEdge.BottomRight:
                            if (pHorizontalChange <= MinWidth)
                                break;
                            Width = pHorizontalChange;
                            if (pVerticalChange <= MinHeight)
                                break;
                            Height = pVerticalChange;
                            break;
                        case WindowBorderEdge.Bottom:
                            if (pVerticalChange <= MinHeight)
                                break;
                            Height = pVerticalChange;
                            break;
                        case WindowBorderEdge.BottomLeft:
                            if (Width - nHorizontalChange <= MinWidth)
                                break;
                            Left += nHorizontalChange;
                            Width -= nHorizontalChange;
                            if (pVerticalChange <= MinHeight)
                                break;
                            Height = pVerticalChange;
                            break;
                    }
                }
            };

            border.MouseLeftButtonDown += (sender, e) =>
            {
                border.ReleaseMouseCapture();
            };
        }
        private void RegisterBorder()
        {
            borderLeft =        (FrameworkElement)GetTemplateChild("PART_WindowBorderLeft");
            borderRight =       (FrameworkElement)GetTemplateChild("PART_WindowBorderRight");
            borderTopLeft =     (FrameworkElement)GetTemplateChild("PART_WindowBorderTopLeft");
            borderTopRight =    (FrameworkElement)GetTemplateChild("PART_WindowBorderTopRght");
            borderTop =         (FrameworkElement)GetTemplateChild("PART_WindowBorderTop");
            borderBottomRight = (FrameworkElement)GetTemplateChild("PART_WindowBorderBottomRight");
            borderBottomLeft =  (FrameworkElement)GetTemplateChild("PART_WindowBorderBottomLeft");
            borderBottom =      (FrameworkElement)GetTemplateChild("PART_WindowBorderBottom");

            RegisterBoderEvents(WindowBorderEdge.Left, borderLeft);
            RegisterBoderEvents(WindowBorderEdge.Right, borderRight);
            RegisterBoderEvents(WindowBorderEdge.TopLeft, borderTopLeft);
            RegisterBoderEvents(WindowBorderEdge.TopRight, borderTopRight);
            RegisterBoderEvents(WindowBorderEdge.Top, borderTop);
            RegisterBoderEvents(WindowBorderEdge.BottomRight, borderBottomRight);
            RegisterBoderEvents(WindowBorderEdge.BottomLeft, borderBottomLeft);
            RegisterBoderEvents(WindowBorderEdge.Bottom, borderBottom);
        }

        private void RegisterCaption()
        {
            caption = (FrameworkElement)GetTemplateChild("PART_WindowCaption");

            if(caption != null)
            {
                caption.MouseLeftButtonDown += (sender, e) =>
                {
                    restoreTop = e.GetPosition(this).Y;

                    if (e.ClickCount == 2 && e.ChangedButton == System.Windows.Input.MouseButton.Left && (ResizeMode != ResizeMode.CanMinimize && ResizeMode != ResizeMode.NoResize))
                    {
                        if (WindowState != System.Windows.WindowState.Maximized)
                        {
                            if (WindowState != System.Windows.WindowState.Maximized)
                            {
                                WindowState = System.Windows.WindowState.Maximized;
                            }
                            else
                            {
                                WindowState = System.Windows.WindowState.Normal;
                            }
                            return;
                        }
                    }
                    DragMove();
                };

                caption.MouseMove += (sender, e) =>
                {
                    if (e.LeftButton == MouseButtonState.Pressed && caption.IsMouseOver)
                    {
                        if (WindowState == WindowState.Maximized)
                        {
                            WindowState = WindowState.Normal;
                            Top = restoreTop - 10;
                            DragMove();
                        }
                    }
                };
            }
        }

        private void RegisterFrame()
        {
            frame = (FrameworkElement)GetTemplateChild("PART_WindowFrame");
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParm, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }
            return IntPtr.Zero;
        }

        private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = NativeMethods.MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != System.IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                NativeMethods.GetMonitorInfo(monitor, monitorInfo);

                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;

                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }

        private enum WindowBorderEdge
        {
            Left,
            Right,
            Top,
            TopLeft,
            TopRight,
            Bottom,
            BottomLeft,
            BottomRight
        }
    }
}
