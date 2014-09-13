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
using System.Windows.Forms;

namespace DynForm
{
	public interface IDynFormEntity
	{
        /// <summary>
        /// Indicates if the data has been modified in the DynForm and the user pressed the save button. If set to false you should discard the object, since the data has likely been updated before the users pressed cancel!
        /// </summary>
        bool bSaveData { get; set; }

        /// <summary>
        /// Method that allows the entity to perform validation on it's data and return status back to DynForm. Note that all properties are instantly reflected into the entity when changed in the DynForm gui.
        /// </summary>
        /// <param name="propertyName">String containing the name of the property that should be validated.</param>
        /// <returns>Should return a string containing a description of the error, or an empty string when for no error</returns>
        string Validate(string propertyName );

        /// <summary>
        /// This is where the entity builds the DynForm content. Automatically called by DynForm at setup
        /// </summary>
        /// <param name="f">A reference to the DynForm instance being setup.</param>
        void SetupFormFields(DynForm f);
	}
}
