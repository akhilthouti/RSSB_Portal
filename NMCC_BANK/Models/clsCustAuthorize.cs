//using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace INDO_FIN_NET.Models
{
    public class clsCustAuthorize
    {

        //[TblCustomerDetails]   table......for maintaing .CustomerSession....
        public static bool IsAuthCustomerDetails(long UserId, string sessionKey)
        {
            try
            {
                if (UserId == 0)
                    return false;
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

}
