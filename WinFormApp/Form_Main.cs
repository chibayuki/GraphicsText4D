/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2018 chibayuki@foxmail.com

4D绘图测试
Version 18.7.10.0000

This file is part of "4D绘图测试" (GraphicsText4D)

"4D绘图测试" (GraphicsText4D) is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Drawing2D;

namespace WinFormApp
{
    public partial class Form_Main : Form
    {
        #region 版本信息

        private static readonly string ApplicationName = Application.ProductName; // 程序名。
        private static readonly string ApplicationEdition = "18"; // 程序版本。

        private static readonly Int32 MajorVersion = new Version(Application.ProductVersion).Major; // 主版本。
        private static readonly Int32 MinorVersion = new Version(Application.ProductVersion).Minor; // 副版本。
        private static readonly Int32 BuildNumber = new Version(Application.ProductVersion).Build; // 版本号。
        private static readonly Int32 BuildRevision = new Version(Application.ProductVersion).Revision; // 修订版本。
        private static readonly string LabString = "4D"; // 分支名。
        private static readonly string BuildTime = "180710-0000"; // 编译时间。

        #endregion

        #region 窗体构造

        private Com.WinForm.FormManager Me;

        public Com.WinForm.FormManager FormManager
        {
            get
            {
                return Me;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x00020000;

                CreateParams CP = base.CreateParams;

                if (Me != null && Me.FormStyle != Com.WinForm.FormStyle.Dialog)
                {
                    CP.Style = CP.Style | WS_MINIMIZEBOX;
                }

                return CP;
            }
        }

        private void _Ctor(Com.WinForm.FormManager owner)
        {
            InitializeComponent();

            //

            if (owner != null)
            {
                Me = new Com.WinForm.FormManager(this, owner);
            }
            else
            {
                Me = new Com.WinForm.FormManager(this);
            }

            //

            FormDefine();
        }

        public Form_Main()
        {
            _Ctor(null);
        }

        public Form_Main(Com.WinForm.FormManager owner)
        {
            _Ctor(owner);
        }

        private void FormDefine()
        {
            Me.Caption = ApplicationName;
            Me.FormStyle = Com.WinForm.FormStyle.Sizable;
            Me.EnableFullScreen = true;
            Me.ClientSize = new Size(800, 450);
            Me.Theme = Com.WinForm.Theme.Colorful;
            Me.ThemeColor = Com.ColorManipulation.GetRandomColorX();
            Me.FormState = Com.WinForm.FormState.Maximized;

            Me.Loaded += LoadedEvents;
            Me.Resize += ResizeEvents;
            Me.SizeChanged += SizeChangedEvents;
            Me.ThemeChanged += ThemeColorChangedEvents;
            Me.ThemeColorChanged += ThemeColorChangedEvents;
        }

        #endregion

        #region 窗体事件

        private void LoadedEvents(object sender, EventArgs e)
        {
            //
            // 在窗体加载后发生。
            //

            Me.OnSizeChanged();
            Me.OnThemeChanged();

            Panel_GraphArea.Visible = true;
        }

        private void ResizeEvents(object sender, EventArgs e)
        {
            //
            // 在窗体的大小调整时发生。
            //

            Panel_GraphArea.Size = Panel_Client.Size = Panel_Main.Size;

            Panel_Control.Location = new Point(Math.Max(0, Math.Min(Panel_Control.Left, Panel_Client.Width - Label_Control_SubFormTitle.Right)), Math.Max(0, Math.Min(Panel_Control.Top, Panel_Client.Height - Label_Control_SubFormTitle.Bottom)));
        }

        private void SizeChangedEvents(object sender, EventArgs e)
        {
            //
            // 在窗体的大小更改时发生。
            //

            RepaintBmp();
        }

        private void ThemeColorChangedEvents(object sender, EventArgs e)
        {
            //
            // 在窗体的主题色更改时发生。
            //

            Panel_Control.BackColor = Me.RecommendColors.Background_DEC.ToColor();

            Label_Control_SubFormTitle.ForeColor = Me.RecommendColors.Caption.ToColor();
            Label_Control_SubFormTitle.BackColor = Me.RecommendColors.CaptionBar.ToColor();

            //

            Label_Size.ForeColor = Label_Rotation.ForeColor = Me.RecommendColors.Text.ToColor();

            Label_Sx.ForeColor = Label_Sy.ForeColor = Label_Sz.ForeColor = Label_Su.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_Sx.BackColor = Label_Sy.BackColor = Label_Sz.BackColor = Label_Su.BackColor = Me.RecommendColors.Button.ToColor();

            Label_Rxy.ForeColor = Label_Ryz.ForeColor = Label_Rzu.ForeColor = Label_Rux.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_Rxy.BackColor = Label_Ryz.BackColor = Label_Rzu.BackColor = Label_Rux.BackColor = Me.RecommendColors.Button.ToColor();

            Label_Rx.ForeColor = Label_Ry.ForeColor = Label_Rz.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_Rx.BackColor = Label_Ry.BackColor = Label_Rz.BackColor = Me.RecommendColors.Button.ToColor();
        }

        #endregion

        #region 4D绘图

        private void AffineTransform(ref Com.PointD3D Pt, Com.PointD3D Origin, double[,] AffineMatrix)
        {
            //
            // 将一个 3D 坐标以指定点为新的原点进行仿射变换。
            //

            Pt -= Origin;
            Pt.AffineTransform(AffineMatrix);
            Pt += Origin;
        }

        private void AffineTransform(ref Com.PointD4D Pt, Com.PointD4D Origin, double[,] AffineMatrix)
        {
            //
            // 将一个 4D 坐标以指定点为新的原点进行仿射变换。
            //

            Pt -= Origin;
            Pt.AffineTransform(AffineMatrix);
            Pt += Origin;
        }

        private enum Views // 视角枚举。
        {
            XYZ_XY,
            XYZ_YZ,
            XYZ_ZX,

            YZU_XY,
            YZU_YZ,
            YZU_ZX,

            ZUX_XY,
            ZUX_YZ,
            ZUX_ZX,

            UXY_XY,
            UXY_YZ,
            UXY_ZX
        }

        private Bitmap GetProjectionOfTesseract(Com.PointD4D TesseractSize, double[,] AffineMatrix4D, double[,] AffineMatrix3D, Views View, SizeF ImageSize)
        {
            //
            // 获取超立方体的投影。
            //

            double TesseractDiag = Math.Min(ImageSize.Width, ImageSize.Height);

            TesseractSize = TesseractSize.VectorNormalize * TesseractDiag;

            Bitmap ProjBmp = new Bitmap(Math.Max(1, (Int32)ImageSize.Width), Math.Max(1, (Int32)ImageSize.Height));

            Graphics CreateProjBmp = Graphics.FromImage(ProjBmp);

            CreateProjBmp.SmoothingMode = SmoothingMode.AntiAlias;

            //

            Com.PointD4D TesseractCenter = new Com.PointD4D(0, 0, 0, 0);

            Com.PointD4D P4D_0000 = new Com.PointD4D(0, 0, 0, 0);
            Com.PointD4D P4D_1000 = new Com.PointD4D(1, 0, 0, 0);
            Com.PointD4D P4D_0100 = new Com.PointD4D(0, 1, 0, 0);
            Com.PointD4D P4D_1100 = new Com.PointD4D(1, 1, 0, 0);
            Com.PointD4D P4D_0010 = new Com.PointD4D(0, 0, 1, 0);
            Com.PointD4D P4D_1010 = new Com.PointD4D(1, 0, 1, 0);
            Com.PointD4D P4D_0110 = new Com.PointD4D(0, 1, 1, 0);
            Com.PointD4D P4D_1110 = new Com.PointD4D(1, 1, 1, 0);
            Com.PointD4D P4D_0001 = new Com.PointD4D(0, 0, 0, 1);
            Com.PointD4D P4D_1001 = new Com.PointD4D(1, 0, 0, 1);
            Com.PointD4D P4D_0101 = new Com.PointD4D(0, 1, 0, 1);
            Com.PointD4D P4D_1101 = new Com.PointD4D(1, 1, 0, 1);
            Com.PointD4D P4D_0011 = new Com.PointD4D(0, 0, 1, 1);
            Com.PointD4D P4D_1011 = new Com.PointD4D(1, 0, 1, 1);
            Com.PointD4D P4D_0111 = new Com.PointD4D(0, 1, 1, 1);
            Com.PointD4D P4D_1111 = new Com.PointD4D(1, 1, 1, 1);

            P4D_0000 = (P4D_0000 - 0.5) * TesseractSize + TesseractCenter;
            P4D_1000 = (P4D_1000 - 0.5) * TesseractSize + TesseractCenter;
            P4D_0100 = (P4D_0100 - 0.5) * TesseractSize + TesseractCenter;
            P4D_1100 = (P4D_1100 - 0.5) * TesseractSize + TesseractCenter;
            P4D_0010 = (P4D_0010 - 0.5) * TesseractSize + TesseractCenter;
            P4D_1010 = (P4D_1010 - 0.5) * TesseractSize + TesseractCenter;
            P4D_0110 = (P4D_0110 - 0.5) * TesseractSize + TesseractCenter;
            P4D_1110 = (P4D_1110 - 0.5) * TesseractSize + TesseractCenter;
            P4D_0001 = (P4D_0001 - 0.5) * TesseractSize + TesseractCenter;
            P4D_1001 = (P4D_1001 - 0.5) * TesseractSize + TesseractCenter;
            P4D_0101 = (P4D_0101 - 0.5) * TesseractSize + TesseractCenter;
            P4D_1101 = (P4D_1101 - 0.5) * TesseractSize + TesseractCenter;
            P4D_0011 = (P4D_0011 - 0.5) * TesseractSize + TesseractCenter;
            P4D_1011 = (P4D_1011 - 0.5) * TesseractSize + TesseractCenter;
            P4D_0111 = (P4D_0111 - 0.5) * TesseractSize + TesseractCenter;
            P4D_1111 = (P4D_1111 - 0.5) * TesseractSize + TesseractCenter;

            Com.PointD4D RotateCenter4D = TesseractCenter;

            AffineTransform(ref P4D_0000, RotateCenter4D, AffineMatrix4D);
            AffineTransform(ref P4D_1000, RotateCenter4D, AffineMatrix4D);
            AffineTransform(ref P4D_0100, RotateCenter4D, AffineMatrix4D);
            AffineTransform(ref P4D_1100, RotateCenter4D, AffineMatrix4D);
            AffineTransform(ref P4D_0010, RotateCenter4D, AffineMatrix4D);
            AffineTransform(ref P4D_1010, RotateCenter4D, AffineMatrix4D);
            AffineTransform(ref P4D_0110, RotateCenter4D, AffineMatrix4D);
            AffineTransform(ref P4D_1110, RotateCenter4D, AffineMatrix4D);
            AffineTransform(ref P4D_0001, RotateCenter4D, AffineMatrix4D);
            AffineTransform(ref P4D_1001, RotateCenter4D, AffineMatrix4D);
            AffineTransform(ref P4D_0101, RotateCenter4D, AffineMatrix4D);
            AffineTransform(ref P4D_1101, RotateCenter4D, AffineMatrix4D);
            AffineTransform(ref P4D_0011, RotateCenter4D, AffineMatrix4D);
            AffineTransform(ref P4D_1011, RotateCenter4D, AffineMatrix4D);
            AffineTransform(ref P4D_0111, RotateCenter4D, AffineMatrix4D);
            AffineTransform(ref P4D_1111, RotateCenter4D, AffineMatrix4D);

            //

            double TrueLenDist4D = new Com.PointD(Screen.PrimaryScreen.Bounds.Size).VectorModule;

            Com.PointD4D PrjCenter4D = TesseractCenter;

            switch (View)
            {
                case Views.XYZ_XY:
                case Views.XYZ_YZ:
                case Views.XYZ_ZX:
                    PrjCenter4D.U -= (TrueLenDist4D + TesseractDiag / 2);
                    break;

                case Views.YZU_XY:
                case Views.YZU_YZ:
                case Views.YZU_ZX:
                    PrjCenter4D.X -= (TrueLenDist4D + TesseractDiag / 2);
                    break;

                case Views.ZUX_XY:
                case Views.ZUX_YZ:
                case Views.ZUX_ZX:
                    PrjCenter4D.Y -= (TrueLenDist4D + TesseractDiag / 2);
                    break;

                case Views.UXY_XY:
                case Views.UXY_YZ:
                case Views.UXY_ZX:
                    PrjCenter4D.Z -= (TrueLenDist4D + TesseractDiag / 2);
                    break;
            }

            Com.PointD3D P3D_0000 = new Com.PointD3D();
            Com.PointD3D P3D_1000 = new Com.PointD3D();
            Com.PointD3D P3D_0100 = new Com.PointD3D();
            Com.PointD3D P3D_1100 = new Com.PointD3D();
            Com.PointD3D P3D_0010 = new Com.PointD3D();
            Com.PointD3D P3D_1010 = new Com.PointD3D();
            Com.PointD3D P3D_0110 = new Com.PointD3D();
            Com.PointD3D P3D_1110 = new Com.PointD3D();
            Com.PointD3D P3D_0001 = new Com.PointD3D();
            Com.PointD3D P3D_1001 = new Com.PointD3D();
            Com.PointD3D P3D_0101 = new Com.PointD3D();
            Com.PointD3D P3D_1101 = new Com.PointD3D();
            Com.PointD3D P3D_0011 = new Com.PointD3D();
            Com.PointD3D P3D_1011 = new Com.PointD3D();
            Com.PointD3D P3D_0111 = new Com.PointD3D();
            Com.PointD3D P3D_1111 = new Com.PointD3D();

            Func<Com.PointD4D, Com.PointD4D, double, Com.PointD3D> GetProject4D = (Pt, PrjCenter, TrueLenDist) =>
            {
                switch (View)
                {
                    case Views.XYZ_XY:
                    case Views.XYZ_YZ:
                    case Views.XYZ_ZX:
                        return Pt.ProjectToXYZ(PrjCenter, TrueLenDist);

                    case Views.YZU_XY:
                    case Views.YZU_YZ:
                    case Views.YZU_ZX:
                        return Pt.ProjectToYZU(PrjCenter, TrueLenDist);

                    case Views.ZUX_XY:
                    case Views.ZUX_YZ:
                    case Views.ZUX_ZX:
                        return Pt.ProjectToZUX(PrjCenter, TrueLenDist);

                    case Views.UXY_XY:
                    case Views.UXY_YZ:
                    case Views.UXY_ZX:
                        return Pt.ProjectToUXY(PrjCenter, TrueLenDist);

                    default:
                        return Com.PointD3D.NaN;
                }
            };

            P3D_0000 = GetProject4D(P4D_0000, PrjCenter4D, TrueLenDist4D);
            P3D_1000 = GetProject4D(P4D_1000, PrjCenter4D, TrueLenDist4D);
            P3D_0100 = GetProject4D(P4D_0100, PrjCenter4D, TrueLenDist4D);
            P3D_1100 = GetProject4D(P4D_1100, PrjCenter4D, TrueLenDist4D);
            P3D_0010 = GetProject4D(P4D_0010, PrjCenter4D, TrueLenDist4D);
            P3D_1010 = GetProject4D(P4D_1010, PrjCenter4D, TrueLenDist4D);
            P3D_0110 = GetProject4D(P4D_0110, PrjCenter4D, TrueLenDist4D);
            P3D_1110 = GetProject4D(P4D_1110, PrjCenter4D, TrueLenDist4D);
            P3D_0001 = GetProject4D(P4D_0001, PrjCenter4D, TrueLenDist4D);
            P3D_1001 = GetProject4D(P4D_1001, PrjCenter4D, TrueLenDist4D);
            P3D_0101 = GetProject4D(P4D_0101, PrjCenter4D, TrueLenDist4D);
            P3D_1101 = GetProject4D(P4D_1101, PrjCenter4D, TrueLenDist4D);
            P3D_0011 = GetProject4D(P4D_0011, PrjCenter4D, TrueLenDist4D);
            P3D_1011 = GetProject4D(P4D_1011, PrjCenter4D, TrueLenDist4D);
            P3D_0111 = GetProject4D(P4D_0111, PrjCenter4D, TrueLenDist4D);
            P3D_1111 = GetProject4D(P4D_1111, PrjCenter4D, TrueLenDist4D);

            Com.PointD3D RotateCenter3D = new Com.PointD3D();

            switch (View)
            {
                case Views.XYZ_XY:
                case Views.XYZ_YZ:
                case Views.XYZ_ZX:
                    RotateCenter3D = TesseractCenter.XYZ;
                    break;

                case Views.YZU_XY:
                case Views.YZU_YZ:
                case Views.YZU_ZX:
                    RotateCenter3D = TesseractCenter.YZU;
                    break;

                case Views.ZUX_XY:
                case Views.ZUX_YZ:
                case Views.ZUX_ZX:
                    RotateCenter3D = TesseractCenter.ZUX;
                    break;

                case Views.UXY_XY:
                case Views.UXY_YZ:
                case Views.UXY_ZX:
                    RotateCenter3D = TesseractCenter.UXY;
                    break;
            }

            AffineTransform(ref P3D_0000, RotateCenter3D, AffineMatrix3D);
            AffineTransform(ref P3D_1000, RotateCenter3D, AffineMatrix3D);
            AffineTransform(ref P3D_0100, RotateCenter3D, AffineMatrix3D);
            AffineTransform(ref P3D_1100, RotateCenter3D, AffineMatrix3D);
            AffineTransform(ref P3D_0010, RotateCenter3D, AffineMatrix3D);
            AffineTransform(ref P3D_1010, RotateCenter3D, AffineMatrix3D);
            AffineTransform(ref P3D_0110, RotateCenter3D, AffineMatrix3D);
            AffineTransform(ref P3D_1110, RotateCenter3D, AffineMatrix3D);
            AffineTransform(ref P3D_0001, RotateCenter3D, AffineMatrix3D);
            AffineTransform(ref P3D_1001, RotateCenter3D, AffineMatrix3D);
            AffineTransform(ref P3D_0101, RotateCenter3D, AffineMatrix3D);
            AffineTransform(ref P3D_1101, RotateCenter3D, AffineMatrix3D);
            AffineTransform(ref P3D_0011, RotateCenter3D, AffineMatrix3D);
            AffineTransform(ref P3D_1011, RotateCenter3D, AffineMatrix3D);
            AffineTransform(ref P3D_0111, RotateCenter3D, AffineMatrix3D);
            AffineTransform(ref P3D_1111, RotateCenter3D, AffineMatrix3D);

            //

            double TrueLenDist3D = new Com.PointD(Screen.PrimaryScreen.Bounds.Size).VectorModule;

            Com.PointD3D PrjCenter3D = TesseractCenter.XYZ;

            switch (View)
            {
                case Views.XYZ_XY:
                case Views.YZU_XY:
                case Views.ZUX_XY:
                case Views.UXY_XY:
                    PrjCenter3D.Z -= (TrueLenDist3D + TesseractDiag / 2);
                    break;

                case Views.XYZ_YZ:
                case Views.YZU_YZ:
                case Views.ZUX_YZ:
                case Views.UXY_YZ:
                    PrjCenter3D.X -= (TrueLenDist3D + TesseractDiag / 2);
                    break;

                case Views.XYZ_ZX:
                case Views.YZU_ZX:
                case Views.ZUX_ZX:
                case Views.UXY_ZX:
                    PrjCenter3D.Y -= (TrueLenDist3D + TesseractDiag / 2);
                    break;
            }

            Com.PointD P2D_0000 = new Com.PointD();
            Com.PointD P2D_1000 = new Com.PointD();
            Com.PointD P2D_0100 = new Com.PointD();
            Com.PointD P2D_1100 = new Com.PointD();
            Com.PointD P2D_0010 = new Com.PointD();
            Com.PointD P2D_1010 = new Com.PointD();
            Com.PointD P2D_0110 = new Com.PointD();
            Com.PointD P2D_1110 = new Com.PointD();
            Com.PointD P2D_0001 = new Com.PointD();
            Com.PointD P2D_1001 = new Com.PointD();
            Com.PointD P2D_0101 = new Com.PointD();
            Com.PointD P2D_1101 = new Com.PointD();
            Com.PointD P2D_0011 = new Com.PointD();
            Com.PointD P2D_1011 = new Com.PointD();
            Com.PointD P2D_0111 = new Com.PointD();
            Com.PointD P2D_1111 = new Com.PointD();

            Func<Com.PointD3D, Com.PointD3D, double, Com.PointD> GetProject3D = (Pt, PrjCenter, TrueLenDist) =>
            {
                switch (View)
                {
                    case Views.XYZ_XY:
                    case Views.YZU_XY:
                    case Views.ZUX_XY:
                    case Views.UXY_XY:
                        return Pt.ProjectToXY(PrjCenter, TrueLenDist);

                    case Views.XYZ_YZ:
                    case Views.YZU_YZ:
                    case Views.ZUX_YZ:
                    case Views.UXY_YZ:
                        return Pt.ProjectToYZ(PrjCenter, TrueLenDist);

                    case Views.XYZ_ZX:
                    case Views.YZU_ZX:
                    case Views.ZUX_ZX:
                    case Views.UXY_ZX:
                        return Pt.ProjectToZX(PrjCenter, TrueLenDist);

                    default:
                        return Com.PointD.NaN;
                }
            };

            P2D_0000 = GetProject3D(P3D_0000, PrjCenter3D, TrueLenDist3D);
            P2D_1000 = GetProject3D(P3D_1000, PrjCenter3D, TrueLenDist3D);
            P2D_0100 = GetProject3D(P3D_0100, PrjCenter3D, TrueLenDist3D);
            P2D_1100 = GetProject3D(P3D_1100, PrjCenter3D, TrueLenDist3D);
            P2D_0010 = GetProject3D(P3D_0010, PrjCenter3D, TrueLenDist3D);
            P2D_1010 = GetProject3D(P3D_1010, PrjCenter3D, TrueLenDist3D);
            P2D_0110 = GetProject3D(P3D_0110, PrjCenter3D, TrueLenDist3D);
            P2D_1110 = GetProject3D(P3D_1110, PrjCenter3D, TrueLenDist3D);
            P2D_0001 = GetProject3D(P3D_0001, PrjCenter3D, TrueLenDist3D);
            P2D_1001 = GetProject3D(P3D_1001, PrjCenter3D, TrueLenDist3D);
            P2D_0101 = GetProject3D(P3D_0101, PrjCenter3D, TrueLenDist3D);
            P2D_1101 = GetProject3D(P3D_1101, PrjCenter3D, TrueLenDist3D);
            P2D_0011 = GetProject3D(P3D_0011, PrjCenter3D, TrueLenDist3D);
            P2D_1011 = GetProject3D(P3D_1011, PrjCenter3D, TrueLenDist3D);
            P2D_0111 = GetProject3D(P3D_0111, PrjCenter3D, TrueLenDist3D);
            P2D_1111 = GetProject3D(P3D_1111, PrjCenter3D, TrueLenDist3D);

            Com.PointD BitmapCenter = new Com.PointD(ProjBmp.Size) / 2;

            PointF P_0000 = (P2D_0000 + BitmapCenter).ToPointF();
            PointF P_1000 = (P2D_1000 + BitmapCenter).ToPointF();
            PointF P_0100 = (P2D_0100 + BitmapCenter).ToPointF();
            PointF P_1100 = (P2D_1100 + BitmapCenter).ToPointF();
            PointF P_0010 = (P2D_0010 + BitmapCenter).ToPointF();
            PointF P_1010 = (P2D_1010 + BitmapCenter).ToPointF();
            PointF P_0110 = (P2D_0110 + BitmapCenter).ToPointF();
            PointF P_1110 = (P2D_1110 + BitmapCenter).ToPointF();
            PointF P_0001 = (P2D_0001 + BitmapCenter).ToPointF();
            PointF P_1001 = (P2D_1001 + BitmapCenter).ToPointF();
            PointF P_0101 = (P2D_0101 + BitmapCenter).ToPointF();
            PointF P_1101 = (P2D_1101 + BitmapCenter).ToPointF();
            PointF P_0011 = (P2D_0011 + BitmapCenter).ToPointF();
            PointF P_1011 = (P2D_1011 + BitmapCenter).ToPointF();
            PointF P_0111 = (P2D_0111 + BitmapCenter).ToPointF();
            PointF P_1111 = (P2D_1111 + BitmapCenter).ToPointF();

            //

            List<Com.PointD3D[]> Side3D = new List<Com.PointD3D[]>
            {
                // XY
                new Com.PointD3D[] { P3D_0000, P3D_0100, P3D_1100, P3D_1000 },
                new Com.PointD3D[] { P3D_0010, P3D_0110, P3D_1110, P3D_1010 },
                new Com.PointD3D[] { P3D_0001, P3D_0101, P3D_1101, P3D_1001 },
                new Com.PointD3D[] { P3D_0011, P3D_0111, P3D_1111, P3D_1011 },

                // XZ
                new Com.PointD3D[] { P3D_0000, P3D_0010, P3D_1010, P3D_1000 },
                new Com.PointD3D[] { P3D_0100, P3D_0110, P3D_1110, P3D_1100 },
                new Com.PointD3D[] { P3D_0001, P3D_0011, P3D_1011, P3D_1001 },
                new Com.PointD3D[] { P3D_0101, P3D_0111, P3D_1111, P3D_1101 },

                // XU
                new Com.PointD3D[] { P3D_0000, P3D_0001, P3D_1001, P3D_1000 },
                new Com.PointD3D[] { P3D_0100, P3D_0101, P3D_1101, P3D_1100 },
                new Com.PointD3D[] { P3D_0010, P3D_0011, P3D_1011, P3D_1010 },
                new Com.PointD3D[] { P3D_0110, P3D_0111, P3D_1111, P3D_1110 },

                // YZ
                new Com.PointD3D[] { P3D_0000, P3D_0010, P3D_0110, P3D_0100 },
                new Com.PointD3D[] { P3D_1000, P3D_1010, P3D_1110, P3D_1100 },
                new Com.PointD3D[] { P3D_0001, P3D_0011, P3D_0111, P3D_0101 },
                new Com.PointD3D[] { P3D_1001, P3D_1011, P3D_1111, P3D_1101 },

                // YU
                new Com.PointD3D[] { P3D_0000, P3D_0001, P3D_0101, P3D_0100 },
                new Com.PointD3D[] { P3D_1000, P3D_1001, P3D_1101, P3D_1100 },
                new Com.PointD3D[] { P3D_0010, P3D_0011, P3D_0111, P3D_0110 },
                new Com.PointD3D[] { P3D_1010, P3D_1011, P3D_1111, P3D_1110 },

                // ZU
                new Com.PointD3D[] { P3D_0000, P3D_0001, P3D_0011, P3D_0010 },
                new Com.PointD3D[] { P3D_1000, P3D_1001, P3D_1011, P3D_1010 },
                new Com.PointD3D[] { P3D_0100, P3D_0101, P3D_0111, P3D_0110 },
                new Com.PointD3D[] { P3D_1100, P3D_1101, P3D_1111, P3D_1110 }
            };

            List<PointF[]> Side2D = new List<PointF[]>()
            {
                // XY
                new PointF[] { P_0000, P_0100, P_1100, P_1000 },
                new PointF[] { P_0010, P_0110, P_1110, P_1010 },
                new PointF[] { P_0001, P_0101, P_1101, P_1001 },
                new PointF[] { P_0011, P_0111, P_1111, P_1011 },

                // XZ
                new PointF[] { P_0000, P_0010, P_1010, P_1000 },
                new PointF[] { P_0100, P_0110, P_1110, P_1100 },
                new PointF[] { P_0001, P_0011, P_1011, P_1001 },
                new PointF[] { P_0101, P_0111, P_1111, P_1101 },

                // XU
                new PointF[] { P_0000, P_0001, P_1001, P_1000 },
                new PointF[] { P_0100, P_0101, P_1101, P_1100 },
                new PointF[] { P_0010, P_0011, P_1011, P_1010 },
                new PointF[] { P_0110, P_0111, P_1111, P_1110 },

                // YZ
                new PointF[] { P_0000, P_0010, P_0110, P_0100 },
                new PointF[] { P_1000, P_1010, P_1110, P_1100 },
                new PointF[] { P_0001, P_0011, P_0111, P_0101 },
                new PointF[] { P_1001, P_1011, P_1111, P_1101 },

                // YU
                new PointF[] { P_0000, P_0001, P_0101, P_0100 },
                new PointF[] { P_1000, P_1001, P_1101, P_1100 },
                new PointF[] { P_0010, P_0011, P_0111, P_0110 },
                new PointF[] { P_1010, P_1011, P_1111, P_1110 },

                // ZU
                new PointF[] { P_0000, P_0001, P_0011, P_0010 },
                new PointF[] { P_1000, P_1001, P_1011, P_1010 },
                new PointF[] { P_0100, P_0101, P_0111, P_0110 },
                new PointF[] { P_1100, P_1101, P_1111, P_1110 }
            };

            List<Color> SideColor = new List<Color>()
            {
                // XY
                Colors.Side,
                Colors.Side,
                Colors.Side,
                Colors.Side,

                // XZ
                Colors.Side,
                Colors.Side,
                Colors.Side,
                Colors.Side,

                // XU
                Colors.Side,
                Colors.Side,
                Colors.Side,
                Colors.Side,
                
                // YZ
                Colors.Side,
                Colors.Side,
                Colors.Side,
                Colors.Side,

                // YU
                Colors.Side,
                Colors.Side,
                Colors.Side,
                Colors.Side,

                // ZU
                Colors.Side,
                Colors.Side,
                Colors.Side,
                Colors.Side
            };

            List<Com.PointD3D[]> Line3D = new List<Com.PointD3D[]>()
            {
                // X
                new Com.PointD3D[] { P3D_0000, P3D_1000 },
                new Com.PointD3D[] { P3D_0100, P3D_1100 },
                new Com.PointD3D[] { P3D_0010, P3D_1010 },
                new Com.PointD3D[] { P3D_0001, P3D_1001 },
                new Com.PointD3D[] { P3D_0110, P3D_1110 },
                new Com.PointD3D[] { P3D_0101, P3D_1101 },
                new Com.PointD3D[] { P3D_0011, P3D_1011 },
                new Com.PointD3D[] { P3D_0111, P3D_1111 },

                // Y
                new Com.PointD3D[] { P3D_0000, P3D_0100 },
                new Com.PointD3D[] { P3D_1000, P3D_1100 },
                new Com.PointD3D[] { P3D_0010, P3D_0110 },
                new Com.PointD3D[] { P3D_0001, P3D_0101 },
                new Com.PointD3D[] { P3D_1010, P3D_1110 },
                new Com.PointD3D[] { P3D_1001, P3D_1101 },
                new Com.PointD3D[] { P3D_0011, P3D_0111 },
                new Com.PointD3D[] { P3D_1011, P3D_1111 },

                // Z
                new Com.PointD3D[] { P3D_0000, P3D_0010 },
                new Com.PointD3D[] { P3D_1000, P3D_1010 },
                new Com.PointD3D[] { P3D_0100, P3D_0110 },
                new Com.PointD3D[] { P3D_0001, P3D_0011 },
                new Com.PointD3D[] { P3D_1100, P3D_1110 },
                new Com.PointD3D[] { P3D_1001, P3D_1011 },
                new Com.PointD3D[] { P3D_0101, P3D_0111 },
                new Com.PointD3D[] { P3D_1101, P3D_1111 },

                // U
                new Com.PointD3D[] { P3D_0000, P3D_0001 },
                new Com.PointD3D[] { P3D_1000, P3D_1001 },
                new Com.PointD3D[] { P3D_0100, P3D_0101 },
                new Com.PointD3D[] { P3D_0010, P3D_0011 },
                new Com.PointD3D[] { P3D_1100, P3D_1101 },
                new Com.PointD3D[] { P3D_1010, P3D_1011 },
                new Com.PointD3D[] { P3D_0110, P3D_0111 },
                new Com.PointD3D[] { P3D_1110, P3D_1111 }
            };

            List<PointF[]> Line2D = new List<PointF[]>()
            {
                // X
                new PointF[] { P_0000, P_1000 },
                new PointF[] { P_0100, P_1100 },
                new PointF[] { P_0010, P_1010 },
                new PointF[] { P_0001, P_1001 },
                new PointF[] { P_0110, P_1110 },
                new PointF[] { P_0101, P_1101 },
                new PointF[] { P_0011, P_1011 },
                new PointF[] { P_0111, P_1111 },

                // Y
                new PointF[] { P_0000, P_0100 },
                new PointF[] { P_1000, P_1100 },
                new PointF[] { P_0010, P_0110 },
                new PointF[] { P_0001, P_0101 },
                new PointF[] { P_1010, P_1110 },
                new PointF[] { P_1001, P_1101 },
                new PointF[] { P_0011, P_0111 },
                new PointF[] { P_1011, P_1111 },

                // Z
                new PointF[] { P_0000, P_0010 },
                new PointF[] { P_1000, P_1010 },
                new PointF[] { P_0100, P_0110 },
                new PointF[] { P_0001, P_0011 },
                new PointF[] { P_1100, P_1110 },
                new PointF[] { P_1001, P_1011 },
                new PointF[] { P_0101, P_0111 },
                new PointF[] { P_1101, P_1111 },

                // U
                new PointF[] { P_0000, P_0001 },
                new PointF[] { P_1000, P_1001 },
                new PointF[] { P_0100, P_0101 },
                new PointF[] { P_0010, P_0011 },
                new PointF[] { P_1100, P_1101 },
                new PointF[] { P_1010, P_1011 },
                new PointF[] { P_0110, P_0111 },
                new PointF[] { P_1110, P_1111 },
            };

            List<Color> LineColor = new List<Color>()
            {
                // X
                Colors.X,
                Colors.Line,
                Colors.Line,
                Colors.Line,
                Colors.Line,
                Colors.Line,
                Colors.Line,
                Colors.Line,

                // Y
                Colors.Y,
                Colors.Line,
                Colors.Line,
                Colors.Line,
                Colors.Line,
                Colors.Line,
                Colors.Line,
                Colors.Line,

                // Z
                Colors.Z,
                Colors.Line,
                Colors.Line,
                Colors.Line,
                Colors.Line,
                Colors.Line,
                Colors.Line,
                Colors.Line,

                // U
                Colors.U,
                Colors.Line,
                Colors.Line,
                Colors.Line,
                Colors.Line,
                Colors.Line,
                Colors.Line,
                Colors.Line
            };

            Func<Com.PointD3D, Int32, Int32, Int32> GetAlphaOfPoint = (Pt, MinAlpha, MaxAlpha) =>
            {
                switch (View)
                {
                    case Views.XYZ_XY:
                    case Views.YZU_XY:
                    case Views.ZUX_XY:
                    case Views.UXY_XY:
                        return (Int32)Math.Max(0, Math.Min(((Pt.Z - TesseractCenter.Z) / TesseractDiag + 0.5) * (MinAlpha - MaxAlpha) + MaxAlpha, 255));

                    case Views.XYZ_YZ:
                    case Views.YZU_YZ:
                    case Views.ZUX_YZ:
                    case Views.UXY_YZ:
                        return (Int32)Math.Max(0, Math.Min(((Pt.X - TesseractCenter.X) / TesseractDiag + 0.5) * (MinAlpha - MaxAlpha) + MaxAlpha, 255));

                    case Views.XYZ_ZX:
                    case Views.YZU_ZX:
                    case Views.ZUX_ZX:
                    case Views.UXY_ZX:
                        return (Int32)Math.Max(0, Math.Min(((Pt.Y - TesseractCenter.Y) / TesseractDiag + 0.5) * (MinAlpha - MaxAlpha) + MaxAlpha, 255));

                    default:
                        return 0;
                }
            };

            Func<Int32, Brush> GetBrushOfSide = (Index) =>
            {
                const Int32 _MinAlpha = 16, _MaxAlpha = 48;

                Com.PointD3D Pt_Avg = new Com.PointD3D(0, 0, 0);

                foreach (var V in Side3D[Index])
                {
                    Pt_Avg += V;
                }

                Pt_Avg /= Side3D[Index].Length;

                return new SolidBrush(Color.FromArgb(GetAlphaOfPoint(Pt_Avg, _MinAlpha, _MaxAlpha), SideColor[Index]));
            };

            Func<Int32, Brush> GetBrushOfLine = (Index) =>
            {
                const Int32 _MinAlpha = 32, _MaxAlpha = 96;

                if (Com.PointD.DistanceBetween(new Com.PointD(Line2D[Index][0]), new Com.PointD(Line2D[Index][1])) > 1)
                {
                    Int32 Alpha0 = GetAlphaOfPoint(Line3D[Index][0], _MinAlpha, _MaxAlpha), Alpha1 = GetAlphaOfPoint(Line3D[Index][1], _MinAlpha, _MaxAlpha);

                    return new LinearGradientBrush(Line2D[Index][0], Line2D[Index][1], Color.FromArgb(Alpha0, LineColor[Index]), Color.FromArgb(Alpha1, LineColor[Index]));
                }
                else
                {
                    Int32 Alpha0 = GetAlphaOfPoint(Line3D[Index][0], _MinAlpha, _MaxAlpha);

                    return new SolidBrush(Color.FromArgb(Alpha0, LineColor[Index]));
                }
            };

            for (int i = 0; i < Side2D.Count; i++)
            {
                CreateProjBmp.FillPolygon(GetBrushOfSide(i), Side2D[i]);
            }

            for (int i = 0; i < Line2D.Count; i++)
            {
                CreateProjBmp.DrawLine(new Pen(GetBrushOfLine(i), 2F), Line2D[i][0], Line2D[i][1]);
            }

            CreateProjBmp.DrawString("X", new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134), new SolidBrush(Color.FromArgb(GetAlphaOfPoint(P3D_1000, 64, 192), Colors.X)), P_1000);
            CreateProjBmp.DrawString("Y", new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134), new SolidBrush(Color.FromArgb(GetAlphaOfPoint(P3D_0100, 64, 192), Colors.Y)), P_0100);
            CreateProjBmp.DrawString("Z", new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134), new SolidBrush(Color.FromArgb(GetAlphaOfPoint(P3D_0010, 64, 192), Colors.Z)), P_0010);
            CreateProjBmp.DrawString("U", new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134), new SolidBrush(Color.FromArgb(GetAlphaOfPoint(P3D_0001, 64, 192), Colors.U)), P_0001);

            //

            string ViewName = string.Empty;

            switch (View)
            {
                case Views.XYZ_XY: ViewName = "XYZ-XY 视图 (主视图)"; break;
                case Views.XYZ_YZ: ViewName = "XYZ-YZ 视图"; break;
                case Views.XYZ_ZX: ViewName = "XYZ-ZX 视图"; break;

                case Views.YZU_XY: ViewName = "YZU-XY 视图"; break;
                case Views.YZU_YZ: ViewName = "YZU-YZ 视图"; break;
                case Views.YZU_ZX: ViewName = "YZU-ZX 视图"; break;

                case Views.ZUX_XY: ViewName = "ZUX-XY 视图"; break;
                case Views.ZUX_YZ: ViewName = "ZUX-YZ 视图"; break;
                case Views.ZUX_ZX: ViewName = "ZUX-ZX 视图"; break;

                case Views.UXY_XY: ViewName = "UXY-XY 视图"; break;
                case Views.UXY_YZ: ViewName = "UXY-YZ 视图"; break;
                case Views.UXY_ZX: ViewName = "UXY-ZX 视图"; break;
            }

            CreateProjBmp.DrawString(ViewName, new Font("微软雅黑", 10F, FontStyle.Regular, GraphicsUnit.Point, 134), new SolidBrush(Colors.Text), new PointF(Math.Max(0, (ProjBmp.Width - ProjBmp.Height) / 2), Math.Max(0, (ProjBmp.Height - ProjBmp.Width) / 2)));

            //

            CreateProjBmp.DrawRectangle(new Pen(Color.FromArgb(64, Colors.Border), 1F), new Rectangle(new Point(0, 0), ProjBmp.Size));

            //

            CreateProjBmp.Dispose();

            return ProjBmp;
        }

        private Com.PointD4D TesseractSize = new Com.PointD4D(1, 1, 1, 1); // 超立方体各边长的比例。

        private double[,] AffineMatrix4D = new double[5, 5] // 4D 仿射矩阵。
        {
            { 1, 0, 0, 0, 0 },
            { 0, 1, 0, 0, 0 },
            { 0, 0, 1, 0, 0 },
            { 0, 0, 0, 1, 0 },
            { 0, 0, 0, 0, 1 }
        };

        private double[,] AffineMatrix3D = new double[4, 4] // 3D 仿射矩阵。
        {
            { 1, 0, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 }
        };

        private static class Colors // 颜色。
        {
            public static readonly Color Background = Color.Black;
            public static readonly Color Text = Color.White;
            public static readonly Color Border = Color.White;

            public static readonly Color X = Color.DeepPink;
            public static readonly Color Y = Color.Lime;
            public static readonly Color Z = Color.DeepSkyBlue;
            public static readonly Color U = Color.DarkOrange;

            public static readonly Color Side = Color.White;
            public static readonly Color Line = Color.White;
        }

        private Bitmap Bmp; // 位图。

        private void UpdateBmp()
        {
            //
            // 更新位图。
            //

            if (Bmp != null)
            {
                Bmp.Dispose();
            }

            Bmp = new Bitmap(Math.Max(1, Panel_Main.Width), Math.Max(1, Panel_Main.Height));

            using (Graphics CreateBmp = Graphics.FromImage(Bmp))
            {
                CreateBmp.Clear(Colors.Background);

                //

                const Int32 NumX = 4, NumY = 3;

                SizeF BlockSize = new SizeF((float)Panel_Main.Width / NumX, (float)Panel_Main.Height / NumY);

                Bitmap[,] BmpArray = new Bitmap[NumX, NumY]
                {
                {
                    GetProjectionOfTesseract(TesseractSize, AffineMatrix4D, AffineMatrix3D, Views.XYZ_XY, BlockSize),
                    GetProjectionOfTesseract(TesseractSize, AffineMatrix4D, AffineMatrix3D, Views.XYZ_YZ, BlockSize),
                    GetProjectionOfTesseract(TesseractSize, AffineMatrix4D, AffineMatrix3D, Views.XYZ_ZX, BlockSize)
                },
                {
                    GetProjectionOfTesseract(TesseractSize, AffineMatrix4D, AffineMatrix3D, Views.YZU_XY, BlockSize),
                    GetProjectionOfTesseract(TesseractSize, AffineMatrix4D, AffineMatrix3D, Views.YZU_YZ, BlockSize),
                    GetProjectionOfTesseract(TesseractSize, AffineMatrix4D, AffineMatrix3D, Views.YZU_ZX, BlockSize)
                },
                {
                    GetProjectionOfTesseract(TesseractSize, AffineMatrix4D, AffineMatrix3D, Views.ZUX_XY, BlockSize),
                    GetProjectionOfTesseract(TesseractSize, AffineMatrix4D, AffineMatrix3D, Views.ZUX_YZ, BlockSize),
                    GetProjectionOfTesseract(TesseractSize, AffineMatrix4D, AffineMatrix3D, Views.ZUX_ZX, BlockSize)
                },
                {
                    GetProjectionOfTesseract(TesseractSize, AffineMatrix4D, AffineMatrix3D, Views.UXY_XY, BlockSize),
                    GetProjectionOfTesseract(TesseractSize, AffineMatrix4D, AffineMatrix3D, Views.UXY_YZ, BlockSize),
                    GetProjectionOfTesseract(TesseractSize, AffineMatrix4D, AffineMatrix3D, Views.UXY_ZX, BlockSize)
                }
                };

                for (int x = 0; x < NumX; x++)
                {
                    for (int y = 0; y < NumY; y++)
                    {
                        CreateBmp.DrawImage(BmpArray[x, y], new Point((Int32)(BlockSize.Width * x), (Int32)(BlockSize.Height * y)));
                    }
                }

                //

                foreach (var V in BmpArray)
                {
                    if (V != null)
                    {
                        V.Dispose();
                    }
                }
            }
        }

        private void RepaintBmp()
        {
            //
            // 更新并重绘位图。
            //

            if (Panel_GraphArea.Visible && (Panel_GraphArea.Width > 0 && Panel_GraphArea.Height > 0))
            {
                UpdateBmp();

                if (Bmp != null)
                {
                    Panel_GraphArea.CreateGraphics().DrawImage(Bmp, new Point(0, 0));
                }
            }
        }

        private void Panel_GraphArea_Paint(object sender, PaintEventArgs e)
        {
            //
            // Panel_GraphArea 绘图。
            //

            if (Bmp == null)
            {
                UpdateBmp();
            }

            if (Bmp != null)
            {
                e.Graphics.DrawImage(Bmp, new Point(0, 0));
            }
        }

        #endregion

        #region "控制"子窗口

        private void Panel_Control_Paint(object sender, PaintEventArgs e)
        {
            //
            // Panel_Control 绘图。
            //

            Pen P = new Pen(Me.RecommendColors.Main.ToColor(), 1);
            Control Cntr = sender as Control;
            e.Graphics.DrawRectangle(P, new Rectangle(new Point(0, 0), new Size(Cntr.Width - 1, Cntr.Height - 1)));
            P.Dispose();
        }

        private void Panel_Control_SubFormClient_Paint(object sender, PaintEventArgs e)
        {
            //
            // Panel_Control_SubFormClient 绘图。
            //

            Pen P = new Pen(Me.RecommendColors.Border_DEC.ToColor(), 1);
            Control Ctrl1 = Label_Size, Ctrl2 = Label_Rotation, Cntr = sender as Control;
            e.Graphics.DrawLine(P, new Point(Ctrl1.Right, Ctrl1.Top + Ctrl1.Height / 2), new Point(Cntr.Width - Ctrl1.Left, Ctrl1.Top + Ctrl1.Height / 2));
            e.Graphics.DrawLine(P, new Point(Ctrl2.Right, Ctrl2.Top + Ctrl2.Height / 2), new Point(Cntr.Width - Ctrl2.Left, Ctrl2.Top + Ctrl2.Height / 2));
            P.Dispose();
        }

        private Point ControlPanelLoc = new Point(); // 子窗口位置。
        private Point CursorLoc = new Point(); // 鼠标指针位置。
        private bool ControlPanelIsMoving = false; // 是否正在移动子窗口。

        private void Label_Control_MouseDown(object sender, MouseEventArgs e)
        {
            //
            // 鼠标按下 Label_Control。
            //

            if (e.Button == MouseButtons.Left)
            {
                ControlPanelLoc = Panel_Control.Location;
                CursorLoc = e.Location;
                ControlPanelIsMoving = true;
            }
        }

        private void Label_Control_MouseUp(object sender, MouseEventArgs e)
        {
            //
            // 鼠标释放 Label_Control。
            //

            ControlPanelIsMoving = false;

            Panel_Control.Location = new Point(Math.Max(0, Math.Min(Panel_Control.Left, Panel_Client.Width - Label_Control_SubFormTitle.Right)), Math.Max(0, Math.Min(Panel_Control.Top, Panel_Client.Height - Label_Control_SubFormTitle.Bottom)));
        }

        private void Label_Control_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Control。
            //

            if (ControlPanelIsMoving)
            {
                Panel_Control.Location = new Point(Panel_Control.Left + (e.X - CursorLoc.X), Panel_Control.Top + (e.Y - CursorLoc.Y));
            }
        }

        #endregion

        #region 控制

        private Int32 CursorX = 0; // 鼠标指针 X 坐标。

        private const double RatioPerPixel = 0.01; // 每像素的缩放倍率。
        private Com.PointD4D TesseractSizeCopy = new Com.PointD4D(); // 超立方体各边长的比例。
        private bool ResizeNow = false; // 是否正在缩放立方体的某条边。

        private void Label_Sxyzu_MouseEnter(object sender, EventArgs e)
        {
            //
            // 鼠标进入 Label_Sxyzu。
            //

            ((Label)sender).BackColor = Me.RecommendColors.Button_DEC.ToColor();
        }

        private void Label_Sxyzu_MouseLeave(object sender, EventArgs e)
        {
            //
            // 鼠标离开 Label_Sxyzu。
            //

            ((Label)sender).BackColor = Me.RecommendColors.Button.ToColor();
        }

        private void Label_Sxyzu_MouseDown(object sender, MouseEventArgs e)
        {
            //
            // 鼠标按下 Label_Sxyzu。
            //

            if (e.Button == MouseButtons.Left)
            {
                ((Label)sender).BackColor = Me.RecommendColors.Button_INC.ToColor();
                ((Label)sender).Cursor = Cursors.SizeWE;

                CursorX = e.X;
                TesseractSizeCopy = TesseractSize;
                ResizeNow = true;
            }
        }

        private void Label_Sxyzu_MouseUp(object sender, MouseEventArgs e)
        {
            //
            // 鼠标释放 Label_Sxyzu。
            //

            ResizeNow = false;

            ((Label)sender).BackColor = Me.RecommendColors.Button_DEC.ToColor();
            ((Label)sender).Cursor = Cursors.Default;

            Label_Sx.Text = "X";
            Label_Sy.Text = "Y";
            Label_Sz.Text = "Z";
            Label_Su.Text = "U";
        }

        private void Label_Sx_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Sx。
            //

            if (ResizeNow)
            {
                double ratio = Math.Max(0.01, 1 + (e.X - CursorX) * RatioPerPixel);

                ((Label)sender).Text = "× " + ratio.ToString("F2");

                TesseractSize = Com.PointD4D.Max(new Com.PointD4D(0.001, 0.001, 0.001, 0.001), new Com.PointD4D(TesseractSizeCopy.X * ratio, TesseractSizeCopy.Y, TesseractSizeCopy.Z, TesseractSizeCopy.U).VectorNormalize);

                RepaintBmp();
            }
        }

        private void Label_Sy_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Sy。
            //

            if (ResizeNow)
            {
                double ratio = Math.Max(0.01, 1 + (e.X - CursorX) * RatioPerPixel);

                ((Label)sender).Text = "× " + ratio.ToString("F2");

                TesseractSize = Com.PointD4D.Max(new Com.PointD4D(0.001, 0.001, 0.001, 0.001), new Com.PointD4D(TesseractSizeCopy.X, TesseractSizeCopy.Y * ratio, TesseractSizeCopy.Z, TesseractSizeCopy.U).VectorNormalize);

                RepaintBmp();
            }
        }

        private void Label_Sz_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Sz。
            //

            if (ResizeNow)
            {
                double ratio = Math.Max(0.01, 1 + (e.X - CursorX) * RatioPerPixel);

                ((Label)sender).Text = "× " + ratio.ToString("F2");

                TesseractSize = Com.PointD4D.Max(new Com.PointD4D(0.001, 0.001, 0.001, 0.001), new Com.PointD4D(TesseractSizeCopy.X, TesseractSizeCopy.Y, TesseractSizeCopy.Z * ratio, TesseractSizeCopy.U).VectorNormalize);

                RepaintBmp();
            }
        }

        private void Label_Su_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Su。
            //

            if (ResizeNow)
            {
                double ratio = Math.Max(0.01, 1 + (e.X - CursorX) * RatioPerPixel);

                ((Label)sender).Text = "× " + ratio.ToString("F2");

                TesseractSize = Com.PointD4D.Max(new Com.PointD4D(0.001, 0.001, 0.001, 0.001), new Com.PointD4D(TesseractSizeCopy.X, TesseractSizeCopy.Y, TesseractSizeCopy.Z, TesseractSizeCopy.U * ratio).VectorNormalize);

                RepaintBmp();
            }
        }

        private const double RadPerPixel = Math.PI / 180; // 每像素的旋转弧度。
        private double[,] AffineMatrix4DCopy = null; // 4D 仿射矩阵。
        private double[,] AffineMatrix3DCopy = null; // 3D 仿射矩阵。
        private bool RotateNow = false; // 是否正在旋转立方体。

        private void Label_Rxyzu_MouseEnter(object sender, EventArgs e)
        {
            //
            // 鼠标进入 Label_Rxyzu。
            //

            ((Label)sender).BackColor = Me.RecommendColors.Button_DEC.ToColor();
        }

        private void Label_Rxyzu_MouseLeave(object sender, EventArgs e)
        {
            //
            // 鼠标进入 Label_Rxyzu。
            //

            ((Label)sender).BackColor = Me.RecommendColors.Button.ToColor();
        }

        private void Label_Rxyzu_MouseDown(object sender, MouseEventArgs e)
        {
            //
            // 鼠标按下 Label_Rxyzu。
            //

            if (e.Button == MouseButtons.Left)
            {
                ((Label)sender).BackColor = Me.RecommendColors.Button_INC.ToColor();
                ((Label)sender).Cursor = Cursors.SizeWE;

                CursorX = e.X;
                Com.Matrix2D.Copy(AffineMatrix4D, out AffineMatrix4DCopy);
                Com.Matrix2D.Copy(AffineMatrix3D, out AffineMatrix3DCopy);
                RotateNow = true;
            }
        }

        private void Label_Rxyzu_MouseUp(object sender, MouseEventArgs e)
        {
            //
            // 鼠标释放 Label_Rxyzu。
            //

            RotateNow = false;

            ((Label)sender).BackColor = Me.RecommendColors.Button_DEC.ToColor();
            ((Label)sender).Cursor = Cursors.Default;

            Label_Rxy.Text = "XY";
            Label_Ryz.Text = "YZ";
            Label_Rzu.Text = "ZU";
            Label_Rux.Text = "UX";

            Label_Rx.Text = "X";
            Label_Ry.Text = "Y";
            Label_Rz.Text = "Z";
        }

        private void Label_Rxy_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Rxy。
            //

            if (RotateNow)
            {
                double angle = (e.X - CursorX) * RadPerPixel;

                ((Label)sender).Text = (angle >= 0 ? "+ " : "- ") + (Math.Abs(angle) / Math.PI * 180).ToString("F0") + "°";

                double[,] matrixLeft = Com.PointD4D.RotateXYMatrix(angle);

                if (Com.Matrix2D.Multiply(matrixLeft, AffineMatrix4DCopy, out AffineMatrix4D))
                {
                    RepaintBmp();
                }
            }
        }

        private void Label_Ryz_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Ryz。
            //

            if (RotateNow)
            {
                double angle = (e.X - CursorX) * RadPerPixel;

                ((Label)sender).Text = (angle >= 0 ? "+ " : "- ") + (Math.Abs(angle) / Math.PI * 180).ToString("F0") + "°";

                double[,] matrixLeft = Com.PointD4D.RotateYZMatrix(angle);

                if (Com.Matrix2D.Multiply(matrixLeft, AffineMatrix4DCopy, out AffineMatrix4D))
                {
                    RepaintBmp();
                }
            }
        }

        private void Label_Rzu_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Rzu。
            //

            if (RotateNow)
            {
                double angle = (e.X - CursorX) * RadPerPixel;

                ((Label)sender).Text = (angle >= 0 ? "+ " : "- ") + (Math.Abs(angle) / Math.PI * 180).ToString("F0") + "°";

                double[,] matrixLeft = Com.PointD4D.RotateZUMatrix(angle);

                if (Com.Matrix2D.Multiply(matrixLeft, AffineMatrix4DCopy, out AffineMatrix4D))
                {
                    RepaintBmp();
                }
            }
        }

        private void Label_Rux_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Rux。
            //

            if (RotateNow)
            {
                double angle = (e.X - CursorX) * RadPerPixel;

                ((Label)sender).Text = (angle >= 0 ? "+ " : "- ") + (Math.Abs(angle) / Math.PI * 180).ToString("F0") + "°";

                double[,] matrixLeft = Com.PointD4D.RotateUXMatrix(angle);

                if (Com.Matrix2D.Multiply(matrixLeft, AffineMatrix4DCopy, out AffineMatrix4D))
                {
                    RepaintBmp();
                }
            }
        }

        private void Label_Rx_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Rx。
            //

            if (RotateNow)
            {
                double angle = (e.X - CursorX) * RadPerPixel;

                ((Label)sender).Text = (angle >= 0 ? "+ " : "- ") + (Math.Abs(angle) / Math.PI * 180).ToString("F0") + "°";

                double[,] matrixLeft = Com.PointD3D.RotateXMatrix(angle);

                if (Com.Matrix2D.Multiply(matrixLeft, AffineMatrix3DCopy, out AffineMatrix3D))
                {
                    RepaintBmp();
                }
            }
        }

        private void Label_Ry_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Ry。
            //

            if (RotateNow)
            {
                double angle = (e.X - CursorX) * RadPerPixel;

                ((Label)sender).Text = (angle >= 0 ? "+ " : "- ") + (Math.Abs(angle) / Math.PI * 180).ToString("F0") + "°";

                double[,] matrixLeft = Com.PointD3D.RotateYMatrix(angle);

                if (Com.Matrix2D.Multiply(matrixLeft, AffineMatrix3DCopy, out AffineMatrix3D))
                {
                    RepaintBmp();
                }
            }
        }

        private void Label_Rz_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Rz。
            //

            if (RotateNow)
            {
                double angle = (e.X - CursorX) * RadPerPixel;

                ((Label)sender).Text = (angle >= 0 ? "+ " : "- ") + (Math.Abs(angle) / Math.PI * 180).ToString("F0") + "°";

                double[,] matrixLeft = Com.PointD3D.RotateZMatrix(angle);

                if (Com.Matrix2D.Multiply(matrixLeft, AffineMatrix3DCopy, out AffineMatrix3D))
                {
                    RepaintBmp();
                }
            }
        }

        #endregion

    }
}