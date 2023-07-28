using System.Web.Mvc;

namespace INDO_FIN_NET.Customehelpers
{
    public static class Customhelper
    {
        public static MvcHtmlString submitbutton(string value,string Name)
        
        {
            string str = "<input type='submit' value='" + value + "' name='" + Name + "'/>";
            return new MvcHtmlString(str);

        }

    }
}
