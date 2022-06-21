using System;
namespace Commercial_Controller
{
    class Program
    {
        static void Main(string[] args)
        {
            //int scenarioNumber = Int32.Parse(args[0]);
            int scenarioNumber = 1;
            Scenarios scenarios = new Scenarios();
            scenarios.run(scenarioNumber);
        }
    }
}
