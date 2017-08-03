using System;
using System.Collections.Generic;
using System.Text;

namespace Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 简单Builder模式
            //var builder = new HtmlBuilder("ul");
            //builder.AddChild("li", "sunny");
            //builder.AddChild("li", "day");
            //Console.WriteLine(builder.ToString()); 
            #endregion

            #region Faceted Builder
            //var builder = new PersonBuilder();
            //Person person = builder
            //        .addressBuilder.SetStreetAddress("LiCang").SetPostalCode("266000").SetCity("QingDao")
            //        .jobBuilder.SetCompanyName("gole").SetPosition("general manager").SetIncome(100000);

            //Console.WriteLine(person.ToString());
            #endregion


        }
    }

    #region 简单Builder模式
    public class HtmlElement
    {
        private string _name;
        private string _text;
        public List<HtmlElement> _elements = new List<HtmlElement>();
        private int _indentSize = 2;

        public HtmlElement()
        {

        }

        public HtmlElement(string name, string text)
        {
            this._name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            this._text = text ?? throw new ArgumentNullException(paramName: nameof(text));
        }

        public string ToStringIml(int indent)
        {
            var sb = new StringBuilder();
            var rootSpace = new string(' ', _indentSize * indent);
            sb.AppendLine($"{rootSpace}<{_name}>");

            if(!string.IsNullOrWhiteSpace(_text))
            {
                var innerSpace = new string(' ', _indentSize * (indent + 1));
                sb.Append(innerSpace);
                sb.AppendLine(_text);
            }

            if(_elements!= null && _elements.Count>0)
            {
                foreach (var ele in _elements)
                    sb.Append(ele.ToStringIml(indent + 1));
            }

            sb.AppendLine($"{rootSpace}</{_name}>");

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringIml(0);
        }
    }

    public class HtmlBuilder
    {
        HtmlElement _root = null;
        private string _rootName;
        public HtmlBuilder(string rootName)
        {
            _root = new HtmlElement(rootName, "");
            _rootName = rootName;
        }

        public void AddChild(string name, string text)
        {
            _root._elements.Add(new HtmlElement(name, text));
        }
        public override string ToString()
        {
            return _root.ToString();
        }

        public void Clear()
        {
            _root = new HtmlElement(_rootName, "");
        }
    }
    #endregion

    #region Faceted Builder
    public class Person
    {
        public string StreetAddress, PostCode, City;
        public string CompanyName, Position;
        public int AnnualIncome;

        public override string ToString()
        {
            return $"addess info:{StreetAddress},{PostCode},{City};job info:{CompanyName},{Position},{AnnualIncome}";
        }
    }

    public class PersonBuilder
    {
        protected Person person = new Person();

        public PersonJobBuilder jobBuilder => new PersonJobBuilder(person);
        public PersonAddressBuilder addressBuilder => new PersonAddressBuilder(person);

        //static implicit operator Person是转换操作符，虽然PersonBuilder和Person没有继承派生关系，但也可以通过某种方式转换
        public static implicit operator Person(PersonBuilder pb)
        {
            return pb.person;
        }
    }

    public class PersonJobBuilder : PersonBuilder
    {
        public PersonJobBuilder(Person person)
        {
            this.person = person;
        }

        public PersonJobBuilder SetCompanyName(string companyName)
        {
            person.CompanyName = companyName;
            return this;
        }

        public PersonJobBuilder SetPosition(string position)
        {
            person.Position = position;
            return this;
        }

        public PersonJobBuilder SetIncome(int income)
        {
            person.AnnualIncome = income;
            return this;
        }
    }

    public class PersonAddressBuilder : PersonBuilder
    {
        public PersonAddressBuilder(Person person)
        {
            this.person = person;
        }

        public PersonAddressBuilder SetStreetAddress(string address)
        {
            person.StreetAddress = address;
            return this;
        }

        public PersonAddressBuilder SetPostalCode(string postalCode)
        {
            person.PostCode = postalCode;
            return this;
        }

        public PersonAddressBuilder SetCity(string city)
        {
            person.City = city;
            return this;
        }
    }
    #endregion







}