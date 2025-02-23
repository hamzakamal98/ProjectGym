namespace ProjectGym.Models
{
    public class Response
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public int DataCount { get; set; }

        public IList<object> Data { get; set; } 
    }
}
