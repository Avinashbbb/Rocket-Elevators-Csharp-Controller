namespace Commercial_Controller
{
    //Button on a floor or basement to go back to lobby
    public class CallButton
    { 
    int ID;
    string status;
    int floor;
    string direction;
        public CallButton(int _id,string _status,int _floor, string _direction)
        {
            ID = _id;
            status = _status;
            floor = _floor;
            direction = _direction;
        }
        
    }
}