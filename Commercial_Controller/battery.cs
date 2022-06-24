using System;
using System.Collections.Generic;

namespace Commercial_Controller
{
    
    public class Battery
    {//creating keys

        public int ID;
        public string status = "online";
        public List<Column>columnsList;
        public List<FloorRequestButton>floorRequestsButtonsList;
        private int columnID = 1;

        public Battery(int _ID, int _amountOfColumns, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn)
        {   
            this.ID = _ID;
            this.status = "online";
            this.columnsList = new List<Column>();
            this.floorRequestsButtonsList = new List<FloorRequestButton>();
            
            if (_amountOfBasements > 0)
            {
                createBasementFloorRequestButtons(_amountOfBasements);
                createBasementColumn(_amountOfFloors,_amountOfBasements,_amountOfElevatorPerColumn);
                _amountOfColumns --;
            }

            createFloorRequestButtons(_amountOfFloors);
            createColumns(_amountOfColumns,_amountOfFloors,_amountOfElevatorPerColumn);
            
        }
            // creating methods
        public void createBasementColumn(int _amountOfFloors,int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
            List<int> servedFloors = new List<int>();
            int floor = -1;
                for(int i = 0; i < _amountOfBasements; i++)
                {
                    servedFloors.Add(floor);
                    floor --;
                }
                Column column = new Column(this.columnID,_amountOfFloors,_amountOfElevatorPerColumn,servedFloors,true);//made some modifications to this
                this.columnsList.Add(column);
                this.columnID ++;
        }

        public void createColumns(int _amountOfColumns,int _amountOfFloors,int _amountOfElevatorPerColumn)
        {
            int amountOfFloorsPerColumn = (_amountOfFloors - 1)/_amountOfColumns + 1;
            int floor = 1;
            for(int i = 0; i < _amountOfColumns; i++)
            {
                List<int> servedFloors = new List<int>();
                for(int n = 0; n < amountOfFloorsPerColumn; n++)
                {
                    if(floor <= _amountOfFloors)
                    {
                        servedFloors.Add(floor);
                        floor++;
                    }
                }

                Column column = new Column(this.columnID,_amountOfFloors,_amountOfElevatorPerColumn,servedFloors,false);//made some modifications to this
                this.columnsList.Add(column);
                this.columnID++;
            }
        }
        public void createFloorRequestButtons(int _amountOfFloors)
        {
            int buttonFloor = 1;
            for(int i = 0; i < _amountOfFloors; i++)
            {
                FloorRequestButton floorRequestButton = new FloorRequestButton(Global.floorRequestButtonID,buttonFloor,"Up");
                this.floorRequestsButtonsList.Add(floorRequestButton);
                buttonFloor ++;
                Global.floorRequestButtonID ++;
            }
        }

        public void createBasementFloorRequestButtons(int _amountOfBasements)
        {
            int buttonFloor = -1;
            for(int i = 0; i < _amountOfBasements; i ++)
            {
                FloorRequestButton floorRequestButton = new FloorRequestButton(Global.floorRequestButtonID,buttonFloor,"Down");
                this.floorRequestsButtonsList.Add(floorRequestButton);
                buttonFloor --;
                Global.floorRequestButtonID ++;
            }
        }
        public  Column  findBestColumn(int _requestedFloor)
        {
            //foreach (Column column in columnsList)
            for(int i = 0; i < columnsList.Count; i++)
            {
                if(columnsList[i].servedFloorsList.Contains(_requestedFloor)) {
                    return columnsList[i];
                }

                // while (columnsList[i].servedFloorsList.Contains(_requestedFloor))
                // {
                //     return columnsList[i];
                // }
                
            }
            return null;
        }
        //Simulate when a user press a button at the lobby
        public (Column, Elevator) assignElevator(int _requestedFloor, string _direction)
        {
            Column column = this.findBestColumn(_requestedFloor);
            Elevator elevator = column.findElevator(1,_direction);
            elevator.addNewRequest(1);
            elevator.move();

            elevator.addNewRequest(_requestedFloor);
            elevator.move();
            return(column,elevator);
            
        }
    }
}

