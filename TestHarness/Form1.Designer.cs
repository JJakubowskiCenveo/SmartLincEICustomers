namespace TestHarness
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.txtOrder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtXML = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lstInterface = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButtonDev = new System.Windows.Forms.RadioButton();
            this.radioButtonQA = new System.Windows.Forms.RadioButton();
            this.radioButtonPD = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(133, 112);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Sim Pull";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtOrder
            // 
            this.txtOrder.Location = new System.Drawing.Point(367, 66);
            this.txtOrder.Name = "txtOrder";
            this.txtOrder.Size = new System.Drawing.Size(100, 20);
            this.txtOrder.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(482, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "DeliveryDoc for Pull";
            // 
            // txtXML
            // 
            this.txtXML.Location = new System.Drawing.Point(24, 184);
            this.txtXML.Multiline = true;
            this.txtXML.Name = "txtXML";
            this.txtXML.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtXML.Size = new System.Drawing.Size(796, 393);
            this.txtXML.TabIndex = 6;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(331, 112);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Sim Putback";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(507, 112);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Sim Void";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 168);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(758, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Copy XML from Before EI putback, After re-objectification in Log here for Putback" +
    " and Void testing.  Start with: <EntityShipment and end with </EntityShipment>";
            // 
            // lstInterface
            // 
            this.lstInterface.CheckOnClick = true;
            this.lstInterface.FormattingEnabled = true;
            this.lstInterface.Items.AddRange(new object[] {
            "CMS",
            "Lexis",
            "LexisGlobal",
            "SAP"});
            this.lstInterface.Location = new System.Drawing.Point(38, 22);
            this.lstInterface.Name = "lstInterface";
            this.lstInterface.Size = new System.Drawing.Size(120, 64);
            this.lstInterface.Sorted = true;
            this.lstInterface.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Interface to test";
            // 
            // radioButtonDev
            // 
            this.radioButtonDev.AutoSize = true;
            this.radioButtonDev.Checked = true;
            this.radioButtonDev.Location = new System.Drawing.Point(6, 19);
            this.radioButtonDev.Name = "radioButtonDev";
            this.radioButtonDev.Size = new System.Drawing.Size(88, 17);
            this.radioButtonDev.TabIndex = 11;
            this.radioButtonDev.TabStop = true;
            this.radioButtonDev.Text = "Development";
            this.radioButtonDev.UseVisualStyleBackColor = true;
            // 
            // radioButtonQA
            // 
            this.radioButtonQA.AutoSize = true;
            this.radioButtonQA.Location = new System.Drawing.Point(6, 42);
            this.radioButtonQA.Name = "radioButtonQA";
            this.radioButtonQA.Size = new System.Drawing.Size(40, 17);
            this.radioButtonQA.TabIndex = 12;
            this.radioButtonQA.TabStop = true;
            this.radioButtonQA.Text = "QA";
            this.radioButtonQA.UseVisualStyleBackColor = true;
            // 
            // radioButtonPD
            // 
            this.radioButtonPD.AutoSize = true;
            this.radioButtonPD.Location = new System.Drawing.Point(6, 65);
            this.radioButtonPD.Name = "radioButtonPD";
            this.radioButtonPD.Size = new System.Drawing.Size(76, 17);
            this.radioButtonPD.TabIndex = 13;
            this.radioButtonPD.TabStop = true;
            this.radioButtonPD.Text = "Production";
            this.radioButtonPD.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonDev);
            this.groupBox1.Controls.Add(this.radioButtonPD);
            this.groupBox1.Controls.Add(this.radioButtonQA);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(204, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(137, 86);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Environment for Pull";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 589);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstInterface);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtXML);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtOrder);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtOrder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtXML;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox lstInterface;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButtonDev;
        private System.Windows.Forms.RadioButton radioButtonQA;
        private System.Windows.Forms.RadioButton radioButtonPD;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

