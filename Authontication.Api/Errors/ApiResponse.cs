namespace ApiRover.Errors
{
    public class ApiResponse
    {

        public int StatusCode { get; set; }
        public string? Message { get; set; }


        public ApiResponse(int _statuscode, string? _message = null) {

            StatusCode = _statuscode;
            Message = _message ?? GetDefaultMessageForStatusCode(_statuscode);


        }

        private string? GetDefaultMessageForStatusCode(int statuscode)
        {
            return StatusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized", 
                404 => "Not Found",
                
                500 => "Error Are The Path to the Dark Side",
                _ => null,
            };
        }
    }
}
