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
using System.Text;

namespace DynForm
{
    /// <summary>
    /// Used when adding list controls to DynForm. Basically a Key-Value pair. 
	/// "Id" is used to identify the items, "Text" is what's being shown in 
	/// the control. DynFormList_ValueDataGrid is and extended version used in 
	/// more advanced controls.
    /// </summary>
    public class DynFormList
	{
        public string Id { get; set; }          // this is the reference id (key) for the values shown to the users
		public string Text { get; set; }        // this is the value shown to the user

        public DynFormList()
        {
        }
        
        public DynFormList(string id, string text)
		{
			this.Id = id;
			this.Text = text;
		}

        public DynFormList ShallowCopy()
		{
			return (DynFormList)this.MemberwiseClone();
		}
	}

    public class DynFormList_CheckboxDataGrid : DynFormList
    {
        public bool Checked { get; set; }       // corresponds to the checkbox state of the second column in the DataGridView

        public DynFormList_CheckboxDataGrid(string id, string text, bool datagridChecked)
        {
            this.Id = id;
            this.Text = text;
            this.Checked = datagridChecked;
        }
    }

    public class DynFormList_ValueDataGrid : DynFormList
    {
        public string Value { get; set; }       // contains the text value of the second column in the DataGridView

        public DynFormList_ValueDataGrid(string id, string text, string value)
        {
            this.Id = id;
            this.Text = text;
            this.Value = value;
        }
    }
}
