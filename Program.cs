using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.Globalization;

namespace AddLocalAdmin
{
    class Program
    {
        public static bool AddUserToLocalGroup(string userName, string groupName)
        {
            DirectoryEntry userGroup = null;

            try
            {
                string groupPath = string.Format(CultureInfo.CurrentUICulture, "WinNT://{0}/{1},group", Environment.MachineName, groupName);
                userGroup = new DirectoryEntry(groupPath);

                if ((null == userGroup) || (true == string.IsNullOrEmpty(userGroup.SchemaClassName)) || (0 != string.Compare(userGroup.SchemaClassName, "Group", true, CultureInfo.CurrentUICulture)))
                    return false;

                    String userPath = String.Format(CultureInfo.CurrentUICulture, "WinNT://{0},user", userName);
                userGroup.Invoke("Add", new object[] { userPath });
                userGroup.CommitChanges();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (null != userGroup) userGroup.Dispose();
            }
        }

        static void Main(string[] args)
        {
            AddUserToLocalGroup("DOMAIN/user", "Administrators");
        }
    }
}
