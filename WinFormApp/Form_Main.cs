/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

4D绘图测试 (GraphicsText4D)
Version 19.5.25.1400

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
using System.Threading;

namespace WinFormApp
{
    public partial class Form_Main : Form
    {
        #region 窗体构造

        private Com.WinForm.FormManager Me;

        public Com.WinForm.FormManager FormManager
        {
            get
            {
                return Me;
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

            Panel_GraphArea.BackColor = Colors.Background;

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

            Label_Rxy.ForeColor = Label_Rxz.ForeColor = Label_Rxu.ForeColor = Label_Ryz.ForeColor = Label_Ryu.ForeColor = Label_Rzu.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_Rxy.BackColor = Label_Rxz.BackColor = Label_Rxu.BackColor = Label_Ryz.BackColor = Label_Ryu.BackColor = Label_Rzu.BackColor = Me.RecommendColors.Button.ToColor();

            Label_Rx.ForeColor = Label_Ry.ForeColor = Label_Rz.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_Rx.BackColor = Label_Ry.BackColor = Label_Rz.BackColor = Me.RecommendColors.Button.ToColor();
        }

        #endregion

        #region 4D绘图

        private void AffineTransform(ref Com.PointD3D Pt, Com.PointD3D Origin, Com.Matrix AffineMatrix)
        {
            //
            // 将一个 3D 坐标以指定点为新的原点进行仿射变换。
            //

            Pt -= Origin;
            Pt.AffineTransform(AffineMatrix);
            Pt += Origin;
        }

        private void AffineTransform(ref Com.PointD4D Pt, Com.PointD4D Origin, Com.Matrix AffineMatrix)
        {
            //
            // 将一个 4D 坐标以指定点为新的原点进行仿射变换。
            //

            Pt -= Origin;
            Pt.AffineTransform(AffineMatrix);
            Pt += Origin;
        }

        private enum Views // 视图枚举。
        {
            NULL = -1,

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
            UXY_ZX,

            COUNT
        }

        private Bitmap GetProjectionOfTesseract(Com.PointD4D TesseractSize, Color TesseractColor, Com.Matrix AffineMatrix4D, Com.Matrix AffineMatrix3D, Views View, SizeF ImageSize)
        {
            //
            // 获取超立方体的投影。
            //

            double TesseractDiag = Math.Min(ImageSize.Width, ImageSize.Height);

            TesseractSize = TesseractSize.Normalize * TesseractDiag;

            Bitmap PrjBmp = new Bitmap(Math.Max(1, (int)ImageSize.Width), Math.Max(1, (int)ImageSize.Height));

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

            double TrueLenDist4D = new Com.PointD(Screen.PrimaryScreen.Bounds.Size).Module;

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

            Com.PointD3D P3D_0000 = GetProject4D(P4D_0000, PrjCenter4D, TrueLenDist4D);
            Com.PointD3D P3D_1000 = GetProject4D(P4D_1000, PrjCenter4D, TrueLenDist4D);
            Com.PointD3D P3D_0100 = GetProject4D(P4D_0100, PrjCenter4D, TrueLenDist4D);
            Com.PointD3D P3D_1100 = GetProject4D(P4D_1100, PrjCenter4D, TrueLenDist4D);
            Com.PointD3D P3D_0010 = GetProject4D(P4D_0010, PrjCenter4D, TrueLenDist4D);
            Com.PointD3D P3D_1010 = GetProject4D(P4D_1010, PrjCenter4D, TrueLenDist4D);
            Com.PointD3D P3D_0110 = GetProject4D(P4D_0110, PrjCenter4D, TrueLenDist4D);
            Com.PointD3D P3D_1110 = GetProject4D(P4D_1110, PrjCenter4D, TrueLenDist4D);
            Com.PointD3D P3D_0001 = GetProject4D(P4D_0001, PrjCenter4D, TrueLenDist4D);
            Com.PointD3D P3D_1001 = GetProject4D(P4D_1001, PrjCenter4D, TrueLenDist4D);
            Com.PointD3D P3D_0101 = GetProject4D(P4D_0101, PrjCenter4D, TrueLenDist4D);
            Com.PointD3D P3D_1101 = GetProject4D(P4D_1101, PrjCenter4D, TrueLenDist4D);
            Com.PointD3D P3D_0011 = GetProject4D(P4D_0011, PrjCenter4D, TrueLenDist4D);
            Com.PointD3D P3D_1011 = GetProject4D(P4D_1011, PrjCenter4D, TrueLenDist4D);
            Com.PointD3D P3D_0111 = GetProject4D(P4D_0111, PrjCenter4D, TrueLenDist4D);
            Com.PointD3D P3D_1111 = GetProject4D(P4D_1111, PrjCenter4D, TrueLenDist4D);

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

            double TrueLenDist3D = new Com.PointD(Screen.PrimaryScreen.Bounds.Size).Module;

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

            Com.PointD P2D_0000 = GetProject3D(P3D_0000, PrjCenter3D, TrueLenDist3D);
            Com.PointD P2D_1000 = GetProject3D(P3D_1000, PrjCenter3D, TrueLenDist3D);
            Com.PointD P2D_0100 = GetProject3D(P3D_0100, PrjCenter3D, TrueLenDist3D);
            Com.PointD P2D_1100 = GetProject3D(P3D_1100, PrjCenter3D, TrueLenDist3D);
            Com.PointD P2D_0010 = GetProject3D(P3D_0010, PrjCenter3D, TrueLenDist3D);
            Com.PointD P2D_1010 = GetProject3D(P3D_1010, PrjCenter3D, TrueLenDist3D);
            Com.PointD P2D_0110 = GetProject3D(P3D_0110, PrjCenter3D, TrueLenDist3D);
            Com.PointD P2D_1110 = GetProject3D(P3D_1110, PrjCenter3D, TrueLenDist3D);
            Com.PointD P2D_0001 = GetProject3D(P3D_0001, PrjCenter3D, TrueLenDist3D);
            Com.PointD P2D_1001 = GetProject3D(P3D_1001, PrjCenter3D, TrueLenDist3D);
            Com.PointD P2D_0101 = GetProject3D(P3D_0101, PrjCenter3D, TrueLenDist3D);
            Com.PointD P2D_1101 = GetProject3D(P3D_1101, PrjCenter3D, TrueLenDist3D);
            Com.PointD P2D_0011 = GetProject3D(P3D_0011, PrjCenter3D, TrueLenDist3D);
            Com.PointD P2D_1011 = GetProject3D(P3D_1011, PrjCenter3D, TrueLenDist3D);
            Com.PointD P2D_0111 = GetProject3D(P3D_0111, PrjCenter3D, TrueLenDist3D);
            Com.PointD P2D_1111 = GetProject3D(P3D_1111, PrjCenter3D, TrueLenDist3D);

            Com.PointD BitmapCenter = new Com.PointD(PrjBmp.Size) / 2;

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

            List<Com.PointD3D[]> Element3D = new List<Com.PointD3D[]>(56)
            {
                // XY 面
                new Com.PointD3D[] { P3D_0000, P3D_0100, P3D_1100, P3D_1000 },
                new Com.PointD3D[] { P3D_0010, P3D_0110, P3D_1110, P3D_1010 },
                new Com.PointD3D[] { P3D_0001, P3D_0101, P3D_1101, P3D_1001 },
                new Com.PointD3D[] { P3D_0011, P3D_0111, P3D_1111, P3D_1011 },

                // XZ 面
                new Com.PointD3D[] { P3D_0000, P3D_0010, P3D_1010, P3D_1000 },
                new Com.PointD3D[] { P3D_0100, P3D_0110, P3D_1110, P3D_1100 },
                new Com.PointD3D[] { P3D_0001, P3D_0011, P3D_1011, P3D_1001 },
                new Com.PointD3D[] { P3D_0101, P3D_0111, P3D_1111, P3D_1101 },

                // XU 面
                new Com.PointD3D[] { P3D_0000, P3D_0001, P3D_1001, P3D_1000 },
                new Com.PointD3D[] { P3D_0100, P3D_0101, P3D_1101, P3D_1100 },
                new Com.PointD3D[] { P3D_0010, P3D_0011, P3D_1011, P3D_1010 },
                new Com.PointD3D[] { P3D_0110, P3D_0111, P3D_1111, P3D_1110 },
                        
                // YZ 面
                new Com.PointD3D[] { P3D_0000, P3D_0010, P3D_0110, P3D_0100 },
                new Com.PointD3D[] { P3D_1000, P3D_1010, P3D_1110, P3D_1100 },
                new Com.PointD3D[] { P3D_0001, P3D_0011, P3D_0111, P3D_0101 },
                new Com.PointD3D[] { P3D_1001, P3D_1011, P3D_1111, P3D_1101 },

                // YU 面
                new Com.PointD3D[] { P3D_0000, P3D_0001, P3D_0101, P3D_0100 },
                new Com.PointD3D[] { P3D_1000, P3D_1001, P3D_1101, P3D_1100 },
                new Com.PointD3D[] { P3D_0010, P3D_0011, P3D_0111, P3D_0110 },
                new Com.PointD3D[] { P3D_1010, P3D_1011, P3D_1111, P3D_1110 },

                // ZU 面
                new Com.PointD3D[] { P3D_0000, P3D_0001, P3D_0011, P3D_0010 },
                new Com.PointD3D[] { P3D_1000, P3D_1001, P3D_1011, P3D_1010 },
                new Com.PointD3D[] { P3D_0100, P3D_0101, P3D_0111, P3D_0110 },
                new Com.PointD3D[] { P3D_1100, P3D_1101, P3D_1111, P3D_1110 },

                // X 棱
                new Com.PointD3D[] { P3D_0000, P3D_1000 },
                new Com.PointD3D[] { P3D_0100, P3D_1100 },
                new Com.PointD3D[] { P3D_0010, P3D_1010 },
                new Com.PointD3D[] { P3D_0001, P3D_1001 },
                new Com.PointD3D[] { P3D_0110, P3D_1110 },
                new Com.PointD3D[] { P3D_0101, P3D_1101 },
                new Com.PointD3D[] { P3D_0011, P3D_1011 },
                new Com.PointD3D[] { P3D_0111, P3D_1111 },

                // Y 棱
                new Com.PointD3D[] { P3D_0000, P3D_0100 },
                new Com.PointD3D[] { P3D_1000, P3D_1100 },
                new Com.PointD3D[] { P3D_0010, P3D_0110 },
                new Com.PointD3D[] { P3D_0001, P3D_0101 },
                new Com.PointD3D[] { P3D_1010, P3D_1110 },
                new Com.PointD3D[] { P3D_1001, P3D_1101 },
                new Com.PointD3D[] { P3D_0011, P3D_0111 },
                new Com.PointD3D[] { P3D_1011, P3D_1111 },

                // Z 棱
                new Com.PointD3D[] { P3D_0000, P3D_0010 },
                new Com.PointD3D[] { P3D_1000, P3D_1010 },
                new Com.PointD3D[] { P3D_0100, P3D_0110 },
                new Com.PointD3D[] { P3D_0001, P3D_0011 },
                new Com.PointD3D[] { P3D_1100, P3D_1110 },
                new Com.PointD3D[] { P3D_1001, P3D_1011 },
                new Com.PointD3D[] { P3D_0101, P3D_0111 },
                new Com.PointD3D[] { P3D_1101, P3D_1111 },

                // U 棱
                new Com.PointD3D[] { P3D_0000, P3D_0001 },
                new Com.PointD3D[] { P3D_1000, P3D_1001 },
                new Com.PointD3D[] { P3D_0100, P3D_0101 },
                new Com.PointD3D[] { P3D_0010, P3D_0011 },
                new Com.PointD3D[] { P3D_1100, P3D_1101 },
                new Com.PointD3D[] { P3D_1010, P3D_1011 },
                new Com.PointD3D[] { P3D_0110, P3D_0111 },
                new Com.PointD3D[] { P3D_1110, P3D_1111 }
            };

            List<PointF[]> Element2D = new List<PointF[]>(56)
            {
                // XY 面
                new PointF[] { P_0000, P_0100, P_1100, P_1000 },
                new PointF[] { P_0010, P_0110, P_1110, P_1010 },
                new PointF[] { P_0001, P_0101, P_1101, P_1001 },
                new PointF[] { P_0011, P_0111, P_1111, P_1011 },

                // XZ 面
                new PointF[] { P_0000, P_0010, P_1010, P_1000 },
                new PointF[] { P_0100, P_0110, P_1110, P_1100 },
                new PointF[] { P_0001, P_0011, P_1011, P_1001 },
                new PointF[] { P_0101, P_0111, P_1111, P_1101 },

                // XU 面
                new PointF[] { P_0000, P_0001, P_1001, P_1000 },
                new PointF[] { P_0100, P_0101, P_1101, P_1100 },
                new PointF[] { P_0010, P_0011, P_1011, P_1010 },
                new PointF[] { P_0110, P_0111, P_1111, P_1110 },

                // YZ 面
                new PointF[] { P_0000, P_0010, P_0110, P_0100 },
                new PointF[] { P_1000, P_1010, P_1110, P_1100 },
                new PointF[] { P_0001, P_0011, P_0111, P_0101 },
                new PointF[] { P_1001, P_1011, P_1111, P_1101 },

                // YU 面
                new PointF[] { P_0000, P_0001, P_0101, P_0100 },
                new PointF[] { P_1000, P_1001, P_1101, P_1100 },
                new PointF[] { P_0010, P_0011, P_0111, P_0110 },
                new PointF[] { P_1010, P_1011, P_1111, P_1110 },

                // ZU 面
                new PointF[] { P_0000, P_0001, P_0011, P_0010 },
                new PointF[] { P_1000, P_1001, P_1011, P_1010 },
                new PointF[] { P_0100, P_0101, P_0111, P_0110 },
                new PointF[] { P_1100, P_1101, P_1111, P_1110 },
                        
                // X 棱
                new PointF[] { P_0000, P_1000 },
                new PointF[] { P_0100, P_1100 },
                new PointF[] { P_0010, P_1010 },
                new PointF[] { P_0001, P_1001 },
                new PointF[] { P_0110, P_1110 },
                new PointF[] { P_0101, P_1101 },
                new PointF[] { P_0011, P_1011 },
                new PointF[] { P_0111, P_1111 },

                // Y 棱
                new PointF[] { P_0000, P_0100 },
                new PointF[] { P_1000, P_1100 },
                new PointF[] { P_0010, P_0110 },
                new PointF[] { P_0001, P_0101 },
                new PointF[] { P_1010, P_1110 },
                new PointF[] { P_1001, P_1101 },
                new PointF[] { P_0011, P_0111 },
                new PointF[] { P_1011, P_1111 },

                // Z 棱
                new PointF[] { P_0000, P_0010 },
                new PointF[] { P_1000, P_1010 },
                new PointF[] { P_0100, P_0110 },
                new PointF[] { P_0001, P_0011 },
                new PointF[] { P_1100, P_1110 },
                new PointF[] { P_1001, P_1011 },
                new PointF[] { P_0101, P_0111 },
                new PointF[] { P_1101, P_1111 },

                // U 棱
                new PointF[] { P_0000, P_0001 },
                new PointF[] { P_1000, P_1001 },
                new PointF[] { P_0100, P_0101 },
                new PointF[] { P_0010, P_0011 },
                new PointF[] { P_1100, P_1101 },
                new PointF[] { P_1010, P_1011 },
                new PointF[] { P_0110, P_0111 },
                new PointF[] { P_1110, P_1111 },
            };

            //

            List<Color> ElementColor = new List<Color>(Element3D.Count);

            for (int i = 0; i < Element3D.Count; i++)
            {
                switch (i)
                {
                    case 24:
                        ElementColor.Add(Colors.X);
                        break;

                    case 32:
                        ElementColor.Add(Colors.Y);
                        break;

                    case 40:
                        ElementColor.Add(Colors.Z);
                        break;

                    case 48:
                        ElementColor.Add(Colors.U);
                        break;

                    default:
                        {
                            if (i < 24)
                            {
                                ElementColor.Add(TesseractColor);
                            }
                            else
                            {
                                ElementColor.Add(Com.ColorManipulation.ShiftLightnessByHSL(TesseractColor, 0.5));
                            }
                        }
                        break;
                }
            }

            //

            List<double> ElementZAvg = new List<double>(Element3D.Count);

            for (int i = 0; i < Element3D.Count; i++)
            {
                Com.PointD3D[] Element = Element3D[i];

                double ZAvg = 0;

                foreach (Com.PointD3D P in Element)
                {
                    switch (View)
                    {
                        case Views.XYZ_XY:
                        case Views.YZU_XY:
                        case Views.ZUX_XY:
                        case Views.UXY_XY:
                            ZAvg += P.Z;
                            break;

                        case Views.XYZ_YZ:
                        case Views.YZU_YZ:
                        case Views.ZUX_YZ:
                        case Views.UXY_YZ:
                            ZAvg += P.X;
                            break;

                        case Views.XYZ_ZX:
                        case Views.YZU_ZX:
                        case Views.ZUX_ZX:
                        case Views.UXY_ZX:
                            ZAvg += P.Y;
                            break;
                    }
                }

                ZAvg /= Element.Length;

                ElementZAvg.Add(ZAvg);
            }

            List<int> ElementIndex = new List<int>(ElementZAvg.Count);

            for (int i = 0; i < ElementZAvg.Count; i++)
            {
                ElementIndex.Add(i);
            }

            for (int i = 0; i < ElementZAvg.Count; i++)
            {
                for (int j = i + 1; j < ElementZAvg.Count; j++)
                {
                    if (ElementZAvg[ElementIndex[i]] < ElementZAvg[ElementIndex[j]] || (ElementZAvg[ElementIndex[i]] <= ElementZAvg[ElementIndex[j]] + 2F && Element2D[ElementIndex[i]].Length < Element2D[ElementIndex[j]].Length))
                    {
                        int Temp = ElementIndex[i];
                        ElementIndex[i] = ElementIndex[j];
                        ElementIndex[j] = Temp;
                    }
                }
            }

            //

            using (Graphics Grph = Graphics.FromImage(PrjBmp))
            {
                Grph.SmoothingMode = SmoothingMode.AntiAlias;

                //

                for (int i = 0; i < ElementIndex.Count; i++)
                {
                    int EIndex = ElementIndex[i];

                    Color EColor = ElementColor[EIndex];

                    if (!EColor.IsEmpty && EColor.A > 0)
                    {
                        PointF[] Element = Element2D[EIndex];

                        if (Element.Length >= 3)
                        {
                            try
                            {
                                using (SolidBrush Br = new SolidBrush(EColor))
                                {
                                    Grph.FillPolygon(Br, Element);
                                }
                            }
                            catch { }
                        }
                        else if (Element.Length == 2)
                        {
                            double PrjZ = 0;

                            switch (View)
                            {
                                case Views.XYZ_XY:
                                case Views.YZU_XY:
                                case Views.ZUX_XY:
                                case Views.UXY_XY:
                                    PrjZ = PrjCenter3D.Z;
                                    break;

                                case Views.XYZ_YZ:
                                case Views.YZU_YZ:
                                case Views.ZUX_YZ:
                                case Views.UXY_YZ:
                                    PrjZ = PrjCenter3D.X;
                                    break;

                                case Views.XYZ_ZX:
                                case Views.YZU_ZX:
                                case Views.ZUX_ZX:
                                case Views.UXY_ZX:
                                    PrjZ = PrjCenter3D.Y;
                                    break;
                            }

                            float EdgeWidth = (TrueLenDist3D == 0 ? 2F : (float)(TrueLenDist3D / (ElementZAvg[EIndex] - PrjZ) * 2F));

                            try
                            {
                                Brush Br;

                                Func<Color, double, int> GetAlpha = (Cr, Z) =>
                                {
                                    int Alpha;

                                    if (TrueLenDist3D == 0)
                                    {
                                        Alpha = Cr.A;
                                    }
                                    else
                                    {
                                        if (Z - PrjZ <= TrueLenDist3D)
                                        {
                                            Alpha = Cr.A;
                                        }
                                        else
                                        {
                                            Alpha = (int)Math.Max(0, Math.Min(TrueLenDist3D / (Z - PrjZ) * Cr.A, 255));
                                        }
                                    }

                                    if (EdgeWidth < 1)
                                    {
                                        Alpha = (int)(Alpha * EdgeWidth);
                                    }

                                    return Alpha;
                                };

                                if (Com.PointD.DistanceBetween(new Com.PointD(Element[0]), new Com.PointD(Element[1])) > 1)
                                {
                                    int Alpha0 = 0, Alpha1 = 0;

                                    switch (View)
                                    {
                                        case Views.XYZ_XY:
                                        case Views.YZU_XY:
                                        case Views.ZUX_XY:
                                        case Views.UXY_XY:
                                            {
                                                Alpha0 = GetAlpha(EColor, Element3D[EIndex][0].Z);
                                                Alpha1 = GetAlpha(EColor, Element3D[EIndex][1].Z);
                                            }
                                            break;

                                        case Views.XYZ_YZ:
                                        case Views.YZU_YZ:
                                        case Views.ZUX_YZ:
                                        case Views.UXY_YZ:
                                            {
                                                Alpha0 = GetAlpha(EColor, Element3D[EIndex][0].X);
                                                Alpha1 = GetAlpha(EColor, Element3D[EIndex][1].X);
                                            }
                                            break;

                                        case Views.XYZ_ZX:
                                        case Views.YZU_ZX:
                                        case Views.ZUX_ZX:
                                        case Views.UXY_ZX:
                                            {
                                                Alpha0 = GetAlpha(EColor, Element3D[EIndex][0].Y);
                                                Alpha1 = GetAlpha(EColor, Element3D[EIndex][1].Y);
                                            }
                                            break;
                                    }

                                    Br = new LinearGradientBrush(Element[0], Element[1], Color.FromArgb(Alpha0, EColor), Color.FromArgb(Alpha1, EColor));
                                }
                                else
                                {
                                    int Alpha = GetAlpha(EColor, ElementZAvg[EIndex]);

                                    Br = new SolidBrush(Color.FromArgb(Alpha, EColor));
                                }

                                using (Pen Pn = new Pen(Br, EdgeWidth))
                                {
                                    Grph.DrawLines(Pn, Element);
                                }

                                if (Br != null)
                                {
                                    Br.Dispose();
                                }
                            }
                            catch { }
                        }
                    }
                }

                //

                Func<Com.PointD3D, int, int, int> GetAlphaOfPoint = (Pt, MinAlpha, MaxAlpha) =>
                {
                    switch (View)
                    {
                        case Views.XYZ_XY:
                        case Views.YZU_XY:
                        case Views.ZUX_XY:
                        case Views.UXY_XY:
                            return (int)Math.Max(0, Math.Min(((Pt.Z - TesseractCenter.Z) / TesseractDiag + 0.5) * (MinAlpha - MaxAlpha) + MaxAlpha, 255));

                        case Views.XYZ_YZ:
                        case Views.YZU_YZ:
                        case Views.ZUX_YZ:
                        case Views.UXY_YZ:
                            return (int)Math.Max(0, Math.Min(((Pt.X - TesseractCenter.X) / TesseractDiag + 0.5) * (MinAlpha - MaxAlpha) + MaxAlpha, 255));

                        case Views.XYZ_ZX:
                        case Views.YZU_ZX:
                        case Views.ZUX_ZX:
                        case Views.UXY_ZX:
                            return (int)Math.Max(0, Math.Min(((Pt.Y - TesseractCenter.Y) / TesseractDiag + 0.5) * (MinAlpha - MaxAlpha) + MaxAlpha, 255));

                        default:
                            return 0;
                    }
                };

                Grph.DrawString("X", new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134), new SolidBrush(Color.FromArgb(GetAlphaOfPoint(P3D_1000, 64, 192), Colors.X)), P_1000);
                Grph.DrawString("Y", new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134), new SolidBrush(Color.FromArgb(GetAlphaOfPoint(P3D_0100, 64, 192), Colors.Y)), P_0100);
                Grph.DrawString("Z", new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134), new SolidBrush(Color.FromArgb(GetAlphaOfPoint(P3D_0010, 64, 192), Colors.Z)), P_0010);
                Grph.DrawString("U", new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134), new SolidBrush(Color.FromArgb(GetAlphaOfPoint(P3D_0001, 64, 192), Colors.U)), P_0001);

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

                Grph.DrawString(ViewName, new Font("微软雅黑", 10F, FontStyle.Regular, GraphicsUnit.Point, 134), new SolidBrush(Colors.Text), new PointF(Math.Max(0, (PrjBmp.Width - PrjBmp.Height) / 4), Math.Max(0, (PrjBmp.Height - PrjBmp.Width) / 4)));

                //

                Grph.DrawRectangle(new Pen(Color.FromArgb(64, Colors.Border), 1F), new Rectangle(new Point(-1, -1), PrjBmp.Size));
            }

            return PrjBmp;
        }

        private Com.PointD4D TesseractSize = new Com.PointD4D(1, 1, 1, 1); // 超立方体各边长的比例。

        private Com.Matrix AffineMatrix4D = Com.Matrix.Identity(5); // 4D 仿射矩阵。
        private Com.Matrix AffineMatrix3D = Com.Matrix.Identity(4); // 3D 仿射矩阵。

        private static class Colors // 颜色。
        {
            public static readonly Color Background = Color.Black;
            public static readonly Color Text = Color.White;
            public static readonly Color Border = Color.White;

            public static readonly Color X = Color.DeepPink;
            public static readonly Color Y = Color.Lime;
            public static readonly Color Z = Color.DeepSkyBlue;
            public static readonly Color U = Color.DarkOrange;
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

            Bmp = new Bitmap(Math.Max(1, Panel_GraphArea.Width), Math.Max(1, Panel_GraphArea.Height));

            using (Graphics Grph = Graphics.FromImage(Bmp))
            {
                Grph.Clear(Colors.Background);

                //

                int N = (int)Views.COUNT;
                double R = Math.Sqrt(N);
                int W = Math.Max(1, (int)Math.Floor(R * Math.Sqrt((double)Panel_GraphArea.Width / Panel_GraphArea.Height)));
                int H = Math.Max(1, (int)Math.Floor(R * Math.Sqrt((double)Panel_GraphArea.Height / Panel_GraphArea.Width)));

                while (W * H < N || W * H >= N + Math.Min(W, H))
                {
                    if (W * H < N)
                    {
                        if ((W + 1) * H >= N && W * (H + 1) >= N)
                        {
                            if (Math.Abs((double)Panel_GraphArea.Width / (W + 1) - (double)Panel_GraphArea.Height / H) <= Math.Abs((double)Panel_GraphArea.Width / W - (double)Panel_GraphArea.Height / (H + 1)))
                            {
                                W++;
                            }
                            else
                            {
                                H++;
                            }
                        }
                        else if ((W + 1) * H >= N)
                        {
                            W++;
                        }
                        else if (W * (H + 1) >= N)
                        {
                            H++;
                        }
                        else
                        {
                            W++;
                            H++;
                        }
                    }
                    else
                    {
                        if ((W - 1) * H >= N && W * (H - 1) >= N)
                        {
                            if (Math.Abs((double)Panel_GraphArea.Width / (W - 1) - (double)Panel_GraphArea.Height / H) <= Math.Abs((double)Panel_GraphArea.Width / W - (double)Panel_GraphArea.Height / (H - 1)))
                            {
                                W--;
                            }
                            else
                            {
                                H--;
                            }
                        }
                        else if ((W - 1) * H >= N)
                        {
                            W--;
                        }
                        else if (W * (H - 1) >= N)
                        {
                            H--;
                        }
                        else if ((W - 1) * (H - 1) >= N)
                        {
                            W--;
                            H--;
                        }
                    }
                }

                SizeF BlockSize = new SizeF((float)Panel_GraphArea.Width / W, (float)Panel_GraphArea.Height / H);

                Bitmap[] PrjBmpArray = new Bitmap[(int)Views.COUNT]
                {
                    GetProjectionOfTesseract(TesseractSize, Me.RecommendColors.Main_DEC.AtAlpha(128).ToColor(), AffineMatrix4D, AffineMatrix3D, Views.XYZ_XY, BlockSize),
                    GetProjectionOfTesseract(TesseractSize, Me.RecommendColors.Main_DEC.AtAlpha(128).ToColor(), AffineMatrix4D, AffineMatrix3D, Views.XYZ_YZ, BlockSize),
                    GetProjectionOfTesseract(TesseractSize, Me.RecommendColors.Main_DEC.AtAlpha(128).ToColor(), AffineMatrix4D, AffineMatrix3D, Views.XYZ_ZX, BlockSize),

                    GetProjectionOfTesseract(TesseractSize, Me.RecommendColors.Main_DEC.AtAlpha(128).ToColor(), AffineMatrix4D, AffineMatrix3D, Views.YZU_XY, BlockSize),
                    GetProjectionOfTesseract(TesseractSize, Me.RecommendColors.Main_DEC.AtAlpha(128).ToColor(), AffineMatrix4D, AffineMatrix3D, Views.YZU_YZ, BlockSize),
                    GetProjectionOfTesseract(TesseractSize, Me.RecommendColors.Main_DEC.AtAlpha(128).ToColor(), AffineMatrix4D, AffineMatrix3D, Views.YZU_ZX, BlockSize),

                    GetProjectionOfTesseract(TesseractSize, Me.RecommendColors.Main_DEC.AtAlpha(128).ToColor(), AffineMatrix4D, AffineMatrix3D, Views.ZUX_XY, BlockSize),
                    GetProjectionOfTesseract(TesseractSize, Me.RecommendColors.Main_DEC.AtAlpha(128).ToColor(), AffineMatrix4D, AffineMatrix3D, Views.ZUX_YZ, BlockSize),
                    GetProjectionOfTesseract(TesseractSize, Me.RecommendColors.Main_DEC.AtAlpha(128).ToColor(), AffineMatrix4D, AffineMatrix3D, Views.ZUX_ZX, BlockSize),

                    GetProjectionOfTesseract(TesseractSize, Me.RecommendColors.Main_DEC.AtAlpha(128).ToColor(), AffineMatrix4D, AffineMatrix3D, Views.UXY_XY, BlockSize),
                    GetProjectionOfTesseract(TesseractSize, Me.RecommendColors.Main_DEC.AtAlpha(128).ToColor(), AffineMatrix4D, AffineMatrix3D, Views.UXY_YZ, BlockSize),
                    GetProjectionOfTesseract(TesseractSize, Me.RecommendColors.Main_DEC.AtAlpha(128).ToColor(), AffineMatrix4D, AffineMatrix3D, Views.UXY_ZX, BlockSize)
                };

                for (int i = 0; i < PrjBmpArray.Length; i++)
                {
                    Bitmap PrjBmp = PrjBmpArray[i];

                    if (PrjBmp != null)
                    {
                        Grph.DrawImage(PrjBmp, new Point((int)(BlockSize.Width * (i % W)), (int)(BlockSize.Height * (i / W))));

                        PrjBmp.Dispose();
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

        private void BackgroundWorker_RepaintBmpDelay_DoWork(object sender, DoWorkEventArgs e)
        {
            //
            // 后台更新位图。
            //

            UpdateBmp();
        }

        private void BackgroundWorker_RepaintBmpDelay_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //
            // 后台更新位图完成，重绘位图。
            //

            if (Bmp != null)
            {
                Panel_GraphArea.CreateGraphics().DrawImage(Bmp, new Point(0, 0));
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

        private Point CursorLoc = new Point(); // 鼠标指针位置。
        private bool SubFormIsMoving = false; // 是否正在移动子窗口。

        private void Label_Control_SubFormTitle_MouseDown(object sender, MouseEventArgs e)
        {
            //
            // 鼠标按下 Label_Control_SubFormTitle。
            //

            if (e.Button == MouseButtons.Left)
            {
                CursorLoc = e.Location;

                SubFormIsMoving = true;
            }
        }

        private void Label_Control_SubFormTitle_MouseUp(object sender, MouseEventArgs e)
        {
            //
            // 鼠标释放 Label_Control_SubFormTitle。
            //

            if (e.Button == MouseButtons.Left)
            {
                CancelMoveSubForm();

                Panel_Control.Location = new Point(Math.Max(0, Math.Min(Panel_Control.Left, Panel_Client.Width - Label_Control_SubFormTitle.Right)), Math.Max(0, Math.Min(Panel_Control.Top, Panel_Client.Height - Label_Control_SubFormTitle.Bottom)));
            }

            SubFormIsMoving = false;
        }

        private void Label_Control_SubFormTitle_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Control_SubFormTitle。
            //

            if (SubFormIsMoving)
            {
                NewLocation = new Point(Panel_Control.Left + (e.X - CursorLoc.X), Panel_Control.Top + (e.Y - CursorLoc.Y));

                TryToMoveSubForm(NewLocation);
            }
        }

        private DateTime LastMoveSubForm = new DateTime(); // 上次移动子窗口的日期时间。
        private Point NewLocation = new Point(); // 子窗口的新位置。
        private bool MoveSubFormCanceled = false; // 是否已取消移动子窗口。

        private void TryToMoveSubForm(Point newLocation)
        {
            //
            // 尝试移动子窗口。
            //

            if (!BackgroundWorker_MoveSubFormDelay.IsBusy)
            {
                MoveSubFormCanceled = false;

                BackgroundWorker_MoveSubFormDelay.RunWorkerAsync();
            }
        }

        private void CancelMoveSubForm()
        {
            //
            // 取消移动子窗口。
            //

            MoveSubFormCanceled = true;
        }

        private void BackgroundWorker_MoveSubFormDelay_DoWork(object sender, DoWorkEventArgs e)
        {
            //
            // 移动子窗口延迟。
            //

            while ((DateTime.Now - LastMoveSubForm).TotalMilliseconds < 16)
            {
                Thread.Sleep(4);
            }
        }

        private void BackgroundWorker_MoveSubFormDelay_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //
            // 移动子窗口延迟完成，更新子窗口位置。
            //

            if (!MoveSubFormCanceled)
            {
                Panel_Control.Location = NewLocation;

                LastMoveSubForm = DateTime.Now;
            }
        }

        #endregion

        #region 控制

        private const double RatioPerPixel = 0.01; // 每像素的缩放倍率。
        private const double RadPerPixel = Math.PI / 180; // 每像素的旋转弧度。

        private int CursorX = 0; // 鼠标指针 X 坐标。
        private bool AdjustNow = false; // 是否正在调整。

        private Com.PointD4D TesseractSizeCopy = new Com.PointD4D(); // 超立方体各边长的比例。
        private Com.Matrix AffineMatrix4DCopy = null; // 4D 仿射矩阵。
        private Com.Matrix AffineMatrix3DCopy = null; // 3D 仿射矩阵。

        private void Label_Control_MouseEnter(object sender, EventArgs e)
        {
            //
            // 鼠标进入 Label_Control。
            //

            ((Label)sender).BackColor = Me.RecommendColors.Button_DEC.ToColor();
        }

        private void Label_Control_MouseLeave(object sender, EventArgs e)
        {
            //
            // 鼠标离开 Label_Control。
            //

            ((Label)sender).BackColor = Me.RecommendColors.Button.ToColor();
        }

        private void Label_Control_MouseDown(object sender, MouseEventArgs e)
        {
            //
            // 鼠标按下 Label_Control。
            //

            if (e.Button == MouseButtons.Left)
            {
                ((Label)sender).BackColor = Me.RecommendColors.Button_INC.ToColor();
                ((Label)sender).Cursor = Cursors.SizeWE;

                TesseractSizeCopy = TesseractSize;
                AffineMatrix4DCopy = AffineMatrix4D.Copy();
                AffineMatrix3DCopy = AffineMatrix3D.Copy();

                CursorX = e.X;
                AdjustNow = true;
            }
        }

        private void Label_Control_MouseUp(object sender, MouseEventArgs e)
        {
            //
            // 鼠标释放 Label_Control。
            //

            if (e.Button == MouseButtons.Left)
            {
                AdjustNow = false;

                ((Label)sender).BackColor = (Com.Geometry.CursorIsInControl((Label)sender) ? Me.RecommendColors.Button_DEC.ToColor() : Me.RecommendColors.Button.ToColor());
                ((Label)sender).Cursor = Cursors.Default;

                Label_Sx.Text = "X";
                Label_Sy.Text = "Y";
                Label_Sz.Text = "Z";
                Label_Su.Text = "U";
                Label_Rxy.Text = "ZU (X-Y)";
                Label_Rxz.Text = "YU (X-Z)";
                Label_Rxu.Text = "YZ (X-U)";
                Label_Ryz.Text = "XU (Y-Z)";
                Label_Ryu.Text = "XZ (Y-U)";
                Label_Rzu.Text = "XY (Z-U)";
                Label_Rx.Text = "X (Y-Z)";
                Label_Ry.Text = "Y (Z-X)";
                Label_Rz.Text = "Z (X-Y)";
            }
        }

        private void Label_Sx_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Sx。
            //

            if (AdjustNow && !BackgroundWorker_RepaintBmpDelay.IsBusy)
            {
                double ratio = Math.Max(0.01, 1 + (e.X - CursorX) * RatioPerPixel);

                ((Label)sender).Text = "× " + ratio.ToString("F2");

                TesseractSize = Com.PointD4D.Max(new Com.PointD4D(0.001, 0.001, 0.001, 0.001), new Com.PointD4D(TesseractSizeCopy.X * ratio, TesseractSizeCopy.Y, TesseractSizeCopy.Z, TesseractSizeCopy.U).Normalize);

                BackgroundWorker_RepaintBmpDelay.RunWorkerAsync();
            }
        }

        private void Label_Sy_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Sy。
            //

            if (AdjustNow && !BackgroundWorker_RepaintBmpDelay.IsBusy)
            {
                double ratio = Math.Max(0.01, 1 + (e.X - CursorX) * RatioPerPixel);

                ((Label)sender).Text = "× " + ratio.ToString("F2");

                TesseractSize = Com.PointD4D.Max(new Com.PointD4D(0.001, 0.001, 0.001, 0.001), new Com.PointD4D(TesseractSizeCopy.X, TesseractSizeCopy.Y * ratio, TesseractSizeCopy.Z, TesseractSizeCopy.U).Normalize);

                BackgroundWorker_RepaintBmpDelay.RunWorkerAsync();
            }
        }

        private void Label_Sz_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Sz。
            //

            if (AdjustNow && !BackgroundWorker_RepaintBmpDelay.IsBusy)
            {
                double ratio = Math.Max(0.01, 1 + (e.X - CursorX) * RatioPerPixel);

                ((Label)sender).Text = "× " + ratio.ToString("F2");

                TesseractSize = Com.PointD4D.Max(new Com.PointD4D(0.001, 0.001, 0.001, 0.001), new Com.PointD4D(TesseractSizeCopy.X, TesseractSizeCopy.Y, TesseractSizeCopy.Z * ratio, TesseractSizeCopy.U).Normalize);

                BackgroundWorker_RepaintBmpDelay.RunWorkerAsync();
            }
        }

        private void Label_Su_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Su。
            //

            if (AdjustNow && !BackgroundWorker_RepaintBmpDelay.IsBusy)
            {
                double ratio = Math.Max(0.01, 1 + (e.X - CursorX) * RatioPerPixel);

                ((Label)sender).Text = "× " + ratio.ToString("F2");

                TesseractSize = Com.PointD4D.Max(new Com.PointD4D(0.001, 0.001, 0.001, 0.001), new Com.PointD4D(TesseractSizeCopy.X, TesseractSizeCopy.Y, TesseractSizeCopy.Z, TesseractSizeCopy.U * ratio).Normalize);

                BackgroundWorker_RepaintBmpDelay.RunWorkerAsync();
            }
        }

        private void Label_Rxy_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Rxy。
            //

            if (AdjustNow && !BackgroundWorker_RepaintBmpDelay.IsBusy)
            {
                double angle = (e.X - CursorX) * RadPerPixel;

                ((Label)sender).Text = (angle >= 0 ? "+ " : "- ") + (Math.Abs(angle) / Math.PI * 180).ToString("F0") + "°";

                Com.Matrix matrixLeft = Com.PointD4D.RotateMatrix(0, 1, angle);

                AffineMatrix4D = Com.Matrix.Multiply(matrixLeft, AffineMatrix4DCopy);

                if (!Com.Matrix.IsNullOrEmpty(AffineMatrix4D))
                {
                    BackgroundWorker_RepaintBmpDelay.RunWorkerAsync();
                }
            }
        }

        private void Label_Rxz_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Rxz。
            //

            if (AdjustNow && !BackgroundWorker_RepaintBmpDelay.IsBusy)
            {
                double angle = (e.X - CursorX) * RadPerPixel;

                ((Label)sender).Text = (angle >= 0 ? "+ " : "- ") + (Math.Abs(angle) / Math.PI * 180).ToString("F0") + "°";

                Com.Matrix matrixLeft = Com.PointD4D.RotateMatrix(0, 2, angle);

                AffineMatrix4D = Com.Matrix.Multiply(matrixLeft, AffineMatrix4DCopy);

                if (!Com.Matrix.IsNullOrEmpty(AffineMatrix4D))
                {
                    BackgroundWorker_RepaintBmpDelay.RunWorkerAsync();
                }
            }
        }

        private void Label_Rxu_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Rxu。
            //

            if (AdjustNow && !BackgroundWorker_RepaintBmpDelay.IsBusy)
            {
                double angle = (e.X - CursorX) * RadPerPixel;

                ((Label)sender).Text = (angle >= 0 ? "+ " : "- ") + (Math.Abs(angle) / Math.PI * 180).ToString("F0") + "°";

                Com.Matrix matrixLeft = Com.PointD4D.RotateMatrix(0, 3, angle);

                AffineMatrix4D = Com.Matrix.Multiply(matrixLeft, AffineMatrix4DCopy);

                if (!Com.Matrix.IsNullOrEmpty(AffineMatrix4D))
                {
                    BackgroundWorker_RepaintBmpDelay.RunWorkerAsync();
                }
            }
        }

        private void Label_Ryz_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Ryz。
            //

            if (AdjustNow && !BackgroundWorker_RepaintBmpDelay.IsBusy)
            {
                double angle = (e.X - CursorX) * RadPerPixel;

                ((Label)sender).Text = (angle >= 0 ? "+ " : "- ") + (Math.Abs(angle) / Math.PI * 180).ToString("F0") + "°";

                Com.Matrix matrixLeft = Com.PointD4D.RotateMatrix(1, 2, angle);

                AffineMatrix4D = Com.Matrix.Multiply(matrixLeft, AffineMatrix4DCopy);

                if (!Com.Matrix.IsNullOrEmpty(AffineMatrix4D))
                {
                    BackgroundWorker_RepaintBmpDelay.RunWorkerAsync();
                }
            }
        }

        private void Label_Ryu_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Ryu。
            //

            if (AdjustNow && !BackgroundWorker_RepaintBmpDelay.IsBusy)
            {
                double angle = (e.X - CursorX) * RadPerPixel;

                ((Label)sender).Text = (angle >= 0 ? "+ " : "- ") + (Math.Abs(angle) / Math.PI * 180).ToString("F0") + "°";

                Com.Matrix matrixLeft = Com.PointD4D.RotateMatrix(1, 3, angle);

                AffineMatrix4D = Com.Matrix.Multiply(matrixLeft, AffineMatrix4DCopy);

                if (!Com.Matrix.IsNullOrEmpty(AffineMatrix4D))
                {
                    BackgroundWorker_RepaintBmpDelay.RunWorkerAsync();
                }
            }
        }

        private void Label_Rzu_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Rzu。
            //

            if (AdjustNow && !BackgroundWorker_RepaintBmpDelay.IsBusy)
            {
                double angle = (e.X - CursorX) * RadPerPixel;

                ((Label)sender).Text = (angle >= 0 ? "+ " : "- ") + (Math.Abs(angle) / Math.PI * 180).ToString("F0") + "°";

                Com.Matrix matrixLeft = Com.PointD4D.RotateMatrix(2, 3, angle);

                AffineMatrix4D = Com.Matrix.Multiply(matrixLeft, AffineMatrix4DCopy);

                if (!Com.Matrix.IsNullOrEmpty(AffineMatrix4D))
                {
                    BackgroundWorker_RepaintBmpDelay.RunWorkerAsync();
                }
            }
        }

        private void Label_Rx_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Rx。
            //

            if (AdjustNow && !BackgroundWorker_RepaintBmpDelay.IsBusy)
            {
                double angle = (e.X - CursorX) * RadPerPixel;

                ((Label)sender).Text = (angle >= 0 ? "+ " : "- ") + (Math.Abs(angle) / Math.PI * 180).ToString("F0") + "°";

                Com.Matrix matrixLeft = Com.PointD3D.RotateXMatrix(angle);

                AffineMatrix3D = Com.Matrix.Multiply(matrixLeft, AffineMatrix3DCopy);

                if (!Com.Matrix.IsNullOrEmpty(AffineMatrix3D))
                {
                    BackgroundWorker_RepaintBmpDelay.RunWorkerAsync();
                }
            }
        }

        private void Label_Ry_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Ry。
            //

            if (AdjustNow && !BackgroundWorker_RepaintBmpDelay.IsBusy)
            {
                double angle = (e.X - CursorX) * RadPerPixel;

                ((Label)sender).Text = (angle >= 0 ? "+ " : "- ") + (Math.Abs(angle) / Math.PI * 180).ToString("F0") + "°";

                Com.Matrix matrixLeft = Com.PointD3D.RotateYMatrix(angle);

                AffineMatrix3D = Com.Matrix.Multiply(matrixLeft, AffineMatrix3DCopy);

                if (!Com.Matrix.IsNullOrEmpty(AffineMatrix3D))
                {
                    BackgroundWorker_RepaintBmpDelay.RunWorkerAsync();
                }
            }
        }

        private void Label_Rz_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // 鼠标经过 Label_Rz。
            //

            if (AdjustNow && !BackgroundWorker_RepaintBmpDelay.IsBusy)
            {
                double angle = (e.X - CursorX) * RadPerPixel;

                ((Label)sender).Text = (angle >= 0 ? "+ " : "- ") + (Math.Abs(angle) / Math.PI * 180).ToString("F0") + "°";

                Com.Matrix matrixLeft = Com.PointD3D.RotateZMatrix(angle);

                AffineMatrix3D = Com.Matrix.Multiply(matrixLeft, AffineMatrix3DCopy);

                if (!Com.Matrix.IsNullOrEmpty(AffineMatrix3D))
                {
                    BackgroundWorker_RepaintBmpDelay.RunWorkerAsync();
                }
            }
        }

        #endregion

    }
}