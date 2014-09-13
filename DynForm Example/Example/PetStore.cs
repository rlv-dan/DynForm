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

namespace DynForm
{
	// This Class shows to work with some more complext data structures
	// Make sure that you read and understand the Person class example first
	class PetStore : IDynFormEntity
	{
		public List<DynFormList_ValueDataGrid> CageTypes { get; set; }
		public List<DynFormList_ValueDataGrid> PetsInStock { get; set; }
		public List<DynFormList_CheckboxDataGrid> FeedingTime { get; set; }

		public PetStore()
		{
			// Add some default data to work with
			PetsInStock = new List<DynFormList_ValueDataGrid>();
			PetsInStock.Add( new DynFormList_ValueDataGrid( "RatID", "Rat", "5" ) );
			PetsInStock.Add( new DynFormList_ValueDataGrid( "BirdID", "Bird", "17" ) );
			PetsInStock.Add( new DynFormList_ValueDataGrid( "FishID", "Fish", "3" ) );

			CageTypes = new List<DynFormList_ValueDataGrid>();
			CageTypes.Add( new DynFormList_ValueDataGrid( "RatID", "Rat", "Cage1" ) );
			CageTypes.Add( new DynFormList_ValueDataGrid( "BirdID", "Bird", "Cage3" ) );
			CageTypes.Add( new DynFormList_ValueDataGrid( "FishID", "Fish", "Cage4" ) );
			CageTypes.Add( new DynFormList_ValueDataGrid( "GremlinID", "Gremlin", "Cage0" ) );

			FeedingTime = new List<DynFormList_CheckboxDataGrid>();
			FeedingTime.Add( new DynFormList_CheckboxDataGrid( "Dog", "Dog", false ) );
			FeedingTime.Add( new DynFormList_CheckboxDataGrid( "Bird", "Bird", false ) );
			FeedingTime.Add( new DynFormList_CheckboxDataGrid( "Gremlin", "Gremlin", true ) );
		}

		// --- Below are implementations of methods required by the IDynFormEntity interface ---

		public bool bSaveData { get; set; }

		public string Validate( string propertyName )
		{
			// This example skips validation, see Person class for validation examples
			return "";	// Empty string = Everything ok
		}

		public void SetupFormFields( DynForm f )
		{
			// Create lists with available items
			// For convenience I do it here, but this data can come from anywhere if needed
			var allPets = new List<DynFormList>();
			allPets.Add( new DynFormList( "DogID", "Dog" ) );
			allPets.Add( new DynFormList( "CatID", "Cat" ) );
			allPets.Add( new DynFormList( "RatID", "Rat" ) );
			allPets.Add( new DynFormList( "LizardID", "Lizard" ) );
			allPets.Add( new DynFormList( "BirdID", "Bird" ) );
			allPets.Add( new DynFormList( "FishID", "Fish" ) );
			allPets.Add( new DynFormList( "RabbitID", "Rabbit" ) );
			allPets.Add( new DynFormList( "GremlinID", "Gremlin" ) );
			
			var allCages = new List<DynFormList>();
			allCages.Add( new DynFormList( "Cage0", "Unknown" ) );
			allCages.Add( new DynFormList( "Cage1", "Small Cage" ) );
			allCages.Add( new DynFormList( "Cage2", "Medium Cage" ) );
			allCages.Add( new DynFormList( "Cage3", "Large Cage" ) );
			allCages.Add( new DynFormList( "Cage4", "Aquarium" ) );


			// Add controls to edit in the DynForm
			f.AddLayoutValueDataGrid( "Pets in Stock", "PetsInStock", "Pet", "Left (F2 to edit)", allPets, "Add Pet", "0" );
			f.AddLayoutDropdownDataGrid( "Cage Type", "CageTypes", allCages, "Pet", "Cage" );
			f.AddLayoutCheckboxDataGrid( "Feeding time", "FeedingTime", "Pet", "Do not feed after midnight" );
		}

	}
}
