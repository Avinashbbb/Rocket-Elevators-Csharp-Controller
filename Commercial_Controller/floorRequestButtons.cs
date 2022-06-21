namespace Commercial_Controller
{
    //Button on a floor or basement to go back to lobby
    public class FloorRequestButton
    {
        int ID;
        string status;
        int floor;
        string direction;
        public FloorRequestButton(int _id,int _floor, string _direction)
        {
            ID = _id;
            status = "boo";
            floor = _floor;
            direction = _direction;

        }
    }
}