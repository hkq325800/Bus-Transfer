namespace NewIndex
{
    partial class NewIndex
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewIndex));
            this.btnNewIndex = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnNewIndex
            // 
            this.btnNewIndex.Location = new System.Drawing.Point(181, 207);
            this.btnNewIndex.Name = "btnNewIndex";
            this.btnNewIndex.Size = new System.Drawing.Size(75, 23);
            this.btnNewIndex.TabIndex = 0;
            this.btnNewIndex.Text = "确认";
            this.btnNewIndex.UseVisualStyleBackColor = true;
            // 
            // NewIndex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnNewIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewIndex";
            this.Text = "新增站点";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNewIndex;
    }
}

