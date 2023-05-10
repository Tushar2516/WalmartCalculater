using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalmartCalculater.Actions;
namespace WalmartCalculater.Model
{
   public class GroupServices
    {
        List<Group> groups;

        public GroupServices()
        {
            groups = new List<Group>();
        }

        public List<Group> GetGroups()
        {
            return groups;
        }

        public bool AddGroup(Group group)
        {
            if (Validator.ValidateGroup(group))
            {
                groups.Add(group);
                return true;
            }
            else
            {
                throw new Exception("Invalid group details.");
            }
        }

        public bool DeleteGroup(int GroupId)
        {
            int DeletedGroupCount = 0;
            if (GroupId != null)
            {
                DeletedGroupCount = groups.RemoveAll(group => group.GroupId.Equals(GroupId));
            }
            return DeletedGroupCount > 0;
        }

        public bool UpdateGroup(Group newGroup)
        {
            bool hasUpdated = false;
            if (Validator.ValidateGroup(newGroup))
            {
                foreach (Group currentGroup in groups)
                {
                    if (currentGroup.GroupId.Equals(newGroup.GroupId))
                    {
                        currentGroup.GroupName = newGroup.GroupName;
                        currentGroup.People = newGroup.People;
                        hasUpdated = true;
                        break;
                    }
                }

            }
            else
            {
                throw new Exception("Invalid Group details");
            }
            return hasUpdated;
        }
    }
}
