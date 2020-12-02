namespace Domain.Wrappers
{
    public class Response<T>
    {
        public T Data { get; set; }

        public Response(T data)
        {
            Data = data;
        }
    }
}
