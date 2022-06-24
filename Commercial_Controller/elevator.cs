using System.Threading;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Elevator
    {
        public int ID;
        public string status;
        public int amountOfFloors;
        public int currentFloor;
        public Door door;
        public List<int> floorRequestsList;
        public string direction;
        bool overweight;
        bool obstruction;
        string overweightAlarm;
        public List<int> completedRequestsList;
        public Elevator(int _ID,string _status, int _amountOfFloors, int _currentFloor )
        {
            ID = _ID;
            status = _status;
            amountOfFloors = _amountOfFloors;
            currentFloor = _currentFloor;
            door = new Door(_ID,"closed");
            floorRequestsList = new List<int>();
            direction = null;
            overweight = false;
            obstruction = false;
            completedRequestsList = new List<int>();
        }
        public void move()
        {
            while(this.floorRequestsList.Count != 0)
            {
                int destination = floorRequestsList[0];
                this.status = "moving";
                if(this.direction == "up")
                {
                    while(this.currentFloor < destination)
                    {
                        this.currentFloor++;
                    }
                }
                else if(this.direction == "down")
                {
                    while(this.currentFloor > destination)
                    {
                        this.currentFloor--;
                    }
                }
                this.status = "stopped";
                this.operateDoors();
                this.floorRequestsList.RemoveAt(0);
                this.completedRequestsList.Add(destination);
            }
            this.status = "idle";
        }
        public void sortFloorList()
        {
            if(this.direction == "Up")
            {
                this.floorRequestsList.Sort();
            }else
            {
                this.floorRequestsList.Reverse();
            }
        }
        public void operateDoors()
        {
            this.door.status = "opened";
            Thread.Sleep(5000);
            if (!overweight)
            {
                this.door.status = "closing";
                if(!obstruction)
                {
                    this.door.status = "closed";
                }else
                {
                    this.operateDoors();
                }
            }else
            {
                while(overweight)
                {
                    overweightAlarm = "Activated";
                }
                this.operateDoors();
            }
        }
        public void addNewRequest(int requestedFloor)
        {
            if(!this.floorRequestsList.Contains(requestedFloor))
            {
                this.floorRequestsList.Add(requestedFloor);
            }
            if(this.currentFloor < requestedFloor)
            {
                this.direction = "up";
            }
            if(this.currentFloor > requestedFloor)
            {
                this.direction = "down";
            }
        }
        
    }
}