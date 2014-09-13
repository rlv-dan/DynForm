namespace DynForm
{
	partial class Example
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonNewPerson = new System.Windows.Forms.Button();
			this.buttonEditPerson = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonPetStore = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonNewPerson
			// 
			this.buttonNewPerson.Location = new System.Drawing.Point(17, 23);
			this.buttonNewPerson.Name = "buttonNewPerson";
			this.buttonNewPerson.Size = new System.Drawing.Size(104, 23);
			this.buttonNewPerson.TabIndex = 8;
			this.buttonNewPerson.Text = "New Person";
			this.buttonNewPerson.UseVisualStyleBackColor = true;
			this.buttonNewPerson.Click += new System.EventHandler(this.buttonNewPerson_Click);
			// 
			// buttonEditPerson
			// 
			this.buttonEditPerson.Location = new System.Drawing.Point(17, 56);
			this.buttonEditPerson.Name = "buttonEditPerson";
			this.buttonEditPerson.Size = new System.Drawing.Size(104, 23);
			this.buttonEditPerson.TabIndex = 9;
			this.buttonEditPerson.Text = "Edit Person";
			this.buttonEditPerson.UseVisualStyleBackColor = true;
			this.buttonEditPerson.Click += new System.EventHandler(this.buttonEditPerson_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.buttonPetStore);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.buttonNewPerson);
			this.groupBox2.Controls.Add(this.buttonEditPerson);
			this.groupBox2.Location = new System.Drawing.Point(12, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(504, 126);
			this.groupBox2.TabIndex = 12;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "DynForm Examples";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(127, 94);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(301, 13);
			this.label3.TabIndex = 13;
			this.label3.Text = "Button 3: Shows how setup and edit more complex data types.";
			// 
			// buttonPetStore
			// 
			this.buttonPetStore.Location = new System.Drawing.Point(17, 89);
			this.buttonPetStore.Name = "buttonPetStore";
			this.buttonPetStore.Size = new System.Drawing.Size(104, 23);
			this.buttonPetStore.TabIndex = 12;
			this.buttonPetStore.Text = "Petstore Inventory";
			this.buttonPetStore.UseVisualStyleBackColor = true;
			this.buttonPetStore.Click += new System.EventHandler(this.buttonPetStore_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(127, 61);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(369, 13);
			this.label2.TabIndex = 11;
			this.label2.Text = "Button 2: Shows how use an already existing entity of type Person and edit it.";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(127, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(336, 13);
			this.label1.TabIndex = 10;
			this.label1.Text = "Button 1: Shows how to create a new entity of type Person and edit it.";
			// 
			// Example
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(529, 152);
			this.Controls.Add(this.groupBox2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Example";
			this.Text = "DynForm Examples";
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonNewPerson;
		private System.Windows.Forms.Button buttonEditPerson;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonPetStore;
		private System.Windows.Forms.Label label3;

	}
}

