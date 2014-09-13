/*
 *  DynForm
 *  Copyright (C) 2014  RL Vision
 *  
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *  
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *  
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * */
 
 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DynForm
{
    /// <summary>
    /// DynForm makes it easy to construct an edit form by automating many aspects, such as visual appearance and error checking
    /// </summary>
    public partial class DynForm : Form
	{
        // --- Private vars ---
        private bool bInitDone = false;
        private Dictionary<Control, string> sourceProperties = new Dictionary<Control, string>();
		private IDynFormEntity sourceDataEntity;
        private Control bFirstInputControl = null;
        private Dictionary<Control, DynFormSelector> dynFormSelectors = new Dictionary<Control, DynFormSelector>();

        // --- Constructor ---
        /// <summary>
        /// DynForm constructor
        /// </summary>
        /// <param name="formTitle">Window caption</param>
        /// <param name="entity">Object containing the data to be edited. Must implement the IDynFormEntity interface</param>
        public DynForm(string formTitle, IDynFormEntity entity )
        {
            InitializeComponent();
            tableLayoutPanel.RowCount--;

            // setup form
            this.Text = formTitle;
            sourceDataEntity = entity;
            entity.SetupFormFields(this);
            AddLayoutRow(20);

            tableLayoutPanel.AutoSize = true;
            groupBox.AutoSize = true;
            this.AutoSize = true;

            // done
            bInitDone = true;
        }

		// --- Private methods for simplifying layout management ---
		private void AddLayoutRow()
		{
			RowStyle rs = new RowStyle( SizeType.AutoSize );
			tableLayoutPanel.RowCount++;
			tableLayoutPanel.RowStyles.Add( rs );
		}

        private void AddLayoutRow( int pixels )
        {
            RowStyle rs = new RowStyle(SizeType.Absolute);
            rs.Height = pixels;
            tableLayoutPanel.RowCount++;
            tableLayoutPanel.RowStyles.Add(rs);

            tableLayoutPanel.Controls.Add( new Label() , 0, tableLayoutPanel.RowCount);     // need a control or else the tablelayoutpanel won't work correctly
        }

        private void SetLabel(string label)
        {
			Label l = new Label();
			l.Text = label;
            l.TextAlign = ContentAlignment.MiddleRight;
            l.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
			tableLayoutPanel.Controls.Add( l, 0, tableLayoutPanel.RowCount );
        }

		// --- Public methods for adding widgets to the form. Call these from your IDynFormEntity classes ---

        /// <summary>
        /// Adds a header to the form. Useful to separate difference areas of a form
        /// </summary>
        /// <param name="label">Header text</param>
        public void AddLayoutHeader(string label)
        {
            AddLayoutRow(10);
            AddLayoutRow();

            Label l = new Label();
            l.Text = label;
            l.Font = new Font(l.Font, FontStyle.Bold);
            l.ForeColor = Color.FromArgb(0, 0x33, 0x99);
            l.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel.Controls.Add(l, 0, tableLayoutPanel.RowCount);
        }

        /// <summary>
        /// Adds a read only label to the DynForm. Use when presenting data such as a primary key that is not allowed to be changed
        /// </summary>
        /// <param name="label">Label for the form item</param>
        /// <param name="propertyName">String containing the name of the property in the entity to use as source for the label text. This property must be of type <i>String</i></param>
        public void AddLayoutLabel(string label, string propertyName )
        {
            AddLayoutRow();
            SetLabel(label);

            Label newControl = new Label();
            var prop = sourceDataEntity.GetType().GetProperty(propertyName).GetValue(sourceDataEntity, null);
            if (prop != null)
            {
                if (prop is DateTime)
                {
                    DateTime dt = (DateTime)prop;
                    newControl.Text = dt.ToString("yyyy-MM-dd");
                }
                else
                    newControl.Text = prop.ToString();
            }
            //newControl.Text = sourceDataEntity.GetType().GetProperty(propertyName).GetValue(sourceDataEntity, null).ToString();
            newControl.AutoSize = false;
            newControl.Dock = DockStyle.Fill;
            newControl.TextAlign = ContentAlignment.MiddleLeft;
            newControl.ForeColor = SystemColors.GrayText;

            tableLayoutPanel.Controls.Add(newControl, 1, tableLayoutPanel.RowCount);

        }

        /// <summary>
        /// Adds a <i>Textbox</i> to the DynForm
        /// </summary>
        /// <param name="label">Label for the form item</param>
        /// <param name="propertyName">String containing the name of the property in the entity to bind to this control. This property must be of type <i>String</i></param>
        /// <param name="maxLength">Limits the allowed length of the input box</param>
        public void AddLayoutTextBox(string label, string propertyName, int maxLength = 0)
		{

			AddLayoutRow();
            SetLabel(label);

			TextBox newControl = new TextBox();
            var prop = sourceDataEntity.GetType().GetProperty(propertyName).GetValue(sourceDataEntity, null);
            if (prop != null ) newControl.Text = prop.ToString();

            if (maxLength != 0) newControl.MaxLength = maxLength;
			newControl.TextChanged += new EventHandler( ValueChanged );
            newControl.Width = 240;
            tableLayoutPanel.Controls.Add(newControl, 1, tableLayoutPanel.RowCount);
        
            sourceProperties.Add(newControl, propertyName);
            if (bFirstInputControl == null) bFirstInputControl = newControl;
        }

        /// <summary>
        /// Adds a <i>Masked Textbox</i> to the DynForm
        /// </summary>
        /// <param name="label">Label for the form item</param>
        /// <param name="propertyName">String containing the name of the property in the entity to bind to this control. This property must be of type <i>String</i></param>
        /// <param name="mask">The mask pattern to use in the MaskedTextBox control</param>
        public void AddLayoutMaskedTextBox(string label, string propertyName, string mask)
        {
            AddLayoutRow();
            SetLabel(label);

            MaskedTextBox newControl = new MaskedTextBox();
            newControl.Mask = mask;
            //newControl.Text = sourceDataEntity.GetType().GetProperty(propertyName).GetValue(sourceDataEntity, null).ToString();
            var prop = sourceDataEntity.GetType().GetProperty(propertyName).GetValue(sourceDataEntity, null) as string;
            if (prop != null) newControl.Text = prop.ToString();

            newControl.TextChanged += new EventHandler(ValueChanged);
            newControl.Width = 240;
            tableLayoutPanel.Controls.Add(newControl, 1, tableLayoutPanel.RowCount);

            sourceProperties.Add(newControl, propertyName);
            if (bFirstInputControl == null) bFirstInputControl = newControl;
        }

        /// <summary>
        /// Adds a <i>Checkbox</i> to the DynForm
        /// </summary>
        /// <param name="label">Label for the form item</param>
        /// <param name="propertyName">String containing the name of the property in the entity to bind to this control. Must be of type <i>Boolean</i></param>
        public void AddLayoutCheckbox(string label, string propertyName)
        {
            AddLayoutRow();

            CheckBox newControl = new CheckBox();
            newControl.Checked = (bool)sourceDataEntity.GetType().GetProperty(propertyName).GetValue(sourceDataEntity, null);

            newControl.Text = label;
            newControl.CheckedChanged += new EventHandler(ValueChanged);
            tableLayoutPanel.Controls.Add(newControl, 1, tableLayoutPanel.RowCount);

            sourceProperties.Add(newControl, propertyName);
            if (bFirstInputControl == null) bFirstInputControl = newControl;
        }

        /// <summary>
        /// Adds a <i>NumericUpDown</i> control to the DynForm
        /// </summary>
        /// <param name="label">Label for the form item</param>
        /// <param name="propertyName">String containing the name of the property in the entity to bind to this control. This property must be of type <i>Decimal</i></param>
        /// <param name="labelAfter">Label to put after the control, for example "meters"</param>
        /// <param name="numDecimals">Number of numDecimals to use. Enter 0 for intergers only.</param>
        /// <param name="min">Smallest allowed number</param>
        /// <param name="max">Largest allowed number</param>
        public void AddLayoutNumberBox(string label, string propertyName, int numDecimals = 0, int min = 0, int max = Int32.MaxValue)
        {
            AddLayoutRow();
            SetLabel(label);

            NumericUpDown newControl = new NumericUpDown();
            newControl.Maximum = max;
            newControl.Minimum = min;
            newControl.DecimalPlaces = numDecimals;
            newControl.ValueChanged += new EventHandler(ValueChanged);
            newControl.Width = 55;


            decimal val = Convert.ToDecimal(sourceDataEntity.GetType().GetProperty(propertyName).GetValue(sourceDataEntity, null));
            if (val < min) val = min;
            if (val > max) val = max;
            newControl.Value = val;

            // convert num of numDecimals to "up-down steps", ex:  0 -> 1   1 -> 0.1    2 -> 0.01
            decimal d = 1;
            while( numDecimals > 0 )
            {
                d /= 10;
                numDecimals--;
            }
            newControl.Increment = d;

            tableLayoutPanel.Controls.Add(newControl, 1, tableLayoutPanel.RowCount);

            sourceProperties.Add(newControl, propertyName);
            if (bFirstInputControl == null) bFirstInputControl = newControl;
        }

        /// <summary>
        /// Adds a <i>DateTimePicker</i> control to the DynForm
        /// </summary>
        /// <param name="label">Label for the form item</param>
        /// <param name="propertyName">String containing the name of the property in the entity to bind to this control. This property must be of type <i>DateTime</i></param>
        public void AddLayoutDatePicker(string label, string propertyName)
        {
            AddLayoutRow();
            SetLabel(label);

            DateTimePicker newControl = new DateTimePicker();
            newControl.Value = (DateTime)sourceDataEntity.GetType().GetProperty(propertyName).GetValue(sourceDataEntity, null);

            newControl.ValueChanged += new EventHandler(ValueChanged);
            tableLayoutPanel.Controls.Add(newControl, 1, tableLayoutPanel.RowCount);

            sourceProperties.Add(newControl, propertyName);
            if (bFirstInputControl == null) bFirstInputControl = newControl;
        }

        /// <summary>
        /// Adds a dropdown style <i>ComboBox</i> menu to the DynForm
        /// </summary>
        /// <param name="label">Label for the form item</param>
        /// <param name="propertyName">String containing the name of the property in the entity to bind to this control. This property must be of type <i>List&lt;DynFormList&gt;</i></param>
        /// <param name="sourceList">Contains items to show in the dropdown combobox.</param>
        public void AddLayoutDropdown(string label, string propertyName, List<DynFormList> sourceList)
        {
            AddLayoutRow();
            SetLabel(label);

            ComboBox newControl = new ComboBox();
            newControl.DropDownStyle = ComboBoxStyle.DropDownList;

            // add items to dropdown list (don't use datasource here)
            string selectedId = sourceDataEntity.GetType().GetProperty(propertyName).GetValue(sourceDataEntity, null) as string;
            for (int i = 0; i < sourceList.Count; i++)
            {
                newControl.Items.Add( sourceList[i] );
                if (sourceList[i].Id == selectedId)
                {
                    newControl.SelectedIndex = i;
                }
            }
            newControl.DisplayMember = "Text";
            newControl.ValueMember = "Id";
            newControl.SelectedValueChanged += new EventHandler(ValueChanged);
            newControl.Width = 240;

            tableLayoutPanel.Controls.Add(newControl, 1, tableLayoutPanel.RowCount);

            sourceProperties.Add(newControl, propertyName);
            if (bFirstInputControl == null) bFirstInputControl = newControl;
        }

        /// <summary>
        /// Adds a <i>ListBox</i> to the DynForm
        /// </summary>
        /// <param name="label">Label for the form item</param>
        /// <param name="propertyName">String containing the name of the property in the entity to bind to this control. This property must be of type <i>List&lt;DynFormList&gt;</i></param>
        /// <param name="sourceList">List containing all possible items that the user can add to the list</param>
        /// <param name="addTitle">Caption of the secondary window used to add items to the list</param>
        public void AddLayoutListbox(string label, string propertyName, List<DynFormList> sourceList, string addTitle)
        {
            AddLayoutRow();
            SetLabel(label);

            ListBox newControl = new ListBox();

            newControl.DataSource = sourceDataEntity.GetType().GetProperty(propertyName).GetValue(sourceDataEntity, null) as List<DynFormList>;
            newControl.DisplayMember = "Text";
            newControl.ValueMember = "Id";
            newControl.DataSourceChanged += new EventHandler(ValueChanged);
            newControl.Width = 240;

            tableLayoutPanel.Controls.Add(newControl, 1, tableLayoutPanel.RowCount);

			// create a new instance of the selector form to use later from the add button
            dynFormSelectors.Add(newControl, new DynFormSelector(addTitle, sourceList, null));

            sourceProperties.Add(newControl, propertyName);
            if (bFirstInputControl == null) bFirstInputControl = newControl;


            // each listbox needs add/del buttons underneath too
            AddLayoutRow();

            Button b1 = new Button();
            b1.Text = "Add";
            b1.AutoSize = true;
            b1.Click += new EventHandler(ListBoxAddClick);
            Button b2 = new Button();
            b2.Text = "Remove";
            b2.AutoSize = true;
            b2.Left = b1.Left + b1.Width + 2;
            b2.Click += new EventHandler(ListBoxDelClick);

            b1.Tag = newControl;
            b2.Tag = newControl;

            Panel p = new Panel();
            p.AutoSize = true;
            p.Controls.Add(b1);
            p.Controls.Add(b2);

            tableLayoutPanel.Controls.Add(p, 1, tableLayoutPanel.RowCount);
        }

        /// <summary>
        /// Adds a <i>DataGridView</i> to the DynForm with two columns, The second column contains checkboxes
        /// </summary>
        /// <param name="label">Label for the form item</param>
        /// <param name="propertyName">String containing the name of the property in the entity to bind to this control. This property must be of type <i>List&lt;DynFormList_CheckboxDataGrid&gt;</i></param>
        /// <param name="columnNameMain">Header of the main column (item names)</param>
        /// <param name="columnNameCheckboxes">Header of the editable column (checkboxes)</param>
        public void AddLayoutCheckboxDataGrid(string label, string propertyName, string columnNameMain, string columnNameCheckboxes)
        {
            AddLayoutRow();
            SetLabel(label);

            DataGridView newControl = new DataGridView();
            newControl.RowHeadersVisible = false;
            newControl.AllowUserToAddRows = false;
            newControl.AllowUserToDeleteRows = false;
            newControl.AllowUserToOrderColumns = false;
            newControl.AllowUserToResizeRows = false;
            newControl.BorderStyle = BorderStyle.None;
            newControl.Width = 240;

            newControl.CellValueChanged += new DataGridViewCellEventHandler(DataGridCellValueChanged);
            newControl.CellContentClick += new DataGridViewCellEventHandler(DataGridCellClicked);
            newControl.CellContentDoubleClick += new DataGridViewCellEventHandler(DataGridCellClicked);

            DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
            col1.HeaderText = columnNameMain;
            col1.ReadOnly = true;
            col1.DataPropertyName = "text";
            col1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            newControl.Columns.Add(col1);

            DataGridViewCheckBoxColumn col2 = new DataGridViewCheckBoxColumn();
            col2.HeaderText = columnNameCheckboxes;
            col2.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            newControl.Columns.Add(col2);


            List<DynFormList_CheckboxDataGrid> dataList = sourceDataEntity.GetType().GetProperty(propertyName).GetValue(sourceDataEntity, null) as List<DynFormList_CheckboxDataGrid>;
            newControl.Tag = dataList;

            for (int i = 0; i < dataList.Count; i++)
            {
                DynFormList_CheckboxDataGrid data = dataList[i];
                int n = newControl.Rows.Add();
                newControl.Rows[n].Cells[0].Value = data.Text;
                newControl.Rows[n].Cells[1].Value = data.Checked;
            }
                        
            tableLayoutPanel.Controls.Add(newControl, 1, tableLayoutPanel.RowCount);

            sourceProperties.Add(newControl, propertyName);
            if (bFirstInputControl == null) bFirstInputControl = newControl;


            // put select all/none buttons underneath too
            AddLayoutRow();

            Random rnd = new Random();

            Button b1 = new Button();
            b1.Text = "Select all";
            b1.Name = "all" + rnd.Next();
            b1.AutoSize = true;
            b1.Click += new EventHandler(DatagridSelectAllNoneClick);
            Button b2 = new Button();
            b2.Text = "Deselect all";
            b2.Name = "none" + rnd.Next();
            b2.AutoSize = true;
            b2.Left = b1.Left + b1.Width + 2;
            b2.Click += new EventHandler(DatagridSelectAllNoneClick);

            b1.Tag = newControl;
            b2.Tag = newControl;

            Panel p = new Panel();
            p.AutoSize = true;
            p.Controls.Add(b1);
            p.Controls.Add(b2);

            tableLayoutPanel.Controls.Add(p, 1, tableLayoutPanel.RowCount);

        }

        /// <summary>
        /// Adds a <i>DataGridView</i> list to the DynForm with two columns. The second column contains dropdown style ComboBoxes
        /// </summary>
        /// <param name="label">Label for the form item</param>
        /// <param name="propertyName">String containing the name of the property in the entity to bind to this control. This property must be of type <i>List&lt;DynFormList_ValueDataGrid&gt;</i></param>
        /// <param name="sourceList">Contains the list of items to show in the dropdown comboboxes</param>
        /// <param name="columnNameMain">Header of the main column (item names)</param>
        /// <param name="columnNameDropdown">Header of the editable column (dropdown list)</param>
        public void AddLayoutDropdownDataGrid(string label, string propertyName, List<DynFormList> sourceList, string columnNameMain, string columnNameDropdown)
        {
            AddLayoutRow();
            SetLabel(label);

            DataGridView newControl = new DataGridView();
            newControl.RowHeadersVisible = false;
            newControl.AllowUserToAddRows = false;
            newControl.AllowUserToDeleteRows = false;
            newControl.AllowUserToOrderColumns = false;
            newControl.AllowUserToResizeRows = false;
            newControl.BorderStyle = BorderStyle.None;
            newControl.Width = 240;

            newControl.CellValueChanged += new DataGridViewCellEventHandler(DataGridCellValueChanged);
            newControl.CellContentClick += new DataGridViewCellEventHandler(DataGridCellClicked);
            newControl.CellContentDoubleClick += new DataGridViewCellEventHandler(DataGridCellClicked);

            DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
            col1.HeaderText = columnNameMain;
            col1.ReadOnly = true;
            col1.DataPropertyName = "text";
            col1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            newControl.Columns.Add(col1);

            DataGridViewComboBoxColumn col2 = new DataGridViewComboBoxColumn();
            col2.HeaderText = columnNameDropdown;
            //col2.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            foreach (var o in sourceList) col2.Items.Add(o);
            col2.ValueMember = "Id";
            col2.DisplayMember = "Text";
            newControl.Columns.Add(col2);


            List<DynFormList_ValueDataGrid> dataList = sourceDataEntity.GetType().GetProperty(propertyName).GetValue(sourceDataEntity, null) as List<DynFormList_ValueDataGrid>;
            newControl.Tag = dataList;

            for (int i = 0; i < dataList.Count; i++)
            {
                DynFormList_ValueDataGrid data = dataList[i];
                int n = newControl.Rows.Add();
                newControl.Rows[n].Cells[0].Value = data.Text;
                newControl.Rows[n].Cells[1].Value = data.Value;
            }

            tableLayoutPanel.Controls.Add(newControl, 1, tableLayoutPanel.RowCount);
            sourceProperties.Add(newControl, propertyName);
            if (bFirstInputControl == null) bFirstInputControl = newControl;
        }

        /// <summary>
        /// Adds a <i>DataGridView</i> list to the DynForm with two columns. The second column contains an input field.
        /// </summary>
        /// <param name="label">Label for the form item</param>
        /// <param name="propertyName">String containing the name of the property in the entity to bind to this control. This property must be of type <i>List&lt;DynFormList_ValueDataGrid&gt;</i></param>
        /// <param name="columnNameMain">Header of the main column (item names)</param>
        /// <param name="columnNameValue">Header of the editable column (textbox)</param>
		/// /// <param name="dafaultValue">Default value in the editable column when entering a new item</param>
        /// <param name="maxLength">Limits the number of characters allowed to type in the textbox</param>
        public void AddLayoutValueDataGrid(string label, string propertyName, string columnNameMain, string columnNameValue, List<DynFormList> sourceList, string addTitle, string defaultValue, int maxLength = 0)
        {
            AddLayoutRow();
            SetLabel(label);

            DataGridView newControl = new DataGridView();
            newControl.RowHeadersVisible = false;
            newControl.AllowUserToAddRows = false;
            newControl.AllowUserToDeleteRows = false;
            newControl.AllowUserToOrderColumns = false;
            newControl.AllowUserToResizeRows = false;
            newControl.BorderStyle = BorderStyle.None;
            newControl.Width = 240;

            newControl.CellValueChanged += new DataGridViewCellEventHandler(DataGridCellValueChanged);

            DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
            col1.HeaderText = columnNameMain;
            col1.ReadOnly = true;
            col1.DataPropertyName = "text";
            col1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            newControl.Columns.Add(col1);

            DataGridViewTextBoxColumn col2 = new DataGridViewTextBoxColumn();
            col2.HeaderText = columnNameValue;
			col2.Tag = defaultValue;
            if (maxLength != 0) col2.MaxInputLength = maxLength;
            //col2.DefaultCellStyle.Format = @"0\%";
            newControl.Columns.Add(col2);

            List<DynFormList_ValueDataGrid> dataList = sourceDataEntity.GetType().GetProperty(propertyName).GetValue(sourceDataEntity, null) as List<DynFormList_ValueDataGrid>;
            newControl.Tag = dataList;

            for (int i = 0; i < dataList.Count; i++)
            {
                DynFormList_ValueDataGrid data = dataList[i];
                int n = newControl.Rows.Add();
                newControl.Rows[n].Tag = data.Id;
                newControl.Rows[n].Cells[0].Value = data.Text;
                newControl.Rows[n].Cells[1].Value = data.Value;
            }

            tableLayoutPanel.Controls.Add(newControl, 1, tableLayoutPanel.RowCount);
            sourceProperties.Add(newControl, propertyName);
            if (bFirstInputControl == null) bFirstInputControl = newControl;

            // create a new instance of the selector form to use later from the add button
            //newControl.Tag = new DynFormSelector(addTitle, sourceList, dataList);
            dynFormSelectors.Add(newControl, new DynFormSelector(addTitle, sourceList, dataList));

            // add add/del buttons underneath too
            AddLayoutRow();

            Button b1 = new Button();
            b1.Text = "Add";
            b1.AutoSize = true;
            b1.Click += new EventHandler(DataGridViewAddClick);
            Button b2 = new Button();
            b2.Text = "Remove";
            b2.AutoSize = true;
            b2.Left = b1.Left + b1.Width + 2;
            b2.Click += new EventHandler(DataGridViewDelClick);

            b1.Tag = newControl;
            b2.Tag = newControl;

            Panel p = new Panel();
            p.AutoSize = true;
            p.Controls.Add(b1);
            p.Controls.Add(b2);

            tableLayoutPanel.Controls.Add(p, 1, tableLayoutPanel.RowCount);

        }

       
        // --- Events to perform validation. The actual validation is performed by each iDynFormEntity class' Validate() method ---
		private void ValueChanged( object sender, EventArgs e )
		{
            if (!sourceProperties.ContainsKey(sender as Control)) return;
            if (!bInitDone) return;

            string propertyName = sourceProperties[ sender as Control];

            // update & validate source propetry
            if (sender is TextBox)
            {
                string value = (sender as TextBox).Text;
                sourceDataEntity.GetType().GetProperty(propertyName).SetValue(sourceDataEntity, value, null);
            }
            else if (sender is MaskedTextBox)
            {
                string value = (sender as MaskedTextBox).Text;
                sourceDataEntity.GetType().GetProperty(propertyName).SetValue(sourceDataEntity, value, null);
            }
            else if (sender is DateTimePicker)
            {
                DateTime value = (sender as DateTimePicker).Value;
                sourceDataEntity.GetType().GetProperty(propertyName).SetValue(sourceDataEntity, value, null);
            }
            else if (sender is NumericUpDown)
            {
                // todo: updown can be empty -> manage in lost focus??
                Decimal value = (sender as NumericUpDown).Value;
                System.Reflection.PropertyInfo p = sourceDataEntity.GetType().GetProperty(propertyName);

                if (p.PropertyType == typeof(decimal)) sourceDataEntity.GetType().GetProperty(propertyName).SetValue(sourceDataEntity, value, null);
                if (p.PropertyType == typeof(int) ) sourceDataEntity.GetType().GetProperty(propertyName).SetValue(sourceDataEntity, Convert.ToInt32(value), null);
                if (p.PropertyType == typeof(double)) sourceDataEntity.GetType().GetProperty(propertyName).SetValue(sourceDataEntity, Convert.ToDouble(value), null);

            }
            else if (sender is CheckBox)
            {
                bool value = (sender as CheckBox).Checked;
                sourceDataEntity.GetType().GetProperty(propertyName).SetValue(sourceDataEntity, value, null);
            }
            else if (sender is ComboBox)
            {
                ComboBox cb = sender as ComboBox;
                string id = "";
                if (cb.SelectedItem != null) id = (cb.SelectedItem as DynFormList).Id;
                sourceDataEntity.GetType().GetProperty(propertyName).SetValue(sourceDataEntity, id, null);
            }
            else if (sender is ListBox)
            {
                if ((sender as ListBox).DataSource == null) return;
                List<DynFormList> value = (sender as ListBox).DataSource as List<DynFormList>;
                sourceDataEntity.GetType().GetProperty(propertyName).SetValue(sourceDataEntity, value, null);
            }
            else return;

            // let entity class validate the new value
            string err = sourceDataEntity.Validate( propertyName );
            if (err.StartsWith("!"))    //warning
            {
                err = err.Substring(1); // remove "!"
                errorProviderWarning.SetError(sender as Control, err);
                errorProviderWarning.SetIconPadding(sender as Control, 3);
                errorProvider.SetError(sender as Control, "");
            }
            else if (err.Trim() != "" )    // error
            {
                errorProvider.SetError(sender as Control, err);
                errorProvider.SetIconPadding(sender as Control, 3);
                errorProviderWarning.SetError(sender as Control, "");
            }
            else
            {
                errorProvider.SetError(sender as Control, "");
                errorProviderWarning.SetError(sender as Control, "");
            }
            CheckForRemainingErrors();

		}

        void DataGridCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // DataGridView: update soruce property & validate new value

            if (!sourceProperties.ContainsKey(sender as Control)) return;
            if (!bInitDone) return;

            if (e != null)
            {
                int col = e.ColumnIndex;
                int row = e.RowIndex;
                DataGridView grid = sender as DataGridView;

                if (col == 0) return;   // no need to validate first (static) column

                if (grid.Tag is List<DynFormList_CheckboxDataGrid>) // checkboxes
                {
                    List<DynFormList_CheckboxDataGrid> dataList = grid.Tag as List<DynFormList_CheckboxDataGrid>;
                    DynFormList_CheckboxDataGrid data = dataList[row];
                    data.Checked = (bool)grid.Rows[row].Cells[col].Value;
                }
                else    // string values
                {
                    List<DynFormList_ValueDataGrid> dataList = grid.Tag as List<DynFormList_ValueDataGrid>;
                    DynFormList_ValueDataGrid data = dataList[row];
                    data.Value = (string)grid.Rows[row].Cells[col].Value;
                }
            }

            // let entity class validate the new value
            string propertyName = sourceProperties[sender as Control];
            string err = sourceDataEntity.Validate(propertyName);
            if (err.StartsWith("!"))    //warning
            {
                err = err.Substring(1); // remove "!"
                errorProviderWarning.SetError(sender as Control, err);
                errorProviderWarning.SetIconPadding(sender as Control, 3);
                errorProvider.SetError(sender as Control, "");
            }
            else if (err.Trim() != "")    // error
            {
                errorProvider.SetError(sender as Control, err);
                errorProvider.SetIconPadding(sender as Control, 3);
                errorProviderWarning.SetError(sender as Control, "");
            }
            else
            {
                errorProvider.SetError(sender as Control, "");
                errorProviderWarning.SetError(sender as Control, "");
            }
            CheckForRemainingErrors();

        }

        private void CheckForRemainingErrors()	// runs after each event
        {
            if (!bInitDone) return;
                
            // if the form contains an error disable the ok button
            foreach (Control c in tableLayoutPanel.Controls)
            {
                if (errorProvider.GetError(c) != "")
                {
                    btnOk.Enabled = false;
                    return;
                }
            }
            btnOk.Enabled = true;
        }


		// --- ListBox Add/Del buttons ---
		void ListBoxAddClick( object sender, EventArgs e )
		{
			Button b = sender as Button;
			ListBox lb = b.Tag as ListBox;

			// open selection window
			DynFormSelector sel = dynFormSelectors[lb];
			List<DynFormList> data = lb.DataSource as List<DynFormList>;
			sel.AlreadyAdded = new List<string>();
			foreach( DynFormList d in data ) sel.AlreadyAdded.Add( d.Id );
			sel.SelectedItem = null;
			sel.ShowDialog();
			if( sel.SelectedItem != null )
			{
				List<DynFormList> list = lb.DataSource as List<DynFormList>;
				list.Add( new DynFormList( sel.SelectedItem.Id, sel.SelectedItem.Text ) );

				// reload datasource
				lb.DataSource = null;
				lb.DataSource = list;
				lb.DisplayMember = "Text";
				lb.ValueMember = "Id";
			}
		}
		void ListBoxDelClick( object sender, EventArgs e )
		{
			Button b = sender as Button;
			ListBox lb = b.Tag as ListBox;
            if (lb.SelectedValue == null) return;

			List<DynFormList> list = lb.DataSource as List<DynFormList>;
			list.Remove( lb.SelectedItem as DynFormList );

            // reload datasource
			lb.DataSource = null;
			lb.DataSource = list;
			lb.DisplayMember = "Text";
			lb.ValueMember = "Id";
		}

        // --- DataGridView Add/Del buttons ---
        void DataGridViewAddClick(object sender, EventArgs e)
        {
            Button b = sender as Button;
            DataGridView dgv = b.Tag as DataGridView;

            // open selection window
            DynFormSelector sel = dynFormSelectors[dgv];
            List<DynFormList_ValueDataGrid> data = sel.Destination as List<DynFormList_ValueDataGrid>;
            sel.AlreadyAdded = new List<string>();
            foreach (DynFormList_ValueDataGrid d in data) sel.AlreadyAdded.Add(d.Id);
            sel.SelectedItem = null;
            sel.ShowDialog();
            if (sel.SelectedItem != null)
            {
                data.Add( new DynFormList_ValueDataGrid( sel.SelectedItem.Id , sel.SelectedItem.Text , "" ));

                int n = dgv.Rows.Add();
                dgv.Rows[n].Tag = sel.SelectedItem.Id;
                dgv.Rows[n].Cells[0].Value = sel.SelectedItem.Text;
                dgv.Rows[n].Cells[1].Value = dgv.Columns[1].Tag.ToString();
            }

        }
        void DataGridViewDelClick(object sender, EventArgs e)
        {
            Button b = sender as Button;
            DataGridView dgv = b.Tag as DataGridView;

            if (dgv.CurrentRow == null) return;
            string removedId = (string)dgv.CurrentRow.Tag;
            dgv.Rows.Remove(dgv.CurrentRow);

            DynFormSelector sel = dynFormSelectors[dgv];
            List<DynFormList_ValueDataGrid> data = sel.Destination as List<DynFormList_ValueDataGrid>;
            for( int i=0; i<data.Count; i++ )
            {
                if (data[i].Id == removedId) data.Remove(data[i]);
            }
            DataGridCellValueChanged(dgv, null);
        }

        // --- Datagridview with Checkboxes: Select All/None buttons ---
        void DatagridSelectAllNoneClick(object sender, EventArgs e)
        {
            Button b = sender as Button;
            DataGridView grid = b.Tag as DataGridView;

            string propertyName = sourceProperties[grid as Control];
            List<DynFormList_CheckboxDataGrid> dataList = sourceDataEntity.GetType().GetProperty(propertyName).GetValue(sourceDataEntity, null) as List<DynFormList_CheckboxDataGrid>;

            for (int i = 0; i < dataList.Count; i++)
            {
                DynFormList_CheckboxDataGrid data = dataList[i];
                if (b.Name.StartsWith("all"))
                {
                    data.Checked = true;
                    grid.Rows[i].Cells[1].Value = true;
                }
                else
                {
                    data.Checked = false;
                    grid.Rows[i].Cells[1].Value = false;
                }
            }
        }


		// --- Misc events ---
		private void btnOk_Click( object sender, EventArgs e )
		{
            sourceDataEntity.bSaveData = true;
			this.Close();
		}

		private void btnCancel_Click( object sender, EventArgs e )
		{
			this.Close();
		}

        private void DynForm_Load(object sender, EventArgs e)
        {
            btnOk.Enabled = false;
        }

        void DataGridCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            // the event DataGridCellValueChanged() only triggers when lost focus. This forces a instantly trigger for a better user experience
            DataGridView grid = sender as DataGridView;
            grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DynForm_Shown(object sender, EventArgs e)
        {
			sourceDataEntity.bSaveData = false;
            RecursiveValidateAllControls(sender as Control);    // mark required fields (for brand new entities)
            if (bFirstInputControl != null) bFirstInputControl.Select();
        }

        private void RecursiveValidateAllControls(Control parentControl)
        {
            foreach (Control childControl in parentControl.Controls)
            {
                if( childControl is DataGridView )
                    DataGridCellValueChanged(childControl, null);
                else
                    ValueChanged(childControl, null);

                RecursiveValidateAllControls(childControl);
            }
        }
    }
}
