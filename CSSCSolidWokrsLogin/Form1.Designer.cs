using SolidWorks.Interop.sldworks;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CSSCSolidWokrsLogin
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 27);
            this.button1.TabIndex = 0;
            this.button1.Text = "1.连接SolidWorks";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Btn_Connect_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 45);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(142, 27);
            this.button2.TabIndex = 1;
            this.button2.Text = "2.打开和创建";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Btn_OpenAndCreate_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 79);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(142, 27);
            this.button3.TabIndex = 2;
            this.button3.Text = "3.读取零件属性";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Btn_GetPartAttribute_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 113);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(142, 27);
            this.button4.TabIndex = 3;
            this.button4.Text = "4.修改零件";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Btn_ChangeDim_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 147);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(142, 27);
            this.button5.TabIndex = 4;
            this.button5.Text = "5.遍历零件特征";
            this.button5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Btn_Traverse_Feature_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(12, 180);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(142, 27);
            this.button6.TabIndex = 5;
            this.button6.Text = "6.遍历装配体";
            this.button6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Btn_Traverse_Assembly_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(12, 214);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(142, 27);
            this.button7.TabIndex = 6;
            this.button7.Text = "7.遍历视图与球标";
            this.button7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.Btn_Traverse_Drawing_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(12, 248);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(142, 27);
            this.button8.TabIndex = 7;
            this.button8.Text = "8.从装配体制作工程图";
            this.button8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.Btn_MakeDrawingFromAssembly_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(12, 281);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(142, 27);
            this.button9.TabIndex = 8;
            this.button9.Text = "9.新建工程图";
            this.button9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.Btn_NewDrawing_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(12, 315);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(142, 27);
            this.button10.TabIndex = 9;
            this.button10.Text = "10.装配零件";
            this.button10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.Btn_InsertPart_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(12, 349);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(142, 27);
            this.button11.TabIndex = 10;
            this.button11.Text = "11.转换成指定文档";
            this.button11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.Btn_TransToDoc_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 623);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Solidworks二次开发 API";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Btn_Filter_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button button10;
        private Button button11;
    }
}

