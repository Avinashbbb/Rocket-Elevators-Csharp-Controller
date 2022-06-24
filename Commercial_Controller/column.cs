using System;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Column
    {
        public int ID;
        public string status;
        public int amountOfFloors;
        public int amountOfElevators;
        public List<Elevator> elevatorsList;
        public List<CallButton> callButtonsList;
        public List<int> servedFloorsList;
        private int elevatorID;
        public Column(int _ID,int _amountOfFloors,  int _amountOfElevators, List<int> _servedFloors, bool _isBasement)
        {
            this.ID = _ID;
            this.status = "online";
            this.amountOfFloors = _amountOfFloors;
            this.amountOfElevators = _amountOfElevators;
            this.elevatorsList = new List<Elevator>();
            this.callButtonsList = new List<CallButton>();
            this.servedFloorsList = _servedFloors;
            createElevators(_amountOfFloors,_amountOfElevators);
            createCallButtons(_amountOfFloors,_isBasement);
        }
        // declaring methods
        public void createCallButtons(int _amountOfFloors, bool _isBasement)
        {
            if(_isBasement)
            {
                int buttonFloor = -1;
                for(int i = 0; i <_amountOfFloors; i ++)
                {
                    CallButton callButton = new CallButton(Global.callButtonID,"OFF",buttonFloor,"Up");
                    this.callButtonsList.Add(callButton);
                    buttonFloor --;
                    Global.callButtonID ++;
                }
            }else
            {
                int buttonFloor = 1;
                for(int i = 0; i < _amountOfFloors; i++)
                {
                    CallButton callButton = new CallButton(Global.callButtonID,"OFF",buttonFloor,"Down");
                    this.callButtonsList.Add(callButton);
                    buttonFloor ++;
                    Global.callButtonID ++;
                }

            }
        }
        public void createElevators(int _amountOfFloors,int _amountOfElevators)
        {
            for(int i = 0; i < _amountOfElevators; i++)
            {
                Elevator elevator = new Elevator(this.elevatorID,"idle",_amountOfFloors,1);
                this.elevatorsList.Add(elevator);
                this.elevatorID ++;
            }
        }

        //Simulate when a user press a button on a floor to go back to the first floor
        public Elevator requestElevator(int userPosition, string direction)
        {
            Elevator elevator = this.findElevator(userPosition,direction);
            elevator.addNewRequest(userPosition);
            elevator.move();

            elevator.addNewRequest(1);
            elevator.move();

            return elevator;
        }

        public Elevator findElevator(int requestedFloor,string requestedDirection)
        {
            Elevator bestElevator = null;
            int bestScore = 6;
            int referenceGap = 10000000;
            BestElevatorInformations bestElevatorInformations;
            
            if(requestedFloor == 1)
            {
                foreach (Elevator elevator in this.elevatorsList)
                {
                    if( 1 == elevator.currentFloor && elevator.status == "stopped")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(1,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }else if(1 == elevator.currentFloor && elevator.status == "idle")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(2,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }else if(1 > elevator.currentFloor && elevator.direction =="Up")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(3,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }else if(1 < elevator.currentFloor && elevator.direction =="down")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(3,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }else if(elevator.status == "idle")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(4,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }else
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(5,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }
                    bestElevator = bestElevatorInformations.bestElevator;
                    bestScore = bestElevatorInformations.bestScore;
                    referenceGap = bestElevatorInformations.referenceGap;
                }
                
                
            }else
            {
                foreach (Elevator elevator in elevatorsList)
                {
                    if(requestedFloor == elevator.currentFloor && elevator.status =="stopped" && requestedDirection == elevator.direction)
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(1,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }else if (requestedFloor > elevator.currentFloor && elevator.direction == "up" && requestedDirection == "up")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(2,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }else if (requestedFloor < elevator.currentFloor && elevator.direction == "down" && requestedDirection == "down")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(2,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }else if (elevator.status == "idle")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(4,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }else 
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(5,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }
                    bestElevator = bestElevatorInformations.bestElevator;
                    bestScore = bestElevatorInformations.bestScore;
                    referenceGap = bestElevatorInformations.referenceGap;
                }
            }
            return bestElevator;
        }

        public BestElevatorInformations checkIfElevatorIsBetter(int scoreToCheck,Elevator newElevator,int bestScore,int referenceGap,Elevator bestElevator,int floor)
        {
            if(scoreToCheck < bestScore)
            {
                bestScore = scoreToCheck;
                bestElevator = newElevator;
                referenceGap = Math.Abs(newElevator.currentFloor - floor);
            }else if(bestScore == scoreToCheck)
            {
                int gap = Math.Abs(newElevator.currentFloor - floor);
                if (referenceGap > gap )
                {
                    bestElevator = newElevator;
                    referenceGap = gap;
                } 
            }
            BestElevatorInformations bestElevatorInformations = new BestElevatorInformations(bestElevator,bestScore,referenceGap);
            return bestElevatorInformations;
        }

    }
}