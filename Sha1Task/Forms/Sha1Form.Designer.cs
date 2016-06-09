
namespace Sha1Task
{
	partial class Sha1Form
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sha1Form));
            this.label1 = new System.Windows.Forms.Label();
            this.tbInputText = new System.Windows.Forms.TextBox();
            this.bLoadFromFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCollisionText = new System.Windows.Forms.TextBox();
            this.bSaveToFile = new System.Windows.Forms.Button();
            this.bCalcSha1 = new System.Windows.Forms.Button();
            this.bFindCollision = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.lSha1Hash = new System.Windows.Forms.Label();
            this.lSha1Time = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lCollisionTime = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lCollisionHash = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lAttemptCount = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbHashBitCount = new System.Windows.Forms.TextBox();
            this.lHashCount = new System.Windows.Forms.Label();
            this.lTruncSha1Hash = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lColliseHashCount = new System.Windows.Forms.Label();
            this.lTruncCollisionHash = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "Введите данные для вычисления хеша:";
            // 
            // tbInputText
            // 
            this.tbInputText.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbInputText.Location = new System.Drawing.Point(13, 43);
            this.tbInputText.Multiline = true;
            this.tbInputText.Name = "tbInputText";
            this.tbInputText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbInputText.Size = new System.Drawing.Size(299, 124);
            this.tbInputText.TabIndex = 2;
            this.tbInputText.WordWrap = false;
            // 
            // bLoadFromFile
            // 
            this.bLoadFromFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bLoadFromFile.Location = new System.Drawing.Point(318, 43);
            this.bLoadFromFile.Name = "bLoadFromFile";
            this.bLoadFromFile.Size = new System.Drawing.Size(179, 30);
            this.bLoadFromFile.TabIndex = 3;
            this.bLoadFromFile.Text = "Загрузить из файла";
            this.bLoadFromFile.UseVisualStyleBackColor = true;
            this.bLoadFromFile.Click += new System.EventHandler(this.OnBLoadFromFileClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(201, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "Данные найденной коллизии:";
            // 
            // tbCollisionText
            // 
            this.tbCollisionText.BackColor = System.Drawing.Color.White;
            this.tbCollisionText.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCollisionText.Location = new System.Drawing.Point(13, 45);
            this.tbCollisionText.Multiline = true;
            this.tbCollisionText.Name = "tbCollisionText";
            this.tbCollisionText.ReadOnly = true;
            this.tbCollisionText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbCollisionText.Size = new System.Drawing.Size(299, 149);
            this.tbCollisionText.TabIndex = 5;
            // 
            // bSaveToFile
            // 
            this.bSaveToFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSaveToFile.Location = new System.Drawing.Point(318, 164);
            this.bSaveToFile.Name = "bSaveToFile";
            this.bSaveToFile.Size = new System.Drawing.Size(179, 30);
            this.bSaveToFile.TabIndex = 6;
            this.bSaveToFile.Text = "Сохранить в файл";
            this.bSaveToFile.UseVisualStyleBackColor = true;
            this.bSaveToFile.Click += new System.EventHandler(this.OnBSaveToFileClick);
            // 
            // bCalcSha1
            // 
            this.bCalcSha1.BackColor = System.Drawing.Color.White;
            this.bCalcSha1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCalcSha1.Location = new System.Drawing.Point(318, 79);
            this.bCalcSha1.Name = "bCalcSha1";
            this.bCalcSha1.Size = new System.Drawing.Size(179, 113);
            this.bCalcSha1.TabIndex = 7;
            this.bCalcSha1.Text = "Вычислить хеш";
            this.bCalcSha1.UseVisualStyleBackColor = false;
            this.bCalcSha1.Click += new System.EventHandler(this.OnBCalcSha1Click);
            // 
            // bFindCollision
            // 
            this.bFindCollision.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bFindCollision.Location = new System.Drawing.Point(318, 45);
            this.bFindCollision.Name = "bFindCollision";
            this.bFindCollision.Size = new System.Drawing.Size(179, 113);
            this.bFindCollision.TabIndex = 8;
            this.bFindCollision.Text = "Найти коллизию";
            this.bFindCollision.UseVisualStyleBackColor = true;
            this.bFindCollision.Click += new System.EventHandler(this.OnBFindCollisionClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 639);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(533, 28);
            this.statusStrip1.Stretch = false;
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.Paint += new System.Windows.Forms.PaintEventHandler(this.OnStatusLabel_Paint);
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 23);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(61, 202);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 14);
            this.label3.TabIndex = 10;
            this.label3.Text = "sha1:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lSha1Hash
            // 
            this.lSha1Hash.AutoSize = true;
            this.lSha1Hash.ForeColor = System.Drawing.Color.DarkGray;
            this.lSha1Hash.Location = new System.Drawing.Point(105, 202);
            this.lSha1Hash.Name = "lSha1Hash";
            this.lSha1Hash.Size = new System.Drawing.Size(38, 14);
            this.lSha1Hash.TabIndex = 11;
            this.lSha1Hash.Text = "0x00";
            // 
            // lSha1Time
            // 
            this.lSha1Time.AutoSize = true;
            this.lSha1Time.Location = new System.Drawing.Point(105, 241);
            this.lSha1Time.Name = "lSha1Time";
            this.lSha1Time.Size = new System.Drawing.Size(77, 14);
            this.lSha1Time.TabIndex = 13;
            this.lSha1Time.Text = "0:00:00.00";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(53, 241);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 14);
            this.label5.TabIndex = 12;
            this.label5.Text = "Время:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lCollisionTime
            // 
            this.lCollisionTime.AutoSize = true;
            this.lCollisionTime.Location = new System.Drawing.Point(105, 239);
            this.lCollisionTime.Name = "lCollisionTime";
            this.lCollisionTime.Size = new System.Drawing.Size(77, 14);
            this.lCollisionTime.TabIndex = 17;
            this.lCollisionTime.Text = "0:00:00.00";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(53, 239);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 14);
            this.label7.TabIndex = 16;
            this.label7.Text = "Время:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lCollisionHash
            // 
            this.lCollisionHash.AutoSize = true;
            this.lCollisionHash.ForeColor = System.Drawing.Color.DarkGray;
            this.lCollisionHash.Location = new System.Drawing.Point(105, 204);
            this.lCollisionHash.Name = "lCollisionHash";
            this.lCollisionHash.Size = new System.Drawing.Size(38, 14);
            this.lCollisionHash.TabIndex = 15;
            this.lCollisionHash.Text = "0x00";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(61, 204);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 14);
            this.label9.TabIndex = 14;
            this.label9.Text = "sha1:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lAttemptCount
            // 
            this.lAttemptCount.AutoSize = true;
            this.lAttemptCount.Location = new System.Drawing.Point(105, 258);
            this.lAttemptCount.Name = "lAttemptCount";
            this.lAttemptCount.Size = new System.Drawing.Size(15, 14);
            this.lAttemptCount.TabIndex = 19;
            this.lAttemptCount.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(36, 258);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 14);
            this.label11.TabIndex = 18;
            this.label11.Text = "Попыток:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbHashBitCount);
            this.groupBox1.Controls.Add(this.lHashCount);
            this.groupBox1.Controls.Add(this.lTruncSha1Hash);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbInputText);
            this.groupBox1.Controls.Add(this.bLoadFromFile);
            this.groupBox1.Controls.Add(this.bCalcSha1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lSha1Hash);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lSha1Time);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(510, 270);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Исходные данные";
            // 
            // tbHashBitCount
            // 
            this.tbHashBitCount.Location = new System.Drawing.Point(199, 170);
            this.tbHashBitCount.Name = "tbHashBitCount";
            this.tbHashBitCount.Size = new System.Drawing.Size(113, 22);
            this.tbHashBitCount.TabIndex = 16;
            // 
            // lHashCount
            // 
            this.lHashCount.AutoSize = true;
            this.lHashCount.Location = new System.Drawing.Point(12, 221);
            this.lHashCount.Name = "lHashCount";
            this.lHashCount.Size = new System.Drawing.Size(92, 14);
            this.lHashCount.TabIndex = 14;
            this.lHashCount.Text = "Хеш(60 бит):";
            this.lHashCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lTruncSha1Hash
            // 
            this.lTruncSha1Hash.AutoSize = true;
            this.lTruncSha1Hash.Location = new System.Drawing.Point(105, 221);
            this.lTruncSha1Hash.Name = "lTruncSha1Hash";
            this.lTruncSha1Hash.Size = new System.Drawing.Size(38, 14);
            this.lTruncSha1Hash.TabIndex = 15;
            this.lTruncSha1Hash.Text = "0x00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 173);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(180, 14);
            this.label4.TabIndex = 10;
            this.label4.Text = "Количество значимых бит:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lColliseHashCount);
            this.groupBox2.Controls.Add(this.lTruncCollisionHash);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.tbCollisionText);
            this.groupBox2.Controls.Add(this.lAttemptCount);
            this.groupBox2.Controls.Add(this.bSaveToFile);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.bFindCollision);
            this.groupBox2.Controls.Add(this.lCollisionTime);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.lCollisionHash);
            this.groupBox2.Location = new System.Drawing.Point(12, 302);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(510, 290);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Поиск коллизии";
            // 
            // lColliseHashCount
            // 
            this.lColliseHashCount.AutoSize = true;
            this.lColliseHashCount.Location = new System.Drawing.Point(12, 222);
            this.lColliseHashCount.Name = "lColliseHashCount";
            this.lColliseHashCount.Size = new System.Drawing.Size(92, 14);
            this.lColliseHashCount.TabIndex = 20;
            this.lColliseHashCount.Text = "Хеш(60 бит):";
            this.lColliseHashCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lTruncCollisionHash
            // 
            this.lTruncCollisionHash.AutoSize = true;
            this.lTruncCollisionHash.Location = new System.Drawing.Point(105, 222);
            this.lTruncCollisionHash.Name = "lTruncCollisionHash";
            this.lTruncCollisionHash.Size = new System.Drawing.Size(38, 14);
            this.lTruncCollisionHash.TabIndex = 21;
            this.lTruncCollisionHash.Text = "0x00";
            // 
            // Sha1Form
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(533, 667);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.Name = "Sha1Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sha1Task. Работа с криптоалгоритмом";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnSha1FormFormClosing);
            this.Load += new System.EventHandler(this.OnSha1FormLoad);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbHashBitCount;
		private System.Windows.Forms.Label lTruncCollisionHash;
		private System.Windows.Forms.Label lColliseHashCount;
		private System.Windows.Forms.Label lTruncSha1Hash;
		private System.Windows.Forms.Label lHashCount;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label lAttemptCount;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label lCollisionHash;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lCollisionTime;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lSha1Time;
		private System.Windows.Forms.Label lSha1Hash;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ToolStripStatusLabel statusLabel;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.Button bFindCollision;
		private System.Windows.Forms.Button bCalcSha1;
		private System.Windows.Forms.Button bSaveToFile;
		private System.Windows.Forms.TextBox tbCollisionText;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button bLoadFromFile;
		private System.Windows.Forms.TextBox tbInputText;
		private System.Windows.Forms.Label label1;
	}
}
