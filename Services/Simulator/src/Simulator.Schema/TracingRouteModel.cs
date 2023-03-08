namespace Simulator.Schema
{
    public class TracingRouteModel
    {
        public string OrderId { get; set; }
        public Route CurrentRoute { get; set; }
        public bool IsFinished { get; set; }
    }
}
