namespace UserAPI.Models
{
    public class ResponseMessage
    {
        public string Mensagem { get; set; } = string.Empty;

        public ResponseMessage(string message)
        {
                Mensagem = message;
        }
    }
}
