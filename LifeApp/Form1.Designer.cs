namespace LifeApp
{
    partial class WorldSpace
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.BeginLife = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BeginLife
            // 
            this.BeginLife.Location = new System.Drawing.Point(474, 400);
            this.BeginLife.Name = "BeginLife";
            this.BeginLife.Size = new System.Drawing.Size(75, 23);
            this.BeginLife.TabIndex = 0;
            this.BeginLife.Text = "Start";
            this.BeginLife.UseVisualStyleBackColor = true;
            this.BeginLife.Click += new System.EventHandler(this.BeginLife_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(430, 240);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 51);
            this.label1.TabIndex = 1;
            this.label1.Text = "Красные круги - хищники.\n Зеленые - растения. \n Желтые - травоядные. ";
            // 
            // WorldSpace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 500);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BeginLife);
            this.Name = "WorldSpace";
            this.Text = "World";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BeginLife;
        private System.Windows.Forms.Label label1;
    }
}

