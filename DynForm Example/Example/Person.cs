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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DynForm
{
	// This class shows how to create an object that can be edited in a DynForm
	// It shows the basic data types, setting up the form and validating the data
	class Person : IDynFormEntity
	{
		// Define the properties to be edited
		public string SSN { get; set; }
		public string Name { get; set; }
		public DateTime Birthday { get; set; }
		public string FavoriteColor { get; set; }
		public int LuckyNumber { get; set; }
		public bool LikesPizza { get; set; }
		public List<DynFormList> Pets { get; set; }

		// Simple constructor with just the nesessary
		public Person()
		{
			SSN = "";
			Birthday = DateTime.Now;
			Pets = new List<DynFormList>();
		}


		// --- Below are implementations of properties and methods required by the IDynFormEntity interface ---

		public bool bSaveData { get; set; }

		public string Validate( string propertyName )
		{
			// This method is triggered when data is changed. Here we can inspect 
			// the new data and tell DynForm if it is valid. If it is valid, simply 
			// return an empty string. Otherwise, return a string instructing the user
			// what the problem is. DynForm will display this error message and will not
			// allow saving the form until the error is corrected.

			switch( propertyName )
			{
				case "SSN":
					Match match = Regex.Match(this.SSN, @"\d\d\d-\d\d-\d\d\d\d", RegexOptions.IgnoreCase);
					if( !match.Success ) return "Please enter a valid SSN";
					break;
				case "Name":
					if( !this.Name.Trim().Contains(" ") ) return "Name should be in format 'Firstname Lastname'";
					break;
				case "FavoriteColor":
					if( this.FavoriteColor == null ) return "You must set a favorite color";
					break;
				case "Pets":
					foreach( var pet in this.Pets )
					{
						if( pet.Text == "Cat" ) return "!Atchoo! You are allergic to cats...";	// Begin with ! to display a warning instead of error.
					}					
					break;
			}
			return "";
		}

		public void SetupFormFields( DynForm f )
		{
			// This is where we "design" the form to be displayed.
			// Most follow this format: f.AddSomething( "Label", "PropertyName" )
			
			// It's usually a good thing to begin by adding a header
			f.AddLayoutHeader( "Person Details" );

			// If we are creating a new person we let the user set the SSN, but when editing an 
			// existing persons we just display the SSN in a label, since you are not allowed 
			// to change your SSN.
			if( this.SSN == "" )
				f.AddLayoutMaskedTextBox( "Social Security Number", "SSN", "000-00-0000" );	// See MSDN for info on setting up the MaskedTextBox validation pattern
			else
				f.AddLayoutLabel( "Social Security Number", "SSN" );

			// Name is a simple text input box
			f.AddLayoutTextBox( "Full name", "Name", 32 );

			// Dates can be set using a date picker
			f.AddLayoutDatePicker( "Date of bith", "Birthday" );

			// New header. Separate the form into logical section for improved usability
			f.AddLayoutHeader( "Trivia" );

			// The dropdown list needs some data to work with. This data can be created on the fly, like here, or obtained from any other data source you with
			var colors = new List<DynFormList>();
			colors.Add( new DynFormList( null, "Not set" ) );
			colors.Add( new DynFormList("Red","Red like roses") );
			colors.Add( new DynFormList( "Green", "Green like a summer field" ) );
			colors.Add( new DynFormList( "Blue", "Blue like the deep ocean" ) );
			f.AddLayoutDropdown("Favourite colour", "FavoriteColor", colors);

			// Number spinner. The last arguments allows you to set number of decimals and min/max values
			f.AddLayoutNumberBox( "Lucky Number", "LuckyNumber", 0, 1, 16 );

			// A simple checkbox working against a boolean
			f.AddLayoutCheckbox( "Likes pizza?", "LikesPizza" );

			// The listbox allows you to select items from a list.
			// Again, we use the DynFormList to provide DynForm with source data (allPets)
			var allPets = new List<DynFormList>();
			allPets.Add( new DynFormList( "DogID", "Dog" ) );
			allPets.Add( new DynFormList( "CatID", "Cat" ) );
			allPets.Add( new DynFormList( "RatID", "Rat" ) );
			allPets.Add( new DynFormList( "LizardID", "Lizard" ) );
			allPets.Add( new DynFormList( "BirdID", "Bird" ) );
			allPets.Add( new DynFormList( "FishID", "Fish" ) );
			allPets.Add( new DynFormList( "RabbitID", "Rabbit" ) );
			f.AddLayoutListbox( "Pets", "Pets", allPets, "Add a pet" );
		}
	}
}
