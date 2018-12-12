namespace WinFormApp
{
    partial class Form_Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.Panel_Main = new System.Windows.Forms.Panel();
            this.Panel_Client = new System.Windows.Forms.Panel();
            this.Panel_Control = new System.Windows.Forms.Panel();
            this.Label_Control_SubFormTitle = new System.Windows.Forms.Label();
            this.Panel_Control_SubFormClient = new System.Windows.Forms.Panel();
            this.Label_Size = new System.Windows.Forms.Label();
            this.Label_Sx = new System.Windows.Forms.Label();
            this.Label_Sy = new System.Windows.Forms.Label();
            this.Label_Sz = new System.Windows.Forms.Label();
            this.Label_Su = new System.Windows.Forms.Label();
            this.Label_Rotation = new System.Windows.Forms.Label();
            this.Label_Rxy = new System.Windows.Forms.Label();
            this.Label_Rxz = new System.Windows.Forms.Label();
            this.Label_Rxu = new System.Windows.Forms.Label();
            this.Label_Ryz = new System.Windows.Forms.Label();
            this.Label_Ryu = new System.Windows.Forms.Label();
            this.Label_Rzu = new System.Windows.Forms.Label();
            this.Label_Rx = new System.Windows.Forms.Label();
            this.Label_Ry = new System.Windows.Forms.Label();
            this.Label_Rz = new System.Windows.Forms.Label();
            this.Panel_GraphArea = new System.Windows.Forms.Panel();
            this.BackgroundWorker_RepaintBmpDelay = new System.ComponentModel.BackgroundWorker();
            this.BackgroundWorker_MoveSubFormDelay = new System.ComponentModel.BackgroundWorker();
            this.Panel_Main.SuspendLayout();
            this.Panel_Client.SuspendLayout();
            this.Panel_Control.SuspendLayout();
            this.Panel_Control_SubFormClient.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel_Main
            // 
            this.Panel_Main.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Main.Controls.Add(this.Panel_Client);
            this.Panel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_Main.Location = new System.Drawing.Point(0, 0);
            this.Panel_Main.Name = "Panel_Main";
            this.Panel_Main.Size = new System.Drawing.Size(800, 450);
            this.Panel_Main.TabIndex = 0;
            // 
            // Panel_Client
            // 
            this.Panel_Client.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Client.Controls.Add(this.Panel_Control);
            this.Panel_Client.Controls.Add(this.Panel_GraphArea);
            this.Panel_Client.Location = new System.Drawing.Point(0, 0);
            this.Panel_Client.Name = "Panel_Client";
            this.Panel_Client.Size = new System.Drawing.Size(800, 450);
            this.Panel_Client.TabIndex = 0;
            // 
            // Panel_Control
            // 
            this.Panel_Control.Controls.Add(this.Label_Control_SubFormTitle);
            this.Panel_Control.Controls.Add(this.Panel_Control_SubFormClient);
            this.Panel_Control.Location = new System.Drawing.Point(0, 0);
            this.Panel_Control.Name = "Panel_Control";
            this.Panel_Control.Size = new System.Drawing.Size(265, 165);
            this.Panel_Control.TabIndex = 0;
            // 
            // Label_Control_SubFormTitle
            // 
            this.Label_Control_SubFormTitle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Control_SubFormTitle.ForeColor = System.Drawing.Color.White;
            this.Label_Control_SubFormTitle.Location = new System.Drawing.Point(0, 0);
            this.Label_Control_SubFormTitle.Name = "Label_Control_SubFormTitle";
            this.Label_Control_SubFormTitle.Size = new System.Drawing.Size(265, 20);
            this.Label_Control_SubFormTitle.TabIndex = 0;
            this.Label_Control_SubFormTitle.Text = "控制";
            this.Label_Control_SubFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_Control_SubFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Control_SubFormTitle_MouseDown);
            this.Label_Control_SubFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Control_SubFormTitle_MouseMove);
            this.Label_Control_SubFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Control_SubFormTitle_MouseUp);
            // 
            // Panel_Control_SubFormClient
            // 
            this.Panel_Control_SubFormClient.Controls.Add(this.Label_Size);
            this.Panel_Control_SubFormClient.Controls.Add(this.Label_Sx);
            this.Panel_Control_SubFormClient.Controls.Add(this.Label_Sy);
            this.Panel_Control_SubFormClient.Controls.Add(this.Label_Sz);
            this.Panel_Control_SubFormClient.Controls.Add(this.Label_Su);
            this.Panel_Control_SubFormClient.Controls.Add(this.Label_Rotation);
            this.Panel_Control_SubFormClient.Controls.Add(this.Label_Rxy);
            this.Panel_Control_SubFormClient.Controls.Add(this.Label_Rxz);
            this.Panel_Control_SubFormClient.Controls.Add(this.Label_Rxu);
            this.Panel_Control_SubFormClient.Controls.Add(this.Label_Ryz);
            this.Panel_Control_SubFormClient.Controls.Add(this.Label_Ryu);
            this.Panel_Control_SubFormClient.Controls.Add(this.Label_Rzu);
            this.Panel_Control_SubFormClient.Controls.Add(this.Label_Rx);
            this.Panel_Control_SubFormClient.Controls.Add(this.Label_Ry);
            this.Panel_Control_SubFormClient.Controls.Add(this.Label_Rz);
            this.Panel_Control_SubFormClient.Location = new System.Drawing.Point(0, 20);
            this.Panel_Control_SubFormClient.Name = "Panel_Control_SubFormClient";
            this.Panel_Control_SubFormClient.Size = new System.Drawing.Size(265, 145);
            this.Panel_Control_SubFormClient.TabIndex = 0;
            this.Panel_Control_SubFormClient.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_Control_SubFormClient_Paint);
            // 
            // Label_Size
            // 
            this.Label_Size.AutoSize = true;
            this.Label_Size.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Size.ForeColor = System.Drawing.Color.White;
            this.Label_Size.Location = new System.Drawing.Point(5, 5);
            this.Label_Size.Name = "Label_Size";
            this.Label_Size.Size = new System.Drawing.Size(32, 17);
            this.Label_Size.TabIndex = 0;
            this.Label_Size.Text = "大小";
            // 
            // Label_Sx
            // 
            this.Label_Sx.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Sx.ForeColor = System.Drawing.Color.White;
            this.Label_Sx.Location = new System.Drawing.Point(5, 25);
            this.Label_Sx.Name = "Label_Sx";
            this.Label_Sx.Size = new System.Drawing.Size(60, 20);
            this.Label_Sx.TabIndex = 0;
            this.Label_Sx.Text = "X";
            this.Label_Sx.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_Sx.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseDown);
            this.Label_Sx.MouseEnter += new System.EventHandler(this.Label_Control_MouseEnter);
            this.Label_Sx.MouseLeave += new System.EventHandler(this.Label_Control_MouseLeave);
            this.Label_Sx.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Sx_MouseMove);
            this.Label_Sx.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseUp);
            // 
            // Label_Sy
            // 
            this.Label_Sy.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Sy.ForeColor = System.Drawing.Color.White;
            this.Label_Sy.Location = new System.Drawing.Point(70, 25);
            this.Label_Sy.Name = "Label_Sy";
            this.Label_Sy.Size = new System.Drawing.Size(60, 20);
            this.Label_Sy.TabIndex = 0;
            this.Label_Sy.Text = "Y";
            this.Label_Sy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_Sy.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseDown);
            this.Label_Sy.MouseEnter += new System.EventHandler(this.Label_Control_MouseEnter);
            this.Label_Sy.MouseLeave += new System.EventHandler(this.Label_Control_MouseLeave);
            this.Label_Sy.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Sy_MouseMove);
            this.Label_Sy.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseUp);
            // 
            // Label_Sz
            // 
            this.Label_Sz.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Sz.ForeColor = System.Drawing.Color.White;
            this.Label_Sz.Location = new System.Drawing.Point(135, 25);
            this.Label_Sz.Name = "Label_Sz";
            this.Label_Sz.Size = new System.Drawing.Size(60, 20);
            this.Label_Sz.TabIndex = 0;
            this.Label_Sz.Text = "Z";
            this.Label_Sz.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_Sz.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseDown);
            this.Label_Sz.MouseEnter += new System.EventHandler(this.Label_Control_MouseEnter);
            this.Label_Sz.MouseLeave += new System.EventHandler(this.Label_Control_MouseLeave);
            this.Label_Sz.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Sz_MouseMove);
            this.Label_Sz.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseUp);
            // 
            // Label_Su
            // 
            this.Label_Su.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Su.ForeColor = System.Drawing.Color.White;
            this.Label_Su.Location = new System.Drawing.Point(200, 25);
            this.Label_Su.Name = "Label_Su";
            this.Label_Su.Size = new System.Drawing.Size(60, 20);
            this.Label_Su.TabIndex = 0;
            this.Label_Su.Text = "U";
            this.Label_Su.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_Su.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseDown);
            this.Label_Su.MouseEnter += new System.EventHandler(this.Label_Control_MouseEnter);
            this.Label_Su.MouseLeave += new System.EventHandler(this.Label_Control_MouseLeave);
            this.Label_Su.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Su_MouseMove);
            this.Label_Su.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseUp);
            // 
            // Label_Rotation
            // 
            this.Label_Rotation.AutoSize = true;
            this.Label_Rotation.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Rotation.ForeColor = System.Drawing.Color.White;
            this.Label_Rotation.Location = new System.Drawing.Point(5, 50);
            this.Label_Rotation.Name = "Label_Rotation";
            this.Label_Rotation.Size = new System.Drawing.Size(32, 17);
            this.Label_Rotation.TabIndex = 0;
            this.Label_Rotation.Text = "旋转";
            // 
            // Label_Rxy
            // 
            this.Label_Rxy.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Rxy.ForeColor = System.Drawing.Color.White;
            this.Label_Rxy.Location = new System.Drawing.Point(5, 70);
            this.Label_Rxy.Name = "Label_Rxy";
            this.Label_Rxy.Size = new System.Drawing.Size(60, 20);
            this.Label_Rxy.TabIndex = 0;
            this.Label_Rxy.Text = "ZU (X-Y)";
            this.Label_Rxy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_Rxy.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseDown);
            this.Label_Rxy.MouseEnter += new System.EventHandler(this.Label_Control_MouseEnter);
            this.Label_Rxy.MouseLeave += new System.EventHandler(this.Label_Control_MouseLeave);
            this.Label_Rxy.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Rxy_MouseMove);
            this.Label_Rxy.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseUp);
            // 
            // Label_Rxz
            // 
            this.Label_Rxz.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Rxz.ForeColor = System.Drawing.Color.White;
            this.Label_Rxz.Location = new System.Drawing.Point(70, 70);
            this.Label_Rxz.Name = "Label_Rxz";
            this.Label_Rxz.Size = new System.Drawing.Size(60, 20);
            this.Label_Rxz.TabIndex = 0;
            this.Label_Rxz.Text = "YU (X-Z)";
            this.Label_Rxz.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_Rxz.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseDown);
            this.Label_Rxz.MouseEnter += new System.EventHandler(this.Label_Control_MouseEnter);
            this.Label_Rxz.MouseLeave += new System.EventHandler(this.Label_Control_MouseLeave);
            this.Label_Rxz.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Rxz_MouseMove);
            this.Label_Rxz.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseUp);
            // 
            // Label_Rxu
            // 
            this.Label_Rxu.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Rxu.ForeColor = System.Drawing.Color.White;
            this.Label_Rxu.Location = new System.Drawing.Point(135, 70);
            this.Label_Rxu.Name = "Label_Rxu";
            this.Label_Rxu.Size = new System.Drawing.Size(60, 20);
            this.Label_Rxu.TabIndex = 0;
            this.Label_Rxu.Text = "YZ (X-U)";
            this.Label_Rxu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_Rxu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseDown);
            this.Label_Rxu.MouseEnter += new System.EventHandler(this.Label_Control_MouseEnter);
            this.Label_Rxu.MouseLeave += new System.EventHandler(this.Label_Control_MouseLeave);
            this.Label_Rxu.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Rxu_MouseMove);
            this.Label_Rxu.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseUp);
            // 
            // Label_Ryz
            // 
            this.Label_Ryz.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Ryz.ForeColor = System.Drawing.Color.White;
            this.Label_Ryz.Location = new System.Drawing.Point(5, 95);
            this.Label_Ryz.Name = "Label_Ryz";
            this.Label_Ryz.Size = new System.Drawing.Size(60, 20);
            this.Label_Ryz.TabIndex = 0;
            this.Label_Ryz.Text = "XU (Y-Z)";
            this.Label_Ryz.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_Ryz.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseDown);
            this.Label_Ryz.MouseEnter += new System.EventHandler(this.Label_Control_MouseEnter);
            this.Label_Ryz.MouseLeave += new System.EventHandler(this.Label_Control_MouseLeave);
            this.Label_Ryz.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Ryz_MouseMove);
            this.Label_Ryz.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseUp);
            // 
            // Label_Ryu
            // 
            this.Label_Ryu.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Ryu.ForeColor = System.Drawing.Color.White;
            this.Label_Ryu.Location = new System.Drawing.Point(70, 95);
            this.Label_Ryu.Name = "Label_Ryu";
            this.Label_Ryu.Size = new System.Drawing.Size(60, 20);
            this.Label_Ryu.TabIndex = 0;
            this.Label_Ryu.Text = "XZ (Y-U)";
            this.Label_Ryu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_Ryu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseDown);
            this.Label_Ryu.MouseEnter += new System.EventHandler(this.Label_Control_MouseEnter);
            this.Label_Ryu.MouseLeave += new System.EventHandler(this.Label_Control_MouseLeave);
            this.Label_Ryu.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Ryu_MouseMove);
            this.Label_Ryu.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseUp);
            // 
            // Label_Rzu
            // 
            this.Label_Rzu.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Rzu.ForeColor = System.Drawing.Color.White;
            this.Label_Rzu.Location = new System.Drawing.Point(135, 95);
            this.Label_Rzu.Name = "Label_Rzu";
            this.Label_Rzu.Size = new System.Drawing.Size(60, 20);
            this.Label_Rzu.TabIndex = 0;
            this.Label_Rzu.Text = "XY (Z-U)";
            this.Label_Rzu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_Rzu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseDown);
            this.Label_Rzu.MouseEnter += new System.EventHandler(this.Label_Control_MouseEnter);
            this.Label_Rzu.MouseLeave += new System.EventHandler(this.Label_Control_MouseLeave);
            this.Label_Rzu.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Rzu_MouseMove);
            this.Label_Rzu.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseUp);
            // 
            // Label_Rx
            // 
            this.Label_Rx.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Rx.ForeColor = System.Drawing.Color.White;
            this.Label_Rx.Location = new System.Drawing.Point(5, 120);
            this.Label_Rx.Name = "Label_Rx";
            this.Label_Rx.Size = new System.Drawing.Size(60, 20);
            this.Label_Rx.TabIndex = 0;
            this.Label_Rx.Text = "X (Y-Z)";
            this.Label_Rx.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_Rx.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseDown);
            this.Label_Rx.MouseEnter += new System.EventHandler(this.Label_Control_MouseEnter);
            this.Label_Rx.MouseLeave += new System.EventHandler(this.Label_Control_MouseLeave);
            this.Label_Rx.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Rx_MouseMove);
            this.Label_Rx.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseUp);
            // 
            // Label_Ry
            // 
            this.Label_Ry.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Ry.ForeColor = System.Drawing.Color.White;
            this.Label_Ry.Location = new System.Drawing.Point(70, 120);
            this.Label_Ry.Name = "Label_Ry";
            this.Label_Ry.Size = new System.Drawing.Size(60, 20);
            this.Label_Ry.TabIndex = 0;
            this.Label_Ry.Text = "Y (Z-X)";
            this.Label_Ry.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_Ry.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseDown);
            this.Label_Ry.MouseEnter += new System.EventHandler(this.Label_Control_MouseEnter);
            this.Label_Ry.MouseLeave += new System.EventHandler(this.Label_Control_MouseLeave);
            this.Label_Ry.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Ry_MouseMove);
            this.Label_Ry.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseUp);
            // 
            // Label_Rz
            // 
            this.Label_Rz.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Rz.ForeColor = System.Drawing.Color.White;
            this.Label_Rz.Location = new System.Drawing.Point(135, 120);
            this.Label_Rz.Name = "Label_Rz";
            this.Label_Rz.Size = new System.Drawing.Size(60, 20);
            this.Label_Rz.TabIndex = 0;
            this.Label_Rz.Text = "Z (X-Y)";
            this.Label_Rz.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_Rz.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseDown);
            this.Label_Rz.MouseEnter += new System.EventHandler(this.Label_Control_MouseEnter);
            this.Label_Rz.MouseLeave += new System.EventHandler(this.Label_Control_MouseLeave);
            this.Label_Rz.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label_Rz_MouseMove);
            this.Label_Rz.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_Control_MouseUp);
            // 
            // Panel_GraphArea
            // 
            this.Panel_GraphArea.BackColor = System.Drawing.Color.Transparent;
            this.Panel_GraphArea.Location = new System.Drawing.Point(0, 0);
            this.Panel_GraphArea.Name = "Panel_GraphArea";
            this.Panel_GraphArea.Size = new System.Drawing.Size(800, 450);
            this.Panel_GraphArea.TabIndex = 0;
            this.Panel_GraphArea.Visible = false;
            this.Panel_GraphArea.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_GraphArea_Paint);
            // 
            // BackgroundWorker_RepaintBmpDelay
            // 
            this.BackgroundWorker_RepaintBmpDelay.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_RepaintBmpDelay_DoWork);
            this.BackgroundWorker_RepaintBmpDelay.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RepaintBmpDelay_RunWorkerCompleted);
            // 
            // BackgroundWorker_MoveSubFormDelay
            // 
            this.BackgroundWorker_MoveSubFormDelay.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_MoveSubFormDelay_DoWork);
            this.BackgroundWorker_MoveSubFormDelay.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_MoveSubFormDelay_RunWorkerCompleted);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Panel_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Panel_Main.ResumeLayout(false);
            this.Panel_Client.ResumeLayout(false);
            this.Panel_Control.ResumeLayout(false);
            this.Panel_Control_SubFormClient.ResumeLayout(false);
            this.Panel_Control_SubFormClient.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel_Main;
        private System.Windows.Forms.Panel Panel_Client;
        private System.Windows.Forms.Panel Panel_GraphArea;
        private System.Windows.Forms.Panel Panel_Control;
        private System.Windows.Forms.Label Label_Rxu;
        private System.Windows.Forms.Label Label_Rxy;
        private System.Windows.Forms.Label Label_Ryz;
        private System.Windows.Forms.Label Label_Rzu;
        private System.Windows.Forms.Label Label_Rx;
        private System.Windows.Forms.Label Label_Ry;
        private System.Windows.Forms.Label Label_Rz;
        private System.Windows.Forms.Label Label_Su;
        private System.Windows.Forms.Label Label_Sx;
        private System.Windows.Forms.Label Label_Sy;
        private System.Windows.Forms.Label Label_Sz;
        private System.Windows.Forms.Label Label_Control_SubFormTitle;
        private System.Windows.Forms.Label Label_Size;
        private System.Windows.Forms.Label Label_Rotation;
        private System.Windows.Forms.Panel Panel_Control_SubFormClient;
        private System.ComponentModel.BackgroundWorker BackgroundWorker_RepaintBmpDelay;
        private System.Windows.Forms.Label Label_Ryu;
        private System.Windows.Forms.Label Label_Rxz;
        private System.ComponentModel.BackgroundWorker BackgroundWorker_MoveSubFormDelay;
    }
}