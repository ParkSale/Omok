namespace Omok
{
    partial class playForm
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
            this.boardBox = new System.Windows.Forms.PictureBox();
            this.turn = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.boardBox)).BeginInit();
            this.SuspendLayout();
            // 
            // boardBox
            // 
            this.boardBox.BackColor = System.Drawing.Color.Olive;
            this.boardBox.Location = new System.Drawing.Point(12, 12);
            this.boardBox.Name = "boardBox";
            this.boardBox.Size = new System.Drawing.Size(634, 552);
            this.boardBox.TabIndex = 0;
            this.boardBox.TabStop = false;
            this.boardBox.Paint += new System.Windows.Forms.PaintEventHandler(this.boardBox_Paint);
            this.boardBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.boardBox_MouseDown);
            // 
            // turn
            // 
            this.turn.AutoSize = true;
            this.turn.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.turn.Location = new System.Drawing.Point(652, 39);
            this.turn.Name = "turn";
            this.turn.Size = new System.Drawing.Size(251, 21);
            this.turn.TabIndex = 1;
            this.turn.Text = "현재 BLACK의 차례입니다";
            // 
            // playForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(921, 576);
            this.Controls.Add(this.turn);
            this.Controls.Add(this.boardBox);
            this.Name = "playForm";
            this.Text = "play";
            ((System.ComponentModel.ISupportInitialize)(this.boardBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox boardBox;
        private System.Windows.Forms.Label turn;
    }
}