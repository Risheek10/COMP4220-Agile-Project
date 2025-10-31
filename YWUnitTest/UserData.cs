/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using ywBookStoreLIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ywBookStoreLIB;

namespace ywBookStoreLIB
{
    public class UserData
    {
        public int UserID { set; get; }
        public string LoginName { set; get; }
        public string Password { set; get; }
        public Boolean LoggedIn { set; get; }

        public Boolean LogIn(string loginName, string passWord)
        {
            var dbUser = new DALUserInfo();
            UserID = dbUser.LogIn(loginName, passWord);
            if (UserID > 0)
            {
                return true;
            }
            else
            {
                if (loginName != "" && passWord != "")
                {//format check
                    if (passWord.Length < 6)
                    {
                        return false;//format fail
                    }
                    else
                    {
                        if (passWord[0] < 65 || (passWord[0] > 90 && passWord[0] < 97) || passWord[0] > 122)
                        {
                            return false; //format fail
                        }
                        else
                        {
                            for (int i = 0; i < passWord.Length; i++)
                            {
                                if (passWord[i] < 48 || (passWord[i] > 57 && passWord[i] < 65) || (passWord[i] > 90 && passWord[i] < 97) || passWord[i] > 122)
                                {
                                    return false;//format fail
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            return false;//username and password doesn't match
                        }
                    }
                }
                else//if not fill
                {
                    return false;
                }
            }
            

        }
    }
}