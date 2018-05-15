namespace plugin_multisign
{
    partial class MSign
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
            this.comboBoxName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.wifsList = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.wifText = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comAsset = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.amountText = new System.Windows.Forms.TextBox();
            this.balanceText = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.targetAddr = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // comboBoxName
            // 
            this.comboBoxName.FormattingEnabled = true;
            this.comboBoxName.Location = new System.Drawing.Point(146, 27);
            this.comboBoxName.Name = "comboBoxName";
            this.comboBoxName.Size = new System.Drawing.Size(530, 26);
            this.comboBoxName.TabIndex = 0;
            this.comboBoxName.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "合约地址";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 18);
            this.label2.TabIndex = 2;
            // 
            // wifsList
            // 
            this.wifsList.FormattingEnabled = true;
            this.wifsList.ItemHeight = 18;
            this.wifsList.Location = new System.Drawing.Point(146, 88);
            this.wifsList.Name = "wifsList";
            this.wifsList.Size = new System.Drawing.Size(530, 130);
            this.wifsList.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "WIF私钥";
            // 
            // wifText
            // 
            this.wifText.Location = new System.Drawing.Point(146, 224);
            this.wifText.Name = "wifText";
            this.wifText.Size = new System.Drawing.Size(442, 28);
            this.wifText.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(601, 224);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 28);
            this.button1.TabIndex = 7;
            this.button1.Text = "添加";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(72, 276);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 18);
            this.label4.TabIndex = 8;
            this.label4.Text = "资产";
            // 
            // comAsset
            // 
            this.comAsset.FormattingEnabled = true;
            this.comAsset.Location = new System.Drawing.Point(146, 273);
            this.comAsset.Name = "comAsset";
            this.comAsset.Size = new System.Drawing.Size(121, 26);
            this.comAsset.TabIndex = 9;
            this.comAsset.SelectedIndexChanged += new System.EventHandler(this.comAsset_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 399);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 18);
            this.label5.TabIndex = 10;
            this.label5.Text = "转账金额";
            // 
            // amountText
            // 
            this.amountText.Location = new System.Drawing.Point(146, 396);
            this.amountText.Name = "amountText";
            this.amountText.Size = new System.Drawing.Size(121, 28);
            this.amountText.TabIndex = 11;
            // 
            // balanceText
            // 
            this.balanceText.AutoSize = true;
            this.balanceText.Location = new System.Drawing.Point(314, 276);
            this.balanceText.Name = "balanceText";
            this.balanceText.Size = new System.Drawing.Size(98, 18);
            this.balanceText.TabIndex = 12;
            this.balanceText.Text = "0.00000000";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(643, 428);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(115, 39);
            this.button2.TabIndex = 13;
            this.button2.Text = "转账";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(36, 337);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 18);
            this.label6.TabIndex = 14;
            this.label6.Text = "目标地址";
            // 
            // targetAddr
            // 
            this.targetAddr.Location = new System.Drawing.Point(146, 337);
            this.targetAddr.Name = "targetAddr";
            this.targetAddr.Size = new System.Drawing.Size(530, 28);
            this.targetAddr.TabIndex = 15;
            // 
            // MSign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 494);
            this.Controls.Add(this.targetAddr);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.balanceText);
            this.Controls.Add(this.amountText);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comAsset);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.wifText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.wifsList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxName);
            this.Name = "MSign";
            this.Text = "sign";
            this.Load += new System.EventHandler(this.sign_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox wifsList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox wifText;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comAsset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox amountText;
        private System.Windows.Forms.Label balanceText;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox targetAddr;
    }
}