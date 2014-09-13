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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynForm
{
	public partial class Example : Form
	{
		public Example()
		{
			InitializeComponent();
		}

		// Button 1
		private void buttonNewPerson_Click( object sender, EventArgs e )
		{
			// This example creates a new Person object and opens a DynForm to edit it

			Person newPerson = new Person();
			var f = new DynForm( "New Person", newPerson );
			f.ShowDialog();

			// bSaveData indicates what button the user pressed (Save or Cancel)
			if( newPerson.bSaveData )
			{
				// Print the result to the console
				string pets = "";
				foreach( var pet in newPerson.Pets ) pets += pet.Text + " / ";
				Console.WriteLine(
					String.Format( "SSN: {0}\r\nName: {1}\r\nBirthday: {2}\r\nLucky Number: {3}\r\nFavorite Color: {4}\r\nLikes Pizza: {5}\r\nPets: {6}",
					newPerson.SSN,
					newPerson.Name,
					newPerson.Birthday.ToLongDateString(),
					newPerson.LuckyNumber,
					newPerson.FavoriteColor,
					newPerson.LikesPizza ? "Yes" : "No",
					pets
					));		
			}
			else
			{
				Console.WriteLine("User clicked cancel...");
			}
		}

		// Button 2
		private void buttonEditPerson_Click( object sender, EventArgs e )
		{
			// This example shows hows to edit an already existing object. This it is slightly 
			// more complicated due to the fact that the object sent to DynForm is always modified,
			// even if the user pressed cancel. Thus we need to work with a copy of the data to 
			// make sure to preserve the original data.

			// This is the source object being edited
			Person originalPerson = new Person() { SSN = "123-45-6789", Name = "Lois Lane", Birthday = new DateTime(1938,06,01), LuckyNumber = 7, LikesPizza = true, FavoriteColor = "Blue" };
			// Create a copy of the source object
			Person copyPerson = new Person() { SSN = originalPerson.SSN, Name = originalPerson.Name, Birthday = originalPerson.Birthday, LuckyNumber = originalPerson.LuckyNumber, LikesPizza = originalPerson.LikesPizza, FavoriteColor = originalPerson.FavoriteColor };
			// Open DynForm to edit the copy
			var f = new DynForm( "New Person", copyPerson );
			f.ShowDialog();

			// Only update the source object if the user pressed Save
			string output = "";
			if( copyPerson.bSaveData )
			{
				originalPerson = copyPerson;
				output += "Updating the original object!\r\n\r\n";
			}
			else
			{
				output += "User clicked cancel, keeping original object...\r\n\r\n";
			}

			// Print the result to the console
			string pets = "";
			foreach( var pet in originalPerson.Pets ) pets += pet.Text + " / ";
			output += String.Format( "SSN: {0}\r\nName: {1}\r\nBirthday: {2}\r\nLucky Number: {3}\r\nFavorite Color: {4}\r\nLikes Pizza: {5}\r\nPets: {6}",
				originalPerson.SSN,
				originalPerson.Name,
				originalPerson.Birthday.ToLongDateString(),
				originalPerson.LuckyNumber,
				originalPerson.FavoriteColor,
				originalPerson.LikesPizza ? "Yes" : "No",
				pets
				);
			Console.WriteLine( output );
		}

		// Button 3
		private void buttonPetStore_Click( object sender, EventArgs e )
		{
			// This example shows how to work with more advanced data structures.
			// Go to the PetStore class to read more.
			PetStore petStore = new PetStore();
			var f = new DynForm( "Petstore Inventory", petStore );
			f.ShowDialog();
		}

	}
}
