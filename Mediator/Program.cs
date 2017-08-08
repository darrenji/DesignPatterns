using System;
using System.Collections.Generic;
using System.Linq;

namespace Mediator
{
    class Program
    {
        static void Main(string[] args)
        {
            #region meidator
            //var room = new ChatRoom();

            //var john = new Person("john");
            //var jane = new Person("Jane");

            //room.Join(john);
            //room.Join(jane);

            //john.Say("hi");
            //jane.Say("oh, hey john");

            //var simon = new Person("Simon");
            //room.Join(simon);
            //simon.Say("hi everyone");

            //jane.PrivateMessage("Simon", "glad you could join us!"); 
            #endregion

            #region Client Example
            MediatorManager mediator = new MediatorManager();

            DevTeamAbstract devTeam = new DevTeamA(mediator);
            devTeam._devTeamType = "Dev Team A";
            mediator.devTeam = devTeam;

            ClientAbstract client = new ClientA(mediator);
            client._clientName = "ClientA";
            mediator.client = client;

            client.SendQueryToMediator();
            #endregion
        }
    }

    #region Chat Room
    public class Person
    {
        public string Name;
        public ChatRoom Room;
        private List<string> chatLog = new List<string>();

        public Person(string name)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
        }

        public void Say(string message)
        {
            Room.Broadcast(Name, message);
        }

        public void PrivateMessage(string who, string message)
        {
            Room.Message(Name, who, message);
        }

        public void Receive(string sender, string message)
        {
            string s = $"{sender}:'{message}'";
            chatLog.Add(s);
            Console.WriteLine($"[{Name}'s chat session] {s}");
        }
    }

    //mediator
    public class ChatRoom
    {
        private List<Person> people = new List<Person>();

        public void Join(Person p)
        {
            string jonMsg = $"{p.Name} joins the chat";
            Broadcast("room", jonMsg);

            p.Room = this;
            people.Add(p);
        }

        public void Broadcast(string source, string message)
        {
            foreach (var p in people)
                if (p.Name != source)
                    p.Receive(source, message);
        }

        public void Message(string source, string destination, string message)
        {
            people.FirstOrDefault(p => p.Name == destination)
                ?.Receive(source, message);
        }
    }
    #endregion

    #region Client Example
    public abstract class ClientAbstract
    {
        public string _clientName;
        protected MediatorManager _mediatorManager;

        public ClientAbstract(MediatorManager mediator)
        {
            _mediatorManager = mediator;
        }

        //客户端接受到信息
        public void ReceiveRequirementsFromMediator(DevTeamAbstract devTeam)
        {
            Console.WriteLine(this._clientName + " has received requirements from " + devTeam._devTeamType);
        }

        //1 发出信息
        public void SendQueryToMediator()
        {
            //2 mediatorManager接受到信息
            _mediatorManager.ReceiveRequirementsFromClient();
        }
    }

    public abstract class DevTeamAbstract
    {
        public string _devTeamType;
        protected MediatorManager _mediator;

        public DevTeamAbstract(MediatorManager mediator)
        {
            _mediator = mediator;
        }

        //4 接受到信息
        public void ReceiveRequirementsFromMediator(ClientAbstract client)
        {
            Console.WriteLine(this._devTeamType + " has received requirements form " + client._clientName);
        }


        //5 发出信息
        public void SendQueryToMediator()
        {
            //6 mediator接受到信息
            _mediator.ReceiveQueryFromDevTeam();
        }
    }

    public class MediatorManager
    {
        public ClientAbstract client { get; set; }
        public DevTeamAbstract devTeam { get; set; }

        //2 mediator接收到信息
        public void ReceiveRequirementsFromClient()
        {
            // 3 devTeam接受到信息
            devTeam.ReceiveRequirementsFromMediator(client);
        }

        //6 mediator接受到信息
        public void ReceiveQueryFromDevTeam()
        {
            //7客户端接受到信息
            client.ReceiveRequirementsFromMediator(devTeam);
        }
    }

    public class ClientA : ClientAbstract
    {
        public ClientA(MediatorManager mediaor):base(mediaor)
        {

        }
    }

    public class DevTeamA :DevTeamAbstract
    {
        public DevTeamA(MediatorManager mediator):base(mediator)
        {

        }
    }

    #endregion
}