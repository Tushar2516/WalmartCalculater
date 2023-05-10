using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalmartCalculater.Actions;
using System.ComponentModel;
namespace WalmartCalculater.Model
{
  public  class Group : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanges(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        private int groupId;

        public int GroupId
        {
            get { return groupId; }
            set { groupId = IDGenerator.GetIdForGroup(); OnPropertyChanges("groupId"); }
        }

        private string groupName;

        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; OnPropertyChanges("GroupName"); }
        }


        private List<Person> people;

        public List<Person> People
        {
            get { return people; }
            set { people = value; OnPropertyChanges("People"); }
        }
        public Group()
        {
            people = new List<Person>();
        }

        public List<Person> GetPeople()
        {
            return people;
        }

        public bool AddPerson(Person person)
        {
            if (Validator.ValidatePerson(person))
            {
                people.Add(person);
                return true;
            }
            else
            {
                throw new Exception("Invalid details for person");
            }
        }

        public bool UpdatePerson(Person updatedPerson)
        {
            foreach (Person person in people)
            {
                if (person.Name.Equals(updatedPerson.Name))
                {
                    person.Name = updatedPerson.Name;
                    return true;
                }
            }
            return false;
        }

        public bool RemovePerson(Person person)
        {
            int DeletedPersonCount=0;
            if (Validator.ValidatePerson(person))
            {
               DeletedPersonCount = people.RemoveAll(currentPerson => currentPerson.Name.Equals(person.Name));

            }
            return DeletedPersonCount > 0;
        }
    }
}
