namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            numericUpDown1 = new NumericUpDown();
            dataGridView1 = new DataGridView();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            dataGridView2 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(64, 64);
            label1.Name = "label1";
            label1.Size = new Size(183, 20);
            label1.TabIndex = 0;
            label1.Text = "Оберіть розмірність СНР";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(64, 143);
            label2.Name = "label2";
            label2.Size = new Size(334, 20);
            label2.TabIndex = 1;
            label2.Text = "Введіть елементи вектора початкових умов Х0";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(258, 62);
            numericUpDown1.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(41, 27);
            numericUpDown1.TabIndex = 2;
            numericUpDown1.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(64, 180);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(150, 188);
            dataGridView1.TabIndex = 3;
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ActiveCaption;
            button1.Location = new Point(124, 409);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 4;
            button1.Text = "Розв'язати";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.Location = new Point(280, 409);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 5;
            button2.Text = "Очистити";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(450, 409);
            button3.Name = "button3";
            button3.Size = new Size(94, 29);
            button3.TabIndex = 6;
            button3.Text = "Завершити";
            button3.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(429, 143);
            label3.Name = "label3";
            label3.Size = new Size(201, 20);
            label3.TabIndex = 7;
            label3.Text = "Введіть необхідні значення";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(418, 185);
            label4.Name = "label4";
            label4.Size = new Size(95, 20);
            label4.TabIndex = 8;
            label4.Text = "Точність Eps";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(467, 251);
            label5.Name = "label5";
            label5.Size = new Size(46, 20);
            label5.TabIndex = 9;
            label5.Text = "KMax";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(720, 143);
            label6.Name = "label6";
            label6.Size = new Size(177, 20);
            label6.TabIndex = 10;
            label6.Text = "Вектор розв'язку СНР X*";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(700, 418);
            label7.Name = "label7";
            label7.Size = new Size(48, 20);
            label7.TabIndex = 11;
            label7.Text = "N_Iter";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(519, 182);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 12;
            textBox1.Text = "1e-6";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(519, 248);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(125, 27);
            textBox2.TabIndex = 13;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(758, 415);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(125, 27);
            textBox3.TabIndex = 14;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(700, 180);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(216, 188);
            dataGridView2.TabIndex = 15;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(950, 480);
            Controls.Add(dataGridView2);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            Controls.Add(numericUpDown1);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "ЛАА Метод Ньютона для СНР";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private NumericUpDown numericUpDown1;
        private DataGridView dataGridView1;
        private Button button1;
        private Button button2;
        private Button button3;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private DataGridView dataGridView2;
    }
}