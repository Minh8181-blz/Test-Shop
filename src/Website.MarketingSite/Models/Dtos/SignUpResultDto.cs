namespace Website.MarketingSite.Models.Dtos
{
    public class SignUpResultDto
    {
        public bool Succeeded { get; set; }
        public string Code { get; set; }
        public UserDto User { get; set; }
        public object Data { get; set; }
    }
}
