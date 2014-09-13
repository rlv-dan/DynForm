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
using System.Linq;

namespace DynForm
{
    /// <summary>
    /// DynFormSelector is part of DynForm and give the ability to select items from a predefined list, which can be filtered for easier finding
    /// </summary>
    public partial class DynFormSelector : Form
	{
		public DynFormList SelectedItem {get; set;}
        public object Destination { get; set; }
        public List<string> AlreadyAdded { get; set; }
        
        private List<DynFormList> listboxSource;

        public DynFormSelector(string formTitle, List<DynFormList> list, object destination )
		{
			InitializeComponent();

            listboxSource = list;
            DisplayFilteredList();
            Destination = destination;
			this.Text = formTitle;
            txtFilter.Select();
		}

		private void DynFormSelector_Load( object sender, EventArgs e )
		{
            SelectedItem = null;
		}

		private void btnOk_Click( object sender, EventArgs e )
		{
			SelectedItem = listBox.SelectedItem as DynFormList;
			this.Close();
		}

		private void listBox_DoubleClick( object sender, EventArgs e )
		{
			btnOk.PerformClick();
		}

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            DisplayFilteredList();
        }

        private void DisplayFilteredList()
        {
            // remove items already in destination list
            List<DynFormList> list = new List<DynFormList>();
            foreach( DynFormList toAdd in listboxSource )
            {
                if (AlreadyAdded != null)
                {
                    if (!AlreadyAdded.Contains(toAdd.Id)) list.Add(toAdd);
                }
                else
                {
                    list.Add(toAdd);
                }
            }

            // filter based on textbox content
            var filteredList = list.Where(x => x.Text.ToLower().Contains(txtFilter.Text.Trim().ToLower()));

            listBox.DataSource = filteredList.ToList();
            listBox.DisplayMember = "Text";
            listBox.ValueMember = "Id";
            if (listBox.Items.Count > 0) btnOk.Enabled = true; else btnOk.Enabled = false;
        }

        private void txtFilter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (listBox.SelectedIndex > 0) listBox.SelectedIndex = listBox.SelectedIndex - 1;
            }
            if (e.KeyCode == Keys.Down)
            {
                if (listBox.SelectedIndex < listBox.Items.Count-1) listBox.SelectedIndex = listBox.SelectedIndex + 1;
            }
        }

        private void DynFormSelector_Shown(object sender, EventArgs e)
        {
            txtFilter.Focus();
            DisplayFilteredList();
        }

	}
}
