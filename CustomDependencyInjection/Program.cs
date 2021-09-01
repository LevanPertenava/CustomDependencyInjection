using System;
using System.Linq;
using System.Reflection;

namespace CustomDependencyInjection
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBuilder<ServiceConsumer> serviceBuilder = new ServiceBuilder<ServiceConsumer>();
            var service = serviceBuilder.GetService();
        }
    }

    public class ServiceBuilder<T> where T : class
    {
        public T GetService()
        {
            return (T)GetService(typeof(T));
        }
        private object GetService(Type type)
        {
            var constructor = type.GetConstructors().Single();
            var parameters = constructor.GetParameters();

            if (parameters.Length > 0)
            {
                var parameterImplementations = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    parameterImplementations[i] = GetService(parameters[i].ParameterType);
                }
                return Activator.CreateInstance(type, parameterImplementations);
            }
            return Activator.CreateInstance(type);
        }
    }

    public class ServiceConsumer
    {
        private readonly HelloService _hello;
        public ServiceConsumer(HelloService hello)
        {
            _hello = hello;
            Console.WriteLine(_hello.Message());
        }
    }

    public class HelloService
    {
        private readonly MessageService _message;
        public HelloService(MessageService message)
        {
            _message = message;
            Console.WriteLine(_message.Message());
        }
        public string Message()
        {
            return "Hello";
        }
    }

    public class MessageService
    {
        public string Message()
        {
            return "Yo";
        }
    }
}
